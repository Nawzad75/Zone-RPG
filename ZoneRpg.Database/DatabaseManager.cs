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
        // Gets a zone from the database
        //
        public Zone GetZone(int zoneId)
        {
            var parameters = new { id = zoneId };
            string sql = "SELECT * FROM zone WHERE id = @id";
            return _connection.Query<Zone>(sql, parameters).First();
        }

        //
        // Gets all entities from the database
        //
        public List<Entity> GetEntities(int zoneId = 1)
        {
            string sql = @"
                SELECT * FROM entity
                INNER JOIN entity_type ON entity_type.id = entity.entity_type_id
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
        public List<Player> GetPlayers()
        {
            string sql = @"
                SELECT 
                    c.*,
                    e.*,
                    cc.id,
                    cc.name,
                    cc.base_attack as BaseAttack,
                    cc.base_attack_per_level as BaseAttackPerLevel,
                    cc.max_hp as MaxHp,
                    cc.max_hp_per_level as MaxHpPerLevel
                FROM `character` c
                INNER JOIN entity e ON e.id = c.entity_id
                INNER JOIN character_class cc ON cc.id = c.character_class_id
                WHERE e.entity_type_id = @EntityTypeId";


            List<Player> players = _connection.Query<Player, Entity, CharacterClass, Player>(
                sql,
                (player, entity, characterClass) =>
                {
                    player.Entity = entity;
                    player.CharacterClass = characterClass;
                    return player;
                },
                new { @EntityTypeId = (int)EntityType.Player }
            ).ToList();


            return players;
        }

        //
        //
        //
        public Monster? GetMonsterByEntityId(int id)
        {
            string sql = @"SELECT 
                c.*,
                mc.id,
                mc.name,
                mc.base_attack as BaseAttack,
                mc.base_attack_per_level as BaseAttackPerLevel,
                mc.max_hp as MaxHp,
                mc.max_hp_per_level as MaxHpPerLevel
                FROM `character` c
                    INNER JOIN monster_class mc ON mc.id = c.character_class_id
                    WHERE entity_id = @id";

            var results = _connection.Query<Monster, MonsterClass, Monster>(
                sql,
            (monster, monsterClass) =>
            {
                monster.MonsterClass = monsterClass;
                return monster;
            },
             new { id });
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
            // Entityn först, så vi får ett entity_id att ge till character
            string entity_sql = @"
                INSERT INTO entity 
                    (entity_type_id, symbol, zone_id, x, y)
                VALUES
                    (@EntityType, @Symbol, @ZoneId, @X, @Y);
                SELECT LAST_INSERT_ID();";
            var enntityParameters = new
            {
                EntityType = character.Entity.EntityType,
                Symbol = character.Entity.Symbol,
                ZoneId = character.Entity.ZoneId,
                X = character.Entity.X,
                Y = character.Entity.Y,
            };

            character.Entity.Id = _connection.Query<int>(entity_sql, character.Entity).First();

            //  Character 
            string sql = @"
                INSERT INTO `character` 
                    (name, hp, is_monster, xp, character_class_id, entity_id)
                VALUES 
                    (@name, @hp, @is_monster, @xp, @character_class_id, @entity_id)";

            // INSERT INTO `character` 

            var parameters = new
            {
                name = character.Name,
                hp = character.Hp,
                max_hp = character.MaxHp,
                xp = character.Xp,
                is_monster = character.IsMonster,
                skill = character.Skill,
                character_class_id = character.CharacterClass.Id,
                entity_id = character.Entity.Id
            };

            _connection.Execute(sql, parameters);

        }
        public void InsertMessage(Message message)
        {
            string sql = @"
            INSERT INTO `message`( character_id , character_name , text) 

            VALUES( @character_id , @character_name , @text )";

            var parameters = new
            {
                character_id = message.character.id,
                character_name = message.character.Name,
                text = message.Text,
            };

            _connection.Execute(sql, parameters);
        }


        public List<Message> GetMessages()
        {
            string sql = @"
                SELECT * FROM `message` m 
                INNER JOIN `character` c ON c.name = m.character_name";

            List<Message> messages = _connection.Query<Message, Character, Message>(sql, (message, character) =>
            {
                message.character = character;
                return message;
            }).ToList();

            return messages;
        }


    }
}