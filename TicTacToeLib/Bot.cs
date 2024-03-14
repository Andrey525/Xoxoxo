namespace TicTacToeLib
{
    public class Bot
    {
        public Game Game { get; set; }
        private IHelper Helper { get; set; }
        public TicTacToeValue Value { get; set; }

        public Bot(IHelper helper)
        {
            Helper = helper;
        }

        private async Task<Point> GetHelp()
        {
            return await Helper.GetHelp(new State(Game._state));
        }

        public async Task MakeMove(object sender, EventArgs e)
        {
            if ((TicTacToeValue)Game._state.ProgressState == Value)
            {
                var cellCoordinates = await GetHelp();
                await Game.FillCell(cellCoordinates.X, cellCoordinates.Y, Value);
            }
        }
    }
}
