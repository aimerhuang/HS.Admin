using System;
using System.Configuration;
using System.Data;
using System.Collections; 
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.DataAccess.Client;

using Hyt.DataImporter.TaskThread;
using Hyt.ProductImport;
using Hyt.DataImporter.Task;
using System.Data.SqlClient;

namespace Hyt.DataImporter
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
        /// <param name="dtData">需要导入的datatable</param>
        /// <returns></returns>
        public static void BulkToOracle(DataTable dtData)
        {
                        
            using (OracleConnection conn_bulkcopy = new OracleConnection(ConfigurationManager.AppSettings["OracleConnectionString"].ToString()))
            {
               //OracleTransaction myTransaction = conn_bulkcopy.BeginTransaction();

                try
                {
                    conn_bulkcopy.Open();
                    using (OracleBulkCopy bcp = new OracleBulkCopy(conn_bulkcopy))
                    {
                        bcp.DestinationTableName = dtData.TableName;
                        if (bcp.DestinationTableName != "WHwarehousearea1")
                        {
                            bcp.BatchSize = dtData.Rows.Count;
                            SetColumnMapping(dtData, bcp);
                            bcp.WriteToServer(dtData);
                        }
                    }                    
                }
                catch (Exception ex)
                {
                    throw ex;

                }

            }
           
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

        /// <summary>
        /// 根据表名设置列映射
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="OracleBulkCopy">批量复制对象</param>
        private static void SetColumnMapping(DataTable dtData, OracleBulkCopy bcp)
        {
            object myObject = GetInstance("Hyt.DataImporter.Task." + dtData.TableName);
            ((BaseTask)myObject).SetColumnMapping(bcp);
           
        }
    }
}
