using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace pyWinSrv
{
    internal class clsHttpListener : clsListener
    {
        private HttpListener m_HttpListener;

        public clsHttpListener(string szName, int nPort)
        {
            m_szName = szName;
            m_nPort = nPort;
            m_srvProtocol = SrvProtocol.HTTP;

            m_HttpListener = new HttpListener();
        }

        public override void fnStart()
        {
            
        }

        public override void fnStop()
        {

        }
    }
}
