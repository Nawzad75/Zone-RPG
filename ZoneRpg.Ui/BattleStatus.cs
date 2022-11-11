namespace ZoneRpg.Ui
{
    public class BattleStatus
    {
        private BattleState _state;
        private List<string> _messages = new List<string>();

        public BattleStatus(BattleState state)
        {
            _state = state;
        }

        public void AddMessage(string v)
        {
            _messages.Add(v);
        }

        public void SetState(BattleState newState)
        {
            _state = newState;
        }

        public BattleState GetState()
        {
            return _state;
        }

        public List<string> GetMessages()
        {
            return _messages;
        }
    }
}