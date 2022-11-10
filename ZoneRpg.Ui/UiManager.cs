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

        zone.Player = CreateOrChoosePlayer();

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

    private Character CreateOrChoosePlayer()
    {
        CreateOption createOption = (CreateOption)new Menu(
            "Create or choose a character!",
            new string[] { "Create", "Choose" }
        ).Run();

        if (createOption == CreateOption.Create)
        {
            return CreatePlayer();
        }

        return ChoosePlayer();

    }

    private void DrawFightResult()
    {
        Console.WriteLine(_fightResult);
    }

    private void HandleFight(Zone zone)
    {
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
        var allArrowKeys = new[] {
            ConsoleKey.UpArrow,
            ConsoleKey.DownArrow,
            ConsoleKey.LeftArrow,
            ConsoleKey.RightArrow
        };

        if (allArrowKeys.Contains(cki.Key))
        {
            zone.Player.Move(cki.Key, zone);
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

        Character player = new Character();

        player.Entity.Symbol = 'P';
        Console.WriteLine("Enter Character Name");
        player.Name = Console.ReadLine()!; //här skickar vi in namnet som spelaren skriver in
        Console.Clear();
        CharacterClass characterClass = (CharacterClass)new Menu(
            "Choose class",
            new string[] { "Warrior", "Mage", "Rogue" }
        ).Run(); //Här skickar vi in spelarens klass
        return player;
    }

    //
    // Choose player
    //
    public Character ChoosePlayer()
    {
        Console.Clear();
        List<Character> characters = _db.GetCharacters();
        string[] options = characters.Select(c => $"{c.Name}  (id: {c.id})").ToArray();
        int index = new Menu("Choose a character:", options).Run();
        return characters[index];
    }

    //
    // Create monster
    //
    public Monster CreateMonster()
    {

        Monster monster = new Monster();
        monster.Entity.Symbol = 'M';
        monster.Name = "Monster";


        return monster;
    }
    public Kista CreateChest()
    {

        Kista kista = new Kista();
        kista.Entity.Symbol = 'K';
        kista.Name = "Kista";
        return kista;
    }
    public void OpenChest()
    {
        Character player = new Character();
        Kista kista = new Kista();

        if (kista.Entity.X == player.Entity.X && kista.Entity.Y == player.Entity.Y)
        {
            Console.WriteLine("Du har öppnat en kista och hittat en ny vapen");
        }
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

