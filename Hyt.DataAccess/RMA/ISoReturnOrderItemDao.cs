using Hyt.DataAccess.Base;
using Hyt.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.RMA
{
    /// <summary>
    /// RMA销售单明细关系表
    /// </summary>
    /// <remarks>2014-06-16  朱成果 创建</remarks>
    public abstract class ISoReturnOrderItemDao : DaoBase<ISoReturnOrderItemDao>
    {
        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>新增记录编号</returns>
        /// <remarks>2014-06-16  朱成果 创建</remarks>
        public abstract void Insert(SoReturnOrderItem entity);

        /// <summary>
        /// 获取关系表详情
        /// </summary>
        /// <param name="orderItemSysNo">RMA销售单明细编号</param>
        /// <returns></returns>
        /// <remarks>2014-06-16  朱成果 创建</remarks>
        public abstract SoReturnOrderItem GetSoReturnOrderItem(int orderItemSysNo);

        /// <summary>
        /// 获取关系表详情
        /// </summary>
        /// <param name="transactionSysNo">事物编号</param>
        /// <returns></returns>
        /// <remarks>2014-06-16  杨浩 创建</remarks>
        public abstract IList<SoReturnOrderItem> GetSoReturnOrderItem(string transactionSysNo);
    }
}
