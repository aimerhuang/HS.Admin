using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.SellBusiness;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;
using Hyt.Util;
using Hyt.DataAccess.Order;
namespace Hyt.BLL.Order
{
    /// <summary>
    /// 订单明细管理
    /// </summary>
    /// <remarks>2016-1-7 王耀发 创建</remarks>
    public class SoOrderItemBo : BOBase<SoOrderItemBo>
    {
        /// <summary>
        /// 获取订单明细
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        /// <remarks>2016-1-7 王耀发 创建</remarks>
        public Pager<CBSoOrderItem> GetOrderItemsRecordList(ParaOrderItemRecordFilter filter)
        {
            return ISoOrderItemDao.Instance.GetOrderItemsRecordList(filter);
        }

        public List<CBSoOrderItem> GetCBOrderItemListBySysNos(string SysNos)
        {
            return ISoOrderItemDao.Instance.GetCBOrderItemListBySysNos(SysNos);
        }
    }
}
