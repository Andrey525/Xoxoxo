using TicTacToeLib;
namespace Intellectual.Data
{
    public class Intellect : IntellectBase
    {
        public Intellect(ILogger<IntellectBase> logger, TicTacToeModel model) : base(logger, model) { }
        public override Tuple<int, int> GetBestMoveCoord()
        {
            Move result;
            try
            {
                result = Minimax(_model);
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
        private Move Minimax(TicTacToeModel model)
        {
            if (model.State == TicTacToeState.XWin)
            {
                return new Move { Score = -10 };
            }
            else if (model.State == TicTacToeState.OWin)
            {
                return new Move { Score = 10 };
            }
            else if (model.State == TicTacToeState.Draw)
            {
                return new Move { Score = 0 };
            }

            var availableFields = GetAvailableFields(model);

            var moves = new List<Move>();

            TicTacToeState state = TicTacToeState.Invalid;
            for (int i = 0; i < availableFields.Count; i++)
            {
                TicTacToeMemento memento = model.SaveState();
                state = memento.State;

                model.MakeMove(availableFields[i].Row, availableFields[i].Col, (TicTacToeValue)model.State);

                var result = Minimax(model);

                model.RestoreState(memento);

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
            if (state == TicTacToeState.WaitOMove)
            {
                bestScore = moves.Select(t => t.Score).Max();
            }
            else
            {
                bestScore = moves.Select(t => t.Score).Min();
            }

            // Some randomization
            var indexes = moves.Select(t => t).Where(t => t.Score == bestScore);

            Random random = new Random();
            var randIndex = random.Next(0, indexes.ToArray().Length);
            var bestMove = indexes.ToArray()[randIndex];
            //

            return bestMove;
        }
    }
}