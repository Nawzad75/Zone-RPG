using ZoneRpg.Database;
using ZoneRpg.GameLogic;
using ZoneRpg.Models;
using ZoneRpg.Shared;

namespace ZoneRpg.UserInterface
{
    public class Ui
    {
        // UI medlämmar
        private DatabaseManager _db;
        private Game _game;
        private ZoneRenderer _zoneRenderer = new ZoneRenderer();
        private IRenderer _playerRenderer;
        private IRenderer _monsterRenderer;
        private IRenderer _battleRenderer;
        private IRenderer _chatBoxRenderer;

        enum GetPlayerMenu
        {
            CreatePlayer,
            ChoosePlayer
        }

        // Constructor
        public Ui(DatabaseManager db, Game game)
        {
            _db = db;
            _game = game;
            _chatBoxRenderer = new ChatboxRenderer(game.ChatBox, 50, 0);
            _playerRenderer = new CharacterRenderer(0, _game.Zone.Height + 5, 30, 4, ConsoleColor.Cyan);
            _monsterRenderer = new CharacterRenderer(33, _game.Zone.Height + 5, 30, 4, ConsoleColor.Red);
            _battleRenderer = new BattleRenderer(game.BattleManager, 0, _game.Zone.Height + 11, 63, 2);

            SetupConsole();
        }

        // Sätter upp UI
        private void SetupConsole()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.CursorVisible = false;
            Console.Clear();
        }

        // The big render function!
        public void Render()
        {
            switch (_game.State)
            {
                case GameState.MainMenu:
                    new StartMenu().RunMainMenu();
                    _game.SetState(GameState.GetPlayer);
                    Render();  // Renderar igen för att visa det nya statet innan vi läser inmatning
                    break;

                case GameState.GetPlayer:
                    _game.SetPlayer(CreateOrChoosePlayer());
                    ((CharacterRenderer)_playerRenderer).SetCharacter(_game.Player);
                    _game.SetState(GameState.ZoneTransition);
                    break;

                case GameState.Zone:
                    DrawZone();
                    _playerRenderer.Draw();
                    _monsterRenderer.Draw();
                    _chatBoxRenderer.Draw();
                    break;


                case GameState.ZoneTransition:
                    _playerRenderer.SetRect(0, _game.Zone.Height + 5, 30, 4);
                    _monsterRenderer.SetRect(33, _game.Zone.Height + 5, 30, 4);
                    _battleRenderer.SetRect(0, _game.Zone.Height + 11, 63, 2);
                    Console.Clear();
                    break;

                case GameState.Dead:
                    Console.Clear();
                    Console.WriteLine("You died! Press <Enter> to respawn!");
                    break;

                case GameState.Battle:
                    DrawZone();

                    // IRenderer till CharacterRenderer, så vi kan hämta/ändra nuvarande Monster
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

        // Ritar zonen och innehåll
        private void DrawZone()
        {
            _zoneRenderer.DrawZone(_game.Zone);
            _zoneRenderer.DrawEntities(_game.Zone.Entities);
            _zoneRenderer.DrawPlayerEntity(_game.Player.Entity);
        }

        // Låter spelaren välja en karaktär eller skapa en ny
        private Player CreateOrChoosePlayer()
        {
            string prompt = "Create or choose a character!";
            string[] options = new string[] { "Create", "Choose" };
            GetPlayerMenu createOption = (GetPlayerMenu)new Menu(prompt, options).Run();

            if (createOption == GetPlayerMenu.CreatePlayer)
            {
                Player player = CreatePlayer();
                _db.InsertCharacter(player);
                return player;
            }
            return ChoosePlayer();
        }

        // Skapar en ny spelare
        public Player CreatePlayer()
        {
            Console.Clear();
            Console.WriteLine("Enter Character Name");

            // Här skickar vi in namnet som spelaren skriver in
            string name = Console.ReadLine()!;
            if (string.IsNullOrEmpty(name))
            {
                Console.WriteLine("Your entery was blank, press enter & try again");
                Console.ReadKey();
                return CreatePlayer();
            }
            else
            {
                // 1. Hämta alla klasser från databasen
                List<CharacterClass> characterClasses = _db.GetAllCharacterClasses();

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

        // Lambda-funktion omgjord till en metod (x => x.Name)
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
            // Hoppa över input (som blockar) om vi inte är i är i ett transition-state.
            if (_game.State == GameState.ZoneTransition)
            {
                _game.SetState(GameState.Zone);
                return;
            }

            ConsoleKeyInfo cki = Console.ReadKey();
            switch (_game.State)
            {
                case GameState.Zone:
                    if (Constants.AllArrowKeys.Contains(cki.Key))
                    {
                        _game.MovePlayer(cki.Key);
                    }

                    if (cki.Key == ConsoleKey.T)
                    {
                        CreateMessage();
                    }

                    if (cki.Key == ConsoleKey.I)
                    {
                        ShowInventory();
                    }

                    if (cki.Key == ConsoleKey.Q)
                    {
                        Environment.Exit(0);
                    }

                    break;

                case GameState.Loot:
                    EquipLootUi();
                    _game.SetState(GameState.ZoneTransition);
                    (_monsterRenderer as CharacterRenderer)!.SetCharacter(null);
                    break;

                case GameState.Battle:
                    break;

                case GameState.Dead:
                    if (cki.Key == ConsoleKey.Enter)
                    {
                        Console.Clear();
                        (_monsterRenderer as CharacterRenderer)!.SetCharacter(null);
                        _game.RespawnPlayer();
                    }
                    break;
            }
        }

        // UI for equipping loot
        private void EquipLootUi()
        {
            bool isDoneLooting = false;
            string prompt = "You have found loot! Choose to equip:";
            while (!isDoneLooting)
            {

                string[] menuOptions = _game.CurrentLoot.Select(x => x.ToString()).ToArray();
                menuOptions = menuOptions.Append("Done").ToArray();
                int selected = new Menu(prompt, menuOptions).Run();

                if (selected == menuOptions.Length - 1)
                {
                    isDoneLooting = true;
                }
                else
                {
                    Item item = _game.CurrentLoot[selected];
                    item.Id = _db.InsertItem(item);
                    _game.Player.EquipItem(item);
                    _db.UpdatePlayerEquipment(_game.Player);
                    prompt = "You have equipped " + item.ItemInfo.Name + ".";
                    _game.CurrentLoot.RemoveAt(selected);
                }
            }
        }


        // Skapar meddelande
        public void CreateMessage()
        {
            Console.Clear();
            Console.WriteLine("Write a message");
            string input = Console.ReadLine()!;
            Message message = new Message(input, character: _game.Player);
            _db.InsertMessage(message);
        }

        // Visar inventory
        public void ShowInventory()
        {
            Console.Clear();
            Console.WriteLine("Weapon: " + _game.Player.Weapon);
            Console.WriteLine("Boots : " + _game.Player.Boots);
            Console.WriteLine("Helmet: " + _game.Player.Helm);
            Console.WriteLine("Press enter to continue");
            Console.ReadLine();
        }
    }
}