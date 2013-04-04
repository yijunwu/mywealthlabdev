namespace Steema.TeeChart.Tools
{
    using System;
    using System.Collections;
    using System.Drawing;

    public class ScrollAttributes : ZoomAttributes
    {
        public Bitmap b = new Bitmap(1, 1);
        public int bottomAxisPos;
        public Rectangle destRect = new Rectangle();
        public ArrayList frames = new ArrayList();
        public Rectangle grabRect = new Rectangle();
        public Hashtable imgList;
        public int mouseAction = 1;
        public Rectangle newChartRect = new Rectangle();
        public int newLeftWall;
        public int newWidth;
        public int oldHeight;
        public int oldWidth;
        public Rectangle renderRect = new Rectangle();
        public int rightAxisMargin;
        public int scrollPages = 4;
        public Rectangle sourceRect = new Rectangle();
        public int startPos;
    }
}

