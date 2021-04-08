using System;
using System.Collections.Generic;
using System.Linq;

namespace BravelyDefault2 {
    class ViewModel {
        public ViewModel() { }

        public bool SaveLoaded => SaveData.Instance().Loaded;

        public uint Money {
            get {
                if(!SaveData.Instance().Loaded) {
                    return 0;
                }

                GVASData data = Util.ReadData("money");

                if(data == null) {
                    return 0;
                }

                return SaveData.Instance().ReadNumber(data.Address);
            }

            set {
                GVASData data = Util.ReadData("money");

                if(data == null) {
                    return;
                }

                Util.WriteNumber(data.Address, value, 0, 9999999);
            }
        }

        public TimeSpan PlayTime {
            get => SaveData.Instance().PlayTime;
            set { }
        }

        public DateTime SaveDate {
            get => SaveData.Instance().SaveDate;
            set { }
        }

        public Character ViewModelSeth => SaveData.Characters["Seth"];
        public Character ViewModelGloria => SaveData.Characters["Gloria"];
        public Character ViewModelElvis => SaveData.Characters["Elvis"];
        public Character ViewModelAdelle => SaveData.Characters["Adelle"];
        public Dictionary<string, string> CharacterJobsList => Job.List.Select(job => new { job.SaveDataID, job.Name }).ToDictionary(x => x.Name, x => x.SaveDataID);
    }
}
