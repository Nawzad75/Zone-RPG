using ZoneRpg.Database;
using ZoneRpg.Shared;

namespace ZoneRpg.GameLogic
{
    public class BattleManager
    {
        private IFighter? _player;
        private IFighter? _monster;
        private DatabaseManager _db;
        private int _turn = 0;
        public BattleStatus Status = new BattleStatus(BattleState.NotInBattle);

        public BattleManager(DatabaseManager db)
        {
            _db = db;
        }



        public void AddMonsterToBattle(IFighter monster)
        {
            _monster = monster;
            Status.State = BattleState.InBattle;
        }

        public void AddPlayerToBattle(IFighter player)
        {
            _player = player;
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

            _turn++;

            if (_turn == 1)
            {
                Status.State = BattleState.BattleJustStarted;
            }

            // Spelaren attackerar
            _monster.TakeDamage(_monster.Attack);
            Status.AddMessage($"{_player.Name} attacks {_monster.Name} for {_player.Attack} damage!");

            // B died (_monster)
            if (_monster.Hp <= 0)
            {
                Status.AddMessage($"{_monster.Name} has died!");
                Status.State = BattleState.Won;
                _monster = null;
                return;
            }


            // Monstret attackerar
            _player.TakeDamage(_monster.Attack);
            Status.AddMessage($"{_monster.Name} attacks {_player.Name} for {_monster.Attack} damage!");

            // A died (_player)
            if (_player.Hp <= 0)
            {
                Status.AddMessage($"{_player.Name} has died!");
                Status.State = BattleState.Lost;
                _monster = null;
            }

        }


        public void LookForMonsters(List<Entity> entities)
        {
            // Om vi redan är i en fight, så behöver vi inte leta efter nya fiender
            if (Status.State != BattleState.NotInBattle)
            {
                return;
            }

            // Filtera listan med entities så att vi bara får med de som är monster, och som är nära spelaren
            List<Entity> monstersEntities = entities.Where(WhereNearbyMonster).ToList();

            if (monstersEntities.Count > 0)
            {
                int id = monstersEntities.First().Id;
                Monster? monster = _db.GetMonsterByEntityId(id);
                if (monster != null)
                {
                    AddMonsterToBattle(monster);
                }
            }

        }

        //
        // Checks if a monster is next to the player
        //
        private bool WhereNearbyMonster(Entity entity)
        {
            // Vi vill vara säkra på att _player finns
            if(_player == null)
            {
                return false;
            }
            
            // Om entityn inte är ett monster:
            if (entity.EntityType != EntityType.Monster)
            {
                return false;
            }

            // Om entityn är längre bort än 1 steg:
            if (
                Math.Abs(_player.Entity.X - entity.X) > 1
                || Math.Abs(_player.Entity.Y - entity.Y) > 1)
            {
                return false;
            }

            // Kommer vi så här långt betyder det att vi har hittat nära monster! 
            return true;
        }

        //
        //
        //
        public BattleStatus GetBattleStatus()
        {
            return Status;
        }


        public IFighter? GetMonster()
        {
            return _monster;
        }
    }
}


