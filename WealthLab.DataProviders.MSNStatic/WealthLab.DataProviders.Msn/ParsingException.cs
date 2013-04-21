using System;
namespace WealthLab.DataProviders.Msn
{
	internal class ParsingException : Exception
	{
		public ParsingException(string message) : base(message)
		{
		}
	}
}
