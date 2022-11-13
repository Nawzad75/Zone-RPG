namespace ZoneRpg.Shared
{

    public class Message
    {
        public int Id { get; set; }
  
        public Character character { get; set; } = new Character();
        public string Text { get; set; } = "";
        // public DateTime Time { get; set; } = DateTime.Now;
        public Message(){}
        public Message(string text, Character character)
        {
            Text = text;
            this.character = character;
        }
       
    }
}