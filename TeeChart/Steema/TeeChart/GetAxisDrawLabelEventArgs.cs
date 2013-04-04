namespace Steema.TeeChart
{
    using System;

    public class GetAxisDrawLabelEventArgs : EventArgs
    {
        private bool drawlabel;
        private string text;
        private int x;
        private int y;
        private int z;

        public GetAxisDrawLabelEventArgs(int X, int Y, int Z, string Text, bool DrawLabel)
        {
            this.x = X;
            this.y = Y;
            this.z = Z;
            this.text = Text;
            this.drawlabel = DrawLabel;
        }

        public bool DrawLabel
        {
            get
            {
                return this.drawlabel;
            }
            set
            {
                this.drawlabel = value;
            }
        }

        public string Text
        {
            get
            {
                return this.text;
            }
            set
            {
                this.text = value;
            }
        }

        public int X
        {
            get
            {
                return this.x;
            }
            set
            {
                this.x = value;
            }
        }

        public int Y
        {
            get
            {
                return this.y;
            }
            set
            {
                this.y = value;
            }
        }

        public int Z
        {
            get
            {
                return this.z;
            }
            set
            {
                this.z = value;
            }
        }
    }
}

