namespace TicTacToeLib
{
    public class Player
    {
        public Game Game { get; set; }
        public TicTacToeValue Value { get; set; }
        public async Task MakeMove(int row, int col)
        {
            await Game.FillCell(row, col, Value);
        }
    }
}
