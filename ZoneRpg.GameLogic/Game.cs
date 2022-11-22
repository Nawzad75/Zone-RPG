using ZoneRpg.Database;
using ZoneRpg.Loot;
using ZoneRpg.Models;
using ZoneRpg.Shared;

namespace ZoneRpg.GameLogic
{


    public class Game
    {
        DatabaseManager _db;
        public Zone Zone { get; private set; }
        public GameState State { get; private set; } = GameState.MainMenu;
        public BattleManager BattleManager { get; private set; }
        public Player Player { get; private set; } = new Player();
        private LootGenerator _lootGenerator;
        public List<Item> CurrentLoot = new List<Item>();

        // Hanterar meddelanden om vad som händer i spelet. Chat och loot.
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
            ChatBox.Messages = _db.GetAllMessages();
            OpenChest();
            BattleManager.LookForMonsters(Zone.Entities);
            BattleManager.ProgressBattle();
            PropagateBattleState();
            _db.UpdateCharacterHp(Player);
            if (BattleManager.Monster != null)
            {
                _db.UpdateCharacterHp((Character)BattleManager.Monster);
            }

        }

        private void PropagateBattleState()
        {
            // Propagera "BattleState" --> "GameState"
            if (BattleManager.State == BattleState.InBattle)
            {
                SetState(GameState.Battle);
            }
            else if (BattleManager.State == BattleState.Lost)
            {
                SetState(GameState.Dead);
            }
            else if (BattleManager.State == BattleState.Won)
            {
                _db.DeleteCharacter((Character)BattleManager.Monster!);
                CurrentLoot = _lootGenerator.GenerateLoot();
                SetState(GameState.Loot);
                BattleManager.Reset();
            }
        }

        public void SetState(GameState state)
        {
            State = state;
        }

        // Player setter
        public void SetPlayer(Player player)
        {
            Player = player;
            BattleManager.Player = Player;
        }

        // Flyttar en spelare
        public void MovePlayer(ConsoleKey key)
        {
            Player.Move(key, Zone, CheckCollisions());
            _db.UpdateEntityPosition(Player.Entity);
        }

        // Respawnar en spelare, nollställer states
        public void RespawnPlayer()
        {
            Player.Entity.X = Constants.StartPositionX;
            Player.Entity.Y = Constants.StartPositionY;
            Player.Hp = Player.CharacterClass.MaxHp;
            _db.UpdateCharacterHp(Player);
            _db.UpdateEntityPosition(Player.Entity);
            BattleManager.Reset();
            SetState(GameState.Zone);
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
                List<ItemInfo> allItemInfos = _db.GetAllItemInfos();
                ItemInfo? sword = allItemInfos.Find(item => item.Name == "Sword");

                if (sword != null)
                {
                    Item item = new Item();
                    item.ItemInfo = sword;
                    Player.Weapon = item;
                    Player.Weapon.Id = _db.InsertWeapon(Player.Weapon);
                    _db.UpdatePlayerWeapon(Player);
                    Message message = new Message("Du har hittat " + item.ItemInfo.Name, Player, ConsoleColor.Yellow);
                    ChatBox.LootMessages.Add(message);
                }
            }
        }

        // Kollar om spelaren kan kollidera med något
        public Collisions CheckCollisions()
        {
            Collisions collisions = new();
            foreach (var entity in Zone.Entities)
            {
                if (  entity.EntityType == EntityType.Player 
                   || entity.EntityType == EntityType.Monster 
                   || entity.EntityType == EntityType.Stone)
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