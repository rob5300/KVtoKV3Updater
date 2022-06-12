using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KVSurfaceUpdater
{
    public partial class ConversionForm : UserControl
    {
        public string Prefix { get; private set; }

        private bool showPrefixInput;

        public ConversionForm(bool showPrefixInput)
        {
            this.showPrefixInput = showPrefixInput;
            InitializeComponent();
        }

        private void ConversionForm_Load(object sender, EventArgs e)
        {
            newFilePrefixPanel.Enabled = showPrefixInput;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            Prefix = filePrefixBox.Text;
        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
