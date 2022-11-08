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
            // mainMenu.DisplayOptions();
            Console.ReadKey(true);

            StartGame startGame = new StartGame();
            startGame.RunMainMenu();

            UiManager uiManager = new UiManager(db);



            var pos = Console.GetCursorPosition();
            Zone zone = new Zone("Borås");
            uiManager.Draw(zone);
            zone.entities.Add(new Entity(pos.Left + 3, pos.Top + 5, 'M', 10));
            uiManager.DrawEntity(zone);

            Console.ReadLine();

        }

<<<<<<< HEAD
=======



>>>>>>> 90e8b8568411f6cbfb114ecd2971c7ff8539f7a4
    }
}
