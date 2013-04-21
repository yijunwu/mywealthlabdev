using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
internal class Control2 : UserControl
{
	internal class Class9
	{
		public string string_0;
		public string string_1;
		public Class9(string prefix, string country)
		{
			this.string_0 = prefix;
			this.string_1 = country;
		}
	}
	private IContainer icontainer_0;
	private GroupBox groupBox_0;
	private TextBox textBox_0;
	private Label label_0;
	private Label label_1;
	private ComboBox comboBox_0;
	public Class13 Class13_0
	{
		get
		{
			return new Class13(Delegate258.smethod_0(this.textBox_0), Enum4.User);
		}
	}
	public Control2()
	{
		this.method_4();
	}
	public void method_0()
	{
		if (Delegate260.smethod_0(Delegate259.smethod_0(this.comboBox_0)) == 0)
		{
			Delegate261.smethod_0(Delegate259.smethod_0(this.comboBox_0), new Control2.Class9("None", ""));
			Delegate261.smethod_0(Delegate259.smethod_0(this.comboBox_0), new Control2.Class9("AU:", "Australia"));
			Delegate261.smethod_0(Delegate259.smethod_0(this.comboBox_0), new Control2.Class9("BE:", "Belgium"));
			Delegate261.smethod_0(Delegate259.smethod_0(this.comboBox_0), new Control2.Class9("CA:", "Canada"));
			Delegate261.smethod_0(Delegate259.smethod_0(this.comboBox_0), new Control2.Class9("FR:", "France"));
			Delegate261.smethod_0(Delegate259.smethod_0(this.comboBox_0), new Control2.Class9("IT:", "Italy"));
			Delegate261.smethod_0(Delegate259.smethod_0(this.comboBox_0), new Control2.Class9("DE:", "Germany"));
			Delegate261.smethod_0(Delegate259.smethod_0(this.comboBox_0), new Control2.Class9("GB:", "United Kingdom"));
			Delegate261.smethod_0(Delegate259.smethod_0(this.comboBox_0), new Control2.Class9("JP:", "Japan"));
			Delegate261.smethod_0(Delegate259.smethod_0(this.comboBox_0), new Control2.Class9("NL:", "Netherlands"));
			Delegate261.smethod_0(Delegate259.smethod_0(this.comboBox_0), new Control2.Class9("ES:", "Spain"));
			Delegate261.smethod_0(Delegate259.smethod_0(this.comboBox_0), new Control2.Class9("SE:", "Sweden"));
		}
		Delegate262.smethod_0(this.textBox_0);
		Delegate263.smethod_0(this.comboBox_0, 0);
	}
	private void method_1(object sender, DrawItemEventArgs e)
	{
		Graphics object_ = Delegate264.smethod_0(e);
		Rectangle rectangle_ = Delegate265.smethod_0(e);
		Font font = Delegate41.smethod_0(Delegate267.smethod_0(Delegate266.smethod_0(e)), Delegate268.smethod_0(Delegate266.smethod_0(e)), FontStyle.Bold);
		if (Delegate269.smethod_0(e) >= 0)
		{
			Font font_ = (Delegate269.smethod_0(e) == 0) ? Delegate266.smethod_0(e) : font;
			string string_ = (Delegate270.smethod_0(Delegate259.smethod_0(this.comboBox_0), Delegate269.smethod_0(e)) as Control2.Class9).string_0;
			string string_2 = (Delegate270.smethod_0(Delegate259.smethod_0(this.comboBox_0), Delegate269.smethod_0(e)) as Control2.Class9).string_1;
			if ((Delegate271.smethod_0(e) & DrawItemState.Selected) != DrawItemState.None)
			{
				Delegate273.smethod_0(Delegate264.smethod_0(e), Delegate42.smethod_0(Delegate272.smethod_0(KnownColor.Highlight)), rectangle_);
				Delegate275.smethod_0(object_, string_, font_, Delegate42.smethod_0(Delegate272.smethod_0(KnownColor.HighlightText)), Delegate274.smethod_0(rectangle_));
				Delegate275.smethod_0(object_, Delegate114.smethod_0("\t\t", string_2), Delegate266.smethod_0(e), Delegate42.smethod_0(Delegate272.smethod_0(KnownColor.HighlightText)), Delegate274.smethod_0(rectangle_));
				Delegate276.smethod_0(e);
			}
			else
			{
				Delegate273.smethod_0(Delegate264.smethod_0(e), Delegate42.smethod_0(Delegate277.smethod_0(e)), rectangle_);
				Delegate275.smethod_0(object_, string_, font_, Delegate42.smethod_0(Delegate277.smethod_1(e)), Delegate274.smethod_0(rectangle_));
				Delegate275.smethod_0(object_, Delegate114.smethod_0("\t\t", string_2), Delegate266.smethod_0(e), Delegate42.smethod_0(Delegate277.smethod_1(e)), Delegate274.smethod_0(rectangle_));
			}
		}
		Delegate278.smethod_0(object_);
	}
	private void method_2(object sender, EventArgs e)
	{
		Class13 @class = new Class13(Delegate258.smethod_0(this.textBox_0), Enum4.User);
		@class.bool_0 = false;
		@class.char_0 = ' ';
		@class.method_3((Delegate279.smethod_0(this.comboBox_0) as Control2.Class9).string_0, ':');
		Delegate64.smethod_0(this.textBox_0, Delegate63.smethod_0(@class));
	}
	private void method_3(object sender, KeyPressEventArgs e)
	{
		char c = Delegate280.smethod_0(e);
		if (c >= 'a' && c <= 'z')
		{
			int num = (int)c;
			num = num - 97 + 65;
			Delegate282.smethod_0(e, Delegate281.smethod_0(num));
		}
	}
	protected override void Dispose(bool disposing)
	{
		if (disposing && this.icontainer_0 != null)
		{
			this.icontainer_0.Dispose();
		}
		Delegate72.smethod_0(this, disposing);
	}
	private void method_4()
	{
		this.groupBox_0 = Delegate6.smethod_0();
		this.comboBox_0 = Delegate43.smethod_0();
		this.label_1 = Delegate7.smethod_0();
		this.label_0 = Delegate7.smethod_0();
		this.textBox_0 = Delegate44.smethod_0();
		Delegate73.smethod_0(this.groupBox_0);
		Delegate73.smethod_1(this);
		Delegate75.smethod_0(Delegate74.smethod_0(this.groupBox_0), this.comboBox_0);
		Delegate75.smethod_0(Delegate74.smethod_0(this.groupBox_0), this.label_1);
		Delegate75.smethod_0(Delegate74.smethod_0(this.groupBox_0), this.label_0);
		Delegate75.smethod_0(Delegate74.smethod_0(this.groupBox_0), this.textBox_0);
		Delegate76.smethod_0(this.groupBox_0, Delegate12.smethod_0(7, 7));
		Delegate77.smethod_0(this.groupBox_0, "grpSymbols");
		Delegate78.smethod_0(this.groupBox_0, RightToLeft.No);
		Delegate79.smethod_0(this.groupBox_0, Delegate13.smethod_0(550, 343));
		Delegate80.smethod_0(this.groupBox_0, 5);
		Delegate81.smethod_0(this.groupBox_0, false);
		Delegate64.smethod_0(this.groupBox_0, "Symbols");
		Delegate283.smethod_0(this.comboBox_0, DrawMode.OwnerDrawFixed);
		Delegate284.smethod_0(this.comboBox_0, ComboBoxStyle.DropDownList);
		Delegate285.smethod_0(this.comboBox_0, true);
		Delegate76.smethod_0(this.comboBox_0, Delegate12.smethod_0(66, 315));
		Delegate286.smethod_0(this.comboBox_0, 12);
		Delegate77.smethod_0(this.comboBox_0, "cmbPrefix");
		Delegate79.smethod_0(this.comboBox_0, Delegate13.smethod_0(206, 21));
		Delegate80.smethod_0(this.comboBox_0, 3);
		Delegate287.smethod_0(this.comboBox_0, new DrawItemEventHandler(this.method_1));
		Delegate288.smethod_0(this.comboBox_0, new EventHandler(this.method_2));
		Delegate82.smethod_0(this.label_1, true);
		Delegate76.smethod_0(this.label_1, Delegate12.smethod_0(3, 318));
		Delegate77.smethod_0(this.label_1, "lblPrefix");
		Delegate79.smethod_0(this.label_1, Delegate13.smethod_0(57, 13));
		Delegate80.smethod_0(this.label_1, 2);
		Delegate64.smethod_0(this.label_1, "Add prefix:");
		Delegate82.smethod_0(this.label_0, true);
		Delegate76.smethod_0(this.label_0, Delegate12.smethod_0(6, 19));
		Delegate77.smethod_0(this.label_0, "lblSymbols");
		Delegate79.smethod_0(this.label_0, Delegate13.smethod_0(266, 13));
		Delegate80.smethod_0(this.label_0, 1);
		Delegate64.smethod_0(this.label_0, "Enter Symbols below, separated by spaces or commas.");
		Delegate94.smethod_1(this.textBox_0, Delegate93.smethod_2());
		Delegate76.smethod_0(this.textBox_0, Delegate12.smethod_0(6, 40));
		Delegate289.smethod_0(this.textBox_0, true);
		Delegate77.smethod_0(this.textBox_0, "txtSymbols");
		Delegate290.smethod_0(this.textBox_0, ScrollBars.Both);
		Delegate79.smethod_0(this.textBox_0, Delegate13.smethod_0(537, 267));
		Delegate80.smethod_0(this.textBox_0, 0);
		Delegate291.smethod_0(this.textBox_0, new KeyPressEventHandler(this.method_3));
		Delegate90.smethod_0(this, Delegate16.smethod_0(6f, 13f));
		Delegate91.smethod_0(this, AutoScaleMode.Font);
		Delegate75.smethod_0(Delegate74.smethod_1(this), this.groupBox_0);
		Delegate77.smethod_1(this, "WizardPageSymbols");
		Delegate78.smethod_0(this, RightToLeft.Yes);
		Delegate79.smethod_1(this, Delegate13.smethod_0(560, 353));
		Delegate67.smethod_2(this.groupBox_0, false);
		Delegate73.smethod_2(this.groupBox_0);
		Delegate67.smethod_3(this, false);
	}
}
