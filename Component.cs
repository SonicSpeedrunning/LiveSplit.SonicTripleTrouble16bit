using System.Xml;
using System.Windows.Forms;
using LiveSplit.Model;
using LiveSplit.UI;
using LiveSplit.UI.Components;

namespace LiveSplit.SonicTripleTrouble16bit
{
    partial class SonicTripleTrouble16bitComponent : LogicComponent
    {
        public override string ComponentName => "Sonic Triple Trouble - 16-bit - Autosplitter";
        private Settings Settings { get; set; }
        private readonly TimerModel timer;
        private readonly Watchers watchers;

        public SonicTripleTrouble16bitComponent(LiveSplitState state)
        {
            timer = new TimerModel { CurrentState = state };
            Settings = new Settings();
            watchers = new Watchers("Sonic Triple Trouble 16-Bit");
        }

        public override void Dispose()
        {
            Settings.Dispose();
            watchers.Dispose();
        }

        public override XmlNode GetSettings(XmlDocument document) { return this.Settings.GetSettings(document); }

        public override Control GetSettingsControl(LayoutMode mode) { return this.Settings; }

        public override void SetSettings(XmlNode settings) { this.Settings.SetSettings(settings); }

        public override void Update(IInvalidator invalidator, LiveSplitState state, float width, float height, LayoutMode mode)
        {
            // If LiveSplit is not connected to the game, of course there's no point in going further
            if (!watchers.Init()) return;

            // Main update logic is inside the watcher class in order to avoid exposing unneded stuff to the outside
            watchers.Update();

            if (timer.CurrentState.CurrentPhase == TimerPhase.Running || timer.CurrentState.CurrentPhase == TimerPhase.Paused)
            {
                timer.CurrentState.IsGameTimePaused = IsLoading();
                if (GameTime() != null) timer.CurrentState.SetGameTime(GameTime());
                if (Reset()) timer.Reset();
                else if (Split()) timer.Split();
            }

            if (timer.CurrentState.CurrentPhase == TimerPhase.NotRunning)
            {
                if (Start()) timer.Start();
            }
        }
    }
}
