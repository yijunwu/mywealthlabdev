using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using WealthLab;
using WealthLab.DataProviders.AsciiFilesStatic;

internal static class Class6
{
    private static Config config_0 = Config.Desereailize();
    private static Delegate0 delegate0_0;
    private static Delegate0 delegate0_1;
    private static EventHandler eventHandler_0;
    private static readonly object object_0 = new object();

    public static void smethod_0(Delegate0 delegate0_2)
    {
        Delegate0 delegate3;
        Delegate0 delegate2 = delegate0_0;
        do
        {
            delegate3 = delegate2;
            Delegate0 delegate4 = (Delegate0) Delegate.Combine(delegate3, delegate0_2);
            delegate2 = Interlocked.CompareExchange<Delegate0>(ref delegate0_0, delegate4, delegate3);
        }
        while (delegate2 != delegate3);
    }

    public static void smethod_1(Delegate0 delegate0_2)
    {
        Delegate0 delegate3;
        Delegate0 delegate2 = delegate0_0;
        do
        {
            delegate3 = delegate2;
            Delegate0 delegate4 = (Delegate0) Delegate.Remove(delegate3, delegate0_2);
            delegate2 = Interlocked.CompareExchange<Delegate0>(ref delegate0_0, delegate4, delegate3);
        }
        while (delegate2 != delegate3);
    }

    public static void smethod_10(Bars bars_0, string string_0, string string_1)
    {
        if (smethod_8(string_0, string_1) == -1)
        {
            AsciiCache item = new AsciiCache();
            item.WriteCache(bars_0, string_0, string_1);
            lock (object_0)
            {
                config_0.AsciiCacheList.Add(item);
                config_0.Serialize();
            }
            if (delegate0_0 != null)
            {
                delegate0_0(new EventArgs0(item));
            }
        }
    }

    public static void smethod_11()
    {
        lock (object_0)
        {
            smethod_6().AsciiCacheList.Clear();
            smethod_6().Serialize();
            if (eventHandler_0 != null)
            {
                eventHandler_0(null, EventArgs.Empty);
            }
            if (Directory.Exists(Config.CachePath))
            {
                Directory.Delete(Config.CachePath, true);
            }
        }
    }

    public static void smethod_2(Delegate0 delegate0_2)
    {
        Delegate0 delegate3;
        Delegate0 delegate2 = delegate0_1;
        do
        {
            delegate3 = delegate2;
            Delegate0 delegate4 = (Delegate0) Delegate.Combine(delegate3, delegate0_2);
            delegate2 = Interlocked.CompareExchange<Delegate0>(ref delegate0_1, delegate4, delegate3);
        }
        while (delegate2 != delegate3);
    }

    public static void smethod_3(Delegate0 delegate0_2)
    {
        Delegate0 delegate3;
        Delegate0 delegate2 = delegate0_1;
        do
        {
            delegate3 = delegate2;
            Delegate0 delegate4 = (Delegate0) Delegate.Remove(delegate3, delegate0_2);
            delegate2 = Interlocked.CompareExchange<Delegate0>(ref delegate0_1, delegate4, delegate3);
        }
        while (delegate2 != delegate3);
    }

    public static void smethod_4(EventHandler eventHandler_1)
    {
        EventHandler handler2;
        EventHandler handler = eventHandler_0;
        do
        {
            handler2 = handler;
            EventHandler handler3 = (EventHandler) Delegate.Combine(handler2, eventHandler_1);
            handler = Interlocked.CompareExchange<EventHandler>(ref eventHandler_0, handler3, handler2);
        }
        while (handler != handler2);
    }

    public static void smethod_5(EventHandler eventHandler_1)
    {
        EventHandler handler2;
        EventHandler handler = eventHandler_0;
        do
        {
            handler2 = handler;
            EventHandler handler3 = (EventHandler) Delegate.Remove(handler2, eventHandler_1);
            handler = Interlocked.CompareExchange<EventHandler>(ref eventHandler_0, handler3, handler2);
        }
        while (handler != handler2);
    }

    public static Config smethod_6()
    {
        return config_0;
    }

    public static Bars smethod_7(string string_0, string string_1)
    {
        int num = smethod_8(string_0, string_1);
        if (num > -1)
        {
            AsciiCache cache = config_0.AsciiCacheList[num];
            if (smethod_9(cache, num))
            {
                return cache.ReadCache();
            }
        }
        return null;
    }

    private static int smethod_8(string string_0, string string_1)
    {
        for (int i = 0; i < config_0.AsciiCacheList.Count; i++)
        {
            if ((config_0.AsciiCacheList[i].SourceFileName == string_0) && (config_0.AsciiCacheList[i].DataSet == string_1))
            {
                return i;
            }
        }
        return -1;
    }

    private static bool smethod_9(AsciiCache asciiCache_0, int int_0)
    {
        string cachePath = asciiCache_0.GetCachePath(asciiCache_0.FileName);
        if ((File.Exists(cachePath) && File.Exists(asciiCache_0.SourceFileName)) && asciiCache_0.SourceLastWriteTime.Equals(File.GetLastWriteTime(asciiCache_0.SourceFileName)))
        {
            return true;
        }
        lock (object_0)
        {
            config_0.AsciiCacheList.RemoveAt(int_0);
            config_0.Serialize();
        }
        if (File.Exists(cachePath))
        {
            File.Delete(cachePath);
        }
        foreach (string str2 in asciiCache_0.DataSeriesFiles)
        {
            string path = asciiCache_0.GetCachePath(str2);
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
        if (delegate0_1 != null)
        {
            delegate0_1(new EventArgs0(asciiCache_0));
        }
        return false;
    }

    public delegate void Delegate0(EventArgs0 eventArgs0_0);
}

