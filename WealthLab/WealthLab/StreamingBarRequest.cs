namespace WealthLab
{
    using System;
    using System.Runtime.CompilerServices;

    public class StreamingBarRequest
    {
        [CompilerGenerated]
        private BarGenerator barGenerator;
        [CompilerGenerated]
        private int barInterval;
        [CompilerGenerated]
        private IStreamingUpdate istreamingUpdate;
        [CompilerGenerated]
        private string symbol;

        public int BarInterval
        {
            [CompilerGenerated]
            get
            {
                return this.barInterval;
            }
            [CompilerGenerated]
            set
            {
                this.barInterval = value;
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
                return this.barGenerator;
            }
            [CompilerGenerated]
            set
            {
                this.barGenerator = value;
            }
        }

        public IStreamingUpdate Request
        {
            [CompilerGenerated]
            get
            {
                return this.istreamingUpdate;
            }
            [CompilerGenerated]
            set
            {
                this.istreamingUpdate = value;
            }
        }

        public string Symbol
        {
            [CompilerGenerated]
            get
            {
                return this.symbol;
            }
            [CompilerGenerated]
            set
            {
                this.symbol = value;
            }
        }
    }
}

