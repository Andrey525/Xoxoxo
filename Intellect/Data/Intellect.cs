using TicTacToeLib;

namespace Intellectual.Data
{
    public static class Intellect
    {
        public static Tuple<int, int> GetBestMoveCoord(TicTacToeModel model)
        {
            int[,] table = new int[3, 3];
            for (int i = 0; i < table.GetLength(0); i++)
            {
                for (int j = 0; j < table.GetLength(1); j++)
                {
                    table[i, j] = (int)model.Table[i, j];
                }
            }

            if (model.State == TicTacToeState.WaitOMove)
            {
                var result = Minimax(table, 2);
                return Tuple.Create(result.i, result.j);
            }
            else if (model.State == TicTacToeState.WaitXMove)
            {
                var result = Minimax(table, 1);
                return Tuple.Create(result.i, result.j);
            }
            return Tuple.Create(0, 0);
        }

        private static List<Coord> GetAvailableFields(in int[,] table)
        {
            var availableFields = new List<Coord>();
            for (int i = 0; i < TicTacToeTable.Size; i++)
            {
                for (int j = 0; j < TicTacToeTable.Size; j++)
                {
                    if (table[i, j] == 0)
                    {
                        availableFields.Add(new Coord(i, j));
                    }
                }
            }
            return availableFields;
        }

        private struct Coord
        {
            public Coord(int x, int y)
            {
                i = x;
                j = y;
            }
            public int i { get; set; }
            public int j { get; set; }
        }
        private struct Move
        {
            public int i { get; set; }
            public int j { get; set; }
            public int score { get; set; }
        }

        private static bool HaveWinner(in int[,] table, int who)
        {
            return (table[0, 0] == who && table[0, 1] == who && table[0, 2] == who) ||
                (table[1, 0] == who && table[1, 1] == who && table[1, 2] == who) ||
                (table[2, 0] == who && table[2, 1] == who && table[2, 2] == who) ||
                (table[0, 0] == who && table[1, 0] == who && table[2, 0] == who) ||
                (table[0, 1] == who && table[1, 1] == who && table[2, 1] == who) ||
                (table[0, 2] == who && table[1, 2] == who && table[2, 2] == who) ||
                (table[0, 0] == who && table[1, 1] == who && table[2, 2] == who) ||
                (table[0, 2] == who && table[1, 1] == who && table[2, 0] == who);
        }

        private static Move Minimax(in int[,] table, int who)
        {
            var availableFields = GetAvailableFields(table);

            if (HaveWinner(table, 1))
            {
                return new Move { score = -10 };
            }
            else if (HaveWinner(table, 2))
            {
                return new Move { score = 10 };
            }
            else if (availableFields.Count == 0)
            {
                return new Move { score = 0 };
            }

            var moves = new List<Move>();

            for (int i = 0; i < availableFields.Count; i++)
            {
                var move = new Move();
                move.i = availableFields[i].i;
                move.j = availableFields[i].j;
                table[move.i, move.j] = who;

                if (who == 1)
                {
                    var result = Minimax(table, 2);
                    move.score = result.score;
                }
                else
                {
                    var result = Minimax(table, 1);
                    move.score = result.score;
                }

                table[move.i, move.j] = 0;

                moves.Add(move);
            }

            int bestMove = 0;
            int bestScore;
            if (who == 2)
            {
                bestScore = -10000;
                for (int i = 0; i < moves.Count; i++)
                {
                    if (moves[i].score > bestScore)
                    {
                        bestScore = moves[i].score;
                        bestMove = i;
                    }
                }
            }
            else
            {
                bestScore = 10000;
                for (int i = 0; i < moves.Count; i++)
                {
                    if (moves[i].score < bestScore)
                    {
                        bestScore = moves[i].score;
                        bestMove = i;
                    }
                }
            }

            // Some randomization
            var indexes = new List<int>();
            for (int i = 0; i < moves.Count; i++)
            {
                if (moves[i].score == bestScore)
                    indexes.Add(i);
            }

            Random random = new Random();
            var randIndex = random.Next(0, indexes.Count);
            bestMove = indexes[randIndex];
            //

            return moves[bestMove];
        }
    }
}