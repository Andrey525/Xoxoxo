using TicTacToeLib;

namespace Intellectual.Data
{
    public class IntellectStupid : IntellectBase
    {
        public IntellectStupid(ILogger<IntellectBase> logger, TicTacToeModel model) : base(logger, model) { }
        public override Tuple<int, int> GetBestMoveCoord()
        {
            var availableFields = GetAvailableFields(_model);
            Random random = new Random();
            int index = random.Next(0, availableFields.Count);
            return new Tuple<int, int>(availableFields[index].Row, availableFields[index].Col);
        }
    }
}
