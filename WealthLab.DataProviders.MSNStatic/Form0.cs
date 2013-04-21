using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using WealthLab.DataProviders.Msn;
internal class Form0 : Form
{
	private IContainer icontainer_0;
	private Button button_0;
	private Button button_1;
	private Label label_0;
	private Panel panel_0;
	private NumericUpDown numericUpDown_0;
	private Label label_1;
	private NumericUpDown numericUpDown_1;
	private Label label_2;
	private CheckBox checkBox_0;
	private string string_0 = string.Empty;
	protected override void Dispose(bool disposing)
	{
		if (disposing && this.icontainer_0 != null)
		{
			this.icontainer_0.Dispose();
		}
		Delegate212.smethod_0(this, disposing);
	}
	private void method_0()
	{
		this.button_0 = Delegate11.smethod_0();
		this.button_1 = Delegate11.smethod_0();
		this.label_0 = Delegate7.smethod_0();
		this.panel_0 = Delegate28.smethod_0();
		this.numericUpDown_0 = Delegate29.smethod_0();
		this.label_1 = Delegate7.smethod_0();
		this.numericUpDown_1 = Delegate29.smethod_0();
		this.label_2 = Delegate7.smethod_0();
		this.checkBox_0 = Delegate9.smethod_0();
		Delegate73.smethod_0(this.panel_0);
		((ISupportInitialize)this.numericUpDown_0).BeginInit();
		((ISupportInitialize)this.numericUpDown_1).BeginInit();
		Delegate73.smethod_1(this);
		Delegate92.smethod_0(this.button_0, AnchorStyles.Bottom | AnchorStyles.Right);
		Delegate213.smethod_0(this.button_0, DialogResult.Cancel);
		Delegate76.smethod_0(this.button_0, Delegate12.smethod_0(443, 150));
		Delegate77.smethod_0(this.button_0, "btnCancel");
		Delegate79.smethod_0(this.button_0, Delegate13.smethod_0(80, 24));
		Delegate80.smethod_0(this.button_0, 9);
		Delegate64.smethod_0(this.button_0, "Cancel");
		Delegate85.smethod_0(this.button_0, true);
		Delegate92.smethod_0(this.button_1, AnchorStyles.Bottom | AnchorStyles.Right);
		Delegate213.smethod_0(this.button_1, DialogResult.OK);
		Delegate76.smethod_0(this.button_1, Delegate12.smethod_0(357, 150));
		Delegate77.smethod_0(this.button_1, "btnOk");
		Delegate79.smethod_0(this.button_1, Delegate13.smethod_0(80, 24));
		Delegate80.smethod_0(this.button_1, 8);
		Delegate64.smethod_0(this.button_1, "OK");
		Delegate85.smethod_0(this.button_1, true);
		Delegate88.smethod_0(this.button_1, new EventHandler(this.method_1));
		Delegate82.smethod_0(this.label_0, true);
		Delegate89.smethod_0(this.label_0, Delegate15.smethod_0("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 204));
		Delegate76.smethod_0(this.label_0, Delegate12.smethod_0(80, 14));
		Delegate77.smethod_0(this.label_0, "lblInfo");
		Delegate79.smethod_0(this.label_0, Delegate13.smethod_0(352, 13));
		Delegate80.smethod_0(this.label_0, 2);
		Delegate64.smethod_0(this.label_0, "These settings will apply to all DataSets of the MSN provider");
		Delegate94.smethod_1(this.panel_0, Delegate93.smethod_1());
		Delegate75.smethod_0(Delegate74.smethod_0(this.panel_0), this.label_0);
		Delegate76.smethod_0(this.panel_0, Delegate12.smethod_0(0, 0));
		Delegate77.smethod_0(this.panel_0, "pnlInfo");
		Delegate79.smethod_0(this.panel_0, Delegate13.smethod_0(536, 41));
		Delegate80.smethod_0(this.panel_0, 3);
		Delegate76.smethod_0(this.numericUpDown_0, Delegate12.smethod_0(12, 56));
		object arg_2BB_0 = this.numericUpDown_0;
		int[] array = new int[4];
		array[0] = 10;
		Delegate214.smethod_0(arg_2BB_0, Delegate30.smethod_0(array));
		object arg_2D7_0 = this.numericUpDown_0;
		int[] array2 = new int[4];
		array2[0] = 1;
		Delegate214.smethod_1(arg_2D7_0, Delegate30.smethod_0(array2));
		Delegate77.smethod_0(this.numericUpDown_0, "numThreadCount");
		Delegate79.smethod_0(this.numericUpDown_0, Delegate13.smethod_0(44, 20));
		Delegate80.smethod_0(this.numericUpDown_0, 0);
		object arg_323_0 = this.numericUpDown_0;
		int[] array3 = new int[4];
		array3[0] = 1;
		Delegate214.smethod_2(arg_323_0, Delegate30.smethod_0(array3));
		Delegate82.smethod_0(this.label_1, true);
		Delegate76.smethod_0(this.label_1, Delegate12.smethod_0(62, 58));
		Delegate77.smethod_0(this.label_1, "lblThreadCount");
		Delegate79.smethod_0(this.label_1, Delegate13.smethod_0(356, 13));
		Delegate80.smethod_0(this.label_1, 5);
		Delegate64.smethod_0(this.label_1, "Number of threads when loading historical data (recommended setting: 10)");
		Delegate76.smethod_0(this.numericUpDown_1, Delegate12.smethod_0(12, 82));
		object arg_3B6_0 = this.numericUpDown_1;
		int[] array4 = new int[4];
		array4[0] = 5;
		Delegate214.smethod_0(arg_3B6_0, Delegate30.smethod_0(array4));
		object arg_3D5_0 = this.numericUpDown_1;
		int[] array5 = new int[4];
		array5[0] = 1;
		Delegate214.smethod_1(arg_3D5_0, Delegate30.smethod_0(array5));
		Delegate77.smethod_0(this.numericUpDown_1, "numAttemptCount");
		Delegate79.smethod_0(this.numericUpDown_1, Delegate13.smethod_0(43, 20));
		Delegate80.smethod_0(this.numericUpDown_1, 1);
		object arg_424_0 = this.numericUpDown_1;
		int[] array6 = new int[4];
		array6[0] = 1;
		Delegate214.smethod_2(arg_424_0, Delegate30.smethod_0(array6));
		Delegate82.smethod_0(this.label_2, true);
		Delegate76.smethod_0(this.label_2, Delegate12.smethod_0(62, 86));
		Delegate77.smethod_0(this.label_2, "lblAttemptCount");
		Delegate79.smethod_0(this.label_2, Delegate13.smethod_0(383, 13));
		Delegate80.smethod_0(this.label_2, 7);
		Delegate64.smethod_0(this.label_2, "Number of times to retry when server didn't return data (recommended setting: 5)");
		Delegate82.smethod_0(this.checkBox_0, true);
		Delegate76.smethod_0(this.checkBox_0, Delegate12.smethod_0(12, 108));
		Delegate77.smethod_0(this.checkBox_0, "cbDividendAdj");
		Delegate79.smethod_0(this.checkBox_0, Delegate13.smethod_0(162, 17));
		Delegate80.smethod_0(this.checkBox_0, 4);
		Delegate64.smethod_0(this.checkBox_0, "Perform Dividend Adjustment");
		Delegate85.smethod_0(this.checkBox_0, true);
		Delegate90.smethod_0(this, Delegate16.smethod_0(6f, 13f));
		Delegate91.smethod_0(this, AutoScaleMode.Font);
		Delegate215.smethod_0(this, Delegate13.smethod_0(535, 180));
		Delegate75.smethod_0(Delegate74.smethod_1(this), this.checkBox_0);
		Delegate75.smethod_0(Delegate74.smethod_1(this), this.label_2);
		Delegate75.smethod_0(Delegate74.smethod_1(this), this.numericUpDown_1);
		Delegate75.smethod_0(Delegate74.smethod_1(this), this.label_1);
		Delegate75.smethod_0(Delegate74.smethod_1(this), this.numericUpDown_0);
		Delegate75.smethod_0(Delegate74.smethod_1(this), this.panel_0);
		Delegate75.smethod_0(Delegate74.smethod_1(this), this.button_1);
		Delegate75.smethod_0(Delegate74.smethod_1(this), this.button_0);
		Delegate216.smethod_0(this, FormBorderStyle.FixedSingle);
		Delegate217.smethod_0(this, false);
		Delegate217.smethod_1(this, false);
		Delegate77.smethod_1(this, "ProviderSettingsForm");
		Delegate217.smethod_2(this, false);
		Delegate217.smethod_3(this, false);
		Delegate218.smethod_0(this, FormStartPosition.CenterParent);
		Delegate64.smethod_0(this, "Provider Settings");
		Delegate219.smethod_0(this, new EventHandler(this.method_3));
		Delegate67.smethod_2(this.panel_0, false);
		Delegate73.smethod_2(this.panel_0);
		((ISupportInitialize)this.numericUpDown_0).EndInit();
		((ISupportInitialize)this.numericUpDown_1).EndInit();
		Delegate67.smethod_3(this, false);
		Delegate73.smethod_3(this);
	}
	public Form0()
	{
		this.string_0 = MsnStaticProvider.DataPath;
		this.method_0();
	}
	private void method_1(object sender, EventArgs e)
	{
		if (Delegate51.smethod_0(this.string_0, string.Empty))
		{
			throw Delegate31.smethod_0("The settings file folder is not specified");
		}
		new MsnClientSettings
		{
			ThreadCount = Delegate221.smethod_0(Delegate220.smethod_0(this.numericUpDown_0)),
			AttemptCount = Delegate221.smethod_0(Delegate220.smethod_0(this.numericUpDown_1)),
			DividendAdj = Delegate70.smethod_0(this.checkBox_0)
		}.Serialize(this.string_0);
	}
	private void method_2(MsnClientSettings msnClientSettings_0)
	{
		Delegate214.smethod_2(this.numericUpDown_0, Delegate222.smethod_0(msnClientSettings_0.ThreadCount));
		Delegate214.smethod_2(this.numericUpDown_1, Delegate222.smethod_0(msnClientSettings_0.AttemptCount));
		Delegate66.smethod_0(this.checkBox_0, msnClientSettings_0.DividendAdj);
	}
	private void method_3(object sender, EventArgs e)
	{
		MsnClientSettings msnClientSettings = MsnClientSettings.Deserialize(this.string_0);
		msnClientSettings = ((msnClientSettings == null) ? new MsnClientSettings() : msnClientSettings);
		this.method_2(msnClientSettings);
	}
}
