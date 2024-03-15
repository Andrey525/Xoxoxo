namespace TicTacToeLib
{
    public class Bot
    {
        private readonly IEnumerable<IHelper> _helpers;
        private readonly FailoverBase _failover;
        private IHelper Helper { get; set; }
        public Game Game { get; set; }
        public TicTacToeValue Value { get; set; }

        public Bot(IEnumerable<IHelper> helpers, FailoverBase failoverBase)
        {
            if (!helpers.Any())
            {
                throw new Exception("Bot: No helpers");
            }

            _helpers = helpers;
            Helper = _helpers.First();

            _failover = failoverBase;
        }

        public bool ChangeHelper(Type type)
        {
            foreach (var helper in _helpers)
            {
                if (helper.GetType() == type)
                {
                    Helper = helper;
                    return true;
                }
            }
            return false;
        }

        private async Task<Point> GetHelp()
        {
            return await Helper.GetHelp(new State(Game._state));
        }

        public async Task MakeMove(object sender, EventArgs e)
        {
            if ((TicTacToeValue)Game._state.ProgressState == Value)
            {
                Point? cellCoordinates = null;

                try
                {
                    cellCoordinates = await GetHelp();
                }
                catch (Exception)
                {
                    Helper = _failover.ChangeHelper(Helper.GetType());
                    cellCoordinates = await GetHelp();
                }

                if (cellCoordinates == null)
                {
                    throw new NullReferenceException("cellCoordinates == null");
                }
                await Game.FillCell((int)cellCoordinates?.X, (int)cellCoordinates?.Y, Value);
            }
        }
    }
}
