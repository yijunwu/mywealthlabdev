using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Net;
using System.Text;
using System.Windows.Forms;
using WealthLab.DataProviders.Helper;
using WealthLab.DataProviders.Msn;
internal class Control1 : UserControl
{
	private IContainer icontainer_0;
	private GroupBox groupBox_0;
	private TreeView treeView_0;
	private ListView listView_0;
	private Label label_0;
	private Button button_0;
	private Label label_1;
	private Label label_2;
	private Button button_1;
	private Button button_2;
	private Label label_3;
	private ColumnHeader columnHeader_0;
	private string string_0;
	private Font font_0;
	private Font font_1;
	private bool bool_0;
	private ClassificationGroup classificationGroup_0;
	private int int_0;
	public Class13 Class13_0
	{
		get
		{
			Class13 @class = new Class13();
			IEnumerator enumerator = Delegate109.smethod_0(Delegate108.smethod_0(this.listView_0));
			try
			{
				while (enumerator.MoveNext())
				{
					ListViewItem object_ = (ListViewItem)enumerator.Current;
					@class.method_0((Delegate110.smethod_0(object_) as ClassificationGroup).Symbols, Enum4.User);
				}
			}
			finally
			{
				IDisposable disposable = enumerator as IDisposable;
				if (disposable != null)
				{
					disposable.Dispose();
				}
			}
			return @class;
		}
	}
	public string String_0
	{
		get
		{
			StringBuilder object_ = Delegate20.smethod_0();
			IEnumerator enumerator = Delegate109.smethod_0(Delegate108.smethod_0(this.listView_0));
			try
			{
				while (enumerator.MoveNext())
				{
					ListViewItem object_2 = (ListViewItem)enumerator.Current;
					Delegate111.smethod_0(object_, (Delegate110.smethod_0(object_2) as ClassificationGroup).Name);
					Delegate111.smethod_0(object_, ", ");
				}
			}
			finally
			{
				IDisposable disposable = enumerator as IDisposable;
				if (disposable != null)
				{
					disposable.Dispose();
				}
			}
			string object_3 = Delegate63.smethod_0(object_);
			if (Delegate112.smethod_0(object_3) > 103)
			{
				object_3 = Delegate113.smethod_0(object_3, 100);
				object_3 = Delegate114.smethod_0(object_3, "...");
			}
			return Delegate115.smethod_0(object_3, new char[]
			{
				' ',
				','
			});
		}
	}
	public string String_1
	{
		get
		{
			StringBuilder object_ = Delegate20.smethod_0();
			IEnumerator enumerator = Delegate109.smethod_0(Delegate108.smethod_0(this.listView_0));
			try
			{
				while (enumerator.MoveNext())
				{
					ListViewItem object_2 = (ListViewItem)enumerator.Current;
					Delegate111.smethod_0(object_, (Delegate110.smethod_0(object_2) as ClassificationGroup).ID);
					Delegate111.smethod_0(object_, ",");
				}
			}
			finally
			{
				IDisposable disposable = enumerator as IDisposable;
				if (disposable != null)
				{
					disposable.Dispose();
				}
			}
			return Delegate63.smethod_0(object_);
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
	private void method_0()
	{
		this.groupBox_0 = Delegate6.smethod_0();
		this.label_3 = Delegate7.smethod_0();
		this.label_2 = Delegate7.smethod_0();
		this.button_1 = Delegate11.smethod_0();
		this.button_2 = Delegate11.smethod_0();
		this.listView_0 = Delegate17.smethod_0();
		this.columnHeader_0 = Delegate18.smethod_0();
		this.label_0 = Delegate7.smethod_0();
		this.button_0 = Delegate11.smethod_0();
		this.label_1 = Delegate7.smethod_0();
		this.treeView_0 = Delegate19.smethod_0();
		Delegate73.smethod_0(this.groupBox_0);
		Delegate73.smethod_1(this);
		Delegate92.smethod_0(this.groupBox_0, AnchorStyles.None);
		Delegate75.smethod_0(Delegate74.smethod_0(this.groupBox_0), this.label_3);
		Delegate75.smethod_0(Delegate74.smethod_0(this.groupBox_0), this.label_2);
		Delegate75.smethod_0(Delegate74.smethod_0(this.groupBox_0), this.button_1);
		Delegate75.smethod_0(Delegate74.smethod_0(this.groupBox_0), this.button_2);
		Delegate75.smethod_0(Delegate74.smethod_0(this.groupBox_0), this.listView_0);
		Delegate75.smethod_0(Delegate74.smethod_0(this.groupBox_0), this.label_0);
		Delegate75.smethod_0(Delegate74.smethod_0(this.groupBox_0), this.button_0);
		Delegate75.smethod_0(Delegate74.smethod_0(this.groupBox_0), this.label_1);
		Delegate75.smethod_0(Delegate74.smethod_0(this.groupBox_0), this.treeView_0);
		Delegate76.smethod_0(this.groupBox_0, Delegate12.smethod_0(7, 7));
		Delegate77.smethod_0(this.groupBox_0, "grpClassification");
		Delegate78.smethod_0(this.groupBox_0, RightToLeft.No);
		Delegate79.smethod_0(this.groupBox_0, Delegate13.smethod_0(550, 343));
		Delegate80.smethod_0(this.groupBox_0, 0);
		Delegate81.smethod_0(this.groupBox_0, false);
		Delegate64.smethod_0(this.groupBox_0, "Classification groups");
		Delegate82.smethod_0(this.label_3, true);
		Delegate94.smethod_0(this.label_3, Delegate93.smethod_0());
		Delegate76.smethod_0(this.label_3, Delegate12.smethod_0(6, 59));
		Delegate77.smethod_0(this.label_3, "lblAvailableGroups");
		Delegate79.smethod_0(this.label_3, Delegate13.smethod_0(85, 13));
		Delegate80.smethod_0(this.label_3, 8);
		Delegate64.smethod_0(this.label_3, "Available groups");
		Delegate82.smethod_0(this.label_2, true);
		Delegate94.smethod_0(this.label_2, Delegate93.smethod_0());
		Delegate76.smethod_0(this.label_2, Delegate12.smethod_0(288, 59));
		Delegate77.smethod_0(this.label_2, "lblSelectedGroups");
		Delegate79.smethod_0(this.label_2, Delegate13.smethod_0(144, 13));
		Delegate80.smethod_0(this.label_2, 7);
		Delegate64.smethod_0(this.label_2, "Selected groups (0 Symbols) ");
		Delegate76.smethod_0(this.button_1, Delegate12.smethod_0(260, 192));
		Delegate77.smethod_0(this.button_1, "btnRemoveGroup");
		Delegate79.smethod_0(this.button_1, Delegate13.smethod_0(25, 25));
		Delegate80.smethod_0(this.button_1, 6);
		Delegate64.smethod_0(this.button_1, "<");
		Delegate85.smethod_0(this.button_1, true);
		Delegate88.smethod_0(this.button_1, new EventHandler(this.method_16));
		Delegate76.smethod_0(this.button_2, Delegate12.smethod_0(260, 161));
		Delegate77.smethod_0(this.button_2, "btnAddGroup");
		Delegate79.smethod_0(this.button_2, Delegate13.smethod_0(25, 25));
		Delegate80.smethod_0(this.button_2, 5);
		Delegate64.smethod_0(this.button_2, ">");
		Delegate85.smethod_0(this.button_2, true);
		Delegate88.smethod_0(this.button_2, new EventHandler(this.method_13));
		Delegate96.smethod_0(Delegate95.smethod_0(this.listView_0), new ColumnHeader[]
		{
			this.columnHeader_0
		});
		Delegate97.smethod_0(this.listView_0, ColumnHeaderStyle.None);
		Delegate98.smethod_0(this.listView_0, false);
		Delegate76.smethod_0(this.listView_0, Delegate12.smethod_0(291, 75));
		Delegate77.smethod_0(this.listView_0, "lvSelected");
		Delegate79.smethod_0(this.listView_0, Delegate13.smethod_0(253, 230));
		Delegate80.smethod_0(this.listView_0, 4);
		Delegate98.smethod_1(this.listView_0, false);
		Delegate99.smethod_0(this.listView_0, View.Details);
		Delegate100.smethod_0(this.listView_0, new EventHandler(this.method_15));
		Delegate88.smethod_1(this.listView_0, new EventHandler(this.method_20));
		Delegate101.smethod_0(this.columnHeader_0, "Group");
		Delegate102.smethod_0(this.columnHeader_0, 221);
		Delegate82.smethod_0(this.label_0, true);
		Delegate76.smethod_0(this.label_0, Delegate12.smethod_0(170, 318));
		Delegate77.smethod_0(this.label_0, "lblUpdateClassification");
		Delegate79.smethod_0(this.label_0, Delegate13.smethod_0(72, 13));
		Delegate80.smethod_0(this.label_0, 3);
		Delegate64.smethod_0(this.label_0, "Last updated:");
		Delegate103.smethod_0(this.label_0, new PaintEventHandler(this.method_10));
		Delegate76.smethod_0(this.button_0, Delegate12.smethod_0(6, 312));
		Delegate77.smethod_0(this.button_0, "btnUpdateClassification");
		Delegate79.smethod_0(this.button_0, Delegate13.smethod_0(158, 24));
		Delegate80.smethod_0(this.button_0, 2);
		Delegate64.smethod_0(this.button_0, "Refresh classification");
		Delegate85.smethod_0(this.button_0, true);
		Delegate88.smethod_0(this.button_0, new EventHandler(this.method_11));
		Delegate82.smethod_0(this.label_1, true);
		Delegate76.smethod_0(this.label_1, Delegate12.smethod_0(6, 19));
		Delegate104.smethod_0(this.label_1, Delegate13.smethod_0(550, 40));
		Delegate77.smethod_0(this.label_1, "lblClassification");
		Delegate79.smethod_0(this.label_1, Delegate13.smethod_0(529, 26));
		Delegate80.smethod_0(this.label_1, 1);
		Delegate64.smethod_0(this.label_1, "Select one or more Classification Groups below and add them to the Selected list to the right. Your DataSet will contain all of the Symbols in the selected Groups.");
		Delegate105.smethod_0(this.treeView_0, false);
		Delegate76.smethod_0(this.treeView_0, Delegate12.smethod_0(6, 75));
		Delegate77.smethod_0(this.treeView_0, "treeClassification");
		Delegate79.smethod_0(this.treeView_0, Delegate13.smethod_0(248, 230));
		Delegate80.smethod_0(this.treeView_0, 0);
		Delegate88.smethod_1(this.treeView_0, new EventHandler(this.method_17));
		Delegate106.smethod_0(this.treeView_0, new TreeViewEventHandler(this.method_12));
		Delegate90.smethod_0(this, Delegate16.smethod_0(6f, 13f));
		Delegate91.smethod_0(this, AutoScaleMode.Font);
		Delegate75.smethod_0(Delegate74.smethod_1(this), this.groupBox_0);
		Delegate77.smethod_1(this, "YahooWizardPageClassification");
		Delegate78.smethod_0(this, RightToLeft.Yes);
		Delegate79.smethod_1(this, Delegate13.smethod_0(560, 353));
		Delegate107.smethod_0(this, new EventHandler(this.method_6));
		Delegate67.smethod_2(this.groupBox_0, false);
		Delegate73.smethod_2(this.groupBox_0);
		Delegate67.smethod_3(this, false);
	}
	public Control1()
	{
		this.method_0();
	}
	private void method_1()
	{
		this.method_4();
		Delegate64.smethod_0(this.label_0, "Please wait, updating Classification...");
		Delegate116.smethod_0();
		MsnStaticProvider.ClassificationFile.method_2(true);
	}
	private void method_2(ClassificationGroup classificationGroup_1, TreeNodeCollection treeNodeCollection_0)
	{
		for (int i = 0; i < classificationGroup_1.Groups.Count; i++)
		{
			TreeNode object_ = Delegate117.smethod_0(treeNodeCollection_0, classificationGroup_1.Groups[i].Name);
			Delegate118.smethod_0(object_, classificationGroup_1.Groups[i]);
			if (Delegate51.smethod_0(classificationGroup_1.Groups[i].Type, "Symbols"))
			{
				Delegate121.smethod_0(object_, Delegate120.smethod_0(Delegate119.smethod_0(object_), " (", classificationGroup_1.Groups[i].SymbolsCount, ")"));
			}
			this.method_2(classificationGroup_1.Groups[i], Delegate122.smethod_0(object_));
		}
	}
	private void method_3()
	{
		if (!MsnStaticProvider.ClassificationFile.Boolean_0)
		{
			Delegate64.smethod_0(this.label_0, "Classification created:");
			this.method_5();
			return;
		}
		this.method_4();
		try
		{
			Delegate64.smethod_0(this.label_0, "Please wait, processing the Classification data...");
			Delegate116.smethod_0();
			Delegate124.smethod_0(Delegate123.smethod_0(this.treeView_0));
			Delegate125.smethod_0(Delegate108.smethod_0(this.listView_0));
			this.classificationGroup_0 = MsnStaticProvider.ClassificationFile.ClassificationGroup_0;
			if (this.classificationGroup_0 != null)
			{
				this.method_2(this.classificationGroup_0, Delegate123.smethod_0(this.treeView_0));
				if (this.classificationGroup_0 != null)
				{
					Delegate64.smethod_0(this.label_3, Delegate126.smethod_0("Available groups ({0} Symbols)", this.classificationGroup_0.SymbolsCount));
				}
			}
		}
		catch (Exception object_)
		{
			Delegate128.smethod_0(this, Delegate127.smethod_0(object_), "Error processing Classification data", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
		finally
		{
			this.method_5();
		}
		Delegate64.smethod_0(this.label_0, Delegate126.smethod_0("Classification created: {0}", this.classificationGroup_0.Update.ToLocalTime()));
		Delegate116.smethod_0();
	}
	private void method_4()
	{
		this.bool_0 = true;
		Delegate130.smethod_0(this, Delegate129.smethod_0());
	}
	private void method_5()
	{
		this.bool_0 = false;
		Delegate130.smethod_0(this, Delegate129.smethod_1());
	}
	private void method_6(object sender, EventArgs e)
	{
		Delegate116.smethod_0();
		if (!MsnStaticProvider.ClassificationFile.Boolean_0)
		{
			if (Delegate131.smethod_0("Classification data file was not found.\n\rDownload it now?", "Classification not found", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
			{
				this.method_1();
				return;
			}
		}
		else
		{
			this.method_3();
		}
	}
	public void method_7(string string_1)
	{
		MsnStaticProvider.ClassificationFile.Event_1 += new AsyncCompletedEventHandler(this.method_9);
		MsnStaticProvider.ClassificationFile.Event_0 += new DownloadProgressChangedEventHandler(this.method_8);
		this.string_0 = string_1;
		this.font_0 = Delegate132.smethod_0(this.label_0);
		this.font_1 = Delegate21.smethod_0(Delegate132.smethod_0(this.label_0), FontStyle.Bold);
		Delegate125.smethod_0(Delegate108.smethod_0(this.listView_0));
		Delegate133.smethod_0(this.treeView_0);
		Delegate67.smethod_0(this.button_2, false);
		Delegate67.smethod_0(this.button_1, false);
		this.method_14();
	}
	private void method_8(object sender, DownloadProgressChangedEventArgs e)
	{
		Delegate64.smethod_0(this.label_0, Delegate126.smethod_0("Download progress {0}%", Delegate134.smethod_0(e)));
		Delegate116.smethod_0();
	}
	private void method_9(object sender, AsyncCompletedEventArgs e)
	{
		Delegate116.smethod_0();
		if (Delegate135.smethod_0(e) != null)
		{
			Delegate128.smethod_0(this, Delegate114.smethod_0("Error while downloading Classification file: ", Delegate127.smethod_0(Delegate135.smethod_0(e))), "File download error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
		this.method_3();
	}
	private void method_10(object sender, PaintEventArgs e)
	{
		Delegate89.smethod_0(this.label_0, this.bool_0 ? this.font_1 : this.font_0);
	}
	private void method_11(object sender, EventArgs e)
	{
		this.method_1();
	}
	private void method_12(object sender, TreeViewEventArgs e)
	{
		if (Delegate51.smethod_0((Delegate137.smethod_0(Delegate136.smethod_0(e)) as ClassificationGroup).Type, "Symbols"))
		{
			Delegate67.smethod_0(this.button_2, true);
			return;
		}
		Delegate67.smethod_0(this.button_2, false);
	}
	private void method_13(object sender, EventArgs e)
	{
		this.method_18();
	}
	private void method_14()
	{
		this.int_0 = 0;
		IEnumerator enumerator = Delegate109.smethod_0(Delegate108.smethod_0(this.listView_0));
		try
		{
			while (enumerator.MoveNext())
			{
				ListViewItem object_ = (ListViewItem)enumerator.Current;
				this.int_0 += Delegate138.smethod_0((Delegate110.smethod_0(object_) as ClassificationGroup).SymbolsCount);
			}
		}
		finally
		{
			IDisposable disposable = enumerator as IDisposable;
			if (disposable != null)
			{
				disposable.Dispose();
			}
		}
		Delegate64.smethod_0(this.label_2, Delegate126.smethod_0("Selected groups ({0} Symbols)", this.int_0));
	}
	private void method_15(object sender, EventArgs e)
	{
		Delegate67.smethod_0(this.button_1, true);
	}
	private void method_16(object sender, EventArgs e)
	{
		this.method_19();
	}
	private void method_17(object sender, EventArgs e)
	{
		this.method_18();
	}
	private void method_18()
	{
		TreeNode treeNode = Delegate139.smethod_0(this.treeView_0);
		if (treeNode != null)
		{
			ClassificationGroup classificationGroup = (ClassificationGroup)Delegate137.smethod_0(treeNode);
			if (Delegate51.smethod_0(classificationGroup.Type, "Symbols"))
			{
				IEnumerator enumerator = Delegate109.smethod_0(Delegate108.smethod_0(this.listView_0));
				try
				{
					while (enumerator.MoveNext())
					{
						ListViewItem object_ = (ListViewItem)enumerator.Current;
						if (Delegate110.smethod_0(object_) == classificationGroup)
						{
							return;
						}
					}
				}
				finally
				{
					IDisposable disposable = enumerator as IDisposable;
					if (disposable != null)
					{
						disposable.Dispose();
					}
				}
				Delegate142.smethod_0(Delegate141.smethod_0(Delegate108.smethod_0(this.listView_0), Delegate140.smethod_0("{0} ({1})", classificationGroup.Name, classificationGroup.SymbolsCount)), classificationGroup);
				Delegate145.smethod_0(Delegate144.smethod_0(Delegate108.smethod_0(this.listView_0), Delegate143.smethod_0(Delegate108.smethod_0(this.listView_0)) - 1));
				this.method_14();
			}
		}
	}
	private void method_19()
	{
		if (Delegate146.smethod_0(this.listView_0) != null)
		{
			IEnumerator enumerator = Delegate147.smethod_0(Delegate146.smethod_0(this.listView_0));
			try
			{
				while (enumerator.MoveNext())
				{
					ListViewItem object_ = (ListViewItem)enumerator.Current;
					Delegate145.smethod_1(object_);
				}
			}
			finally
			{
				IDisposable disposable = enumerator as IDisposable;
				if (disposable != null)
				{
					disposable.Dispose();
				}
			}
		}
		Delegate67.smethod_0(this.button_1, false);
		this.method_14();
	}
	private void method_20(object sender, EventArgs e)
	{
		this.method_19();
	}
}
