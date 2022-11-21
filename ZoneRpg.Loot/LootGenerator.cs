using ZoneRpg.Shared;
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

            // Loot that will be returned (after we have added items to it)
            List<Item> loot = new List<Item>();

            // All available items
            List<ItemInfo> allItemInfos = _db.GetAllItemInfos();

            // If we have specified a rarity, filter out all items that are not that rarity
            if (rarity > 0)
            {
                allItemInfos = allItemInfos.Where(x => x.Rarity == rarity).ToList();
            }

            int lootAmount = random.Next(1, 3);

            // Add random item to the loot
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
