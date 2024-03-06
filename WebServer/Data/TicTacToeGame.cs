namespace WebServer.Data
{
    public enum TicTacToeValues
    {
        Free,
        X,
        O
    }

    public enum TicTacToeGameState
    {
        WaitXMove,
        WaitOMove,
        XWin,
        OWin,
        Draw,
        Invalid
    }
    public class TicTacToeGame
    {
        public TicTacToeValues[,] Table { get; private set; }
        private const int _size = 3;
        private int _moveCount;
        private void UpdateGameState(int row, int col, TicTacToeValues lastValue)
        {
            for (int i = 0; i < _size; i++)
            {
                if (Table[row, i] != lastValue)
                    break;
                if (i == _size - 1)
                {
                    GameState = SetWinStateByValue(lastValue);
                    return;
                }
            }

            for (int i = 0; i < _size; i++)
            {
                if (Table[i, col] != lastValue)
                    break;
                if (i == _size - 1)
                {
                    GameState = SetWinStateByValue(lastValue);
                    return;
                }
            }

            for (int i = 0; i < _size; i++)
            {
                if (Table[i, i] != lastValue)
                    break;
                if (i == _size - 1)
                {
                    GameState = SetWinStateByValue(lastValue);
                    return;
                }
            }

            for (int i = 0; i < _size; i++)
            {
                if (Table[i, (_size - 1) - i] != lastValue)
                    break;
                if (i == _size - 1)
                {
                    GameState = SetWinStateByValue(lastValue);
                    return;
                }
            }

            if (_moveCount == _size * _size)
            {
                GameState = TicTacToeGameState.Draw;
                return;
            }

            GameState = (GameState == TicTacToeGameState.WaitXMove) ? TicTacToeGameState.WaitOMove
                                                                    : TicTacToeGameState.WaitXMove;
        }

        private TicTacToeGameState SetWinStateByValue(TicTacToeValues value)
        {
            switch (value)
            {
                case TicTacToeValues.X:
                    return TicTacToeGameState.XWin;
                case TicTacToeValues.O:
                    return TicTacToeGameState.OWin;
                default:
                    return TicTacToeGameState.Invalid;
            }
        }
        public TicTacToeGame()
        {
            Table = new TicTacToeValues[_size, _size];
            _moveCount = 0;
            GameState = TicTacToeGameState.WaitXMove;
        }
        public TicTacToeGameState GameState { get; private set; }
        public void MakeMove(int row, int col, TicTacToeValues value)
        {
            if (GameState != TicTacToeGameState.WaitXMove &&
                GameState != TicTacToeGameState.WaitOMove)
            {
                throw new Exception("Game was over. But you make move");
            }

            if ((row < 0 || row >= _size) ||
                (col < 0 || col >= _size) ||
                !Enum.IsDefined(typeof(TicTacToeValues), value))
            {
                throw new ArgumentException("Invalid agrument");
            }

            if (Table[row, col] != TicTacToeValues.Free)
            {
                throw new Exception("Cell is busy");
            }

            Table[row, col] = value;
            _moveCount++;
            UpdateGameState(row, col, value);
        }

    }
}
