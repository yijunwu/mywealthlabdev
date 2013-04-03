namespace WealthLab
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class OptimizationResult
    {
        private List<double> list_0;
        private List<double> list_1;
        [CompilerGenerated]
        private string string_0;

        public OptimizationResult()
        {
            this.list_0 = new List<double>();
            this.list_1 = new List<double>();
        }

        public OptimizationResult(string symbol)
        {
            this.list_0 = new List<double>();
            this.list_1 = new List<double>();
            this.Symbol = symbol;
        }

        public List<double> ParameterValues
        {
            get
            {
                return this.list_0;
            }
            set
            {
                this.list_0 = value;
            }
        }

        public List<double> Results
        {
            get
            {
                return this.list_1;
            }
            set
            {
                this.list_1 = value;
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

