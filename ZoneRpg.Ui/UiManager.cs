using ZoneRpg.Database;
using ZoneRpg.Shared;

namespace ZoneRpg.Ui;
public class UiManager
{
    private DatabaseManager _db;
   
    public UiManager(DatabaseManager db)
    {
        Console.Clear();
        _db = db;
    }
    public void DrawZone(Zone zone)
    {
        Console.Clear();
       
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
        Console.WriteLine();
        Console.WriteLine("                Zone: " + zone.Name);
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
        Menu menu = new Menu("Ange din class" , new string[] { "Warrior", "Mage", "Rogue" });
        Player player = new Player();
        Console.Clear();
        Console.WriteLine("Enter Character Name");
        string name = Console.ReadLine(); //här skickar vi in namnet som spelaren skriver in
        Console.Clear();
        CharacterClass characterClass = (CharacterClass)menu.Run(); //Här skickar vi in spelarens klass
        Console.Clear();
         
        return new Player();


    }
}
