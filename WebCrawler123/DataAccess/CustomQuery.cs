using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
//using System.Data.OracleClient;
using System.Collections;
using MySql.Data.MySqlClient;
namespace DataAccess
{
    /// <summary>
    /// This class handles Database Connectivity. 
    /// It facilitates Select, Insert, Update, Delete statements execution.
    /// </summary>
    public class CustomQuery
    {
        #region "Data Members"
        public DataAccess.DBConnectionManager _ConnectionManager;
        #endregion

        #region "Constructors"
        /// <summary>
        /// Class Construtor
        /// </summary>
        public CustomQuery()
        {
            _ConnectionManager = new DBConnectionManager();
        }
        #endregion

        #region "Private Methods"
        /// <summary>
        /// This method is responsible for executing the Select Query on a desired SQL Server Database.
        /// </summary>
        /// <param name="pSQL">The query to be executed</param>
        /// <param name="pConnString">The connection string of the DB to be accessed</param>
        /// <returns>Datatable containing resultset</returns>
        private DataTable _ExecuteQuery_SQL(string pSQL, string pConnString)
        {
            DataTable lDataTable = null;
            lDataTable = new DataTable();

            MySqlConnection lSQLConn = _ConnectionManager.OpenSqlConnection(pConnString);
            MySqlCommand sqlCmd = new MySqlCommand();
            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.CommandText = pSQL;
            sqlCmd.Connection = lSQLConn;
            sqlCmd.CommandTimeout = 1200;
            MySqlDataAdapter lSqlDataAdopter = new MySqlDataAdapter(sqlCmd);
            // -- Changing DropDown -- Shahid Rasool -- Start 
            try
            {
                lSqlDataAdopter.Fill(lDataTable);
                
            }
            catch (Exception e)
            {   

            }
            // -- Changing DropDown -- Shahid Rasool -- End
            _ConnectionManager.CloseSqlConnection(ref lSQLConn);
            return lDataTable;
        }
        
        /// <summary>
        /// This method is responsible for executing the SQL Select Command on a desired SQL Server Database
        /// </summary>
        /// <param name="pSQLCommand"> The SQL Select Command to be executed </param>
        /// <returns> Datatable containing the resultset </returns>
        private DataTable _ExecuteQuery_SQL(MySqlCommand pSQLCommand)
        {
            DataTable lDataTable = null;
            lDataTable = new DataTable();
            pSQLCommand.CommandTimeout = 1200;
            MySqlDataAdapter lSqlDataAdopter = new MySqlDataAdapter(pSQLCommand);
            lSqlDataAdopter.Fill(lDataTable);
            return lDataTable;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pSqlCommand"></param>
        /// <param name="pSQLConn"></param>
        /// <param name="pSQLTrans"></param>
        /// <returns></returns>
        private int _ExecuteNonQuery_SQL(MySqlCommand pSqlCommand)
        {
            int affectedRow = -1;
            try
            {
                affectedRow = pSqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return affectedRow;
        }

        /// <summary>
        /// This method is responsible for executing the SQL Statement on a desired SQL Server Database.
        /// </summary>
        /// <param name="pSQL"> The query to be executed </param>
        /// <param name="pSQLConn"> The SQL Connection object used to access DB </param>
        /// <returns> Number of Affected Rows </returns>
        private int _ExecuteNonQuery_SQL(string pSQL, MySqlConnection pSQLConn)
        {
            int affectedRow = -1;
            MySqlCommand sqlComm = null;
            try
            {

                sqlComm = new MySqlCommand(pSQL, pSQLConn);
                sqlComm.CommandTimeout = 600;

                affectedRow = sqlComm.ExecuteNonQuery();
                // TODO: this might need changing.
                //pSQLConn.Close(); // Added later on
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (null != sqlComm)
                {
                    sqlComm.Dispose();
                }
            }
            return affectedRow;
        }

        /// <summary>
        /// This method is responsible for executing the SQL Statement for an SqlTransaction on a desired SQL Server Database.
        /// </summary>
        /// <param name="pSQL"> The query to be executed </param>
        /// <param name="pSQLConn"> The SQL Connection object used to access DB </param>
        /// <param name="pSQLTrans"> The SQL Transaction object used to maintain the transaction </param>
        /// <returns> Number of Affected Rows </returns>
        private int _ExecuteNonQuery_SQL(string pSQL, MySqlConnection pSQLConn, MySqlTransaction pSQLTrans)
        {
            int affectedRow = -1;
            MySqlCommand sqlComm = null;
            try
            {
                sqlComm = new MySqlCommand(pSQL, pSQLConn, pSQLTrans);
                affectedRow = sqlComm.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (null != sqlComm)
                {
                    sqlComm.Dispose();
                }
            }
            return affectedRow;
        }

        /// <summary>
        /// This method is responsible for executing the SQL Statement for an SqlTransaction on a desired SQL Server Database.
        /// </summary>
        /// <param name="pSqlCommand"> The command to be executed </param>
        /// <param name="pSQLConn"> The SQL Connection object used to access DB </param>
        /// <param name="pSQLTrans"> The SQL Transaction object used to maintain the transaction </param>
        /// <returns> Number of Affected Rows </returns>
        private int _ExecuteNonQuery_SQL(MySqlCommand pSqlCommand, MySqlConnection pSQLConn, MySqlTransaction pSQLTrans)
        {
            int affectedRow = -1;
            try
            {
                pSqlCommand.CommandTimeout = 1200;
                pSqlCommand.Connection = pSQLConn;
                pSqlCommand.Transaction = pSQLTrans;
                affectedRow = pSqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return affectedRow;
        }

        /// <summary>
        /// This method is responsible for executing the SQL Statement on a desired SQL Server Database.
        /// </summary>
        /// <param name="pSQL">The query used to construct Insert, Update, Delete Commands </param>
        /// <param name="pConnString">The connection string of the DB to be accessed</param>
        /// <param name="pDT"> The datatable which contains data to be updated </param>
        /// <returns> Number of Affected Rows </returns>
        private int _ExecuteNonQuery_SQL(string pSQL, string pConnString, DataTable pDT)
        {
            DataTable ldt = new DataTable();
            MySqlDataAdapter lSqlDataAdopter = new MySqlDataAdapter(pSQL, pConnString);

            MySqlCommandBuilder lCmdBuilder = new MySqlCommandBuilder(lSqlDataAdopter);

            lSqlDataAdopter.InsertCommand = lCmdBuilder.GetInsertCommand();
            lSqlDataAdopter.UpdateCommand = lCmdBuilder.GetUpdateCommand();
            lSqlDataAdopter.DeleteCommand = lCmdBuilder.GetDeleteCommand();
            return lSqlDataAdopter.Update(pDT);
        }

        /// <summary>
        /// This method is responsible for executing the SQL Statement on a desired SQL Server Database.
        /// </summary>
        /// <param name="pSQL">The query used to construct Insert, Update, Delete Commands </param>
        /// <param name="pConnString">The connection string of the DB to be accessed</param>
        /// <param name="pDT"> The datatable which contains data to be updated </param>
        /// <returns> Number of Affected Rows </returns>
        private int _ExecuteNonQueryBatch_SQL(string pSQL, string pConnString, DataTable pDT)
        {
            DataTable ldt = new DataTable();
            MySqlDataAdapter lSqlDataAdopter = new MySqlDataAdapter(pSQL, pConnString);

            MySqlCommandBuilder lCmdBuilder = new MySqlCommandBuilder(lSqlDataAdopter);

            lSqlDataAdopter.InsertCommand = lCmdBuilder.GetInsertCommand();
            lSqlDataAdopter.UpdateCommand = lCmdBuilder.GetUpdateCommand();
            lSqlDataAdopter.DeleteCommand = lCmdBuilder.GetDeleteCommand();


            if (lSqlDataAdopter.UpdateCommand != null)
                lSqlDataAdopter.UpdateCommand.UpdatedRowSource = UpdateRowSource.None;
            if (lSqlDataAdopter.InsertCommand != null)
                lSqlDataAdopter.InsertCommand.UpdatedRowSource = UpdateRowSource.None;
            if (lSqlDataAdopter.DeleteCommand != null)
                lSqlDataAdopter.DeleteCommand.UpdatedRowSource = UpdateRowSource.None;

            lSqlDataAdopter.UpdateBatchSize = 1000;
            return lSqlDataAdopter.Update(pDT);
        }

        /// <summary>
        /// This method is used to executed Inser/ Update/ Select/ Delete SQL Command on desired SQL Database
        /// </summary>
        /// <param name="pSelectCommand"> Select command to be executed </param>
        /// <param name="pInsertCommand"> Insert command to be executed </param>
        /// <param name="pUpdateCommand"> Update command to be executed </param>
        /// <param name="pDeleteCommand"> Delete command to be executed </param>
        /// <param name="pSQLConn"> Connection to the desired SQL Server </param>
        /// <param name="pDT"> Datatable to be used with Update  </param>
        /// <returns>Number of Affected Rows</returns>
        private int _ExecuteNonQuery_SQL(MySqlCommand pSelectCommand, MySqlCommand pInsertCommand, MySqlCommand pUpdateCommand, MySqlCommand pDeleteCommand, MySqlConnection pSQLConn, DataTable pDT)
        {
            DataTable ldt = new DataTable();
            MySqlDataAdapter lSqlDataAdopter = new MySqlDataAdapter();

            if (pSelectCommand != null)
            {
                lSqlDataAdopter.SelectCommand = pSelectCommand;
                lSqlDataAdopter.SelectCommand.Connection = pSQLConn;
            }
            if (pUpdateCommand != null)
            {
                lSqlDataAdopter.UpdateCommand = pUpdateCommand;
                lSqlDataAdopter.UpdateCommand.Connection = pSQLConn;
            }
            if (pDeleteCommand != null)
            {
                lSqlDataAdopter.DeleteCommand = pDeleteCommand;
                lSqlDataAdopter.DeleteCommand.Connection = pSQLConn;
            }
            if (pInsertCommand != null)
            {
                lSqlDataAdopter.InsertCommand = pInsertCommand;
                lSqlDataAdopter.InsertCommand.Connection = pSQLConn;
            }
            return lSqlDataAdopter.Update(pDT);
        }

        /// <summary>
        /// This method is used to executed Inser/ Update/ Select/ Delete SQL Command on desired SQL Database
        /// </summary>
        /// <param name="pSelectCommand"> Select command to be executed </param>
        /// <param name="pInsertCommand"> Insert command to be executed </param>
        /// <param name="pUpdateCommand"> Update command to be executed </param>
        /// <param name="pDeleteCommand"> Delete command to be executed </param>
        /// <param name="pSQLConn"> Connection to the desired SQL Server </param>
        /// <param name="pDT"> Datatable to be used with Update  </param>
        /// <returns>Number of Affected Rows</returns>
        private int _ExecuteNonQueryBatch_SQL(MySqlCommand pSelectCommand, MySqlCommand pInsertCommand, MySqlCommand pUpdateCommand, MySqlCommand pDeleteCommand, MySqlConnection pSQLConn, DataTable pDT)
        {
            DataTable ldt = new DataTable();
            MySqlDataAdapter lSqlDataAdopter = new MySqlDataAdapter();

            if (pSelectCommand != null)
            {
                lSqlDataAdopter.SelectCommand = pSelectCommand;
                lSqlDataAdopter.SelectCommand.Connection = pSQLConn;
            }
            if (pUpdateCommand != null)
            {
                lSqlDataAdopter.UpdateCommand = pUpdateCommand;
                lSqlDataAdopter.UpdateCommand.Connection = pSQLConn;
            }
            if (pDeleteCommand != null)
            {
                lSqlDataAdopter.DeleteCommand = pDeleteCommand;
                lSqlDataAdopter.DeleteCommand.Connection = pSQLConn;
            }
            if (pInsertCommand != null)
            {
                lSqlDataAdopter.InsertCommand = pInsertCommand;
                lSqlDataAdopter.InsertCommand.Connection = pSQLConn;
            }

            if (pUpdateCommand != null)
            {
                lSqlDataAdopter.UpdateCommand.UpdatedRowSource = UpdateRowSource.None;
            }
            if (pInsertCommand != null)
            {
                lSqlDataAdopter.InsertCommand.UpdatedRowSource = UpdateRowSource.None;
            }

            lSqlDataAdopter.UpdateBatchSize = 1000;
            return lSqlDataAdopter.Update(pDT);
        }

        /// <summary>
        /// This method is used to executed Inser/ Update/ Select/ Delete SQL Command for an SqlTransaction on desired SQL Database
        /// </summary>
        /// <param name="pSelectCommand"> Select command to be executed </param>
        /// <param name="pInsertCommand"> Insert command to be executed </param>
        /// <param name="pUpdateCommand"> Update command to be executed </param>
        /// <param name="pDeleteCommand"> Delete command to be executed </param>
        /// <param name="pSQLConn"> Connection to the desired SQL Server </param>
        /// <param name="pDT"> Datatable to be used with Update  </param>
        /// <param name="pSQLTrans"> The SQL Transaction object used to maintain the transaction </param>
        /// <returns>Number of Affected Rows</returns>
        private int _ExecuteNonQuery_SQL(MySqlCommand pSelectCommand, MySqlCommand pInsertCommand, MySqlCommand pUpdateCommand, MySqlCommand pDeleteCommand, DataTable pDT, MySqlConnection pSQLConn, MySqlTransaction pSQLTrans)
        {
            DataTable ldt = new DataTable();
            MySqlDataAdapter lSqlDataAdopter = new MySqlDataAdapter();

            if (pSelectCommand != null)
            {
                lSqlDataAdopter.SelectCommand = pSelectCommand;
                lSqlDataAdopter.SelectCommand.Connection = pSQLConn;
                lSqlDataAdopter.SelectCommand.Transaction = pSQLTrans;
            }
            if (pUpdateCommand != null)
            {
                lSqlDataAdopter.UpdateCommand = pUpdateCommand;
                lSqlDataAdopter.UpdateCommand.Connection = pSQLConn;
                lSqlDataAdopter.UpdateCommand.Transaction = pSQLTrans;
            }
            if (pDeleteCommand != null)
            {
                lSqlDataAdopter.DeleteCommand = pDeleteCommand;
                lSqlDataAdopter.DeleteCommand.Connection = pSQLConn;
                lSqlDataAdopter.DeleteCommand.Transaction = pSQLTrans;
            }
            if (pInsertCommand != null)
            {
                lSqlDataAdopter.InsertCommand = pInsertCommand;
                lSqlDataAdopter.InsertCommand.Connection = pSQLConn;
                lSqlDataAdopter.InsertCommand.Transaction = pSQLTrans;
            }
            return lSqlDataAdopter.Update(pDT);
        }

        /// <summary>
        /// This method is used to executed Inser/ Update/ Select/ Delete SQL Command for an SqlTransaction on desired SQL Database
        /// </summary>
        /// <param name="pSelectCommand"> Select command to be executed </param>
        /// <param name="pInsertCommand"> Insert command to be executed </param>
        /// <param name="pUpdateCommand"> Update command to be executed </param>
        /// <param name="pDeleteCommand"> Delete command to be executed </param>
        /// <param name="pSQLConn"> Connection to the desired SQL Server </param>
        /// <param name="pDT"> Datatable to be used with Update  </param>
        /// <param name="pSQLTrans"> The SQL Transaction object used to maintain the transaction </param>
        /// <returns>Number of Affected Rows</returns>
        private int _ExecuteNonQueryBatch_SQL(MySqlCommand pSelectCommand, MySqlCommand pInsertCommand, MySqlCommand pUpdateCommand, MySqlCommand pDeleteCommand, DataTable pDT, MySqlConnection pSQLConn, MySqlTransaction pSQLTrans)
        {
            DataTable ldt = new DataTable();
            MySqlDataAdapter lSqlDataAdopter = new MySqlDataAdapter();

            if (pSelectCommand != null)
            {
                lSqlDataAdopter.SelectCommand = pSelectCommand;
                lSqlDataAdopter.SelectCommand.Connection = pSQLConn;
                lSqlDataAdopter.SelectCommand.Transaction = pSQLTrans;
            }
            if (pUpdateCommand != null)
            {
                lSqlDataAdopter.UpdateCommand = pUpdateCommand;
                lSqlDataAdopter.UpdateCommand.Connection = pSQLConn;
                lSqlDataAdopter.UpdateCommand.Transaction = pSQLTrans;
            }
            if (pDeleteCommand != null)
            {
                lSqlDataAdopter.DeleteCommand = pDeleteCommand;
                lSqlDataAdopter.DeleteCommand.Connection = pSQLConn;
                lSqlDataAdopter.DeleteCommand.Transaction = pSQLTrans;
            }
            if (pInsertCommand != null)
            {
                lSqlDataAdopter.InsertCommand = pInsertCommand;
                lSqlDataAdopter.InsertCommand.Connection = pSQLConn;
                lSqlDataAdopter.InsertCommand.Transaction = pSQLTrans;
            }


            if (lSqlDataAdopter.UpdateCommand != null)
                lSqlDataAdopter.UpdateCommand.UpdatedRowSource = UpdateRowSource.None;
            if (lSqlDataAdopter.InsertCommand != null)
                lSqlDataAdopter.InsertCommand.UpdatedRowSource = UpdateRowSource.None;
            if (lSqlDataAdopter.DeleteCommand != null)
                lSqlDataAdopter.DeleteCommand.UpdatedRowSource = UpdateRowSource.None;

            lSqlDataAdopter.UpdateBatchSize = 1000;

            return lSqlDataAdopter.Update(pDT);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pSqlCommand"></param>
        /// <param name="affectedRow"></param>
        /// <returns></returns>
        private MySqlCommand _ExecuteNonQuery_SQL(MySqlCommand pSqlCommand, out int affectedRow)
        {
            affectedRow = -1;
            try
            {
                affectedRow = pSqlCommand.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return pSqlCommand;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pSqlCommand"></param>
        /// <returns></returns>
        private int _ExecuteNonQuery_SQL(ref MySqlCommand pSqlCommand)
        {
            int affectedRow = -1;
            try
            {
                affectedRow = pSqlCommand.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                throw ex;
            }


            return affectedRow;
        }
        #endregion

        #region "Public Methods"

        public DataTable ExecuteSQLQuery(string pSQL)
        {
            string connString = _ConnectionManager.GetConnectionString();
            return this._ExecuteQuery_SQL(pSQL, connString);
        }
        /// <summary>
        /// This function will be used to fill the datatable by using the properties of sqlCommand
        /// </summary>
        /// <param name="sqlCmd"></param>
        /// <returns>The datatable filled</returns>
        public DataTable ExecuteSQLQuery(ref MySqlCommand sqlCmd, string commandText)
        {
            DataTable lDataTable = new DataTable();
            sqlCmd.CommandText = commandText;

            MySqlDataAdapter lSqlDataAdopter = new MySqlDataAdapter(sqlCmd);
            lSqlDataAdopter.Fill(lDataTable);
            return lDataTable;
        }

        /// <summary>
        /// This function will be used to execute non-queries
        /// </summary>
        /// <param name="sqlCmd">sqlCmd passed as reference</param>
        /// <param name="commandText">the command to be execueted</param>
        /// <returns>affected rows</returns>
        public int ExecuteNonQuery(ref MySqlCommand sqlCmd, string commandText)
        {
            //DataTable lDataTable = new DataTable();
            sqlCmd.CommandText = commandText;
            return _ExecuteNonQuery_SQL(ref sqlCmd);
            //return lDataTable;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pSQL"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(string pSQL)
        {
            string connString = _ConnectionManager.GetConnectionString();
            MySqlConnection lSQLConn = _ConnectionManager.OpenSqlConnection(connString);
            MySqlCommand sqlCmd = new MySqlCommand();
            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.CommandText = pSQL;
            sqlCmd.Connection = lSQLConn;
            sqlCmd.CommandTimeout = 1200;

            //CustomQuery ret = new CustomQuery();

            return this._ExecuteNonQuery_SQL(sqlCmd);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sqlCmd"></param>
        /// <returns></returns>
        public MySqlTransaction OpenConnectionAndBeginTranscation(out MySqlCommand sqlCmd)
        {
            string connString = _ConnectionManager.GetConnectionString();
            MySqlConnection sqlConn = _ConnectionManager.OpenSqlConnection(connString);
            sqlCmd = new MySqlCommand();
            MySqlTransaction stran = null;

            sqlCmd.CommandType = CommandType.Text;

            sqlCmd.Connection = sqlConn;
            sqlCmd.CommandTimeout = 1200;

            stran = sqlConn.BeginTransaction();

            sqlCmd.Transaction = stran;

            return stran;
        }

        /// <summary>
        /// This function is used to Roll back transactin and to close the sql connection.
        /// </summary>
        /// <param name="sqlcmd"></param>
        public void RollBackTransaction(ref MySqlCommand sqlcmd)
        {
            sqlcmd.Transaction.Rollback();
            sqlcmd.Connection.Close();
        }

        /// <summary>
        /// This function commits the transactiona nd then closes the connection
        /// </summary>
        /// <param name="sqlcmd">sql command passed as reference</param>
        public void CommitTranscation(ref MySqlCommand sqlcmd)
        {
            sqlcmd.Transaction.Commit();
            sqlcmd.Connection.Close();
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="paramList"></param>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public int ExecuteCommand(ArrayList paramList, string commandText)
        {
            string connString = _ConnectionManager.GetConnectionString();
            MySqlConnection lSQLConn = _ConnectionManager.OpenSqlConnection(connString);
            MySqlCommand cmd = new MySqlCommand();
            for (int i = 0; i < paramList.Count; i++)
            {
                cmd.Parameters.Add((SqlParameter)paramList[i]);
                //   cmd.Parameters.Add((SqlParameter)paramList[1]);
            }
            //cmd.Parameters.Add((SqlParameter)paramList[0]);
            //cmd.Parameters.Add((SqlParameter)paramList[1]);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = commandText;
            cmd.Connection = lSQLConn;
            //cmd = new SqlCommand("InsertHashTable", lSQLConn);           

            return this._ExecuteNonQuery_SQL(cmd);
        }

        /// <summary>
        /// This function will be used to execute stored procedures that return values.
        /// </summary>
        /// <param name="paramList"></param>
        /// <param name="commandText"></param>
        /// <param name="rowsAffected"></param>
        /// <returns></returns>
        public MySqlCommand ExecuteCommand(ArrayList paramList, string commandText, out int rowsAffected)
        {
            string connString = _ConnectionManager.GetConnectionString();
            MySqlConnection lSQLConn = _ConnectionManager.OpenSqlConnection(connString);
            MySqlCommand cmd = new MySqlCommand();
            for (int i = 0; i < paramList.Count; i++)
            {
                cmd.Parameters.Add((MySqlParameter)paramList[i]);
                //   cmd.Parameters.Add((SqlParameter)paramList[1]);
            }
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = commandText;
            cmd.Connection = lSQLConn;
            //cmd = new SqlCommand("InsertHashTable", lSQLConn);
            //int rowsAffected = -1;
            return this._ExecuteNonQuery_SQL(cmd, out rowsAffected);
        }

        /// <summary>
        /// This function will be used to execute the stored procedures
        /// </summary>
        /// <param name="sqlCmd">Sql command</param>
        /// <param name="paramList">Paramerter list of stored procedure</param>
        /// <param name="commandText">The name of the Command</param>
        /// <returns></returns>
        public int ExecuteCommand(ref MySqlCommand sqlCmd, ArrayList paramList, string commandText)
        {
            //string connString = _ConnectionManager.GetConnectionString();
            //SqlConnection lSQLConn = _ConnectionManager.OpenSqlConnection(connString);
            //SqlCommand cmd = new SqlCommand();

            for (int i = 0; i < paramList.Count; i++)
            {
                sqlCmd.Parameters.Add((MySqlParameter)paramList[i]);
                //   cmd.Parameters.Add((SqlParameter)paramList[1]);
            }
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.CommandText = commandText;

            //sqlCmd.Connection = lSQLConn;
            //cmd = new SqlCommand("InsertHashTable", lSQLConn);
            //int rowsAffected = -1;

            return this._ExecuteNonQuery_SQL(ref sqlCmd);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pSQLs"></param>
        /// <returns></returns>
        public int ExecuteNonQueries(ArrayList pSQLs)
        {
            int affectedRows = -2;
            string connString = _ConnectionManager.GetConnectionString();
            MySqlConnection lSQLConn = _ConnectionManager.OpenSqlConnection(connString);
            MySqlTransaction lSQLTrans = lSQLConn.BeginTransaction();
            try
            {
                foreach (string pSQL in pSQLs)
                {
                    MySqlCommand sqlCmd = new MySqlCommand();
                    sqlCmd.CommandType = CommandType.Text;
                    sqlCmd.CommandText = pSQL;
                    sqlCmd.Connection = lSQLConn;
                    sqlCmd.Transaction = lSQLTrans;
                    sqlCmd.CommandTimeout = 1200;
                    CustomQuery ret = new CustomQuery();

                    affectedRows = ret._ExecuteNonQuery_SQL(sqlCmd);
                }

                lSQLTrans.Commit();
            }
            catch (Exception Ex)
            {
                lSQLTrans.Rollback();
                throw Ex;
            }
            return affectedRows;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sqlCmd"></param>
        /// <returns></returns>
        public MySqlConnection OpenConnection(out MySqlCommand sqlCmd)
        {
            string connString = _ConnectionManager.GetConnectionString();
            MySqlConnection sqlConn = _ConnectionManager.OpenSqlConnection(connString);
            sqlCmd = new MySqlCommand();
            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.Connection = sqlConn;
            sqlCmd.CommandTimeout = 1200;
            return sqlConn;
        }
        #endregion
    }
}
