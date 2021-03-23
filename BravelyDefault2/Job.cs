using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BravelyDefault2 {
    class Job {
        public virtual string SaveDataID => "";
        public virtual string Name => "";
        public ushort Offset { get; set; }
        public int EXP { get; set; }
        public bool LevelLimitUnlocked { get; set; }

        public void RefreshOffset(byte[] haystack, int index = 0) {
            int raw_offset = Util.SearchBytes(haystack, SaveDataID, index);

            if(raw_offset < 0) {
                return;
            }

            Offset = (ushort)(raw_offset + SaveDataID.Length + 1 + 0x21);
        }
    }

    class Freelancer : Job {
        public override string SaveDataID => "JE_Sobriety";
        public override string Name => "Freelancer";
    }

    class Monk : Job {
        public override string SaveDataID => "JE_Monk";
        public override string Name => "Monk";
    }

    class WhiteMage : Job {
        public override string SaveDataID => "JE_White_Mage";
        public override string Name => "White Mage";
    }

    class BlackMage : Job {
        public override string SaveDataID => "JE_Black_Mage";
        public override string Name => "Black Mage";
    }

    class Vanguard : Job {
        public override string SaveDataID => "JE_Vanguard";
        public override string Name => "Vanguard";
    }

    class Bard : Job {
        public override string SaveDataID => "JE_Troubadour";
        public override string Name => "Bard";
    }

    class BeastMaster : Job {
        public override string SaveDataID => "JE_Tamer";
        public override string Name => "Beast Master";
    }

    class Thief : Job {
        public override string SaveDataID => "JE_Thief";
        public override string Name => "Thief";
    }

    class Gambler : Job {
        public override string SaveDataID => "JE_Gambler";
        public override string Name => "Gambler";
    }

    class Berserker : Job {
        public override string SaveDataID => "JE_Berzerk";
        public override string Name => "Berserker";
    }

    class RedMage : Job {
        public override string SaveDataID => "JE_Red_Mage";
        public override string Name => "Red Mage";
    }

    class Ranger : Job {
        public override string SaveDataID => "JE_Hunter";
        public override string Name => "Ranger";
    }

    class Shieldmaster : Job {
        public override string SaveDataID => "JE_Shield_Master";
        public override string Name => "Shieldmaster";
    }

    class Pictomancer : Job {
        public override string SaveDataID => "JE_Pictomancer";
        public override string Name => "Pictomancer";
    }

    class Dragoon : Job {
        public override string SaveDataID => "JE_Dragoon_Warrior";
        public override string Name => "Dragoon";
    }

    class Spiritmaster : Job {
        public override string SaveDataID => "JE_Master";
        public override string Name => "Spiritmaster";
    }

    class Swordmaster : Job {
        public override string SaveDataID => "JE_Sword_Master";
        public override string Name => "Swordmaster";
    }

    class Oracle : Job {
        public override string SaveDataID => "JE_Oracle";
        public override string Name => "Oracle";
    }

    class SalveMaker : Job {
        public override string SaveDataID => "JE_Doctor";
        public override string Name => "Salve-Maker";
    }

    class Arcanist : Job {
        public override string SaveDataID => "JE_Demon";
        public override string Name => "Arcanist";
    }

    class Bastion : Job {
        public override string SaveDataID => "JE_Judgement";
        public override string Name => "Bastion";
    }

    class Phantom : Job {
        public override string SaveDataID => "JE_Phantom";
        public override string Name => "Phantom";
    }

    class Hellblade : Job {
        public override string SaveDataID => "JE_Cursed_Sword";
        public override string Name => "Hellblade";
    }

    class Bravebearer : Job {
        public override string SaveDataID => "JE_Brave";
        public override string Name => "Bravebearer";
    }
}
