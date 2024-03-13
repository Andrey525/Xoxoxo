using TicTacToeLib;
namespace Intellectual.Services.Helpers
{
    public static class TableConverter
    {
        public static void Fill(this TicTacToeTable table, Google.Protobuf.Collections.RepeatedField<int> list)
        {
            for (int i = 0; i < table.Size; i++)
            {
                for (int j = 0; j < table.Size; j++)
                {
                    table[i, j] = (TicTacToeValue)list[i * table.Size + j];
                }
            }
        }
    }
}
