using ZoneRpg.Database;
using ZoneRpg.Shared;

namespace ZoneRpg.GameLogic
{
    public class BattleManager
    {
        public BattleState State { get; set; }

        private IFighter? _player;
        private IFighter? _monster ;
        private DatabaseManager _db;

        private List<string> _messages = new List<string>();

        public BattleManager(DatabaseManager db)
        {
            _db = db;
        }

        public void SetMonster(IFighter monster)
        {
            _monster = monster;
        }

        public IFighter? GetMonster()
        {
            return _monster;
        }

        public void SetPlayer(IFighter player)
        {
            _player = player;
        }

        public IFighter? GetPlayer()
        {
            return _player;
        }

        public void AddMessage(string message)
        {
            _messages.Add(message);
        }

        public List<string> GetMessages()
        {
            return _messages;
        }

        //
        //
        //
        public void ProgressBattle()
        {
            if (_player == null || _monster == null)
            {
                return;
            }


            // Spelaren attackerar
            _monster.TakeDamage(_player.GetAttack());
            AddMessage("[player] attacks [monster] for [player_attack] damage!");

            // B died (_monster)
            if (_monster.Hp <= 0)
            {
                AddMessage($"{_monster.Name} has died!");
                State = BattleState.Won;
                _monster = null;
                return;
            }

            // Monstret attackerar
            _player.TakeDamage(_monster.GetAttack());
            AddMessage("[monster] attacks [player] for [monster_attack] damage!");

            // A died (_player)
            if (_player.Hp <= 0)
            {
                AddMessage($"{_player.Name} has died!");
                State = BattleState.Lost;
                _monster = null;
            }

        }


        public void LookForMonsters(List<Entity> entities)
        {
            // Om vi redan är i en fight, så behöver vi inte leta efter nya fiender
            if (_player == null || State != BattleState.NotInBattle)
            {
                return ;
            }

            // Filtera listan med entities så att vi bara får med de som är monster, och som är nära spelaren
            List<Entity> monstersEntities = entities.Where(WhereNearbyMonster).ToList();

            if (monstersEntities.Count > 0)
            {
                Monster? monster = _db.GetMonsterByEntityId(monstersEntities.First().Id);
                if (monster != null)
                {
                    SetMonster(monster);
                    State = BattleState.InBattle;
                    return ;
                }
            }

        }

        //
        // Checks if a monster is next to the player
        //
        private bool WhereNearbyMonster(Entity entity)
        {            // Om entityn inte är ett monster:
            if (entity.EntityType != EntityType.Monster)
            {
                return false;
            }

            // Om entityn är längre bort än 1 steg:
            if (
                Math.Abs(_player!.GetX() - entity.X) > 1
                || Math.Abs(_player.GetY() - entity.Y) > 1)
            {
                return false;
            }

            // Kommer vi så här långt betyder det att vi har hittat nära monster! 
            return true;
        }



    }
}


