using ZoneRpg.Shared;

namespace ZoneRpg.Models
{
    public class Player : Character, IFighter
    {
        // Tom konstruktor för att dappar skall hänga med
        public Player() : base() { }

        public Player(string name, CharacterClass characterClass) : base(name)
        {
            // Vi använder första bokstaven i spelarens namn som symbol
            Entity.Symbol = name.Substring(0, 1).ToUpper();
            Entity.X = Constants.StartPositionX;
            Entity.Y = Constants.StartPositionY;
            Entity.EntityType = EntityType.Player;
            CharacterClass = characterClass;
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
            int attack = CharacterClass.BaseAttack;
            if (Weapon != null)
            {
                attack += Weapon.ItemInfo.Attack;
            }
            return attack;
        }

        public int GetDefense()
        {
            int defense = CharacterClass.BaseDefense;
            if (Boots != null)
            {
                defense += Boots.ItemInfo.Defense;
            }
            if (Helm != null)
            {
                defense += Helm.ItemInfo.Defense;
            }
            return defense;
        }

        public string GetClassReadable()
        {
            return CharacterClass.Name;
        }

        public int TakeDamage(int damage)
        {
            int damageTaken = Math.Max(damage - GetDefense(), 0);
            Hp -= damageTaken;
            return damageTaken;            
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

        public void EquipItem(Item item)
        {
            if (item.ItemInfo.ItemType == ItemType.Weapon)
            {
                Weapon = item;
            }
            else if (item.ItemInfo.ItemType == ItemType.Boots)
            {
                Boots = item;
            }
            else if (item.ItemInfo.ItemType == ItemType.Helmet)
            {
                Helm = item;
            }
        }
    }
}