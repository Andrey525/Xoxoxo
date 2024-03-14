namespace TicTacToeLib
{
    public enum TicTacToeValue
    {
        No,
        X,
        O
    }
    public enum TicTacToeState
    {
        WaitXMove = TicTacToeValue.X,
        WaitOMove = TicTacToeValue.O,
        XWin,
        OWin,
        Draw,
        Invalid
    }
}
