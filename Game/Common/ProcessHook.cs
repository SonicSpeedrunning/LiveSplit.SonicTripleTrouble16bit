using System;
using System.Linq;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace LiveSplit.SonicTripleTrouble16bit
{
    /// <summary>
    /// Custom class with the ability to automatically hook to a target process using Tasks
    /// </summary>
    class ProcessHook
    {
        // Internal stuff
        private readonly string[] processNames;
        private readonly CancellationTokenSource CancelToken;

        // Properties we probably want to expose
        public Process Game { get; protected set; }
        public GameInitStatus InitStatus { get; set; }
        public bool IsGameHooked => Game != null && !Game.HasExited;

        public ProcessHook(params string[] exeNames)
        {
            processNames = exeNames;
            InitStatus = GameInitStatus.NotStarted;
            CancelToken = new CancellationTokenSource();
            Task.Run(TryConnect, CancelToken.Token);
        }

        private async Task TryConnect()
        {
            while (!CancelToken.IsCancellationRequested)
            {
                foreach (var entry in processNames)
                {
                    Game = Process.GetProcessesByName(entry).OrderByDescending(p => p.StartTime).FirstOrDefault(p => !p.HasExited);
                    if (Game != null)
                    {
                        Game.Exited += CallBackTryConnect;
                        return;
                    }
                }
                await Task.Delay(1500, CancelToken.Token);
            }
        }

        private void CallBackTryConnect(object sender, EventArgs e)
        {
            Game.Exited -= CallBackTryConnect;
            InitStatus = GameInitStatus.NotStarted;
            Game?.Dispose();
            Game = null;
            Task.Run(TryConnect, CancelToken.Token);
        }

        /// <summary>
        /// Releases the resources used by ProcessHook and cancels the internal TryConnect task.
        /// </summary>
        public void Dispose()
        {
            CancelToken?.Cancel();
            CancelToken?.Dispose();
            Game?.Dispose();
        }
    }

    enum GameInitStatus
    {
        NotStarted,
        InProgress,
        Completed
    }
}
