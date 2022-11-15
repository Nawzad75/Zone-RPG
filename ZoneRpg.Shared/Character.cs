namespace ZoneRpg.Shared
{
    public class Character : IFighter
    {
        public int id { get; set; }
        public string Name { get; set; } = "<Unnamed>";
        public bool IsMonster { get; set; } = false;
        public int Hp { get; set; } = 100;
        public int MaxHp { get; set; } = 100;
        public int Xp { get; set; }
        public int Level { get; set; }
        public int Skill { get; set; }
        public CharacterClass CharacterClass { get; set; } = new CharacterClass();
        public Entity Entity { get; set; } = new Entity();

        // Items
        public Item? ItemWeapon { get; set; }
        public Item? ItemBoots { get; set; }
        public Item? ItemHelm { get; set; }

        public Character() { }
        public Character(string name)
        {
            Name = name;
        }
        


        //funktion för att levela upp spelaren.
        public void PlayerLevelUp()
        {
            Level++;
            Xp = 0;

        }

        //Funktion så vi lägger till xp till spelaren, lägger in int xp som parameter för att kunna använda den i andra funktioner
        public void PlayerAddXp(int xp)
        {
            this.Xp += xp;
            if (this.Xp >= 100)
            {
                PlayerLevelUp();
            }
        }

        //När vi dödar en monster så ska vi få xp och levela upp. Samt få loot.
        /*   public void PlayerKillMonster(Monster monster)
          {
              PlayerAddXp(monster.Loot);
          } */

        public void Move(ConsoleKey key, Zone zone)
        {
            switch (key)
            {
                case ConsoleKey.UpArrow:
                    MoveUp(1);
                    break;

                case ConsoleKey.DownArrow:
                    MoveDown(zone.Height);
                    break;

                case ConsoleKey.LeftArrow:
                    MoveLeft(1);
                    break;

                case ConsoleKey.RightArrow:
                    MoveRight(zone.Width);
                    break;
            }
        }

        public void MoveUp(int limit)
        {
            if (Entity.Y > limit)
            {
                Entity.Y--;
            }
        }
        public void MoveDown(int limit)
        {
            if (Entity.Y < limit)
            {
                Entity.Y++;
            }
        }
        public void MoveLeft(int limit)
        {
            if (Entity.X > limit)
            {
                Entity.X--;
            }
        }
        public void MoveRight(int limit)
        {
            if (Entity.X < limit)
            {
                Entity.X++;
            }
        }


        //
        // Nedan implementerar vi "IFighter" interfacet.
        // --------------------------------------------
        public int GetAttack()
        {
            return CharacterClass.BaseAttack;// + ItemIdWeapon?.AttackBonus ?? 0;  
        }

        public int GetDefense()
        {
            // return CharacterClass.BaseDefense;// + ItemIdHelm?.DefenseBonus ?? 0;
            return 0;
        }

        public string GetClassReadable()
        {
            return CharacterClass.Name;
        }

        public void TakeDamage(int damage)
        {
            Hp -= damage;
        }

        public int GetX()
        {
            return Entity.X;
        }

        public int GetY()
        {
            return Entity.Y;
        }
    }
}