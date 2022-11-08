using ZoneRpg.Database;
namespace ZoneRpg.Shared
{
    public class Item
    {
        public int Id { get; set; }
        public Player? Player = null; // Om itemet har en ägare, så finns ägaren här:
        public ItemInfo ItemType { get; set; }
        public Item(ItemInfo itemType)
        {
            ItemType = itemType;
        }

        override public string ToString()
        {
            return ItemType.Description;
        }
    }
}