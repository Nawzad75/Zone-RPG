namespace ZoneRpg.Shared
{
    public class MessageBox
    {
        public int Height { get; set; } = 12;
        public int Width { get; set; } = 45;
        public string Name { get; set; } = "Unamed zone";

        public List<Entity> Entities = new();
        public Player Player = new();
        public MessageBox() { }
        public MessageBox(string name)
        {
            Height = 12;
            Width = 45;
            Name = "Chat";
        }

        public void AddMessage(Message message)
        {
            throw new NotImplementedException();
        }
    }

}
