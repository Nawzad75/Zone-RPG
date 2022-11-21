namespace ZoneRpg.Shared
{
    public class Player : Character, IFighter
    {
        // Tom konstruktor för att dappar skall hänga med
        public Player() : base() { }

        //constructor
        public Player(string name, CharacterClass characterClass) : base(name)
        {
            CharacterClass = characterClass;
            Entity.X = Constants.StartPositionX;
            Entity.Y = Constants.StartPositionY;
            Entity.Symbol = name.Substring(0, 1).ToUpper();
            Entity.EntityType = EntityType.Player;            
        }

        public void Respawn()
        {
            Entity.X = Constants.StartPositionX;
            Entity.Y = Constants.StartPositionY;
            Hp = MaxHp;
        }

        // Implementation av "IFighter" interface.
        // ---------------------------------------
        public int GetAttack()
        {
            if (Weapon == null) 
            {
                return CharacterClass.BaseAttack;
            } 
            else 
            {
                return CharacterClass.BaseAttack + Weapon.ItemInfo.Attack;
            }
        }

        public int GetDefense()
        {
            return 0;
        }

        public string GetClassReadable()
        {
            return CharacterClass.Name;
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
            return CharacterClass.MaxHp;
        }
    }
}