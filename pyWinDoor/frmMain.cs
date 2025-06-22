namespace pyWinDoor
{
    public partial class frmMain : Form
    {

        public frmMain()
        {
            InitializeComponent();
        }

        void setup()
        {

        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            setup();
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
            
        }
        //About
        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {

        }
    }
}
