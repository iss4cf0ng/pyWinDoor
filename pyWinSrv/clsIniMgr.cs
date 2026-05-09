using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace pyWinSrv
{
    internal class clsIniMgr
    {
        private string m_szIniFileName { get; set; }

        public clsIniMgr(string szIniFileName)
        {
            m_szIniFileName = szIniFileName;
        }

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        private static extern long WritePrivateProfileString(string szSection, string szKey, string szValue, string szFilePath);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        private static extern int GetPrivateProfileString(string szSection, string szKey, string szDefault, StringBuilder sbRetVal, int nSize, string szFilePath);

        public string fnRead(string szSection, string szKey, string szDefault = "")
        {
            StringBuilder sb = new StringBuilder(255);
            GetPrivateProfileString(szSection, szKey, szDefault, sb, sb.Length, m_szIniFileName);

            return sb.ToString();
        }

        public void fnWrite(string szSection, string szKey, string szValue)
        {
            WritePrivateProfileString(szSection, szKey, szValue, m_szIniFileName);
        }
    }
}
