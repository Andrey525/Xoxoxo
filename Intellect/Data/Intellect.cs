namespace Intellectual.Data
{
    public static class Intellect
    {
        public const int Size = 3;
        public static Tuple<int, int> GetBestMoveCoord(in int[,] table)
        {
            int who;
            try
            {
                CheckTable(table);
                ReadState(table, out who);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            var result = Minimax(table, who);
            return Tuple.Create(result.i, result.j);
        }

        private static void CheckTable(in int[,] table)
        {
            if (table.GetLength(0) != Size && table.GetLength(1) != Size)
            {
                throw new ArgumentException("Invalid table size");
            }

            int FreeCount = 0;
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    if (table[i, j] != 0 &&
                        table[i, j] != 1 &&
                        table[i, j] != 2)
                    {
                        throw new ArgumentException("Invalid value in table");
                    }

                    if (table[i, j] == 0)
                        FreeCount++;
                }
            }

            if (FreeCount == 0)
                throw new Exception("No free cell!");
        }

        private static void ReadState(in int[,] table, out int who)
        {
            int Xcount = 0;
            int Ocount = 0;
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    if (table[i, j] == 1)
                        Xcount++;
                    else if (table[i, j] == 2)
                        Ocount++;
                }
            }

            // определим, чей следующий ход
            if (Xcount == Ocount)
                who = 1;
            else if (Xcount == Ocount + 1)
                who = 2;
            else
                throw new Exception("Invalid Game State");
            //

            // проверим, мб пришло состояние, где уже есть победитель
            int equalCount = 0;
            // проверка по строкам
            for (int i = 0; i < Size; i++)
            {
                var firstValue = table[i, 0];
                // свободная ячейка
                if (firstValue == 0)
                    continue;

                equalCount = 1;
                for (int j = 1; j < Size; j++)
                {
                    if (table[i, j] == firstValue)
                        equalCount++;
                }

                if (equalCount == Size)
                    throw new Exception("Have winner. Game is over. Or may be invalid state");
            }
            // проверка по столбцам
            for (int i = 0; i < Size; i++)
            {
                var firstValue = table[0, i];
                // свободная ячейка
                if (firstValue == 0)
                    continue;

                equalCount = 1;
                for (int j = 1; j < Size; j++)
                {
                    if (table[j, i] == firstValue)
                        equalCount++;
                }

                if (equalCount == Size)
                    throw new Exception("Have winner. Game is over. Or may be invalid state");
            }
            // проверка главной диагонали
            equalCount = 0;
            for (int i = 0; i < Size; i++)
            {
                if (table[i, i] == 0)
                    break;

                if (table[i, i] == table[0, 0])
                    equalCount++;

                if (equalCount == Size)
                    throw new Exception("Have winner. Game is over.");
            }
            // проверка побочной диагонали
            equalCount = 0;
            for (int i = 0; i < Size; i++)
            {
                if (table[i, (Size - 1) - i] == 0)
                    break;

                if (table[i, (Size - 1) - i] == table[0, Size - 1])
                    equalCount++;

                if (equalCount == Size)
                    throw new Exception("Have winner. Game is over.");
            }
            //
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

        private static List<Coord> GetAvailableFields(in int[,] table)
        {
            var availableFields = new List<Coord>();
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
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