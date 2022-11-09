using ZoneRpg.Database;
using ZoneRpg.Shared;

namespace ZoneRpg.Ui;
public class FightManager
{
    IFighter _a;
    IFighter _b;

    public FightManager(IFighter a, IFighter b)
    {
        _a = a;
        _b = b;        
    }

    public string Fight(){
        _a.TakeDamage(_b.GetAttack());
        _b.TakeDamage(_b.GetAttack());
        return $"{_a.GetName()}: {_a.GetHp()}\n {_b.GetName()}: {_b.GetHp()}";
    }

}

