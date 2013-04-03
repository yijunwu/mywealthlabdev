namespace WealthLab.Cryptography
{
    using System;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;

    public class RijndaelCryptography
    {
        private RijndaelManaged rijndaelManaged_0 = new RijndaelManaged();

        public RijndaelCryptography(byte[] byte_0, byte[] byte_1)
        {
            this.rijndaelManaged_0.Key = byte_0;
            this.rijndaelManaged_0.IV = byte_1;
        }

        public void DecryptFile(string inFileName, string outFileName)
        {
            using (FileStream stream = new FileStream(inFileName, FileMode.Open))
            {
                using (FileStream stream2 = new FileStream(outFileName, FileMode.Create))
                {
                    using (CryptoStream stream3 = new CryptoStream(stream, this.rijndaelManaged_0.CreateDecryptor(), CryptoStreamMode.Read))
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

        public string DecryptString(string string_0)
        {
            byte[] buffer2;
            byte[] buffer = Convert.FromBase64String(string_0);
            using (MemoryStream stream = new MemoryStream(buffer))
            {
                using (CryptoStream stream2 = new CryptoStream(stream, this.rijndaelManaged_0.CreateDecryptor(), CryptoStreamMode.Read))
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
                    using (CryptoStream stream3 = new CryptoStream(stream2, this.rijndaelManaged_0.CreateEncryptor(), CryptoStreamMode.Write))
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

        public string EncryptString(string string_0)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(string_0);
            using (MemoryStream stream = new MemoryStream())
            {
                using (CryptoStream stream2 = new CryptoStream(stream, this.rijndaelManaged_0.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    stream2.Write(bytes, 0, bytes.Length);
                    stream2.FlushFinalBlock();
                }
                return Convert.ToBase64String(stream.ToArray());
            }
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
                return this.rijndaelManaged_0.IV;
            }
        }

        public byte[] Key
        {
            get
            {
                return this.rijndaelManaged_0.Key;
            }
        }
    }
}

