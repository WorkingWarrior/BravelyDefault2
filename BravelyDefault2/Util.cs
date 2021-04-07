using System;
using System.Collections.Generic;
using System.Text;

namespace BravelyDefault2 {
    class Util {
        public const int INTEGER_SIZE = 4;
        public const int TERMINATOR_LENGTH = 1;

        public static T Clamp<T>(T value, T min, T max) where T : IComparable<T> {
            if(value.CompareTo(min) < 0) {
                return min;
            }

            if(value.CompareTo(max) > 0) {
                return max;
            }

            return value;
        }
        public static void WriteNumber(uint address, uint value, uint min, uint max) {
            SaveData.Instance().WriteNumber(address, Clamp(value, min, max));
        }

        public static int IndexOf(byte[] haystack, string needle, int index = 0) {
            return IndexOf(haystack, Encoding.UTF8.GetBytes(needle), index);
        }

        public static int IndexOf(byte[] haystack, byte[] needle, int index = 0) {
            if(haystack == null) {
                throw new ArgumentNullException("Buffer doesn't exist");
            }

            int len = needle.Length;
            int limit = haystack.Length - len;

            for(; index <= limit; index++) {
                int k = 0;

                for(; k < len; k++) {
                    if(needle[k] != haystack[index + k]) {
                        break;
                    }
                }

                if(k == len) {
                    return index;
                }
            }

            return -1;
        }

        public static List<uint> IndexesOf(byte[] haystack, string needle, int index = 0) {
            return IndexesOf(haystack, Encoding.UTF8.GetBytes(needle), index);
        }
        public static List<uint> IndexesOf(byte[] haystack, byte[] needle, int index = 0) {
            List<uint> result = new();

            while(index > -1) {
                index = IndexOf(haystack, needle, index);

                if(index > -1) {
                    result.Add((uint)index);

                    index += needle.Length;
                } else {
                    break;
                }
            }

            return result;
        }

        public static GVASData ReadData(String name) {
            return ReadData(name, 0);
        }

        public static GVASData ReadData(String name, uint index) {
            GVAS gvas = new(null);
            List<uint> list = SaveData.Instance().IndexesOf(name, index);

            if(list.Count == 0) {
                return null;
            }

            gvas.AppendValue(list[0]);

            return gvas.Key(name);
        }
    }
}
