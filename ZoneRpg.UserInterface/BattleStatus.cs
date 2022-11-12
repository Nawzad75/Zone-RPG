namespace ZoneRpg.UserInterface
{
    public class BattleStatus
    {
        public BattleState State { get; set; }
        private List<string> _messages = new List<string>();

        public BattleStatus(BattleState state)
        {
            State = state;
        }

        public void AddMessage(string v)
        {
            _messages.Add(v);
        }


        public List<string> GetMessages()
        {
            return _messages;
        }
    }
}