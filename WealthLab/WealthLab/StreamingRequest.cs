namespace WealthLab
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct StreamingRequest
    {
        private string symbol;
        private IStreamingUpdate istreamingUpdate;
        private WealthLab.MarketInfo marketInfo;
        public StreamingRequest(string symbol, IStreamingUpdate request, WealthLab.MarketInfo info)
        {
            this.symbol = symbol;
            this.istreamingUpdate = request;
            this.marketInfo = info;
        }

        public string Symbol
        {
            get
            {
                return this.symbol;
            }
        }
        public IStreamingUpdate Request
        {
            get
            {
                return this.istreamingUpdate;
            }
        }
        public WealthLab.MarketInfo MarketInfo
        {
            get
            {
                return this.marketInfo;
            }
        }
    }
}

