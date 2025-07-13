using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data;

namespace pyWinSrv
{
    public class clsSqlite : IDisposable
    {
        private bool _disposed = false;

        public string m_szDbFileName { get; set; }
        public string m_szConnString { get; set; }
        public bool m_bIsOpen { get { return m_sqlConn != null && m_sqlConn.State == ConnectionState.Open; } }

        public SQLiteConnection m_sqlConn;

        private Dictionary<string, string[]> m_dicDbTable = new Dictionary<string, string[]>
        {
            {
                "Logs",
                new string[]
                {
                    "LogType",
                    "Owner",
                    "Msg",
                    "Date",
                }
            },
            {
                "Listener",
                new string[]
                {
                    "Name",
                    "Port",
                    "Protocol"
                }
            },
            {
                "Victim",
                new string[]
                {
                    "OnlineID",
                    "Dir",
                    "OS",
                    "KLF",
                    "PD",
                    "CreateDate",
                    "LastOnlineDate",
                    "Uptime",
                }
            },
            {
                "User",
                new string[]
                {
                    "Username",
                    "Password",
                    "CreateDate",
                }
            },
        };

        #region Events

        public delegate void dgLogsEventHandler(string szMsg);
        public event dgLogsEventHandler NewLogs;
        public delegate void dgSaveListenerEventHandler(string szMsg);
        public event dgSaveListenerEventHandler ListenerSaved;

        #endregion

        public clsSqlite(string szDbFileName)
        {
            m_szDbFileName = szDbFileName;
            m_szConnString = $"Data Source={szDbFileName};Compress=True";

            if (m_sqlConn == null || !File.Exists(m_szDbFileName))
            {
                m_sqlConn = new SQLiteConnection(m_szConnString);
                if (!File.Exists(m_szDbFileName) && !fnbCreateDB())
                {
                    MessageBox.Show("Failed to create database file.", "clsSqlite", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    fnbOpenDB();

                    if (!fnbIsValidDbFile())
                    {
                        MessageBox.Show("Invalid database file: " + szDbFileName, "Invalid Database", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        fnCloseDB();
                    }
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool bDisposing)
        {
            if (!_disposed)
            {
                if (bDisposing)
                {
                    fnCloseDB();
                    m_sqlConn.Dispose();
                }

                _disposed = true;
            }
        }

        ~clsSqlite()
        {
            Dispose(false);
        }

        #region Database

        public bool fnbIsValidDbFile()
        {
            foreach (string szTableName in m_dicDbTable.Keys)
            {
                //Check table.
                string szQuery = $"SELECT name FROM sqlite_master WHERE type='table' AND name='{szTableName}';";
                DataTable dt = fnSqlQuery(szQuery);
                if (dt.Rows.Count == 0)
                    return false;

                //Check columns.
                szQuery = $"PRAGMA table_info({szTableName});";
                dt = fnSqlQuery(szQuery);
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr.ItemArray.Length == 0)
                        return false;

                    string szColName = dr[1].ToString();
                    if (!m_dicDbTable[szTableName].Contains(szColName))
                        return false;
                }
            }

            return true;
        }

        public bool fnbCreateDB()
        {
            if (!fnbOpenDB())
                return false;

            foreach (string szTable in m_dicDbTable.Keys)
                if (!fnbCreateTable(szTable))
                    return false;

            return true;
        }

        public bool fnbOpenDB()
        {
            try
            {
                m_sqlConn.Open();
                return true;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message, "fnbOpenDB()", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public void fnCloseDB()
        {
            if (m_sqlConn != null)
            {
                m_sqlConn.Close();
            }
        }

        public bool fnbCreateTable(string szTableName)
        {
            string szColQuery = string.Join(", ", m_dicDbTable[szTableName].Select(x => $"{x} TEXT").ToArray());
            string szQuery = $"CREATE TABLE {szTableName} ({szColQuery});";

            DataTable dt = fnSqlQuery(szQuery);

            return true;
        }

        public DataTable fnSqlQuery(string szQuery)
        {
            DataTable dt = new DataTable();
            using (var adapter = new SQLiteDataAdapter(szQuery, m_sqlConn))
            {
                adapter.Fill(dt);
                return dt;
            }
        }

        #endregion

        #region pyWinSrv

        public bool fnbListenerExists(string szName)
        {
            string szQuery = $"SELECT 1 FROM \"Listener\" WHERE Name = \"{szName}\";";
            DataTable dt = fnSqlQuery(szQuery);

            if (dt.Rows.Count == 0)
                return false;

            return dt.Rows[0][0].ToString() == "1";
        }

        public bool fnbSaveListener(string szName, int nPort, SrvProtocol srvProtocol)
        {
            string szQuery = string.Empty;

            if (fnbListenerExists(szName))
                szQuery = $"UPDATE Listener SET Port = \"{nPort}\", Protocol = \"{srvProtocol.ToString()}\" WHERE Name = \"{szName}\";";
            else
                szQuery = $"INSERT INTO Listener (Name, Port, Protocol) VALUES (\"{szName}\", \"{nPort}\", \"{srvProtocol.ToString()}\");";

            DataTable dt = fnSqlQuery(szQuery);

            if (!fnbListenerExists(szName))
            {
                MessageBox.Show("Failed to write listener config into database.", "fnSaveListener()", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            //Save listener config successfully.
            ListenerSaved($"Saved listener: {szName}[Port:{nPort}]");

            return true;
        }

        public bool fnbDeleteListener(string szName)
        {
            if (!fnbListenerExists(szName))
            {
                MessageBox.Show("Cannot find listener: " + szName, "fnbDeleteListener()", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            string szQuery = $"DELETE FROM Listener WHERE Name = \"{szName}\";";
            fnSqlQuery(szQuery);

            if (fnbListenerExists(szName))
            {
                MessageBox.Show("Failed to delete listener: " + szName, "fnbDeleteListener()", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        public List<tdfListener> fnlsGetAllListener()
        {
            List<tdfListener> lsResult = new List<tdfListener>();
            string szQuery = $"SELECT * FROM Listener;";
            DataTable dt = fnSqlQuery(szQuery);
            foreach (DataRow dr in dt.Rows)
            {
                string szName = dr[0].ToString();
                int nPort = int.Parse(dr[1].ToString());
                SrvProtocol sp = (SrvProtocol)Enum.Parse(typeof(SrvProtocol), dr[2].ToString());

                tdfListener tdfL = new tdfListener()
                {
                    szName = szName,
                    nPort = nPort,
                    srvProtocol = sp,
                };

                lsResult.Add(tdfL);
            }

            return lsResult;
        }

        #endregion
    }
}
