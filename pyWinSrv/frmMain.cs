namespace pyWinSrv
{
    public partial class frmMain : Form
    {
        public clsSqlite m_sqlConn;
        private Dictionary<string, clsListener> m_dicListener = new Dictionary<string, clsListener>();

        public frmMain()
        {
            InitializeComponent();
        }

        #region pyWinSrv



        #endregion
        #region Listener

        void fnLoadListener()
        {
            listView1.Items.Clear();
            var lsListener = m_sqlConn.fnlsGetAllListener();
            foreach (var l in lsListener)
            {
                ListViewItem item = new ListViewItem(l.szName);
                item.SubItems.Add(l.nPort.ToString());
                item.SubItems.Add(l.srvProtocol.ToString());

                listView1.Items.Add(item);
            }
        }

        void fnStartListener()
        {
            //Load listener into dictionary.
            m_dicListener.Clear();
            var lsListener = m_sqlConn.fnlsGetAllListener();
            foreach (var l in lsListener)
            {
                switch (l.srvProtocol)
                {
                    case SrvProtocol.TCP:
                        m_dicListener[l.szName] = new clsTcpListener(l.szName, l.nPort);
                        break;
                    case SrvProtocol.DNS:
                        m_dicListener[l.szName] = new clsUdpListener(l.szName, l.nPort);
                        break;
                    case SrvProtocol.HTTP:
                        m_dicListener[l.szName] = new clsHttpListener(l.szName, l.nPort);
                        break;
                }
            }

            //Start listening
            foreach (string szName in m_dicListener.Keys)
            {
                var l = m_dicListener[szName];

            }
        }
        void fnStopListener()
        {

        }
        void fnRestartListener()
        {
            //todo: stop all listener and start it again.
        }
        void fnExit()
        {
            //todo: stop all listener and exit application.
        }

        #endregion

        /// <summary>
        /// Application initialization.
        /// </summary>
        /// <param name="szDbFileName"></param>
        void fnSetup(string szDbFileName = "db.sqlite")
        {
            //Class
            m_sqlConn = new clsSqlite(szDbFileName);
            if (!m_sqlConn.m_bIsOpen)
            {
                MessageBox.Show("Failed to initialize database.", "fnSetup()", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            m_dicListener.Clear();

            //Controls
            listView1.FullRowSelect = true;

            fnLoadListener();

            textBox1.Text = m_sqlConn.m_szDbFileName;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            fnSetup();
        }

        //New
        private void button1_Click(object sender, EventArgs e)
        {
            frmEditListener f = new frmEditListener();
            f.m_sqlConn = m_sqlConn;

            f.ShowDialog();
        }
        //Edit
        private void button2_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
                return;

            ListViewItem item = listView1.SelectedItems[0];

            frmEditListener f = new frmEditListener();
            f.m_szName = item.Text;
            f.m_sqlConn = m_sqlConn;

            f.ShowDialog();
        }
        //Delete
        private void button3_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
                return;

            ListViewItem item = listView1.SelectedItems[0];

            DialogResult dr = MessageBox.Show("Delete listener: " + item.Text, "Sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                if (m_sqlConn.fnbDeleteListener(item.Text))
                {
                    fnLoadListener();
                    MessageBox.Show("Delete listener successfully.", "OK", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Failed to delete listener.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        //Open database file.
        private void button8_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                if (m_sqlConn != null && m_sqlConn.m_bIsOpen)
                {
                    m_sqlConn.fnCloseDB();
                    m_sqlConn.Dispose();
                }

                fnSetup(ofd.FileName);
            }
        }
        //Open logs file.
        private void button9_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        //Start
        private void button4_Click(object sender, EventArgs e)
        {
            new Thread(() => fnStartListener()).Start();
        }
        //Stop
        private void button5_Click(object sender, EventArgs e)
        {
            new Thread(() => fnStopListener()).Start();
        }

        //Restart
        private void button6_Click(object sender, EventArgs e)
        {
            
        }
        //Exit
        private void button7_Click(object sender, EventArgs e)
        {

        }
    }
}
