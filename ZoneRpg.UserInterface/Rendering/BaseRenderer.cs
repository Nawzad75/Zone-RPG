namespace ZoneRpg.UserInterface
{
    internal class BaseRenderer
    {
        protected int _x = 0;
        protected int _y = 0;
        protected int _width = 19;
        protected int _height = 3;

        public void SetRect(int x, int y, int width, int height)
        {
            _x = x;
            _y = y;
            _width = width;
            _height = height;
        }

        public virtual void Draw()
        {
            ConsoleUtils.DrawBox(_x, _y, _width, _height);
            Console.SetCursorPosition(_x + 2, _y + 1);
        }
    }
}