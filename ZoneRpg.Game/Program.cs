using System.Text;
using ZoneRpg.Database;
using ZoneRpg.Ui;

namespace ZoneRpg.Game
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            DatabaseManager db = new DatabaseManager();
            db.SeedDatabase();
            

            UiManager uiManager = new UiManager(db);
            uiManager.Run();
        }
    }
}
