using System;
using System.Linq;

namespace BravelyDefault2 {
    class ViewModel {
        public ViewModel() {

        }

        public uint Money {
            get {
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

        public Character ViewModelSeth => SaveData.Characters.Single(c => c.Name == "Seth");
        public Character ViewModelGloria => SaveData.Characters.Single(c => c.Name == "Gloria");
        public Character ViewModelElvis => SaveData.Characters.Single(c => c.Name == "Elvis");
        public Character ViewModelAdelle => SaveData.Characters.Single(c => c.Name == "Adelle");
    }
}
