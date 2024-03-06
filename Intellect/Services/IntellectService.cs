using Grpc.Core;

namespace Intellectual.Services
{
    public class Intellect : IntellectService.IntellectServiceBase
    {
        private readonly ILogger<Intellect> _logger;

        public Intellect(ILogger<Intellect> logger)
        {
            _logger = logger;
            _logger.LogInformation($"Intellect: new creation");
        }

        public override Task<CoordReply> CallToFriend(TableState request, ServerCallContext context)
        {
            const int size = Intellectual.Data.Intellect.Size;
            var table = new int[size, size];

            if (request.Rows.Count != size)
            {
                _logger.LogError($"CallToFriend: Wrong rows count");
                return Task.FromResult(new CoordReply { Status = -1 });
            }

            for (int i = 0; i < size; i++)
            {
                if (request.Rows[i].Values.Count != size)
                {
                    _logger.LogError($"CallToFriend: Wrong values count");
                    return Task.FromResult(new CoordReply { Status = -1 });
                }

                for (int j = 0; j < size; j++)
                {
                    table[i, j] = request.Rows[i].Values[j];
                }
            }

            Tuple<int, int> coords;
            try
            {
                coords = Intellectual.Data.Intellect.GetBestMoveCoord(table);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return Task.FromResult(new CoordReply { Status = -1 });
            }

            _logger.LogInformation($"CallToFriend: row:{coords.Item1}; col:{coords.Item2};");

            return Task.FromResult(new CoordReply
            {
                Row = coords.Item1,
                Col = coords.Item2
            });
        }
    }
}