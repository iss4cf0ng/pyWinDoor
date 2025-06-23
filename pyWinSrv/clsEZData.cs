using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pyWinSrv
{
    internal class clsEZData
    {
        public clsEZData()
        {
            
        }

        public static string fnStrEncToB64(string szData) => Convert.ToBase64String(Encoding.UTF8.GetBytes(szData));
        public static string fnB64DecToStr(string szData) => Encoding.UTF8.GetString(Convert.FromBase64String(szData));

        public static List<string> fnlsStrEncToB64(List<string> lsData) => lsData.Select(x => fnStrEncToB64(x)).ToList();
        public static List<string> fnlsB64DecToStr(List<string> lsData) => lsData.Select(x => fnB64DecToStr(x)).ToList();

        public static string fnlpListToSendData(List<string> lsData) => string.Join(",", fnlsStrEncToB64(lsData));
        public static List<string> fnlsRecvDataToList(string szData) => fnlsB64DecToStr(szData.Split(",").ToList());
    }
}
