namespace ZoneRpg.Models
{
    public class ItemInfo
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public int ItemTypeId { get; set; }
        public int Rarity { get; set; }
        public string Description { get; set; } = "";
        public int Attack { get; set; }
        public int Defense { get; set; }

        // En liten genväg för att få ItemTypeId som ett "ItemType-enum" värde
        public ItemType ItemType { get { return (ItemType)ItemTypeId; } }

        public override string ToString()
        {
            return $"ItemInfo id: {Id}, name: {Name}, type: {ItemType}, typeid: {ItemTypeId}";
        }
    }
}