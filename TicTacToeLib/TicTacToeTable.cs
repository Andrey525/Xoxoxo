namespace TicTacToeLib
{
    public class TicTacToeTable
    {
        public const int Size = 3;
        private TicTacToeValue[,] _values;
        public TicTacToeValue this[int row, int col]
        {
            get => _values[row, col];
            internal set
            {
                if (_values[row, col] == TicTacToeValue.No)
                {
                    _values[row, col] = value;
                }
                else
                {
                    throw new Exception($"The cell has content {_values[row, col]}");
                }
            }
        }
        public TicTacToeTable()
        {
            _values = new TicTacToeValue[Size, Size];
        }
    }
}
