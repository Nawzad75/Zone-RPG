using ZoneRpg.Database;
using ZoneRpg.Shared;

namespace ZoneRpg.Ui
{
    public class UiManager
    {
        // Ui Members
        private DatabaseManager _db;
        private InputState _inputState;
        private BattleManager? _battleManager;
        private IZoneVisualizer _zoneVisualizer = new ZoneVisualizerAscii();
        // private IZoneVisualizer _zoneVisualizer = new ZoneVisualizerUtf8();

        // Constructor
        public UiManager(DatabaseManager db)
        {
            _db = db;
            _battleManager = new BattleManager(_db);
        }

        //
        // Run!
        //
        public void Run()
        {
            // Prepare the console
            Console.Clear();
            Console.CursorVisible = false;

            // Show the intro screen and start menu
            new StartGame().RunMainMenu();

            // Get the starting zone for the player
            Zone zone = _db.GetZone();
            zone.Player = CreateOrChoosePlayer();
            _battleManager.AddPlayerToBattle(zone.Player);
            

            while (true)
            {
                zone.Entities = _db.GetEntities();
                _zoneVisualizer.DrawZone(zone);
                _zoneVisualizer.DrawBattle(_battleManager.Status);
                _zoneVisualizer.DrawEntities(zone.Entities);
                _zoneVisualizer.DrawPlayerEntity(zone.Player.Entity);

                HandlePlayerDeath(_battleManager.Status);

                ReadInput(zone);
                OpenChest(zone);
                // LookForFight(zone);
                // _fightManager.HandleFight();
                _battleManager.LookForMonsters(zone.Entities);
                _battleManager.ProgressBattle();
            }
        }

        //
        //
        //
        private void HandlePlayerDeath(BattleStatus status)
        {
            if (status.State == BattleState.Lost)
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
        private Character CreateOrChoosePlayer()
        {
            string prompt = "Create or choose a character!";
            CreateChooseMenu createOption = new Menu<CreateChooseMenu>(prompt).Run();

            if (createOption == CreateChooseMenu.Create)
            {
                Character player = CreatePlayer();
                _db.InsertCharacter(player);
                return player;
            }
            return ChoosePlayer();
        }


        //
        // Create a player
        //
        public Character CreatePlayer()
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


        public void OpenChest(Zone zone)
        {
            Entity? chestEntity = zone.Entities.Find(entity => entity.EntityType == EntityType.Chest);
            if (chestEntity == null)
            {
                return;
            }

            if (chestEntity.X == zone.Player.Entity.X && chestEntity.Y == zone.Player.Entity.Y)
            {
                Console.WriteLine("Du har öppnat en kista och hittat en ny vapen");

                Console.ReadKey();
            }


        }

        // Reads user input and takes action.
        //
        // @param zone - We need a zone to restrict the player movement
        //
        private void ReadInput(Zone zone)
        {
            ConsoleKeyInfo cki = Console.ReadKey();

            switch (_inputState)
            {
                case InputState.ZoneMovement:
                    if (Constants.AllArrowKeys.Contains(cki.Key))
                    {
                        zone.Player.Move(cki.Key, zone);
                        _db.UpdateEntityPosition(zone.Player.Entity);

                    }
                    break;

                case InputState.Battle:

                    break;

                case InputState.Dead:
                    if (cki.Key == ConsoleKey.Enter)
                    {
                        Console.Clear();
                        Player player = (Player) zone.Player; // Cast from "Character" till "Player"
                        player.Respawn();
                        _battleManager.Reset();
                        _inputState = InputState.ZoneMovement;
                    }
                    break;


            }



        }
    }
}