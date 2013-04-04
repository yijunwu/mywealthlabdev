namespace MS123.Web.Strategies
{
    using System;

    public class HashAlgorithmEngine
    {
        public static string MD5(string str)
        {
            return Convert.ToBase64String(RSACryptography.GetMD5Hash(str));
        }

        public static byte[] MD5(byte[] data)
        {
            return RSACryptography.GetMD5Hash(data);
        }
    }
}

