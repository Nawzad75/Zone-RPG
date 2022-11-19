namespace ZoneRpg.Shared
{
    public class Character
    {
        public int Id { get; set; }
        public string Name { get; set; } = "<Unnamed>";
        public int Hp { get; set; } = 100;
        public int MaxHp { get; set; } = 100;
        public int Xp { get; set; }
        public int Level { get; set; }
        public int Skill { get; set; }
        public CharacterClass CharacterClass { get; set; } = new CharacterClass();
        public Entity Entity { get; set; } = new Entity();

        // För dapper
        public int EntityId { get { return Entity.Id; } }
       

        // Items
        public Item? Weapon { get; set; }
        public Item? Boots { get; set; }
        public Item? Helm { get; set; }

        public Character(){}
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



    }
}