using Grpc.Net.Client;
using Intellectual;

namespace WinFormsClient
{
    internal static class Program
    {


        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            using var channel = GrpcChannel.ForAddress("http://localhost:5291");
            var grpcClient = new IntellectService.IntellectServiceClient(channel);
            ApplicationConfiguration.Initialize();
            Application.Run(new TicTacToe3x3(grpcClient));
        }
    }
}