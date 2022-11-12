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
                    Entity.Hp = 100;
                    break;

                case CharacterClass.Mage:
                    Entity.Hp = 50;
                    break;

                case CharacterClass.Rogue:
                    Entity.Hp = 75;
                    break;
            }
        }

        public void Respawn()
        {
            Entity.X = Constants.StartPositionX;
            Entity.Y = Constants.StartPositionY;
            Entity.Hp = 100;
        }
    }
}