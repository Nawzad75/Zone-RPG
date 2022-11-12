using System.Text;
using ZoneRpg.Database;
using ZoneRpg.Shared;
using ZoneRpg.GameLogic;

namespace ZoneRpg.UserInterface
{
    public class Ui
    {
        // Ui Members
        private DatabaseManager _db;
        private IZoneRenderer _zoneRenderer = new ZoneRendererAscii();
        private ICharacterRenderer _playerRenderer = new CharacterRenderer();
        private ICharacterRenderer _monsterRenderer = new CharacterRenderer();
        private InputState _inputState = InputState.ZoneMovement;
        BattleManager _battleManager;
        private Game _game;

        // Constructor
        public Ui(DatabaseManager db, Game game)
        {
            _db = db;
            _game = game;
            _battleManager = game.BattleManager;
            Setup();
        }

        // Setup the UI
        public void Setup()
        {
            // Prepare the console
            Console.OutputEncoding = Encoding.UTF8;
            Console.CursorVisible = false;
            Console.Clear();

            // Show the intro screen and start menu
            new StartGame().RunMainMenu();

            _playerRenderer.SetDrawOrigin(2, _game.Zone.Height + 2);
            _playerRenderer.SetAccentColor(ConsoleColor.Cyan);
            _monsterRenderer.SetDrawOrigin(24, _game.Zone.Height + 2);
            _monsterRenderer.SetAccentColor(ConsoleColor.Red);
        }

        //
        // Run!
        //
        public void Render()
        {
            if (_game.state == GameState.GetPlayerCharacter){
                _game.Player = CreateOrChoosePlayer();
                _game.state = GameState.Playing;
            }
            
            _zoneRenderer.DrawZone(_game.Zone);
            _zoneRenderer.DrawEntities(_game.Zone.Entities);
            _zoneRenderer.DrawPlayerEntity(_game.Player.Entity);
            _zoneRenderer.DrawBattle(_battleManager.GetBattleStatus());
            _playerRenderer.DrawCharacter(_game.Player);
            _monsterRenderer.DrawCharacter(_battleManager.GetMonster());

            // Är detta UI? (Jag tror inte det) 
            // Hur flyttar vi det nån annanstans?
            OpenChest();
            _battleManager.LookForMonsters(_game.Zone.Entities);
            _battleManager.ProgressBattle();
            HandlePlayerDeath();

        }

        private void HandlePlayerDeath()
        {
            if (_game.Player != null && _game.Player.IsDead())
            {
                Console.Clear();
                Console.WriteLine("You died!");
                Console.WriteLine("Press any key to exit...");
                _inputState = InputState.Dead;
            }
        }

        //
        // Let the player choose a character or create a new one
        //
        private Player CreateOrChoosePlayer()
        {
            string prompt = "Create or choose a character!";
            CreateChooseMenu createOption = new Menu<CreateChooseMenu>(prompt).Run();

            if (createOption == CreateChooseMenu.Create)
            {
                Player player = CreatePlayer();
                _db.InsertCharacter(player);
                return player;
            }
            return ChoosePlayer();
        }

        //
        // Create a player
        //
        public Player CreatePlayer()
        {
            Console.Clear();
            Console.WriteLine("Enter Character Name");
            string name = Console.ReadLine()!; //här skickar vi in namnet som spelaren skriver in
            CharacterClass characterClass = new Menu<CharacterClass>("Choose class").Run();

            return new Player(name, characterClass);
        }

        //
        // Choose player
        //
        public Player ChoosePlayer()
        {
            Console.Clear();
            List<Player> players = _db.GetPlayers();
            string[] options = players.Select(c => $"{c.Name}  (id: {c.id})").ToArray();
            int index = new Menu<int>("Choose a character:", options).Run();

            return players[index];
        }

        //
        // Create monster
        //
        public Monster CreateMonster()
        {
            Monster monster = new Monster();
            monster.Entity.Symbol = "🐉";
            monster.Name = "Monster";
            return monster;
        }

        public Kista CreateChest()
        {
            Kista kista = new Kista();
            kista.Entity.Symbol = "K";
            kista.Name = "Kista";
            return kista;
        }


        public void OpenChest()
        {
            Entity? chestEntity = _game.Zone.Entities.Find(entity => entity.EntityType == EntityType.Chest);
            if (chestEntity == null)
            {
                return;
            }

            if (chestEntity.X == _game.Player.Entity.X && chestEntity.Y == _game.Player.Entity.Y)
            {
                //öppnar kistan och får ett svärd från databasen
                Console.WriteLine("Du har öppnat en kista och fått ett svärd!");
                List<ItemInfo> allItemInfos = _db.GetAllItemInfos();
                ItemInfo? sword = allItemInfos.Find(item => item.Name == "Sword");

                if (sword != null)
                {
                    // zone.Player.
                }


            }


        }

        // Reads user input and takes action.
        //
        // @param zone - We need a zone to restrict the player movement
        //
        public void ReadInput()
        {
            ConsoleKeyInfo cki = Console.ReadKey();

            switch (_inputState)
            {
                case InputState.ZoneMovement:
                    if (Constants.AllArrowKeys.Contains(cki.Key))
                    {
                        _game.Player.Move(cki.Key, _game.Zone);
                        _db.UpdateEntityPosition(_game.Player.Entity);
                    }
                    break;

                case InputState.Battle:

                    break;

                case InputState.Dead:
                    if (cki.Key == ConsoleKey.Enter)
                    {
                        RespawnPlayer();
                    }
                    break;
            }
        }

        private void RespawnPlayer()
        {
            Console.Clear();
            _game.Player.Respawn();
            _db.UpdateEntityPosition(_game.Player.Entity);
            _inputState = InputState.ZoneMovement;
        }
    }
}