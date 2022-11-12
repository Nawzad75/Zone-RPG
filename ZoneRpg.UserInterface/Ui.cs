﻿using ZoneRpg.Database;
using ZoneRpg.Shared;

namespace ZoneRpg.UserInterface
{
    public class Ui
    {
        // Ui Members
        private DatabaseManager _db;
        private InputState _inputState;
        private BattleManager _battleManager;
        private IZoneRenderer _zoneRenderer = new ZoneRendererAscii();
        private ICharacterRenderer _playerRenderer = new CharacterRenderer();
        private ICharacterRenderer _monsterRenderer = new CharacterRenderer();
        private Zone _zone = new Zone();
        private Player _player = new Player(); 

        // Constructor
        public Ui(DatabaseManager db)
        {
            _db = db;
            _battleManager = new BattleManager(_db);
        }

        //
        // Run!
        //
        public void Run()
        {
            // Prepare the console
            Console.Clear();
            Console.CursorVisible = false;

            // Show the intro screen and start menu
            new StartGame().RunMainMenu();

            // Get the starting zone for the player
            _zone = _db.GetZone();
            _player = CreateOrChoosePlayer();

            _playerRenderer.SetDrawOrigin(0, _zone.Height + 2);
            _monsterRenderer.SetDrawOrigin(20, _zone.Height + 2);


            while (true)
            {
                _zone.Entities = _db.GetEntities();
                _zoneRenderer.DrawZone(_zone);
                _zoneRenderer.DrawEntities(_zone.Entities);
                _zoneRenderer.DrawPlayerEntity(_zone.Player.Entity);
                _zoneRenderer.DrawBattle(_battleManager.Status);

                _playerRenderer.DrawCharacter(_player);
                _monsterRenderer.DrawCharacter(_battleManager.GetMonster());

                ReadInput();
                OpenChest();

                _battleManager.LookForMonsters(_zone.Entities);
                _battleManager.ProgressBattle();
                HandlePlayerDeath();
            }
        }

        private void HandlePlayerDeath()
        {
            if (_player != null && _player.IsDead())
            {
                Console.Clear();
                Console.WriteLine("You died!");
                Console.WriteLine("Press any key to exit...");
                _inputState = InputState.Dead;
            }
        }


        //
        // Let the player choose a character or create a new one
        //
        private Player CreateOrChoosePlayer()
        {
            string prompt = "Create or choose a character!";
            CreateChooseMenu createOption = new Menu<CreateChooseMenu>(prompt).Run();

            if (createOption == CreateChooseMenu.Create)
            {
                Player player = CreatePlayer();
                _db.InsertCharacter(player);
                return player;
            }
            return ChoosePlayer();
        }


        //
        // Create a player
        //
        public Player CreatePlayer()
        {
            Console.Clear();
            Console.WriteLine("Enter Character Name");
            string name = Console.ReadLine()!; //här skickar vi in namnet som spelaren skriver in
            CharacterClass characterClass = new Menu<CharacterClass>("Choose class").Run();

            return new Player(name, characterClass);
        }

        //
        // Choose player
        //
        public Player ChoosePlayer()
        {
            Console.Clear();
            List<Player> players = _db.GetPlayers();
            string[] options = players.Select(c => $"{c.Name}  (id: {c.id})").ToArray();
            int index = new Menu<int>("Choose a character:", options).Run();

            return players[index];
        }

        //
        // Create monster
        //
        public Monster CreateMonster()
        {
            Monster monster = new Monster();
            monster.Entity.Symbol = "🐉";
            monster.Name = "Monster";
            return monster;
        }

        public Kista CreateChest()
        {
            Kista kista = new Kista();
            kista.Entity.Symbol = "K";
            kista.Name = "Kista";
            return kista;
        }


        public void OpenChest()
        {
            Entity? chestEntity = _zone.Entities.Find(entity => entity.EntityType == EntityType.Chest);
            if (chestEntity == null)
            {
                return;
            }

            if (chestEntity.X == _zone.Player.Entity.X && chestEntity.Y == _zone.Player.Entity.Y)
            {
                //öppnar kistan och får ett svärd från databasen
                Console.WriteLine("Du har öppnat en kista och fått ett svärd!");
                List<ItemInfo> allItemInfos = _db.GetAllItemInfos();
                ItemInfo? sword = allItemInfos.Find(item => item.Name == "Sword");

                if (sword != null)
                {
                    // zone.Player.
                }


            }


        }

        // Reads user input and takes action.
        //
        // @param zone - We need a zone to restrict the player movement
        //
        private void ReadInput()
        {
            ConsoleKeyInfo cki = Console.ReadKey();

            switch (_inputState)
            {
                case InputState.ZoneMovement:
                    if (Constants.AllArrowKeys.Contains(cki.Key))
                    {
                        _zone.Player.Move(cki.Key, _zone);
                        _db.UpdateEntityPosition(_zone.Player.Entity);

                    }
                    break;

                case InputState.Battle:

                    break;

                case InputState.Dead:
                    if (cki.Key == ConsoleKey.Enter)
                    {
                        RespawnPlayer();
                    }
                    break;
            }
        }

        private void RespawnPlayer()
        {
            Console.Clear();
            Player player = _zone.Player;
            player.Respawn();

            _inputState = InputState.ZoneMovement;
        }
    }
}