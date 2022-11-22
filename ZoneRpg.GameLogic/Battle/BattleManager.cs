using ZoneRpg.Database;
using ZoneRpg.Models;
using ZoneRpg.Shared;

namespace ZoneRpg.GameLogic
{
    public class BattleManager
    {
        public BattleState State { get; set; }
        public IFighter? Player { get; set; }
        public IFighter? Monster { get; set; }
        private DatabaseManager _db;

        private List<string> _messages = new List<string>();

        public BattleManager(DatabaseManager db)
        {
            _db = db;
        }


        public void AddMessage(string message)
        {
            _messages.Add(message);
        }

        public List<string> GetMessages()
        {
            return _messages;
        }

        // Funktion för att slutföra en runda (spelaren attackerar och monster attackerar en gång)
        public void ProgressBattle()
        {
            if (Player == null || Monster == null || State != BattleState.InBattle)
            {
                return;
            }

            // Spelaren attackerar
            Monster.TakeDamage(Player.GetAttack());
            AddMessage("[player] attacks [monster] for [player_attack] damage!");
            _db.UpdateCharacterHp((Character)Monster);
            
            // B död (_monster)
            if (Monster.Hp <= 0)
            {
                AddMessage($"{Monster.Name} has died!");
                State = BattleState.Won;
                return;
            }

            // Monstret attackerar
            Player.TakeDamage(Monster.GetAttack());
            AddMessage("[monster] attacks [player] for [monster_attack] damage!");
            _db.UpdateCharacterHp((Character)Player);

            // Spelaren dog
            if (Player.Hp <= 0)
            {
                AddMessage($"{Player.Name} has died!");
                State = BattleState.Lost;
            }

        }

        public void LookForMonsters(List<Entity> entities)
        {
            // Om vi redan är i en fight, så behöver vi inte leta efter nya fiender
            if (Player == null || State != BattleState.NotInBattle)
            {
                return;
            }

            // Filtera listan med entities så att vi bara får med de som är monster, och som är nära spelaren
            List<Entity> monstersEntities = entities.Where(WhereNearbyMonster).ToList();

            if (monstersEntities.Count > 0)
            {
                Monster? monster = _db.GetMonsterByEntityId(monstersEntities.First().Id);
                if (monster != null)
                {
                    Monster = monster;
                    State = BattleState.InBattle;
                    return;
                }
            }

        }

        // Kontrollerar om en entity är en monster, och om den är nära spelaren
        private bool WhereNearbyMonster(Entity entity)
        {            // Om entityn inte är ett monster:
            if (entity.EntityType != EntityType.Monster)
            {
                return false;
            }

            // Om entityn är längre bort än 1 steg:
            if (
                Math.Abs(Player!.GetX() - entity.X) > 1
                || Math.Abs(Player.GetY() - entity.Y) > 1)
            {
                return false;
            }

            // Kommer vi så här långt betyder det att vi har hittat nära monster! 
            return true;
        }

        internal void Reset()
        {
            State = BattleState.NotInBattle;
            Monster = null;
        }
    }
}


