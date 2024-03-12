namespace TicTacToeLib
{
    public class TicTacToeModel
    {
        private int _moveCount;
        private TicTacToeTable _table;
        public TicTacToeState State { get; private set; }

        public TicTacToeModel(TicTacToeValue firstMoveValue, int tableSize)
        {
            _moveCount = 0;
            _table = new TicTacToeTable(tableSize);
            State = SetWaitStateByValue(firstMoveValue);
        }

        public TicTacToeModel(TicTacToeMemento memento)
        {
            _moveCount = memento.MoveCount;
            State = memento.State;
            _table = memento.Table.Clone();
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
        }

        public void MakeMove(int row, int col, TicTacToeValue value)
        {
            if (State != TicTacToeState.WaitXMove &&
                State != TicTacToeState.WaitOMove)
            {
                throw new Exception("Game was over. But you make move");
            }

            if ((row < 0 || row >= _table.Size) ||
                (col < 0 || col >= _table.Size) ||
                !Enum.IsDefined(typeof(TicTacToeValue), value))
            {
                throw new ArgumentException("Invalid agrument");
            }

            _table[row, col] = value;
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

            if (_moveCount == _table.Size * _table.Size)
            {
                State = TicTacToeState.Draw;
                return;
            }

            State = (State == TicTacToeState.WaitXMove) ? TicTacToeState.WaitOMove
                                                        : TicTacToeState.WaitXMove;
        }

        private TicTacToeValue DetermineWinner(int row, int col, TicTacToeValue value)
        {
            for (int i = 0; i < _table.Size; i++)
            {
                if (_table[row, i] != value)
                    break;
                if (i == _table.Size - 1)
                    return value;
            }

            for (int i = 0; i < _table.Size; i++)
            {
                if (_table[i, col] != value)
                    break;
                if (i == _table.Size - 1)
                    return value;
            }

            for (int i = 0; i < _table.Size; i++)
            {
                if (_table[i, i] != value)
                    break;
                if (i == _table.Size - 1)
                    return value;
            }

            for (int i = 0; i < _table.Size; i++)
            {
                if (_table[i, (_table.Size - 1) - i] != value)
                    break;
                if (i == _table.Size - 1)
                    return value;
            }
            return TicTacToeValue.No;
        }

        private TicTacToeValue DetermineWinner(TicTacToeValue value)
        {
            for (int i = 0; i < _table.Size; i++)
            {
                for (int j = 0; j < _table.Size; j++)
                {
                    if (_table[i, j] != value)
                        break;
                    if (j == _table.Size - 1)
                        return value;
                }
            }

            for (int i = 0; i < _table.Size; i++)
            {
                for (int j = 0; j < _table.Size; j++)
                {
                    if (_table[j, i] != value)
                        break;
                    if (j == _table.Size - 1)
                        return value;
                }
            }

            for (int i = 0; i < _table.Size; i++)
            {
                if (_table[i, i] != value)
                    break;
                if (i == _table.Size - 1)
                    return value;
            }

            for (int i = 0; i < _table.Size; i++)
            {
                if (_table[i, (_table.Size - 1) - i] != value)
                    break;
                if (i == _table.Size - 1)
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