using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
internal class Control0 : UserControl
{
	private IContainer icontainer_0;
	private GroupBox groupBox_0;
	private RadioButton radioButton_0;
	private RadioButton radioButton_1;
	private Label label_0;
	private CheckBox checkBox_0;
	private Button button_0;
	private DateTimePicker dateTimePicker_0;
	private Label label_1;
	private Label label_2;
	private Label label_3;
	public DateTime DateTime_0
	{
		get
		{
			return Delegate68.smethod_0(this.dateTimePicker_0);
		}
	}
	public bool Boolean_0
	{
		get
		{
			return Delegate69.smethod_0(this.radioButton_0);
		}
	}
	public bool Boolean_1
	{
		get
		{
			return Delegate70.smethod_0(this.checkBox_0);
		}
	}
	public Control0()
	{
		this.method_4();
		Delegate64.smethod_0(this.label_3, Delegate63.smethod_0(Delegate62.smethod_0(Delegate61.smethod_0(Delegate60.smethod_0()))));
	}
	public void method_0()
	{
		Delegate65.smethod_0(this.radioButton_1, true);
		Delegate66.smethod_0(this.checkBox_0, false);
		Delegate67.smethod_0(this.checkBox_0, false);
	}
	private void method_1(object sender, EventArgs e)
	{
		Delegate67.smethod_0(this.checkBox_0, Delegate69.smethod_0(sender as RadioButton));
	}
	private void method_2(object sender, EventArgs e)
	{
	}
	private void method_3(object sender, EventArgs e)
	{
		Form0 object_ = new Form0();
		Delegate71.smethod_0(object_, this);
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
		this.label_2 = Delegate7.smethod_0();
		this.dateTimePicker_0 = Delegate8.smethod_0();
		this.label_1 = Delegate7.smethod_0();
		this.checkBox_0 = Delegate9.smethod_0();
		this.radioButton_0 = Delegate10.smethod_0();
		this.radioButton_1 = Delegate10.smethod_0();
		this.label_0 = Delegate7.smethod_0();
		this.button_0 = Delegate11.smethod_0();
		this.label_3 = Delegate7.smethod_0();
		Delegate73.smethod_0(this.groupBox_0);
		Delegate73.smethod_1(this);
		Delegate75.smethod_0(Delegate74.smethod_0(this.groupBox_0), this.label_2);
		Delegate75.smethod_0(Delegate74.smethod_0(this.groupBox_0), this.dateTimePicker_0);
		Delegate75.smethod_0(Delegate74.smethod_0(this.groupBox_0), this.label_1);
		Delegate75.smethod_0(Delegate74.smethod_0(this.groupBox_0), this.checkBox_0);
		Delegate75.smethod_0(Delegate74.smethod_0(this.groupBox_0), this.radioButton_0);
		Delegate75.smethod_0(Delegate74.smethod_0(this.groupBox_0), this.radioButton_1);
		Delegate75.smethod_0(Delegate74.smethod_0(this.groupBox_0), this.label_0);
		Delegate76.smethod_0(this.groupBox_0, Delegate12.smethod_0(7, 7));
		Delegate77.smethod_0(this.groupBox_0, "grpOptions");
		Delegate78.smethod_0(this.groupBox_0, RightToLeft.No);
		Delegate79.smethod_0(this.groupBox_0, Delegate13.smethod_0(550, 312));
		Delegate80.smethod_0(this.groupBox_0, 0);
		Delegate81.smethod_0(this.groupBox_0, false);
		Delegate64.smethod_0(this.groupBox_0, "MSN Dataset Options");
		Delegate82.smethod_0(this.label_2, true);
		Delegate76.smethod_0(this.label_2, Delegate12.smethod_0(8, 106));
		Delegate77.smethod_0(this.label_2, "lblStartingDateDesc");
		Delegate79.smethod_0(this.label_2, Delegate13.smethod_0(327, 13));
		Delegate80.smethod_0(this.label_2, 8);
		Delegate64.smethod_0(this.label_2, "Which date would you like to use as starting date of collected data?");
		Delegate83.smethod_0(this.dateTimePicker_0, DateTimePickerFormat.Short);
		Delegate76.smethod_0(this.dateTimePicker_0, Delegate12.smethod_0(84, 127));
		Delegate77.smethod_0(this.dateTimePicker_0, "dtStartingDate");
		Delegate79.smethod_0(this.dateTimePicker_0, Delegate13.smethod_0(112, 20));
		Delegate80.smethod_0(this.dateTimePicker_0, 7);
		Delegate84.smethod_0(this.dateTimePicker_0, Delegate14.smethod_0(2000, 1, 1, 0, 0, 0, 0));
		Delegate82.smethod_0(this.label_1, true);
		Delegate76.smethod_0(this.label_1, Delegate12.smethod_0(8, 131));
		Delegate77.smethod_0(this.label_1, "lblStartingDate");
		Delegate79.smethod_0(this.label_1, Delegate13.smethod_0(70, 13));
		Delegate80.smethod_0(this.label_1, 6);
		Delegate64.smethod_0(this.label_1, "Starting date:");
		Delegate82.smethod_0(this.checkBox_0, true);
		Delegate76.smethod_0(this.checkBox_0, Delegate12.smethod_0(31, 86));
		Delegate77.smethod_0(this.checkBox_0, "cbUpdateGroup");
		Delegate79.smethod_0(this.checkBox_0, Delegate13.smethod_0(329, 17));
		Delegate80.smethod_0(this.checkBox_0, 2);
		Delegate64.smethod_0(this.checkBox_0, "Update DataSet composition when Classification group changes");
		Delegate85.smethod_0(this.checkBox_0, true);
		Delegate67.smethod_1(this.checkBox_0, false);
		Delegate86.smethod_0(this.checkBox_0, new EventHandler(this.method_2));
		Delegate82.smethod_0(this.radioButton_0, true);
		Delegate76.smethod_0(this.radioButton_0, Delegate12.smethod_0(11, 63));
		Delegate77.smethod_0(this.radioButton_0, "rbClassification");
		Delegate79.smethod_0(this.radioButton_0, Delegate13.smethod_0(290, 17));
		Delegate80.smethod_0(this.radioButton_0, 1);
		Delegate65.smethod_1(this.radioButton_0, true);
		Delegate64.smethod_0(this.radioButton_0, "Select the Symbols from predefined Classification groups");
		Delegate85.smethod_0(this.radioButton_0, true);
		Delegate87.smethod_0(this.radioButton_0, new EventHandler(this.method_1));
		Delegate82.smethod_0(this.radioButton_1, true);
		Delegate76.smethod_0(this.radioButton_1, Delegate12.smethod_0(11, 40));
		Delegate77.smethod_0(this.radioButton_1, "rbManual");
		Delegate79.smethod_0(this.radioButton_1, Delegate13.smethod_0(289, 17));
		Delegate80.smethod_0(this.radioButton_1, 0);
		Delegate65.smethod_1(this.radioButton_1, true);
		Delegate64.smethod_0(this.radioButton_1, "Enter symbols manually or paste them from the Clipboard");
		Delegate85.smethod_0(this.radioButton_1, true);
		Delegate82.smethod_0(this.label_0, true);
		Delegate76.smethod_0(this.label_0, Delegate12.smethod_0(6, 19));
		Delegate77.smethod_0(this.label_0, "lblOptions");
		Delegate79.smethod_0(this.label_0, Delegate13.smethod_0(346, 13));
		Delegate80.smethod_0(this.label_0, 0);
		Delegate64.smethod_0(this.label_0, "How do you want to select the Symbols that will make up your DataSet?");
		Delegate76.smethod_0(this.button_0, Delegate12.smethod_0(7, 330));
		Delegate77.smethod_0(this.button_0, "btnProviderSettings");
		Delegate79.smethod_0(this.button_0, Delegate13.smethod_0(176, 24));
		Delegate80.smethod_0(this.button_0, 1);
		Delegate64.smethod_0(this.button_0, "Provider Settings");
		Delegate85.smethod_0(this.button_0, true);
		Delegate88.smethod_0(this.button_0, new EventHandler(this.method_3));
		Delegate82.smethod_0(this.label_3, true);
		Delegate89.smethod_0(this.label_3, Delegate15.smethod_0("Microsoft Sans Serif", 6.5f, FontStyle.Regular, GraphicsUnit.Point, 204));
		Delegate76.smethod_0(this.label_3, Delegate12.smethod_0(525, 342));
		Delegate77.smethod_0(this.label_3, "lblVersion");
		Delegate78.smethod_0(this.label_3, RightToLeft.Yes);
		Delegate79.smethod_0(this.label_3, Delegate13.smethod_0(34, 12));
		Delegate80.smethod_0(this.label_3, 9);
		Delegate64.smethod_0(this.label_3, "1.0.2.3");
		Delegate90.smethod_0(this, Delegate16.smethod_0(6f, 13f));
		Delegate91.smethod_0(this, AutoScaleMode.Font);
		Delegate75.smethod_0(Delegate74.smethod_1(this), this.label_3);
		Delegate75.smethod_0(Delegate74.smethod_1(this), this.button_0);
		Delegate75.smethod_0(Delegate74.smethod_1(this), this.groupBox_0);
		Delegate77.smethod_1(this, "WizardPageStart");
		Delegate78.smethod_0(this, RightToLeft.Yes);
		Delegate79.smethod_1(this, Delegate13.smethod_0(560, 353));
		Delegate67.smethod_2(this.groupBox_0, false);
		Delegate73.smethod_2(this.groupBox_0);
		Delegate67.smethod_3(this, false);
		Delegate73.smethod_3(this);
	}
}
