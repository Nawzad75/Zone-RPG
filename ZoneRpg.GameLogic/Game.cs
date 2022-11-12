using ZoneRpg.Database;
using ZoneRpg.Shared;

namespace ZoneRpg.GameLogic
{


    public class Game
    {
        public Zone Zone { get; private set; }
        public Player Player { get; set; } = new Player(); // Placeholder until we get a real player
        public BattleManager BattleManager { get; set; }
        public GameState state { get; set; } = GameState.GetPlayerCharacter;
        DatabaseManager _db;

        public Game(DatabaseManager db)
        {
            _db = db;
            BattleManager = new BattleManager(_db);
            Zone = _db.GetZone(1);
        }

        public void SetZone(int zoneId)
        {
            Zone = _db.GetZone(zoneId);
        }
        public void SetPlayer(Player player)
        {
            Player = player;
        }

        public void Setup()
        {

        }

        public void Update()
        {
            Zone.Entities = _db.GetEntities();
        }
    }
}