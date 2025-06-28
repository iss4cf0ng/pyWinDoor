using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace pyWinSrv
{
    internal class clsVictim
    {
        public Socket m_sktVictim;

        public string m_szOnlineID;

        public clsVictim(Socket socket)
        {
            m_sktVictim = socket;
        }

        public clsVictim()
        {

        }
    }
}
