using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pyWinSrv
{
    public enum SrvProtocol
    {
        TCP,
        DNS,
        HTTP,
        SMB,
        SMBv2,
    }

    public enum LogsType
    {
        System,
        Error,
    }

    public class clsGlobal
    {

    }

    #region typedef

    public struct tdfListener
    {
        public string szName;
        public int nPort;
        public SrvProtocol srvProtocol;
    }

    #endregion
}
