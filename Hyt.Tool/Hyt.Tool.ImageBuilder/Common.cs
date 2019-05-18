using System;
using System.Configuration;
using System.Data;
using System.Collections; 
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Hyt.ProductImport;
using System.Data.SqlClient;

namespace Hyt.Tool.ImageBuilder.BLL

{
    public class Common 
    {
        private static DataSet rds=new DataSet();
        public static DataSet RDS
        {
            get { return rds; }
            set { rds = value; }
        }

        public static int percent=0;      

        /// <summary>
        /// 导入到Oracle
        /// </summary>
        /// <returns></returns>
        public DataTable  GetProducts()
        {
            string sSql = "SELECT * from PdProduct where DealerSysNo=108";


            using (SqlConnection myConn = new SqlConnection(ConfigurationManager.AppSettings["OracleConnectionString"].ToString()))
            {

                SqlDataAdapter myAdapter = new SqlDataAdapter(sSql, myConn);

                myAdapter.Fill(Common.RDS, "PdProduct");


            }
            return RDS.Tables[0];
        }

        /// <summary>
        /// 根据类名获取实例
        /// </summary>
        /// <param name="className">类名</param>
        /// <returns>对象实例</returns>
        private static object  GetInstance(string className )
        {
            System.Reflection.Assembly asm = System.Reflection.Assembly.GetExecutingAssembly();
            Object obj = asm.CreateInstance(className, true);
            return obj;
        }

    
    }
}
