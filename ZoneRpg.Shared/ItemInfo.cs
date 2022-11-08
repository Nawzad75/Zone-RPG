using ZoneRpg.Database;
namespace ZoneRpg.Shared
{
    public class ItemInfo
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public int ItemTypeId { get; set; }
        public int Rarity { get; set; }
        public string Description { get; set; } = "";


    }
}