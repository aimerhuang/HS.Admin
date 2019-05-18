using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace WindowsPiPei
{
    class DB
    {

        public class SqlHelper
        {
            public SqlHelper()
            {
                //
                // TODO: 在此处添加构造函数逻辑
                //
            }

            /// <summary>
            /// 连接字符串
            /// </summary>
            private static readonly string connectionString = ConfigurationManager.ConnectionStrings["AIS20160412101109"].ConnectionString;

            private static SqlConnection _conn;
            /// <summary>
            /// 获取连接
            /// </summary>
            public static SqlConnection Conn
            {
                get
                {
                    if (_conn == null) _conn = new SqlConnection(connectionString);

                    if (_conn.State == ConnectionState.Broken)
                    {
                        _conn.Close();
                        _conn.Open();
                    }
                    else if (_conn.State == ConnectionState.Closed)
                    {
                        _conn.Open();
                    }
                    return _conn;
                }
            }


            /// <summary>
            /// 关闭连接
            /// </summary>
            public static void CloseConnection()
            {
                if (Conn == null) return;
                if (Conn.State == ConnectionState.Open || Conn.State == ConnectionState.Broken)
                {
                    Conn.Close();
                }
            }

            /// <summary>
            /// 增删改
            /// </summary>
            /// <param name="sql">sql语句</param>
            /// <param name="type">执行解析类型 默认 sql</param>
            /// <param name="param">参数</param>
            /// <returns></returns>
            public static int ExecuteNonQuery(string sql, CommandType type = CommandType.Text, params  SqlParameter[] param)
            {
                //创建执行对象
                SqlCommand cmd = new SqlCommand(sql, Conn);
                //判断是否存在参数
                if (param != null && param.Length > 0)
                {
                    cmd.Parameters.AddRange(param);
                }
                cmd.CommandType = type;//指定解析类型

                return cmd.ExecuteNonQuery();//返回执行后的结果


            }

            /// <summary>
            /// 查询
            /// </summary>
            /// <param name="sql">sql语句</param>
            /// <param name="type">执行解析类型 默认 sql</param>
            /// <param name="param">参数</param>
            /// <returns>SqlDataReader</returns>
            public static SqlDataReader GetDataReader(string sql, CommandType type = CommandType.Text, params  SqlParameter[] param)
            {

                SqlCommand cmd = new SqlCommand(sql, Conn);
                if (param != null && param.Length > 0)
                {
                    cmd.Parameters.AddRange(param);
                }
                cmd.CommandType = type;

                return cmd.ExecuteReader();

            }
            /// <summary>
            /// 查询(断开式连接)
            /// </summary>
            /// <param name="sql">sql语句</param>
            /// <param name="type">执行解析类型 默认 sql</param>
            /// <param name="param">参数</param>
            /// <returns>DataTable</returns>
            public static DataTable GetTable(string sql, CommandType type = CommandType.Text, params  SqlParameter[] param)
            {
                DataSet ds = new DataSet();
                SqlDataAdapter sda = new SqlDataAdapter(sql, Conn);
                if (param != null && param.Length > 0)
                {
                    sda.SelectCommand.Parameters.AddRange(param);
                }
                sda.SelectCommand.CommandType = type;
                sda.Fill(ds);//填充数据到ds

                return ds.Tables[0];

            }






        }


    }
}
