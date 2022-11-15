using ZoneRpg.Shared;

public interface IFighter
{
    // Properties    
    int Hp { get; set; }
    string Name { get; set; }
    
    // Methods
    int GetX();
    int GetY();
    int GetAttack();   
    void TakeDamage(int damage);

    // Kan vara character class, kan vara Monster class, 
    // det fina med ett interface är att vi inte bryr oss just nu
    string GetClassReadable(); 

}