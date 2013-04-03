namespace WealthLab
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct StreamingRequest
    {
        private string string_0;
        private IStreamingUpdate istreamingUpdate_0;
        private WealthLab.MarketInfo marketInfo_0;
        public StreamingRequest(string symbol, IStreamingUpdate request, WealthLab.MarketInfo info)
        {
            this.string_0 = symbol;
            this.istreamingUpdate_0 = request;
            this.marketInfo_0 = info;
        }

        public string Symbol
        {
            get
            {
                return this.string_0;
            }
        }
        public IStreamingUpdate Request
        {
            get
            {
                return this.istreamingUpdate_0;
            }
        }
        public WealthLab.MarketInfo MarketInfo
        {
            get
            {
                return this.marketInfo_0;
            }
        }
    }
}

