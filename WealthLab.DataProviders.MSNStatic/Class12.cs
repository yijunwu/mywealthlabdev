using System;
using WealthLab.DataProviders.Helper;
internal class Class12
{
	private ClassificationGroup classificationGroup_0;
	private Class13 class13_0 = new Class13();
	private Class13 class13_1 = new Class13();
	private string string_0;
	public Class13 Class13_0
	{
		get
		{
			return this.class13_0;
		}
	}
	public Class13 Class13_1
	{
		get
		{
			return this.class13_1;
		}
	}
	public Class12(ClassificationGroup root)
	{
		this.classificationGroup_0 = root;
	}
	private void method_0(string string_1, ClassificationGroup classificationGroup_1)
	{
		for (int i = 0; i < classificationGroup_1.Groups.Count; i++)
		{
			if (Delegate51.smethod_0(classificationGroup_1.Groups[i].Type, "Symbols") && Delegate51.smethod_0(classificationGroup_1.Groups[i].ID, string_1))
			{
				this.string_0 = classificationGroup_1.Groups[i].Symbols;
				return;
			}
			this.method_0(string_1, classificationGroup_1.Groups[i]);
		}
	}
	public Class13 method_1(Class13 class13_2, params string[] string_1)
	{
		Log.TraceMethod(new object[]
		{
			Delegate63.smethod_0(class13_2)
		});
		Class13 @class = new Class13();
		for (int i = 0; i < string_1.Length; i++)
		{
			string string_2 = string_1[i];
			this.string_0 = string.Empty;
			this.method_0(string_2, this.classificationGroup_0);
			@class.method_0(this.string_0, Enum4.User);
		}
		this.class13_0.list_0.Clear();
		this.class13_1.list_0.Clear();
		foreach (string current in @class.list_0)
		{
			if (!class13_2.list_0.Contains(current))
			{
				this.class13_1.list_0.Add(current);
			}
		}
		foreach (string current2 in class13_2.list_0)
		{
			if (!@class.list_0.Contains(current2))
			{
				this.class13_0.list_0.Add(current2);
			}
		}
		return @class;
	}
}
