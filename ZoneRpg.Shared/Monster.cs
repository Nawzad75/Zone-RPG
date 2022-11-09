namespace ZoneRpg.Shared
{
    public class Monster : Character
    {
        
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