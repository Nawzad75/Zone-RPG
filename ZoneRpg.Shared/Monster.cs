namespace ZoneRpg.Shared
{
    public class Monster : Character
    {
        
        public Monster()
        {
            Name = "Monster";
            Hp = 0;
            Level = 1;
        }

        public void SetMonster()
        {
            this.Level = Level;
            Hp = Level * 5;
        }
    }
}