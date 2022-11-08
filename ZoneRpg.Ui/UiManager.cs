using ZoneRpg.Database;
using ZoneRpg.Shared;

namespace ZoneRpg.Ui;
public class UiManager
{
    private DatabaseManager _db;

    public UiManager(DatabaseManager db)
    {
        _db = db;
    }
    public void Draw(Zone zone)
    {
        // zone.Height=;
        // zone.Width = 50;
        // Console.WriteLine("Write Zone name:");
        // zone.Name = Console.ReadLine();

        for (int i = 0; i < zone.Width; i++)
        {
            Console.Write("-");

        }
        Console.WriteLine("");

        for (int i = 0; i < zone.Height+1; i++)
        {
            //   Console.WriteLine("|                                            |");
            Console.Write("|");
            for (int j = 0; j < zone.Width-2; j++)
            {
                Console.Write(" ");

            }
            Console.WriteLine("|");

        }
        
        for (int i = 0; i < zone.Width; i++)
        {
            Console.Write("-");

        }


    }
}
