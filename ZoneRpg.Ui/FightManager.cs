using ZoneRpg.Database;
using ZoneRpg.Shared;

namespace ZoneRpg.Ui;
public class FightManager
{
    Character _a;
    Character _b;

    public FightManager(Character a, Character b)
    {
        _a = a;
        _b = b;
    }
}