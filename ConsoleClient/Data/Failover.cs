using TicTacToeLib;
namespace ConsoleClient.Data
{
    public class Failover : FailoverBase
    {
        public Failover(IEnumerable<IHelper> helpers) : base(helpers)
        {
        }
        public override IHelper ChangeHelper(Type replacemenеType)
        {
            foreach (var helper in _helpers)
            {
                if (helper.GetType() == replacemenеType)
                {
                    continue;
                }
                return helper;
            }
            return _helpers.First();
        }
    }
}
