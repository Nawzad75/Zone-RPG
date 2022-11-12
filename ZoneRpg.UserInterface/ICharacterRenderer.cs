using ZoneRpg.Shared;

namespace ZoneRpg.UserInterface
{
    internal interface ICharacterRenderer
    {
        void SetDrawOrigin(int x, int y);
        void SetAccentColor(ConsoleColor color);
        void DrawCharacter(IFighter? character);
    }
}