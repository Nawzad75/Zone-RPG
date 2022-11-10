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

                DrawZone(zone);
                DrawFightResult();
                DrawEntities(zone);
                DrawPlayer(zone);
                ReadInput(zone);
                // LookForFight(zone);
                // _fightManager.HandleFight();
            }
        }

        //
        // Let the player choose a character or create a new one
        //
        private Character CreateOrChoosePlayer()
        {
            CreateOption createOption = (CreateOption)new Menu(
                "Create or choose a character!",
                new string[] { "Create", "Choose" }
            ).Run();

            return (createOption == CreateOption.Create) ? CreatePlayer() : ChoosePlayer();
        }
    

        //
        // Draw the fight results
        //
        private void DrawFightResult()
        {
            Console.WriteLine(_fightResult);
        }

        //
        // Reads user input and takes action!
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



        //
        // Draws a Zone
        //
        public void DrawZone(Zone zone)
        {

            string c1 = "╔";
            string c2 = "╗";
            string c3 = "╝";
            string c4 = "╚";
            string space = "═";
            string I = "║";
            Console.SetCursorPosition(0, 0);
            Console.Write(c1);
            for (int i = 1; i < zone.Width - 1; i++)
            {
                Console.Write(space);
            }
            Console.WriteLine(c2);

            for (int i = 2; i < zone.Height; i++)
            {
                Console.Write(I);
                for (int j = 2; j < zone.Width; j++)
                {
                    Console.Write(" ");
                }
                Console.WriteLine(I);
            }
            Console.Write(c4);

            for (int i = 2; i < zone.Width; i++)
            {
                Console.Write(space);

            }
            Console.WriteLine(c3);
            Console.WriteLine("                Zone: " + zone.Name);
        }

        //
        // Draws all entities in the zone
        //
        public void DrawEntities(Zone zone)
        {
            foreach (var item in zone.Entities)
            {
                Console.SetCursorPosition(item.X, item.Y);
                Console.WriteLine(item.Symbol);

            }
        }

        //
        // Create a player
        //
        public Character CreatePlayer()
        {

            Character player = new Character();
            player.Entity.X = 22;
            player.Entity.Y = 6;
            player.Entity.Symbol = 'P';
            player.Entity.EntityType = EntityType.Player;
            Console.WriteLine("Enter Character Name");
            player.Name = Console.ReadLine()!; //här skickar vi in namnet som spelaren skriver in
            Console.Clear();
            CharacterClass characterClass = (CharacterClass)new Menu(
                "Choose class",
                new string[] { "Warrior", "Mage", "Rogue" }
            ).Run(); //Här skickar vi in spelarens klass
            return player;
        }

        //
        // Choose player
        //
        public Character ChoosePlayer()
        {
            Console.Clear();
            List<Character> characters = _db.GetCharacters();
            string[] options = characters.Select(c => $"{c.Name}  (id: {c.id})").ToArray();
            int index = new Menu("Choose a character:", options).Run();
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
        public void OpenChest()
        {
            Character player = new Character();
            Kista kista = new Kista();

            if (kista.Entity.X == player.Entity.X && kista.Entity.Y == player.Entity.Y)
            {
                Console.WriteLine("Du har öppnat en kista och hittat en ny vapen");
            }
        }

        //
        // Draw the player
        //
        public void DrawPlayer(Zone zone)
        {
            Console.SetCursorPosition(zone.Player.Entity.X, zone.Player.Entity.Y);
            Console.WriteLine(zone.Player.Entity.Symbol);
        }
    }

}