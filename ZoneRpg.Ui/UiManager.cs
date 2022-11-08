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
        foreach (var item in zone.Entities)
        {
            Console.SetCursorPosition(item.X, item.Y);
            Console.WriteLine(item.Symbol);
            
        }
    }
    public void Run()
    {
        Player player = CreatePlayer();
        _db.InsertPlayer(player);
    }

    public Player CreatePlayer()
    {
        Menu menu = new Menu("Choose class" , new string[] { "Warrior", "Mage", "Rogue" });
        Player player = new Player();

        Console.Clear();
        Console.WriteLine("Enter Character Name");
        player.Name = Console.ReadLine(); //här skickar vi in namnet som spelaren skriver in
        Console.Clear();
        CharacterClass characterClass = (CharacterClass)menu.Run(); //Här skickar vi in spelarens klass
        Console.Clear();

        return player;
    }
    public void DrawPlayer(Zone zone)
    {
        Console.SetCursorPosition(zone.Player.entity.X, zone.Player.entity.Y);
        Console.WriteLine(zone.Player.name);      
    }
}
