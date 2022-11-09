namespace ZoneRpg.Shared
{
    public class Character 
    {
        public string Name { get; set; }
        public int Xp { get; set; }
        public int Level { get; set; }
        public int Skill { get; set; }
        public CharacterClass CharacterClass { get; set; }
        public int Attack { get; set; }
        public int Health { get; set; }
        public bool IsMob { get; set; } = true;
        public Entity Entity { get; set; } = new Entity();
        public Item ItemIdWeapon { get; set; }
        public Item ItemIdBoots { get; set; }
        public Item ItemIdHelm { get; set; }

       



        public Character(){}
        
        
        // här sätter jag spelarens stats (attack och health) beroende på vilken klass spelaren har valt

        public void CharacterClassSet()
        {
            // if (CharacterClass == "Warrior")
            // {
            //     Attack = 5;
            //     Health = 10;
            // }
            // else if (CharacterClass == "Hitter")
            // {
            //     Attack = 3;
            //     Health = 5;
            // }
        }

        //funktion för att levela upp spelaren.
        public void LevelUp()
        {
            Level++;
            Xp = 0;
            
        }

        //Funktion så vi lägger till xp till spelaren, lägger in int xp som parameter för att kunna använda den i andra funktioner
        public void AddXp(int xp)
        {
            this.Xp += xp;
            if (this.Xp >= 100)
            {
                LevelUp();
            }
        }

        //När vi dödar en monster så ska vi få xp och levela upp. Samt få loot.
        public void KillMonster(Monster monster)
        {
            AddXp(monster.Loot);
        }
    }
}






