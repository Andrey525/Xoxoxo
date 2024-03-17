using ConsoleClient.Data;
using Grpc.Net.Client;
using Intellectual;
using Microsoft.Extensions.Logging;
using TicTacToeLib;

namespace ConsoleClient
{
    public static class Program
    {

        public static async Task Main()
        {
            Battle battle = new Battle();
            await battle.Run();
        }
    }

    public class Battle
    {
        const int LineSize = 3;
        Game game;
        Bot bot;
        Task<Point> PlayerInterractionHandler()
        {
            Console.WriteLine("Enter row: ");
            var row = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter col: ");
            var col = int.Parse(Console.ReadLine());
            return Task.FromResult(new Point(row, col));
        }

        Task<Point> BotMoveHandler()
        {
            Console.WriteLine("Bot move:");
            return bot.MakeMove();
        }

        void PrintTable()
        {
            for (int i = 0; i < game.LineSize; i++)
            {
                for (int j = 0; j < game.LineSize; j++)
                {
                    Console.Write($" | {game.GetValue(i, j)} | ");
                }
                Console.WriteLine();
            }
        }

        public async Task Run()
        {
            using var channel = GrpcChannel.ForAddress("http://localhost:5291");
            var grpcClient = new IntellectService.IntellectServiceClient(channel);

            IHelper remoteHelper = new RemoteHelper(grpcClient);
            IHelper localHelper = new LocalHelper();
            var helpers = new List<IHelper> { remoteHelper, localHelper };
            FailoverBase failover = new Failover(helpers);

            using ILoggerFactory factory = LoggerFactory.Create(builder => builder.AddConsole());
            var logger = factory.CreateLogger<Game>();

            game = new Game(logger);
            bot = new Bot(helpers, failover);
            bot.Game = game;

            Console.WriteLine("Do you want to play for X? <yes/no>");

            var answer = Console.ReadLine();
            if (answer?.ToLower() == "yes" || answer?.ToLower() == "y")
            {
                game.XMove += PlayerInterractionHandler;
                game.OMove += BotMoveHandler;
            }
            else
            {
                game.OMove += PlayerInterractionHandler;
                game.XMove += BotMoveHandler;
            }

            bot.ChangeHelper(typeof(RemoteHelper));
            game.GameStateUpdate += PrintTable;
            game.GameOver += () => Console.WriteLine($"Game over! Winner: {game.Winner}");
            game.GameOver += PrintTable;
            game.Init(LineSize);
            await game.Run();
        }
    }

}
