using ZoneRpg.GameLogic;

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
            foreach (string message in _battleManager.GetMessages()){
                Console.SetCursorPosition(_x + 2, _y + 1);
                Console.Write(message);
            }

        }

        public void SetAccentColor(ConsoleColor color)
        {

        }

    }
}