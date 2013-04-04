namespace QWhale.Syntax.Serialization
{
    using System;
    using System.Drawing;

    public class XmlColorInfo
    {
        public static Color DeserializeColor(string color)
        {
            if (color != string.Empty)
            {
                string[] strArray = color.Split(new char[] { ':' });
                if (strArray.Length == 1)
                {
                    return Color.FromName(strArray[0]);
                }
                if (strArray.Length == 4)
                {
                    byte alpha = byte.Parse(strArray[0]);
                    byte red = byte.Parse(strArray[1]);
                    byte green = byte.Parse(strArray[2]);
                    byte blue = byte.Parse(strArray[3]);
                    return Color.FromArgb(alpha, red, green, blue);
                }
            }
            return Color.Empty;
        }

        public static string SerializeColor(Color color)
        {
            if (color.IsNamedColor)
            {
                return color.Name;
            }
            if (color == Color.Empty)
            {
                return string.Empty;
            }
            return string.Format("{0}:{1}:{2}:{3}", new object[] { color.A, color.R, color.G, color.B });
        }
    }
}

