using BravelyDefault2.Jobs;
using System;

namespace BravelyDefault2 {
    abstract class Character {
        private const uint MinLevelValue = 1;
        private const uint MaxLevelValue = 99;
        private const int JobDataValuesOffset = 0x3E;
        public string Name { get; set; }
        public ushort BaseOffset { get; set; }
        public uint HPOffset { get; set; } = 0x20;
        public uint MaxHPOffset { get; set; } = 0x6B;
        public uint MPOffset { get; set; } = 0x44;
        public uint MaxMPOffset { get; set; } = 0x92;
        public uint EXPOffset { get; set; } = 0x035E;
        public uint LevelOffset { get; set; } = 0x0339;
        public uint MainJobOffset { get; set; } = 0x0397;
        public uint SecondJobOffset { get; set; }
        public uint HP { get; set; }
        public uint MaxHP { get; set; }
        public uint MP { get; set; }
        public uint MaxMP { get; set; }
        public uint EXP { get; set; }
        private uint _Level;
        public uint Level {
            get => _Level;
            set => _Level = Util.Clamp(value, MinLevelValue, MaxLevelValue);
        }
        public Job MainJob { get; set; }
        public Job SecondJob { get; set; }
        public Job[] Jobs { get; } = {
            new Freelancer(), new Monk(), new WhiteMage(), new BlackMage(),
            new Vanguard(), new Bard(), new BeastMaster(), new Thief(),
            new Gambler(), new Berserker(), new RedMage(), new Ranger(),
            new Shieldmaster(), new Pictomancer(), new Dragoon(), new Spiritmaster(),
            new Swordmaster(), new Oracle(), new SalveMaker(), new Arcanist(),
            new Bastion(), new Phantom(), new Hellblade(), new Bravebearer()
        };

        public void Populate(byte[] rawSaveData) {
            HP = BitConverter.ToUInt32(rawSaveData, (int)(BaseOffset + HPOffset));
            MaxHP = BitConverter.ToUInt32(rawSaveData, (int)(BaseOffset + MaxHPOffset));
            MP = BitConverter.ToUInt32(rawSaveData, (int)(BaseOffset + MPOffset));
            MaxMP = BitConverter.ToUInt32(rawSaveData, (int)(BaseOffset + MaxMPOffset));
            EXP = BitConverter.ToUInt32(rawSaveData, (int)(BaseOffset + EXPOffset));
            Level = BitConverter.ToUInt32(rawSaveData, (int)(BaseOffset + LevelOffset));

            _ = EXPToLevel(EXP);

            uint mainJobIDLength = (uint)(BitConverter.ToUInt32(rawSaveData, (int)(MainJobOffset - sizeof(uint))) - Job.JobPrefix.Length);
            string mainJobID = SaveData.Instance().ReadText((uint)(MainJobOffset + Job.JobPrefix.Length), mainJobIDLength);

            MainJob = Job.FromID(mainJobID);

            uint secondJobIDLength = (uint)(BitConverter.ToUInt32(rawSaveData, (int)(SecondJobOffset - sizeof(uint))) - Job.JobPrefix.Length);
            string secondJobID = SaveData.Instance().ReadText((uint)(SecondJobOffset + Job.JobPrefix.Length), secondJobIDLength);

            SecondJob = Job.FromID(secondJobID);

            foreach(Job j in Jobs) {
                j.EXP = BitConverter.ToInt32(rawSaveData, (int)(j.Offset + Job.EXPValueOffset));
                j.LevelLimitUnlocked = SaveData.Instance().ReadBoolean(j.Offset + Job.LevelLimitValueOffset);
            }
        }

        public uint LevelToEXP(uint levelValue) {
            return 80 * ((4 * levelValue) + 5);
        }

        public uint EXPToLevel(uint expValue) {
            return (expValue - 5) / 4;
        }

        public void UpdateBufferData(byte[] rawSaveData) {
            try {
                Array.Copy(BitConverter.GetBytes(HP), 0, rawSaveData, (int)(BaseOffset + HPOffset), sizeof(uint));
                Array.Copy(BitConverter.GetBytes(MaxHP), 0, rawSaveData, (int)(BaseOffset + MaxHPOffset), sizeof(uint));
                Array.Copy(BitConverter.GetBytes(MP), 0, rawSaveData, (int)(BaseOffset + MPOffset), sizeof(uint));
                Array.Copy(BitConverter.GetBytes(MaxMP), 0, rawSaveData, (int)(BaseOffset + MaxMPOffset), sizeof(uint));
                Array.Copy(BitConverter.GetBytes(EXP), 0, rawSaveData, (int)(BaseOffset + EXPOffset), sizeof(uint));
                Array.Copy(BitConverter.GetBytes(Level), 0, rawSaveData, (int)(BaseOffset + LevelOffset), sizeof(uint));
            } catch(Exception e) {
                Console.WriteLine("{0} Exception caught.", e);
            }

            foreach(Job j in Jobs) {
                try {
                    Array.Copy(BitConverter.GetBytes(j.EXP), 0, rawSaveData, j.Offset + Job.EXPValueOffset, sizeof(uint));
                } catch(Exception e) {
                    Console.WriteLine("{0} Exception caught.", e);
                }
            }
        }

        public void FindOffsets(byte[] rawSaveData) {
            int nameOffset = Util.IndexOf(rawSaveData, Name);

            if(nameOffset < 0) {
                throw new Exception("Cannot find crucial values, SaveData is broken");
            }

            BaseOffset = (ushort)(nameOffset + Name.Length + sizeof(byte));

            int tmp = Util.IndexOf(rawSaveData, Job.MainJobID, BaseOffset);

            if(tmp < 0) {
                throw new Exception("Cannot find crucial values, SaveData is broken");
            }

            MainJobOffset = (uint)(tmp + Job.MainJobID.Length + sizeof(byte) + Job.NameValueOffset);

            tmp = Util.IndexOf(rawSaveData, Job.SecondJobID, (int)MainJobOffset);

            if(tmp < 0) {
                throw new Exception("Cannot find crucial values, SaveData is broken");
            }

            SecondJobOffset = (uint)(tmp + Job.SecondJobID.Length + sizeof(byte) + Job.NameValueOffset);

            int jobsOffset = Util.IndexOf(rawSaveData, "JobData", BaseOffset);

            if(jobsOffset < 0) {
                throw new Exception("Cannot find crucial values, SaveData is broken");
            }

            jobsOffset += JobDataValuesOffset;

            foreach(Job j in Jobs) {
                int job_offset = Util.IndexOf(rawSaveData, j.SaveDataID, jobsOffset);

                if(job_offset == -1) {
                    Console.WriteLine("Shit happened");
                    break;
                }

                j.RefreshOffset(rawSaveData, job_offset);
            }
        }
    }
}