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
            Console.ForegroundColor = _accentColor;
            ConsoleUtils.DrawBox(_x,_y, 20, 10);
            

            Console.SetCursorPosition(_x + 2, _y + 3);
            Console.Write("Name: " + character?.Name ?? "");
            
            Console.SetCursorPosition(_x + 2, _y + 4);
            Console.Write("Hp: " + character?.Hp ?? "");

            Console.SetCursorPosition(_x + 2, _y + 5);
            Console.Write("Attack: " + character?.Attack ?? "");


            Console.ResetColor();           
        }


    }
}