// Sonic Triple Trouble 16-Bit
// Autosplitter
// Coding: Jujstme
// Version 1.0.0 (Aug 7th, 2022)

state ("Sonic Triple Trouble 16-Bit") {}

startup
{
    vars.Acts = new Dictionary<int, byte>{
        { 70, 0 }, { 71, 0 }, { 72, 0 },
        { 40, 1 },
        { 41, 2 },
        { 42, 3 },
        { 43, 4 },
        { 44, 5 },
        { 45, 6 },
        { 46, 7 },
        { 47, 8 },
        { 48, 9 },
        { 49, 10 },
        { 50, 11 }, { 51, 11 },
        { 52, 12 },
        { 53, 13 },
        { 54, 14 },
        { 55, 15 },
        { 56, 16 },
        { 57, 17 },
        { 58, 18 },
        { 9, 19 }, // Sonic & Tails' Credits
        { 0, 19 }, // Super Sonic ending
        { 1, 19 }, { 2, 19 }, // Knuckles' ending
    };

    string[,] Settings =
    {
        { "1" , "Zone Zero (Sonic & Tails)", null },
        { "0", "Angel Island (Knuckles)", null },
        { "2", "Great Turquoise - Act 1", null },
        { "3", "Great Turquoise - Act 2", null },
        { "4", "Sunset Park - Act 1", null },
        { "5", "Sunset Park - Act 2", null },
        { "6", "Sunset Park - Act 3", null },
        { "7", "Meta Junglira - Act 1", null },
        { "8", "Meta Junglira - Act 2", null },
        { "9" , "Egg Zeppelin", null },
        { "10", "Robotnik Winter - Act 1", null },
        { "11", "Robotnik Winter - Act 2", null },
        { "12", "Tidal Plant - Act 1", null },
        { "13", "Tidal Plant - Act 2", null },
        { "14", "Tidal Plant - Act 3", null },
        { "15", "Atomic Destroyer - Act 1", null },
        { "16", "Atomic Destroyer - Act 2", null },
        { "17", "Atomic Destroyer - Act 3", null },
        { "18" , "Final Trouble (Sonic & Tails)", null }
    };
    for (int i = 0; i < Settings.GetLength(0); i++) settings.Add(Settings[i, 0], true, Settings[i, 1], Settings[i, 2]);
}

init
{
    // Inizialize the main watchers and the variables we need for sigscanning
    vars.watchers = new MemoryWatcherList();
    var ptr = IntPtr.Zero;
    var scanner = new SignatureScanner(game, modules.First().BaseAddress, modules.First().ModuleMemorySize);
    Action checkptr = () => { if (ptr == IntPtr.Zero) throw new Exception("Sigscanning failed!"); };

    ptr = scanner.Scan(new SigScanTarget(6, "4D 0F 45 F5 8B 0D") { OnFound = (p, s, addr) => addr + 0x4 + p.ReadValue<int>(addr) }); checkptr();
    vars.watchers.Add(new MemoryWatcher<int>(ptr) { Name = "RoomID" });

    //ptr = scanner.Scan(new SigScanTarget(8, "48 8B 7C 24 20 48 89 35 ???????? 48 89 35") { OnFound = (p, s, addr) => addr + 0x4 + p.ReadValue<int>(addr) });
    //vars.watchers.Add(new MemoryWatcher<double>(new DeepPointer(ptr, 0x48, 0x10, 0x340, 0x850)) { Name = "ChaosEmeralds" });

    // Defining variables used later in the script
    current.Act = 19;
}

update
{
    vars.watchers.UpdateAll(game);
    if (vars.Acts.ContainsKey(vars.watchers["RoomID"].Current)) current.Act = vars.Acts[vars.watchers["RoomID"].Current];
}

start
{
    return vars.watchers["RoomID"].Old == 10 && vars.watchers["RoomID"].Current == 12;
}

split
{
    /* Sonic ending
    if (settings["17"] && current.Act == 17 && vars.watchers["ChaosEmeralds"].Current == vars.watchers["ChaosEmeralds"].Old + 1 && vars.watchers["ChaosEmeralds"].Current != 7d)
    {
        return true;
    }
    */

    // Normal act completion splitting
    if (current.Act == old.Act + 1 || (current.Act == 2 && old.Act == 0) || (current.Act == 19 && old.Act == 17))
    {
        return settings[old.Act.ToString()];
    }
}

reset
{
    return vars.watchers["RoomID"].Old == 10 && vars.watchers["RoomID"].Current == 12;
}