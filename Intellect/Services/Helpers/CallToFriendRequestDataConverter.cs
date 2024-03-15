using TicTacToeLib;
namespace Intellectual.Services.Helpers
{
    public static class GetMoveCoordinatesRequestDataConverter
    {
        static public async void RestoreGameDataFromRequest(Game game, GameState request)
        {
            var values = new TicTacToeValue[request.Size, request.Size];

            for (int i = 0; i < values.GetLength(0); i++)
            {
                for (int j = 0; j < values.GetLength(1); j++)
                {
                    values[i, j] = (TicTacToeValue)request.Values[i * values.GetLength(0) + j];
                }
            }

            State state = new State(request.Size, request.MoveCount, (TicTacToeState)request.State, values);

            await game.Init(request.Size);
            game.RestoreState(state);
        }
    }
}
