using ZoneRpg.Shared;

namespace ZoneRpg.UserInterface
{
    internal class CharacterRenderer : BaseRenderer, IRenderer
    {            
        private IFighter? _character;
        private ConsoleColor _accentColor;

        public CharacterRenderer(ConsoleColor accentColor = ConsoleColor.White)
        {
            SetAccentColor(accentColor);
        }

        public void SetCharacter(IFighter? character)
        {
            _character = character;
        }

        public void SetAccentColor(ConsoleColor color)
        {
            _accentColor = color;
        }

        public bool hasCharacter()
        {
            return _character != null;
        }

        public override void Draw()
        {
            if (_character == null)
            {
                base.Draw();
                return;
            }
            base.Draw();
           
            Console.SetCursorPosition(_x + 2, _y);
            Console.ForegroundColor = _accentColor;
            Console.Write($" {_character?.Name ?? ""} ");

            Console.SetCursorPosition(_x + 2, _y + 1);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Class:  ");
            Console.ForegroundColor = _accentColor;
            Console.Write(_character?.GetClassReadable() ?? "");

            Console.SetCursorPosition(_x + 2, _y + 2);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("HP:     ");
            Console.ForegroundColor = _accentColor;
            Console.Write(_character?.Hp.ToString() ?? "");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(" / " + _character?.GetMaxHp().ToString() ?? "");

            Console.SetCursorPosition(_x + 2, _y + 3);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Attack: ");
            Console.ForegroundColor = _accentColor;
            Console.Write(_character?.GetAttack().ToString() ?? "");

            Console.ResetColor();
        }
    }
}