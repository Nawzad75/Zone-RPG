namespace ZoneRpg.Ui
{
    public class FightResult
    {
        private FightState _state;
        private List<string> _messages { get; set; } = new List<string>();

        public FightResult(FightState state)
        {
            _state = state;
        }

        internal void AddMessage(string v)
        {
            _messages.Add(v);
        }

        internal void SetState(FightState newState)
        {
            _state = newState;
        }
    }
}