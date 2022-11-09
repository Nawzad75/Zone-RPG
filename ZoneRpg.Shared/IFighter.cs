public interface IFighter
{
    string GetName();

    int GetHp();

    int GetAttack();
    
    void TakeDamage(int damage);
}