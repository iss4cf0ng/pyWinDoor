namespace pyWinDoor
{
    public partial class frmMain : Form
    {

        public frmMain()
        {
            InitializeComponent();
        }

        void fnSetup()
        {

        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            fnSetup();
        }

        //Build
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            frmBuilder f = new frmBuilder();

            f.Show();
        }
        //Setting
        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            frmSetting f = new frmSetting();

            f.ShowDialog();
        }
        //About
        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            frmAbout f = new frmAbout();

            f.Show();
        }

        //Info
        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView1.SelectedItems)
            {

            }
        }
        //File
        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {

        }
        //Task
        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {

        }
        //Service
        private void toolStripMenuItem7_Click(object sender, EventArgs e)
        {

        }
        //Shell
        private void toolStripMenuItem8_Click(object sender, EventArgs e)
        {

        }
        //Registry
        private void toolStripMenuItem9_Click(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItem10_Click(object sender, EventArgs e)
        {

        }
    }
}
