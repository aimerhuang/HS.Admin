using Hyt.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Logistics
{
    public abstract class IMkExpressLogDao : Hyt.DataAccess.Base.DaoBase<IMkExpressLogDao>
    {
        /// <summary>
        /// 获取物流日志
        /// </summary>
        /// <param name="pagerFilter">查询过滤对象</param>
        /// <returns>返回物流单日志表</returns>
        /// <remarks>2013-12-19 沈强 创建</remarks>
        public abstract Pager<MkExpressLog> GetLogisticsDeliveryItems(Pager<MkExpressLog> pagerFilter);

        /// <summary>
        /// 根据物流单号，获取物流日志
        /// </summary>
        /// <param name="expressNo">物流单号</param>
        /// <returns>返回物流单日志表</returns>
        /// <remarks>2013-12-19 沈强 创建</remarks>
        public abstract IList<MkExpressLog> GetMkExpressLogList(string expressNo);

        /// <summary>
        /// 批量插入物流日志
        /// </summary>
        /// <param name="logs">物流日志集合</param>
        /// <returns></returns>
        /// <remarks>2013-12-19 沈强 创建</remarks>
        public abstract void Insert(List<MkExpressLog> logs);
    }
}
