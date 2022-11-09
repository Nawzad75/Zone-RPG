using ZoneRpg.Database;
using ZoneRpg.Shared;

namespace ZoneRpg.Ui;
public class FightManager
{
    IFighter _a;
    IFighter _b;
    int _turn = 0;

    public FightManager(IFighter a, IFighter b)
    {
        _a = a;
        _b = b;        
    }

    public string Fight(){
        _turn++;
        _a.TakeDamage(_b.GetAttack());
        _b.TakeDamage(_b.GetAttack());
        string fightStartMessage = _turn == 1 ? "Fight started!" : "";
        return $@"{fightStartMessage}
            {_a.GetName()} attacks {_b.GetName()} for {_a.GetAttack()} damage! 
            {_b.GetName()} attacks {_a.GetName()} for {_b.GetAttack()} damage!";
    }

}

