
namespace Intellectual.Data
{
    public class Intellect
    {
        private readonly ILogger<Intellect> _logger;
        private Table MTable;
        public Intellect(ILogger<Intellect> logger, Table mTable)
        {
            _logger = logger;
            MTable = mTable;
        }
        public Tuple<int, int> GetBestMoveCoord(Table table)
        {
            Value whoseMove;
            Move result;
            try
            {
                ReadState(table, out whoseMove);
                result = Minimax(table, whoseMove);
            }
            catch (Exception e)
            {
                _logger.LogError($"GetBestMoveCoord: {e.Message}");
                throw new Exception(e.Message);
            }
            return Tuple.Create(result.Coord.Row, result.Coord.Col);
        }

        private void ReadState(Table table, out Value whoseMove)
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
        }

        private bool IsWinner(Table table, Value value)
        {
            for (int i = 0; i < Table.Size; i++)
            {
                for (int j = 0; j < Table.Size; j++)
                {
                    if (table[i, j] != value)
                        break;
                    if (j == Table.Size - 1)
                        return true;
                }
            }

            for (int i = 0; i < Table.Size; i++)
            {
                for (int j = 0; j < Table.Size; j++)
                {
                    if (table[j, i] != value)
                        break;
                    if (j == Table.Size - 1)
                        return true;
                }
            }

            for (int i = 0; i < Table.Size; i++)
            {
                if (table[i, i] != value)
                    break;
                if (i == Table.Size - 1)
                    return true;
            }

            for (int i = 0; i < Table.Size; i++)
            {
                if (table[i, (Table.Size - 1) - i] != value)
                    break;
                if (i == Table.Size - 1)
                    return true;
            }
            return false;
        }

        private List<Coord> GetAvailableFields(Table table)
        {
            var availableFields = new List<Coord>();
            for (int i = 0; i < Table.Size; i++)
            {
                for (int j = 0; j < Table.Size; j++)
                {
                    if (table[i, j] == Value.No)
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

        private Move Minimax(Table table, Value whoseMove)
        {
            var availableFields = GetAvailableFields(table);

            if (IsWinner(table, Value.X))
            {
                return new Move { Score = -10 };
            }
            else if (IsWinner(table, Value.O))
            {
                return new Move { Score = 10 };
            }
            else if (availableFields.Count == 0)
            {
                return new Move { Score = 0 };
            }

            var moves = new List<Move>();

            for (int i = 0; i < availableFields.Count; i++)
            {
                var move = new Move();
                var coord = new Coord();
                coord.Row = availableFields[i].Row;
                coord.Col = availableFields[i].Col;
                move.Coord = coord;

                table[move.Coord.Row, move.Coord.Col] = whoseMove;

                if (whoseMove == Value.X)
                {
                    var result = Minimax(table, Value.O);
                    move.Score = result.Score;
                }
                else
                {
                    var result = Minimax(table, Value.X);
                    move.Score = result.Score;
                }

                table[move.Coord.Row, move.Coord.Col] = Value.No;

                moves.Add(move);
            }

            int bestScore;
            if (whoseMove == Value.O)
            {
                bestScore = int.MinValue;
                for (int i = 0; i < moves.Count; i++)
                {
                    if (moves[i].Score > bestScore)
                    {
                        bestScore = moves[i].Score;
                    }
                }
            }
            else
            {
                bestScore = int.MaxValue;
                for (int i = 0; i < moves.Count; i++)
                {
                    if (moves[i].Score < bestScore)
                    {
                        bestScore = moves[i].Score;
                    }
                }
            }

            // Some randomization
            var indexes = new List<int>();
            for (int i = 0; i < moves.Count; i++)
            {
                if (moves[i].Score == bestScore)
                    indexes.Add(i);
            }

            Random random = new Random();
            var randIndex = random.Next(0, indexes.Count);
            var bestMove = indexes[randIndex];
            //

            return moves[bestMove];
        }
    }
}