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

    //
    // Run!
    //
    public void Run()
    {
        Console.CursorVisible = false;
        Zone zone = _db.GetZone();
        zone.Player = CreatePlayer();

        _db.InsertPlayer(zone.Player);
        while (true)
        {
            zone.Entities = _db.GetEntities();

            DrawZone(zone);
            DrawEntity(zone);
            DrawPlayer(zone);

            ReadInput(zone);

        }
    }

    private static void ReadInput(Zone zone)
    {
        ConsoleKeyInfo cki = Console.ReadKey();

        if (cki.Key == ConsoleKey.UpArrow)
        {
            if (zone.Player.Entity.Y<zone.Height)
            {
                zone.Player.Entity.Y--;
            }
            // zone.Player.Entity.Y--;
        }
        if (cki.Key == ConsoleKey.DownArrow)
        {
            // if (zone.Player.Entity.Y < zone.Height)
            // {
            //     zone.Player.Entity.Y++;
            // }
            zone.Player.Entity.Y++;
        }
        if (cki.Key == ConsoleKey.LeftArrow)
        {
            zone.Player.Entity.X--;
        }
        if (cki.Key == ConsoleKey.RightArrow)
        {
            zone.Player.Entity.X++;
        }
    }


    //
    // Draws a Zone in the console
    //
    public void DrawZone(Zone zone)
    {
        Console.SetCursorPosition(0, 0);
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

    //
    // Draws all entities in the zone
    //
    public void DrawEntity(Zone zone)
    {
        foreach (var item in zone.Entities)
        {
            Console.SetCursorPosition(item.X, item.Y);
            Console.WriteLine(item.Symbol);

        }
    }

    //
    // Create a player
    //
    public Player CreatePlayer()
    {
        Menu menu = new Menu("Choose class", new string[] { "Warrior", "Mage", "Rogue" });
        Player player = new Player();

        Console.Clear();
        player.Entity.Symbol = 'P';
        Console.WriteLine("Enter Character Name");
        player.Name = Console.ReadLine(); //här skickar vi in namnet som spelaren skriver in
        Console.Clear();
        CharacterClass characterClass = (CharacterClass)menu.Run(); //Här skickar vi in spelarens klass
        Console.Clear();

        return player;
    }

    //
    // Draw the player
    //
    public void DrawPlayer(Zone zone)
    {
        Console.SetCursorPosition(zone.Player.Entity.X, zone.Player.Entity.Y);
        Console.WriteLine(zone.Player.Entity.Symbol);
    }
}
