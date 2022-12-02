using System.Collections.Generic;

namespace LiveSplit.SonicTripleTrouble16bit
{
    readonly struct Levels
    {
        public static readonly Dictionary<int, byte> Acts = new Dictionary<int, byte>
        {
            { 70, 0 }, { 71, 0 }, { 72, 0 },    // Angel Island (Knuckles)
            { 40, 1 },                          // Zone Zero (Sonic & Tails)
            { 41, 2 },                          // Great Turquoise Act 1
            { 42, 3 },                          // Great Turquoise Act 2
            { 43, 4 },                          // Sunset Park Act 1
            { 44, 5 },                          // Sunset Park Act 2
            { 45, 6 },                          // Sunset Park Act 3
            { 46, 7 },                          // Meta Junglira Act 1
            { 47, 8 },                          // Meta Junglira Act 2
            { 48, 9 },                          // Egg Zeppelin
            { 49, 10 },                         // Robotnik Winter Act 1
            { 50, 11 }, { 51, 11 },             // Robotnik Winter Act 2
            { 52, 12 },                         // Tidal Plant Act 1
            { 53, 13 },                         // Tidal Plant Act 2
            { 54, 14 },                         // Tidal Plant Act 3
            { 55, 15 },                         // Atomic Destroyer Act 1
            { 56, 16 },                         // Atomic Destroyer Act 2
            { 57, 17 },                         // Atomic Destroyer Act 3
            { 58, 18 },                         // Final Trouble (Sonic & Tails)
            { 9, 19 },                          // Sonic & Tails' Credits
            { 0, 19 },                          // Super Sonic ending
            { 1, 19 }, { 2, 19 },               // Knuckles' ending
            { 69, 69 },                         // Purple Palace (secret stage)
        };
    }
}