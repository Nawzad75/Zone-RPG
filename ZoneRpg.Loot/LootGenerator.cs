using ZoneRpg.Models;
using ZoneRpg.Database;

namespace ZoneRpg.Loot
{

    public class LootGenerator
    {
        private DatabaseManager _db;
        public LootGenerator(DatabaseManager db)
        {
            _db = db;
        }

        // Generera loot
        public List<Item> GenerateLoot(int rarity = 0)
        {
            Random random = new Random();

            // Loot som vi returnerar (efter att vi har lagt till items till den)
            List<Item> loot = new List<Item>();

            // Alla items som finns i databasen
            List<ItemInfo> allItemInfos = _db.GetAllItemInfos();

            // Om vi har specificerat en rarity, filtrera bort alla items som inte är den rarityn
            if (rarity > 0)
            {
                allItemInfos = allItemInfos.Where(x => x.Rarity == rarity).ToList();
            }

            int lootAmount = random.Next(1, 3);

            // Lägg till ett random item till looten
            for (int i = 0; i < lootAmount; i++)
            {
                int randomItemIndex = random.Next(0, allItemInfos.Count);
                ItemInfo randomItemInfo = allItemInfos[randomItemIndex];
                loot.Add(new Item(randomItemInfo));
            }

            return loot;
        }
    }
}
