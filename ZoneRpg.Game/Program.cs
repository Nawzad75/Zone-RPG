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

            string prompt = "Welcome to the game";
            string[] options = { "Start", "Exit" };
            Menu mainMenu = new Menu(prompt, options);
            mainMenu.DisplayOptions();
            Console.ReadKey(true);


            UiManager uiManager = new UiManager(db);
            uiManager.Draw(new Zone(12,45,"Borås"));
           
           

        }
        
    }
}
