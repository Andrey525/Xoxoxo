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
        public async Task<Tuple<int, int>> ThinkAboutHowToMove(TicTacToeMemento memento)
        {
            var tableState = new TableState();
            for (int i = 0; i < memento.Table.Size; i++)
            {
                for (int j = 0; j < memento.Table.Size; j++)
                {
                    tableState.Values.Add((int)memento.Table[i, j]);
                }
            }
            tableState.Size = memento.Table.Size;
            tableState.MoveCount = memento.MoveCount;
            tableState.State = (int)memento.State;
            var reply = await Client.CallToFriendAsync(tableState);

            if (reply.Status == StatusCode.Error)
            {
                throw new Exception("Invalid reply");
            }

            return Tuple.Create(reply.CellCoord.Row, reply.CellCoord.Col);
        }
    }
}
