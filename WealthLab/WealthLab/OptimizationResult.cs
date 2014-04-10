namespace WealthLab
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class OptimizationResult
    {
        private List<double> parameterValues;
        private List<double> results;
        [CompilerGenerated]
        private string symbol;

        public OptimizationResult()
        {
            this.parameterValues = new List<double>();
            this.results = new List<double>();
        }

        public OptimizationResult(string symbol)
        {
            this.parameterValues = new List<double>();
            this.results = new List<double>();
            this.Symbol = symbol;
        }

        public List<double> ParameterValues
        {
            get
            {
                return this.parameterValues;
            }
            set
            {
                this.parameterValues = value;
            }
        }

        public List<double> Results
        {
            get
            {
                return this.results;
            }
            set
            {
                this.results = value;
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

