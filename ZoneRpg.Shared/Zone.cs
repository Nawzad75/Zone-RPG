namespace ZoneRpg.Shared
{
    public class Zone
    {
        public int Height { get; set; }
        public int Width { get; set; }
        public string Name { get; set; }
        public Zone( string name)
        {
            Height = 12;
            Width = 45;
            Name = name;
        }
        public List<Entity>entities=new();
    }

}
