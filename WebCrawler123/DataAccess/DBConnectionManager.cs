using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using MySql.Data.MySqlClient;


namespace DataAccess
{
    /// <summary>
    /// The objective of this class is to manage DB connection state. Used to open and close DB connection.
    /// </summary>
    public class DBConnectionManager
    {
        /// <summary>
        /// returns connection string.
        /// </summary>
        /// <returns></returns>
        public string GetConnectionString()
        {
            //return System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connString"].ConnectionString;

            //System.Configuration.Configuration rootWebConfig1 =
            //    System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration(null);
            System.Configuration.Configuration rootWebConfig = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("/MyWebSiteRoot");
            System.Configuration.ConnectionStringSettings connString;
            if (rootWebConfig.ConnectionStrings.ConnectionStrings.Count > 0)
            {
                foreach (ConnectionStringSettings conn in rootWebConfig.ConnectionStrings.ConnectionStrings)
                {
                    if (conn.Name == "DefaultConnection")
                    {
                        connString = conn;
                        return connString.ConnectionString.ToString();
                    }
                }
                //connString = rootWebConfig.ConnectionStrings.ConnectionStrings[2].Name;


            }
            return "";

        }

        /// <summary>
        /// Opens SQL Connection for specified connection string
        /// </summary>
        /// <param name="connString">Connection string</param>
        /// <returns>Opened SQL Connection</returns>
        public MySqlConnection OpenSqlConnection(string connString)
        {
            MySqlConnection sqlConn = null;
            try
            {
                sqlConn = new MySqlConnection(connString);
                sqlConn.Open();
            }
            catch (Exception ex)
            {
                //throw ex;
            }
            return sqlConn;
        }

        /// <summary>
        /// Closes SQL Connection
        /// </summary>
        /// <param name="pConn">SQL Connection object</param>
        public void CloseSqlConnection(ref MySqlConnection pConn)
        {
            if (pConn != null && pConn.State == ConnectionState.Open)
            {
                pConn.Close();
            }
        }
    }
}
