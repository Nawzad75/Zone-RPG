namespace ZoneRpg.Models
{

    // Denna klass finns ännu inte i databasen (och används inte än). 
    // Kan någon göra en git issue på detta?
    public class Kista
    {
        public int Id { get; set; }
        public string Name { get; set; } = "Unamed chest";
        public int Type { get; set; }
        public int Symbol { get; set; }
        Zone zone = new Zone();
        public Entity Entity { get; set; } = new Entity();

        public Kista() { }
    }
}

