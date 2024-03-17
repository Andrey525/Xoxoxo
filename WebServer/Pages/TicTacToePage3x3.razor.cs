using Microsoft.AspNetCore.Components;
using TicTacToeLib;
using WebServer.Data;

namespace WebServer.Pages
{
    public partial class TicTacToePage3x3
    {
        [Inject] Game? Game { get; set; }
        [Inject] Bot? Bot { get; set; }
        const int LineSize = 3;
        int _nextMoveRow;
        int _nextMoveCol;
        TaskCompletionSource tcs;

        protected override void OnInitialized()
        {
            Bot.Game = Game;

            Bot.ChangeHelper(typeof(RemoteHelper));

            tcs = new TaskCompletionSource();
        }

        private void ReloadPage()
        {
            NavigationManager.NavigateTo(NavigationManager.Uri, true);
        }

        private async Task OnChooseButtonClick(Game.MoveCallback XmoveHandler, Game.MoveCallback OmoveHandler)
        {
            Game.XMove += XmoveHandler;
            Game.OMove += OmoveHandler;
            Game.GameStateUpdate += StateHasChanged;
            Game.Init(LineSize);
            await Game.Run();
        }

        private void OnButtonClick(int row, int col)
        {
            _nextMoveRow = row;
            _nextMoveCol = col;
            tcs.TrySetResult();
        }

        private async Task<Point> MakeMove()
        {
            /* 
             * Since this method is called immediately when the event occurs.
             * We should wait for the user to click the button.
             */
            await tcs.Task;
            tcs = new TaskCompletionSource();

            return new Point(_nextMoveRow, _nextMoveCol);
        }
    }
}
