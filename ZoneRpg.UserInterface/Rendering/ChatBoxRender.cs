
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

            foreach (var message in _chatBox.Messages.TakeLast(12))
            {
                Console.SetCursorPosition(60, y++);
                Console.Write( message.character.Name + " " + message.Text);
            }

        }

        public void SetAccentColor(ConsoleColor color)
        {
            throw new System.NotImplementedException();
        }
       
    }
    
}