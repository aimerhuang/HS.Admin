

using System;
using Hyt.Model;
using System.Collections.Generic;
using Hyt.DataAccess.Base;
using Hyt.DataAccess.Distribution;
namespace Hyt.DataAccess.Oracle.Distribution
{
    /// <summary>
    /// 分销商预存款主表
    /// </summary>
    /// <remarks>2013-09-10  朱成果 创建</remarks>
    public class DsPrePaymentDaoImpl : IDsPrePaymentDao
    {

        #region 数据记录增，删，改，查
        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>新增记录编号</returns>
        /// <remarks>2013-09-10  朱成果 创建</remarks>
        public override int Insert(DsPrePayment entity)
        {
            entity.SysNo = Context.Insert("DsPrePayment", entity)
                                        .AutoMap(o => o.SysNo)
                                        .ExecuteReturnLastId<int>("SysNo");
            return entity.SysNo;
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>影响的行</returns>
        /// <remarks>2016-1-7 杨浩 创建</remarks>
        public override int Update(DsPrePayment entity)
        {

            return Context.Update("DsPrePayment", entity)
                    .AutoMap(o => o.SysNo)
                    .Where("SysNo", entity.SysNo)
                    .Execute();
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns>数据实体</returns>
        /// <remarks>2013-09-10  朱成果 创建</remarks>
        public override DsPrePayment GetEntity(int sysNo)
        {

            return Context.Sql("select * from DsPrePayment where SysNo=@SysNo")
                   .Parameter("SysNo", sysNo)
              .QuerySingle<DsPrePayment>();
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns></returns>
        /// <remarks>2013-09-10  朱成果 创建</remarks>
        public override void Delete(int sysNo)
        {
            Context.Sql("Delete from DsPrePayment where SysNo=@SysNo")
                 .Parameter("SysNo", sysNo)
            .Execute();
        }
        #endregion

        /// <summary>
        /// 根据分销商编号获取存款信息
        /// </summary>
        /// <param name="dealerSysNo">分销商编号</param>
        /// <returns>数据实体</returns>
        /// <remarks>2013-09-10  朱成果 创建</remarks>
        public override DsPrePayment GetEntityByDealerSysNo(int dealerSysNo)
        {

            return Context.Sql("select * from DsPrePayment where DealerSysNo=@DealerSysNo")
                .Parameter("DealerSysNo", dealerSysNo)
           .QuerySingle<DsPrePayment>();
        }

        /// <summary>
        /// 完成商城订单，更新预付款状态
        /// </summary>
        /// <param name="hytorderid">商城订单</param>
        /// <remarks>2014-04-22  朱成果 创建</remarks>
        public override void CompleteOrderPrePayment(int hytorderid)
        {
            Context.StoredProcedure("Proc_PrePayment_Complete") //执行存储过程
                .Parameter("v_hytorder", hytorderid)
                .Execute();

        }

        /// <summary>
        /// 修改分销商余额提示额
        /// </summary>
        /// <param name="dealerSysNo">分销商系统编号</param>
        /// <param name="alertAmount">余额提示额</param>
        /// <returns></returns>
        public override bool UpdateAlertAmount(int dealerSysNo, decimal alertAmount)
        {
            var res = Context.Update("DsPrePayment")
                                .Column("AlertAmount", alertAmount)
                                .Where("DealerSysNo", dealerSysNo)
                                .Execute();
            return res > 0;
        }
        /// <summary>
        /// 更新 预存款可用余额.
        /// </summary>
        /// <param name="dealerSysNo">分销商系统编号.</param>
        /// <param name="availableAmount">预存款可用余额.</param>
        /// <param name="operatorSysNo">操作人sysno.</param>
        /// <returns></returns>
        public override bool UpdateAvailableAmount(int dealerSysNo, decimal availableAmount, int operatorSysNo)
        {
            var res = Context.Sql(@"update DsPrePayment set AvailableAmount = (AvailableAmount+@0),FrozenAmount=(FrozenAmount-@0),LastUpdateBy=@1,LastUpdateDate=@2
                                        where DealerSysNo=@3", Math.Abs(availableAmount), operatorSysNo, DateTime.Now, dealerSysNo).Execute();
            return res > 0;
        }
        /// <summary>
        /// 添加分销商累积预存金额
        /// </summary>
        /// <param name="dealerSysNo">分销商系统编号</param>
        /// <param name="totalPrestoreAmount">预存金额</param>
        /// <param name="operatorSysNo">操作人sysno.</param>
        /// <returns>true或false</returns>
        public override bool AddTotalPrestoreAmount(int dealerSysNo, decimal totalPrestoreAmount, int operatorSysNo)
        {
            var res = Context.Sql(@"update DsPrePayment set TotalPrestoreAmount = (TotalPrestoreAmount+@0),
                                        LastUpdateBy=@1,LastUpdateDate=@2
                                        where DealerSysNo=@3",Math.Abs(totalPrestoreAmount), operatorSysNo, DateTime.Now, dealerSysNo).Execute();
            return res > 0;
        }

        /// <summary>
        /// 添加 预存款可用余额.
        /// </summary>
        /// <param name="dealerSysNo">分销商系统编号.</param>
        /// <param name="availableAmount">预存款可用余额.</param>
        /// <param name="operatorSysNo">操作人sysno.</param>
        /// <returns>true或false</returns>
        public override bool AddAvailableAmount(int dealerSysNo, decimal availableAmount, int operatorSysNo)
        {
            var res = Context.Sql(@"update DsPrePayment set AvailableAmount = (AvailableAmount+@0),LastUpdateBy=@1,LastUpdateDate=@2
                                        where DealerSysNo=@3", Math.Abs(availableAmount), operatorSysNo, DateTime.Now, dealerSysNo).Execute();
            return res > 0;
        }

        /// <summary>
        /// 减少 预存款可用余额.
        /// </summary>
        /// <param name="dealerSysNo">分销商系统编号.</param>
        /// <param name="availableAmount">预存款可用余额.</param>
        /// <param name="operatorSysNo">操作人sysno.</param>
        /// <returns>true或false</returns>
        public override bool SubtractAvailableAmount(int dealerSysNo, decimal availableAmount, int operatorSysNo)
        {
            var res = Context.Sql(@"update DsPrePayment set AvailableAmount = (AvailableAmount-@0),LastUpdateBy=@1,LastUpdateDate=@2
                                        where AvailableAmount >=@3
                                        and DealerSysNo=@4", Math.Abs(availableAmount), operatorSysNo,
                DateTime.Now, Math.Abs(availableAmount), dealerSysNo).Execute();
            return res > 0;
        }
        /// <summary>
        /// 更新付款单数值
        /// </summary>
        /// <param name="SysNo"></param>
        /// <param name="Value"></param>
        /// <remarks>2016-1-7 王耀发  创建</remarks>
        public override void UpdatePaymentValue(int SysNo, decimal Value)
        {
            Context.Sql("Update DsPrePayment set AvailableAmount = AvailableAmount + @Value,FrozenAmount = FrozenAmount - @Value where SysNo=@SysNo")
                   .Parameter("Value", Value)
                   .Parameter("SysNo", SysNo).Execute();
        }

        /// <summary>
        /// 更新付款单数值
        /// </summary>
        /// <param name="SysNo"></param>
        /// <param name="Value"></param>
        /// <remarks>2016-1-7 王耀发  创建</remarks>
        public override void UpdatePaymentValueConfirm(int SysNo, decimal Value)
        {
            Context.Sql("Update DsPrePayment set FrozenAmount = FrozenAmount - @Value where SysNo=@SysNo")
                   .Parameter("Value", Value)
                   .Parameter("SysNo", SysNo).Execute();
        }
    }
}
