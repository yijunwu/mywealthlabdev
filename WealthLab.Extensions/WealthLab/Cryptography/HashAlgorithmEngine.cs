namespace WealthLab.Cryptography
{
    using System;

    public class HashAlgorithmEngine
    {
        public static string MD5(string string_0)
        {
            return Convert.ToBase64String(RSACryptography.GetMD5Hash(string_0));
        }

        public static byte[] MD5(byte[] data)
        {
            return RSACryptography.GetMD5Hash(data);
        }
    }
}

