namespace BravelyDefault2 {
    public class ComboBoxPairs {
        public string Key { get; set; }
        public string Value { get; set; }

        public ComboBoxPairs(string _key, string _value) {
            Key = _key;
            Value = _value;
        }
        public override string ToString() {
            return Key;
        }
    }
}
