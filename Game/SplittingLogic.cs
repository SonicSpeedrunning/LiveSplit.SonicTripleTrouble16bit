using System;

namespace LiveSplit.SonicTripleTrouble16bit
{
    partial class SonicTripleTrouble16bitComponent
    {
        private bool Start()
        {
            return Settings.Start && watchers.RoomID.Old == 10 && watchers.RoomID.Current == 12;
        }

        private bool Split()
        {
            if (
                watchers.Act.Current == watchers.Act.Old + 1
                || (watchers.Act.Old == 11 && watchers.Act.Current == 69) || (watchers.Act.Old == 69 && watchers.Act.Current == 12) // Special case for Purple Palace
                || (watchers.Act.Old == 0 && watchers.Act.Current == 2)   // Knuckles' transition from AIZ to Great Turquoise
                || (watchers.Act.Old == 17 && watchers.Act.Current == 19) // Beat the Game ending
                )
                return Settings["c" + watchers.Act.Old];
            else return false;
        }

        private bool Reset()
        {
            return Settings.Reset && watchers.RoomID.Old == 10 && watchers.RoomID.Current == 12;
        }

        private bool IsLoading()
        {
            return false;
        }

        private TimeSpan? GameTime()
        {
            return null;
        }
    }
}