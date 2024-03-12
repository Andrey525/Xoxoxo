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

        // Need Validate in TicTacToeModel class
        /*private void ReadState(Table table, out Value whoseMove)
        {
            int Xcount = 0;
            int Ocount = 0;
            int freeCount = 0;
            for (int i = 0; i < Table.Size; i++)
            {
                for (int j = 0; j < Table.Size; j++)
                {
                    if (!Enum.IsDefined(typeof(Value), table[i, j]))
                    {
                        throw new ArgumentException("Invalid value in table");
                    }

                    if (table[i, j] == Value.X)
                        Xcount++;
                    else if (table[i, j] == Value.O)
                        Ocount++;
                    else
                        freeCount++;
                }
            }

            if (freeCount == 0)
            {
                throw new Exception("No free cells");
            }

            if (Xcount == Ocount)
            {
                // last move was by O
                whoseMove = Value.X;
            }
            else if (Xcount == Ocount + 1)
            {
                // last move was by X
                whoseMove = Value.O;
            }
            else
                throw new Exception("Invalid State");

            if (IsWinner(table, Value.X) || IsWinner(table, Value.O))
            {
                throw new Exception("Already have winner. Or may be invalid State");
            }
        }*/
        private List<Coord> GetAvailableFields(TicTacToeModel model)
        {
            var availableFields = new List<Coord>();
            for (int i = 0; i < 3; i++) // !!! table size needed
            {
                for (int j = 0; j < 3; j++) // !!! table size needed
                {
                    if (model.GetValue(i, j) == TicTacToeValue.No)
                    {
                        availableFields.Add(new Coord(i, j));
                    }
                }
            }
            return availableFields;
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

        private Move Minimax(TicTacToeModel model)
        {
            var availableFields = GetAvailableFields(model);

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

            var moves = new List<Move>();

            TicTacToeMemento memento = null;
            for (int i = 0; i < availableFields.Count; i++)
            {
                var move = new Move();
                var coord = new Coord();
                coord.Row = availableFields[i].Row;
                coord.Col = availableFields[i].Col;
                move.Coord = coord;

                memento = model.SaveState();
                model.MakeMove(move.Coord.Row, move.Coord.Col, (TicTacToeValue)model.State);

                var result = Minimax(model);
                move.Score = result.Score;

                model.RestoreState(memento);

                moves.Add(move);
            }

            int bestScore;
            if (memento?.State == TicTacToeState.WaitOMove)
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
    }
}