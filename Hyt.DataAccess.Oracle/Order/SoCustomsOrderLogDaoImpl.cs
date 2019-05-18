using Hyt.DataAccess.Order;
using Hyt.Model.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Oracle.Order
{
    /// <summary>
    /// 海关订单日志
    /// </summary>
    /// <remarks>2016-1-2 杨浩 创建</remarks>
    public class SoCustomsOrderLogDaoImpl : ISoCustomsOrderLogDao
    {
        /// <summary>
        /// 获取海关订单日志详情
        /// </summary>
        /// <param name="orderSysNo">订单编号</param>
        /// <param name="customsChannel">海关通道</param>
        /// <returns></returns>
        public override SoCustomsOrderLog GetCustomsOrderLogInfo(int orderSysNo, int customsChannel)
        {
            return Context.Sql("select * from  SoCustomsOrderLog where orderSysNo=@orderSysNo and customsChannel=@customsChannel")
               .Parameter("orderSysNo", orderSysNo)
               .Parameter("customsChannel", customsChannel)
               .QuerySingle<SoCustomsOrderLog>();
        }
        /// <summary>
        /// 新增海关订单日志
        /// </summary>
        /// <param name="model">海关订单日志实体类</param>
        /// <returns></returns>
        public override int AddCustomsOrderLog(SoCustomsOrderLog model)
        {
            return Context.Insert("SoCustomsOrderLog", model)
                   .AutoMap(x=>x.SysNo)
                   .ExecuteReturnLastId<int>();
        }
        /// <summary>
        /// 更新海关订单日志
        /// </summary>
        /// <param name="model">海关订单日志实体类</param>
        /// <returns></returns>
        public override int UpdateCustomsOrderLog(SoCustomsOrderLog model)
        {
            return Context.Update("SoCustomsOrderLog", model)
                  .AutoMap(x => x.SysNo,x=>x.CreatedBy,x=>x.CustomsChannel,x=>x.CreateDate,x=>x.OrderSysNo)
                  .Where("sysNo",model.SysNo)
                  .Execute();
        }
        /// <summary>
        /// 是否唯一海关订单日志
        /// </summary>
        /// <param name="orderSysNo">订单编号</param>
        /// <param name="customsChannel">海关通道</param>
        /// <returns></returns>
        public override int IsOnly(int orderSysNo, int customsChannel)
        {
            return Context.Sql("select count(1) from SoCustomsOrderLog where orderSysNo=@orderSysNo and customsChannel=@customsChannel")
                .Parameter("orderSysNo",orderSysNo)
                .Parameter("customsChannel",customsChannel)
                .QuerySingle<int>();
        }
    }
}
