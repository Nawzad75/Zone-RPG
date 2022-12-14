
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
            SpawnMonstersInZone(2,1);
            SpawnMonstersInZone(3,10);
            Console.WriteLine("------------------------\n\n");
            Thread.Sleep(20000);
        }
    }

    private static void SpawnMonstersInZone(int zoneId, int maxMonsters = 3)
    {
        List<Monster> monsters = db.GetAllMonsters(zoneId);
        List<Entity> entites = db.GetZoneEntities(zoneId);
        Console.WriteLine($"There are {monsters.Count} monsters in zone {zoneId}.");

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
            } while (entites.Any(e => e.X == newMonser.Entity.X && e.Y == newMonser.Entity.Y));


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