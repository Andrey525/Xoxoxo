using Microsoft.Extensions.Logging;

namespace TicTacToeLib
{
    public class Game : IDisposable
    {
        private readonly ILogger<Game> _logger;
        internal State _state;
        public int LineSize => _state.LineSize;

        public event MoveCallback? XMove;
        public event MoveCallback? OMove;
        public delegate Task<Point> MoveCallback();

        public event UpdateComponentCallback? GameOver;
        public event UpdateComponentCallback? GameStateUpdate;
        public delegate void UpdateComponentCallback();

        public Game(ILogger<Game> logger)
        {
            _logger = logger;
            _state = new State(3);
            IsStarted = false;
        }

        public bool IsStarted { get; private set; }
        public bool IsOvered
        {
            get
            {
                if (!IsStarted)
                    return false;

                if (_state.ProgressState != TicTacToeState.WaitXMove &&
                    _state.ProgressState != TicTacToeState.WaitOMove)
                {
                    return true;
                }
                else
                    return false;
            }
        }

        public void Init(int lineSize)
        {
            if (!IsStarted)
            {
                _state = new State(lineSize);
                IsStarted = true;
            }
        }
        public TicTacToeValue GetValue(int row, int col)
        {
            return _state.Values[row, col];
        }

        public TicTacToeValue Winner
        {
            get
            {
                switch (_state.ProgressState)
                {
                    case TicTacToeState.XWin:
                        return TicTacToeValue.X;
                    case TicTacToeState.OWin:
                        return TicTacToeValue.O;
                    default:
                        return TicTacToeValue.No;
                }
            }
        }

        public async Task Run()
        {
            if (XMove == null || OMove == null)
            {
                return;
            }
            while (!IsOvered)
            {
                TicTacToeValue moveValue = (_state.ProgressState == TicTacToeState.WaitXMove) ? TicTacToeValue.X : TicTacToeValue.O;

                Point coordinates = await ((moveValue == TicTacToeValue.X) ? XMove.Invoke() : OMove.Invoke());

                UpdateGameState(coordinates.X, coordinates.Y, moveValue);
            }
            GameOver?.Invoke();
        }

        private bool ValidateMove(int row, int col, TicTacToeValue value)
        {
            if (_state.ProgressState != (TicTacToeState)value)
            {
                _logger.LogWarning("Progress State does not imply this meaning at the moment");
                return false;
            }

            if ((row < 0 || row >= _state.LineSize) ||
                (col < 0 || col >= _state.LineSize))
            {
                _logger.LogError("Invalid agrument");
                return false;
            }

            if (_state.Values[row, col] != TicTacToeValue.No)
            {
                _logger.LogError($"Cell {row}:{col} is busy (value:{_state.Values[row, col]})");
                return false;
            }

            return true;
        }

        public void UpdateGameState(int row, int col, TicTacToeValue value)
        {
            bool isValidMove = ValidateMove(row, col, value);
            if (!isValidMove)
            {
                return;
            }

            _state.Values[row, col] = value;
            _state.CurrentMoveCount++;
            var winner = DetermineWinner(row, col, value);
            if (winner != TicTacToeValue.No)
            {
                _state.ProgressState = (winner == TicTacToeValue.O) ? TicTacToeState.OWin
                                                     : TicTacToeState.XWin;
                GameStateUpdate?.Invoke();
                return;
            }

            if (_state.CurrentMoveCount == _state.TotalCellCount)
            {
                _state.ProgressState = TicTacToeState.Draw;
                GameStateUpdate?.Invoke();
                return;
            }

            _state.ProgressState = (_state.ProgressState == TicTacToeState.WaitXMove) ? TicTacToeState.WaitOMove
                                                                                      : TicTacToeState.WaitXMove;
            GameStateUpdate?.Invoke();
        }

        private TicTacToeValue DetermineWinner(int row, int col, TicTacToeValue value)
        {
            for (int i = 0; i < _state.LineSize; i++)
            {
                if (_state.Values[row, i] != value)
                    break;
                if (i == _state.LineSize - 1)
                    return value;
            }

            for (int i = 0; i < _state.LineSize; i++)
            {
                if (_state.Values[i, col] != value)
                    break;
                if (i == _state.LineSize - 1)
                    return value;
            }

            for (int i = 0; i < _state.LineSize; i++)
            {
                if (_state.Values[i, i] != value)
                    break;
                if (i == _state.LineSize - 1)
                    return value;
            }

            for (int i = 0; i < _state.LineSize; i++)
            {
                if (_state.Values[i, (_state.LineSize - 1) - i] != value)
                    break;
                if (i == _state.LineSize - 1)
                    return value;
            }
            return TicTacToeValue.No;
        }

        private TicTacToeValue DetermineWinner(TicTacToeValue value)
        {
            for (int i = 0; i < _state.LineSize; i++)
            {
                for (int j = 0; j < _state.LineSize; j++)
                {
                    if (_state.Values[i, j] != value)
                        break;
                    if (j == _state.LineSize - 1)
                        return value;
                }
            }

            for (int i = 0; i < _state.LineSize; i++)
            {
                for (int j = 0; j < _state.LineSize; j++)
                {
                    if (_state.Values[j, i] != value)
                        break;
                    if (j == _state.LineSize - 1)
                        return value;
                }
            }

            for (int i = 0; i < _state.LineSize; i++)
            {
                if (_state.Values[i, i] != value)
                    break;
                if (i == _state.LineSize - 1)
                    return value;
            }

            for (int i = 0; i < _state.LineSize; i++)
            {
                if (_state.Values[i, (_state.LineSize - 1) - i] != value)
                    break;
                if (i == _state.LineSize - 1)
                    return value;
            }
            return TicTacToeValue.No;
        }

        public void RestoreState(State state)
        {
            if (_state.LineSize != state.LineSize)
            {
                _logger.LogError($"Can't restore state, because line sizes not equal ({_state.LineSize} != {state.LineSize})");
                throw new Exception("Not supported input state");
            }
            _state = new State(state);
            try
            {
                ValidateState();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public State SaveState()
        {
            return new State(_state);
        }

        private void ValidateState()
        {
            int Xcount = 0;
            int Ocount = 0;
            int freeCount = 0;
            for (int i = 0; i < _state.LineSize; i++)
            {
                for (int j = 0; j < _state.LineSize; j++)
                {
                    if (!Enum.IsDefined(typeof(TicTacToeValue), _state.Values[i, j]))
                    {
                        throw new ArgumentException("Invalid value in table");
                    }

                    if (_state.Values[i, j] == TicTacToeValue.X)
                        Xcount++;
                    else if (_state.Values[i, j] == TicTacToeValue.O)
                        Ocount++;
                    else
                        freeCount++;
                }
            }

            if ((freeCount == 0) ||
                (_state.CurrentMoveCount != _state.TotalCellCount - freeCount) ||
                ((Xcount != Ocount) && (Xcount != Ocount + 1)) ||
                (_state.ProgressState != TicTacToeState.WaitXMove && _state.ProgressState != TicTacToeState.WaitOMove) ||
                (Xcount == Ocount && _state.ProgressState != TicTacToeState.WaitXMove) ||
                (Xcount == Ocount + 1 && _state.ProgressState != TicTacToeState.WaitOMove))
            {
                throw new Exception("Invalid State");
            }

            if (TicTacToeValue.No != DetermineWinner(TicTacToeValue.X) ||
                TicTacToeValue.No != DetermineWinner(TicTacToeValue.O))
            {
                throw new Exception("Already have winner. Or may be invalid State");
            }
        }

        public void Dispose()
        {
            XMove = null;
            OMove = null;
            GameOver = null;
            GameStateUpdate = null;
        }
    }
}
