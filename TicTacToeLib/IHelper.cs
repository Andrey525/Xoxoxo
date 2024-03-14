namespace TicTacToeLib
{
    public interface IHelper
    {
        /*Point GetHelp(State state);*/
        Task<Point> GetHelp(State state);
    }
}
