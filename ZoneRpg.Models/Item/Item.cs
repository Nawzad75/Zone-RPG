namespace ZoneRpg.Models
{
    public class Item
    {
        public int Id { get; set; }
        public Character? Player = null; // Om itemet har en ägare, så finns ägaren här:
        public ItemInfo ItemInfo { get; set; } = new ItemInfo();

        public Item() { } // Parameterlös konstruktor för Dapper
        public int ItemInfoId { get { return ItemInfo.Id; } } // För dapper

        public Item(ItemInfo itemInfo)
        {
            ItemInfo = itemInfo;
        }

        override public string ToString()
        {
            string output = $"{ItemInfo.Name}";
            if  (ItemInfo.Attack > 0)
            {
                output += $"  (Attack: {ItemInfo.Attack})";
            }
            if (ItemInfo.Defense > 0)
            {
                output += $"  (Defense: {ItemInfo.Defense})";
            }
            return output;
        }
    }
}