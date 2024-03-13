using TicTacToeLib;
namespace Intellectual.Data
{
    public class Intellect
    {
        private readonly ILogger<Intellect> _logger;
        private TicTacToeModel _model;
        public Intellect(ILogger<Intellect> logger, TicTacToeModel model)
        {
            _logger = logger;
            _model = model;
        }
        public Tuple<int, int> GetBestMoveCoord()
        {
            Move result;
            try
            {
                result = Minimax(_model);
            }
            catch (Exception e)
            {
                _logger.LogError($"GetBestMoveCoord: {e.Message}");
                throw new Exception(e.Message);
            }
            return Tuple.Create(result.Coord.Row, result.Coord.Col);
        }
        private List<Coord> GetAvailableFields(TicTacToeModel model)
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
        private Move Minimax(TicTacToeModel model)
        {
            if (model.State == TicTacToeState.XWin)
            {
                return new Move { Score = -10 };
            }
            else if (model.State == TicTacToeState.OWin)
            {
                return new Move { Score = 10 };
            }
            else if (model.State == TicTacToeState.Draw)
            {
                return new Move { Score = 0 };
            }

            var availableFields = GetAvailableFields(model);

            var moves = new List<Move>();

            TicTacToeState state = TicTacToeState.Invalid;
            for (int i = 0; i < availableFields.Count; i++)
            {
                TicTacToeMemento memento = model.SaveState();
                state = memento.State;

                model.MakeMove(availableFields[i].Row, availableFields[i].Col, (TicTacToeValue)model.State);

                var result = Minimax(model);

                model.RestoreState(memento);

                moves.Add(new Move()
                {
                    Coord = new Coord()
                    {
                        Col = availableFields[i].Col,
                        Row = availableFields[i].Row
                    },
                    Score = result.Score
                });
            }

            int bestScore;
            if (state == TicTacToeState.WaitOMove)
            {
                bestScore = moves.Select(t => t.Score).Max();
            }
            else
            {
                bestScore = moves.Select(t => t.Score).Min();
            }

            // Some randomization
            var indexes = moves.Select(t => t).Where(t => t.Score == bestScore);

            Random random = new Random();
            var randIndex = random.Next(0, indexes.ToArray().Length);
            var bestMove = indexes.ToArray()[randIndex];
            //

            return bestMove;
        }

        private struct Coord
        {
            public Coord(int row, int col)
            {
                Row = row;
                Col = col;
            }
            public int Row { get; set; }
            public int Col { get; set; }
        }
        private struct Move
        {
            public Coord Coord { get; set; }
            public int Score { get; set; }
        }
    }
}