using ZoneRpg.Shared;

public interface IFighter
{
    // Properties
    Entity Entity { get; set; }
    int Hp { get; set; }
    string Name { get; set; }
    int Attack { get; set; }

    // Methods
    void TakeDamage(int damage);
}