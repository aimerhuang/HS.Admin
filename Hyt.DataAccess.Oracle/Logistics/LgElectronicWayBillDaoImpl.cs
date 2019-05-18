using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Logistics;
using Hyt.Model;
using Hyt.Model.WorkflowStatus;

namespace Hyt.DataAccess.Oracle.Logistics
{
    /// <summary>
    /// 电子面单信息
    /// </summary>
    /// <remarks> 
    /// 2015-9-28 杨浩 创建
    /// </remarks>
    public class LgElectronicWayBillDaoImpl:ILgElectronicWayBillDao
    {
        /// <summary>
        /// 插入电子面单信息
        /// </summary>
        /// <param name="model">电子面单信息</param>
        /// <remarks> 
        /// 2015-9-28 杨浩 创建
        /// </remarks>
        public override int Insert(LgElectronicWayBill model)
        {
            return Context.Insert("LgElectronicWayBill", model)
                         .AutoMap(x => x.SysNo)
                         .ExecuteReturnLastId<int>("SysNo");
        }

        /// <summary>
        /// 更新电子面单状态
        /// </summary>
        /// <param name="whstockoutsysno">出库单编号</param>
        /// <param name="status">状态</param>
        /// <param name="userSysNo">用户编号</param>
        /// <returns>结果（成功返回true,否则返回false）</returns>
        /// <remarks>2015-9-29 杨浩 创建</remarks>
        public override bool UpdateStatus(int whstockoutsysno, LogisticsStatus.电子面单状态 status,int userSysNo)
        {
            int result = Context.Update("LgElectronicWayBill")
                .Column("status", (int)status)
                .Column("lastupdateby", userSysNo)
                .Column("lastupdatedate", DateTime.Now)
                .Where("whstockoutsysno", whstockoutsysno)
                .Execute();
            return result > 0;
        }

        /// <summary>
        /// 根据出库单编号获取电子面单信息
        /// </summary>
        /// <param name="stockOutSysNo">出库单编号</param>
        /// <returns>电子面单信息</returns>
        /// <remarks>2015-9-29 杨浩 创建</remarks>
        public override LgElectronicWayBill GetElectronicWayBillByStockOutSysNo(int stockOutSysNo)
        {
            return Context.Sql(@"select * from LgElectronicWayBill  where WhStockOutSysNo=@0  ", stockOutSysNo).QuerySingle<LgElectronicWayBill>();
        }
    }
}
