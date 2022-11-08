namespace ZoneRpg.Shared
{
    public class Entity
    {
        public int X { get; set; }
        public int Y { get; set; }
        public char Symbol { get; set; }
        public int Hp { get; set; }
        public Entity(int x, int y, char symbol, int hp)
        {
            X = x;
            Y = y;
            Symbol = symbol;
            Hp = hp;
        }
    }
}