using TicTacToeLib;

namespace Intellectual.Data
{
    public class IntellectStupid : IntellectBase
    {
        public IntellectStupid(ILogger<IntellectBase> logger, Game game) : base(logger, game) { }
        public override Task<Point> GetBestMoveCoord()
        {
            var availableFields = GetAvailableFields();
            int index = _random.Next(0, availableFields.Count);
            return Task.FromResult(new Point(availableFields[index].X, availableFields[index].Y));
        }
    }
}
