using BravelyDefault2.Jobs;
using System;
using System.Collections.Generic;

namespace BravelyDefault2 {
    abstract class Character {
        private const int jobdata_length = 0x3E;
        public string Name { get; set; }
        public ushort BaseOffset { get; set; }
        public uint HPOffset { get; set; } = 0x20;
        public uint MaxHPOffset { get; set; } = 0x6B;
        public uint MPOffset { get; set; } = 0x44;
        public uint MaxMPOffset { get; set; } = 0x92;
        public uint EXPOffset { get; set; } = 0x035E;
        public uint MainJobOffset { get; set; } = 0x0397;
        public uint SecondJobOffset { get; set; }
        public uint HP { get; set; }
        public uint MaxHP { get; set; }
        public uint MP { get; set; }
        public uint MaxMP { get; set; }
        public uint EXP { get; set; }
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

            uint mainJobIDLength = (uint)(BitConverter.ToUInt32(rawSaveData, (int)(MainJobOffset - Util.INTEGER_SIZE)) - Job.JobPrefix.Length);
            string mainJobID = SaveData.Instance().ReadText((uint)(MainJobOffset + Job.JobPrefix.Length), mainJobIDLength);

            MainJob = Job.FromID(mainJobID);

            uint secondJobIDLength = (uint)(BitConverter.ToUInt32(rawSaveData, (int)(SecondJobOffset - Util.INTEGER_SIZE)) - Job.JobPrefix.Length);
            string secondJobID = SaveData.Instance().ReadText((uint)(SecondJobOffset + Job.JobPrefix.Length), secondJobIDLength);

            SecondJob = Job.FromID(secondJobID);

            foreach(Job j in Jobs) {
                j.EXP = BitConverter.ToInt32(rawSaveData, (int)(j.Offset + Job.EXPValueOffset));
                j.LevelLimitUnlocked = SaveData.Instance().ReadBoolean(j.Offset + Job.LevelLimitValueOffset);
            }
        }

        public void UpdateBufferData(byte[] rawSaveData) {
            try {
                Array.Copy(BitConverter.GetBytes(HP), 0, rawSaveData, (int)(BaseOffset + HPOffset), Util.INTEGER_SIZE);
                Array.Copy(BitConverter.GetBytes(MaxHP), 0, rawSaveData, (int)(BaseOffset + MaxHPOffset), Util.INTEGER_SIZE);
                Array.Copy(BitConverter.GetBytes(MP), 0, rawSaveData, (int)(BaseOffset + MPOffset), Util.INTEGER_SIZE);
                Array.Copy(BitConverter.GetBytes(MaxMP), 0, rawSaveData, (int)(BaseOffset + MaxMPOffset), Util.INTEGER_SIZE);
                Array.Copy(BitConverter.GetBytes(EXP), 0, rawSaveData, (int)(BaseOffset + EXPOffset), Util.INTEGER_SIZE);
            } catch(Exception e) {
                Console.WriteLine("{0} Exception caught.", e);
            }

            foreach(Job j in Jobs) {
                try {
                    Array.Copy(BitConverter.GetBytes(j.EXP), 0, rawSaveData, j.Offset + Job.EXPValueOffset, Util.INTEGER_SIZE);
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

            BaseOffset = (ushort)(nameOffset + Name.Length + Util.TERMINATOR_LENGTH);

            int tmp = Util.IndexOf(rawSaveData, Job.MainJobID, BaseOffset);

            if(tmp < 0) {
                throw new Exception("Cannot find crucial values, SaveData is broken");
            }

            MainJobOffset = (uint)(tmp + Job.MainJobID.Length + Util.TERMINATOR_LENGTH + Job.NameValueOffset);

            tmp = Util.IndexOf(rawSaveData, Job.SecondJobID, (int)MainJobOffset);

            if(tmp < 0) {
                throw new Exception("Cannot find crucial values, SaveData is broken");
            }

            SecondJobOffset = (uint)(tmp + Job.SecondJobID.Length + Util.TERMINATOR_LENGTH + Job.NameValueOffset);

            int jobsOffset = Util.IndexOf(rawSaveData, "JobData", BaseOffset);

            if(jobsOffset < 0) {
                throw new Exception("Cannot find crucial values, SaveData is broken");
            }

            jobsOffset += jobdata_length;

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