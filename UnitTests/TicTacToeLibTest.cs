using Microsoft.Extensions.Logging;
using TicTacToeLib;
namespace UnitTests
{
    [TestFixture]
    public class Tests
    {
        Game _game;
        const int LineSize = 3;
        static int MoveCnt;
        Dictionary<int, Point> pointsX;
        Dictionary<int, Point> pointsO;

        [SetUp]
        public void Setup()
        {
            using ILoggerFactory factory = LoggerFactory.Create(builder => builder.AddConsole());
            var logger = factory.CreateLogger<Game>();
            _game = new Game(logger);
            _game.Init(LineSize);
            MoveCnt = 0;
            pointsX = new Dictionary<int, Point>();
            pointsO = new Dictionary<int, Point>();
        }

        [Test]
        public void InvalidStateRestore1()
        {
            State state = new State(LineSize);
            state.ProgressState = TicTacToeState.WaitOMove;
            try
            {
                _game.RestoreState(state);
            }
            catch
            {
                Assert.Pass();
            }
            Assert.Fail("At start first move by X, here must be exception");
        }

        [Test]
        public void InvalidStateRestore2()
        {
            State state = new State(LineSize + 1);
            try
            {
                _game.RestoreState(state);
            }
            catch
            {
                Assert.Pass();
            }
            Assert.Fail("Line sizes not equal, here must be exception");
        }

        [Test]
        public async Task Round1()
        {
            _game.Init(LineSize);
            _game.XMove += Xmove1;
            _game.OMove += Omove1;

            pointsX.Add(0, new Point(1, 1));
            pointsX.Add(2, new Point(2, 0));
            pointsX.Add(4, new Point(2, 2));

            pointsO.Add(1, new Point(0, 2));
            pointsO.Add(3, new Point(0, 0));
            pointsO.Add(5, new Point(0, 1));

            await _game.Run();

            Assert.IsTrue(_game.Winner == TicTacToeValue.O, "In this case Winner must be O!");
            return;
        }

        private Task<Point> Xmove1()
        {
            return Task.FromResult(pointsX[MoveCnt++]);
        }

        private Task<Point> Omove1()
        {
            return Task.FromResult(pointsO[MoveCnt++]);
        }

        [Test]
        public async Task Round2()
        {
            _game.Init(LineSize);
            _game.XMove += Xmove2;
            _game.OMove += Omove2;

            pointsX.Add(0, new Point(1, 1));
            pointsX.Add(2, new Point(2, 0));
            pointsX.Add(4, new Point(2, 2));
            pointsX.Add(6, new Point(1, 2));
            pointsX.Add(8, new Point(0, 1));

            pointsO.Add(1, new Point(0, 2));
            pointsO.Add(3, new Point(0, 0));
            pointsO.Add(5, new Point(2, 1));
            pointsO.Add(7, new Point(1, 0));

            await _game.Run();

            Assert.IsTrue(_game.Winner == TicTacToeValue.No, "In this case no Winner, it is draw!");
            return;
        }

        private Task<Point> Xmove2()
        {
            return Task.FromResult(pointsX[MoveCnt++]);
        }

        private Task<Point> Omove2()
        {
            return Task.FromResult(pointsO[MoveCnt++]);
        }
    }
}