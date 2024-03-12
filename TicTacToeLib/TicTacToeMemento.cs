namespace TicTacToeLib
{
    public class TicTacToeMemento
    {
        public int MoveCount { get; private set; }
        public TicTacToeState State { get; private set; }
        public TicTacToeTable Table { get; private set; }

        public TicTacToeMemento(int moveCount, TicTacToeState state, TicTacToeTable table)
        {
            MoveCount = moveCount;
            State = state;
            Table = table;
        }
    }
}
