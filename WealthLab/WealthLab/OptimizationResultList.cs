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
        private List<string> list_0 = new List<string>();
        private List<string> list_1 = new List<string>();
        private List<OptimizationResult> list_2 = new List<OptimizationResult>();
        private OptimizationResultList optimizationResultList_0;
        [CompilerGenerated]
        private string string_0;
        [CompilerGenerated]
        private string string_1;
        [CompilerGenerated]
        private string string_2;

        public void Add(OptimizationResult optimizationResult_0)
        {
            this.list_2.Add(optimizationResult_0);
            if (!this.list_1.Contains(optimizationResult_0.Symbol))
            {
                this.list_1.Add(optimizationResult_0.Symbol);
            }
        }

        public void Clear()
        {
            this.list_2.Clear();
            this.Names.Clear();
            this.list_2.Clear();
            this.list_1.Clear();
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
            foreach (OptimizationResult result in this.list_2)
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
            using (List<OptimizationResult>.Enumerator enumerator = this.list_2.GetEnumerator())
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
            using (List<OptimizationResult>.Enumerator enumerator = this.list_2.GetEnumerator())
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
            using (List<OptimizationResult>.Enumerator enumerator = this.list_2.GetEnumerator())
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
                    for (int i = 0; i < this.list_2.Count; i++)
                    {
                        OptimizationResult result = this.list_2[i];
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
                return this.list_0;
            }
            set
            {
                this.list_0 = value;
            }
        }

        public string OptimizationMethod
        {
            [CompilerGenerated]
            get
            {
                return this.string_2;
            }
            [CompilerGenerated]
            set
            {
                this.string_2 = value;
            }
        }

        public List<OptimizationResult> Results
        {
            get
            {
                return this.list_2;
            }
            set
            {
                this.list_2 = value;
            }
        }

        public string Scorecard
        {
            [CompilerGenerated]
            get
            {
                return this.string_1;
            }
            [CompilerGenerated]
            set
            {
                this.string_1 = value;
            }
        }

        public string StrategyID
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

        public List<string> Symbols
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
    }
}

