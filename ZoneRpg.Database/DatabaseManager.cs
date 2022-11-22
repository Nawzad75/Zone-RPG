using Dapper;
using MySqlConnector;
using dotenv.net;
using ZoneRpg.Models;
using System.Diagnostics;

namespace ZoneRpg.Database
{
    public class DatabaseManager
    {
        // Databas anslutning
        MySqlConnection _connection;

       
        // Ansluter och sparar bort anslutningen för senare användning
        public DatabaseManager()
        {
            // läser in .env filen 
            DotEnv.Load();
            IDictionary<string, string> envVars = DotEnv.Read();


            // Ansluter till databasen
            _connection = new MySqlConnection(envVars["DB_CONNECTION_STRING"]);

            // Kollar så att vi är anslutna till databasen
            try
            {
                _connection.Open();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }

        }

        // Hämtar alla "ItemInfo's" från databasen
        public List<ItemInfo> GetAllItemInfos()
        {
            string sql = "SELECT * FROM item_info";
            return _connection.Query<ItemInfo>(sql).ToList();
        }

        // Hämtar en zon från databasen
        public Zone GetZone(int zoneId)
        {
            string sql = "SELECT * FROM zone WHERE id = @zoneId";
            return _connection.QuerySingle<Zone>(sql, new { zoneId });
        }

        // Hämtar alla entities från databasen
        public List<Entity> GetEntities(int zoneId = 1)
        {
            string sql = @"
                SELECT * FROM entity
                INNER JOIN entity_type ON entity_type.id = entity.entity_type_id
                WHERE entity.zone_id = @zoneId";

            return _connection.Query<Entity, EntityType, Entity>(
                sql,
                (entity, entity_type) =>
                {
                    entity.EntityType = entity_type;
                    return entity;
                },
                new { zoneId }
            ).ToList();
        }

        // Hämtar karaktärer från databasen
        public List<Player> GetPlayers()
        {
            string sql = @"
                SELECT * FROM `character` c
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

            
            // Hämtar vapen, skor och hjälm från databasen
            foreach (Player player in players)
            {
                player.Weapon = GetItem(player.weapon_id);
                player.Boots = GetItem(player.boots_id);
                player.Helm = GetItem(player.helm_id);
            }


            return players;
        }

        public Item? GetItem(int? itemId)
        {
            if (itemId == null)
            {
                return null;
            }
            string sql = @"
                SELECT * FROM item
                INNER JOIN item_info ON item_info.id = item.item_info_id
                WHERE item.id = @itemId";

            IEnumerable<Item>? results = _connection.Query<Item, ItemInfo, Item>(
                sql,
                (item, itemInfo) =>
                {
                    item.ItemInfo = itemInfo;
                    return item;
                },
                new { itemId }
            );

            // Vi vill ha null om det inte finns något item med det id:t
            // därför använder vi "FirstOrDefault".
            return results.FirstOrDefault();
        }



        // Hämtar ett specifikt monster utifrån dess entity-id
        public Monster? GetMonsterByEntityId(int id)
        {
            string sql = @"
                SELECT * FROM `character` c
                INNER JOIN entity e ON e.id = c.entity_id
                INNER JOIN monster_class mc ON mc.id = c.character_class_id
                WHERE entity_id = @id";

            var results = _connection.Query<Monster, Entity, MonsterClass, Monster>(
                sql,
            (monster, entity, monsterClass) =>
            {
                monster.Entity = entity;
                monster.MonsterClass = monsterClass;
                return monster;
            },
             new { id });
            return (results.Count() > 0) ? results.First() : null;
        }

        // Hämtar alla "character classes"
        public List<CharacterClass> GetClasses()
        {
            string sql = @"SELECT * FROM character_class c";
            return _connection.Query<CharacterClass>(sql).ToList();
        }

        // Uppdaterar en entity's position 
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

        // För in en spelare i databasen
        public void InsertCharacter(Character character)
        {
            character.Entity.Id = InsertEntity(character.Entity);

            //  Karaktär
            string sql = @"
                INSERT INTO `character` 
                    (name, hp, xp, character_class_id, entity_id)
                VALUES 
                    (@Name, @Hp, @Xp, @CharacterClassId, @EntityId)";

            _connection.Execute(sql, character);

        }

        // Adds a new entity
        public int InsertEntity(Entity entity)
        {
            string sql = @"
                INSERT INTO entity 
                    (entity_type_id, symbol, zone_id, x,  y)
                VALUES  
                    (@EntityTypeId, @Symbol, @ZoneId, @X, @Y);
                SELECT LAST_INSERT_ID()";

            return _connection.QuerySingle<int>(sql, entity);
        }

        public void InsertMessage(Message message)
        {
            string sql = @"
                INSERT INTO `message`( character_id , text, datetime) 
                VALUES( @CharacterId , @Text , @Datetime)";

            var parameters = new
            {
                CharacterId = message.Character.Id,
                Text = message.Text,
                Datetime = message.DateTime
            };

            _connection.Execute(sql, parameters);
        }


        public List<Message> GetMessages()
        {
            string sql = @"
                SELECT * FROM `message` m 
                INNER JOIN `character` c ON c.id = m.character_id";

            List<Message> messages = _connection.Query<Message, Character, Message>(sql, (message, character) =>
            {
                message.Character = character;
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

        // Lägg till vapen i databasen
        public int InsertWeapon(Item weapon)
        {
            string sql = @"
                INSERT INTO `item`(`character_id`, `item_info_id`) VALUES (NULL, @ItemInfoId);
                SELECT LAST_INSERT_ID()";
                
            return _connection.QuerySingle<int>(sql, weapon);
        }


        public void UpdatePlayerWeapon(Player player)
        {

            string sql = @"
                UPDATE `character` 
                SET weapon_id = @Weapon 
                WHERE id = @id";

            int? Weapon = null;
            if (player.Weapon != null)
            {
                Weapon = player.Weapon.Id;

            }
            var parameters = new
            {
                Weapon,
                id = player.Id
            };

            _connection.Execute(sql, parameters);
        }

        public void InsertMonster(Monster monster)
        {
            monster.Entity.Id = InsertEntity(monster.Entity);

            
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

        public void DeleteMonstersInZone(int zoneId)
        {
            List<Monster> monsters = GetMonsters(zoneId);
            foreach (Monster monster in monsters)
            {
                DeleteCharacter(monster);
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


        // Uppdatera karaktärens liv
        public void UpdateCharacterHp(Character character)
        {
            string sql = @"UPDATE `character` SET hp = @Hp WHERE id = @id";

            var parameters = new
            {
                Hp = character.Hp,
                id = character.Id
            };

            _connection.Execute(sql, parameters);
        }

        // Tar bort en karaktär (och entity!) från databasen
        public void DeleteCharacter(Character? character)
        {
            if (character == null)
            {
                return;
            }
            if (character.Entity.Id == 0)
            {
                Console.WriteLine("Character entity 0!!");
                Console.ReadKey();
                return;
            }


            DeleteEntity(character.Entity.Id);
            string sql = "DELETE FROM `character` WHERE id = @id";
            _connection.Execute(sql, new { id = character.Id });

        }
    }
}