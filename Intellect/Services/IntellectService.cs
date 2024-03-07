using Grpc.Core;
using TicTacToeLib;

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
            if (request.Values.Count != TicTacToeTable.Size * TicTacToeTable.Size)
            {
                _logger.LogError($"CallToFriend: Wrong count elems");
                return Task.FromResult(new CoordReply { Status = -1 });
            }

            var values = new List<TicTacToeValue>();

            for (int i = 0; i < request.Values.Count; i++)
            {
                values.Add((TicTacToeValue)request.Values[i]);
            }


            TicTacToeModel model = new TicTacToeModel((List<TicTacToeValue>)values);

            Tuple<int, int> coords;
            try
            {
                coords = Intellectual.Data.Intellect.GetBestMoveCoord(model);
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