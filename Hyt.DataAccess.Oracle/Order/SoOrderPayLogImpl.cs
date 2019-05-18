using Hyt.DataAccess.Order;
using Hyt.Model.Generated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Oracle.Order
{
    /// <summary>
    /// 订单支付日志
    /// </summary>
    /// <remarks>2017-04-2 杨浩 创建</remarks>
    public class SoOrderPayLogImpl : ISoOrderPayLogDao
    {
        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="SysNo">系统编号</param>
        /// <returns></returns>
        ///<remarks>2017-04-2 杨浩 创建</remarks>
        public override SoOrderPayLog GetEntity(int sysNo)
        {
            return Context.Sql("select * from SoOrderPayLog where sysno=@0", sysNo)
                .QuerySingle<SoOrderPayLog>();
        }
        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="SysNo">订单编号</param>
        /// <param name="paymentTypeSysNo">支付类型</param>
        /// <returns></returns>
        ///<remarks>2017-04-2 杨浩 创建</remarks>
        public override SoOrderPayLog GetOrderPayLogByOrderSysNo(int orderSysNo, int paymentTypeSysNo)
        {
            string sqlWhere = "";
            if (paymentTypeSysNo > 0)
                sqlWhere += " and paymentTypeSysNo=" + paymentTypeSysNo;

            return Context.Sql(string.Format("select top 1 * from SoOrderPayLog where orderSysNo={0} {1} ", orderSysNo, sqlWhere))
                .QuerySingle<SoOrderPayLog>();
        }

        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="submitOrderNumber">提交单号</param>
        /// <param name="paymentTypeSysNo">支付类型</param>
        /// <returns></returns>
        ///<remarks>2017-04-2 杨浩 创建</remarks>
        public override SoOrderPayLog GetOrderPayLogBySubmitOrderNumber(int submitOrderNumber, int paymentTypeSysNo)
        {
            string sqlWhere = "";
            if (paymentTypeSysNo > 0)
                sqlWhere += " and paymentTypeSysNo=" + paymentTypeSysNo;

            return Context.Sql(string.Format("select top 1 * from SoOrderPayLog where submitOrderNumber={0} {1}", submitOrderNumber, sqlWhere))
               .QuerySingle<SoOrderPayLog>();
        }

        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="entity">销售单主表实体</param>
        /// <returns>订单支付日志实体（带编号）</returns>
        /// <remarks>2017-04-2 杨浩 创建</remarks>
        public override SoOrderPayLog InsertEntity(SoOrderPayLog entity)
        {
            entity.SysNo = Context.Insert("SoOrderPayLog", entity)
                                .AutoMap(o => o.SysNo)
                                .ExecuteReturnLastId<int>("SysNo");
            return entity;
        }
        /// <summary>
        /// 更新状态值
        /// </summary>
        /// <param name="orderSysNo">销售单编号</param>
        /// <param name="paymentTypeSysNo">支付类型</param>
        /// <param name="status">状态值</param>
        /// <returns></returns>
        /// <remarks>2017-04-2 杨浩 创建</remarks>
        public override bool UpdateOrderPayLogStatus(int orderSysNo, int paymentTypeSysNo, int status)
        {
           var r = Context.Sql("update SoOrderPayLog set status=@status where orderSysNo=@orderSysNo and paymentTypeSysNo=@paymentTypeSysNo")
                  .Parameter("status", status)
                  .Parameter("orderSysNo", orderSysNo)
                  .Parameter("paymentTypeSysNo", paymentTypeSysNo)
                  .Execute();

           return (r > 0);
        }

        /// <summary>
        /// 更新订单支付日志
        /// </summary>
        /// <param name="soOrder">订单支付日志实体</param>
        /// <returns>t:成功 f:失败</returns>
        /// <remarks>2017-04-2 杨浩 创建</remarks>
        public override bool Update(SoOrderPayLog entity)
        {
            var r = Context.Update("SoOrderPayLog", entity)
                          .AutoMap(o => o.SysNo)
                          .Where("SysNo",entity.SysNo)
                          .Execute();
            return (r > 0);
        }


    }
}
