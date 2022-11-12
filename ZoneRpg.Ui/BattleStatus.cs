namespace ZoneRpg.Ui
{
    public class BattleStatus
    {
        public BattleState State { get; private set; }
        private List<string> _messages = new List<string>();

        public BattleStatus(BattleState state)
        {
            State = state;
        }

        public void AddMessage(string v)
        {
            _messages.Add(v);
        }

        public void SetState(BattleState newState)
        {
            State = newState;
        }

        public List<string> GetMessages()
        {
            return _messages;
        }
    }
}