using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Diagnostics;

namespace Extra.SMS.Zhangxun
{
    /// <summary>
    /// 数据操作
    /// </summary>
    /// <remarks>2013-6-26 杨浩 添加</remarks>
    internal class DatabaseAccess
    {
        private readonly static string connectionString = ConfigurationManager.AppSettings["ConnectionString_SMS"];

       /// <summary>
        /// 批处理
       /// </summary>
       /// <param name="query"></param>
       /// <param name="mobiles"></param>
       /// <param name="msg"></param>
       /// <returns>受影响行数</returns>
        /// <remarks>2013-6-26 杨浩 添加</remarks>
        public static int ExecuteSql(string query, IList<String> mobiles, string msg)
        {
            int rowCount = -1;//受影响行数
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                conn.StatisticsEnabled = true;
                SqlParameterCollection p = null;
                //using (TransactionScope scope = new TransactionScope())
                //{
                using (var cmd = new SqlCommand(query, conn))
                {
                    //conn.EnlistTransaction(Transaction.Current);
                    p = cmd.Parameters;
                    for (int i = 0; i < mobiles.Count; i++)
                    {
                        p.Add("Phone" + i, SqlDbType.VarChar, 50).Value = mobiles[i];
                        p.Add("Msg" + i, SqlDbType.NVarChar, 500).Value = msg;
                    }
                    try
                    {
                        rowCount = cmd.ExecuteNonQuery();
                        //scope.Complete(); 
                    }
                    catch (SqlException ex)
                    {
                        EventLog.WriteEntry("Application", "Error in SendMSmsByDB:" + ex.Message + "\n", EventLogEntryType.Error, 101);
                    }
                }
                //}
            }
            return rowCount;
        }

        /// <summary>
        /// 批处理
        /// </summary>
        /// <param name="table"></param>
        /// <param name="msg"></param>
        /// <param name="StoredName"></param>
        /// <param name="TypeName"></param>
        /// <returns>受影响行数</returns>
        /// <remarks>2013-6-26 杨浩 添加</remarks>
        public static int ExecuteSql(DataTable table, string msg, string StoredName,string TypeName)
        {
            int rowCount = -1;//受影响行数
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                conn.StatisticsEnabled = true;
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText =StoredName;
                    SqlParameterCollection p = cmd.Parameters;
                    if(!string.IsNullOrEmpty(msg))
                    p.Add("Msg", SqlDbType.NVarChar, 500).Value = msg;
                    SqlParameter sp = p.AddWithValue("rows", table);
                    sp.SqlDbType = SqlDbType.Structured;
                    sp.TypeName = TypeName;                    
                    try
                    {
                        rowCount = cmd.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                        EventLog.WriteEntry("Application", "Error in SendMSmsByDB:" + ex.Message + "\n", EventLogEntryType.Error, 101);
                    }
                }
            }
            return rowCount;
        }

        /// <summary>
        /// 批处理
        /// </summary>
        /// <param name="mobiles"></param>
        /// <param name="StoredName"></param>
        /// <param name="TypeName"></param>
        /// <returns>受影响行数</returns>
        /// <remarks>2013-6-26 杨浩 添加</remarks>
        public static int ExecuteSql(DataTable mobiles, string StoredName,string TypeName)
        {
            return ExecuteSql(mobiles, string.Empty, StoredName,TypeName);
        }

        /// <summary>
        /// 执行查询语句，返回SqlDataReader ( 注意：调用该方法后，一定要对SqlDataReader进行Close )
        /// </summary>
        /// <param name="strSQL">查询语句</param>
        /// <returns>SqlDataReader</returns>
        /// <remarks>2013-6-26 杨浩 添加</remarks>
        public static SqlDataReader ExecuteReader(string strSQL)
        {
            var connection = new SqlConnection(connectionString);
            var cmd = new SqlCommand(strSQL, connection);
            try
            {
                connection.Open();
                SqlDataReader myReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                return myReader;
            }
            catch (SqlException e)
            {
                throw e;
            }
        }
    }
}
