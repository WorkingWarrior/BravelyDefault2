namespace BravelyDefault2 {
    class Job {
        public const string JobPrefix = "EJobEnum::";
        public virtual string SaveDataID => "";
        public virtual string Name => "";
        public ushort Offset { get; set; }
        public int EXP { get; set; }
        public bool LevelLimitUnlocked { get; set; }
        public void RefreshOffset(byte[] haystack, int index = 0) {
            int raw_offset = Util.IndexOf(haystack, SaveDataID, index);

            if(raw_offset < 0) {
                return;
            }

            Offset = (ushort)(raw_offset + SaveDataID.Length + 1 + 0x21);
        }

        static public Job GetJobFromID(string saveDataID) {
            if(string.IsNullOrEmpty(saveDataID.Trim())) {
                throw new System.ArgumentException();
            }

            Job j = new();

            switch(saveDataID) {
                case "JE_Sobriety":
                    j = new Freelancer();
                    break;
                case "JE_Monk":
                    j = new Monk();
                    break;
                case "JE_White_Mage":
                    j = new WhiteMage();
                    break;
                case "JE_Black_Mage":
                    j = new BlackMage();
                    break;
                case "JE_Vanguard":
                    j = new Vanguard();
                    break;
                case "JE_Troubadour":
                    j = new Bard();
                    break;
                case "JE_Tamer":
                    j = new BeastMaster();
                    break;
                case "JE_Thief":
                    j = new Thief();
                    break;
                case "JE_Gambler":
                    j = new Gambler();
                    break;
                case "JE_Berzerk":
                    j = new Berserker();
                    break;
                case "JE_Red_Mage":
                    j = new RedMage();
                    break;
                case "JE_Hunter":
                    j = new Ranger();
                    break;
                case "JE_Shield_Master":
                    j = new Shieldmaster();
                    break;
                case "JE_Pictomancer":
                    j = new Pictomancer();
                    break;
                case "JE_Dragoon_Warrior":
                    j = new Dragoon();
                    break;
                case "JE_Master":
                    j = new Spiritmaster();
                    break;
                case "JE_Sword_Master":
                    j = new Swordmaster();
                    break;
                case "JE_Oracle":
                    j = new Oracle();
                    break;
                case "JE_Doctor":
                    j = new SalveMaker();
                    break;
                case "JE_Demon":
                    j = new Arcanist();
                    break;
                case "JE_Judgement":
                    j = new Bastion();
                    break;
                case "JE_Phantom":
                    j = new Phantom();
                    break;
                case "JE_Cursed_Sword":
                    j = new Hellblade();
                    break;
                case "JE_Brave":
                    j = new Bravebearer();
                    break;
                default:
                    break;
            }

            return j;
        }
    }
}