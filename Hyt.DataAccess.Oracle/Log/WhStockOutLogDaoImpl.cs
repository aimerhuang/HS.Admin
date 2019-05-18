using Hyt.DataAccess.Log;

namespace Hyt.DataAccess.Oracle.Log
{
    /// <summary>
    /// 出库单详细日志数据访问
    /// </summary>
    /// <remarks>2014-12-18 杨浩 创建</remarks>
    public class WhStockOutLogDaoImpl : IWhStockOutLogDao
    {
        /// <summary>
        /// 创建出库单详细日志
        /// </summary>
        /// <param name="log">日志</param>
        /// <returns>日志主键</returns>
        /// <remarks>2014-12-18 杨浩 创建</remarks>
        public override int CreateWhStockOutLog(Model.WhStockOutLog log)
        {
            var sysNo = Context.Insert("WhStockOutLog", log)
                             .AutoMap(o => o.SysNo)
                             .ExecuteReturnLastId<int>("SysNo");
            return sysNo;
        }
    }
}
