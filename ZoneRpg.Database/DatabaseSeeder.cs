using MySqlConnector;
using Dapper;

namespace ZoneRpg.Database
{
    internal class DatabaseSeeder
    {
        private MySqlConnection _connection;

        public DatabaseSeeder(MySqlConnection connection)
        {
            _connection = connection;
        }


        //
        // Recreates the item_type table and inserts data
        //
        public void SeedMonster()
        {

            _connection.Execute(@"Create table monster_info (
                id int not null auto_increment,
                name VARCHAR(255) not null,
                type VARCHAR(255) not null,
                PRIMARY KEY (id)");

                // _connection.Execute(@"INSERT")

           
        }
        public void SeedItemType()
        {
            // Ta bort allt!
            _connection.Execute("DROP TABLE IF EXISTS item_type");
            // Skapa om tabellen
            _connection.Execute(@"CREATE TABLE item_type (
                id INT NOT NULL AUTO_INCREMENT, 
                name VARCHAR(30), 
                PRIMARY KEY (id)
            )");
            // Lägg till data
            _connection.Execute("INSERT INTO item_type (name) VALUES ('Weapon')");
            _connection.Execute("INSERT INTO item_type (name) VALUES ('Boots')");
            _connection.Execute("INSERT INTO item_type (name) VALUES ('Helmet')");
        }

        //
        // Recreates the item_info table and inserts data
        //
        public void SeedItemInfo()
        {
            _connection.Execute("DROP TABLE IF EXISTS item_info");
            _connection.Execute(@"CREATE TABLE item_info (
                id INT NOT NULL AUTO_INCREMENT, 
                name VARCHAR(30), 
                description VARCHAR(100), 
                rarity INT, 
                item_type_id INT,
                PRIMARY KEY (id)
            )");

            string[] values = new String[] {
                @"('Sword', 'A sword', 1, 1)",
                @"('Axe', 'An axe', 1, 1)",
                @"('Boots of Speed', 'Boots that makes you run faster', 2, 2)",
                @"('Boots of Jumping', 'Boots that makes you jump higher', 2, 2)",
                @"('Helmet of Strength', 'Helmet that makes you stronger', 2, 3)",
                @"('Helmet of Wisdom', 'Helmet that makes you wiser', 2, 3)",
            };

            string columns = "(name, description, rarity, item_type_id)";
            foreach (string value in values)
            {
                _connection.Execute($"INSERT INTO item_info {columns} VALUES {value}");
            }
        }
    }
}