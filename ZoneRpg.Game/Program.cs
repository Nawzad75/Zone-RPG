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
           
           
           
            var pos = Console.GetCursorPosition();
            Zone zone = new Zone("Borås");
            uiManager.Draw(zone);
            zone.entities.Add(new Entity(pos.Left+3,pos.Top+ 5,'M',10));
            uiManager.DrawEntity(zone);
           
           Console.ReadLine();

        }




    }
}
