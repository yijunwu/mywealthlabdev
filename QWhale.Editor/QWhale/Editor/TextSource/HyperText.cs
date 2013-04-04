namespace QWhale.Editor.TextSource
{
    using QWhale.Editor;
    using System;
    using System.Collections;

    public class HyperText
    {
        private static Hashtable identsTable;

        private static void InitIdentsTable()
        {
            if (identsTable == null)
            {
                identsTable = new Hashtable();
                for (char ch = 'A'; ch <= 'Z'; ch = (char) (ch + '\x0001'))
                {
                    identsTable.Add(ch, ch);
                }
                for (char ch2 = 'a'; ch2 <= 'z'; ch2 = (char) (ch2 + '\x0001'))
                {
                    identsTable.Add(ch2, ch2);
                }
                for (char ch3 = '0'; ch3 <= '9'; ch3 = (char) (ch3 + '\x0001'))
                {
                    identsTable.Add(ch3, ch3);
                }
                identsTable.Add('_', '_');
            }
        }

        public static bool IsEmailString(string text)
        {
            bool flag = string.Compare(text, 0, EditConsts.MailTo, 0, EditConsts.MailTo.Length, true) == 0;
            if (!flag)
            {
                int length = text.Length;
                int index = text.IndexOf("@");
                int num3 = text.LastIndexOf("@");
                int num4 = text.LastIndexOf(".");
                flag = (((index >= 0) && (index == num3)) && (num4 > index)) && (index != (length - 1));
                if (flag)
                {
                    InitIdentsTable();
                    flag = identsTable.ContainsKey(text[index]);
                }
            }
            return flag;
        }

        public static bool IsFileString(string text)
        {
            return (string.Compare(text, 0, EditConsts.FileProtocol, 0, EditConsts.FileProtocol.Length, true) == 0);
        }

        public static bool IsFtpString(string text)
        {
            return (string.Compare(text, 0, EditConsts.FTPProtocol, 0, EditConsts.FTPProtocol.Length, true) == 0);
        }

        public static bool IsGopherString(string text)
        {
            return (string.Compare(text, 0, EditConsts.GopherProtocol, 0, EditConsts.GopherProtocol.Length, true) == 0);
        }

        public static bool IsHttpString(string text)
        {
            return (string.Compare(text, 0, EditConsts.HTTPProtocol, 0, EditConsts.HTTPProtocol.Length, true) == 0);
        }

        public static bool IsHyperText(string text)
        {
            if (((!IsEmailString(text) && !IsWWWString(text)) && (!IsHttpString(text) && !IsFtpString(text))) && !IsGopherString(text))
            {
                return IsFileString(text);
            }
            return true;
        }

        public static bool IsWWWString(string text)
        {
            return (string.Compare(text, 0, EditConsts.WWW, 0, EditConsts.WWW.Length, true) == 0);
        }
    }
}

