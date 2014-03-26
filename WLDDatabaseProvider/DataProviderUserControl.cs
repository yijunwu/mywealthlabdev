using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WLDDatabaseProvider
{
    public partial class DataProviderUserControl : System.Windows.Forms.UserControl
    {
        public DataProviderUserControl()
        {
            InitializeComponent();
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }

        public string getDatabaseName()
        {
            return txtDatabase.Text;
        }

        public string getTradeTableName()
        {
            return txtTable.Text;
        }
        
        public string getSymbolColumn()
        {
            return txtSymbolColumn.Text;
        }

        public string getDateColumnName()
        {
            return txtDateColumn.Text;
        }
        public string getOpenColumnName()
        {
            return txtOpenColumn.Text;
        }
        public string getCloseColumnName()
        {
            return txtCloseColumn.Text;
        }
        public string getHighColumnName()
        {
            return txtHighColumn.Text;
        }
        public string getLowColumnName()
        {
            return txtLowColumn.Text;
        }

        public string getVolumeColumnName()
        {
            return txtVolumeColumn.Text;
        }

        public string getConnectionString()
        {
            return txtConnectionStr.Text;
        }

        public string getQueryStr()
        {
            return txtQuery.Text;
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtDateColumn_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
