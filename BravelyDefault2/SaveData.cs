using System;
using System.Collections.Generic;

namespace BravelyDefault2 {
    class SaveData {
        private const int HeaderLength = 0x0C;
        private static SaveData mThis;
        static readonly byte[] magic = new byte[] { 0x53, 0x41, 0x56, 0x45 };
        private string mFileName = null;
        private byte[] mHeader = new byte[HeaderLength];
        private byte[] mBuffer = null;
        private readonly System.Text.Encoding mEncode = System.Text.Encoding.UTF8;
        public uint Adventure { private get; set; } = 0;
        public static Character[] Characters { get; private set; } = new Character[] { new Seth(), new Gloria(), new Elvis(), new Adelle() };

        private SaveData() { }

        private uint CalcAddress(uint address) {
            return address;
        }

        public static SaveData Instance() {
            if(mThis == null) {
                mThis = new SaveData();
            }

            return mThis;
        }

        public bool Open(string filename) {
            if(System.IO.File.Exists(filename) == false) {
                return false;
            }

            byte[] tmp = System.IO.File.ReadAllBytes(filename);

            for(int i = 0; i < magic.Length; i++) {
                if(magic[i] != tmp[i]) {
                    return false;
                }
            }

            byte[] comp = new byte[tmp.Length - mHeader.Length];

            Array.Copy(tmp, mHeader.Length, comp, 0, comp.Length);
            Array.Copy(tmp, mHeader, mHeader.Length);

            try {
                mBuffer = Ionic.Zlib.ZlibStream.UncompressBuffer(comp);
            } catch {
                return false;
            }

            foreach(Character c in Characters) {
                c.FindOffsets(mBuffer);
                c.Populate(mBuffer);
            }

            mFileName = filename;

            Backup();

            return true;
        }

        public bool Save() {
            if(mFileName == null || mBuffer == null) {
                return false;
            }

            #region CHARACTER_DATA_UPDATE
            foreach(Character c in Characters) {
                c.UpdateBufferData(mBuffer);
            }
            #endregion

            byte[] comp = Ionic.Zlib.ZlibStream.CompressBuffer(mBuffer);
            byte[] tmp = new byte[mHeader.Length + comp.Length];

            Array.Copy(mHeader, tmp, mHeader.Length);
            Array.Copy(comp, 0, tmp, mHeader.Length, comp.Length);
            System.IO.File.WriteAllBytes(mFileName, tmp);

            return true;
        }

        public bool SaveAs(string filename) {
            if(mFileName == null || mBuffer == null) {
                return false;
            }

            mFileName = filename;

            return Save();
        }

        public void Import(string filename) {
            if(mFileName == null) {
                return;
            }

            mBuffer = System.IO.File.ReadAllBytes(filename);
        }

        public void Export(string filename) {
            System.IO.File.WriteAllBytes(filename, mBuffer);
        }

        public uint ReadNumber(uint address, bool header = false) {
            byte[] buffer = header ? mHeader : mBuffer;

            if(null == buffer) {
                throw new ArgumentNullException();
            }

            address = CalcAddress(address);

            if(address + 4 > buffer.Length) {
                throw new IndexOutOfRangeException();
            }

            return ReadNumber(buffer, (int)address);
        }

        private uint ReadNumber(byte[] buffer, int address) {
            uint result = 0;

            try {
                result = BitConverter.ToUInt32(buffer, address);
            } catch(Exception e) {
                Console.WriteLine("{0} Exception caught.", e);
            }

            return result;
        }

        public byte[] ReadValue(uint address, uint size, bool header = false) {
            byte[] buffer = header ? mHeader : mBuffer;
            byte[] result = new byte[size];

            if(null == buffer) {
                throw new ArgumentNullException();
            }

            address = CalcAddress(address);

            if(address + size > buffer.Length) {
                throw new IndexOutOfRangeException();
            }

            Array.Copy(buffer, address, result, 0, size);

            return result;
        }

        // 0 to 7.
        public bool ReadBit(uint address, uint bit, bool header = false) {
            byte[] buffer = header ? mHeader : mBuffer;

            if(bit < 0) {
                return false;
            }

            if(bit > 7) {
                return false;
            }

            if(buffer == null) {
                return false;
            }

            address = CalcAddress(address);

            if(address > buffer.Length) {
                return false;
            }

            byte mask = (byte)(1 << (int)bit);
            byte result = (byte)(buffer[address] & mask);

            return result != 0;
        }

        public string ReadText(uint address, uint size, bool header = false) {
            byte[] buffer = header ? mHeader : mBuffer;

            if(buffer == null) {
                return "";
            }

            address = CalcAddress(address);

            if(address + size > buffer.Length) {
                return "";
            }

            byte[] tmp = new byte[size];

            for(uint i = 0; i < size; i++) {
                if(buffer[address + i] == 0) {
                    break;
                }

                tmp[i] = buffer[address + i];
            }

            return mEncode.GetString(tmp).Trim('\0');
        }

        public void ReadArray(uint address, uint size) {
            throw new NotImplementedException();
        }

        #region WRITE_NUMBER
        public void WriteNumber(uint address, uint value, bool header = false) {
            byte[] buffer = header ? mHeader : mBuffer;

            if(null == buffer) {
                throw new ArgumentNullException();
            }

            address = CalcAddress(address);

            if(address + 4 > buffer.Length) {
                throw new IndexOutOfRangeException();
            }

            WriteNumber(buffer, (int)address, value);
        }

        private void WriteNumber(byte[] buffer, int address, uint value) {
            byte[] result = BitConverter.GetBytes(value);

            try {
                Array.Copy(result, 0, buffer, address, 4);
            } catch(Exception e) {
                Console.WriteLine("{0} Exception caught.", e);
            }
        }
        #endregion

        // 0 to 7.
        public void WriteBit(uint address, uint bit, bool value, bool header = false) {
            if(bit < 0 || bit > 7) {
                throw new IndexOutOfRangeException();
            }

            byte[] buffer = header ? mHeader : mBuffer;

            if(null == buffer) {
                throw new ArgumentNullException();
            }

            address = CalcAddress(address);

            if(address > buffer.Length) {
                throw new IndexOutOfRangeException();
            }

            byte mask = (byte)(1 << (int)bit);

            if(value) {
                buffer[address] = (byte)(buffer[address] | mask);
            } else {
                buffer[address] = (byte)(buffer[address] & ~mask);
            }
        }

        public void WriteText(uint address, uint size, string value, bool header = false) {
            byte[] buffer = header ? mHeader : mBuffer;

            if(null == buffer) {
                throw new ArgumentNullException();
            }

            address = CalcAddress(address);

            if(address + size > buffer.Length) {
                throw new IndexOutOfRangeException();
            }

            byte[] tmp = mEncode.GetBytes(value);

            Array.Resize(ref tmp, (int)size);
            Array.Copy(tmp, 0, buffer, address, size);
        }

        public void WriteValue(uint address, byte[] buffer, bool header = false) {
            byte[] destBuffer = header ? mHeader : mBuffer;

            if(destBuffer == null) {
                throw new ArgumentNullException();
            }

            address = CalcAddress(address);

            if(address + buffer.Length > destBuffer.Length) {
                throw new IndexOutOfRangeException();
            }

            try {
                Array.Copy(buffer, 0, destBuffer, address, buffer.Length);
            } catch(Exception e) {
                Console.WriteLine("{0} Exception caught.", e);
            }
        }

        public void Fill(uint address, uint size, byte number, bool header = false) {
            byte[] buffer = header ? mHeader : mBuffer;

            if(null == buffer) {
                throw new ArgumentNullException();
            }

            address = CalcAddress(address);

            if(address + size > buffer.Length) {
                throw new IndexOutOfRangeException();
            }

            Array.Fill(buffer, number, (int)address, (int)size);
        }

        public void Copy(uint from, uint to, uint size, bool header = false) {
            byte[] buffer = header ? mHeader : mBuffer;

            if(null == buffer) {
                throw new ArgumentNullException();
            }

            from = CalcAddress(from);
            to = CalcAddress(to);

            if(from + size > buffer.Length) {
                throw new IndexOutOfRangeException();
            }

            if(to + size > buffer.Length) {
                throw new IndexOutOfRangeException();
            }

            try {
                Array.Copy(buffer, from, buffer, to, size);
            } catch(Exception e) {
                Console.WriteLine("{0} Exception caught.", e);
            }
        }

        public void Swap(uint from, uint to, uint size, bool header = false) {
            byte[] buffer = header ? mHeader : mBuffer;

            if(null == buffer) {
                throw new ArgumentNullException();
            }

            from = CalcAddress(from);
            to = CalcAddress(to);

            if(from + size > buffer.Length) {
                throw new IndexOutOfRangeException();
            }

            if(to + size > buffer.Length) {
                throw new IndexOutOfRangeException();
            }

            byte[] tmp = new byte[size];

            try {
                Array.Copy(buffer, to, tmp, 0, size);
                Array.Copy(buffer, from, buffer, to, size);
                Array.Copy(tmp, 0, buffer, from, size);
            } catch(Exception e) {
                Console.WriteLine("{0} Exception caught.", e);
            }
        }

        public List<uint> FindAddress(string name, uint index, bool header = false) {
            byte[] buffer = header ? mHeader : mBuffer;
            List<uint> result = new();

            if(null == buffer) {
                return result;
            }

            for(; index < mBuffer.Length; index++) {
                if(buffer[index] != name[0]) {
                    continue;
                }

                int len = 1;

                for(; len < name.Length; len++) {
                    if(buffer[index + len] != name[len]) {
                        break;
                    }
                }

                if(len >= name.Length) {
                    result.Add(index);
                }

                index += (uint)len;
            }

            return result;
        }

        private void Backup() {
            DateTime now = DateTime.Now;
            String path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            path = System.IO.Path.Combine(path, "backup");

            if(!System.IO.Directory.Exists(path)) {
                System.IO.Directory.CreateDirectory(path);
            }

            path = System.IO.Path.Combine(path,
                String.Format("{0:0000}-{1:00}-{2:00} {3:00}-{4:00}", now.Year, now.Month, now.Day, now.Hour, now.Minute));
            System.IO.File.Copy(mFileName, path, true);
        }
    }
}
