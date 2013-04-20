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

///WYJ fix, original name: Class26
internal class DataFetcher
{
    private bool cancelFlag;
    private StaticDataHandler staticDataHandler;
    private StaticErrorHandler staticErrorHandler;
    private StreamingDataHandler streamingDataHandler;
    private StreamingErrorHandler streamingErrorHandler;
    private Dictionary<string, DateTimeVolume> dictionary_0 = new Dictionary<string, DateTimeVolume>();
    private IFormatProvider iformatProvider_0;
    private List<string> subscribedSymbols = new List<string>();  ///WYJ note: probably the real-time symbol list
    private List<ThreadControl> threadControls = new List<ThreadControl>();
    private Queue<DataRequest> queue_0 = new Queue<DataRequest>();
    private Thread thread_0;

    public DataFetcher()
    {
        CultureInfo info = new CultureInfo("en-US") {
            NumberFormat = { NumberDecimalSeparator = "." }
        };
        this.iformatProvider_0 = info;
    }

    public void AddStaticDataHandler(StaticDataHandler handler)
    {
        StaticDataHandler prevHandler;
        StaticDataHandler tmp = this.staticDataHandler;
        do
        {
            prevHandler = tmp;
            StaticDataHandler delegate4 = (StaticDataHandler) Delegate.Combine(prevHandler, handler);
            tmp = Interlocked.CompareExchange<StaticDataHandler>(ref this.staticDataHandler, delegate4, prevHandler);
        }
        while (tmp != prevHandler);
    }

    public void DeleteStaticDataHandler(StaticDataHandler handler)
    {
        StaticDataHandler prevHandler;
        StaticDataHandler tmp = this.staticDataHandler;
        do
        {
            prevHandler = tmp;
            StaticDataHandler delegate4 = (StaticDataHandler) Delegate.Remove(prevHandler, handler);
            tmp = Interlocked.CompareExchange<StaticDataHandler>(ref this.staticDataHandler, delegate4, prevHandler);
        }
        while (tmp != prevHandler);
    }

    public void SetCancelFlag(bool cancel)
    {
        this.cancelFlag = cancel;
    }

    public void unsubscribeSymbol(string symbol)
    {
        lock (this.subscribedSymbols)
        {
            this.subscribedSymbols.Remove(symbol);
        }
    }

    public void subscribeSymbol(string symbol)
    {
        lock (this.subscribedSymbols)
        {
            this.subscribedSymbols.Add(symbol);
        }
    }

    public void login()
    {
        Logger.LogParameters(new object[0]);
        if (((Login.GetCookie() == null) && (YahooStaticProvider.ClientSettings.Login != string.Empty)) && (YahooStaticProvider.ClientSettings.Password != string.Empty))
        {
            Login.LoginWith(YahooStaticProvider.ClientSettings.Login, YahooStaticProvider.ClientSettings.Password);
            if (Login.GetErrorMessage() != null)
            {
                Logger.Log(LogLevel.ERROR, Login.GetErrorMessage());
                MessageBox.Show("Yahoo! login failed. Error: " + Login.GetErrorMessage(), "Yahoo! login error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
    }

    ///WYJ fix, original name method_14()
    public void CancelUpdate()
    {
        Logger.LogParameters(new object[0]);
        if (!this.cancelFlag)
        {
            this.cancelFlag = true;
            lock (this.threadControls)
            {
                foreach (ThreadControl threadControl in this.threadControls)
                {
                    threadControl.notErrorHandling().WaitOne();
                    threadControl.GetThread().Abort();
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

    private string requestData(string string_0, bool bool_1)  ///WYJ note, the mothod that sends HTTP request to get data
    {
        string end = null;
        int num = 0;
        while (true)
        {
            num++;
            object[] string0 = new object[] { string_0, bool_1, string.Concat("Attempt ", num) };
            Logger.LogParameters(string0);
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(string_0);
            if (bool_1)
            {
                if (Login.GetCookie() == null)
                {
                    this.login();
                }
                if (Login.GetCookie() != null)
                {
                    httpWebRequest.Headers.Add(HttpRequestHeader.Cookie, Login.GetCookie());
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
                        Logger.Log(string.Concat("Result  ", string_0, "\r\n", end));
                    }
                }
            }
            catch (Exception exception1)
            {
                Exception exception = exception1;
                Logger.Log(LogLevel.ERROR, exception.Message);
                if (exception as WebException == null || num >= YahooStaticProvider.ClientSettings.AttemptCount)
                {
                    throw exception;
                }
                else
                {
                    Logger.Log(LogLevel.WARNING, string.Concat("New attempt ", string_0));
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

    public Bars processDataRequest(DataRequest req, bool isStreaming)  ///WYJ note, bool_1 probably means real-time data, which requires login
    {
        Logger.LogParameters(new object[] { req.getSymbol() });
        string str = this.getUrl(req.getSymbol(), req.getStartDate(), req.getEndDate(), DataTypeEnum.Quote);
        string str2 = this.requestData(str, false);
        Bars bars = this.parseQuoteData(req.getSymbol(), str2);
        if (isStreaming || YahooStaticProvider.ClientSettings.AlwaysPartialBar)
        {
            double open;
            double high;
            double low;
            double volume;
            if (Login.GetCookie() == null)
            {
                this.login();
            }
            str = this.getRealTimeDataUrl(req.getSymbol());
            str2 = this.requestData(str, true);
            Quote quote = this.parseQuote(str2, out open, out high, out low, out volume);
            if ((quote == null) || ((quote.TimeStamp.Date <= bars.Date[bars.Count - 1]) && (bars.Count != 0)))
            {
                return bars;
            }
            bars.Add(quote.TimeStamp.Date, open, high, low, quote.Price, volume);
        }
        return bars;
    }

    ///WYJ fix, original name: method_17
    private bool isNotEmptyOrNA(string string_0)   ///WYJ note, this method is never used
    {
        return ((!(string_0 == "N/A") && !(string_0 == string.Empty)) && !(string_0.Trim(new char[] { '"' }) == string.Empty));
    }


    ///WYJ fix, original name method_18
    private void calculateIncremetalVolume(ref Quote quote, double volume)   ///WYJ note, calculate incremental volume
    {
        if (this.dictionary_0.ContainsKey(quote.Symbol))
        {
            DateTimeVolume lastVolume = this.dictionary_0[quote.Symbol];
            if (lastVolume.dateTime.Date == quote.TimeStamp.Date)  ///WYJ note, the same day, subtract the previous volume to get the volume in the time period
            {
                quote.Size = volume - lastVolume.volume;
            }
            else  ///WYJ note, a new day, use the raw value as the volume in the time period
            {
                quote.Size = volume;
            }
            lastVolume.dateTime = quote.TimeStamp;
            lastVolume.volume = volume;
        }
        else
        {
            quote.Size = 0.0;   ///WYJ note, shouldn't it be volume, instead of 0.0?
            this.dictionary_0.Add(quote.Symbol, new DateTimeVolume(quote.TimeStamp, volume));
        }
    }

    ///WYJ fix, original signature public Quote method_19(string string_0)
    public Quote GetRealTimeQuoteForSymbol(string string_0)
    {
        string str = this.getRealTimeDataUrl(string_0);
        try
        {
            double open;
            double high;
            double low;
            double volume;
            string str2 = this.requestData(str, true);
            return this.parseQuote(str2, out open, out high, out low, out volume);
        }
        catch
        {
            return null;
        }
    }

    public void AddStaticErrorHandler(StaticErrorHandler delegate2_1)
    {
        StaticErrorHandler delegate3;
        StaticErrorHandler delegate2 = this.staticErrorHandler;
        do
        {
            delegate3 = delegate2;
            StaticErrorHandler delegate4 = (StaticErrorHandler) Delegate.Combine(delegate3, delegate2_1);
            delegate2 = Interlocked.CompareExchange<StaticErrorHandler>(ref this.staticErrorHandler, delegate4, delegate3);
        }
        while (delegate2 != delegate3);
    }

    ///WYJ fix, original name method_20
    private Quote parseQuote(string string_0, out double open, out double high, out double low, out double volume)
    {
        double num;
        double num2;
        string[] strArray = string_0.Split(new char[] { ',' });
        Quote quote = new Quote();
        volume = num = 0.0;
        low = num2 = num;
        open = high = num2;
        try
        {
            quote.Symbol = strArray[0].Trim(new char[] { '"' });
            quote.TimeStamp = DateTime.ParseExact(strArray[1].Trim(new char[] { '"' }) + " " + strArray[2].Trim(new char[] { '"' }), "M/d/yyyy h:mmtt", this.iformatProvider_0);
            quote.Open = open = Convert.ToDouble(strArray[3], this.iformatProvider_0);
            high = Convert.ToDouble(strArray[4], this.iformatProvider_0);
            low = Convert.ToDouble(strArray[5], this.iformatProvider_0);
            quote.Price = Convert.ToDouble(strArray[6], this.iformatProvider_0);
            volume = Convert.ToDouble(strArray[7], this.iformatProvider_0);
            quote.PreviousClose = Convert.ToDouble(strArray[8], this.iformatProvider_0);
            try
            {
                quote.Bid = Convert.ToDouble(strArray[9], this.iformatProvider_0);
                quote.Ask = Convert.ToDouble(strArray[10], this.iformatProvider_0);
            }
            catch
            {
            }
            this.calculateIncremetalVolume(ref quote, volume);
        }
        catch (Exception exception)
        {
            string str = exception.Message + " Line: " + string_0;
            Logger.Log(LogLevel.ERROR, str);
            quote = null;
        }
        return quote;
    }

    ///WYJ fix, original name method_21
    private void processStreamingQuoteData(string string_0)  ///WYJ fix, meaningful name candidate: processQuoteData
    {
        foreach (string str in string_0.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries))
        {
            double open;
            double high;
            double low;
            double volume;
            Quote quote = this.parseQuote(str, out open, out high, out low, out volume);
            if ((this.streamingDataHandler != null) && (quote != null))
            {
                this.streamingDataHandler(this, new StreamingDataEventArgs(quote, open, high, low));
            }
        }
    }

    ///WYJ fix, code from Reflector
    /*
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
                        Thread.Sleep(0x7d0);
                        goto Label_0006;
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
        Thread.Sleep(0x7d0);
        goto Label_0006;
    } */

    ///WYJ fix, original signature: private void method_22()
    private void requestAndProcessStreaming()
    {
        List<string> subscribed = new List<string>();
        while (!this.cancelFlag)
        {
            if (this.subscribedSymbols.Count > 0)
            {
                try
                {
                    lock (this.subscribedSymbols)
                    {
                        StringBuilder stringBuilder = new StringBuilder();
                        subscribed.Clear();
                        for (int i = 0; i < this.subscribedSymbols.Count; i++)
                        {
                            stringBuilder.Append(this.subscribedSymbols[i]);
                            stringBuilder.Append("+");
                            if ((i + 1) % 50 == 0 || i == this.subscribedSymbols.Count - 1)
                            {
                                subscribed.Add(stringBuilder.ToString());
                                stringBuilder.Remove(0, stringBuilder.Length);
                            }
                        }
                    }
                    foreach (string str in subscribed)
                    {
                        if (this.cancelFlag)
                        {
                            break;
                        }
                        string str1 = this.getRealTimeDataUrl(str);
                        string str2 = this.requestData(str1, true);
                        this.processStreamingQuoteData(str2);
                    }
                }
                catch (Exception exception1)
                {
                    Exception exception = exception1;
                    if (this.streamingErrorHandler != null)
                    {
                        this.streamingErrorHandler(this, new StreamingErrorEventArgs(string.Empty, exception.Message));
                    }
                }
            }
            Thread.Sleep(2000);
        }
    }

    ///WYJ fix, original name: method_23
    public void startStreamingRequesterAndProcessor()
    {
        this.cancelFlag = false;
        this.thread_0 = new Thread(new ThreadStart(this.requestAndProcessStreaming));
        this.thread_0.IsBackground = true;
        this.thread_0.Start();
    }

    ///WYJ fix, original signature: private Class28 method_24(string string_0)
    private SplitAndDividend parseDividendAndSplitData(string string_0)
    {
        SplitAndDividend snDData = new SplitAndDividend();
        foreach (string str in string_0.Split(new char[] { '\n' }))
        {
            if (str.StartsWith("DIVIDEND,"))
            {
                FundamentalItem item2 = new FundamentalItemYahooDividend("Dividend (Yahoo! Finance)");
                string[] strArray5 = str.Split(new char[] { ',' });
                item2.Date = DateTime.ParseExact(strArray5[1].Trim(), "yyyyMMdd", this.iformatProvider_0);
                item2.Value = Convert.ToDouble(strArray5[2].Trim(), this.iformatProvider_0);
                snDData.getDividend().Add(item2);
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
                snDData.getSplit().Add(item);
            }
        }
        snDData.getDividend().Reverse();
        snDData.getSplit().Reverse();
        return snDData;
    }

    ///WYJ fix, original name: method_25
    private SplitAndDividend processSplitAndDividendDataRequest(DataRequest class27_0)  ///WYJ note, get dividend and split data
    {
        string str = this.getUrl(class27_0.getSymbol(), class27_0.getSnDStartDate(), class27_0.getEndDate(), DataTypeEnum.SnD);
        string str2 = this.requestData(str, true);
        return this.parseDividendAndSplitData(str2);
    }

    ///WYJ fix, code from Reflector
    /*
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
    } */

    ///WYJ fix, original name: method_26
    private void processDataRequestQueue()  ///WYJ note, this is the method that is used in the downloading thread, see updateSecurityData method of this class
    {
        ThreadControl thread = null;
        try
        {
            try
            {
                lock (this.threadControls)
                {
                    thread = this.threadControls[Convert.ToInt32(Thread.CurrentThread.Name)];
                }
                while (!this.cancelFlag)
                {
                    DataRequest class27 = null;
                    lock (this.queue_0)
                    {
                        if (this.queue_0.Count <= 0)
                        {
                            break;
                        }
                        else
                        {
                            class27 = this.queue_0.Dequeue();
                        }
                    }
                    if (class27 == null)
                    {
                        continue;
                    }
                    Exception exception = null;
                    Bars bars = null;
                    SplitAndDividend class28 = null;
                    try
                    {
                        bars = this.processDataRequest(class27, (int)(class27.getDataType() & DataTypeEnum.RealTime) != 0);
                        if ((int)(class27.getDataType() & DataTypeEnum.SnD) != 0)
                        {
                            class28 = this.processSplitAndDividendDataRequest(class27);
                        }
                    }
                    catch (Exception exception2)
                    {
                        Exception exception1 = exception2;
                        Logger.Log(LogLevel.WARNING, string.Concat(class27.getSymbol(), " ", exception1.Message));
                        exception = exception1;
                    }
                    thread.notErrorHandling().Reset();
                    if (!this.cancelFlag)
                    {
                        if (exception != null)
                        {
                            if (this.staticErrorHandler != null)
                            {
                                this.staticErrorHandler(this, new StaticErrorEventArgs(class27, exception));
                            }
                        }
                        else
                        {
                            if (this.staticDataHandler != null)
                            {
                                this.staticDataHandler(this, new StaticDataEventArgs(class27, bars, class28));
                            }
                        }
                    }
                    thread.notErrorHandling().Set();
                }
            }
            catch (ThreadAbortException threadAbortException)
            {
                Logger.Log("Thread Abort");
            }
            catch (Exception exception4)
            {
                Exception exception3 = exception4;
                Logger.Log(LogLevel.ERROR, string.Concat("Thread execution error. ", exception3.Message));
                if (this.staticErrorHandler != null)
                {
                    this.staticErrorHandler(this, new StaticErrorEventArgs(null, exception3));
                }
            }
        }
        finally
        {
            if (thread != null)
            {
                thread.getThreadFinishEvent().Set();
            }
        }
    }
    ///WYJ fix, original signature: public void method_27(List<Class27> list_2)
    public void updateSecurityData(List<DataRequest> list_2) ///WYJ note, probably method that fetches data
    {
        Logger.LogParameters(new object[0]);
        this.threadControls.Clear();
        List<ManualResetEvent> threadList = new List<ManualResetEvent>();
        foreach (DataRequest class3 in list_2)
        {
            this.queue_0.Enqueue(class3);
        }
        int num2 = (this.queue_0.Count < YahooStaticProvider.ClientSettings.ThreadCount) ? this.queue_0.Count : YahooStaticProvider.ClientSettings.ThreadCount;
        for (int i = 0; i < num2; i++)
        {
            Thread thread = new Thread(new ThreadStart(this.processDataRequestQueue)) {
                Name = i.ToString(),
                IsBackground = true
            };
            ThreadControl item = new ThreadControl(thread);
            this.threadControls.Add(item);
            threadList.Add(item.getThreadFinishEvent());
        }
        foreach (ThreadControl class2 in this.threadControls)
        {
            class2.GetThread().Start();
        }
        WaitHandle.WaitAll(threadList.ToArray());
    }

    ///WYJ fix, original signature: private Bars method_28(string string_0, string string_1)
    private Bars parseQuoteData(string string_0, string string_1)
    {
        Logger.LogParameters(new object[] { string_0 });
        Bars bars = new Bars(Encoder.encode(string_0), BarScale.Daily, 0);
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
            Logger.Log(LogLevel.ERROR, str2);
            throw new QuoteDataParseException(str2);
        }
        return bars;
    }

    ///WYJ note, original name: method_29, this method is only used in RequestHistoricalData, which is never used anywhere
    ///This method should be named: requestDataBetweenDates
    public Bars method_29(string string_0, DateTime dateTime_0, DateTime dateTime_1)  ///WYJ note, this method is only used by YahooStaticProvider.RequestHistoricalData(), which is not used anywhere
    {
        DataRequest class2 = new DataRequest(string_0, dateTime_0, dateTime_1, dateTime_1, DataTypeEnum.Quote);
        Bars bars = null;
        try
        {
            bars = this.processDataRequest(class2, true);   ///WYJ note, second param true means real-time data
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

    public void DeleteStaticErrorHandler(StaticErrorHandler handler)
    {
        StaticErrorHandler prevHandler;
        StaticErrorHandler tmp = this.staticErrorHandler;
        do
        {
            prevHandler = tmp;
            StaticErrorHandler delegate4 = (StaticErrorHandler) Delegate.Remove(prevHandler, handler);
            tmp = Interlocked.CompareExchange<StaticErrorHandler>(ref this.staticErrorHandler, delegate4, prevHandler);
        }
        while (tmp != prevHandler);
    }

    ///WYJ fix, original signature: private string method_30(string string_0)
    private string getRealTimeDataUrl(string string_0)
    {
        return string.Format("http://download.finance.yahoo.com/d/quotes.csv?s={0}&f=sd1t1ohgl1vpba&e=.csv", string_0);  ///WYJ this is probably the real-time data url
    }

    private string getUrl(string string_0, DateTime startDate, DateTime endDate, DataTypeEnum enum4_0)
    {
        int num = startDate.Month - 1;
        int day = startDate.Day;
        int year = startDate.Year;
        int num4 = endDate.Month - 1;
        int num5 = endDate.Day;
        int num6 = endDate.Year;
        string str = "d";
        switch (enum4_0)
        {
            case DataTypeEnum.Quote:
                return string.Format("http://ichart.yahoo.com/table.csv?s={0}&a={1}&b={2}&c={3}&d={4}&e={5}&f={6}&g={7}&ignore=.csv", new object[] { string_0, num, day, year, num4, num5, num6, str });

            case DataTypeEnum.SnD:
                return string.Format("http://ichart.yahoo.com/x?s={0}&a={1}&b={2}&c={3}&d={4}&e={5}&f={6}&g=v&y=0&z=40000", new object[] { string_0, num, day, year, num4, num5, num6 });
        }
        return string.Empty;
    }

    public Dictionary<string, string> GetSymbolNames(List<DataRequest> requestList)
    {
        List<string> list = new List<string>();
        foreach (DataRequest class2 in requestList)
        {
            list.Add(class2.getSymbol());
        }
        return this.GetSymbolNames(list);
    }

    public Dictionary<string, string> GetSymbolNames(List<string> symbolList)
    {
        Dictionary<string, string> dictionary = new Dictionary<string, string>();
        StringBuilder builder = new StringBuilder();
        for (int i = 0; i < symbolList.Count; i++)
        {
            builder.Append(symbolList[i]);
            builder.Append("+");
            if (((((i % 0xc7) == 0) || (i == (symbolList.Count - 1))) && (i != 0)) || (symbolList.Count == 1))
            {
                string url = string.Format("http://finance.yahoo.com/d/quotes.csv?s={0}&f=sn", builder.ToString());
                foreach (string str5 in this.requestData(url, false).Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    string[] strArray4 = str5.Split(new string[] { "\",\"" }, StringSplitOptions.None);
                    if (strArray4.Length == 2)
                    {
                        string key = strArray4[0].Trim(new char[] { '"' });
                        string name = strArray4[1].Trim(new char[] { '"', '\r' });
                        if (!dictionary.ContainsKey(key))
                        {
                            dictionary.Add(key, name);
                        }
                    }
                }
                builder.Remove(0, builder.Length);
            }
            if (this.cancelFlag)
            {
                return dictionary;
            }
        }
        return dictionary;
    }

    ///WYJ fix, original name method_4
    public void AddStreamingErrorHandler(StreamingErrorHandler handler)
    {
        StreamingErrorHandler prevHandler;
        StreamingErrorHandler tmp = this.streamingErrorHandler;
        do
        {
            prevHandler = tmp;
            StreamingErrorHandler delegate4 = (StreamingErrorHandler) Delegate.Combine(prevHandler, handler);
            tmp = Interlocked.CompareExchange<StreamingErrorHandler>(ref this.streamingErrorHandler, delegate4, prevHandler);
        }
        while (tmp != prevHandler);
    }

    public void DeleteStreamingErrorHandler(StreamingErrorHandler handler)
    {
        StreamingErrorHandler prevHandler;
        StreamingErrorHandler tmp = this.streamingErrorHandler;
        do
        {
            prevHandler = tmp;
            StreamingErrorHandler delegate4 = (StreamingErrorHandler) Delegate.Remove(prevHandler, handler);
            tmp = Interlocked.CompareExchange<StreamingErrorHandler>(ref this.streamingErrorHandler, delegate4, prevHandler);
        }
        while (tmp != prevHandler);
    }

    public void AddStreamingDataHandler(StreamingDataHandler handler)
    {
        StreamingDataHandler prevHandler;
        StreamingDataHandler tmp = this.streamingDataHandler;
        do
        {
            prevHandler = tmp;
            StreamingDataHandler delegate4 = (StreamingDataHandler) Delegate.Combine(prevHandler, handler);
            tmp = Interlocked.CompareExchange<StreamingDataHandler>(ref this.streamingDataHandler, delegate4, prevHandler);
        }
        while (tmp != prevHandler);
    }

    public void DeleteStreamingDataHandler(StreamingDataHandler handler)
    {
        StreamingDataHandler prevHandler;
        StreamingDataHandler tmp = this.streamingDataHandler;
        do
        {
            prevHandler = tmp;
            StreamingDataHandler delegate4 = (StreamingDataHandler) Delegate.Remove(prevHandler, handler);
            tmp = Interlocked.CompareExchange<StreamingDataHandler>(ref this.streamingDataHandler, delegate4, prevHandler);
        }
        while (tmp != prevHandler);
    }

    public int GetSubscribedSymbolCount()
    {
        return this.subscribedSymbols.Count;
    }

    public bool GetCancelFlag()
    {
        return this.cancelFlag;
    }

    public delegate void StaticDataHandler(object sender, StaticDataEventArgs e);

    public delegate void StaticErrorHandler(object sender, StaticErrorEventArgs e);

    public delegate void StreamingDataHandler(object sender, StreamingDataEventArgs e);

    public delegate void StreamingErrorHandler(object sender, StreamingErrorEventArgs e);
}

