using Grpc.Core;
using Intellectual.Services.Helpers;
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
            if (request.Values.Count != request.Size * request.Size)
            {
                _logger.LogError($"CallToFriend: Wrong count elems");
                return Task.FromResult(new CoordReply { Status = StatusCode.Error });
            }

            var table = new TicTacToeTable(request.Size);
            table.Fill(request.Values);

            TicTacToeMemento memento = new TicTacToeMemento(request.MoveCount, (TicTacToeState)request.State, table);

            TicTacToeModel model = new TicTacToeModel(memento);

            Tuple<int, int> coords;
            try
            {
                using ILoggerFactory factory = LoggerFactory.Create(builder => builder.AddConsole());
                var logger = factory.CreateLogger<Data.Intellect>();
                var intellect = new Data.Intellect(logger, model);
                coords = intellect.GetBestMoveCoord();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return Task.FromResult(new CoordReply { Status = StatusCode.Error });
            }

            _logger.LogInformation($"CallToFriend: row:{coords.Item1}; col:{coords.Item2};");

            return Task.FromResult(new CoordReply
            {
                Status = StatusCode.Success,
                CellCoord = new Coord
                {
                    Row = coords.Item1,
                    Col = coords.Item2
                }
            });
        }
    }
}