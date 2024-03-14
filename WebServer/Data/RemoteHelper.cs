using Intellectual;
/*using Microsoft.AspNetCore.Components;*/
using TicTacToeLib;
namespace WebServer.Data
{
    public class RemoteHelper : IHelper
    {
        IntellectService.IntellectServiceClient Client { get; set; }

        public RemoteHelper(IntellectService.IntellectServiceClient client)
        {
            Client = client;
        }

        public async Task<Point> GetHelp(State state)
        {
            var tableState = new TableState();
            for (int i = 0; i < state.LineSize; i++)
            {
                for (int j = 0; j < state.LineSize; j++)
                {
                    tableState.Values.Add((int)state.Values[i, j]);
                }
            }
            tableState.Size = state.LineSize;
            tableState.MoveCount = state.CurrentMoveCount;
            tableState.State = (int)state.ProgressState;
            var reply = await Client.CallToFriendAsync(tableState);

            if (reply.Status == StatusCode.Error)
            {
                throw new Exception("Invalid reply");
            }

            return new Point { X = reply.CellCoord.Row, Y = reply.CellCoord.Col };
        }
    }
}
