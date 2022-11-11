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
            /* seeder.SeedMonster();
            seeder.SeedItemType();
            seeder.SeedItemInfo(); */
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
        public List<Entity> GetEntities(int zoneId = 1)
        {
            string sql = @"
                SELECT * FROM entity
                INNER JOIN entity_type 
                    ON entity.entity_type_id = entity_type.id
                WHERE entity.zone_id = @zoneId";
            
            List<Entity> entities = _connection.Query<Entity, EntityType, Entity>(sql, (entity, entity_type) =>
            {
                entity.EntityType = entity_type;
                return entity;
            }, new { zoneId }).ToList();

            return entities;
        }

        //
        // Gets a characters from the database
        //
        public List<Character> GetCharacters()
        {
            string sql = @"
                SELECT * FROM `character` c  
                INNER JOIN entity e ON e.id = c.entity_id";

            List<Character> characters = _connection.Query<Character, Entity, Character>(sql, (character, entity) =>
            {
                character.Entity = entity;
                return character;
            }).ToList();

            return characters;
        }

        //
        //
        //
        public Monster? GetMonsterByEntityId(int id)
        {
            string sql = "SELECT * FROM `character` WHERE entity_id = @id";
            var results = _connection.Query<Monster>(sql, new { id });
            return (results.Count() > 0) ? results.First() : null;
        }


        public void UpdateEntityPosition(Entity entity)
        {
            string sql = "UPDATE entity SET x = @x, y = @y WHERE id = @id";
            _connection.Execute(sql, new
            {
                x = entity.X,
                y = entity.Y,
                id = entity.Id
            });
        }

        //
        // Inserts a player into the database
        // 
        public void InsertCharacter(Character character)
        {
            string entity_sql = @"
                INSERT INTO entity 
                    (entity_type_id, symbol, zone_id, x, y, hp)
                VALUES
                    (@EntityType, @Symbol, @ZoneId, @X, @Y, @Hp);
                SELECT LAST_INSERT_ID();";
            var enntityParameters = new
            {
                EntityType = character.Entity.EntityType,
                Symbol = character.Entity.Symbol,
                Zoneid = character.Entity.ZoneId,
                X = character.Entity.X,
                Y = character.Entity.Y,
                HP = character.Entity.Hp

            };

            character.Entity.Id = _connection.Query<int>(entity_sql, character.Entity).First();

            //  ------------------------------------------------------

            string sql = @"
                INSERT INTO `character` 
                    (name, hp, xp, is_monster, skill_id, characterclass_id, entity_id)
                VALUES 
                    (@name, @hp, @xp, @is_monster, @skill, @characterclass_id, @entity_id)";

            var parameters = new
            {
                name = character.Name,
                hp = character.Hp,
                xp = character.Xp,
                is_monster = character.Is_Monster,
                skill = character.Skill,
                characterclass_id = (int)character.CharacterClass,
                entity_id = character.Entity.Id
            };

            _connection.Execute(sql, parameters);

        }

    }
}