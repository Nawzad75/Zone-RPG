namespace ZoneRpg.Shared
{
    public class Monster : Character, IFighter
    {
        public MonsterClass MonsterClass { get; set; } = new MonsterClass();

        public Monster()
        {
            Name = "Monster";
            Hp = 10;
            Level = 1;
        }

        public void SetMonster()
        {
            this.Level = Level;
            Hp = Level * 5;
        }

        //
        // Nedan implementerar vi "IFighter" interfacet.
        // --------------------------------------------
        public int GetAttack()
        {
            return MonsterClass.BaseAttack;// + ItemIdWeapon?.AttackBonus ?? 0;  
        }

        public int GetDefense()
        {
            // return CharacterClass.BaseDefense;// + ItemIdHelm?.DefenseBonus ?? 0;
            return 0;
        }

        public string GetClassReadable()
        {
            return MonsterClass.Name;
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
            return MonsterClass.MaxHp;
        }
    }
}