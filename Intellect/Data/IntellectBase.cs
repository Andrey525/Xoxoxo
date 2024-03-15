using TicTacToeLib;
namespace Intellectual.Data
{
    public abstract class IntellectBase
    {
        protected readonly ILogger<IntellectBase> _logger;
        protected Game _game;
        protected Random _random;
        public IntellectBase(ILogger<IntellectBase> logger, Game game)
        {
            _logger = logger;
            _game = game;
            _random = new Random();
        }
        public abstract Task<Point> GetBestMoveCoord();
        protected List<Point> GetAvailableFields()
        {
            var availableFields = new List<Point>();
            for (int i = 0; i < _game.LineSize; i++)
            {
                for (int j = 0; j < _game.LineSize; j++)
                {
                    if (_game.GetValue(i, j) == TicTacToeValue.No)
                    {
                        availableFields.Add(new Point(i, j));
                    }
                }
            }
            return availableFields;
        }
    }
}
