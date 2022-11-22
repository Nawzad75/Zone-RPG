using ZoneRpg.Database;
using ZoneRpg.Loot;
using ZoneRpg.Models;
using ZoneRpg.Shared;


namespace ZoneRpg.GameLogic
{
    public class Game
    {
        DatabaseManager _db;
        public Zone Zone { get; private set; } = new Zone();
        public GameState State { get; private set; } = GameState.MainMenu;
        public BattleManager BattleManager { get; private set; }
        public Player Player { get; private set; } = new Player();
        private LootGenerator _lootGenerator;
        public List<Item> CurrentLoot = new List<Item>();

        // Gamestate setter. Så att ui:t kan driva spelet.
        public void SetState(GameState state)
        {
            State = state;
        }

        // Player setter
        public void SetPlayer(Player player)
        {
            Player = player;
            BattleManager.Player = player;
            SetZone(player.Entity.ZoneId);
        }

        private void SetZone(int targetZoneId)
        {
            Zone = _db.GetZone(targetZoneId);
            SetState(GameState.ZoneTransition);
        }

        // Hanterar meddelanden om vad som händer i spelet. Chat och loot.
        public ChatBox ChatBox { get; set; } = new ChatBox("ZoneRpg");

        public Game(DatabaseManager db)
        {
            _db = db;
            BattleManager = new BattleManager(_db);
            _lootGenerator = new LootGenerator(_db);
        }

        // Den stora gameloopen!
        public void Update()
        {
            Zone.Entities = _db.GetZoneEntities(Player.Entity.ZoneId);
            ChatBox.Messages = _db.GetAllMessages();
            FindAndOpenChests();
            FindAndOpenDoors();
            BattleManager.LookForMonsters(Zone.Entities);
            BattleManager.ProgressBattle();
            PropagateBattleState();
            _db.UpdateCharacterHp(Player);
            if (BattleManager.Monster != null)
            {
                _db.UpdateCharacterHp((Character)BattleManager.Monster);
            }
        }

        // Här kommer "BatteState" att påverka "GameState"
        private void PropagateBattleState()
        {
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
                CurrentLoot = _lootGenerator.GenerateLoot();
                SetState(GameState.Loot);
                // Monstret är besegrat, ta bort det från databasen!
                _db.DeleteCharacter((Character)BattleManager.Monster!);
                BattleManager.Reset();
            }
        }

        // "Ber" en spelare att flytta på sig, och uppdaterar databasen med den nya positionen.
        public void MovePlayer(ConsoleKey key)
        {
            CollisionMap collisions = EntityUtils.GetCollisions(Player, Zone.Entities);
            Player.Move(key, Zone, collisions);
            _db.UpdateEntityPosition(Player.Entity);
        }

        // Respawnar en spelare, nollställer states
        public void RespawnPlayer()
        {
            Player.Entity.X = Constants.StartPositionX;
            Player.Entity.Y = Constants.StartPositionY;
            Player.Entity.ZoneId = Constants.StartingZoneId;
            Player.Hp = Player.CharacterClass.MaxHp;
            _db.UpdateCharacterHp(Player);
            _db.UpdateEntityPosition(Player.Entity);
            BattleManager.Reset();
            SetZone(Player.Entity.ZoneId);
            SetState(GameState.Zone);
        }

        // Letar efter chests som spelaren står på och öppnar dem.  (Bug!, Se git issue #48)
        public void FindAndOpenChests()
        {
            Entity? chestEntity = Zone.Entities.Find(entity => entity.EntityType == EntityType.Chest);
            if (chestEntity == null)
            {
                return;
            }

            if (chestEntity.X == Player.Entity.X && chestEntity.Y == Player.Entity.Y)
            {
                OpenSwordChest();
            }
        }

        // Öppnar en kista och ger spelaren en svärd
        private void OpenSwordChest()
        {
            List<ItemInfo> allItemInfos = _db.GetAllItemInfos();
            ItemInfo? sword = allItemInfos.Find(item => item.Name == "Rusty sword");

            if (sword != null)
            {
                Item item = new Item();
                item.ItemInfo = sword;
                item.Id = _db.InsertItem(item);
                Player.EquipItem(item);
                _db.UpdatePlayerEquipment(Player);
                Message message = new Message("Du har hittat " + item.ItemInfo.Name, Player, ConsoleColor.Yellow);
                ChatBox.LootMessages.Add(message);
            }
        }

        // Letar efter dörrar som spelaren står på och använder dem för att byta zon. 
        public void FindAndOpenDoors()
        {
            // Hitta entities av typen "Door" som finns på samma position som spelaren.
            Entity? doorEntity = Zone.Entities.Find(e =>
                (e.X == Player.Entity.X && e.Y == Player.Entity.Y)
                && (e.EntityType == EntityType.Door)
            );

            if (doorEntity == null)
            {
                return;
            }

            Door door = _db.GetDoorByEntity(doorEntity);
            Player.Entity.ZoneId = door.TargetZoneId;
            Player.Entity.X = door.TargetX;
            Player.Entity.Y = door.TargetY;
            _db.UpdateEntityPosition(Player.Entity);
            SetZone(door.TargetZoneId);
        }


    }
}