using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Base;
using Hyt.Model;
using Hyt.Model.WorkflowStatus;

namespace Hyt.DataAccess.Logistics
{
    public abstract class ILgElectronicWayBillDao : DaoBase<ILgElectronicWayBillDao>
    {
        /// <summary>
        /// 插入电子面单信息
        /// </summary>
        /// <param name="model">物流信息实体</param>
        /// <remarks> 
        /// 2015-9-28 杨浩 创建
        /// </remarks>
        public abstract int Insert(LgElectronicWayBill model);

        /// <summary>
        /// 更新电子面单状态
        /// </summary>
        /// <param name="whstockoutsysno">出库单编号</param>
        /// <param name="status">状态</param>
        /// <param name="userSysNo">用户编号</param>
        /// <returns>结果（成功返回true,否则返回false）</returns>
        /// <remarks>2015-9-29 杨浩  创建</remarks>
        public abstract bool UpdateStatus(int whstockoutsysno, LogisticsStatus.电子面单状态 status, int userSysNo);

        /// <summary>
        /// 根据出库单编号获取电子面单信息
        /// </summary>
        /// <param name="stockOutSysNo">出库单编号</param>
        /// <returns>电子面单信息</returns>
        /// <remarks>2015-9-29 杨浩  创建</remarks>
        public abstract LgElectronicWayBill GetElectronicWayBillByStockOutSysNo(int stockOutSysNo);
    }
}
