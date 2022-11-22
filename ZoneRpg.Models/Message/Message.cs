namespace ZoneRpg.Models
{

    public class Message
    {
        public int Id { get; set; }
        public ConsoleColor Color {get; set;}
        public DateTime DateTime {get; set;}
        public Character Character { get; set; } = new Character();
        public string Text { get; set; } = "";
        // public DateTime Time { get; set; } = DateTime.Now;
        public Message(){
            Color = ConsoleColor.Cyan;
        }
        public Message(string text, Character character, ConsoleColor color = ConsoleColor.White)
        {
            Text = text;
            Character = character;
            Color = color;
            DateTime = DateTime.Now;
        }
       
    }
}