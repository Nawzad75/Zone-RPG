using ZoneRpg.GameLogic;
using ZoneRpg.Shared;

namespace ZoneRpg.UserInterface
{
    internal class BattleRenderer : BaseRenderer, IRenderer
    {
        private readonly BattleManager _battleManager;

        public BattleRenderer(BattleManager battleManager)
        {
            _battleManager = battleManager;
        }

        public override void Draw()
        {
            base.Draw();
            Console.SetCursorPosition(_x + 2, _y + 1);
            Console.Write("Battle state: " + _battleManager.State);

            // Skriv ut de senaste 5 meddelandena, nyast först
            List<string> messages = _battleManager.GetMessages();
            int count = Math.Min(messages.Count, Constants.MaxBattleMessages);
            for (int i = count - 1; i >= 0; i--)
            {
                Console.SetCursorPosition(_x + 2, _y + 3 + i);
                Console.Write(messages[messages.Count - 1 - i]);
            }

        }

        public void SetAccentColor(ConsoleColor color)
        {

        }

    }
}