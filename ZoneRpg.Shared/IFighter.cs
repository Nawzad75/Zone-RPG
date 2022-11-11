using ZoneRpg.Shared;

public interface IFighter
{
    Entity Entity { get; set; }

    string GetName();

    int GetHp();

    int GetAttack();
    
    void TakeDamage(int damage);
}