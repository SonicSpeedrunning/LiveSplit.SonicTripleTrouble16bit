// Sonic Triple Trouble 16-Bit
// Autosplitter
// Coding: Jujstme
// Version 1.0.3 (Oct 19th, 2022)

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
        { 69, 69 }, // Purple Palace (secret stage)
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
        { "69", "Purple Palace", null },
        { "12", "Tidal Plant - Act 1", null },
        { "13", "Tidal Plant - Act 2", null },
        { "14", "Tidal Plant - Act 3", null },
        { "15", "Atomic Destroyer - Act 1", null },
        { "16", "Atomic Destroyer - Act 2", null },
        { "17", "Atomic Destroyer - Act 3", null },
        { "18", "Final Trouble (Sonic & Tails)", null }
    };
    for (int i = 0; i < Settings.GetLength(0); i++) settings.Add(Settings[i, 0], true, Settings[i, 1], Settings[i, 2]);
}

init
{
    // Inizialize the variables we need for sigscanning
    var ptr = IntPtr.Zero;
    var scanner = new SignatureScanner(game, modules.First().BaseAddress, modules.First().ModuleMemorySize);

    Action<int, string> scan = (o, sig) =>
    {
        ptr = scanner.Scan(new SigScanTarget(o, sig) { OnFound = (p, s, addr) => p.Is64Bit() ? addr + 0x4 + p.ReadValue<int>(addr) : p.ReadPointer(addr) });
        if (ptr == IntPtr.Zero) throw new NullReferenceException("Sigscanning failed!");
    };

    switch (game.Is64Bit())
    {
        case true: scan(6, "4D 0F 45 F5 8B 0D");             break;
        default:   scan(2, "8B 0D ???????? 83 C4 04 3B 0D"); break;
    }

    vars.RoomID = new MemoryWatcher<int>(ptr);

    // Defining variables used later in the script
    current.Act = 19;
}

update
{
    vars.RoomID.Update(game);
    if (vars.Acts.ContainsKey(vars.RoomID.Current)) current.Act = vars.Acts[vars.RoomID.Current];
}

start
{
    return vars.RoomID.Old == 10 && vars.RoomID.Current == 12;
}

split
{
    // Normal act completion splitting
    if (
        current.Act == old.Act + 1
        || (old.Act == 11 && current.Act == 69) || (old.Act == 69 && current.Act == 12) // special case for Purple Palace
        || (old.Act == 0 && current.Act == 2)   // Knuckles' transition from AIZ to Great Turquoise
        || (current.Act == 19 && old.Act == 17) // Beat the Game ending
       )
    {
        return settings[old.Act.ToString()];
    }
}

reset
{
    return vars.RoomID.Old == 10 && vars.RoomID.Current == 12;
}
