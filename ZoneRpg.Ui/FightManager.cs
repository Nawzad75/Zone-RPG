using ZoneRpg.Database;
using ZoneRpg.Shared;

namespace ZoneRpg.Ui;
public class FightManager
{
    IFighter _a;
    IFighter? _b;
    DatabaseManager _db;
    int _turn = 0;

    public FightManager(DatabaseManager db, IFighter a)
    {
        _db = db;
        _a = a;
    }


    public FightResult Fight(){
        if (_b == null)
        {
            return new FightResult(FightState.NoFight);
        }
        
        _turn++;

        if (_turn == 1 ) {
            return new FightResult(FightState.FightStarted);
        }  

        FightResult result = new FightResult(FightState.Fighting);        
        _a.TakeDamage(_b.GetAttack());
        result.AddMessage($"{_b.GetName()} attacks {_a.GetName()} for {_b.GetAttack()} damage!");
        
        // A died (player)
        if (_a.GetHp() <= 0)
        {
            result.AddMessage($"{_a.GetName()} has died!");
            result.SetState(FightState.Lost);
            return result;
        }

        _b.TakeDamage(_b.GetAttack());
        result.AddMessage($"{_a.GetName()} attacks {_b.GetName()} for {_a.GetAttack()} damage!");
        
        // B died (monster)
        if (_b.GetHp() <= 0)
        {
            result.AddMessage($"{_b.GetName()} has died!");
            result.SetState(FightState.Won);
            return result;
        }

        return result;
    }

    private void HandleFight(Zone zone)
    {
    }



}

