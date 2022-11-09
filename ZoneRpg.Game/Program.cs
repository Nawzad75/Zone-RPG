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
            

            UiManager uiManager = new UiManager(db);
            uiManager.Run();
        }
    }
}
