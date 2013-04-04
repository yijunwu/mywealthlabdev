namespace QWhale.Common
{
    using System;
    using System.Drawing;

    public class Consts
    {
        public const char AveChar = 'x';
        public static Color BlueItemsAreaColor = Color.FromArgb(0xd6, 0xdf, 0xf7);
        public const char CR = '\r';
        public const string CRLF = "\r\n";
        public static char[] crlfArray = new char[] { '\r', '\n' };
        public static Color DefaultControlBackColor = SystemColors.Window;
        public static Color DefaultControlForeColor = SystemColors.WindowText;
        public static Color DefaultHotTrackBackColor = Color.FromArgb(0xc1, 210, 0xee);
        public static Color DefaultHotTrackColor = SystemColors.HotTrack;
        public static Color DefaultPressedBackColor = Color.FromArgb(0x98, 0xb5, 0xe2);
        public static Color DefaultWindowFrameColor = SystemColors.WindowFrame;
        public static Color DefaultXPHeaderForeColor = Color.FromArgb(0x21, 0x5d, 0xc6);
        public static Color DefaultXPItemsForeColor = Color.FromArgb(0x42, 0x8e, 0xff);
        public static Color HomeSteadItemsAreaColor = Color.FromArgb(0xf6, 0xf6, 0xec);
        public const char LF = '\n';
        public const char LongSpace = '　';
        public static Color MetallicItemsAreaColor = Color.FromArgb(240, 0xf1, 0xf5);
        public const char Space = ' ';
        public const char Tab = '\t';
        public const string TabStr = "\t";

        private Consts()
        {
        }
    }
}

