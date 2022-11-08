namespace ZoneRpg.Game
{
    public class Monster
    {
        public string name { get; set; }

        public int attack { get; set; }
        public int health { get; set; }
        public int level { get; set; }
        public int loot { get; set; }

        Monster monster = new Monster();
        public Monster()
        {
            name = "Monster";
            attack = 0;
            health = 0;
            level = 1;
        }

        public void SetMonster()
        {
            this.level = level;
            attack = level * 2;
            health = level * 5;
        }
    }
}