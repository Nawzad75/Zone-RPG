
namespace ZoneRpg.UserInterface
{
    internal interface IRenderer
    {
        void SetRect(int x, int y, int width, int height);

        void SetAccentColor(ConsoleColor color);
        
        void Draw();
    }
}