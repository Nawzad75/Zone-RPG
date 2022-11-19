namespace ZoneRpg.Shared
{
    public class Entity
    {
        public int Id { get; set; }
        public string Symbol { get; set; } = "❓"; // Placeholder symbol, (should be changed)
        public int ZoneId { get; set; } = 1;
        public int X { get; set; }
        public int Y { get; set; }
        public EntityType EntityType { get; set; }
        
        // För dapper vill ha en int istället för en enum: 
        public int EntityTypeId { get { return (int)EntityType; } }
        
        // Konstruktorer
        public Entity() { }
        public Entity(int x, int y, string symbol)
        {
            X = x;
            Y = y;
            Symbol = symbol;
        }
    }
}