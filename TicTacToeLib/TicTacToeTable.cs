namespace TicTacToeLib
{
    public class TicTacToeTable
    {
        public readonly int Size;
        protected TicTacToeValue[,] _values;
        public TicTacToeValue this[int row, int col]
        {
            get => _values[row, col];
            set
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
        public TicTacToeTable(int size)
        {
            Size = size;
            _values = new TicTacToeValue[Size, Size];
        }

        public TicTacToeTable Clone()
        {
            var newTable = new TicTacToeTable(Size);
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    newTable[i, j] = _values[i, j];
                }
            }
            return newTable;
        }
    }
}
