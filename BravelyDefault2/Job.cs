using BravelyDefault2.Jobs;
using System;
using System.Linq;

namespace BravelyDefault2 {
    class Job {
        public const string JobPrefix = "EJobEnum::";
        public const string MainJobID = "MainJobId";
        public const string SecondJobID = "OptionalJobId";
        public const int NameValueOffset = 0x2B;
        public const int EXPValueOffset = 0x21;
        public const int LevelLimitValueOffset = 0x57;
        public virtual string SaveDataID => string.Empty;
        public virtual string Name => string.Empty;
        public uint Offset { get; set; }
        public int EXP { get; set; }
        public bool LevelLimitUnlocked { get; set; }

        public static readonly Job[] List = {
            new Freelancer(), new Monk(), new WhiteMage(), new BlackMage(),
            new Vanguard(), new Bard(), new BeastMaster(), new Thief(),
            new Gambler(), new Berserker(), new RedMage(), new Ranger(),
            new Shieldmaster(), new Pictomancer(), new Dragoon(), new Spiritmaster(),
            new Swordmaster(), new Oracle(), new SalveMaker(), new Arcanist(),
            new Bastion(), new Phantom(), new Hellblade(), new Bravebearer()
        };
        public void RefreshOffset(byte[] haystack, int index = 0) {
            int raw_offset = Util.IndexOf(haystack, SaveDataID, index);

            if(raw_offset < 0) {
                return;
            }

            Offset = (uint)(raw_offset + SaveDataID.Length + Util.TERMINATOR_LENGTH);
        }

        public static string NameFromID(string saveDataID) {
            if(string.IsNullOrEmpty(saveDataID.Trim())) {
                throw new ArgumentException(message: "Invalid job ID");
            }

            string name = string.Empty;

            try {
                name = List.First(j => j.SaveDataID == saveDataID).Name;
            } catch(Exception e) {
                Console.WriteLine(e);
            }

            return name;
        }

        public static Job FromID(string saveDataID) {
            if(string.IsNullOrEmpty(saveDataID)) {
                return null;
            }

            Job j = new();

            try {
                j = List.First(j => j.SaveDataID == saveDataID);
            } catch(Exception e) {
                Console.WriteLine(e);
            }

            return j;
        }
    }
}