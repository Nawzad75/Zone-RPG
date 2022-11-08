using Dapper;
using MySqlConnector;
using dotenv.net;
using ZoneRpg.Shared;

namespace ZoneRpg.Database
{
    public class DatabaseManager
    {
        // Database connection
        MySqlConnection _connection;

        //
        // Konstruktor
        //
        public DatabaseManager()
        {
            // Läs in dovenv filen med databas connection string
            DotEnv.Load();
            IDictionary<string, string> envVars = DotEnv.Read();

            // anslut till databasen
            _connection = new MySqlConnection(envVars["DB_CONNECTION_STRING"]);

            // Om det blir ett mysql-fel, skrivs det inte ut i konsolen automatiskt
            try
            {
                _connection.Open();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

        }

        //
        //
        //
        public List<ItemInfo> GetAllItemInfos()
        {
            string sql = "SELECT * FROM item_info";
            List<ItemInfo> itemInfos = _connection.Query<ItemInfo>(sql).ToList();
            return itemInfos;
        }

        //
        // Seedar databasen (och skapar vissa tabller på nytt)
        //
        public void SeedDatabase()
        {
            DatabaseSeeder seeder = new DatabaseSeeder(_connection);
            seeder.SeedItemType();
            seeder.SeedItemInfo();
        }


        //
        // Generisk insert-metod
        //
        public void Insert<T>(T data) where T : IDbTable
        {
            // T.ex. "item_info"
            string table = data.GetTableName();

            // T.ex. "name, item_type_id, rarity, description"
            string columns = string.Join(", ", data.GetColumns());

            // T.ex:  "@name, @item_type_id, @rarity, @description"
            string anonymousColumns = string.Join(", ", data.GetColumns().Select(x => "@" + x));

            string sql = $"INSERT INTO {table} ({columns}) VALUES ({anonymousColumns})";

            _connection.Execute(sql, data.GetValues());
        }
        public Zone GetZone()
        {
            string sql = "SELECT * FROM zone";

            Zone zone = _connection.Query<Zone>(sql).First();
            // zone.Height=12;
            // zone.Width= 45;
            return zone;
        }
        public List<Entity> GetEntities()
        {
            string sql = "SELECT * FROM entity";
            List<Entity> entities = _connection.Query<Entity>(sql).ToList();
            return entities;
        }
        public Player GetPlayer()
        {
            string sql = "SELECT * FROM player";
            Player player = _connection.Query<Player>(sql).First();
            return player;

        }
        public void InsertPlayer(Player player)

        {
            var sql = @"INSERT INTO player (name, xp, is_mob, skill_id, characterclass_id, entity_id)
             VALUES (@name, @xp, @is_mob, @skill, @characterclass_id, @entity_id)";
             

            Console.WriteLine("player.Name: " + player.Name);
            Console.WriteLine("player.isMob: " + player.IsMob);
            Console.WriteLine("player.CharacterClass: " + (int)player.CharacterClass);

            var parameters = new
            {
                name = player.Name,
                xp = player.Xp,
                is_mob = player.IsMob,
                skill = player.Skill,
                characterclass_id = (int)player.CharacterClass,
                entity_id = player.Entity.Id
            };

            Console.WriteLine("parameter.characterclass_id: " + parameters.characterclass_id);

            _connection.Execute(sql, parameters);
        }
    }


}