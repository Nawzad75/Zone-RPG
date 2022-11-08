using ZoneRpg.Loot;
using ZoneRpg.Database;
using ZoneRpg.Shared;
using ZoneRpg.Ui;

namespace ZoneRpg.Game
{
    internal class Program
    {


        private static void Main(string[] args)

        {
            DatabaseManager db = new DatabaseManager();
            db.SeedDatabase();

            // --- Marcus testar items ---
            Console.WriteLine("-- New loot dropped! --");
            LootGenerator generator = new LootGenerator(db);
            var loot = generator.GenerateLoot();
            foreach (var item in loot)
            {
                Console.WriteLine("\t" + item);
            };
            Console.WriteLine("-- End of loot --\n\n");
            // --- 


            string prompt = "Welcome to the game";
            string[] options = { "Start", "Exit" };
            Menu mainMenu = new Menu(prompt, options);
            mainMenu.DisplayOptions();
            Console.ReadKey(true);
            
            StartGame startGame = new StartGame();
            startGame.RunMainMenu();

            UiManager uiManager = new UiManager(db);
            uiManager.Run();


            Player player = new Player();
            
            
            Zone zone = db.GetZone();
            while (true)
            {
                uiManager.DrawZone(zone);

                zone.Entities = db.GetEntities();

                uiManager.DrawEntity(zone);
                
                ConsoleKeyInfo cki= Console.ReadKey();
                
                if (cki.Key==ConsoleKey.UpArrow)
                {
                    player.entity.Y--;
                }
                if (cki.Key == ConsoleKey.DownArrow)
                {
                    player.entity.Y++;
                }
                if (cki.Key == ConsoleKey.LeftArrow)
                {
                    player.entity.X--;
                }
                if (cki.Key == ConsoleKey.RightArrow)
                {
                    player.entity.X++;
                }

            }

            
           
            
            
            
            
            
        }



    }
}
