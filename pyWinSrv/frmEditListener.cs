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
    public partial class frmEditListener : Form
    {
        public clsSqlite m_sqlConn;
        public string m_szName;

        public frmEditListener()
        {
            InitializeComponent();
        }

        void fnSetup()
        {
            comboBox1.Items.AddRange(new object[]
            {
                "TCP",
                "HTTP",
                "DNS",
            });
            comboBox1.SelectedIndex = 0;

            if (!string.IsNullOrEmpty(m_szName))
                textBox1.Text = m_szName;


        }

        private void frmEditListener_Load(object sender, EventArgs e)
        {
            fnSetup();
        }

        //Save
        private void button1_Click(object sender, EventArgs e)
        {
            string szName = textBox1.Text;
            int nPort = (int)numericUpDown1.Value;
            SrvProtocol sp = (SrvProtocol)Enum.Parse(typeof(SrvProtocol), comboBox1.Text);

            //Check error
            if (string.IsNullOrEmpty(szName))
            {
                MessageBox.Show("Name cannot be null or empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (sp == null)
            {
                MessageBox.Show("SrvProtocol is null.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!m_sqlConn.fnbSaveListener(szName, nPort, sp))
            {
                Close();
            }
            else
            {
                MessageBox.Show("Failed to save listener!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
