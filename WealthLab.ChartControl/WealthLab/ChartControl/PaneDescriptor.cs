namespace WealthLab.ChartControl
{
    using System;
    using System.IO;

    public class PaneDescriptor
    {
        private bool bool_0;
        private int int_0;
        private string string_0;

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
                return this.bool_0;
            }
            set
            {
                this.bool_0 = value;
            }
        }

        public string Description
        {
            get
            {
                return this.string_0;
            }
            set
            {
                this.string_0 = value;
            }
        }

        public int Height
        {
            get
            {
                return this.int_0;
            }
            set
            {
                this.int_0 = value;
            }
        }
    }
}

