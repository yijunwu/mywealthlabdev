namespace MS123.Web.Strategies
{
    using System;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;

    public class RijndaelCryptography
    {
        private RijndaelManaged _rm = new RijndaelManaged();

        public RijndaelCryptography(byte[] key, byte[] iv)
        {
            this._rm.Key = key;
            this._rm.IV = iv;
        }

        public void DecryptFile(string inFileName, string outFileName)
        {
            using (FileStream stream = new FileStream(inFileName, FileMode.Open))
            {
                using (FileStream stream2 = new FileStream(outFileName, FileMode.Create))
                {
                    using (CryptoStream stream3 = new CryptoStream(stream, this._rm.CreateDecryptor(), CryptoStreamMode.Read))
                    {
                        int num;
                        while ((num = stream3.ReadByte()) != -1)
                        {
                            stream2.WriteByte((byte) num);
                        }
                    }
                }
            }
        }

        public string DecryptString(string str)
        {
            byte[] buffer2;
            byte[] buffer = Convert.FromBase64String(str);
            using (MemoryStream stream = new MemoryStream(buffer))
            {
                using (CryptoStream stream2 = new CryptoStream(stream, this._rm.CreateDecryptor(), CryptoStreamMode.Read))
                {
                    buffer2 = new byte[buffer.Length];
                    stream2.Read(buffer2, 0, buffer.Length);
                }
            }
            return Encoding.UTF8.GetString(buffer2);
        }

        public void EncryptFile(string inFileName, string outFileName)
        {
            using (FileStream stream = new FileStream(inFileName, FileMode.Open))
            {
                using (FileStream stream2 = new FileStream(outFileName, FileMode.Create))
                {
                    using (CryptoStream stream3 = new CryptoStream(stream2, this._rm.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        int num;
                        while ((num = stream.ReadByte()) != -1)
                        {
                            stream3.WriteByte((byte) num);
                        }
                    }
                }
            }
        }

        public string EncryptString(string str)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(str);
            using (MemoryStream stream = new MemoryStream())
            {
                using (CryptoStream stream2 = new CryptoStream(stream, this._rm.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    stream2.Write(bytes, 0, bytes.Length);
                    stream2.FlushFinalBlock();
                }
                return Convert.ToBase64String(stream.ToArray());
            }
        }

        public static RijndaelCryptography GetGlobalInstance()
        {
            byte[] globalKey = GetGlobalKey();
            return new RijndaelCryptography(globalKey, GetGlobalIV());
        }

        public static byte[] GetGlobalIV()
        {
            return new byte[] { 8, 0x85, 0x7e, 0x3d, 0, 0x4e, 0x86, 0xbb, 0x6d, 100, 0xaf, 0xe0, 0x47, 0x6c, 0x5b, 70 };
        }

        public static byte[] GetGlobalKey()
        {
            return new byte[] { 
                0xa1, 0xd1, 0x39, 12, 0x2d, 0xc5, 110, 0x4f, 0x58, 0x35, 0x61, 12, 0xf1, 0xe5, 0xa2, 0x53, 
                120, 0x36, 2, 0x12, 0x86, 0x69, 0x91, 0x51, 240, 0xe3, 0x1c, 5, 0x4b, 0x8a, 0x99, 0xac
             };
        }

        public static RijndaelCryptography GetRandomInstance()
        {
            byte[] randomKey = GetRandomKey();
            return new RijndaelCryptography(randomKey, GetRandomIV());
        }

        public static byte[] GetRandomIV()
        {
            RijndaelManaged managed = new RijndaelManaged();
            managed.GenerateIV();
            return managed.IV;
        }

        public static byte[] GetRandomKey()
        {
            RijndaelManaged managed = new RijndaelManaged();
            managed.GenerateKey();
            return managed.Key;
        }

        public byte[] IV
        {
            get
            {
                return this._rm.IV;
            }
        }

        public byte[] Key
        {
            get
            {
                return this._rm.Key;
            }
        }
    }
}

