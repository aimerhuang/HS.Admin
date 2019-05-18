using Hyt.DataAccess.Order;
using Hyt.Model.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Oracle.Order
{
    /// <summary>
    /// 订单支付单推送日志
    /// </summary>
    /// <remarks>2017-08-14 杨浩 创建</remarks>
    public class SoOrderPayPushLogDaoImpl:ISoOrderPayPushLogDao 
    {
        /// <summary>
        /// 插入订单支付单日志
        /// </summary>
        /// <param name="item"></param>
        /// <returns>操作是否成功</returns>
        /// <remarks>2017-08-14 杨浩 创建</remarks>
        public override SoOrderPayPushLog Insert(SoOrderPayPushLog entity)
        {
            entity.SysNo = Context.Insert("SoOrderPayPushLog", entity)
                                .AutoMap(o => o.SysNo)
                                .ExecuteReturnLastId<int>("SysNo");
            return entity;
        }

        /// <summary>
        /// 获取订单商品明细
        /// </summary>
        /// <param name="sysNo">订单编号</param>
        /// <returns></returns>
        /// <remarks>2017-08-14 杨浩 创建</remarks>
        public override IList<SoOrderPayPushLog> GetOrderPayPushLogList(int sysNo)
        {
            return Context.Sql("select * from SoOrderPayPushLog where OrderSysNo=@OrderSysNo")
               .Parameter("OrderSysNo", sysNo)
               .QueryMany<SoOrderPayPushLog>();
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ///  <remarks>2017-08-14 杨浩 创建</remarks>
        public override int Update(SoOrderPayPushLog entity)
        {
            return Context.Update("SoOrderPayPushLog", entity)
                .AutoMap(o => o.SysNo)
                .Where("SysNo", entity.SysNo)
                .Execute();
        }

        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="markId"></param>
        /// <returns></returns>
        /// <remarks>2017-08-15 杨浩 创建</remarks>
        public override SoOrderPayPushLog GetModel(string markId)
        {
            return Context.Sql("select * from SoOrderPayPushLog where MarkId=@MarkId")
              .Parameter("MarkId", markId)
              .QuerySingle<SoOrderPayPushLog>();
        }

        /// <summary>
        /// 获取订单支付状态不是已支付的推送日志列表
        /// </summary>
        /// <param name="paymentTypeSysNo">支付类系统编号</param>
        /// <returns></returns>
        /// <remarks>2017-08-15 杨浩 创建</remarks>
        public override IList<SoOrderPayPushLog> GetOrderNoPayLog(int paymentTypeSysNo)
        {
            return Context.Sql(@"select * from [SoOrderPayPushLog] where OrderSysNo in 
            (
                select sysno from SoOrder where PayStatus = 10
                and SysNo in
                (
                   select OrderSysNo from [SoOrderPayPushLog] where PaymentTypeSysNo = @paymentTypeSysNo
                ) 
             )")
             .Parameter("paymentTypeSysNo", paymentTypeSysNo)
            .QueryMany<SoOrderPayPushLog>();
        }
    }
}
