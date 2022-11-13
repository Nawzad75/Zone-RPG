namespace ZoneRpg.Shared
{
    public class Player : Character
    {
        // Tom konstruktor för att dappar skall hänga med
        public Player() : base() { }

        //constructor
        public Player(string name, CharacterClass characterClass) : base()
        {
            Name = name;
            CharacterClass = characterClass;
            Entity.X = Constants.StartPositionX;
            Entity.Y = Constants.StartPositionY;
            Entity.Symbol = "😱";
            Entity.EntityType = EntityType.Player;
            switch (characterClass)
            {
                case CharacterClass.Warrior:
                    MaxHp = 100;
                    break;

                case CharacterClass.Mage:
                    MaxHp = 50;
                    break;

                case CharacterClass.Rogue:
                    MaxHp = 75;
                    break;
            }
        }

        public bool IsDead()
        {
            return Hp <= 0;
        }

        public void Respawn()
        {
            Entity.X = Constants.StartPositionX;
            Entity.Y = Constants.StartPositionY;
            Hp = MaxHp;
        }
    }
}