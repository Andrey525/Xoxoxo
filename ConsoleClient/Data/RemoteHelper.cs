using Intellectual;
using TicTacToeLib;
namespace ConsoleClient.Data
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
            var gameState = new GameState();
            for (int i = 0; i < state.LineSize; i++)
            {
                for (int j = 0; j < state.LineSize; j++)
                {
                    gameState.Values.Add((int)state.Values[i, j]);
                }
            }
            gameState.Size = state.LineSize;
            gameState.MoveCount = state.CurrentMoveCount;
            gameState.State = (int)state.ProgressState;
            var reply = await Client.GetMoveCoordinatesAsync(gameState);

            if (reply.Status == CoordinatesReply.Types.StatusCode.Error)
            {
                throw new Exception($"Invalid reply: {reply.ErrorReason}");
            }

            return new Point { X = reply.CellCoordinates.Row, Y = reply.CellCoordinates.Col };
        }
    }
}
