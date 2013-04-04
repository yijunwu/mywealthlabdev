namespace Fidelity.Components
{
    using System;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;

    public class Cryptography
    {
        public static string Crypt(string s_Data, string s_Password, bool b_Encrypt)
        {
            byte[] buffer2;
            string str;
            byte[] rgbSalt = new byte[] { 0x26, 0x19, 0x81, 0x4e, 160, 0x6d, 0x95, 0x34, 0x26, 0x75, 100, 5, 0xf6 };
            PasswordDeriveBytes bytes = new PasswordDeriveBytes(s_Password, rgbSalt);
            Rijndael rijndael = Rijndael.Create();
            rijndael.Key = bytes.GetBytes(0x20);
            rijndael.IV = bytes.GetBytes(0x10);
            ICryptoTransform transform = b_Encrypt ? rijndael.CreateEncryptor() : rijndael.CreateDecryptor();
            MemoryStream stream = new MemoryStream();
            CryptoStream stream2 = new CryptoStream(stream, transform, CryptoStreamMode.Write);
            if (b_Encrypt)
            {
                buffer2 = Encoding.Unicode.GetBytes(s_Data);
            }
            else
            {
                buffer2 = Convert.FromBase64String(s_Data);
            }
            try
            {
                stream2.Write(buffer2, 0, buffer2.Length);
                stream2.Close();
                if (b_Encrypt)
                {
                    return Convert.ToBase64String(stream.ToArray());
                }
                return Encoding.Unicode.GetString(stream.ToArray());
            }
            catch
            {
                str = null;
            }
            return str;
        }
    }
}

