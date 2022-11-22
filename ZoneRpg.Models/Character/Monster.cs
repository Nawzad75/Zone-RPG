namespace ZoneRpg.Models
{
    public class Monster : Character, IFighter
    {
        public MonsterClass? MonsterClass { get; set; }
        
        // För dapper
        public int MonsterClassId { get { return MonsterClass!.Id; } }
        public Monster() {}

        public Monster(MonsterClass monsterClass, int zoneId)
        {
            Name = "Monster";
            Level = 1;
            MonsterClass = monsterClass;
            Hp = MonsterClass.MaxHp;
            Entity.EntityType = EntityType.Monster; 
            Entity.Symbol = "🐉";
            Entity.ZoneId = zoneId;
        }

        public void SetMonster()
        {
            Level = Level;
            Hp = Level * 5;
        }

        // Nedan implementerar vi "IFighter" interfacet.
        public int GetAttack()
        {
            return MonsterClass!.BaseAttack;
        }

        public int GetDefense()
        {
            return 0;
        }

        public string GetClassReadable()
        {
            return MonsterClass!.Name;
        }

        public void TakeDamage(int damage)
        {
            Hp -= damage;
        }

        public int GetX()
        {
            return Entity.X;
        }

        public int GetY()
        {
            return Entity.Y;
        }

        public int GetMaxHp()
        {
            return MonsterClass!.MaxHp;
        }
    }
}