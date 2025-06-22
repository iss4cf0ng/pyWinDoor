using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace pyWinSrv
{
    internal class clsUdpListener : clsListener
    {
        private UdpClient m_UdpClient;
        private bool m_IsListening;

        public clsUdpListener(string szName, int nPort)
        {
            m_szName = szName;
            m_nPort = nPort;
            m_srvProtocol = SrvProtocol.DNS;

            m_UdpClient = new UdpClient(nPort);
        }

        public void fnStart()
        {
            m_IsListening = true;
            fnDnsSrvReceive();
        }

        public void fnDnsSrvReceive()
        {
            m_UdpClient.BeginReceive(new AsyncCallback(fnDnsBeginReceiveCallback), m_UdpClient);

            if (m_IsListening)
                fnDnsSrvReceive();

            //here is the test
        }

        public void fnStop()
        {
            m_IsListening = false;
        }

        public void fnDnsBeginReceiveCallback(IAsyncResult ar)
        {
            UdpClient udpClient = (UdpClient)ar.AsyncState;
            if (udpClient == null)
                return;

            IPEndPoint iepRemote = new IPEndPoint(IPAddress.Any, 0);
            byte[] abRecvData = udpClient.Receive(ref iepRemote);

        }

        private string fnlpParseQueryName(byte[] abData, int nOffset)
        {
            StringBuilder sb = new StringBuilder();
            while (abData[nOffset] != 0)
            {
                int nLen = abData[nOffset++];
                for (int i = 0; i < nLen; i++)
                    sb.Append((char)abData[nOffset++]);

                sb.Append('.');
            }

            return sb.ToString().TrimEnd('.');
        }

        private byte[] fnabBuildTxtResponse(byte[] abRequest, string szTxtData)
        {
            //Variables
            byte[] abResponse = new byte[512];
            int nTransactionId = (abResponse[0] << 8) | abResponse[1];
            int nIndex = 0;

            //DNS header
            abResponse[nIndex++] = abResponse[0]; //Transaction ID.
            abResponse[nIndex++] = abResponse[1];
            abResponse[nIndex++] = 0x81; //Flags
            abResponse[nIndex++] = 0x80;
            abResponse[nIndex++] = 0x00; abResponse[nIndex++] = 0x01; //Question
            abResponse[nIndex++] = 0x00; abResponse[nIndex++] = 0x01; //Answers
            abResponse[nIndex++] = 0x00; abResponse[nIndex++] = 0x00; //Authority RRs
            abResponse[nIndex++] = 0x00; abResponse[nIndex++] = 0x00; //Additional RRs

            //Question
            int nQnameEnd = 12;
            while (abRequest[nQnameEnd] != 0)
                nQnameEnd++;
            nQnameEnd += 5; //Include null and QTYPE+QCLASS
            Array.Copy(abRequest, 12, abResponse, nIndex, nQnameEnd - 12);
            nIndex += nQnameEnd - 12;

            //Answer
            abResponse[nIndex++] = 0xC0; abResponse[nIndex++] = 0x0C; //Pointer to domain name.
            abResponse[nIndex++] = 0x00; abResponse[nIndex++] = 0x10; //Type TXT
            abResponse[nIndex++] = 0x00; abResponse[nIndex++] = 0x01; //Class IN
            abResponse[nIndex++] = 0x00; abResponse[nIndex++] = 0x00; abResponse[nIndex++] = 0x00; abResponse[nIndex++] = 0x3C; //TTL 60s

            byte[] abTXT = Encoding.ASCII.GetBytes(szTxtData);
            abResponse[nIndex++] = 0x00; abResponse[nIndex++] = (byte)(abTXT.Length + 1); //RDLENGTH
            abResponse[nIndex++] = (byte)abTXT.Length;
            Array.Copy(abTXT, 0, abResponse, nIndex, abTXT.Length);
            nIndex += abTXT.Length;

            Array.Resize(ref abResponse, nIndex);

            return abResponse;
        }
    }
}
