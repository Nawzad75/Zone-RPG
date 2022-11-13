namespace ZoneRpg.Shared
{

    //
    // Denna klass skall vara synkad med databas tabellen "character_class"
    //
    public enum CharacterClass
    {
        Warrior = 1,
        Mage = 2,
        Rogue = 3
    }

    public static class CharacterClassExtensions
    {
        public static int GetBaseAttack(this CharacterClass cClass)
        {
            return cClass switch
            {
                CharacterClass.Warrior => 10,
                CharacterClass.Mage => 5,
                CharacterClass.Rogue => 7,
                _ => 0
            };
        }
    }
}