namespace Intellectual.Data
{
    public class Table
    {
        public const int Size = 3;
        public const int Count = Size * Size;
        private Value[,] _values;
        public Value this[int row, int col]
        {
            get => _values[row, col];
            set => _values[row, col] = value;
        }
        public Table()
        {
            _values = new Value[Size, Size];
        }
    }
}
