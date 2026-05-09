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
        private clsSqlite m_sqlConn;
        private string m_szName;

        public frmEditListener(clsSqlite sqlConn, string szName)
        {
            InitializeComponent();

            m_sqlConn = sqlConn;
            m_szName = szName;
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
            {
                var listener = m_sqlConn.fnGetListenerWithName(m_szName);
                textBox1.Text = listener.szName;
                numericUpDown1.Value = listener.nPort;
                
                for (int i = 0; i < comboBox1.Items.Count; i++)
                    if (string.Equals(comboBox1.Items[i], listener.srvProtocol.ToString()))
                        comboBox1.SelectedIndex = i;
            }
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

            if (m_sqlConn.fnbSaveListener(szName, nPort, sp))
            {
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                MessageBox.Show("Failed to save listener!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
