using ZoneRpg.Database;
using ZoneRpg.Ui;

internal class Program
{
    private static void Main(string[] args)
    {
        DatabaseManager db = new DatabaseManager();
        Class1 class1 = new Class1(db);

    }
}