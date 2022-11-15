﻿using System.Text;
using System.Drawing;
using Colorful;
using Console = Colorful.Console;
using ZoneRpg.Database;
using ZoneRpg.GameLogic;
using ZoneRpg.Shared;
using System.Collections.Concurrent;

namespace ZoneRpg.UserInterface
{
    public class Ui
    {
        // Ui Members
        private DatabaseManager _db;
        private ZoneRenderer _zoneRenderer = new ZoneRenderer();
        private IRenderer _playerRenderer = new CharacterRenderer();
        private IRenderer _monsterRenderer = new CharacterRenderer();
        private IRenderer _battleRenderer;
        private Game _game;

        // Constructor
        public Ui(DatabaseManager db, Game game)
        {
            _db = db;
            _game = game;
            _battleRenderer = new BattleRenderer(game.BattleManager);
            Setup();
        }

        // Setup the UI
        private void Setup()
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.CursorVisible = false;
            Console.Clear();

            // Rektangeln som visar spelarens stats
            _playerRenderer.SetRect(2, _game.Zone.Height + 3, 30, 4);
            _playerRenderer.SetAccentColor(ConsoleColor.Cyan);

            // Rektangeln som visar monstrets stats
            _monsterRenderer.SetRect(34, _game.Zone.Height + 3, 30, 4);
            _monsterRenderer.SetAccentColor(ConsoleColor.Red);

            // Rektangel som visard  battle-meddelanden
            _battleRenderer.SetRect(2, _game.Zone.Height + 8, 64, 5);
        }

        // Run!
        public void Render()
        {
            switch (_game.State)
            {
                case GameState.MainMenu:
                    new StartGame().RunMainMenu();
                    _game.SetState(GameState.GetPlayer);
                    Render(); // Render again to show the new state before we read input
                    break;

                case GameState.GetPlayer:
                    _game.SetPlayer(CreateOrChoosePlayer());
                    ((CharacterRenderer)_playerRenderer).SetCharacter(_game.GetPlayer());
                    _game.SetState(GameState.Zone);
                    Render(); // Render again to show the new state before we read input
                    break;

                case GameState.Dead:
                    Console.Clear();
                    Console.WriteLine("You died!");
                    Console.WriteLine("Press <Enter> to respawn...");
                    break;

                case GameState.Zone:
                    DrawZone();
                    _playerRenderer.Draw();
                    _monsterRenderer.Draw();
                    break;

                case GameState.Battle:                    
                    DrawZone();
                    // cast IRenderer to CharacterRender, so we can get/set the current Monster
                    CharacterRenderer monsterRenderer = (CharacterRenderer) _monsterRenderer;
                    if (!monsterRenderer.hasCharacter())
                    {
                        monsterRenderer.SetCharacter(_game.BattleManager.GetMonster());
                    }
                    _playerRenderer.Draw();
                    _monsterRenderer.Draw();
                    _battleRenderer.Draw();
                    break;
            }

            // Är detta UI? (delvis?) 
            // Hur flyttar vi det nån annanstans?
            OpenChest();
        }

        private void DrawZone()
        {
            _zoneRenderer.DrawZone(_game.Zone);
            _zoneRenderer.DrawEntities(_game.Zone.Entities);
            _zoneRenderer.DrawPlayerEntity(_game.GetPlayerEntity());
            _zoneRenderer.DrawMessageBox(_game.MessageBox, _game.Zone);
        }



        // Let the player choose a character or create a new one
        private Player CreateOrChoosePlayer()
        {
            string prompt = "Create or choose a character!";
            CreateChooseMenu createOption = new MenuEnum<CreateChooseMenu>(prompt).Run();

            if (createOption == CreateChooseMenu.Create)
            {
                Player player = CreatePlayer();
                _db.InsertCharacter(player);
                return player;
            }            
            return ChoosePlayer();
        }

        // Create a player
        public Player CreatePlayer()
        {
            Console.Clear();
            Console.WriteLine("Enter Character Name");

            string name = Console.ReadLine(); //här skickar vi in namnet som spelaren skriver in
            if (string.IsNullOrEmpty(name))
            {
                Console.WriteLine("Your entery was blank, press enter & try again");
                Console.ReadKey();
                return CreatePlayer();
            }
            else
            {
                // Todo: get all classes from db, and let the player choose
                CharacterClass characterClass = new CharacterClass();
                characterClass.Name = "Warrior";
                characterClass.MaxHp = 100;
                characterClass.BaseAttack = 10;
                return new Player(name, characterClass);
            }

        }

        // Choose player
        public Player ChoosePlayer()
        {
            Console.Clear();
            List<Player> players = _db.GetPlayers();
            string[] options = players.Select(c => $"{c.Name}  (id: {c.id})").ToArray();
            int index = new Menu("Choose a character:", options).Run();
            return players[index];
        }

        // Create monster
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
            Entity? chestEntity = _game.Zone.Entities.Find(entity => entity.EntityType == EntityType.Chest);
            if (chestEntity == null)
            {
                return;
            }

            Entity playerEntity = _game.GetPlayerEntity();

            if (chestEntity.X == playerEntity.X && chestEntity.Y == playerEntity.Y)
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
        // @param zone - We need a zone to restrict the player movement
        public void ReadInput()
        {

            ConsoleKeyInfo cki = Console.ReadKey();
            switch (_game.State)
            {
                case GameState.Zone:
                    if (Constants.AllArrowKeys.Contains(cki.Key))
                    {
                        _game.MovePlayer(cki.Key);
                    }
                    break;

                case GameState.Battle:

                    break;

                case GameState.Dead:
                    if (cki.Key == ConsoleKey.Enter)
                    {
                        _game.RespawnPlayer();
                    }
                    break;
            }
            //Chat start.
            string input = cki.KeyChar.ToString().ToLower();
            if (input == "t")
            {
                Chat();
            }
        }


        public void Chat()
        {
            Console.Clear();
            CreateMessage();
        }

        // Create a message
        public void CreateMessage()
        {
            Console.Clear();
            Console.WriteLine("Write a message");
            string message = Console.ReadLine()!;
            Message message1 = new Message(message, character: _game.GetPlayer());
            _db.InsertMessage(message1);
        }
    }
}