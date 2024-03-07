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
                for (int j = 0; j < TicTacToeTable.Size; j++)
                {
                    tableState.Values.Add((int)gameModel.Table[i, j]);
                }
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
