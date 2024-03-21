using Intellectual;
using Microsoft.Extensions.Logging;
using TicTacToeLib;
using WinFormsClient.Data;
namespace WinFormsClient
{
    public partial class TicTacToe3x3 : Form
    {
        const int LineSize = 3;
        TaskCompletionSource tcs;
        Game game;
        Bot bot;
        int _nextMoveRow;
        int _nextMoveCol;
        public TicTacToe3x3(IntellectService.IntellectServiceClient grpcClient)
        {
            InitializeComponent();
            button1.Click += new EventHandler(button1_Click);
            button2.Click += new EventHandler(button2_Click);
            button3.Click += new EventHandler(button3_Click);
            button4.Click += new EventHandler(button4_Click);
            button5.Click += new EventHandler(button5_Click);
            button6.Click += new EventHandler(button6_Click);
            button7.Click += new EventHandler(button7_Click);
            button8.Click += new EventHandler(button8_Click);
            button9.Click += new EventHandler(button9_Click);

            IHelper remoteHelper = new RemoteHelper(grpcClient);
            IHelper localHelper = new LocalHelper();
            var helpers = new List<IHelper> { remoteHelper, localHelper };
            FailoverBase failover = new Failover(helpers);

            using ILoggerFactory factory = LoggerFactory.Create(builder => builder.AddConsole());
            var logger = factory.CreateLogger<Game>();

            game = new Game(logger);
            bot = new Bot(helpers, failover);
            bot.Game = game;
            bot.ChangeHelper(typeof(RemoteHelper));

            game.XMove += MakeMove;
            game.OMove += bot.MakeMove;
            game.GameStateUpdate += RefreshButtons;

            tcs = new TaskCompletionSource();

            game.Init(LineSize);
            game.Run();
        }

        private async Task<TicTacToeLib.Point> MakeMove()
        {
            /* 
             * Since this method is called immediately when the event occurs.
             * We should wait for the user to click the button.
             */
            await tcs.Task;
            tcs = new TaskCompletionSource();

            return new TicTacToeLib.Point(_nextMoveRow, _nextMoveCol);
        }

        private void RefreshButtons()
        {
            if (game.GetValue(0, 0) != TicTacToeValue.No)
            {
                button1.Text = game.GetValue(0, 0).ToString();
            }
            if (game.GetValue(0, 1) != TicTacToeValue.No)
            {
                button2.Text = game.GetValue(0, 1).ToString();
            }
            if (game.GetValue(0, 2) != TicTacToeValue.No)
            {
                button3.Text = game.GetValue(0, 2).ToString();
            }
            if (game.GetValue(1, 0) != TicTacToeValue.No)
            {
                button4.Text = game.GetValue(1, 0).ToString();
            }
            if (game.GetValue(1, 1) != TicTacToeValue.No)
            {
                button5.Text = game.GetValue(1, 1).ToString();
            }
            if (game.GetValue(1, 2) != TicTacToeValue.No)
            {
                button6.Text = game.GetValue(1, 2).ToString();
            }
            if (game.GetValue(2, 0) != TicTacToeValue.No)
            {
                button7.Text = game.GetValue(2, 0).ToString();
            }
            if (game.GetValue(2, 1) != TicTacToeValue.No)
            {
                button8.Text = game.GetValue(2, 1).ToString();
            }
            if (game.GetValue(2, 2) != TicTacToeValue.No)
            {
                button9.Text = game.GetValue(2, 2).ToString();
            }
        }

        private void TicTacToe3x3_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            _nextMoveRow = 0;
            _nextMoveCol = 0;
            tcs.TrySetResult();

            if (game.GetValue(0, 0) != TicTacToeValue.No)
            {
                button1.Click -= button1_Click;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            _nextMoveRow = 0;
            _nextMoveCol = 1;
            tcs.TrySetResult();

            if (game.GetValue(0, 1) != TicTacToeValue.No)
            {
                button2.Click -= button2_Click;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            _nextMoveRow = 0;
            _nextMoveCol = 2;
            tcs.TrySetResult();

            if (game.GetValue(0, 2) != TicTacToeValue.No)
            {
                button3.Click -= button3_Click;
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {
            _nextMoveRow = 1;
            _nextMoveCol = 0;
            tcs.TrySetResult();

            if (game.GetValue(1, 0) != TicTacToeValue.No)
            {
                button4.Click -= button4_Click;
            }
        }
        private void button5_Click(object sender, EventArgs e)
        {
            _nextMoveRow = 1;
            _nextMoveCol = 1;
            tcs.TrySetResult();

            if (game.GetValue(1, 1) != TicTacToeValue.No)
            {
                button5.Click -= button5_Click;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            _nextMoveRow = 1;
            _nextMoveCol = 2;
            tcs.TrySetResult();

            if (game.GetValue(1, 2) != TicTacToeValue.No)
            {
                button6.Click -= button6_Click;
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            _nextMoveRow = 2;
            _nextMoveCol = 0;
            tcs.TrySetResult();

            if (game.GetValue(2, 0) != TicTacToeValue.No)
            {
                button7.Click -= button7_Click;
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            _nextMoveRow = 2;
            _nextMoveCol = 1;
            tcs.TrySetResult();

            if (game.GetValue(2, 1) != TicTacToeValue.No)
            {
                button8.Click -= button8_Click;
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            _nextMoveRow = 2;
            _nextMoveCol = 2;
            tcs.TrySetResult();

            if (game.GetValue(2, 2) != TicTacToeValue.No)
            {
                button9.Click -= button9_Click;
            }
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}