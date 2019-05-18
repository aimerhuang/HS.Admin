using System.Configuration;
using System.Transactions;

namespace Hyt.DataAccess.Base
{
    /// <summary>
    /// 数据抽象类的基类
    /// </summary>
    /// <typeparam name="T">数据抽象类型</typeparam>
    /// <Remark>
    /// 2013-6-26 杨浩 创建
    /// </Remark>
    public abstract class DaoBase<T> where T: DaoBase<T>
    {
        /// <summary>
        /// 返回抽象接口的具体实例
        /// </summary>
        public static T Instance
        {
            get 
            {
                return DaoFactory<T>.Create();
            }
        }

        /// <summary>
        /// SqlServer连接字符串
        /// </summary>
        private static string SqlServerConnectionString = ConfigurationManager.AppSettings["OracleConnectionString"];

        /// <summary>
        /// SqlServer连接字符串Kis
        /// </summary>
        private static string SqlServerConnectionStringKis = ConfigurationManager.AppSettings["OracleConnectionStringKis"];

        /// <summary>
        /// SqlServer连接字符串B2B
        /// </summary>
        private static string OracleConnectionStringB2B = ConfigurationManager.AppSettings["OracleConnectionStringB2B"];
        
        /// <summary>
        /// SqlServer数据访问上下文
        /// </summary>
        /// <returns>SqlServer</returns>
        /// <remarks>2013-6-26 杨浩 创建</remarks>
        protected IDbContext Context
        {
            get
            {
                //封装连接
                //var context= LocalTransaction.GetDbContext();
                //IDbContext context = new DbContext().ConnectionString(OracleConnectionString, new OracleProvider());
                IDbContext context = new DbContext().ConnectionString(SqlServerConnectionString, new SqlServerProvider());
                return context.IgnoreIfAutoMapFails(true);
            }
        }

        /// <summary>
        /// SqlServer数据访问上下文
        /// </summary>
        /// <returns>SqlServer</returns>
        /// <remarks>2013-6-26 杨浩 创建</remarks>
        protected IDbContext ContextKis
        {
            get
            {
                //封装连接
                //var context= LocalTransaction.GetDbContext();
                //IDbContext context = new DbContext().ConnectionString(OracleConnectionString, new OracleProvider());
                IDbContext context = new DbContext().ConnectionString(SqlServerConnectionStringKis, new SqlServerProvider());
                return context.IgnoreIfAutoMapFails(true);
            }
        }

        /// <summary>
        /// SqlServer数据访问上下文
        /// </summary>
        /// <returns>SqlServer</returns>
        /// <remarks>
        /// 2017-10-11 罗勤瑶 创建
        /// </remarks>
        protected IDbContext ContextB2B
        {
            get
            {
                //封装连接
                //var context= LocalTransaction.GetDbContext();
                //IDbContext context = new DbContext().ConnectionString(OracleConnectionString, new OracleProvider());
                IDbContext context = new DbContext().ConnectionString(OracleConnectionStringB2B, new SqlServerProvider());
                return context.IgnoreIfAutoMapFails(true);
            }
        }
        
    }
}
