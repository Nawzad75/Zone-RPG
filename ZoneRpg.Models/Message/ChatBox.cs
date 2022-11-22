namespace ZoneRpg.Models
{
    public class ChatBox
    {
        public string Name { get; set; }
        public List<Message> Messages { get; set; } = new List<Message>();
        public List<Message> LootMessages {get; set; } = new List<Message>();
        public ChatBox(string name)
        {
            Name = name;
        }
    }
}
