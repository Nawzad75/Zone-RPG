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

        public Game(DatabaseManager db)
        {
            _db = db;
            Zone = _db.GetZone(1);
            BattleManager = new BattleManager(_db);
        }


        public void Update()
        {
            Zone.Entities = _db.GetEntities();
            BattleManager.LookForMonsters(Zone.Entities);

            if(BattleManager.State == BattleState.InBattle)
            {
                SetState(GameState.Battle);
            }

            BattleManager.ProgressBattle();
        }

        //
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
            SetState(GameState.Zone);
        }

        public void SetPlayer(Player player)
        {
            _player = player;
            BattleManager.SetPlayer(_player);
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