using Intellect.Data;
using TicTacToeLib;
namespace Intellectual.Data
{
    public class Intellect : IntellectBase
    {
        public Intellect(ILogger<IntellectBase> logger, Game game) : base(logger, game) { }
        public override async Task<Point> GetBestMoveCoord()
        {
            Move result;
            try
            {
                result = await Minimax();
            }
            catch (Exception e)
            {
                _logger.LogError($"GetBestMoveCoord: {e.Message}");
                throw new Exception(e.Message);
            }
            return new Point(result.Coord.X, result.Coord.Y);
        }

        private async Task<Move> Minimax()
        {
            if (_game.IsOvered)
            {
                switch (_game.Winner)
                {
                    case TicTacToeValue.X:
                        return new Move { Score = -10 };
                    case TicTacToeValue.O:
                        return new Move { Score = 10 };
                    case TicTacToeValue.No:
                        return new Move { Score = 0 };
                    default:
                        break;
                }
            }

            var availableFields = GetAvailableFields();

            var moves = new List<Move>();

            TicTacToeState progressState = TicTacToeState.Invalid;
            for (int i = 0; i < availableFields.Count; i++)
            {
                State state = _game.SaveState();
                progressState = state.ProgressState;

                _game.UpdateGameState(availableFields[i].X, availableFields[i].Y, (TicTacToeValue)state.ProgressState);

                var result = await Minimax();

                _game.RestoreState(state);

                moves.Add(new Move()
                {
                    Coord = new Point(availableFields[i].X, availableFields[i].Y),
                    Score = result.Score
                });
            }

            int bestScore;
            if (progressState == TicTacToeState.WaitOMove)
            {
                bestScore = moves.Select(t => t.Score).Max();
            }
            else
            {
                bestScore = moves.Select(t => t.Score).Min();
            }

            // Some randomization
            var indexes = moves.Where(t => t.Score == bestScore);
            var randIndex = _random.Next(0, indexes.ToArray().Length);
            var bestMove = indexes.ToArray()[randIndex];
            //

            return bestMove;
        }
    }
}