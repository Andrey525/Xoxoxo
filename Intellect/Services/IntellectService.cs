using Grpc.Core;
using Intellectual.Services.Helpers;
using TicTacToeLib;

namespace Intellectual.Services
{
    public class Intellect : IntellectService.IntellectServiceBase
    {
        private readonly ILogger<Intellect> _logger;
        private readonly Game _game;
        private readonly IEnumerable<Data.IntellectBase> _intellects;

        public Intellect(ILogger<Intellect> logger, Game game, IEnumerable<Data.IntellectBase> intellects)
        {
            _logger = logger;
            _game = game;
            _intellects = intellects;

            _logger.LogInformation($"Intellect: new creation");
        }

        public override async Task<CoordinatesReply> GetMoveCoordinates(GameState request, ServerCallContext context)
        {
            if (request.Values.Count != request.Size * request.Size)
            {
                _logger.LogError($"GetMoveCoordinates: Wrong count elems");
                return new CoordinatesReply
                {
                    Status = CoordinatesReply.Types.StatusCode.Error,
                    ErrorReason = "Wrong count elems"
                };
            }

            Point coords;
            try
            {
                GetMoveCoordinatesRequestDataConverter.RestoreGameDataFromRequest(_game, request);

                Data.IntellectBase? intellect = null;
                foreach (var elem in _intellects)
                {
                    if (request.Size == 3 && elem is Data.Intellect)
                    {
                        intellect = elem;
                        break;
                    }
                    else if (elem is Data.IntellectStupid)
                    {
                        intellect = elem;
                        break;
                    }
                }

                if (intellect == null)
                {
                    throw new Exception("Needed intellect object not found");
                }

                coords = await intellect.GetBestMoveCoord();
            }
            catch (Exception e)
            {
                _logger.LogError($"GetMoveCoordinates: {e.Message}");
                return new CoordinatesReply
                {
                    Status = CoordinatesReply.Types.StatusCode.Error,
                    ErrorReason = e.Message
                };
            }

            _logger.LogInformation($"GetMoveCoordinates: row:{coords.X}; col:{coords.Y};");

            return new CoordinatesReply
            {
                Status = CoordinatesReply.Types.StatusCode.Success,
                CellCoordinates = new Coordinates
                {
                    Row = coords.X,
                    Col = coords.Y
                }
            };
        }
    }
}