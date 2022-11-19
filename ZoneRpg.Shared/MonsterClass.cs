namespace ZoneRpg.Shared
{
    public class MonsterClass
    {        
        public int Id { get; set; }
        public string Name { get; set; } = "<Not set>";
        public int BaseAttack { get; set; }
        public int BaseAttackPerLevel;
        public int MaxHp { get; set; }
        public int MaxHpPerLevel { get; set; }

        // Bara för dapper
        public int base_attack { set { BaseAttack = value; } }
        public int base_attack_per_level { set { BaseAttackPerLevel = value; } }
        public int max_hp { set { MaxHp = value; } }
        public int max_hp_per_level { set { MaxHpPerLevel = value; } }

        public MonsterClass() { }
    }
}