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
}