namespace ZoneRpg.Shared
{
    public class Zone
    {
        public int Height { get; set; } = 12;
        public int Width { get; set; } = 45;
        public string Name { get; set; } = "Unamed zone";
       
        public List<Entity> Entities = new();
        public Character Player = new();
        public Zone() { }
        public Zone(string name)
        {
            Height = 12;
            Width = 45;
            Name = name;
        }
    }

}
