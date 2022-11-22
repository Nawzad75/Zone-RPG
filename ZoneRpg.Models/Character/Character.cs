using ZoneRpg.Shared;

namespace ZoneRpg.Models
{
    public class Character
    {
        public int Id { get; set; }
        public string Name { get; set; } = "<Unnamed>";
        public int Hp { get; set; }
        public int MaxHp { get; set; }
        public int Xp { get; set; }
        public int Level { get; set; }
        public int Skill { get; set; }
        public CharacterClass CharacterClass { get; set; } = new CharacterClass();
        public Entity Entity { get; set; } = new Entity();

        // Items
        public Item? Weapon { get; set; }
        public Item? Boots { get; set; }
        public Item? Helm { get; set; }

        // Vi sparar item idn här tillfälligt innan vi har hämtat itemen från databasen.
        public int? WeaponId { get; set; }
        public int? HelmId { get; set; }
        public int? BootsId { get; set; }

        // För dapper
        public int CharacterClassId { get { return CharacterClass.Id; } }
        public int EntityId { get { return Entity.Id; } }

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

        // Flyttar en spelare beroende på knapptryck
        // @param zone - Tar emot en zone för att kunna kolla om spelaren kolliderar med väggar
        // @param collsions - Tar emot ett "collisions" objekt för att kunna kolla om spelaren kolliderar med andra entiteter
        public void Move(ConsoleKey key, Zone zone, CollisionMap collisions)
        {
            switch (key)
            {
                case ConsoleKey.UpArrow:
                    if (!collisions.Up)
                    {
                        MoveUp(1);
                    }
                    break;

                case ConsoleKey.DownArrow:
                    if (!collisions.Down)
                    {
                        MoveDown(zone.Height);
                    }
                    break;

                case ConsoleKey.LeftArrow:
                    if (!collisions.Left)
                    {
                        MoveLeft(1);
                    }
                    break;

                case ConsoleKey.RightArrow:
                    if (!collisions.Right)
                    {
                        MoveRight(zone.Width);
                    }
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