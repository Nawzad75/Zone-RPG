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
            StartGame startGame = new StartGame();
            startGame.RunMainMenu();

            UiManager uiManager = new UiManager(db);
            uiManager.Draw(new Zone(12, 45, "Borås"));


        }




    }
}
