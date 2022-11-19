namespace ZoneRpg.Shared
{
    public class ChatBox
    {
        public int Height { get; set; } = 12;
        public int Width { get; set; } = 45;
        public string Name { get; set; } = "Unamed zone";
        public List<Message> Messages { get; set; } = new List<Message>();
        public List<Entity> Entities = new();
        public Player Player = new();
        public ChatBox() { }
        public ChatBox(string name)
        {
            Height = 12;
            Width = 45;
            Name = "Chat";
        }

    }

}
