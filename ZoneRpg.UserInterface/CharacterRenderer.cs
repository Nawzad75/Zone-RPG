using ZoneRpg.Shared;

namespace ZoneRpg.UserInterface
{
    internal class CharacterRenderer : ICharacterRenderer
    {
        private int _x;
        private int _y;
        private ConsoleColor _accentColor;

        public CharacterRenderer(int x = 0, int y = 0, ConsoleColor accentColor = ConsoleColor.White)
        {
            SetDrawOrigin(x, y);
            SetAccentColor(accentColor);
        }

        public void SetAccentColor(ConsoleColor color)
        {
            _accentColor = color;
        }

        public void SetDrawOrigin(int x, int y)
        {
            _x = x;
            _y = y;
        }

        public void DrawCharacter(IFighter? character)
        {
            ConsoleUtils.DrawBox(_x, _y, 19, 3);
            Console.SetCursorPosition(_x + 2, _y + 1);

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Name:   ");
            Console.ForegroundColor = _accentColor;
            Console.Write(character?.Name ?? "");

            Console.SetCursorPosition(_x + 2, _y + 2);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("HP:     ");
            Console.ForegroundColor = _accentColor;
            Console.Write(character?.Hp.ToString() ?? "");

            Console.SetCursorPosition(_x + 2, _y + 3);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Attack: ");
            Console.ForegroundColor = _accentColor;
            Console.Write(character?.Attack.ToString() ?? "");

            Console.ResetColor();
        }
    }
}