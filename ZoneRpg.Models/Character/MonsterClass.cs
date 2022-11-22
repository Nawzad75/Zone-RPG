namespace ZoneRpg.Models
{
    public class MonsterClass
    {        
        public int Id { get; set; }
        public string Name { get; set; } = "<Not set>";
        public int BaseAttack { get; set; }
        public int BaseAttackPerLevel;
        public int MaxHp { get; set; }
        public int MaxHpPerLevel { get; set; }

        public MonsterClass() { }
    }
}