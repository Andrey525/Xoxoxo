namespace TicTacToeLib
{
    public class Game
    {
        internal State _state = null;
        public int LineSize => _state.LineSize;

        public event Func<object, EventArgs, Task> BotMove;

        public bool IsStarted
        {
            get
            {
                return _state != null;
            }
        }
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

        public TicTacToeValue WhoseMove
        {
            get
            {
                if (!IsStarted || IsOvered)
                    return TicTacToeValue.No;

                switch (_state.ProgressState)
                {
                    case TicTacToeState.WaitXMove:
                        return TicTacToeValue.X;
                    case TicTacToeState.WaitOMove:
                        return TicTacToeValue.O;
                    default:
                        return TicTacToeValue.No;
                }
            }
        }

        public async Task Init(int lineSize)
        {
            if (!IsStarted)
            {
                _state = new State(lineSize);
                if (BotMove != null)
                {
                    await BotMove.Invoke(this, EventArgs.Empty);
                }
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

        public async Task FillCell(int row, int col, TicTacToeValue value)
        {
            if (_state.ProgressState != (TicTacToeState)value)
            {
                Console.WriteLine($"Can't do it");
                return;

            }

            if ((row < 0 || row >= _state.LineSize) ||
                (col < 0 || col >= _state.LineSize))
            {
                Console.WriteLine("Invalid agrument");
                return;
            }

            if (_state.Values[row, col] != TicTacToeValue.No)
            {
                Console.WriteLine("Cell is busy");
                return;
            }

            await UpdateGameState(row, col, value);
        }

        private async Task UpdateGameState(int row, int col, TicTacToeValue value)
        {
            _state.Values[row, col] = value;
            _state.CurrentMoveCount++;
            var winner = DetermineWinner(row, col, value);
            if (winner != TicTacToeValue.No)
            {
                _state.ProgressState = (winner == TicTacToeValue.O) ? TicTacToeState.OWin
                                                     : TicTacToeState.XWin;
                return;
            }

            if (_state.CurrentMoveCount == _state.TotalCellCount)
            {
                _state.ProgressState = TicTacToeState.Draw;
                return;
            }

            _state.ProgressState = (_state.ProgressState == TicTacToeState.WaitXMove) ? TicTacToeState.WaitOMove
                                                                                      : TicTacToeState.WaitXMove;
            // уведоми бота, чтобы тот походил
            if (BotMove != null)
            {
                await BotMove.Invoke(this, EventArgs.Empty);
            }
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
                Console.WriteLine($"Can't restore state, because {_state.LineSize} != {state.LineSize}");
                return;
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
    }
}
