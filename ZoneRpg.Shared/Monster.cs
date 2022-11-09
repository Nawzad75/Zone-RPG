namespace ZoneRpg.Shared
{
    public class Monster 
    {
        public string Name { get; set; }

        public int Attack { get; set; }
        public int Health { get; set; }
        public int Level { get; set; }
        public int Loot { get; set; }
        public Entity Entity { get; set; } = new Entity();
        public int Xp { get; set; }
        public bool IsMob { get; set; }
        public int Skill { get; set; }
        public int CharacterClass { get; set; }


        
        public Monster()
        {
            Name = "Monster";
            Attack = 0;
            Health = 0;
            Level = 1;
        }

        public void SetMonster()
        {
            this.Level = Level;
            Attack = Level * 2;
            Health = Level * 5;
        }
    }
}