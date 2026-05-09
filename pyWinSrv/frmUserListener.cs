using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pyWinSrv
{
    public partial class frmUserListener : Form
    {
        public frmUserListener()
        {
            InitializeComponent();
        }

        void fnSetup()
        {
            textBox1.Text = "5000";
        }

        private void frmUserListener_Load(object sender, EventArgs e)
        {
            fnSetup();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
