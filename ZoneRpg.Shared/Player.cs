namespace ZoneRpg.Shared
{
    public class Player : Character
    {


        
        //constructor
        public Player(string name, CharacterClass characterClass) : base()
        {
            Name = name;
            CharacterClass = characterClass;
            Entity.X = 22;
            Entity.Y = 6;
            Entity.Symbol = 'P';
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
    }
}