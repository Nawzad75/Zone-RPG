using Dapper;
using MySqlConnector;
using dotenv.net;

namespace ZoneRpg.Database
{
    public class DatabaseManager
    {
        MySqlConnection _connection;

        public DatabaseManager()
        {
            // Läs in dovenv filen med databas connection string
            DotEnv.Load();
            IDictionary<string, string> envVars = DotEnv.Read();

            // anslut till databasen
            _connection = new MySqlConnection(envVars["DB_CONNECTION_STRING"]);

            // Om det blir ett mysql-fel, skrivas det inte ut i konsolen, 
            // så vi skriver ut det själva och throwar igen (så )
            try
            {
                _connection.Open();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw e;
            }
        }
    }
}