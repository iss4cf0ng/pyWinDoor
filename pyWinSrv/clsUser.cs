using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace pyWinSrv
{
    internal class clsUser
    {
        public Socket m_sktUser;

        public clsUser(Socket sktUser)
        {
            m_sktUser = sktUser;
        }

        public clsUser()
        {

        }
    }
}
