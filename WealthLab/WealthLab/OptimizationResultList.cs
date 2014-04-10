namespace WealthLab
{
    using Fidelity.Components;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Xml.Serialization;

    public class OptimizationResultList
    {
        private List<string> names = new List<string>();
        private List<string> symbols = new List<string>();
        private List<OptimizationResult> results = new List<OptimizationResult>();
        private OptimizationResultList optimizationResultList_0;
        [CompilerGenerated]
        private string strategyID;
        [CompilerGenerated]
        private string scorecard;
        [CompilerGenerated]
        private string optimizationMethod;

        public void Add(OptimizationResult optimizationResult_0)
        {
            this.results.Add(optimizationResult_0);
            if (!this.symbols.Contains(optimizationResult_0.Symbol))
            {
                this.symbols.Add(optimizationResult_0.Symbol);
            }
        }

        public void Clear()
        {
            this.results.Clear();
            this.Names.Clear();
            this.results.Clear();
            this.symbols.Clear();
            this.optimizationResultList_0 = null;
        }

        public List<double> FindBestMetric(string symbol, string metric, double mult)
        {
            int index = this.Names.IndexOf(metric);
            if (index == -1)
            {
                throw new ArgumentException("Bad metric name: " + metric);
            }
            List<double> parameterValues = null;
            double minValue = double.MinValue;
            foreach (OptimizationResult result in this.results)
            {
                if (result.Symbol == symbol)
                {
                    double num2 = result.Results[index] * mult;
                    if (num2 > minValue)
                    {
                        parameterValues = result.ParameterValues;
                        minValue = num2;
                    }
                }
            }
            return parameterValues;
        }

        public double FindMetric(string symbol, string metric, List<double> values)
        {
            if (symbol == "(Average)")
            {
                return this.AverageResultList.FindMetric("<Average>", metric, values);
            }
            using (List<OptimizationResult>.Enumerator enumerator = this.results.GetEnumerator())
            {
                int num;
                OptimizationResult current;
                while (enumerator.MoveNext())
                {
                    current = enumerator.Current;
                    bool flag = true;
                    for (int i = 0; i < values.Count; i++)
                    {
                        if ((values[i] != current.ParameterValues[i]) || (current.Symbol != symbol))
                        {
                            //goto  Label_0076;   ///WYJ fix, simplify the flow
                            flag = false;
                            break;
                        }
                    }
                    if (flag)
                    {
                        ///goto  Label_0086;
                        num = this.Names.IndexOf(metric);
                        return current.Results[num];
                    }
                }
                return double.NaN;
            }
        }

        public double FindMetric(string symbol, string metric, WealthScript wealthScript_0, StrategyParameter strategyParameter_0, double paramValue)
        {
            if (symbol == "(Average)")
            {
                return this.AverageResultList.FindMetric("<Average>", metric, wealthScript_0, strategyParameter_0, paramValue);
            }
            int index = wealthScript_0.Parameters.IndexOf(strategyParameter_0);
            using (List<OptimizationResult>.Enumerator enumerator = this.results.GetEnumerator())
            {
                int num2;
                OptimizationResult current;
                while (enumerator.MoveNext())
                {
                    current = enumerator.Current;
                    if ((current.ParameterValues[index] != paramValue) || !(current.Symbol == symbol))
                    {
                        continue;
                    }
                    bool flag = true;
                    for (int i = 0; i < wealthScript_0.Parameters.Count; i++)
                    {
                        if ((i != index) && (current.ParameterValues[i] != wealthScript_0.Parameters[i].Value))
                        {
                            ///goto  Label_00AD; ///WYJ fix, simplify the flow
                            flag = false;
                            break;
                        }
                    }
                    if (flag)
                    {
                        ///goto  Label_00BD;
                        num2 = this.Names.IndexOf(metric);
                        return current.Results[num2];
                    }
                }
                return double.NaN;
            }
        }

        public OptimizationResult FindResult(string symbol, List<double> values)
        {
            OptimizationResult result2;
            using (List<OptimizationResult>.Enumerator enumerator = this.results.GetEnumerator())
            {
                OptimizationResult current;
                while (enumerator.MoveNext())
                {
                    current = enumerator.Current;
                    if (!(current.Symbol == symbol))
                    {
                        continue;
                    }
                    bool flag = true;
                    for (int i = 0; i < values.Count; i++)
                    {
                        if (values[i] != current.ParameterValues[i])
                        {
                            ///goto  Label_0051;  ///WYJ fix, simplify the flow
                            flag = false;
                            break;
                        }
                    }
                    if (flag)
                    {
                        ///goto  Label_0061;
                        result2 = current;
                        return result2;
                    }
                }
                return null;
            }
        }

        public static OptimizationResultList LoadFromFile(string fileName)
        {
            FileStream stream = null;
            OptimizationResultList list;
            try
            {
                stream = File.Open(fileName, FileMode.Open, FileAccess.Read);
                XmlSerializer serializer = new XmlSerializer(typeof(OptimizationResultList));
                list = (OptimizationResultList) serializer.Deserialize(stream);
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }
            return list;
        }

        public void SaveToFile(string fileName)
        {
            FileNameValidator.ValidateFileName(fileName);
            FileStream stream = null;
            try
            {
                stream = File.Create(fileName);
                new XmlSerializer(typeof(OptimizationResultList)).Serialize((Stream) stream, this);
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }
        }

        [XmlIgnore]
        public OptimizationResultList AverageResultList
        {
            get
            {
                if (this.optimizationResultList_0 == null)
                {
                    this.optimizationResultList_0 = new OptimizationResultList();
                    foreach (string str3 in this.Names)
                    {
                        this.optimizationResultList_0.Names.Add(str3);
                    }
                    if (this.Symbols.Count == 0)
                    {
                        return this.optimizationResultList_0;
                    }
                    string str2 = this.Symbols[0];
                    for (int i = 0; i < this.results.Count; i++)
                    {
                        OptimizationResult result = this.results[i];
                        if (result.Symbol == str2)
                        {
                            OptimizationResult result2 = new OptimizationResult("<Average>");
                            foreach (double num6 in result.ParameterValues)
                            {
                                result2.ParameterValues.Add(num6);
                            }
                            for (int j = 0; j < result.Results.Count; j++)
                            {
                                result2.Results.Add(0.0);
                            }
                            foreach (string str in this.Symbols)
                            {
                                OptimizationResult result3 = this.FindResult(str, result.ParameterValues);
                                if (result3 != null)
                                {
                                    for (int m = 0; m < result3.Results.Count; m++)
                                    {
                                        List<double> list;
                                        int num4;
                                        (list = result2.Results)[num4 = m] = list[num4] + result3.Results[m];
                                    }
                                }
                            }
                            for (int k = 0; k < result2.Results.Count; k++)
                            {
                                List<double> list2;
                                int num7;
                                (list2 = result2.Results)[num7 = k] = list2[num7] / ((double) this.Symbols.Count);
                            }
                            this.optimizationResultList_0.Add(result2);
                        }
                    }
                }
                return this.optimizationResultList_0;
            }
        }

        public List<string> Names
        {
            get
            {
                return this.names;
            }
            set
            {
                this.names = value;
            }
        }

        public string OptimizationMethod
        {
            [CompilerGenerated]
            get
            {
                return this.optimizationMethod;
            }
            [CompilerGenerated]
            set
            {
                this.optimizationMethod = value;
            }
        }

        public List<OptimizationResult> Results
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

        public string Scorecard
        {
            [CompilerGenerated]
            get
            {
                return this.scorecard;
            }
            [CompilerGenerated]
            set
            {
                this.scorecard = value;
            }
        }

        public string StrategyID
        {
            [CompilerGenerated]
            get
            {
                return this.strategyID;
            }
            [CompilerGenerated]
            set
            {
                this.strategyID = value;
            }
        }

        public List<string> Symbols
        {
            get
            {
                return this.symbols;
            }
            set
            {
                this.symbols = value;
            }
        }
    }
}

