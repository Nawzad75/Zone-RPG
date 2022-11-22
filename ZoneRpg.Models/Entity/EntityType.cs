namespace ZoneRpg.Models
{
    // Denna klass skall vara synkad med databas-tabellen "entity_type"
    public enum EntityType
    {
        Player = 1,
        Monster = 2,
        Chest = 3,
        Door = 4,
        Stone = 5,
    }
}