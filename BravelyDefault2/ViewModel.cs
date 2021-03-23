using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BravelyDefault2 {
    class ViewModel {
        public ViewModel() {

        }

        public uint Money {
            get {
                var data = Util.ReadData("money");
                if(data == null)
                    return 0;
                return SaveData.Instance().ReadNumber(data.Address, data.Size);
            }

            set {
                var data = Util.ReadData("money");
                if(data == null)
                    return;
                Util.WriteNumber(data.Address, data.Size, value, 0, 9999999);
            }
        }

        public Character ViewModelSeth {
            get => SaveData.Characters.Single(c => c.Name == "Seth");
        }
        public Character ViewModelGloria {
            get => SaveData.Characters.Single(c => c.Name == "Gloria");
        }
        public Character ViewModelElvis {
            get => SaveData.Characters.Single(c => c.Name == "Elvis");
        }
        public Character ViewModelAdelle {
            get => SaveData.Characters.Single(c => c.Name == "Adelle");
        }
    }
}
