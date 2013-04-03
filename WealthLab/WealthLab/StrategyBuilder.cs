namespace WealthLab
{
    using Fidelity.Components;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Globalization;
    using System.IO;
    using System.Text;
    using System.Xml.Serialization;
    using WealthLab.Properties;

    [ToolboxBitmap(typeof(StrategyBuilder), "StrategyBuilder")]
    public class StrategyBuilder : Component, IComparer<Rule>, IComparer<StrategyRule>
    {
        private static CultureInfo cultureInfo_0 = new CultureInfo("en-US");
        private static Dictionary<string, string> dictionary_0 = new Dictionary<string, string>();
        private Dictionary<string, string> dictionary_1;
        private GridLines gridLines_0;
        private IContainer icontainer_0;
        private int int_0;
        private int int_1;
        private int int_2;
        private int int_3;
        private int int_4;
        private List<Rule> list_0;
        private List<string> list_1;
        private List<string> list_2;
        private List<string> list_3;
        private static List<IndicatorHelper> list_4 = new List<IndicatorHelper>();
        private static List<string> list_5 = new List<string>();
        private List<string> list_6;
        private List<string> list_7;
        private List<StrategyParameter> list_8;
        private string string_0;

        public StrategyBuilder()
        {
            this.list_0 = new List<Rule>();
            this.list_1 = new List<string>();
            this.list_2 = new List<string>();
            this.list_3 = new List<string>();
            this.string_0 = "";
            this.list_6 = new List<string>();
            this.list_7 = new List<string>();
            this.dictionary_1 = new Dictionary<string, string>();
            this.list_8 = new List<StrategyParameter>();
            this.gridLines_0 = new GridLines();
            this.method_0();
        }

        public StrategyBuilder(IContainer container)
        {
            this.list_0 = new List<Rule>();
            this.list_1 = new List<string>();
            this.list_2 = new List<string>();
            this.list_3 = new List<string>();
            this.string_0 = "";
            this.list_6 = new List<string>();
            this.list_7 = new List<string>();
            this.dictionary_1 = new Dictionary<string, string>();
            this.list_8 = new List<StrategyParameter>();
            this.gridLines_0 = new GridLines();
            container.Add(this);
            this.method_0();
        }

        public string BuildCode(List<StrategyRule> rules, bool singlePosition)
        {
            string multiPosition;
            this.list_8.Clear();
            this.list_6.Clear();
            this.list_2.Clear();
            this.list_3.Clear();
            this.list_7.Clear();
            this.dictionary_1.Clear();
            this.int_1 = 1;
            this.int_3 = 0;
            this.int_4 = 0;
            foreach (StrategyRule rule6 in rules)
            {
                foreach (RuleParameter parameter5 in rule6.Parameters)
                {
                    parameter5.ReplaceValue = "";
                }
                foreach (Rule rule5 in rule6.Conditions)
                {
                    foreach (RuleParameter parameter4 in rule5.Parameters)
                    {
                        parameter4.ReplaceValue = "";
                    }
                }
            }
            this.list_1.Clear();
            this.int_0 = 1;
            foreach (StrategyRule rule30 in rules)
            {
                this.method_3(rule30);
                foreach (Rule rule29 in rule30.Conditions)
                {
                    this.method_3(rule29);
                }
            }
            if (singlePosition)
            {
                multiPosition = Resources.SinglePosition;
            }
            else
            {
                multiPosition = Resources.MultiPosition;
            }
            this.int_2 = 3;
            StringBuilder builder2 = new StringBuilder();
            foreach (StrategyRule rule19 in rules)
            {
                this.method_2(rule19, builder2);
                foreach (Rule rule20 in rule19.Conditions)
                {
                    this.method_2(rule20, builder2);
                }
            }
            string newValue = builder2.ToString();
            foreach (StrategyRule rule7 in rules)
            {
                rule7.ExitsAppliedTo.Clear();
                rule7.EntriesAppliedTo.Clear();
            }
            bool flag = false;
            bool flag2 = false;
            foreach (StrategyRule rule12 in rules)
            {
                if (rule12.RuleType == RuleType.LongEntry)
                {
                    flag = true;
                }
                else if (rule12.RuleType == RuleType.ShortEntry)
                {
                    flag2 = true;
                }
                if (!flag && (rule12.RuleType == RuleType.LongExit))
                {
                    foreach (StrategyRule rule14 in rules)
                    {
                        if (rule14.RuleType == RuleType.LongEntry)
                        {
                            rule14.ExitsAppliedTo.Add(rule12);
                            rule12.EntriesAppliedTo.Add(rule14);
                        }
                    }
                }
                if (!flag2 && (rule12.RuleType == RuleType.ShortExit))
                {
                    foreach (StrategyRule rule13 in rules)
                    {
                        if (rule13.RuleType == RuleType.ShortEntry)
                        {
                            rule13.ExitsAppliedTo.Add(rule12);
                            rule12.EntriesAppliedTo.Add(rule13);
                        }
                    }
                }
            }
            foreach (StrategyRule rule16 in rules)
            {
                StrategyRule rule15;
                StrategyRule rule17;
                if (rule16.RuleType != RuleType.LongExit)
                {
                    goto Label_0420;
                }
                for (int i = rules.IndexOf(rule16) - 1; i >= 0; i--)
                {
                    rule15 = rules[i];
                    if (rule15.RuleType == RuleType.LongEntry)
                    {
                        goto Label_0402;
                    }
                }
                continue;
            Label_0402:
                rule15.ExitsAppliedTo.Add(rule16);
                rule16.EntriesAppliedTo.Add(rule15);
                continue;
            Label_0420:
                if (rule16.RuleType == RuleType.ShortExit)
                {
                    for (int j = rules.IndexOf(rule16) - 1; j >= 0; j--)
                    {
                        rule17 = rules[j];
                        if (rule17.RuleType == RuleType.ShortEntry)
                        {
                            goto Label_045F;
                        }
                    }
                }
                continue;
            Label_045F:
                rule17.ExitsAppliedTo.Add(rule16);
                rule16.EntriesAppliedTo.Add(rule17);
            }
            foreach (StrategyRule rule9 in rules)
            {
                StrategyRule rule10;
                StrategyRule rule11;
                int num2 = 0;
                if ((rule9.RuleType != RuleType.LongEntry) || (rule9.ExitsAppliedTo.Count != 0))
                {
                    goto Label_0522;
                }
                num2 = rules.IndexOf(rule9) + 1;
                for (int k = num2; k < rules.Count; k++)
                {
                    rule10 = rules[k];
                    if (rule10.RuleType == RuleType.LongExit)
                    {
                        goto Label_0504;
                    }
                }
                continue;
            Label_0504:
                rule9.ExitsAppliedTo.Add(rule10);
                rule10.EntriesAppliedTo.Add(rule9);
                continue;
            Label_0522:
                if ((rule9.RuleType == RuleType.ShortEntry) && (rule9.ExitsAppliedTo.Count == 0))
                {
                    num2 = rules.IndexOf(rule9) + 1;
                    for (int m = num2; m < rules.Count; m++)
                    {
                        rule11 = rules[m];
                        if (rule11.RuleType == RuleType.ShortExit)
                        {
                            goto Label_0574;
                        }
                    }
                }
                continue;
            Label_0574:
                rule9.ExitsAppliedTo.Add(rule11);
                rule11.EntriesAppliedTo.Add(rule9);
            }
            int num8 = 1;
            foreach (StrategyRule rule21 in rules)
            {
                if (rule21.IsExit)
                {
                    rule21.ExitName = "Group" + num8;
                    num8++;
                }
            }
            List<StrategyRule> list = new List<StrategyRule>();
            foreach (StrategyRule rule8 in rules)
            {
                if (rule8.IsEntry)
                {
                    list.Add(rule8);
                }
            }
            list.Sort(this);
            this.int_2 = 5;
            string str2 = "";
            foreach (StrategyRule rule28 in list)
            {
                string str16 = this.method_5(rule28);
                string str15 = "";
                foreach (StrategyRule rule27 in rule28.ExitsAppliedTo)
                {
                    str15 = str15 + rule27.ExitName + "|";
                }
                str15 = "\"" + str15 + "\"";
                str16 = str16.Replace("<#SignalName>", str15);
                str2 = str2 + str16;
            }
            multiPosition = multiPosition.Replace("<#EntryBlock>", str2);
            list.Clear();
            foreach (StrategyRule rule26 in rules)
            {
                if (rule26.IsExit)
                {
                    list.Add(rule26);
                }
            }
            list.Sort(this);
            string str5 = "";
            foreach (StrategyRule rule18 in list)
            {
                string str4 = this.method_5(rule18).Replace("<#SignalName>", "\"" + rule18.ExitName + "\"");
                str5 = str5 + str4;
            }
            multiPosition = multiPosition.Replace("<#ExitBlock>", str5).Replace("<#Init>", newValue);
            foreach (StrategyRule rule3 in rules)
            {
                foreach (RuleParameter parameter in rule3.Parameters)
                {
                    this.method_4(parameter, true);
                }
                foreach (Rule rule4 in rule3.Conditions)
                {
                    foreach (RuleParameter parameter2 in rule4.Parameters)
                    {
                        this.method_4(parameter2, true);
                    }
                }
            }
            StringBuilder builder = new StringBuilder();
            foreach (string str12 in this.list_3)
            {
                string str13;
                string str14;
                if (dictionary_0.ContainsKey(str12))
                {
                    str13 = dictionary_0[str12];
                }
                else
                {
                    str13 = "Unknown";
                }
                if (this.dictionary_1.ContainsKey(str13))
                {
                    str14 = this.dictionary_1[str13];
                }
                else
                {
                    str14 = "pane" + (this.dictionary_1.Count + 1);
                    this.dictionary_1[str13] = str14;
                    builder.AppendLine("\t\t\tChartPane " + str14 + " = CreatePane(75,true,true);");
                }
                if (list_5.Contains(str12))
                {
                    builder.AppendLine("\t\t\tPlotFundamentalItems(" + str14 + ",Bars.Symbol,\"" + str12 + "\",Color.Red,LineStyle.Solid,1);");
                }
                else
                {
                    builder.AppendLine("\t\t\tPlotFundamentalItems(" + str14 + ",\"" + str12 + "\",Color.Blue,LineStyle.Solid,1);");
                }
            }
            foreach (string str7 in this.list_2)
            {
                string str9 = str7.Split(new char[] { '.' })[0];
                IndicatorHelper helper = null;
                using (List<IndicatorHelper>.Enumerator enumerator7 = this.IndicatorHelpers.GetEnumerator())
                {
                    IndicatorHelper current;
                    while (enumerator7.MoveNext())
                    {
                        current = enumerator7.Current;
                        if (current.IndicatorType.Name == str9)
                        {
                            goto Label_0AA6;
                        }
                    }
                    goto Label_0ABA;
                Label_0AA6:
                    helper = current;
                }
            Label_0ABA:
                if (helper != null)
                {
                    string str8;
                    if ((helper.IndicatorType.Namespace != "WealthLab.Indicators") && !this.list_6.Contains(helper.IndicatorType.Namespace))
                    {
                        this.list_6.Add(helper.IndicatorType.Namespace);
                    }
                    string key = "PricePane";
                    if (str7.Contains("Volume"))
                    {
                        key = "VolumePane";
                    }
                    if (helper.TargetPane != "")
                    {
                        key = helper.TargetPane;
                        if (this.dictionary_1.ContainsKey(key))
                        {
                            key = this.dictionary_1[key];
                        }
                        else
                        {
                            key = "pane" + (this.dictionary_1.Count + 1);
                            this.dictionary_1[helper.TargetPane] = key;
                            builder.AppendLine("\t\t\tChartPane " + key + " = CreatePane(75,true,true);");
                        }
                    }
                    Color defaultColor = helper.DefaultColor;
                    if (defaultColor.IsKnownColor)
                    {
                        str8 = defaultColor.ToString();
                        int index = str8.IndexOf('[');
                        str8 = str8.Substring(index + 1);
                        str8 = "Color." + str8.Substring(0, str8.Length - 1);
                    }
                    else
                    {
                        str8 = string.Concat(new object[] { "Color.FromArgb(", defaultColor.R, ",", defaultColor.G, ",", defaultColor.B, ")" });
                    }
                    builder.AppendLine(string.Concat(new object[] { "\t\t\tPlotSeries(", key, ",", str7, ",", str8, ",LineStyle.", helper.DefaultStyle, ",", helper.DefaultWidth, ");" }));
                }
            }
            this.int_2 = 3;
            foreach (StrategyRule rule in rules)
            {
                this.method_13(rule, builder);
                foreach (Rule rule2 in rule.Conditions)
                {
                    this.method_13(rule2, builder);
                }
            }
            string str10 = builder.ToString();
            multiPosition = multiPosition.Replace("<#Plot>", str10);
            StringBuilder builder4 = new StringBuilder();
            foreach (StrategyRule rule25 in rules)
            {
                this.method_14(rule25);
                foreach (Rule rule24 in rule25.Conditions)
                {
                    this.method_14(rule24);
                }
            }
            if (this.list_6.Count > 0)
            {
                foreach (string str3 in this.list_6)
                {
                    builder4.AppendLine("using " + str3 + ";");
                }
            }
            multiPosition = multiPosition.Replace("<#Using>", builder4.ToString());
            this.int_4++;
            multiPosition = multiPosition.Replace("<#StartBar>", "GetTradingLoopStartBar(" + this.int_4 + ")");
            foreach (StrategyRule rule23 in rules)
            {
                multiPosition = this.method_15(rule23, multiPosition);
                foreach (Rule rule22 in rule23.Conditions)
                {
                    multiPosition = this.method_15(rule22, multiPosition);
                }
            }
            if (this.list_8.Count > 0)
            {
                StringBuilder builder3 = new StringBuilder();
                foreach (StrategyParameter parameter3 in this.list_8)
                {
                    builder3.AppendLine("\t\tprivate StrategyParameter " + parameter3.Name + ";");
                }
                builder3.AppendLine("\t\tpublic MyStrategy()");
                builder3.AppendLine("\t\t{");
                foreach (StrategyParameter parameter6 in this.list_8)
                {
                    builder3.AppendLine("\t\t\t" + parameter6.Name + " = CreateParameter(\"" + parameter6.Description + "\"," + parameter6.Value.ToString(cultureInfo_0) + "," + parameter6.Start.ToString(cultureInfo_0) + "," + parameter6.Stop.ToString(cultureInfo_0) + "," + parameter6.Step.ToString(cultureInfo_0) + ");");
                }
                builder3.AppendLine("\t\t}");
                builder3.AppendLine("");
                return multiPosition.Replace("<#Constructor>", builder3.ToString());
            }
            return multiPosition.Replace("<#Constructor>", "");
        }

        public void Clear()
        {
            this.list_0.Clear();
        }

        public int Compare(Rule rule_0, Rule rule_1)
        {
            if (rule_0.Category == rule_1.Category)
            {
                return rule_0.Name.CompareTo(rule_1.Name);
            }
            return rule_0.Category.CompareTo(rule_1.Category);
        }

        public int Compare(StrategyRule strategyRule_0, StrategyRule strategyRule_1)
        {
            return strategyRule_0.EntryExitSortCode.CompareTo(strategyRule_1.EntryExitSortCode);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.icontainer_0 != null))
            {
                this.icontainer_0.Dispose();
            }
            base.Dispose(disposing);
        }

        public IndicatorHelper FindRuleIndicatorHelper(RuleParameter ruleParameter_0)
        {
            IndicatorHelper helper2;
            string str = ruleParameter_0.Value;
            int index = str.IndexOf(".Series");
            if (index == -1)
            {
                return null;
            }
            string str2 = str.Substring(0, index);
            using (List<IndicatorHelper>.Enumerator enumerator = this.IndicatorHelpers.GetEnumerator())
            {
                IndicatorHelper current;
                while (enumerator.MoveNext())
                {
                    current = enumerator.Current;
                    if (current.IndicatorType.Name == str2)
                    {
                        goto Label_0059;
                    }
                }
                return null;
            Label_0059:
                helper2 = current;
            }
            return helper2;
        }

        public void LoadAllRules(string path)
        {
            this.list_0.Clear();
            foreach (string str in Directory.GetFiles(path, "*.xml"))
            {
                this.LoadRules(str);
            }
            this.list_0.Sort(this);
            foreach (Rule rule in this.Rules)
            {
                foreach (RuleParameter parameter in rule.Parameters)
                {
                    this.method_1(parameter);
                }
            }
        }

        public void LoadRules(string fileName)
        {
            if (File.Exists(fileName))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<Rule>));
                TextReader textReader = new StreamReader(fileName);
                try
                {
                    List<Rule> list = (List<Rule>) serializer.Deserialize(textReader);
                    foreach (Rule rule in list)
                    {
                        foreach (RuleParameter parameter in rule.Parameters)
                        {
                            if (parameter.ParamType == RuleParamType.ListOfStrings)
                            {
                                int index = parameter.DefaultValue.IndexOf(';');
                                if (index == -1)
                                {
                                    parameter.Value = parameter.DefaultValue;
                                }
                                else
                                {
                                    parameter.Value = parameter.DefaultValue.Substring(0, index);
                                }
                            }
                            else
                            {
                                parameter.Value = parameter.DefaultValue;
                            }
                        }
                    }
                    foreach (Rule rule2 in list)
                    {
                        this.list_0.Add(rule2);
                    }
                }
                finally
                {
                    textReader.Close();
                }
            }
        }

        private void method_0()
        {
            this.icontainer_0 = new Container();
        }

        private void method_1(RuleParameter ruleParameter_0)
        {
            this.gridLines_0.WholeNumbersOnly = (ruleParameter_0.Start == ((int) ruleParameter_0.Start)) && (ruleParameter_0.Stop == ((int) ruleParameter_0.Stop));
            this.gridLines_0.LinesDesired = 15;
            this.gridLines_0.RangeMin = ruleParameter_0.Start;
            this.gridLines_0.RangeMax = ruleParameter_0.Stop;
            ruleParameter_0.Step = this.gridLines_0.GridIncrement;
        }

        private string method_10()
        {
            string str = "cond" + this.int_1;
            this.int_1++;
            return str;
        }

        private void method_11(StringBuilder stringBuilder_0)
        {
            for (int i = 0; i < this.int_2; i++)
            {
                stringBuilder_0.Append("\t");
            }
        }

        private void method_12(string string_1, StringBuilder stringBuilder_0)
        {
            string_1 = string_1.Replace('\r', ' ');
            foreach (string str in string_1.Split(new char[] { '\n' }))
            {
                if ((str != null) && (str.Trim() != ""))
                {
                    this.method_11(stringBuilder_0);
                    stringBuilder_0.AppendLine(str);
                }
            }
        }

        private void method_13(Rule rule_0, StringBuilder stringBuilder_0)
        {
            string plotting = rule_0.Plotting;
            if (plotting != "")
            {
                foreach (RuleParameter parameter in rule_0.Parameters)
                {
                    if ((parameter.ParamType != RuleParamType.String) && (parameter.ParamType != RuleParamType.Fundamental))
                    {
                        string replaceValue = parameter.ReplaceValue;
                    }
                    else
                    {
                        string text2 = "\"" + parameter.ReplaceValue + "\"";
                    }
                    plotting = plotting.Replace("<#" + parameter.Name + ">", parameter.ReplaceValue);
                }
                foreach (RuleVariable variable in rule_0.Variables)
                {
                    plotting = plotting.Replace("<#" + variable.Name + ">", variable.AliasName);
                }
                if (!this.list_7.Contains(plotting))
                {
                    this.list_7.Add(plotting);
                    if (plotting.Contains("<#NewPane"))
                    {
                        if (plotting.Contains("<#NewPane:"))
                        {
                            int index = plotting.IndexOf("<#NewPane:");
                            string oldValue = plotting.Substring(index);
                            int num2 = oldValue.IndexOf(">");
                            oldValue = oldValue.Substring(0, num2 + 1);
                            string str4 = oldValue.Substring(10, oldValue.Length - 11);
                            string key = "pane" + str4;
                            if (!this.dictionary_1.ContainsKey(key))
                            {
                                this.dictionary_1.Add(key, key);
                                stringBuilder_0.AppendLine("\t\t\tChartPane " + key + " = CreatePane(75,true,true);");
                            }
                            plotting = plotting.Replace(oldValue, key);
                        }
                        else
                        {
                            string str5 = "pane" + (this.dictionary_1.Count + 1);
                            this.dictionary_1.Add(str5, str5);
                            plotting = plotting.Replace("<#NewPane>", str5);
                            stringBuilder_0.AppendLine("\t\t\tChartPane " + str5 + " = CreatePane(75,true,true);");
                        }
                    }
                    this.method_12(plotting, stringBuilder_0);
                }
            }
        }

        private void method_14(Rule rule_0)
        {
            if ((rule_0.UsingClause != "") && (rule_0.UsingClause != null))
            {
                foreach (string str in rule_0.UsingClause.Split(new char[] { ';' }))
                {
                    if ((str.Trim() != "") && !this.list_6.Contains(str))
                    {
                        this.list_6.Add(str);
                    }
                }
            }
        }

        private string method_15(Rule rule_0, string string_1)
        {
            foreach (RuleParameter parameter in rule_0.Parameters)
            {
                string replaceValue;
                if ((parameter.ParamType != RuleParamType.String) && (parameter.ParamType != RuleParamType.Fundamental))
                {
                    replaceValue = parameter.ReplaceValue;
                }
                else
                {
                    replaceValue = "\"" + parameter.ReplaceValue + "\"";
                }
                string_1 = string_1.Replace("<#" + parameter.AliasName + ">", replaceValue);
                string_1 = string_1.Replace("<#" + parameter.Name + ">", replaceValue);
            }
            foreach (RuleVariable variable in rule_0.Variables)
            {
                string_1 = string_1.Replace("<#" + variable.AliasName + ">", variable.AliasName);
                string_1 = string_1.Replace("<#" + variable.Name + ">", variable.Name);
            }
            return string_1;
        }

        private string method_16(RuleParameter ruleParameter_0)
        {
            this.method_1(ruleParameter_0);
            string name = "slider" + (this.list_8.Count + 1);
            StrategyParameter item = new StrategyParameter(name, double.Parse(ruleParameter_0.Value), ruleParameter_0.Start, ruleParameter_0.Stop, ruleParameter_0.Step) {
                Description = ruleParameter_0.DisplayName
            };
            this.list_8.Add(item);
            return name;
        }

        private string method_17(RuleParameter ruleParameter_0, object object_0, string string_1)
        {
            double num2;
            double minimumValue;
            double maximumValue;
            string name = "slider" + (this.list_8.Count + 1);
            if (object_0 is RangeBoundDouble)
            {
                RangeBoundDouble num = object_0 as RangeBoundDouble;
                num2 = num.Value;
                minimumValue = num.MinimumValue;
                maximumValue = num.MaximumValue;
            }
            else
            {
                if (!(object_0 is RangeBoundInt32))
                {
                    throw new ArgumentException("Invalid argument type in CreateSlider");
                }
                RangeBoundInt32 num5 = object_0 as RangeBoundInt32;
                num2 = num5.Value;
                minimumValue = num5.MinimumValue;
                maximumValue = num5.MaximumValue;
            }
            ruleParameter_0.Start = minimumValue;
            ruleParameter_0.Stop = maximumValue;
            this.method_1(ruleParameter_0);
            StrategyParameter item = new StrategyParameter(name, num2, minimumValue, maximumValue, ruleParameter_0.Step) {
                Description = string_1
            };
            this.list_8.Add(item);
            return name;
        }

        private void method_2(Rule rule_0, StringBuilder stringBuilder_0)
        {
            if (rule_0.TempInit != "")
            {
                this.method_12(rule_0.TempInit, stringBuilder_0);
            }
        }

        private void method_3(Rule rule_0)
        {
            rule_0.TempBody = rule_0.Body;
            rule_0.TempInit = rule_0.Init;
            foreach (RuleVariable variable in rule_0.Variables)
            {
                variable.AliasName = variable.Name;
                while (this.list_1.Contains(variable.AliasName))
                {
                    variable.AliasName = variable.Name + "_" + this.int_0;
                    this.int_0++;
                }
                this.list_1.Add(variable.AliasName);
                rule_0.TempBody = rule_0.TempBody.Replace("<#" + variable.Name + ">", variable.AliasName);
                rule_0.TempInit = rule_0.TempInit.Replace("<#" + variable.Name + ">", variable.AliasName);
            }
            foreach (RuleParameter parameter in rule_0.Parameters)
            {
                string str;
                if (parameter.ExposeAsSlider)
                {
                    if (parameter.ParamType == RuleParamType.Indicator)
                    {
                        IndicatorHelper helper = this.FindRuleIndicatorHelper(parameter);
                        string str3 = parameter.Value;
                        int index = str3.IndexOf("(");
                        string str7 = str3.Substring(index + 1);
                        index = str7.IndexOf(")");
                        string[] strArray = str7.Substring(0, index).Split(new char[] { ',' });
                        str = parameter.Value;
                        if (strArray.Length == helper.ParameterDefaultValues.Count)
                        {
                            index = str3.IndexOf("(");
                            string str2 = str3.Substring(0, index + 1);
                            int num2 = 0;
                            foreach (object obj2 in helper.ParameterDefaultValues)
                            {
                                if (num2 > 0)
                                {
                                    str2 = str2 + ",";
                                }
                                if ((obj2 is RangeBoundDouble) || (obj2 is RangeBoundInt32))
                                {
                                    string str4 = helper.ParameterDescriptions[num2];
                                    if ((parameter.IndicatorParameterDisplayNames != null) && (parameter.IndicatorParameterDisplayNames.Count >= helper.ParameterDescriptions.Count))
                                    {
                                        str4 = parameter.IndicatorParameterDisplayNames[num2];
                                    }
                                    string str5 = this.method_17(parameter, obj2, str4);
                                    if (obj2 is RangeBoundInt32)
                                    {
                                        str2 = str2 + str5 + ".ValueInt";
                                    }
                                    else
                                    {
                                        str2 = str2 + str5 + ".Value";
                                    }
                                }
                                else
                                {
                                    str2 = str2 + strArray[num2];
                                }
                                num2++;
                            }
                            str = str2 + ")";
                        }
                    }
                    else
                    {
                        string str6 = this.method_16(parameter);
                        if (parameter.ParamType == RuleParamType.Integer)
                        {
                            str = str6 + ".ValueInt";
                        }
                        else
                        {
                            str = str6 + ".Value";
                        }
                    }
                    parameter.ReplaceValue = str;
                }
                else if ((parameter.ParamType != RuleParamType.String) && (parameter.ParamType != RuleParamType.Fundamental))
                {
                    str = parameter.Value;
                }
                else
                {
                    str = "\"" + parameter.Value + "\"";
                }
                rule_0.TempBody = rule_0.TempBody.Replace("<#" + parameter.Name + ">", str);
                rule_0.TempInit = rule_0.TempInit.Replace("<#" + parameter.Name + ">", str);
            }
        }

        private void method_4(RuleParameter ruleParameter_0, bool bool_0)
        {
            if (((ruleParameter_0.ParamType == RuleParamType.Integer) && ruleParameter_0.Name.ToUpper().Contains("PERIOD")) && !ruleParameter_0.ExposeAsSlider)
            {
                int num = int.Parse(ruleParameter_0.Value);
                if (num > this.int_4)
                {
                    this.int_4 = num;
                }
            }
            if (ruleParameter_0.ParamType == RuleParamType.Indicator)
            {
                string[] strArray2 = ruleParameter_0.Value.Split(new char[] { '(' });
                if (strArray2.Length > 0)
                {
                    string str2 = strArray2[0].Split(new char[] { '.' })[0].ToUpper();
                    using (List<IndicatorHelper>.Enumerator enumerator = this.IndicatorHelpers.GetEnumerator())
                    {
                        IndicatorHelper current;
                        while (enumerator.MoveNext())
                        {
                            current = enumerator.Current;
                            if (current.IndicatorType.Name.ToUpper() == str2)
                            {
                                goto Label_00E0;
                            }
                        }
                        goto Label_01A3;
                    Label_00E0:
                        strArray2[1] = strArray2[1].Replace(")", "");
                        string[] strArray = strArray2[1].Split(new char[] { ',' });
                        if ((strArray.Length == current.ParameterDescriptions.Count) && (strArray.Length > 0))
                        {
                            int index = 0;
                            foreach (string str in current.ParameterDescriptions)
                            {
                                if (str.ToUpper().Contains("PERIOD"))
                                {
                                    try
                                    {
                                        int num3 = int.Parse(strArray[index]);
                                        if (num3 > this.int_4)
                                        {
                                            this.int_4 = num3;
                                        }
                                    }
                                    catch
                                    {
                                    }
                                }
                                index++;
                            }
                        }
                    }
                }
            }
        Label_01A3:
            if (bool_0)
            {
                if ((ruleParameter_0.ParamType == RuleParamType.Indicator) && !this.list_2.Contains(ruleParameter_0.ReplaceValue))
                {
                    this.list_2.Add(ruleParameter_0.ReplaceValue);
                }
                if ((ruleParameter_0.ParamType == RuleParamType.Fundamental) && !this.list_3.Contains(ruleParameter_0.Value))
                {
                    this.list_3.Add(ruleParameter_0.Value);
                }
            }
        }

        private string method_5(StrategyRule strategyRule_0)
        {
            StringBuilder builder = new StringBuilder();
            if (strategyRule_0.IsExit)
            {
                this.method_12("if (p.EntrySignal.Contains(\"" + strategyRule_0.ExitName + "|\"))", builder);
                this.method_12("{", builder);
                this.int_2++;
            }
            bool flag = false;
            using (List<Rule>.Enumerator enumerator = strategyRule_0.Conditions.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    Rule current = enumerator.Current;
                    strategyRule_0.ValidOr = false;
                }
            }
            for (int i = 0; i < strategyRule_0.Conditions.Count; i++)
            {
                Rule rule = strategyRule_0.Conditions[i];
                if (rule.RuleType == RuleType.OrDivider)
                {
                    flag = true;
                    if (((i != 0) && (i != (strategyRule_0.Conditions.Count - 1))) && ((strategyRule_0.Conditions[i + 1].RuleType == RuleType.Condition) || (strategyRule_0.Conditions[i + 1].RuleType == RuleType.MultiCondition)))
                    {
                        rule.ValidOr = true;
                        flag = true;
                    }
                }
            }
            if (flag)
            {
                List<Rule> list = new List<Rule>();
                List<string> list2 = new List<string>();
                for (int j = 0; j < strategyRule_0.Conditions.Count; j++)
                {
                    if ((strategyRule_0.Conditions[j].RuleType == RuleType.Condition) || (strategyRule_0.Conditions[j].RuleType == RuleType.MultiCondition))
                    {
                        list.Add(strategyRule_0.Conditions[j]);
                    }
                    if (strategyRule_0.Conditions[j].ValidOr || (j == (strategyRule_0.Conditions.Count - 1)))
                    {
                        string item = this.method_10();
                        list2.Add(item);
                        this.method_12("bool " + item + " = false;", builder);
                        this.method_6(builder, list, item + " = true;");
                        list.Clear();
                    }
                }
                string str = "if (";
                for (int k = 0; k < list2.Count; k++)
                {
                    str = str + list2[k];
                    if (k < (list2.Count - 1))
                    {
                        str = str + " || ";
                    }
                }
                str = str + ")";
                this.method_12(str, builder);
                this.method_12("{", builder);
                this.int_2++;
                this.method_12(strategyRule_0.TempBody, builder);
                this.int_2--;
                this.method_12("}", builder);
            }
            else
            {
                this.method_6(builder, strategyRule_0.Conditions, strategyRule_0.TempBody);
            }
            if (strategyRule_0.IsExit)
            {
                this.int_2--;
                this.method_12("}", builder);
            }
            return builder.ToString();
        }

        private void method_6(StringBuilder stringBuilder_0, List<Rule> list_9, string string_1)
        {
            int count = list_9.Count;
            bool flag = false;
            int num2 = 0;
            string replaceValue = "";
            string str2 = "";
            int num3 = 0;
            string str3 = "cntMultiCondition";
            string str4 = "saveBar";
            foreach (Rule rule in list_9)
            {
                if (rule.RuleType == RuleType.Condition)
                {
                    if (flag)
                    {
                        this.method_8("for(int i = " + str2 + "; i > 0; --i)", stringBuilder_0);
                        this.method_8(rule.TempBody, stringBuilder_0);
                        this.method_12(str3 + "++;", stringBuilder_0);
                        this.method_12("break;", stringBuilder_0);
                        this.method_9(stringBuilder_0);
                        this.method_12("bar--;", stringBuilder_0);
                        this.method_9(stringBuilder_0);
                        this.method_12("bar = " + str4 + ";", stringBuilder_0);
                        num3++;
                    }
                    else
                    {
                        this.method_8(rule.TempBody, stringBuilder_0);
                    }
                }
                else if (rule.RuleType == RuleType.MultiCondition)
                {
                    count--;
                    if (flag)
                    {
                        if (num3 == 0)
                        {
                            flag = false;
                            num2--;
                        }
                        else
                        {
                            this.method_7(stringBuilder_0, replaceValue, str3);
                            count -= num3;
                        }
                    }
                    flag = true;
                    num2++;
                    this.int_3++;
                    num3 = 0;
                    str3 = "cntMultiCondition" + this.int_3.ToString();
                    str4 = "saveBar" + this.int_3.ToString();
                    this.method_12("int " + str3 + " = 0;", stringBuilder_0);
                    this.method_12("int " + str4 + " = bar;", stringBuilder_0);
                    replaceValue = rule.Parameters[0].ReplaceValue;
                    str2 = rule.Parameters[1].ReplaceValue;
                }
            }
            if (flag)
            {
                if (num3 == 0)
                {
                    flag = false;
                    num2--;
                }
                else
                {
                    this.method_7(stringBuilder_0, replaceValue, str3);
                    count -= num3;
                }
            }
            this.method_12(string_1, stringBuilder_0);
            if (flag)
            {
                while (num2-- > 0)
                {
                    this.method_9(stringBuilder_0);
                }
            }
            while (count > 0)
            {
                this.method_9(stringBuilder_0);
                count--;
            }
        }

        private void method_7(StringBuilder stringBuilder_0, string string_1, string string_2)
        {
            this.method_8("if (" + string_2 + " >= " + string_1 + ")", stringBuilder_0);
        }

        private void method_8(string string_1, StringBuilder stringBuilder_0)
        {
            this.method_12(string_1, stringBuilder_0);
            this.method_12("{", stringBuilder_0);
            this.int_2++;
        }

        private void method_9(StringBuilder stringBuilder_0)
        {
            this.int_2--;
            this.method_12("}", stringBuilder_0);
        }

        public void SaveRules(string fileName)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Rule>));
            TextWriter textWriter = new StreamWriter(fileName);
            try
            {
                serializer.Serialize(textWriter, this.list_0);
            }
            finally
            {
                textWriter.Close();
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<IndicatorHelper> IndicatorHelpers
        {
            get
            {
                if (((list_4.Count == 0) && (this.RootPath != null)) && (this.RootPath != ""))
                {
                    AssemblyLoader loader2 = new AssemblyLoader {
                        BaseClass = "IndicatorHelper",
                        Path = this.string_0
                    };
                    foreach (Type type2 in loader2.Types)
                    {
                        IndicatorHelper item = (IndicatorHelper) loader2.CreateInstance(type2);
                        list_4.Add(item);
                    }
                    AssemblyLoader loader = new AssemblyLoader {
                        BaseClass = "FundamentalDataProvider",
                        Path = this.string_0
                    };
                    foreach (Type type in loader.Types)
                    {
                        FundamentalDataProvider provider = (FundamentalDataProvider) loader.CreateInstance(type);
                        IList<string> symbolSpecificItemsProvided = provider.SymbolSpecificItemsProvided;
                        IList<string> nonSymbolSpecificItemsProvided = provider.NonSymbolSpecificItemsProvided;
                        List<string> list = new List<string>();
                        if (symbolSpecificItemsProvided != null)
                        {
                            foreach (string str in symbolSpecificItemsProvided)
                            {
                                if (!list.Contains(str))
                                {
                                    list.Add(str);
                                    list_5.Add(str);
                                }
                            }
                        }
                        if (nonSymbolSpecificItemsProvided != null)
                        {
                            foreach (string str2 in nonSymbolSpecificItemsProvided)
                            {
                                if (!list.Contains(str2))
                                {
                                    list.Add(str2);
                                }
                            }
                        }
                        foreach (string str3 in list)
                        {
                            string str4 = provider.ItemPane(str3);
                            dictionary_0.Add(str3, str4);
                        }
                    }
                }
                return list_4;
            }
        }

        public string RootPath
        {
            get
            {
                return this.string_0;
            }
            set
            {
                this.string_0 = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public List<Rule> Rules
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
    }
}

