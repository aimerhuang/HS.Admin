using Hyt.DataAccess.Base;
using Hyt.Model.Procurement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Procurement
{
    public abstract class IPmPointsOrderDao : DaoBase<IPmPointsOrderDao>
    {
        /// <summary>
        /// 创建分货单
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public abstract int CreatePointsOrder(PmPointsOrder order);
        /// <summary>
        /// 修改分货单
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public abstract void UpdatePointsOrder(PmPointsOrder order);

        /// <summary>
        /// 创建分货单
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public abstract int CreatePointsOrderItem(PmPointsOrderItem order);
        /// <summary>
        /// 修改分货单
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public abstract void UpdatePointsOrderItem(PmPointsOrderItem order);

        /// <summary>
        /// 分货货品列表
        /// </summary>
        /// <param name="PSysNo"></param>
        /// <returns></returns>
        public abstract List<CBPmPointsOrderItem> GetPointsOrderItems(int PSysNo);
        /// <summary>
        /// 获取分货单
        /// </summary>
        /// <param name="SysNo"></param>
        /// <returns></returns>
        public abstract CBPmPointsOrder GetPmPointsOrder(int SysNo);
        /// <summary>
        /// 分货单分页
        /// </summary>
        /// <param name="pager"></param>
        public abstract void GetPmPointsOrderPager(ref Model.Pager<CBPmPointsOrder> pager);
        /// <summary>
        /// 删除分货单货品数据
        /// </summary>
        /// <param name="delSysNos"></param>
        public abstract void DeletePointsOrderData(string delSysNos);
        /// <summary>
        /// 更新分货单状态
        /// </summary>
        /// <param name="SysNo"></param>
        /// <param name="Status"></param>
        public abstract void UpdatePointsOrderStatus(int SysNo,int Status);
        /// <summary>
        /// 获取分货单列表
        /// </summary>
        /// <param name="pSysNoList"></param>
        /// <returns></returns>
        public abstract List<CBPmPointsOrder> GetPointsOrderListByPSysNo(string pSysNoList);

        public abstract List<CBPmPointsOrderItem> GetPointsOrderItemListByStatus(string pSysNoList);
        /// <summary>
        /// 获取采购分货单厂家生产单列表
        /// </summary>
        /// <param name="PSysNo"></param>
        /// <returns></returns>
        public abstract List<CBPmPointsOrder> GetPointsOrderListBySinglePSysNo(int PSysNo);
    }
}
