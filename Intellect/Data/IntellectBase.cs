using TicTacToeLib;
namespace Intellectual.Data
{
    public abstract class IntellectBase
    {
        protected readonly ILogger<IntellectBase> _logger;
        protected TicTacToeModel _model;
        public IntellectBase(ILogger<IntellectBase> logger, TicTacToeModel model)
        {
            _logger = logger;
            _model = model;
        }

        public abstract Tuple<int, int> GetBestMoveCoord();
        protected List<Coord> GetAvailableFields(TicTacToeModel model)
        {
            var availableFields = new List<Coord>();
            for (int i = 0; i < model.TableSize; i++)
            {
                for (int j = 0; j < model.TableSize; j++)
                {
                    if (model.GetValue(i, j) == TicTacToeValue.No)
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
