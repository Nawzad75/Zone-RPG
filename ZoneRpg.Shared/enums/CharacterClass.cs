namespace ZoneRpg.Shared
{
    public class CharacterClass
    {
        public int Id { get; set; }
        public string Name { get; set; } = "<Not set>";
        public int BaseAttack { get; set; }
        public int BaseAttackPerLevel;
        public int MaxHp { get; set; }
        public int MaxHpPerLevel { get; set; }

        public CharacterClass() { }
    }


}