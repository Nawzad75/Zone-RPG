public class Player
{
    public string name { get; set; }
    public int xp { get; set; }
    public int level { get; set; }
    public int skill { get; set; }
    public string CharacterClass { get; set; }
    public int attack { get; set; }
    public int health { get; set; }


    Player player = new Player();

    public Player()
    {

        name = "Player";
        xp = 0;
        level = 1;
        skill = 0;
        CharacterClass = "Warrior";
        attack = 0;
        health = 0;
        SetPlayer();

    }
    // här sätter jag spelarens stats (attack och health) beroende på vilken klass spelaren har valt
    public void SetPlayer()
    {
        level = level;
        skill = skill;
        CharacterClass = CharacterClass;
        attack = level * 2;
        health = level * 5;
    }
    public void CharacterClassSet()
    {
        if (CharacterClass == "Warrior")
        {
            attack = 5;
            health = 10;
        }
        else if (CharacterClass == "Hitter")
        {
            attack = 3;
            health = 5;
        }
    }

    //funktion för att levela upp spelaren.
    public void LevelUp()
    {
        level++;
        xp = 0;
        SetPlayer();
    }

    //Funktion så vi lägger till xp till spelaren, lägger in int xp som parameter för att kunna använda den i andra funktioner
    public void AddXp(int xp)
    {
        this.xp += xp;
        if (this.xp >= 100)
        {
            LevelUp();
        }
    }

    //När vi dödar en monster så ska vi få xp och levela upp. Samt få loot.
    public void KillMonster(Monster monster)
    {
        AddXp(monster.loot);
    }
}






