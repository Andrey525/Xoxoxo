using TicTacToeLib;
namespace WebServer.Data
{
    public class LocalHelper : IHelper
    {
        Random _random = new Random();
        public Task<Point> GetHelp(State state)
        {
            int row, col;
            while (true)
            {
                row = _random.Next(0, state.LineSize);
                col = _random.Next(0, state.LineSize);
                if (state.Values[row, col] == TicTacToeValue.No)
                    break;
            }
            return Task.FromResult(new Point(row, col));
        }
    }
}
