using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pyWinDoor
{
    public partial class frmFileMgr : Form
    {
        /// <summary>
        /// pyWinDoor File Manager
        /// 
        /// Todo:
        ///     File viewer
        ///     Editor
        ///     Copy
        ///     Move
        ///     Delete
        ///     Upload
        ///     Download
        /// </summary>

        public frmFileMgr()
        {
            InitializeComponent();
        }

        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {

        }

        void fnSetup()
        {

        }

        private void frmFileMgr_Load(object sender, EventArgs e)
        {
            fnSetup();
        }
    }
}
