namespace MS123.Web.Strategies
{
    using System;
    using System.Security.Cryptography;
    using System.Text;

    public class RSACryptography
    {
        private RSACryptoServiceProvider _rsa;

        public RSACryptography()
        {
            this._rsa = new RSACryptoServiceProvider(0x400);
        }

        public RSACryptography(string key) : this()
        {
            this._rsa.FromXmlString(key);
        }

        public byte[] CreateSignature(byte[] hash)
        {
            RSAPKCS1SignatureFormatter formatter = new RSAPKCS1SignatureFormatter(this._rsa);
            formatter.SetHashAlgorithm("MD5");
            return formatter.CreateSignature(hash);
        }

        public byte[] DecryptData(byte[] data)
        {
            return this._rsa.Decrypt(data, false);
        }

        public string DecryptString(string str)
        {
            byte[] data = Convert.FromBase64String(str);
            return Encoding.UTF8.GetString(this.DecryptData(data));
        }

        public byte[] EncryptData(byte[] data)
        {
            return this._rsa.Encrypt(data, false);
        }

        public string EncryptString(string str)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(str);
            return Convert.ToBase64String(this.EncryptData(bytes));
        }

        public static byte[] GetMD5Hash(byte[] data)
        {
            HashAlgorithm algorithm = new MD5CryptoServiceProvider();
            return algorithm.ComputeHash(data);
        }

        public static byte[] GetMD5Hash(string str)
        {
            return GetMD5Hash(Encoding.UTF8.GetBytes(str));
        }

        public bool VerifySignature(byte[] hash, byte[] signature)
        {
            RSAPKCS1SignatureDeformatter deformatter = new RSAPKCS1SignatureDeformatter(this._rsa);
            deformatter.SetHashAlgorithm("MD5");
            return deformatter.VerifySignature(hash, signature);
        }

        public RSACryptoServiceProvider CryptoServiceProvider
        {
            get
            {
                return this._rsa;
            }
        }
    }
}

