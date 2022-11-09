namespace ZoneRpg.Shared
{
    public class Monster : Character
    {
        
        public Monster()
        {
            Name = "Monster";
            Attack = 0;
            Hp = 0;
            Level = 1;
        }

        public void SetMonster()
        {
            this.Level = Level;
            Attack = Level * 2;
            Hp = Level * 5;
        }
    }
}