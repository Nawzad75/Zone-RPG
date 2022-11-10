using ZoneRpg.Database;
using ZoneRpg.Shared;

namespace ZoneRpg.Ui;
public class UiManager
{
    private DatabaseManager _db;
    private FightManager? _currentFight;
    private string _fightResult = "";

    public UiManager(DatabaseManager db)
    {
        _db = db;
    }

    //
    // Run!
    //
    public void Run()
    {
        Console.Clear();
        Console.CursorVisible = false;
        new StartGame().RunMainMenu();
        Zone zone = _db.GetZone();
        zone.Player = CreatePlayer();

        _db.InsertCharacter(zone.Player);
        while (true)
        {
            zone.Entities = _db.GetEntities();

            DrawZone(zone);
            DrawFightResult();
            DrawEntities(zone);
            DrawPlayer(zone);
            ReadInput(zone);
            LookForFight(zone);
            HandleFight(zone);
        }
    }

    private void DrawFightResult()
    {
        Console.WriteLine(_fightResult);
    }

    private void HandleFight(Zone zone)
    {
        throw new NotImplementedException();
    }

    //
    // Marcus kommentar:  Tycker inte om att _currentFight ändras här. 
    // Det är gömt och borde nog synas i main loopen.  
    //
    private void LookForFight(Zone zone)
    {
        if (_currentFight != null)
        {
            return;
        }
        foreach (var entity in zone.Entities)
        {
            if (entity.X == zone.Player.Entity.X && entity.Y == zone.Player.Entity.Y && entity.Symbol == 'm')
            {
                Monster monster = _db.GetMonsterByEntityId(entity.Id);
                _currentFight = new FightManager(zone.Player, monster);
            }
        }
    }

    //
    //
    //
    private void ReadInput(Zone zone)
    {
        ConsoleKeyInfo cki = Console.ReadKey();

        if (cki.Key == ConsoleKey.UpArrow)
        {
            zone.Player.MoveUpp();
            _db.UpdateEntityPosition(zone.Player.Entity);
        }
        if (cki.Key == ConsoleKey.DownArrow)
        {
            zone.Player.MoveDown(zone.Height + 1);
            _db.UpdateEntityPosition(zone.Player.Entity);
        }
        if (cki.Key == ConsoleKey.LeftArrow)
        {
            zone.Player.MoveLeft(zone.Width - 45);
            _db.UpdateEntityPosition(zone.Player.Entity);
        }
        if (cki.Key == ConsoleKey.RightArrow)
        {
            zone.Player.MoveRight(zone.Width - 2);
            _db.UpdateEntityPosition(zone.Player.Entity);
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
    public void DrawEntities(Zone zone)
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
    public Character CreatePlayer()
    {
        int Choose;
        Console.WriteLine("Choose one of following optins:");
        Console.WriteLine("1.Creat new character:");
        Console.WriteLine("2.Choose a character:");
        Choose = (Convert.ToInt32(Console.ReadLine()));
        Character player = new Character();
        switch (Choose)

        {
            case 1:

                player.Entity.Symbol = 'P';

                Console.Clear();
                Console.WriteLine("Enter Character Name");

                player.Name = Console.ReadLine()!; //här skickar vi in namnet som spelaren skriver in
                Console.Clear();

                CharacterClass characterClass = (CharacterClass)new Menu(
                    "Choose class",
                    new string[] { "Warrior", "Mage", "Rogue" }
                ).Run(); //Här skickar vi in spelarens klass

                Console.Clear();

                break;
            case 2:
                Console.Clear();
                Console.WriteLine("Choose a character:");
                List<Character> characters = _db.GetCharacters();
                int index = 0;
                foreach (var character in characters)
                {
                    Console.WriteLine($"{index++}. {character.Name}");
                }
                int choice = Convert.ToInt32(Console.ReadLine());
                player = characters[choice];


                break;

        }
        return player;
    }
    public Monster CreateMonster()
    {

        Monster monster = new Monster();
        monster.Entity.Symbol = 'M';
        monster.Name = "Monster";


        return monster;
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
