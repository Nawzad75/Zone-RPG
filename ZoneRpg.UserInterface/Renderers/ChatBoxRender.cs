
using ZoneRpg.Models;

namespace ZoneRpg.UserInterface
{
    internal class ChatboxRenderer : BaseRenderer, IRenderer

    {
        private ChatBox _chatBox;
        public ChatboxRenderer(ChatBox chatBox, int x, int y) : base(x, y, 0, 0)
        {
            _chatBox = chatBox;
        }

        public override void Draw()
        {

            int y = _y;
            List<Message> messages = _chatBox.Messages.TakeLast(12).ToList();
            messages.AddRange(_chatBox.LootMessages);
            messages.Sort((a, b) => (int)(a.DateTime.Ticks - b.DateTime.Ticks));
            foreach (var message in messages)
            {
                Console.SetCursorPosition(_x, y++);
                Console.Write(message.Character.Name + " : ");
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