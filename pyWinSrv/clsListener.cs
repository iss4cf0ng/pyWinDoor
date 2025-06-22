using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pyWinSrv
{
    internal class clsListener
    {
        public string m_szName { get; set; }
        public int m_nPort { get; set; }
        public SrvProtocol m_srvProtocol { get; set; }
        public string m_szDescription { get; set; }

        public clsListener()
        {

        }

        public virtual void fnStart()
        {

        }

        public virtual void fnStop()
        {

        }
    }
}
