using Grpc.Core;
using Intellectual.Data;

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
            if (request.Values.Count != Table.Count)
            {
                _logger.LogError($"CallToFriend: Wrong count elems");
                return Task.FromResult(new CoordReply { Status = -1 });
            }

            var table = new Table();

            for (int i = 0; i < request.Values.Count; i++)
            {
                table[i / Table.Size, i % Table.Size] = (Value)request.Values[i];
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