using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Base;
using Hyt.Model;

namespace Hyt.DataAccess.RMA
{
    /// <summary>
    /// 退换货商品明细
    /// </summary>
    /// <remarks>2013-07-11 朱成果 创建</remarks>
    public abstract class IRcReturnItemDao : DaoBase<IRcReturnItemDao>
    {
        /// <summary>
        /// 根据退换货编号获取退换货商品明细
        /// </summary>
        /// <param name="returnSysNo">退换货编号</param>
        /// <returns></returns>
        /// <remarks>2013-07-11 朱成果 创建</remarks>
        public abstract List<RcReturnItem> GetListByReturnSysNo(int returnSysNo);

        /// <summary>
        /// 插入退换货明细
        /// </summary>
        /// <param name="entity">退换货明细实体</param>
        /// <returns>插入的编号</returns>
        /// <remarks>2013-07-11 朱成果 创建</remarks> 
        public abstract int Insert(RcReturnItem entity);

        /// <summary>
        /// 删除退换货明细列表
        /// </summary>
        /// <param name="returnSysNo">退换货编号</param>
        /// <returns></returns>
        /// <remarks>2013-07-11 朱成果 创建</remarks> 
        public abstract void DeleteByReturnSysNo(int returnSysNo);

        /// <summary>
        /// 获取退换货明细
        /// </summary>
        /// <param name="orderNo">订单号</param>
        /// <returns></returns>
        /// <remarks>2013-08-07 朱成果 创建</remarks> 
        public abstract List<CBRmaReturnItem> GetListByOrder(int orderNo);
        

    }
}
