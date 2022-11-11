namespace ZoneRpg.Shared
{
    public class Character : IFighter
    {
        public int id { get; set; }
        public string Name { get; set; } = "Unnamed player";
        public int Type { get; set; }
        public int Xp { get; set; }
        public int Level { get; set; }
        public int Skill { get; set; }
        public CharacterClass CharacterClass { get; set; }
        public int Attack { get; set; }
        public int Hp { get; set; }
        public bool Is_Monster { get; set; } = false;
        public Entity Entity { get; set; } = new Entity();
        public Item? ItemIdWeapon { get; set; }
        public Item? ItemIdBoots { get; set; }
        public Item? ItemIdHelm { get; set; }

        public Character() { }


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
                    MoveDown(zone.Height - 2);
                    break;

                case ConsoleKey.LeftArrow:
                    MoveLeft(1);
                    break;

                case ConsoleKey.RightArrow:
                    MoveRight(zone.Width - 2);
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

        public int GetAttack()
        {
            return Attack;
        }

        public void TakeDamage(int damage)
        {
            Hp -= damage;
        }

        public string GetName()
        {
            return Name;
        }

        public int GetHp()
        {
            return Hp;
        }


    }
}