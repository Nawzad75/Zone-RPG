
using ZoneRpg.Shared;

using ZoneRpg.GameLogic;

namespace ZoneRpg.UserInterface
{
   internal class ChatboxRenderer : BaseRenderer, IRenderer

    {
        private ChatBox _chatBox;
        public ChatboxRenderer(ChatBox chatBox) 
        {
            _chatBox = chatBox;
            
        }

        public override void Draw()
        {
           
            int y = 0;
            List<Message> messages = _chatBox.Messages.TakeLast(12).ToList();
            messages.AddRange(_chatBox.LootMessages);
            messages.Sort((a,b)=> (int)(a.DateTime.Ticks - b.DateTime.Ticks));
            foreach (var message in messages)
            {
                Console.SetCursorPosition(60, y++);
                Console.Write( message.Character.Name + " : ");
                Console.ForegroundColor = message.Color;
                Console.Write(message.Text);
                Console.ResetColor();
                
                

            }

        }

        public void SetAccentColor(ConsoleColor color)
        {
            throw new System.NotImplementedException();
        }
       
    }
    
}