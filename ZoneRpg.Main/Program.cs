using ZoneRpg.Database;
using ZoneRpg.UserInterface;
using ZoneRpg.GameLogic;

namespace ZoneRpg.Main
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            DatabaseManager db = new DatabaseManager();
                        
            Game game = new Game(db);

            Ui ui = new Ui(db, game);

            while (true)
            {
                game.Update();
                ui.Render();
                ui.ReadInput();
            }
        }
    }
}