using System;
using System.Reflection;
using LiveSplit.Model;
using LiveSplit.UI.Components;
using LiveSplit.SonicTripleTrouble16bit;

[assembly: ComponentFactory(typeof(SonicTripleTrouble16bitFactory))]

namespace LiveSplit.SonicTripleTrouble16bit
{
    public class SonicTripleTrouble16bitFactory : IComponentFactory
    {
        public string ComponentName => "Sonic Triple Trouble - 16-bit - Autosplitter";
        public string Description => "Automatic splitting and Game Time calculation";
        public ComponentCategory Category => ComponentCategory.Control;
        public string UpdateName => this.ComponentName;
        public string UpdateURL => "https://raw.githubusercontent.com/SonicSpeedrunning/LiveSplit.SonicTripleTrouble16bit/main/";
        public Version Version => Assembly.GetExecutingAssembly().GetName().Version;
        public string XMLURL => this.UpdateURL + "Components/update.LiveSplit.SonicTripleTrouble16bit.xml";
        public IComponent Create(LiveSplitState state) { return new SonicTripleTrouble16bitComponent(state); }
    }
}
