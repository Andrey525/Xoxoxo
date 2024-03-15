using ConsoleClient.Data;
using Grpc.Net.Client;
using Intellectual;
using TicTacToeLib;

namespace ConsoleClient
{
    public static class Program
    {
        const int LineSize = 3;
        public static async Task Main()
        {
            using var channel = GrpcChannel.ForAddress("http://localhost:5291");
            var grpcClient = new IntellectService.IntellectServiceClient(channel);

            IHelper remoteHelper = new RemoteHelper(grpcClient);
            IHelper localHelper = new LocalHelper();
            var helpers = new List<IHelper> { remoteHelper, localHelper };
            FailoverBase failover = new Failover(helpers);

            Game game = new Game();
            Bot bot = new Bot(helpers, failover);
            Player player = new Player();

            Console.WriteLine("Do you want to play for X? <yes/no>");

            var answer = Console.ReadLine();
            if (answer?.ToLower() == "yes" || answer?.ToLower() == "y")
            {
                player.Value = TicTacToeValue.X;
                bot.Value = TicTacToeValue.O;
            }
            else
            {
                player.Value = TicTacToeValue.O;
                bot.Value = TicTacToeValue.X;
            }

            game.BotMove += bot.MakeMove;
            bot.Game = game;
            player.Game = game;
            bot.ChangeHelper(typeof(RemoteHelper));
            await game.Init(LineSize);


            PrintTable(game);
            while (!game.IsOvered)
            {
                Console.WriteLine("Enter row: ");
                var row = int.Parse(Console.ReadLine());
                Console.WriteLine("Enter col: ");
                var col = int.Parse(Console.ReadLine());
                await player.MakeMove(row, col);
                PrintTable(game);
            }
            Console.WriteLine($"Game over! Winner: {game.Winner}");
        }

        static void PrintTable(Game game)
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
    }

}
