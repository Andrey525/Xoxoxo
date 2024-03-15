
namespace TicTacToeLib
{
    public abstract class FailoverBase
    {
        protected readonly IEnumerable<IHelper> _helpers;
        public FailoverBase(IEnumerable<IHelper> helpers)
        {
            _helpers = helpers;
        }
        public abstract IHelper ReplaceHelper(Type replacementType);
    }
}
