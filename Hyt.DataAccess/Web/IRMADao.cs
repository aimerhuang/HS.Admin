using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Base;
using Hyt.Model;

namespace Hyt.DataAccess.Web
{
    /// <summary>
    /// 退换货
    /// </summary>
    /// <remarks>2013-09-03 邵斌 创建</remarks>
    public abstract class IRMADao : DaoBase<IRMADao>
    {
        /// <summary>
        /// 通过发票系统编号获取税率
        /// </summary>
        /// <param name="invoiceSysNo">发票系统编号</param>
        /// <returns>返回税点</returns>
        /// <remarks>2013-09-03 邵斌 创建</remarks>
        public abstract decimal GetRateByInvoice(int invoiceSysNo);

        /// <summary>
        /// 退换货申请入库
        /// </summary>
        /// <param name="returnEntity">退换货主表对象</param>
        /// <param name="items">退换货子表对象集合</param>
        /// <returns>返回 true:添加成功 false:失败</returns>
        /// <remarks>2013-09-03 邵斌 创建</remarks>
        public abstract bool InsertRMA(Model.RcReturn returnEntity, IList<Model.RcReturnItem> items);

        /// <summary>
        /// 退还货历史查询
        /// </summary>
        /// <param name="customer">用户系统编号</param>
        /// <param name="endTime">截止时间</param>
        /// <param name="pager">分类对象</param>
        /// <returns></returns>
        /// <remarks>2013-09-03 邵斌 创建</remarks>
        public abstract void Search(int customer, DateTime endTime, ref Pager<CBWebRMA> pager);
    }
}
