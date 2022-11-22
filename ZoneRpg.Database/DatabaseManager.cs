using Dapper;
using MySqlConnector;
using dotenv.net;
using ZoneRpg.Models;
using System.Diagnostics;

namespace ZoneRpg.Database
{
    public class DatabaseManager
    {
        // Database connection
        MySqlConnection _connection;

        // Connect and store away the connection for later use
        public DatabaseManager()
        {
            // Läs in dovenv filen med databas connection string
            DotEnv.Load();
            IDictionary<string, string> envVars = DotEnv.Read();

            // anslut till databasen
            _connection = new MySqlConnection(envVars["DB_CONNECTION_STRING"]);

            // Se till att mysql-fel skrivs ut i konsolen. 
            try
            {
                _connection.Open();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }

        }

        // Get's all "ItemInfo's" from the database
        public List<ItemInfo> GetAllItemInfos()
        {
            string sql = "SELECT * FROM item_info";
            var results = _connection.Query<ItemInfo>(sql).ToList();

            foreach (var item in results)
            {
                Console.WriteLine(item);
            }

            return results;
        }

        // Gets a zone from the database
        public Zone GetZone(int zoneId)
        {
            string sql = "SELECT * FROM zone WHERE id = @zoneId";
            return _connection.QuerySingle<Zone>(sql, new { zoneId });
        }

        // Gets all entities from the database
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

        // Gets a characters from the database
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

            // hämta vapen, hjälm, stövlar
            foreach (Player player in players)
            {
                player.Weapon = GetItem(player.weapon_id);
                player.Boots = GetItem(player.boots_id);
                player.Helm = GetItem(player.helm_id);
            }


            return players;
        }

        // Hämta ett item från databasen
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

            // Vi VILL ha null om item inte finns, därför använder vi "FirstOrDefault".
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
        public List<CharacterClass> GetAllCharacterClasses()
        {
            string sql = @"SELECT * FROM character_class";
            return _connection.Query<CharacterClass>(sql).ToList();
        }

        // Uppdaterar en entity's position 
        public void UpdateEntityPosition(Entity entity)
        {
            string sql = "UPDATE entity SET x = @X, y = @Y WHERE id = @Id";
            _connection.Execute(sql, entity);
        }

        // Inserts a player into the database
        public void InsertCharacter(Character character)
        {
            // Skapa entity för spelaren först. (Vi behöver entity-id)
            character.Entity.Id = InsertEntity(character.Entity);

            string sql = @"
                INSERT INTO `character` 
                    (name, hp, xp, character_class_id, entity_id)
                VALUES 
                    (@Name, @Hp, @Xp, @CharacterClassId, @EntityId)";

            _connection.Execute(sql, character);
        }

        // Lägger till en entity
        public int InsertEntity(Entity entity)
        {
            string sql = @"
                INSERT INTO entity 
                    (entity_type_id, symbol, zone_id, x,  y)
                VALUES  
                    (@EntityTypeId, @Symbol, @ZoneId, @X, @Y);
                SELECT LAST_INSERT_ID()";

            // (@X, @Y, @Symbol, @ZoneId, @EntityTypeId);
            return _connection.QuerySingle<int>(sql, entity);
        }

        // Lägg till ett "Message" i databasen
        public void InsertMessage(Message message)
        {
            string sql = @"
                INSERT INTO `message`( character_id , text, datetime) 
                VALUES( @CharacterId , @Text , @Datetime)";

            _connection.Execute(sql, message);
        }

        // Hämta alla meddelanden
        public List<Message> GetAllMessages()
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

        // Räkna antalet monster i en zon.
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
        public int InsertItem(Item item)
        {
            string sql = @"
                INSERT INTO `item`(`character_id`, `item_info_id`) VALUES (NULL, @ItemInfoId);
                SELECT LAST_INSERT_ID()";

            return _connection.QuerySingle<int>(sql, item);
        }

        // Uppdatera en spelare's vapen
        public void UpdatePlayerEquipment(Player player)
        {
            Console.WriteLine("Weapon id: " + player.Weapon?.Id);
            Console.WriteLine("Helm id: " + player.Helm?.Id);
            Console.WriteLine("Boots id: " + player.Boots?.Id);

            string sql = @"
                UPDATE `character` SET 
                    weapon_id = @WeaponId, 
                    boots_id = @BootsId, 
                    helm_id = @HelmId 
                WHERE id = @Id";

            // Vi VILL spara null om spelaren t.ex. har förlorat sitt vapen.
            var parameters = new
            {
                WeaponId = player.Weapon?.Id,
                BootsId = player.Boots?.Id,
                HelmId = player.Helm?.Id,
                id = player.Id
            };

            _connection.Execute(sql, parameters);
        }

        // Skapa ett nytt monster + entity
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

        // Hämta alla monster i en zon
        public List<Monster> GetAllMonsters(int zoneId)
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

        // Ta bort alla monster i en zon
        public void DeleteMonstersInZone(int zoneId)
        {
            List<Monster> monsters = GetAllMonsters(zoneId);
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

        // Removes a character (and entity!) from the database
        public void DeleteCharacter(Character? character)
        {
            if (character == null)
            {
                return;
            }
            DeleteEntity(character.Entity.Id);
            string sql = "DELETE FROM `character` WHERE id = @id";
            _connection.Execute(sql, new { id = character.Id });

        }
    }
}