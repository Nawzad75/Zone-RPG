using System.Text;
using System.Drawing;
using Colorful;
using Console = Colorful.Console;
using ZoneRpg.Database;
using ZoneRpg.GameLogic;
using ZoneRpg.Shared;
using System.Collections.Concurrent;

namespace ZoneRpg.UserInterface
{
    public class Ui
    {
        // Ui Members
        private DatabaseManager _db;
        private ZoneRenderer _zoneRenderer = new ZoneRenderer();
        private IRenderer _playerRenderer = new CharacterRenderer();
        private IRenderer _monsterRenderer = new CharacterRenderer();
        private IRenderer _battleRenderer;
        private Game _game;
        private IRenderer _chatBoxRenderer;

        // Constructor
        public Ui(DatabaseManager db, Game game)
        {
            _db = db;
            _game = game;
            _battleRenderer = new BattleRenderer(game.BattleManager);
            _chatBoxRenderer = new ChatboxRenderer(game.ChatBox);
            Setup();
        }

        // Setup the UI
        private void Setup()
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.CursorVisible = false;
            Console.Clear();

            // Rektangeln som visar spelarens stats
            _playerRenderer.SetRect(0, _game.Zone.Height + 5, 30, 3);
            _playerRenderer.SetAccentColor(ConsoleColor.Cyan);

            // Rektangeln som visar monstrets stats
            _monsterRenderer.SetRect(33, _game.Zone.Height + 5, 30, 3);
            _monsterRenderer.SetAccentColor(ConsoleColor.Red);

            // Rektangel som visar battle-meddelanden
            _battleRenderer.SetRect(1, _game.Zone.Height + 10, 62, 2);
        }

        // Run!
        public void Render()
        {
            switch (_game.State)
            {
                case GameState.MainMenu:
                    new StartGame().RunMainMenu();
                    _game.SetState(GameState.GetPlayer);
                    Render(); // Render again to show the new state before we read input
                    break;

                case GameState.GetPlayer:
                    _game.SetPlayer(CreateOrChoosePlayer());
                    ((CharacterRenderer)_playerRenderer).SetCharacter(_game.GetPlayer());
                    _game.SetState(GameState.Zone);
                    Render(); // Render again to show the new state before we read input
                    break;

                case GameState.Dead:
                    break;

                case GameState.Zone:
                    DrawZone();
                    _playerRenderer.Draw();
                    _monsterRenderer.Draw();
                    break;

                case GameState.Battle:
                    DrawZone();
                    // cast IRenderer to CharacterRender, so we can get/set the current Monster
                    CharacterRenderer monsterRenderer = (CharacterRenderer)_monsterRenderer;
                    if (!monsterRenderer.hasCharacter())
                    {
                        monsterRenderer.SetCharacter(_game.BattleManager.Monster);
                    }
                    _playerRenderer.Draw();
                    _monsterRenderer.Draw();
                    _battleRenderer.Draw();

                    break;
            }

         
        }

        private void DrawZone()
        {
            _zoneRenderer.DrawZone(_game.Zone);
            _zoneRenderer.DrawEntities(_game.Zone.Entities);
            _zoneRenderer.DrawPlayerEntity(_game.GetPlayerEntity());
            _chatBoxRenderer.Draw();
        }

        // Let the player choose a character or create a new one
        private Player CreateOrChoosePlayer()
        {
            string prompt = "Create or choose a character!";
            string[] options = new string[] { "Create", "Choose" };
            CreateChooseMenu createOption = (CreateChooseMenu)new Menu(prompt, options).Run();

            if (createOption == CreateChooseMenu.Create)
            {
                Player player = CreatePlayer();
                _db.InsertCharacter(player);
                return player;
            }
            return ChoosePlayer();
        }

        // Create a player
        public Player CreatePlayer()
        {
            Console.Clear();
            Console.WriteLine("Enter Character Name");

            string name = Console.ReadLine(); //här skickar vi in namnet som spelaren skriver in
            if (string.IsNullOrEmpty(name))
            {
                Console.WriteLine("Your entery was blank, press enter & try again");
                Console.ReadKey();
                return CreatePlayer();
            }
            else
            {
                // 1. Hämta alla klasser från databasen
                List<CharacterClass> characterClasses = _db.GetClasses();
             
                // 2. Visa meny med alla klasser, låt spelaren välja en
                string prompt = "Choose a class!";
                string[] options = characterClasses.Select(CharacterToString).ToArray();
                int index = new Menu(prompt, options).Run();
                CharacterClass selectedClass = characterClasses[index];

                // 3. Skapa och returnera en ny Player med vald namn och klass:
                Player player = new Player(name, selectedClass);
                return player;
            }

        }

        // Lamba-funktion omgjord till en metod (x => x.Name)
        private string CharacterToString(CharacterClass x)
        {
            return x.Name;
        }

        // Välj en existerande player från en lista
        public Player ChoosePlayer()
        {
            Console.Clear();
            List<Player> players = _db.GetPlayers();
            Console.WriteLine("players.Count: " + players.Count);
            
            // options kommer se ut t.ex. så här: ["Namn  (id: 1)", "namn  (id: 2), "namn  (id: 3)"]
            // "Select" kör lamda-funktionen för varje element i listan.
            string[] options = players.Select(c => $"{c.Name}  (id: {c.Id})").ToArray();
            int index = new Menu("Choose a character:", options).Run();
            return players[index];
        }

        // Läster user input (och skickar till game)
        public void ReadInput()
        {

            ConsoleKeyInfo cki = Console.ReadKey();
            switch (_game.State)
            {
                case GameState.Zone:
                    if (Constants.AllArrowKeys.Contains(cki.Key))
                    {
                        _game.MovePlayer(cki.Key);
                    }
                    break;

                case GameState.Battle:

                    break;

                case GameState.Dead:
                    if (cki.Key == ConsoleKey.Enter)
                    {
                        Console.Clear();
                        _game.RespawnPlayer();
                    }
                    break;
            }
            //Chat start.
            string input = cki.KeyChar.ToString().ToLower();
            if (input == "t")
            {
                Chat();
            }
            
            string input1 = cki.KeyChar.ToString().ToLower();
            if (input1 == "i")
            {
                Inventory();
            }
            

            
        }


        public void Chat()
        {
            Console.Clear();
            CreateMessage();
        }

        // Create a message
        public void CreateMessage()
        {
            Console.Clear();
            Console.WriteLine("Write a message");
            string message = Console.ReadLine()!;
            Message message1 = new Message(message, character: _game.GetPlayer());
            _db.InsertMessage(message1);
        }

        public void Inventory()
        {
            Console.Clear();
            Console.WriteLine( _game.Player.Boots);
            Console.WriteLine(_game.Player.Weapon);
            // Console.WriteLine(_game.Player.Helmet);
            Console.WriteLine("Press enter to continue");
            Console.ReadLine();
        }
    }
}