namespace TicTacToeLib
{
    public class State
    {
        public readonly int LineSize;
        public TicTacToeValue[,] Values { get; set; }
        public int CurrentMoveCount { get; set; }
        public int TotalCellCount => LineSize * LineSize;
        public TicTacToeState ProgressState { get; set; }
        public State(int lineSize)
        {
            LineSize = lineSize;
            CurrentMoveCount = 0;
            ProgressState = TicTacToeState.WaitXMove;
            Values = new TicTacToeValue[LineSize, LineSize];
        }

        public State(State state)
        {
            LineSize = state.LineSize;
            ProgressState = state.ProgressState;
            CurrentMoveCount = state.CurrentMoveCount;
            Values = new TicTacToeValue[LineSize, LineSize];
            for (int i = 0; i < LineSize; i++)
            {
                for (int j = 0; j < LineSize; j++)
                {
                    Values[i, j] = state.Values[i, j];
                }
            }
        }

        public State(int lineSize, int currentMoveCount, TicTacToeState progressState, TicTacToeValue[,] values)
        {
            LineSize = lineSize;
            CurrentMoveCount = currentMoveCount;
            ProgressState = progressState;
            Values = new TicTacToeValue[LineSize, LineSize];
            for (int i = 0; i < LineSize; i++)
            {
                for (int j = 0; j < LineSize; j++)
                {
                    Values[i, j] = values[i, j];
                }
            }
        }
    }
}
