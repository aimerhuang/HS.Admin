using Hyt.Model;

namespace Hyt.DataAccess.Log
{
    /// <summary>
    /// 出库单详细日志数据访问 抽象类
    /// </summary>
    /// <remarks>2014-12-18 杨浩 创建</remarks>
    public abstract class IWhStockOutLogDao : Hyt.DataAccess.Base.DaoBase<IWhStockOutLogDao>
    {
        /// <summary>
        /// 创建出库单详细日志
        /// </summary>
        /// <param name="log">日志</param>
        /// <returns>日志主键</returns>
        /// <remarks>2014-12-18 杨浩 创建</remarks>
        public abstract int CreateWhStockOutLog(WhStockOutLog log);
    }
}
