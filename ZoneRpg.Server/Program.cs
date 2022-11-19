
using ZoneRpg.Database;
using ZoneRpg.Shared;

internal class Program
{
    static DatabaseManager db = new DatabaseManager();
    private static void Main(string[] args)
    {
        db.DeleteMonstersInZone(1);
        while (true)
        {
            Console.WriteLine("Processing: " + DateTime.Now);
            SpawnMonstersInZone(1, 3);
            Console.WriteLine("------------------------\n\n");
            Thread.Sleep(10000);
        }
    }

    ///
    private static void SpawnMonstersInZone(int zoneId, int maxMonsters = 3)
    {
        List<Monster> monsters = db.GetMonsters(zoneId);
        foreach (Monster monster in monsters)
        {
            Console.WriteLine($"Monster: {monster.Name} ({monster.Hp}/{monster.MaxHp})");
        }

        Random rnd = new Random();
        if (monsters.Count < maxMonsters)
        {
            MonsterClass monsterClass = db.GetMonsterClassByName("Dragon");
            Monster monster = new Monster(monsterClass);
            monster.Entity.ZoneId = zoneId;
            // Random position, repeat if position is already taken
            do
            {
                monster.Entity.X = rnd.Next(1, 42);
                monster.Entity.Y = rnd.Next(1, 12);
            } while (monsters.Any(m => m.Entity.X == monster.Entity.X && m.Entity.Y == monster.Entity.Y));

            if (zoneId == 1){
                // Ensure there is a monster at 40, 10
                if (!monsters.Any(m => m.Entity.X == 40 && m.Entity.Y == 10))
                {
                    monster.Entity.X = 40;
                    monster.Entity.Y = 10;
                }
            }

            db.InsertMonster(monster);
            Console.WriteLine("New monster spawned at: " + monster.Entity.X + ", " + monster.Entity.Y);
        }
    }
}