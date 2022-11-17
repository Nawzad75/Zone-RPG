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

            if (_battleManager.State == BattleState.Lost)
            {
                Console.SetCursorPosition(_x + 2, _y + 1);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("You died!");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("\t\t\t Press <Enter> to respawn...");
                return;
            }
            else if (_battleManager.State == BattleState.Won)
            {
                Console.SetCursorPosition(_x + 2, _y + 2);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("You won!");
                return;
            }

            Console.SetCursorPosition(_x + 2, _y);
            Console.Write(" Battle ");

            // Skriv ut de senaste 5 meddelandena, nyast först
            List<string> messages = _battleManager.GetMessages();
            int count = Math.Min(messages.Count, Constants.MaxBattleMessages);
            for (int i = count - 1; i >= 0; i--)
            {
                string message = messages[messages.Count - 1 - i];
                IFighter player = _battleManager.Player!;
                IFighter monster = _battleManager.Monster!;

                // De konstiga ascii escape tecknen här, är för att kunna ändra färg på texten
                message = message
                    .Replace("[player]", $"\u001b[36;1m{player.Name}\u001b[0m")
                    .Replace("[monster]", $"\u001b[31;1m{monster.Name}\u001b[0m")
                    .Replace("[player_attack]", $"\u001b[33;1m{player.GetAttack()}\u001b[0m")
                    .Replace("[monster_attack]", $"\u001b[33;1m{monster.GetAttack()}\u001b[0m");

                Console.SetCursorPosition(_x + 2, _y + 1 + i);
                Console.Write(message);
            }
        }

        public void SetAccentColor(ConsoleColor color)
        {

        }

    }
}