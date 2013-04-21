using System;
using System.Collections.Generic;
using System.Text;
internal class Class13
{
	public bool bool_0 = true;
	public char char_0 = ',';
	public List<string> list_0 = new List<string>();
	private char[] char_1;
	private char[] char_2;
	public string this[int int_0]
	{
		get
		{
			return this.list_0[int_0];
		}
	}
	public int Int32_0
	{
		get
		{
			return this.list_0.Count;
		}
	}
	public Class13()
	{
		char[] expr_21 = new char[4];
		Delegate306.smethod_0(expr_21, fieldof(<PrivateImplementationDetails>{A425EAFD-A689-4AF5-9D5E-3AB6BB6F6CF6}.$$method0x600016d-1).FieldHandle);
		this.char_1 = expr_21;
		char[] expr_38 = new char[3];
		Delegate306.smethod_0(expr_38, fieldof(<PrivateImplementationDetails>{A425EAFD-A689-4AF5-9D5E-3AB6BB6F6CF6}.$$method0x600016d-2).FieldHandle);
		this.char_2 = expr_38;
		base..ctor();
	}
	public Class13(string text, Enum4 mode)
	{
		char[] expr_21 = new char[4];
		Delegate306.smethod_0(expr_21, fieldof(<PrivateImplementationDetails>{A425EAFD-A689-4AF5-9D5E-3AB6BB6F6CF6}.$$method0x600016e-1).FieldHandle);
		this.char_1 = expr_21;
		char[] expr_38 = new char[3];
		Delegate306.smethod_0(expr_38, fieldof(<PrivateImplementationDetails>{A425EAFD-A689-4AF5-9D5E-3AB6BB6F6CF6}.$$method0x600016e-2).FieldHandle);
		this.char_2 = expr_38;
		base..ctor();
		this.method_0(text, mode);
	}
	public Class13(List<string> symbols)
	{
		char[] expr_21 = new char[4];
		Delegate306.smethod_0(expr_21, fieldof(<PrivateImplementationDetails>{A425EAFD-A689-4AF5-9D5E-3AB6BB6F6CF6}.$$method0x600016f-1).FieldHandle);
		this.char_1 = expr_21;
		char[] expr_38 = new char[3];
		Delegate306.smethod_0(expr_38, fieldof(<PrivateImplementationDetails>{A425EAFD-A689-4AF5-9D5E-3AB6BB6F6CF6}.$$method0x600016f-2).FieldHandle);
		this.char_2 = expr_38;
		base..ctor();
		this.list_0 = symbols;
	}
	public void method_0(string string_0, Enum4 enum4_0)
	{
		switch (enum4_0)
		{
		case Enum4.DSString:
			if (string_0 != null)
			{
				string[] array = Delegate307.smethod_0(string_0, this.char_2, StringSplitOptions.RemoveEmptyEntries);
				string[] array2 = array;
				for (int i = 0; i < array2.Length; i++)
				{
					string text = array2[i];
					object arg_4C_0 = text;
					char[] expr_41 = new char[3];
					Delegate306.smethod_0(expr_41, fieldof(<PrivateImplementationDetails>{A425EAFD-A689-4AF5-9D5E-3AB6BB6F6CF6}.$$method0x6000170-4).FieldHandle);
					this.method_2(Delegate115.smethod_0(arg_4C_0, expr_41));
				}
			}
			break;
		case Enum4.User:
			if (string_0 != null)
			{
				string_0 = Delegate193.smethod_1(string_0);
				string[] array4;
				if (Delegate238.smethod_2(string_0, "\""))
				{
					int num = 1;
					string[] array3 = Delegate244.smethod_0(string_0, new char[]
					{
						'"'
					});
					int j;
					for (j = 1; j < array3.Length; j += 2)
					{
						string text2 = array3[j];
						object arg_C0_0 = text2;
						char[] expr_B5 = new char[3];
						Delegate306.smethod_0(expr_B5, fieldof(<PrivateImplementationDetails>{A425EAFD-A689-4AF5-9D5E-3AB6BB6F6CF6}.$$method0x6000170-1).FieldHandle);
						this.method_2(Delegate115.smethod_0(arg_C0_0, expr_B5));
					}
					if (num == 0)
					{
						j = 1;
					}
					else
					{
						j = 0;
					}
					while (j < array3.Length)
					{
						array4 = Delegate307.smethod_0(array3[j], this.char_1, StringSplitOptions.RemoveEmptyEntries);
						string[] array5 = array4;
						for (int k = 0; k < array5.Length; k++)
						{
							string text3 = array5[k];
							object arg_113_0 = text3;
							char[] expr_108 = new char[3];
							Delegate306.smethod_0(expr_108, fieldof(<PrivateImplementationDetails>{A425EAFD-A689-4AF5-9D5E-3AB6BB6F6CF6}.$$method0x6000170-2).FieldHandle);
							this.method_2(Delegate115.smethod_0(arg_113_0, expr_108));
						}
						j += 2;
					}
					return;
				}
				array4 = Delegate307.smethod_0(string_0, this.char_1, StringSplitOptions.RemoveEmptyEntries);
				string[] array6 = array4;
				for (int l = 0; l < array6.Length; l++)
				{
					string text4 = array6[l];
					object arg_168_0 = text4;
					char[] expr_15D = new char[3];
					Delegate306.smethod_0(expr_15D, fieldof(<PrivateImplementationDetails>{A425EAFD-A689-4AF5-9D5E-3AB6BB6F6CF6}.$$method0x6000170-3).FieldHandle);
					this.method_2(Delegate115.smethod_0(arg_168_0, expr_15D));
				}
				return;
			}
			break;
		default:
			return;
		}
	}
	public void method_1()
	{
		this.list_0.Clear();
	}
	private bool method_2(string string_0)
	{
		if (!this.list_0.Contains(string_0) && Delegate51.smethod_1(string_0, ""))
		{
			this.list_0.Add(string_0);
			return true;
		}
		return false;
	}
	public override string ToString()
	{
		StringBuilder object_ = Delegate20.smethod_0();
		if (this.bool_0)
		{
			this.list_0.Sort();
		}
		foreach (string current in this.list_0)
		{
			Delegate111.smethod_0(object_, current);
			Delegate308.smethod_0(object_, this.char_0);
		}
		if (Delegate309.smethod_0(object_) > 0)
		{
			Delegate310.smethod_0(object_, Delegate309.smethod_0(object_) - 1, 1);
		}
		return Delegate63.smethod_0(object_);
	}
	public void method_3(string string_0, char char_3)
	{
		string_0 = (Delegate51.smethod_0(string_0, "None") ? "" : string_0);
		for (int i = 0; i < this.list_0.Count; i++)
		{
			string[] array = Delegate244.smethod_0(this.list_0[i], new char[]
			{
				char_3
			});
			if (array.Length > 0 && Delegate51.smethod_1(array[array.Length - 1], ""))
			{
				this.list_0[i] = Delegate114.smethod_0(string_0, array[array.Length - 1]);
			}
		}
	}
	public void method_4(string string_0, char char_3)
	{
		string_0 = (Delegate51.smethod_0(string_0, "None") ? "" : string_0);
		for (int i = 0; i < this.list_0.Count; i++)
		{
			string[] array = Delegate244.smethod_0(this.list_0[i], new char[]
			{
				char_3
			});
			if (array.Length > 0 && Delegate51.smethod_1(array[0], ""))
			{
				this.list_0[i] = Delegate114.smethod_0(array[0], string_0);
			}
		}
	}
}
