namespace ZoneRpg.Models
{
    public class Item
    {
        public int Id { get; set; }
        public Character? Player = null; // Om itemet har en ägare, så finns ägaren här:
        public ItemInfo? ItemInfo { get; set; }
        
        public Item(){} // Parameterlös konstruktor för Dapper
        public int ItemInfoId { get { return ItemInfo!.Id; } } // För dapper
        
        public Item(ItemInfo itemType)
        {
            ItemInfo = itemType;
        }

        override public string ToString()
        {
            return ItemInfo!.Description;
        }
    }
}