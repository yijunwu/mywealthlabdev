namespace WealthLab.Optimizers
{
    using Fidelity.Components;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;
    using WealthLab;

    public class MonteCarlo : Optimizer, ICustomSettings
    {
        private List<MonteCarloResult> _resultHistory = new List<MonteCarloResult>();
        private Random _rnd = new Random();
        private int _runNumber;
        private int _testNumber;
        private List<double> _windowCenters = new List<double>();
        private List<double> _windowRadius = new List<double>();

        public void ChangeSettings(UserControl ui)
        {
            MonteCarloSettings settings = ui as MonteCarloSettings;
            this.MetricName = settings.Metric;
            this.HighestValue = settings.HighestValue;
            this.RunCount = settings.RunCount;
            this.TestCount = settings.TestCount;
        }

        public override void FirstRun()
        {
            this._runNumber = 1;
            this._testNumber = 1;
            this._windowCenters.Clear();
            this._windowRadius.Clear();
            foreach (StrategyParameter parameter in base.WealthScript.Parameters)
            {
                MonteCarloRunInfo info = new MonteCarloRunInfo();
                double num = parameter.Stop - parameter.Start;
                info.Radius = num / 2.0;
                info.Center = parameter.Start + info.Radius;
                parameter.OptimizerTag = info;
            }
            this.RandomizeParameters();
        }

        public UserControl GetSettingsUI()
        {
            MonteCarloSettings settings = new MonteCarloSettings();
            settings.PopulateMetric(base.Host.MetricNames);
            settings.Metric = this.MetricName;
            settings.HighestValue = this.HighestValue;
            settings.RunCount = this.RunCount;
            settings.TestCount = this.TestCount;
            return settings;
        }

        public override void Initialize()
        {
        }

        public override bool NextRun(SystemPerformance sp, OptimizationResult or)
        {
            MonteCarloResult item = new MonteCarloResult();
            foreach (StrategyParameter parameter in base.WealthScript.Parameters)
            {
                item.Values.Add(parameter.Value);
            }
            int index = base.Host.MetricNames.IndexOf(this.MetricName);
            if (index >= 0)
            {
                item.MetricValue = or.Results[index];
            }
            this._resultHistory.Add(item);
            if (this._testNumber < this.TestCount)
            {
                this._testNumber++;
                this.RandomizeParameters();
                return true;
            }
            if (this._runNumber == this.RunCount)
            {
                return false;
            }
            MonteCarloResult result2 = null;
            double minValue = double.MinValue;
            double num3 = this.HighestValue ? ((double) 1) : ((double) (-1));
            foreach (MonteCarloResult result3 in this._resultHistory)
            {
                if ((result3.MetricValue * num3) > minValue)
                {
                    minValue = result3.MetricValue * num3;
                    result2 = result3;
                }
            }
            for (int i = 0; i < (base.WealthScript.Parameters.Count - 1); i++)
            {
                StrategyParameter parameter2 = base.WealthScript.Parameters[i];
                MonteCarloRunInfo optimizerTag = parameter2.OptimizerTag as MonteCarloRunInfo;
                optimizerTag.Center = result2.Values[i];
                double num5 = parameter2.Stop - parameter2.Start;
                double num6 = num5 / ((double) this.RunCount);
                optimizerTag.Radius -= num6 / 2.0;
            }
            this._resultHistory.Clear();
            this._testNumber = 1;
            this._runNumber++;
            this.RandomizeParameters();
            return true;
        }

        private void RandomizeParameters()
        {
            foreach (StrategyParameter parameter in base.WealthScript.Parameters)
            {
                MonteCarloRunInfo optimizerTag = parameter.OptimizerTag as MonteCarloRunInfo;
                do
                {
                    double num = optimizerTag.Radius * 2.0;
                    double num2 = this._rnd.NextDouble() * num;
                    double num3 = (optimizerTag.Center + num2) - optimizerTag.Radius;
                    if (parameter.IsInteger)
                    {
                        num3 = (int) num3;
                    }
                    parameter.Value = num3;
                }
                while ((parameter.Value < parameter.Start) || (parameter.Value > parameter.Stop));
            }
        }

        public void ReadSettings(ISettingsHost host)
        {
            this.MetricName = host.Get("MetricName", "Net Profit");
            this.HighestValue = host.Get("HighestValue", true);
            this.RunCount = host.Get("RunCount", 20);
            this.TestCount = host.Get("TestCount", 10);
        }

        public void WriteSettings(ISettingsHost host)
        {
            host.Set("MetricName", this.MetricName);
            host.Set("HighestValue", this.HighestValue);
            host.Set("RunCount", this.RunCount);
            host.Set("TestCount", this.TestCount);
        }

        public override string Description
        {
            get
            {
                return "Monte Carlo Optimization examines several random Parameter domains and zeroes in on one profitable Parameter set.";
            }
        }

        public override string FriendlyName
        {
            get
            {
                return "Monte Carlo";
            }
        }

        internal bool HighestValue { get; set; }

        internal string MetricName { get; set; }

        public override double NumberOfRuns
        {
            get
            {
                return (double) (this.RunCount * this.TestCount);
            }
        }

        internal int RunCount { get; set; }

        internal int TestCount { get; set; }
    }
}

