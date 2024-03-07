namespace TicTacToeLib
{
    public class TicTacToeModel
    {
        private int _moveCount;
        public TicTacToeTable Table { get; private set; }
        public TicTacToeState State { get; private set; }

        public TicTacToeModel(TicTacToeValue firstMoveValue)
        {
            _moveCount = 0;
            Table = new TicTacToeTable();
            State = SetWaitStateByValue(firstMoveValue);
        }

        public TicTacToeModel(List<TicTacToeValue> values)
        {
            if (values == null || values.Count != TicTacToeTable.Size * TicTacToeTable.Size)
            {
                throw new ArgumentException("Bad list");
            }
            Table = new TicTacToeTable();
            int Xcount = 0;
            int Ocount = 0;
            int freeCount = 0;
            for (int i = 0; i < TicTacToeTable.Size; i++)
            {
                for (int j = 0; j < TicTacToeTable.Size; j++)
                {
                    if (!Enum.IsDefined(typeof(TicTacToeValue), values[(i * TicTacToeTable.Size) + j]))
                    {
                        throw new ArgumentException("Invalid value in table");
                    }

                    if (values[(i * TicTacToeTable.Size) + j] == TicTacToeValue.X)
                        Xcount++;
                    else if (values[(i * TicTacToeTable.Size) + j] == TicTacToeValue.O)
                        Ocount++;
                    else
                        freeCount++;

                    Table[i, j] = values[(i * TicTacToeTable.Size) + j];
                }
            }

            _moveCount = TicTacToeTable.Size - freeCount;

            if (Xcount == Ocount)
            {
                // last move was by O
                State = SetWaitStateByValue(TicTacToeValue.X);
            }
            else if (Xcount == Ocount + 1)
            {
                // last move was by X
                State = SetWaitStateByValue(TicTacToeValue.O);
            }
            else
                throw new Exception("Invalid Game State");

            if (freeCount == 0)
            {
                State = TicTacToeState.Draw;
                return;
            }

            var winner = DetermineWinner((State == TicTacToeState.WaitXMove) ? TicTacToeValue.O : TicTacToeValue.X);
            if (winner != TicTacToeValue.No)
            {
                State = SetWinStateByWinnerValue(winner);
                return;
            }
        }

        public void MakeMove(int row, int col, TicTacToeValue value)
        {
            if (State != TicTacToeState.WaitXMove &&
                State != TicTacToeState.WaitOMove)
            {
                throw new Exception("Game was over. But you make move");
            }

            if ((row < 0 || row >= TicTacToeTable.Size) ||
                (col < 0 || col >= TicTacToeTable.Size) ||
                !Enum.IsDefined(typeof(TicTacToeValue), value))
            {
                throw new ArgumentException("Invalid agrument");
            }

            Table[row, col] = value;
            _moveCount++;
            UpdateGameState(row, col, value);
        }

        private void UpdateGameState(int row, int col, TicTacToeValue lastValue)
        {
            var winner = DetermineWinner(row, col, lastValue);
            if (winner != TicTacToeValue.No)
            {
                State = SetWinStateByWinnerValue(winner);
                return;
            }

            if (_moveCount == TicTacToeTable.Size * TicTacToeTable.Size)
            {
                State = TicTacToeState.Draw;
                return;
            }

            State = (State == TicTacToeState.WaitXMove) ? TicTacToeState.WaitOMove
                                                        : TicTacToeState.WaitXMove;
        }

        private TicTacToeValue DetermineWinner(int row, int col, TicTacToeValue value)
        {
            for (int i = 0; i < TicTacToeTable.Size; i++)
            {
                if (Table[row, i] != value)
                    break;
                if (i == TicTacToeTable.Size - 1)
                    return value;
            }

            for (int i = 0; i < TicTacToeTable.Size; i++)
            {
                if (Table[i, col] != value)
                    break;
                if (i == TicTacToeTable.Size - 1)
                    return value;
            }

            for (int i = 0; i < TicTacToeTable.Size; i++)
            {
                if (Table[i, i] != value)
                    break;
                if (i == TicTacToeTable.Size - 1)
                    return value;
            }

            for (int i = 0; i < TicTacToeTable.Size; i++)
            {
                if (Table[i, (TicTacToeTable.Size - 1) - i] != value)
                    break;
                if (i == TicTacToeTable.Size - 1)
                    return value;
            }
            return TicTacToeValue.No;
        }

        private TicTacToeValue DetermineWinner(TicTacToeValue value)
        {
            for (int i = 0; i < TicTacToeTable.Size; i++)
            {
                for (int j = 0; j < TicTacToeTable.Size; j++)
                {
                    if (Table[i, j] != value)
                        break;
                    if (j == TicTacToeTable.Size - 1)
                        return value;
                }
            }

            for (int i = 0; i < TicTacToeTable.Size; i++)
            {
                for (int j = 0; j < TicTacToeTable.Size; j++)
                {
                    if (Table[j, i] != value)
                        break;
                    if (j == TicTacToeTable.Size - 1)
                        return value;
                }
            }

            for (int i = 0; i < TicTacToeTable.Size; i++)
            {
                if (Table[i, i] != value)
                    break;
                if (i == TicTacToeTable.Size - 1)
                    return value;
            }

            for (int i = 0; i < TicTacToeTable.Size; i++)
            {
                if (Table[i, (TicTacToeTable.Size - 1) - i] != value)
                    break;
                if (i == TicTacToeTable.Size - 1)
                    return value;
            }
            return TicTacToeValue.No;
        }

        private TicTacToeState SetWinStateByWinnerValue(TicTacToeValue value)
        {
            switch (value)
            {
                case TicTacToeValue.X:
                    return TicTacToeState.XWin;
                case TicTacToeValue.O:
                    return TicTacToeState.OWin;
                default:
                    return TicTacToeState.Invalid;
            }
        }

        private TicTacToeState SetWaitStateByValue(TicTacToeValue value)
        {
            switch (value)
            {
                case TicTacToeValue.X:
                    return TicTacToeState.WaitXMove;
                case TicTacToeValue.O:
                    return TicTacToeState.WaitOMove;
                default:
                    return TicTacToeState.Invalid;
            }
        }
    }
}