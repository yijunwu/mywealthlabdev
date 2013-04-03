namespace WealthLab
{
    using System;
    using System.Runtime.CompilerServices;

    public class StreamingBarRequest
    {
        [CompilerGenerated]
        private BarGenerator barGenerator_0;
        [CompilerGenerated]
        private int int_0;
        [CompilerGenerated]
        private IStreamingUpdate istreamingUpdate_0;
        [CompilerGenerated]
        private string string_0;

        public int BarInterval
        {
            [CompilerGenerated]
            get
            {
                return this.int_0;
            }
            [CompilerGenerated]
            set
            {
                this.int_0 = value;
            }
        }

        public string Code
        {
            get
            {
                return (this.Symbol + "_" + this.BarInterval);
            }
        }

        public BarGenerator Generator
        {
            [CompilerGenerated]
            get
            {
                return this.barGenerator_0;
            }
            [CompilerGenerated]
            set
            {
                this.barGenerator_0 = value;
            }
        }

        public IStreamingUpdate Request
        {
            [CompilerGenerated]
            get
            {
                return this.istreamingUpdate_0;
            }
            [CompilerGenerated]
            set
            {
                this.istreamingUpdate_0 = value;
            }
        }

        public string Symbol
        {
            [CompilerGenerated]
            get
            {
                return this.string_0;
            }
            [CompilerGenerated]
            set
            {
                this.string_0 = value;
            }
        }
    }
}

