using TicTacToeLib;
namespace Intellectual.Services.Helpers
{
    public static class ValueListConverter
    {
        public static void Fill(TicTacToeValue[,] values, Google.Protobuf.Collections.RepeatedField<int> list)
        {
            for (int i = 0; i < values.GetLength(0); i++)
            {
                for (int j = 0; j < values.GetLength(1); j++)
                {
                    values[i, j] = (TicTacToeValue)list[i * values.GetLength(0) + j];
                }
            }
        }
    }
}
