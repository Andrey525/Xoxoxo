namespace TicTacToeLib
{
    public interface IHelper
    {
        Task<Point> GetHelp(State state);
    }
}
