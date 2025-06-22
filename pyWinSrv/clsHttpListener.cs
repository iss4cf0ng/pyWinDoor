using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pyWinSrv
{
    internal class clsHttpListener : clsListener
    {
        public clsHttpListener(string szName, int nPort)
        {
            m_szName = szName;
            m_nPort = nPort;
            m_srvProtocol = SrvProtocol.HTTP;
        }
    }
}
