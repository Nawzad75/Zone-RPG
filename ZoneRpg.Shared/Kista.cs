

namespace ZoneRpg.Shared

{

    
    public class Kista
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Type { get; set; }
        public int Symbol { get; set; }
       
        public Entity Entity { get; set; } = new Entity();

        public Kista(){}


    }
}

