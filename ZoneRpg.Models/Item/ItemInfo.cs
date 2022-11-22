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

        // Genväg för att få ItemTypeId som en enum
        public ItemType ItemType { get { return (ItemType)ItemTypeId; } }
        // Setter för att dapper inte hittar "ItemTypeId"
        public int item_type_id { set { ItemTypeId = value; } } 


        public override string ToString()
        {
            return $"ItemInfo id: {Id}, name: {Name}, type: {ItemType}, typeid: {ItemTypeId}";
        }
    }
}