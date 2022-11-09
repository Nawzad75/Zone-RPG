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
        // Connect and store away the connection for later use
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
        // Get's all "ItemInfo's" from the database
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

        //
        // Gets a zone from the database
        //
        public Zone GetZone()
        {
            string sql = "SELECT * FROM zone";
            Zone zone = _connection.Query<Zone>(sql).First();
            return zone;
        }

        //
        // Gets all entities from the database
        //
        public List<Entity> GetEntities()
        {
            string sql = "SELECT * FROM entity";
            List<Entity> entities = _connection.Query<Entity>(sql).ToList();
            return entities;
        }

        //
        // Gets a player from the database
        //
        public Character GetCharacter()
        {
            string sql = "SELECT * FROM player";
            Character player = _connection.Query<Character>(sql).First();
            return player;
        }

        //
        // Inserts a player into the database
        // 
        public void InsertCharacter(Character character)
        {
            string entity_sql = @"
                INSERT INTO entity 
                    (symbol, zone_id, x, y, hp)
                VALUES
                    (@Symbol, @ZoneId, @X, @Y, @Hp);
                SELECT LAST_INSERT_ID();";

            character.Entity.Id = _connection.Query<int>(entity_sql, character.Entity).First();

            string sql = @"
                INSERT INTO `character` 
                    (name, xp, is_mob, skill_id, characterclass_id, entity_id)
                VALUES 
                    (@name, @xp, @is_mob, @skill, @characterclass_id, @entity_id)";

            var parameters = new
            {
                name = character.Name,
                xp = character.Xp,
                is_mob = character.IsMob,
                skill = character.Skill,
                characterclass_id = (int)character.CharacterClass,
                entity_id = character.Entity.Id
            };

            _connection.Execute(sql, parameters);
            
        }
    }
}