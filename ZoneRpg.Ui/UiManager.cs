using ZoneRpg.Database;
using ZoneRpg.Shared;

namespace ZoneRpg.Ui
{
    public class UiManager
    {
        // Constant array with all arrow keys
        private static readonly ConsoleKey[] _allArrowKeys = new ConsoleKey[] {
            ConsoleKey.UpArrow,
            ConsoleKey.DownArrow,
            ConsoleKey.LeftArrow,
            ConsoleKey.RightArrow
        };

        // Ui Members
        private DatabaseManager _db;
        private FightManager? _fightManager;
        private string _fightResult = "";
        private IZoneVisualizer _zoneVisualizer = new ZoneVisualizerAscii();
        // private IZoneVisualizer _zoneVisualizer = new ZoneVisualizerUtf8();

        // Constructor
        public UiManager(DatabaseManager db)
        {
            _db = db;
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
            _fightManager = new FightManager(_db, zone.Player);

            while (true)
            {
                zone.Entities = _db.GetEntities();
                _zoneVisualizer.DrawZone(zone);
                _zoneVisualizer.DrawEntities(zone.Entities);
                _zoneVisualizer.DrawPlayerEntity(zone.Player.Entity);
                ReadInput(zone);
                OpenChest(zone);
                // LookForFight(zone);
                // _fightManager.HandleFight();
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
        public Character ChoosePlayer()
        {
            Console.Clear();
            List<Character> characters = _db.GetCharacters();
            string[] options = characters.Select(c => $"{c.Name}  (id: {c.id})").ToArray();
            int index = new Menu<int>("Choose a character:", options).Run();

            return characters[index];
        }

        //
        // Create monster
        //
        public Monster CreateMonster()
        {
            Monster monster = new Monster();
            monster.Entity.Symbol = 'M';
            monster.Name = "Monster";
            return monster;
        }

        public Kista CreateChest()
        {
            Kista kista = new Kista();
            kista.Entity.Symbol = 'K';
            kista.Name = "Kista";
            return kista;
        }
       
        public void OpenChest(Zone zone)
        
        
        {
            

            if (kista.Entity.X == player.Entity.X && kista.Entity.Y == player.Entity.Y)
            {
                Console.WriteLine("Du har öppnat en kista och hittat en ny vapen");

                Console.ReadKey();
            }
            //när player och kista är på samma plats så öppnas kistan och spelaren får ett vapen
            
            
        }

        // Reads user input and takes action.
        //
        // @param zone - We need a zone to restrict the player movement
        //
        private void ReadInput(Zone zone)
        {
            ConsoleKeyInfo cki = Console.ReadKey();
            if (_allArrowKeys.Contains(cki.Key))
            {
                zone.Player.Move(cki.Key, zone);
                _db.UpdateEntityPosition(zone.Player.Entity);
            }
        }
    }
}