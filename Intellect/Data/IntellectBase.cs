using TicTacToeLib;
namespace Intellectual.Data
{
    public abstract class IntellectBase
    {
        protected readonly ILogger<IntellectBase> _logger;
        protected Game _game;
        public IntellectBase(ILogger<IntellectBase> logger, Game game)
        {
            _logger = logger;
            _game = game;
        }

        public abstract Task<Tuple<int, int>> GetBestMoveCoord();
        protected List<Coord> GetAvailableFields()
        {
            var availableFields = new List<Coord>();
            for (int i = 0; i < _game.LineSize; i++)
            {
                for (int j = 0; j < _game.LineSize; j++)
                {
                    if (_game.GetValue(i, j) == TicTacToeValue.No)
                    {
                        availableFields.Add(new Coord(i, j));
                    }
                }
            }
            return availableFields;
        }

        protected struct Coord
        {
            public Coord(int row, int col)
            {
                Row = row;
                Col = col;
            }
            public int Row { get; set; }
            public int Col { get; set; }
        }
        protected struct Move
        {
            public Coord Coord { get; set; }
            public int Score { get; set; }
        }
    }
}
