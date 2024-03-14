using TicTacToeLib;
namespace Intellectual.Data
{
    public class Intellect : IntellectBase
    {
        public Intellect(ILogger<IntellectBase> logger, Game game) : base(logger, game) { }
        public override async Task<Tuple<int, int>> GetBestMoveCoord()
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
            return Tuple.Create(result.Coord.Row, result.Coord.Col);
        }

        /* 
         * При таблице больше чем 3x3 увеличивается кол-во комбинаций в разы!!!
         * Метод нуу очень долго обрабатывает все комбинации.
         * Нужна оптимизация.
         */
        private async Task<Move> Minimax()
        {
            if (_game._state.ProgressState == TicTacToeState.XWin)
            {
                return new Move { Score = -10 };
            }
            else if (_game._state.ProgressState == TicTacToeState.OWin)
            {
                return new Move { Score = 10 };
            }
            else if (_game._state.ProgressState == TicTacToeState.Draw)
            {
                return new Move { Score = 0 };
            }

            var availableFields = GetAvailableFields();

            var moves = new List<Move>();

            TicTacToeState progressState = TicTacToeState.Invalid;
            for (int i = 0; i < availableFields.Count; i++)
            {
                State state = _game.SaveState();
                progressState = state.ProgressState;

                await _game.FillCell(availableFields[i].Row, availableFields[i].Col, (TicTacToeValue)state.ProgressState);

                var result = await Minimax();

                _game.RestoreState(state);

                moves.Add(new Move()
                {
                    Coord = new Coord()
                    {
                        Col = availableFields[i].Col,
                        Row = availableFields[i].Row
                    },
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

            Random random = new Random();
            var randIndex = random.Next(0, indexes.ToArray().Length);
            var bestMove = indexes.ToArray()[randIndex];
            //

            return bestMove;
        }
    }
}