using ZoneRpg.Database;
using ZoneRpg.Shared;

namespace ZoneRpg.GameLogic
{


    public class Game
    {
        public Zone Zone { get; private set; }
        public BattleManager BattleManager { get; set; }
        public GameState State { get; private set; } = GameState.MainMenu;
        private Player _player { get; set; } = new Player(); // Placeholder until we get a real player
        DatabaseManager _db;
        public MessageBox MessageBox { get; set; } = new MessageBox("ZoneRpg");

        public Game(DatabaseManager db)
        {
            _db = db;
            BattleManager = new BattleManager(_db);
            Zone = _db.GetZone(1);
        }


        public void Update()
        {
            Zone.Entities = _db.GetEntities();
            Zone.Messages = _db.GetMessages();
            BattleManager.LookForMonsters(Zone.Entities);
            BattleManager.ProgressBattle();
            
        }

        public void SetState(GameState state)
        {
            this.State = state;
        }

        public void MovePlayer(ConsoleKey key)
        {
            _player.Move(key, Zone);
            _db.UpdateEntityPosition(_player.Entity);
        }

        public void RespawnPlayer()
        {
            _player.Entity.X = Constants.StartPositionX;
            _player.Entity.Y = Constants.StartPositionY;
            _db.UpdateEntityPosition(_player.Entity);
            SetState(GameState.Playing);
        }

        public void SetPlayer(Player player)
        {
            _player = player;
        }

        public Player GetPlayer()
        {
            return _player;
        }

        public Entity GetPlayerEntity()
        {
            return _player.Entity;
        }
    }
}