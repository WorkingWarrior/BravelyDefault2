using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public uint HP { get; set; }
        public uint MaxHP { get; set; }
        public uint MP { get; set; }
        public uint MaxMP { get; set; }
        public uint EXP { get; set; }
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

            foreach(Job j in Jobs) {
                j.EXP = BitConverter.ToInt32(rawSaveData, j.Offset);
            }
        }

        public void FindOffsets(byte[] rawSaveData) {
            int nameOffset = Util.SearchBytes(rawSaveData, Name);

            if(nameOffset < 0) {
                return;
            }

            BaseOffset = (ushort)(nameOffset + Name.Length + 1);

            int jobsOffset = Util.SearchBytes(rawSaveData, "JobData", BaseOffset);

            if(jobsOffset < 0) {
                return;
            }

            jobsOffset += jobdata_length;

            foreach(Job j in Jobs) {
                int job_offset = Util.SearchBytes(rawSaveData, j.SaveDataID, jobsOffset);

                if(job_offset == -1) {
                    Console.WriteLine("Shit happened");
                    break;
                }

                j.RefreshOffset(rawSaveData, job_offset);
            }
        }
    }

    class Seth : Character {
        public Seth() {
            Name = "Seth";
        }
    }

    class Gloria : Character {
        public Gloria() {
            Name = "Gloria";
        }
    }

    class Elvis : Character {
        public Elvis() {
            Name = "Elvis";
        }
    }

    class Adelle : Character {
        public Adelle() {
            Name = "Adelle";
        }
    }
}
