using Intellectual;
using TicTacToeLib;

namespace WebServer.Data
{
    public class TicTacToeBot
    {

        public TicTacToeBot(IntellectService.IntellectServiceClient client)
        {
            Client = client;
        }

        private IntellectService.IntellectServiceClient Client { get; set; }
        public async Task<Tuple<int, int>> ThinkAboutHowToMove(TicTacToeModel gameModel)
        {
            var tableState = new TableState();
            for (int i = 0; i < TicTacToeTable.Size; i++)
            {
                var row = new RowState();
                for (int j = 0; j < TicTacToeTable.Size; j++)
                {
                    row.Values.Add((int)gameModel.Table[i, j]);
                }
                tableState.Rows.Add(row);
            }
            var reply = await Client.CallToFriendAsync(tableState);

            if (reply.Status == -1)
            {
                throw new Exception("Invalid reply");
            }

            return Tuple.Create(reply.Row, reply.Col);
        }
    }
}
