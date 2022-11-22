namespace ZoneRpg.Models
{
    public class Door
    {
        public int Id { get; set; }
        public int EntityId { get; set; }
        public int TargetZoneId { get; set; }
        public int TargetX { get; set; }
        public int TargetY { get; set; }
    }
}