using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Tool.DataAccess.Base;
using System.Configuration;

namespace Hyt.ProductImport
{
    /// <summary>
    /// sqlserver数据驱动类
    /// </summary>
    /// <Remark>
    /// 唐永勤 
    /// </Remark>
    public class DataProvider
    {
        /// <summary>
        /// sqlserver端单列模式
        /// </summary>
        protected static string SqlserverConnectionString = ConfigurationManager.AppSettings["SqlConnectionString"];

        protected static IDbContext Context = null;

        public static IDbContext Instance
        {
            get
            {
                Context = new DbContext().ConnectionString(SqlserverConnectionString, new SqlServerProvider());
                return Context;
            }         
        }

        /// <summary>
        /// oracle端单列
        /// </summary>
        protected static string OracleConnectionString = ConfigurationManager.AppSettings["OracleConnectionString"];

        protected static IDbContext OracleContext = null;

        public static IDbContext OracleInstance
        {
            get
            {
                OracleContext = new DbContext().ConnectionString(OracleConnectionString, new OracleProvider());
                return OracleContext;
            }
        }

        
      
    }
    
}
