namespace WealthLab.ChartControl
{
    using System;
    using System.IO;

    public class PaneDescriptor
    {
        private bool abovePricePane;
        private int height;
        private string description;

        public void Read(BinaryReader binaryReader_0)
        {
            this.AbovePricePane = binaryReader_0.ReadBoolean();
            this.Description = binaryReader_0.ReadString();
            this.Height = binaryReader_0.ReadInt32();
        }

        public void Write(BinaryWriter binaryWriter_0)
        {
            binaryWriter_0.Write(this.AbovePricePane);
            binaryWriter_0.Write(this.Description);
            binaryWriter_0.Write(this.Height);
        }

        public bool AbovePricePane
        {
            get
            {
                return this.abovePricePane;
            }
            set
            {
                this.abovePricePane = value;
            }
        }

        public string Description
        {
            get
            {
                return this.description;
            }
            set
            {
                this.description = value;
            }
        }

        public int Height
        {
            get
            {
                return this.height;
            }
            set
            {
                this.height = value;
            }
        }
    }
}

