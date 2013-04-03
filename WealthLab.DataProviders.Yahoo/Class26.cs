using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using WealthLab;
using WealthLab.DataProviders.Yahoo;

internal class Class26
{
    private bool bool_0;
    private Delegate1 delegate1_0;
    private Delegate2 delegate2_0;
    private Delegate3 delegate3_0;
    private Delegate4 delegate4_0;
    private Dictionary<string, Class29> dictionary_0 = new Dictionary<string, Class29>();
    private IFormatProvider iformatProvider_0;
    private List<string> list_0 = new List<string>();
    private List<Class30> list_1 = new List<Class30>();
    private Queue<Class27> queue_0 = new Queue<Class27>();
    private Thread thread_0;

    public Class26()
    {
        CultureInfo info = new CultureInfo("en-US") {
            NumberFormat = { NumberDecimalSeparator = "." }
        };
        this.iformatProvider_0 = info;
    }

    public void method_0(Delegate1 delegate1_1)
    {
        Delegate1 delegate3;
        Delegate1 delegate2 = this.delegate1_0;
        do
        {
            delegate3 = delegate2;
            Delegate1 delegate4 = (Delegate1) Delegate.Combine(delegate3, delegate1_1);
            delegate2 = Interlocked.CompareExchange<Delegate1>(ref this.delegate1_0, delegate4, delegate3);
        }
        while (delegate2 != delegate3);
    }

    public void method_1(Delegate1 delegate1_1)
    {
        Delegate1 delegate3;
        Delegate1 delegate2 = this.delegate1_0;
        do
        {
            delegate3 = delegate2;
            Delegate1 delegate4 = (Delegate1) Delegate.Remove(delegate3, delegate1_1);
            delegate2 = Interlocked.CompareExchange<Delegate1>(ref this.delegate1_0, delegate4, delegate3);
        }
        while (delegate2 != delegate3);
    }

    public void method_10(bool bool_1)
    {
        this.bool_0 = bool_1;
    }

    public void method_11(string string_0)
    {
        lock (this.list_0)
        {
            this.list_0.Remove(string_0);
        }
    }

    public void method_12(string string_0)
    {
        lock (this.list_0)
        {
            this.list_0.Add(string_0);
        }
    }

    public void method_13()
    {
        Class21.smethod_8(new object[0]);
        if (((Class18.smethod_0() == null) && (YahooStaticProvider.ClientSettings.Login != string.Empty)) && (YahooStaticProvider.ClientSettings.Password != string.Empty))
        {
            Class18.smethod_6(YahooStaticProvider.ClientSettings.Login, YahooStaticProvider.ClientSettings.Password);
            if (Class18.smethod_1() != null)
            {
                Class21.smethod_4(Enum2.const_3, Class18.smethod_1());
                MessageBox.Show("Yahoo! login failed. Error: " + Class18.smethod_1(), "Yahoo! login error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
    }

    public void method_14()
    {
        Class21.smethod_8(new object[0]);
        if (!this.bool_0)
        {
            this.bool_0 = true;
            lock (this.list_1)
            {
                foreach (Class30 class2 in this.list_1)
                {
                    class2.method_1().WaitOne();
                    class2.method_0().Abort();
                }
            }
        }
    }

    /* ///WYJ fix, code from Reflector 
    private string method_15(string string_0, bool bool_1)
    {
        string str = null;
        int num = 0;
    Label_0004:
        num++;
        Class21.smethod_8(new object[] { string_0, bool_1, "Attempt " + num });
        HttpWebRequest request = (HttpWebRequest) WebRequest.Create(string_0);
        if (bool_1)
        {
            goto Label_00FA;
        }
    Label_0047:
        request.Timeout = 0x2710;
        try
        {
            HttpWebResponse response = (HttpWebResponse) request.GetResponse();
            using (Stream stream = response.GetResponseStream())
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    str = reader.ReadToEnd();
                    Class21.smethod_2("Result  " + string_0 + "\r\n" + str);
                }
            }
        }
        catch (Exception exception)
        {
            Class21.smethod_4(Enum2.const_3, exception.Message);
            if (!(exception is WebException) || (num >= YahooStaticProvider.ClientSettings.AttemptCount))
            {
                throw exception;
            }
            Class21.smethod_4(Enum2.const_4, "New attempt " + string_0);
        }
        while (str == null)
        {
            if (num < YahooStaticProvider.ClientSettings.AttemptCount)
            {
                goto Label_0004;
            }
            return str;
        Label_00FA:
            if (Class18.smethod_0() == null)
            {
                this.method_13();
            }
            if (Class18.smethod_0() != null)
            {
                request.Headers.Add(HttpRequestHeader.Cookie, Class18.smethod_0());
            }
            goto Label_0047;
        }
        return str;
    } */

    private string method_15(string string_0, bool bool_1)
    {
        string end = null;
        int num = 0;
        while (true)
        {
            num++;
            object[] string0 = new object[] { string_0, bool_1, string.Concat("Attempt ", num) };
            Class21.smethod_8(string0);
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(string_0);
            if (bool_1)
            {
                if (Class18.smethod_0() == null)
                {
                    this.method_13();
                }
                if (Class18.smethod_0() != null)
                {
                    httpWebRequest.Headers.Add(HttpRequestHeader.Cookie, Class18.smethod_0());
                }
            }
            httpWebRequest.Timeout = 10000;
            try
            {
                HttpWebResponse response = (HttpWebResponse)httpWebRequest.GetResponse();
                Stream responseStream = response.GetResponseStream();
                using (responseStream)
                {
                    using (StreamReader streamReader = new StreamReader(responseStream))
                    {
                        end = streamReader.ReadToEnd();
                        Class21.smethod_2(string.Concat("Result  ", string_0, "\r\n", end));
                    }
                }
            }
            catch (Exception exception1)
            {
                Exception exception = exception1;
                Class21.smethod_4(Enum2.const_3, exception.Message);
                if (exception as WebException == null || num >= YahooStaticProvider.ClientSettings.AttemptCount)
                {
                    throw exception;
                }
                else
                {
                    Class21.smethod_4(Enum2.const_4, string.Concat("New attempt ", string_0));
                }
            }
            if (end != null)
            {
                break;
            }
            if (num >= YahooStaticProvider.ClientSettings.AttemptCount)
            {
                break;
            }
        }
        return end;
    }

    public Bars method_16(Class27 class27_0, bool bool_1)
    {
        Class21.smethod_8(new object[] { class27_0.method_0() });
        string str = this.method_31(class27_0.method_0(), class27_0.method_3(), class27_0.method_7(), Enum4.flag_0);
        string str2 = this.method_15(str, false);
        Bars bars = this.method_28(class27_0.method_0(), str2);
        if (bool_1 || YahooStaticProvider.ClientSettings.AlwaysPartialBar)
        {
            double num;
            double num2;
            double num3;
            double num4;
            if (Class18.smethod_0() == null)
            {
                this.method_13();
            }
            str = this.method_30(class27_0.method_0());
            str2 = this.method_15(str, true);
            Quote quote = this.method_20(str2, out num, out num2, out num3, out num4);
            if ((quote == null) || ((quote.TimeStamp.Date <= bars.Date[bars.Count - 1]) && (bars.Count != 0)))
            {
                return bars;
            }
            bars.Add(quote.TimeStamp.Date, num, num2, num3, quote.Price, num4);
        }
        return bars;
    }

    private bool method_17(string string_0)
    {
        return ((!(string_0 == "N/A") && !(string_0 == string.Empty)) && !(string_0.Trim(new char[] { '"' }) == string.Empty));
    }

    private void method_18(ref Quote quote_0, double double_0)
    {
        if (this.dictionary_0.ContainsKey(quote_0.Symbol))
        {
            Class29 class2 = this.dictionary_0[quote_0.Symbol];
            if (class2.dateTime_0.Date == quote_0.TimeStamp.Date)
            {
                quote_0.Size = double_0 - class2.double_0;
            }
            else
            {
                quote_0.Size = double_0;
            }
            class2.dateTime_0 = quote_0.TimeStamp;
            class2.double_0 = double_0;
        }
        else
        {
            quote_0.Size = 0.0;
            this.dictionary_0.Add(quote_0.Symbol, new Class29(quote_0.TimeStamp, double_0));
        }
    }

    public Quote method_19(string string_0)
    {
        string str = this.method_30(string_0);
        try
        {
            double num;
            double num2;
            double num3;
            double num4;
            string str2 = this.method_15(str, true);
            return this.method_20(str2, out num, out num2, out num3, out num4);
        }
        catch
        {
            return null;
        }
    }

    public void method_2(Delegate2 delegate2_1)
    {
        Delegate2 delegate3;
        Delegate2 delegate2 = this.delegate2_0;
        do
        {
            delegate3 = delegate2;
            Delegate2 delegate4 = (Delegate2) Delegate.Combine(delegate3, delegate2_1);
            delegate2 = Interlocked.CompareExchange<Delegate2>(ref this.delegate2_0, delegate4, delegate3);
        }
        while (delegate2 != delegate3);
    }

    private Quote method_20(string string_0, out double double_0, out double double_1, out double double_2, out double double_3)
    {
        double num;
        double num2;
        string[] strArray = string_0.Split(new char[] { ',' });
        Quote quote = new Quote();
        double_3 = num = 0.0;
        double_2 = num2 = num;
        double_0 = double_1 = num2;
        try
        {
            quote.Symbol = strArray[0].Trim(new char[] { '"' });
            quote.TimeStamp = DateTime.ParseExact(strArray[1].Trim(new char[] { '"' }) + " " + strArray[2].Trim(new char[] { '"' }), "M/d/yyyy h:mmtt", this.iformatProvider_0);
            quote.Open = double_0 = Convert.ToDouble(strArray[3], this.iformatProvider_0);
            double_1 = Convert.ToDouble(strArray[4], this.iformatProvider_0);
            double_2 = Convert.ToDouble(strArray[5], this.iformatProvider_0);
            quote.Price = Convert.ToDouble(strArray[6], this.iformatProvider_0);
            double_3 = Convert.ToDouble(strArray[7], this.iformatProvider_0);
            quote.PreviousClose = Convert.ToDouble(strArray[8], this.iformatProvider_0);
            try
            {
                quote.Bid = Convert.ToDouble(strArray[9], this.iformatProvider_0);
                quote.Ask = Convert.ToDouble(strArray[10], this.iformatProvider_0);
            }
            catch
            {
            }
            this.method_18(ref quote, double_3);
        }
        catch (Exception exception)
        {
            string str = exception.Message + " Line: " + string_0;
            Class21.smethod_4(Enum2.const_3, str);
            quote = null;
        }
        return quote;
    }

    private void method_21(string string_0)
    {
        foreach (string str in string_0.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries))
        {
            double num2;
            double num3;
            double num4;
            double num5;
            Quote quote = this.method_20(str, out num2, out num3, out num4, out num5);
            if ((this.delegate3_0 != null) && (quote != null))
            {
                this.delegate3_0(this, new EventArgs4(quote, num2, num3, num4));
            }
        }
    }

    private void method_22()
    {
        List<string> list = new List<string>();
    Label_0006:
        if (this.bool_0)
        {
            return;
        }
        if (this.list_0.Count > 0)
        {
            try
            {
                lock (this.list_0)
                {
                    StringBuilder builder = new StringBuilder();
                    list.Clear();
                    for (int i = 0; i < this.list_0.Count; i++)
                    {
                        builder.Append(this.list_0[i]);
                        builder.Append("+");
                        if ((((i + 1) % 50) == 0) || (i == (this.list_0.Count - 1)))
                        {
                            list.Add(builder.ToString());
                            builder.Remove(0, builder.Length);
                        }
                    }
                }
                foreach (string str in list)
                {
                    if (this.bool_0)
                    {
                        goto Label_013C;
                    }
                    string str2 = this.method_30(str);
                    string str3 = this.method_15(str2, true);
                    this.method_21(str3);
                }
            }
            catch (Exception exception)
            {
                if (this.delegate4_0 != null)
                {
                    this.delegate4_0(this, new EventArgs5(string.Empty, exception.Message));
                }
            }
        }
    Label_013C:
        Thread.Sleep(0x7d0);
        goto Label_0006;
    }

    public void method_23()
    {
        this.bool_0 = false;
        this.thread_0 = new Thread(new ThreadStart(this.method_22));
        this.thread_0.IsBackground = true;
        this.thread_0.Start();
    }

    private Class28 method_24(string string_0)
    {
        Class28 class2 = new Class28();
        foreach (string str in string_0.Split(new char[] { '\n' }))
        {
            if (str.StartsWith("DIVIDEND,"))
            {
                FundamentalItem item2 = new FundamentalItemYahooDividend("Dividend (Yahoo! Finance)");
                string[] strArray5 = str.Split(new char[] { ',' });
                item2.Date = DateTime.ParseExact(strArray5[1].Trim(), "yyyyMMdd", this.iformatProvider_0);
                item2.Value = Convert.ToDouble(strArray5[2].Trim(), this.iformatProvider_0);
                class2.method_0().Add(item2);
            }
            if (str.StartsWith("SPLIT,"))
            {
                FundamentalItem item = new FundamentalItemYahooSplit("Split (Yahoo! Finance)");
                string[] strArray3 = str.Split(new char[] { ',' });
                item.Date = DateTime.ParseExact(strArray3[1].Trim(), "yyyyMMdd", this.iformatProvider_0);
                string[] strArray4 = strArray3[2].Split(new char[] { ':' });
                double num2 = Convert.ToDouble(strArray4[0], this.iformatProvider_0);
                double num3 = Convert.ToDouble(strArray4[1], this.iformatProvider_0);
                item.Value = num2 / num3;
                class2.method_2().Add(item);
            }
        }
        class2.method_0().Reverse();
        class2.method_2().Reverse();
        return class2;
    }

    private Class28 method_25(Class27 class27_0)
    {
        string str = this.method_31(class27_0.method_0(), class27_0.method_5(), class27_0.method_7(), Enum4.flag_1);
        string str2 = this.method_15(str, true);
        return this.method_24(str2);
    }

    private void method_26()
    {
        Class30 class2 = null;
        try
        {
            lock (this.list_1)
            {
                class2 = this.list_1[Convert.ToInt32(Thread.CurrentThread.Name)];
            }
        Label_003C:
            if (this.bool_0)
            {
                return;
            }
            Class27 class3 = null;
            lock (this.queue_0)
            {
                if (this.queue_0.Count > 0)
                {
                    class3 = this.queue_0.Dequeue();
                }
                else
                {
                    return;
                }
            }
            if (class3 == null)
            {
                goto Label_003C;
            }
            Exception exception = null;
            Bars bars = null;
            Class28 class4 = null;
            try
            {
                bars = this.method_16(class3, (class3.method_1() & Enum4.flag_2) != 0);
                if ((class3.method_1() & Enum4.flag_1) != 0)
                {
                    class4 = this.method_25(class3);
                }
            }
            catch (Exception exception2)
            {
                Class21.smethod_4(Enum2.const_4, class3.method_0() + " " + exception2.Message);
                exception = exception2;
            }
            goto Label_0135;
        Label_00E4:
            if (exception == null)
            {
                if (this.delegate1_0 != null)
                {
                    this.delegate1_0(this, new EventArgs2(class3, bars, class4));
                }
            }
            else if (this.delegate2_0 != null)
            {
                this.delegate2_0(this, new EventArgs3(class3, exception));
            }
        Label_0124:
            class2.method_1().Set();
            goto Label_003C;
        Label_0135:
            class2.method_1().Reset();
            if (this.bool_0)
            {
                goto Label_0124;
            }
            goto Label_00E4;
        }
        catch (ThreadAbortException)
        {
            Class21.smethod_2("Thread Abort");
        }
        catch (Exception exception3)
        {
            Class21.smethod_4(Enum2.const_3, "Thread execution error. " + exception3.Message);
            if (this.delegate2_0 != null)
            {
                this.delegate2_0(this, new EventArgs3(null, exception3));
            }
        }
        finally
        {
            if (class2 != null)
            {
                class2.method_2().Set();
            }
        }
    }

    public void method_27(List<Class27> list_2)
    {
        Class21.smethod_8(new object[0]);
        this.list_1.Clear();
        List<ManualResetEvent> list = new List<ManualResetEvent>();
        foreach (Class27 class3 in list_2)
        {
            this.queue_0.Enqueue(class3);
        }
        int num2 = (this.queue_0.Count < YahooStaticProvider.ClientSettings.ThreadCount) ? this.queue_0.Count : YahooStaticProvider.ClientSettings.ThreadCount;
        for (int i = 0; i < num2; i++)
        {
            Thread thread = new Thread(new ThreadStart(this.method_26)) {
                Name = i.ToString(),
                IsBackground = true
            };
            Class30 item = new Class30(thread);
            this.list_1.Add(item);
            list.Add(item.method_2());
        }
        foreach (Class30 class2 in this.list_1)
        {
            class2.method_0().Start();
        }
        WaitHandle.WaitAll(list.ToArray());
    }

    private Bars method_28(string string_0, string string_1)
    {
        Class21.smethod_8(new object[] { string_0 });
        Bars bars = new Bars(Class23.smethod_1(string_0), BarScale.Daily, 0);
        string str = string.Empty;
        int index = -1;
        try
        {
            string[] strArray2 = string_1.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
            index = strArray2.Length - 1;
            while (index >= 1)
            {
                str = strArray2[index];
                string[] strArray3 = str.Split(new char[] { ',' });
                if (strArray3.Length == 7)
                {
                    DateTime time = DateTime.ParseExact(strArray3[0], new string[] { "yyyyMMdd", "yyyy-MM-dd" }, this.iformatProvider_0, DateTimeStyles.None);
                    double open = Convert.ToDouble(strArray3[1], this.iformatProvider_0);
                    double high = Convert.ToDouble(strArray3[2], this.iformatProvider_0);
                    double num4 = Convert.ToDouble(strArray3[3], this.iformatProvider_0);
                    double close = Convert.ToDouble(strArray3[4], this.iformatProvider_0);
                    double volume = Convert.ToDouble(strArray3[5], this.iformatProvider_0);
                    bars.Add(time, open, high, num4, close, volume);
                }
                index--;
            }
        }
        catch (Exception exception)
        {
            string str2 = string.Format("Data parsing error. Symbol: {0}, LineNumber: {1}, String:\r\n {2}\r\nMessage:\r\n{3}\r\nStack Trace:\r\n{4}", new object[] { string_0, index, str, exception.Message, exception.StackTrace });
            Class21.smethod_4(Enum2.const_3, str2);
            throw new Exception0(str2);
        }
        return bars;
    }

    public Bars method_29(string string_0, DateTime dateTime_0, DateTime dateTime_1)
    {
        Class27 class2 = new Class27(string_0, dateTime_0, dateTime_1, dateTime_1, Enum4.flag_0);
        Bars bars = null;
        try
        {
            bars = this.method_16(class2, true);
        }
        catch
        {
        }
        if (bars != null)
        {
            for (int i = bars.Count - 1; i >= 0; i--)
            {
                if ((bars.Date[i] < dateTime_0) || (bars.Date[i] > dateTime_1))
                {
                    bars.Delete(i);
                }
            }
            return bars;
        }
        return new Bars(string_0, BarScale.Daily, 0);
    }

    public void method_3(Delegate2 delegate2_1)
    {
        Delegate2 delegate3;
        Delegate2 delegate2 = this.delegate2_0;
        do
        {
            delegate3 = delegate2;
            Delegate2 delegate4 = (Delegate2) Delegate.Remove(delegate3, delegate2_1);
            delegate2 = Interlocked.CompareExchange<Delegate2>(ref this.delegate2_0, delegate4, delegate3);
        }
        while (delegate2 != delegate3);
    }

    private string method_30(string string_0)
    {
        return string.Format("http://download.finance.yahoo.com/d/quotes.csv?s={0}&f=sd1t1ohgl1vpba&e=.csv", string_0);
    }

    private string method_31(string string_0, DateTime dateTime_0, DateTime dateTime_1, Enum4 enum4_0)
    {
        int num = dateTime_0.Month - 1;
        int day = dateTime_0.Day;
        int year = dateTime_0.Year;
        int num4 = dateTime_1.Month - 1;
        int num5 = dateTime_1.Day;
        int num6 = dateTime_1.Year;
        string str = "d";
        switch (enum4_0)
        {
            case Enum4.flag_0:
                return string.Format("http://ichart.yahoo.com/table.csv?s={0}&a={1}&b={2}&c={3}&d={4}&e={5}&f={6}&g={7}&ignore=.csv", new object[] { string_0, num, day, year, num4, num5, num6, str });

            case Enum4.flag_1:
                return string.Format("http://ichart.yahoo.com/x?s={0}&a={1}&b={2}&c={3}&d={4}&e={5}&f={6}&g=v&y=0&z=40000", new object[] { string_0, num, day, year, num4, num5, num6 });
        }
        return string.Empty;
    }

    public Dictionary<string, string> method_32(List<Class27> list_2)
    {
        List<string> list = new List<string>();
        foreach (Class27 class2 in list_2)
        {
            list.Add(class2.method_0());
        }
        return this.method_33(list);
    }

    public Dictionary<string, string> method_33(List<string> list_2)
    {
        Dictionary<string, string> dictionary = new Dictionary<string, string>();
        StringBuilder builder = new StringBuilder();
        for (int i = 0; i < list_2.Count; i++)
        {
            builder.Append(list_2[i]);
            builder.Append("+");
            if (((((i % 0xc7) == 0) || (i == (list_2.Count - 1))) && (i != 0)) || (list_2.Count == 1))
            {
                string str3 = string.Format("http://finance.yahoo.com/d/quotes.csv?s={0}&f=sn", builder.ToString());
                foreach (string str5 in this.method_15(str3, false).Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    string[] strArray4 = str5.Split(new string[] { "\",\"" }, StringSplitOptions.None);
                    if (strArray4.Length == 2)
                    {
                        string key = strArray4[0].Trim(new char[] { '"' });
                        string str2 = strArray4[1].Trim(new char[] { '"', '\r' });
                        if (!dictionary.ContainsKey(key))
                        {
                            dictionary.Add(key, str2);
                        }
                    }
                }
                builder.Remove(0, builder.Length);
            }
            if (this.bool_0)
            {
                return dictionary;
            }
        }
        return dictionary;
    }

    public void method_4(Delegate4 delegate4_1)
    {
        Delegate4 delegate3;
        Delegate4 delegate2 = this.delegate4_0;
        do
        {
            delegate3 = delegate2;
            Delegate4 delegate4 = (Delegate4) Delegate.Combine(delegate3, delegate4_1);
            delegate2 = Interlocked.CompareExchange<Delegate4>(ref this.delegate4_0, delegate4, delegate3);
        }
        while (delegate2 != delegate3);
    }

    public void method_5(Delegate4 delegate4_1)
    {
        Delegate4 delegate3;
        Delegate4 delegate2 = this.delegate4_0;
        do
        {
            delegate3 = delegate2;
            Delegate4 delegate4 = (Delegate4) Delegate.Remove(delegate3, delegate4_1);
            delegate2 = Interlocked.CompareExchange<Delegate4>(ref this.delegate4_0, delegate4, delegate3);
        }
        while (delegate2 != delegate3);
    }

    public void method_6(Delegate3 delegate3_1)
    {
        Delegate3 delegate3;
        Delegate3 delegate2 = this.delegate3_0;
        do
        {
            delegate3 = delegate2;
            Delegate3 delegate4 = (Delegate3) Delegate.Combine(delegate3, delegate3_1);
            delegate2 = Interlocked.CompareExchange<Delegate3>(ref this.delegate3_0, delegate4, delegate3);
        }
        while (delegate2 != delegate3);
    }

    public void method_7(Delegate3 delegate3_1)
    {
        Delegate3 delegate3;
        Delegate3 delegate2 = this.delegate3_0;
        do
        {
            delegate3 = delegate2;
            Delegate3 delegate4 = (Delegate3) Delegate.Remove(delegate3, delegate3_1);
            delegate2 = Interlocked.CompareExchange<Delegate3>(ref this.delegate3_0, delegate4, delegate3);
        }
        while (delegate2 != delegate3);
    }

    public int method_8()
    {
        return this.list_0.Count;
    }

    public bool method_9()
    {
        return this.bool_0;
    }

    public delegate void Delegate1(object sender, EventArgs2 e);

    public delegate void Delegate2(object sender, EventArgs3 e);

    public delegate void Delegate3(object sender, EventArgs4 e);

    public delegate void Delegate4(object sender, EventArgs5 e);
}

