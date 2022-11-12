using ZoneRpg.Shared;

public interface IFighter
{
    // Properties    
    int Hp { get; set; }
    string Name { get; set; }
    int Attack { get; set; }
    int GetX();
    int GetY();

    // Methods
    void TakeDamage(int damage);
}