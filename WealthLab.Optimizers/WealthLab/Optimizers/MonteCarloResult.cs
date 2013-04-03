namespace WealthLab.Optimizers
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class MonteCarloResult
    {
        private List<double> _values = new List<double>();

        public double MetricValue { get; set; }

        public List<double> Values
        {
            get
            {
                return this._values;
            }
        }
    }
}

