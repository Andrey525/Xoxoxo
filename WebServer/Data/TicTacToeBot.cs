using Intellectual;
using TicTacToeLib;

namespace WebServer.Data
{
    public class TicTacToeBot
    {
        private IntellectService.IntellectServiceClient _client;

        public TicTacToeBot(IntellectService.IntellectServiceClient client)
        {
            _client = client;
        }

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
            var reply = await _client.CallToFriendAsync(tableState);

            if (reply.Status == StatusCode.Error)
            {
                throw new Exception("Invalid reply");
            }

            return Tuple.Create(reply.CellCoord.Row, reply.CellCoord.Col);
        }
    }
}
