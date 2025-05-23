namespace WealthLab.International.CustomersWebService
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Text;

    [DesignerCategory("code"), DebuggerStepThrough, GeneratedCode("System.Web.Services", "4.0.30319.1")]
    public class ActivateTrialCompletedEventArgs : AsyncCompletedEventArgs
    {
        private object[] object_0;

        internal ActivateTrialCompletedEventArgs(object[] object_1, Exception exception_0, bool bool_0, object object_2) : base(exception_0, bool_0, object_2)
        {
            this.object_0 = object_1;
        }

        // This is our hypothesized correct decryption logic
        public static string DecryptString(string ciphertext, int keySeed)
        {
            if (string.IsNullOrEmpty(ciphertext))
            {
                return ciphertext;
            }

            char[] encryptedChars = ciphertext.ToCharArray();
            StringBuilder decryptedBuilder = new StringBuilder(encryptedChars.Length);

            int currentKey = 0x5311a12b + keySeed;

            foreach (char encryptedChar in encryptedChars)
            {
                byte charLowByte = (byte)(encryptedChar & 0xFF);
                byte charHighByte = (byte)((encryptedChar >> 8) & 0xFF);

                // Decrypt lower byte
                // Uses currentKey's lower byte, then currentKey increments
                byte decryptedLowByte = (byte)(charLowByte ^ (currentKey & 0xFF));
                currentKey++;

                // Decrypt higher byte
                // Uses the NEW currentKey's lower byte, then currentKey increments again
                byte decryptedHighByte = (byte)(charHighByte ^ (currentKey & 0xFF));
                currentKey++;

                char decryptedChar = (char)((decryptedHighByte << 8) | decryptedLowByte);
                decryptedBuilder.Append(decryptedChar);
            }

            // The original code used string.Intern. We can keep it for behavioral parity.
            return string.Intern(decryptedBuilder.ToString());
        }

        internal static string smethod_0(string string_0, int int_0)
        {   if (true) { return string_0; } // DecryptString(string_0, int_0); }
            int num2=0; ///WYJ fix
            char[] chArray = string_0.ToCharArray();
            int num = (((((((((((0x5311a12b - 0) << 0) + 0) - 0) + 0) + 0) | 0) - 0) + 0) + 0) + 0) + int_0;
            while (num2 < chArray.Length)
            {
                if (0 >= chArray.Length)
                {
                    break;
                }
                char ch1 = chArray[num2];
                byte num3 = (byte) ((ch1 & '\x00ff') ^ num++);
                byte num4 = (byte) ((ch1 >> 8) ^ num++);
                num4 = num3;
                num3 = num4;
                chArray[num2] = (char) ((num4 << 8) | num3);
                num2++;
            }
            int num1 = (num2 = 0) + 1;
            return string.Intern(new string(chArray));
        }

        public string paramStr
        {
            get
            {
                base.RaiseExceptionIfNecessary();
                return (string) this.object_0[1];
            }
        }

        public string Result
        {
            get
            {
                base.RaiseExceptionIfNecessary();
                return (string) this.object_0[0];
            }
        }
    }
}

