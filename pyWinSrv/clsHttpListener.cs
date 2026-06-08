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

        /// <summary>
        /// HTTP listener
        /// </summary>
        /// <param name="szName"></param>
        /// <param name="nPort"></param>
        public clsHttpListener(string szName, int nPort)
        {
            m_szName = szName;
            m_nPort = nPort;
            m_srvProtocol = SrvProtocol.HTTP;

            m_HttpListener = new HttpListener();
            m_HttpListener.Prefixes.Add($"http://localhost:{m_nPort}/");
        }

        public override void fnStart()
        {
            m_HttpListener.Start();
            m_HttpListener.BeginGetContext(new AsyncCallback(fnGetContext), m_HttpListener);
        }

        public void fnGetContext(IAsyncResult ar)
        {
            try
            {
                HttpListenerContext context = m_HttpListener.EndGetContext(ar);
                m_HttpListener.BeginGetContext(new AsyncCallback(fnGetContext), m_HttpListener);

                HttpListenerRequest req = context.Request;
                string szReqContent = string.Empty;
                using (var reader = new StreamReader(req.InputStream, req.ContentEncoding))
                {
                    szReqContent = reader.ReadToEnd();
                }

                MessageBox.Show(szReqContent);

                HttpListenerResponse resp = context.Response;

                string szRespString = "<h1>hola</h1>";
                byte[] abBuffer = Encoding.UTF8.GetBytes(szRespString);
                resp.ContentLength64 = abBuffer.Length;
                resp.ContentType = "text/html";

                resp.OutputStream.Write(abBuffer, 0, abBuffer.Length);
                resp.OutputStream.Close();
            }
            catch (Exception ex)
            {

            }
        }

        public override void fnStop()
        {
            m_HttpListener.Stop();
        }
    }
}
