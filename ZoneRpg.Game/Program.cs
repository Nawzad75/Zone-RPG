using System.Text;
using ZoneRpg.Database;
using ZoneRpg.UserInterface;

namespace ZoneRpg.Game
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            DatabaseManager db = new DatabaseManager();
            db.SeedDatabase();


            Ui ui = new Ui(db);
            ui.Run();
        }
    }
}
