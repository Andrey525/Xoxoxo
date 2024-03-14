using Grpc.Core;
using Intellectual.Services.Helpers;
using TicTacToeLib;

namespace Intellectual.Services
{
    public class Intellect : IntellectService.IntellectServiceBase
    {
        private readonly ILogger<Intellect> _logger;
        Game Game { get; set; }
        Data.Intellect DataIntellect { get; set; }
        Data.IntellectStupid DataIntellectStupid { get; set; }

        public Intellect(ILogger<Intellect> logger, Game game, Data.Intellect intellect, Data.IntellectStupid intellectStupid)
        {
            _logger = logger;
            _logger.LogInformation($"Intellect: new creation");
            Game = game;
            DataIntellect = intellect;
            DataIntellectStupid = intellectStupid;
        }

        public override async Task<CoordReply> CallToFriend(TableState request, ServerCallContext context)
        {
            if (request.Values.Count != request.Size * request.Size)
            {
                _logger.LogError($"CallToFriend: Wrong count elems");
                return (new CoordReply { Status = StatusCode.Error });
            }

            var values = new TicTacToeValue[request.Size, request.Size];
            ValueListConverter.Fill(values, request.Values);

            State state = new State(request.Size, request.MoveCount, (TicTacToeState)request.State, values);
            Game.Init(request.Size);
            Game.RestoreState(state);

            Tuple<int, int> coords;
            try
            {
                using ILoggerFactory factory = LoggerFactory.Create(builder => builder.AddConsole());
                var logger = factory.CreateLogger<Data.IntellectBase>();
                Data.IntellectBase intellect;
                if (request.Size == 3)
                {
                    intellect = DataIntellect;
                }
                else
                {
                    intellect = DataIntellectStupid;
                }
                coords = await intellect.GetBestMoveCoord();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return (new CoordReply { Status = StatusCode.Error });
            }

            _logger.LogInformation($"CallToFriend: row:{coords.Item1}; col:{coords.Item2};");

            return (new CoordReply
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