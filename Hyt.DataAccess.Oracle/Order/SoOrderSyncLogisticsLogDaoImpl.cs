using Hyt.DataAccess.Order;
using Hyt.Model.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Oracle.Order
{
    /// <summary>
    /// 订单同步物流日志
    /// </summary>
    /// <remarks>2016-7-29 杨浩 创建</remarks>
    public class SoOrderSyncLogisticsLogDaoImpl : ISoOrderSyncLogisticsLogDao
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns></returns>
        public override int Insert(SoOrderSyncLogisticsLog model)
        {
           return Context.Insert<SoOrderSyncLogisticsLog>("SoOrderSyncLogisticsLog", model)
               .AutoMap(o =>o.SysNo)
               .ExecuteReturnLastId<int>();
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="orderSysNo">订单编号</param>
        /// <param name="code">物流代码</param>
        /// <returns></returns>
        public override int DeleteByOrderSysNoAndCode(int orderSysNo, int code)
        {
            return Context.Sql("DELETE FROM SoOrderSyncLogisticsLog WHERE orderSysNo=@orderSysNo and code=@code")
                .Parameter("orderSysNo", orderSysNo)
                .Parameter("code", code)
                .Execute();
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns></returns>
        public override int Update(SoOrderSyncLogisticsLog model)
        {
           return Context.Update<SoOrderSyncLogisticsLog>("SoOrderSyncLogisticsLog", model)
                .AutoMap(x => x.SysNo, x => x.CreateDate, x => x.CreatedBy)
                .Where("Code", model.Code)
                .Where("OrderSysNo", model.OrderSysNo)
                .Execute();
        }
        /// <summary>
        /// 获取订单同步物流日志实体
        /// </summary>
        /// <param name="orderSysNo">订单编号</param>
        /// <param name="code">物流代码</param>
        /// <returns></returns>
        public override SoOrderSyncLogisticsLog GetModel(int orderSysNo, int code)
        {
           return Context.Sql("select * from SoOrderSyncLogisticsLog where orderSysNo=@orderSysNo and code=@code")
                  .Parameter("orderSysNo", orderSysNo)
                  .Parameter("code", code)
                  .QuerySingle<SoOrderSyncLogisticsLog>();
        }

        public override List<SoOrderSyncLogisticsLog> GetModelList(int orderSysno)
        {
            string sql = "select * from SoOrderSyncLogisticsLog where OrderSysNo = '" + orderSysno + "' ";
            return Context.Sql(sql).QueryMany<SoOrderSyncLogisticsLog>();
        }
    }
}
