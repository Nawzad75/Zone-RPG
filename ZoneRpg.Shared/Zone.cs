namespace ZoneRpg.Shared
{
    public class Zone
    {
        public int Height { get; set; }
        public int Width { get; set; }
        public string Name { get; set; }
        public Zone(int height, int width, string name)
        {
            Height = height;
            Width = width;
            Name = name;
        }

    }

}
