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
    public void DrawZone(Zone zone)
    {
        Console.Clear();
        Console.WriteLine();
        Console.WriteLine("                Zone: " + zone.Name);
        for (int i = 0; i < zone.Width; i++)
        {
            Console.Write("-");
        }
        Console.WriteLine("");

        for (int i = 0; i < zone.Height + 1; i++)
        {
            Console.Write("|");
            for (int j = 0; j < zone.Width - 2; j++)
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
    public void DrawEntity(Zone zone)
    {
        foreach (var item in zone.entities)
        {
            Console.SetCursorPosition(item.X, item.Y);
            Console.WriteLine(item.Symbol);
            
        }
    }
    public void Run()
    {
        Player player = Creatplayer();
    }

    public Player Creatplayer()
    {
        Player player = new Player();
        Console.Clear();
        Console.WriteLine("Enter Character Name");
        string name = Console.ReadLine(); //här skickar vi in namnet som spelaren skriver in
        Console.Clear();
        Console.WriteLine("What is your class?\nWarrior\nMage\nRogue\n");
        string CharacterClass = Console.ReadLine(); //Här skickar vi in spelarens klass
        Console.Clear();

        return new Player();


    }
}
