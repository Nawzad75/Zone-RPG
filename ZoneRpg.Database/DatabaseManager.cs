using Dapper;
using MySqlConnector;
using dotenv.net;
using ZoneRpg.Shared;
using System.Diagnostics;

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

        public List<CharacterClass> GetClasses()
        {
            string sql = @"SELECT 
                
                c.id,
                c.name,
                c.base_attack as BaseAttack,
                c.base_attack_per_level as BaseAttackPerLevel,
                c.max_hp as MaxHp,
                c.max_hp_per_level as MaxHpPerLevel
             FROM character_class c";
            List<CharacterClass> characterClasses = _connection.Query<CharacterClass>(sql).ToList();
            return characterClasses;
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
            character.Entity.Id = InsertEntity(character.Entity);

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
                skill = character.Skill,
                character_class_id = character.CharacterClass.Id,
                entity_id = character.Entity.Id
            };

            _connection.Execute(sql, parameters);

        }

        public int InsertEntity(Entity entity)
        {
            string sql = @"
                INSERT INTO entity 
                    (entity_type_id, symbol, zone_id, x,  y)
                VALUES  
                    (@EntityTypeId, @Symbol, @ZoneId, @X, @Y );
                SELECT LAST_INSERT_ID()";

            // (@X, @Y, @Symbol, @ZoneId, @EntityTypeId);
            return _connection.QuerySingle<int>(sql, entity);
        }

        public void InsertMessage(Message message)
        {
            string sql = @"
            INSERT INTO `message`( character_id , character_name , text) 

            VALUES( @character_id , @character_name , @text )";

            var parameters = new
            {
                character_id = message.character.Id,
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

        public int CountMonstersInZone(int zoneId)
        {
            string sql = @"
                SELECT COUNT(*) FROM entity e
                WHERE e.zone_id = @zoneId AND e.entity_type_id = @MonsterType";
        
            return _connection.Query<int>(
                sql,
                new { zoneId, MonsterType = EntityType.Monster }
            ).First();
        }
            
        public void InsertWeaponUpdatePlayer(Player player)
        {
             //Lägg till Item i databasen
            string sql = @"
          INSERT INTO `item`(`character_id`, `item_info_id`) VALUES (NULL, @ItemInfoId)";
            var parameter = new
            {
                ItemInfoId = player.Weapon!.ItemInfo.Id
            };
            _connection.Execute(sql, parameter);

            
        }





        public void InsertMonster(Monster monster)
        {
            monster.Entity.Id = InsertEntity(monster.Entity);

            //  Character 
            string sql = @"
                INSERT INTO `character` 
                    (name, hp, xp, character_class_id, entity_id)
                VALUES 
                    (@Name, @Hp, @Xp, @MonsterClassId, @EntityId)";

            _connection.Execute(sql, monster);
        }

        public List<Monster> GetMonsters(int zoneId)
        {
            string sql = @"
                SELECT * FROM `character` c
                INNER JOIN entity e ON e.id = c.entity_id
                INNER JOIN monster_class mc ON mc.id = c.character_class_id
                WHERE e.entity_type_id = @EntityTypeId AND e.zone_id = @zoneId";

            return _connection.Query<Monster, Entity, MonsterClass, Monster>(
                sql, (monster, entity, monsterClass) =>
                {
                    monster.Entity = entity;
                    monster.MonsterClass = monsterClass;
                    return monster;
                },
                new { @EntityTypeId = (int)EntityType.Monster, zoneId }
            ).ToList();
        }

        public void DeleteMonstersInZone(int zone_id)
        {
            List<Monster> monsters = GetMonsters(zone_id);
            foreach (Monster monster in monsters)
            {
                DeleteEntity(monster.Entity.Id);
                DeleteMonster(monster.Id);
            }
        }

        private void DeleteEntity(int id)
        {
            string sql = @"
                DELETE FROM entity WHERE id = @id;
                SELECT ROW_COUNT();";

            int numDeletedRows = _connection.QuerySingle<int>(sql, new { id });

            Debug.WriteLine("Deleted " + numDeletedRows + " rows");
        }

        private void DeleteMonster(int id)
        {
            string sql = "DELETE FROM `character` WHERE id = @id";
            _connection.Execute(sql, new { id });
        }

        public MonsterClass GetMonsterClassByName(string name)
        {
            string sql = @"
                SELECT * FROM monster_class mc
                WHERE mc.name = @name";
            
            return _connection.QuerySingle<MonsterClass>(sql, new { name });
        }

        public void UpdatePlayerHp(object player)
        {
            throw new NotImplementedException();
        }

        public void UpdateCharacterHp(IFighter player)
        {
            throw new NotImplementedException();
        }
    }
}