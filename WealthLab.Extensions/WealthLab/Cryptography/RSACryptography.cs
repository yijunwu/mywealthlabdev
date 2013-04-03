namespace WealthLab.Cryptography
{
    using System;
    using System.Security.Cryptography;
    using System.Text;

    public class RSACryptography
    {
        private RSACryptoServiceProvider rsacryptoServiceProvider_0;

        public RSACryptography()
        {
            this.rsacryptoServiceProvider_0 = new RSACryptoServiceProvider(0x400);
        }

        public RSACryptography(string string_0) : this()
        {
            this.rsacryptoServiceProvider_0.FromXmlString(string_0);
        }

        public byte[] CreateSignature(byte[] hash)
        {
            RSAPKCS1SignatureFormatter formatter = new RSAPKCS1SignatureFormatter(this.rsacryptoServiceProvider_0);
            formatter.SetHashAlgorithm("MD5");
            return formatter.CreateSignature(hash);
        }

        public byte[] DecryptData(byte[] data)
        {
            return this.rsacryptoServiceProvider_0.Decrypt(data, false);
        }

        public string DecryptString(string string_0)
        {
            byte[] data = Convert.FromBase64String(string_0);
            return Encoding.UTF8.GetString(this.DecryptData(data));
        }

        public byte[] EncryptData(byte[] data)
        {
            return this.rsacryptoServiceProvider_0.Encrypt(data, false);
        }

        public string EncryptString(string string_0)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(string_0);
            return Convert.ToBase64String(this.EncryptData(bytes));
        }

        public static byte[] GetMD5Hash(byte[] data)
        {
            HashAlgorithm algorithm = new MD5CryptoServiceProvider();
            return algorithm.ComputeHash(data);
        }

        public static byte[] GetMD5Hash(string string_0)
        {
            return GetMD5Hash(Encoding.UTF8.GetBytes(string_0));
        }

        public bool VerifySignature(byte[] hash, byte[] signature)
        {
            RSAPKCS1SignatureDeformatter deformatter = new RSAPKCS1SignatureDeformatter(this.rsacryptoServiceProvider_0);
            deformatter.SetHashAlgorithm("MD5");
            return deformatter.VerifySignature(hash, signature);
        }

        public RSACryptoServiceProvider CryptoServiceProvider
        {
            get
            {
                return this.rsacryptoServiceProvider_0;
            }
        }
    }
}

