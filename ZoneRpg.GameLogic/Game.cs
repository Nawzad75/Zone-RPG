using ZoneRpg.Database;
using ZoneRpg.Loot;
using ZoneRpg.Shared;

namespace ZoneRpg.GameLogic
{


    public class Game
    {
        public Zone Zone { get; private set; }
        public GameState State { get; private set; } = GameState.MainMenu;
        public BattleManager BattleManager { get; set; }
        private LootGenerator _lootGenerator;
        public Player Player { get; set; } = new Player();
        DatabaseManager _db;
        public ChatBox ChatBox { get; set; } = new ChatBox("ZoneRpg");

        public Game(DatabaseManager db)
        {
            _db = db;
            Zone = _db.GetZone(1);
            BattleManager = new BattleManager(_db);
            _lootGenerator = new LootGenerator(_db);
        }

        public void Update()
        {
            Zone.Entities = _db.GetEntities();
            ChatBox.Messages = _db.GetMessages();
            OpenChest();
            BattleManager.LookForMonsters(Zone.Entities);

            // Propagera "BattleState" --> "GameState"
            if (BattleManager.State == BattleState.InBattle)
            {
                SetState(GameState.Battle);
            }
            if (BattleManager.State == BattleState.Lost)
            {
                SetState(GameState.Dead);
            }

            if (BattleManager.State == BattleState.Won)
            {
                var loot = _lootGenerator.GenerateLoot();
                SetState(GameState.Loot);
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
            Collisions collisions = CheckCollisions();

            Player.Move(key, Zone, collisions);
            _db.UpdateEntityPosition(Player.Entity);
        }

        public void RespawnPlayer()
        {
            Player.Entity.X = Constants.StartPositionX;
            Player.Entity.Y = Constants.StartPositionY;
            _db.UpdateEntityPosition(Player.Entity);
            BattleManager.State = BattleState.NotInBattle;
            SetState(GameState.Zone);
        }

        public void SetPlayer(Player player)
        {
            Player = player;
            BattleManager.Player = Player;
        }

        public Player GetPlayer()
        {
            return Player;
        }

        public Entity GetPlayerEntity()
        {
            return Player.Entity;
        }
        public void OpenChest()
        {
            Entity? chestEntity = Zone.Entities.Find(entity => entity.EntityType == EntityType.Chest);
            if (chestEntity == null)
            {
                return;
            }

            Entity playerEntity = GetPlayerEntity();

            if (chestEntity.X == playerEntity.X && chestEntity.Y == playerEntity.Y)
            {
                //öppnar kistan och får ett svärd från databasen
                Console.WriteLine("Du har öppnat en kista och fått ett svärd!");
                List<ItemInfo> allItemInfos = _db.GetAllItemInfos();
                ItemInfo? sword = allItemInfos.Find(item => item.Name == "Sword");

                if (sword != null)
                {
                    Item item = new Item();
                    item.ItemInfo = sword;
                    Player.Weapon = item;
                    Player.Weapon.Id = _db.InsertWeapon(Player.Weapon);
                    _db.UpdatePlayerWeapon(Player);

                }

            }

        }
        public Collisions CheckCollisions()
        {
            Collisions collisions = new();
            foreach (var entity in Zone.Entities)
            {
                if (entity.EntityType == EntityType.Player)
                {
                    if ((Player.GetX() - entity.X) == 1 && (Player!.GetY() - entity.Y) == 0)
                    {
                        collisions.Left = true;
                    }
                    if ((Player!.GetX() - entity.X) == -1 && (Player!.GetY() - entity.Y) == 0)
                    {
                        collisions.Right = true;
                    }
                    if ((Player!.GetY() - entity.Y) == 1 && (Player!.GetX() - entity.X) == 0)
                    {
                        collisions.Up = true;
                    }
                    if ((Player!.GetY() - entity.Y) == -1 && (Player!.GetX() - entity.X) == 0)
                    {
                        collisions.Down = true;
                    }
                }
            }
            return collisions;
        }
    }
}