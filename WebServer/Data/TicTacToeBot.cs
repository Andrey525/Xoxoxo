using Intellectual;

namespace WebServer.Data
{
    public class TicTacToeBot
    {

        public TicTacToeBot(IntellectService.IntellectServiceClient client)
        {
            Client = client;
        }

        private IntellectService.IntellectServiceClient Client { get; set; }
        public async Task<Tuple<int, int>> ThinkAboutHowToMove(TicTacToeGame game)
        {
            var tableState = new TableState();
            for (int i = 0; i < game.Table.GetLength(0); i++)
            {
                var row = new RowState();
                for (int j = 0; j < game.Table.GetLength(1); j++)
                {
                    row.Values.Add((int)game.Table[i, j]);
                }
                tableState.Rows.Add(row);
            }
            var reply = await Client.CallToFriendAsync(tableState);
            // check status may be error
            return Tuple.Create(reply.Row, reply.Col);
        }
    }
}
