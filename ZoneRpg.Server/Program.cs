
using ZoneRpg.Database;
using ZoneRpg.Models;

internal class Program
{
    static DatabaseManager db = new DatabaseManager();
    private static void Main(string[] args)
    {
        while (true)
        {
            Console.WriteLine("Processing: " + DateTime.Now);
            SpawnMonstersInZone(1);
            Console.WriteLine("------------------------\n\n");
            Thread.Sleep(20000);
        }
    }

    ///
    private static void SpawnMonstersInZone(int zoneId, int maxMonsters = 3)
    {
        List<Monster> monsters = db.GetMonsters(zoneId);
        foreach (Monster monster in monsters)
        {
            Console.WriteLine($"Monster: {monster.Name} ({monster.Hp}/{monster.MonsterClass!.MaxHp})");
        }

        Random rnd = new Random();
        if (monsters.Count < maxMonsters)
        {
            MonsterClass monsterClass = db.GetMonsterClassByName("Dragon");
            Monster newMonser = new Monster(monsterClass, zoneId);
            Zone zone = db.GetZone(zoneId);
           
            // Slumpa position, om positionen är upptagen, slumpa igen.
            do
            {
                newMonser.Entity.X = rnd.Next(1, zone.Width);
                newMonser.Entity.Y = rnd.Next(1, zone.Height);
            } while (monsters.Any(m => m.Entity.X == newMonser.Entity.X && m.Entity.Y == newMonser.Entity.Y));


            if (zoneId == 1){
                // Ser till att det finns ett monster framför dörren!
                if (!monsters.Any(m => m.Entity.X == 40 && m.Entity.Y == 10))
                {
                    newMonser.Entity.X = 40;
                    newMonser.Entity.Y = 10;
                }
            }

            db.InsertMonster(newMonser);
            Console.WriteLine("New monster spawned at: " + newMonser.Entity.X + ", " + newMonser.Entity.Y);
        }
    }
}