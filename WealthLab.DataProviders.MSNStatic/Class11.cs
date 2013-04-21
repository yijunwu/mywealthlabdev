using System;
internal static class Class11
{
	private static char[] char_0;
	public static string smethod_0(string string_0)
	{
		char[] array = Class11.char_0;
		for (int i = 0; i < array.Length; i++)
		{
			char c = array[i];
			string_0 = Delegate241.smethod_0(string_0, Delegate167.smethod_0("%", (int)c), c.ToString());
		}
		return string_0;
	}
	public static string smethod_1(string string_0)
	{
		char[] array = Class11.char_0;
		for (int i = 0; i < array.Length; i++)
		{
			char c = array[i];
			string_0 = Delegate241.smethod_0(string_0, c.ToString(), Delegate167.smethod_0("%", (int)c));
		}
		return string_0;
	}
	static Class11()
	{
		// Note: this type is marked as 'beforefieldinit'.
		char[] expr_07 = new char[9];
		Delegate306.smethod_0(expr_07, fieldof(<PrivateImplementationDetails>{A425EAFD-A689-4AF5-9D5E-3AB6BB6F6CF6}.$$method0x6000179-1).FieldHandle);
		Class11.char_0 = expr_07;
	}
}
