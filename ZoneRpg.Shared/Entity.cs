namespace ZoneRpg.Shared
{
    class Entity
    {
        
        public int X { get; set; }
        public int Y { get; set; }
        public string CharSymbol { get; set; }

        public int Hp { get; set; }

        public Entity(int x, int y, string charSymbol, int hp)
        {
            X = x;
            Y = y;
            CharSymbol = charSymbol;
            Hp = hp;
        }




    }


}