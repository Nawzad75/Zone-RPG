using ZoneRpg.Database;
namespace ZoneRpg.Shared
{
    public class Item
    {
        public int Id { get; set; }
        public Character? Player = null; // Om itemet har en ägare, så finns ägaren här:
        public ItemInfo ItemInfo { get; set; }
        public Item(ItemInfo itemType)
        {
            ItemInfo = itemType;
        }
        public Item(){}

        override public string ToString()
        {
            return ItemInfo.Description;
        }
    }
}