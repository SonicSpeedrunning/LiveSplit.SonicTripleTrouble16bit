using LiveSplit.ComponentUtil;

namespace LiveSplit.SonicTripleTrouble16bit
{
    partial class Watchers
    {
        // Pointers and offsets
        public MemoryWatcher<int> RoomID { get; set; }
        public FakeMemoryWatcher<int> Act { get; set; }
        
        public Watchers(params string[] gameNames)
        {
            Act = new FakeMemoryWatcher<int>(() => Levels.Acts.TryGetValue(RoomID.Current, out var value) ? value : Act.Current);
            GameProcess = new ProcessHook(gameNames);
        }

        public void Update()
        {
            RoomID.Update(GameProcess.Game);
            Act.Update();
        }

        /// <summary>
        /// A method functionally equivalent to the "init" action of script-based autosplitters.
        /// Used to define MemoryWatchers and other useful variables used in other parts of the component.
        /// </summary>
        private void GetAddresses()
        {
            var scanner = new SignatureScanner(GameProcess.Game, GameProcess.Game.MainModuleWow64Safe().BaseAddress, GameProcess.Game.MainModuleWow64Safe().ModuleMemorySize);
            
            RoomID = GameProcess.Game.Is64Bit()
                ? new MemoryWatcher<int>(scanner.ScanOrThrow(new SigScanTarget(6, "4D 0F 45 F5 8B 0D") { OnFound = (p, s, addr) => addr + 0x4 + p.ReadValue<int>(addr) }))
                : new MemoryWatcher<int>(scanner.ScanOrThrow(new SigScanTarget(2, "8B 0D ???????? 83 C4 04 3B 0D") { OnFound = (p, s, addr) => p.ReadPointer(addr) }));
        }
    }
}