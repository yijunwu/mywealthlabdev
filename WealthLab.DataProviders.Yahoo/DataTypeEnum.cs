using System;

///WYJ fix, original name: Enum4
[Flags]
internal enum DataTypeEnum
{
    Quote = 1,  ///WYJ note: quotes - price and volume
    SnD = 2,  ///WYJ note: Dividend and split data
    RealTime = 4   ///WYJ note: probably means real-time data, which requires login
}

