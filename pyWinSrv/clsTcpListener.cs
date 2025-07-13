using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace pyWinSrv
{
    internal class clsTcpListener : clsListener
    {
        private TcpListener m_tcpListener;

        public clsTcpListener(string szName, int nPort)
        {
            m_szName = szName;  
            m_nPort = nPort;
            m_srvProtocol = SrvProtocol.TCP;

            m_tcpListener = new TcpListener(IPAddress.Any, nPort);
        }

        public override void fnStart()
        {
            m_tcpListener.Start();
            m_tcpListener.BeginAcceptSocket(new AsyncCallback(fnTcpListenerAcceptCallback), m_tcpListener);
        }

        private void fnTcpListenerAcceptCallback(IAsyncResult ar)
        {
            if (ar.AsyncState == null)
                return;

            TcpListener tcpListener = (TcpListener)ar.AsyncState;
            if (tcpListener == null)
                return;

            try
            {
                Socket sktClnt = tcpListener.EndAcceptSocket(ar);
            }
            catch (Exception ex)
            {

            }
        }

        public override void fnStop()
        {
            m_tcpListener.Stop();
        }
    }
}
