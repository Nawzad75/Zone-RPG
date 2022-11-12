using ZoneRpg.Database;
using ZoneRpg.Shared;

namespace ZoneRpg.GameLogic
{


    public class Game
    {
        public Zone Zone { get; private set; }
        public Player Player { get; set; } = new Player(); // Placeholder until we get a real player
        public BattleManager BattleManager { get; set; }
        public GameState state { get; set; } = GameState.MainMenu;
        DatabaseManager _db;

        public Game(DatabaseManager db)
        {
            _db = db;
            BattleManager = new BattleManager(_db);
            Zone = _db.GetZone(1);
        }


        public void Update()
        {
            Zone.Entities = _db.GetEntities();
        }
    }
}