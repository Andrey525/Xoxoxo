namespace TicTacToeLib
{
    public class TicTacToeModel
    {
        private int _moveCount;
        private TicTacToeTable _table;
        private int MaxMoveCount { get => TableSize * TableSize; }
        public int TableSize { get => _table.Size; }
        public TicTacToeState State { get; private set; }

        public TicTacToeModel(TicTacToeValue firstMoveValue, int tableSize)
        {
            _moveCount = 0;
            _table = new TicTacToeTable(tableSize);
            State = (firstMoveValue == TicTacToeValue.O) ? TicTacToeState.WaitOMove
                                                         : TicTacToeState.WaitXMove;
        }

        public TicTacToeModel(TicTacToeMemento memento)
        {
            RestoreState(memento);
        }

        public TicTacToeValue GetValue(int row, int col)
        {
            return _table[row, col];
        }

        public TicTacToeMemento SaveState()
        {
            return new TicTacToeMemento(_moveCount, State, _table.Clone());
        }

        public void RestoreState(TicTacToeMemento memento)
        {
            _moveCount = memento.MoveCount;
            State = memento.State;
            _table = memento.Table.Clone();
            try
            {
                ValidateState();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void MakeMove(int row, int col, TicTacToeValue value)
        {
            if (State != TicTacToeState.WaitXMove &&
                State != TicTacToeState.WaitOMove)
            {
                throw new Exception("Game was over. But you make move");
            }

            if ((row < 0 || row >= TableSize) ||
                (col < 0 || col >= TableSize) ||
                !Enum.IsDefined(typeof(TicTacToeValue), value))
            {
                throw new ArgumentException("Invalid agrument");
            }

            _table[row, col] = value;
            _moveCount++;
            UpdateGameState(row, col, value);
        }

        private void ValidateState()
        {
            int Xcount = 0;
            int Ocount = 0;
            int freeCount = 0;
            for (int i = 0; i < TableSize; i++)
            {
                for (int j = 0; j < TableSize; j++)
                {
                    if (!Enum.IsDefined(typeof(TicTacToeValue), _table[i, j]))
                    {
                        throw new ArgumentException("Invalid value in table");
                    }

                    if (_table[i, j] == TicTacToeValue.X)
                        Xcount++;
                    else if (_table[i, j] == TicTacToeValue.O)
                        Ocount++;
                    else
                        freeCount++;
                }
            }

            if ((freeCount == 0) ||
                (_moveCount != TableSize * TableSize - freeCount) ||
                ((Xcount != Ocount) && (Xcount != Ocount + 1)) ||
                (State != TicTacToeState.WaitXMove && State != TicTacToeState.WaitOMove) ||
                (Xcount == Ocount && State != TicTacToeState.WaitXMove) ||
                (Xcount == Ocount + 1 && State != TicTacToeState.WaitOMove))
            {
                throw new Exception("Invalid State");
            }

            if (TicTacToeValue.No != DetermineWinner(TicTacToeValue.X) ||
                TicTacToeValue.No != DetermineWinner(TicTacToeValue.O))
            {
                throw new Exception("Already have winner. Or may be invalid State");
            }
        }

        private void UpdateGameState(int row, int col, TicTacToeValue lastValue)
        {
            var winner = DetermineWinner(row, col, lastValue);
            if (winner != TicTacToeValue.No)
            {
                State = (winner == TicTacToeValue.O) ? TicTacToeState.OWin
                                                     : TicTacToeState.XWin;
                return;
            }

            if (_moveCount == MaxMoveCount)
            {
                State = TicTacToeState.Draw;
                return;
            }

            State = (State == TicTacToeState.WaitXMove) ? TicTacToeState.WaitOMove
                                                        : TicTacToeState.WaitXMove;
        }

        private TicTacToeValue DetermineWinner(int row, int col, TicTacToeValue value)
        {
            for (int i = 0; i < TableSize; i++)
            {
                if (_table[row, i] != value)
                    break;
                if (i == TableSize - 1)
                    return value;
            }

            for (int i = 0; i < TableSize; i++)
            {
                if (_table[i, col] != value)
                    break;
                if (i == TableSize - 1)
                    return value;
            }

            for (int i = 0; i < TableSize; i++)
            {
                if (_table[i, i] != value)
                    break;
                if (i == TableSize - 1)
                    return value;
            }

            for (int i = 0; i < TableSize; i++)
            {
                if (_table[i, (TableSize - 1) - i] != value)
                    break;
                if (i == TableSize - 1)
                    return value;
            }
            return TicTacToeValue.No;
        }

        private TicTacToeValue DetermineWinner(TicTacToeValue value)
        {
            for (int i = 0; i < TableSize; i++)
            {
                for (int j = 0; j < TableSize; j++)
                {
                    if (_table[i, j] != value)
                        break;
                    if (j == TableSize - 1)
                        return value;
                }
            }

            for (int i = 0; i < TableSize; i++)
            {
                for (int j = 0; j < TableSize; j++)
                {
                    if (_table[j, i] != value)
                        break;
                    if (j == TableSize - 1)
                        return value;
                }
            }

            for (int i = 0; i < TableSize; i++)
            {
                if (_table[i, i] != value)
                    break;
                if (i == TableSize - 1)
                    return value;
            }

            for (int i = 0; i < TableSize; i++)
            {
                if (_table[i, (TableSize - 1) - i] != value)
                    break;
                if (i == _table.Size - 1)
                    return value;
            }
            return TicTacToeValue.No;
        }
    }
}