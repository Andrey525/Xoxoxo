using TicTacToeLib;

namespace Intellectual.Data
{
    public class IntellectStupid : IntellectBase
    {
        public IntellectStupid(ILogger<IntellectBase> logger, Game game) : base(logger, game) { }
        public override async Task<Tuple<int, int>> GetBestMoveCoord()
        {
            var availableFields = GetAvailableFields();
            Random random = new Random();
            int index = random.Next(0, availableFields.Count);
            return new Tuple<int, int>(availableFields[index].Row, availableFields[index].Col);
        }
    }
}
