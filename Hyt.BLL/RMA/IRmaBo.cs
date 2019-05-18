using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Model;

namespace Hyt.BLL.RMA
{
    /// <summary>
    /// 退换货业务
    /// </summary>
    /// <remarks>2013-07-11 朱成果 创建</remarks>
    public interface  IRmaBo
    {
       
    
          /// <summary>
        /// 订单退换货请求
        /// </summary>
        /// <param name="orderSysNo">订单号</param>
        /// <param name="rmaType">退换货类型 Hyt.Model.WorkflowStatus.RmaStatus.RMA类型</param>
        /// <returns>true 允许退换货 false 不允许退换货</returns>
        /// <remarks>2013-08-15 朱成果 创建</remarks>
         bool OrderRMARequest(int orderSysNo, int rmaType);

        /// <summary>
        /// 根据出库单明细系统编号获取退换货申请单
        /// </summary>
        /// <param name="stockOutItemSysNo">出库单明细系统编号</param>
        /// <param name="sourceType">退换货申请单来源</param>
        /// <returns></returns>
        /// <remarks>
        /// 2013/8/21 何方 创建
        /// </remarks>
        IList<RcReturn> Get(int stockOutItemSysNo, Model.WorkflowStatus.RmaStatus.退换货申请单来源? sourceType);
    }
}
