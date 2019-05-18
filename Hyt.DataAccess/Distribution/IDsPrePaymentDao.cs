
using System;
using Hyt.DataAccess.Base;
using Hyt.Model;
using Hyt.Model.Parameter;

namespace Hyt.DataAccess.Distribution
{
    /// <summary>
    /// 分销商预存款
    /// </summary>
    /// <remarks>2013-09-10  杨浩 创建</remarks>
    public abstract class IDsPrePaymentDao : DaoBase<IDsPrePaymentDao>
    {
        /// <summary>
        /// 更新 预存款可用余额.
        /// </summary>
        /// <param name="dealerSysNo">分销商系统编号.</param>
        /// <param name="availableAmount">预存款可用余额.</param>
        /// <param name="operatorSysNo">操作人sysno.</param>
        /// <returns></returns>
        public abstract bool UpdateAvailableAmount(int dealerSysNo, decimal availableAmount, int operatorSysNo);
        /// <summary>
        /// 修改分销商余额提示额
        /// </summary>
        /// <param name="dealerSysNo">分销商系统编号</param>
        /// <param name="alertAmount">余额提示额</param>
        /// <returns></returns>
        public abstract bool UpdateAlertAmount(int dealerSysNo, decimal alertAmount);

        /// <summary>
        /// 添加分销商累积预存金额
        /// </summary>
        /// <param name="dealerSysNo">分销商系统编号</param>
        /// <param name="totalPrestoreAmount">预存金额</param>
        /// <param name="operatorSysNo">操作人sysno.</param>
        /// <returns></returns>
        public abstract bool AddTotalPrestoreAmount(int dealerSysNo, decimal totalPrestoreAmount, int operatorSysNo);

        /// <summary>
        /// 添加 预存款可用余额.
        /// </summary>
        /// <param name="dealerSysNo">分销商系统编号.</param>
        /// <param name="availableAmount">预存款可用余额.</param>
        /// <param name="operatorSysNo">操作人sysno.</param>
        /// <returns></returns>
        public abstract bool AddAvailableAmount(int dealerSysNo, decimal availableAmount, int operatorSysNo);

        /// <summary>
        /// 减少 预存款可用余额.
        /// </summary>
        /// <param name="dealerSysNo">分销商系统编号.</param>
        /// <param name="availableAmount">预存款可用余额.</param>
        /// <param name="operatorSysNo">操作人sysno.</param>
        /// <returns></returns>
        public abstract bool SubtractAvailableAmount(int dealerSysNo, decimal availableAmount, int operatorSysNo);


        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>新增记录编号</returns>
        /// <remarks>2013-09-10  朱成果 创建</remarks>
        public abstract int Insert(DsPrePayment entity);

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns></returns>
        /// <remarks>2016-1-6 杨浩 创建</remarks>
        public abstract int Update(DsPrePayment entity);

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns>数据实体</returns>
        /// <remarks>2013-09-10  朱成果 创建</remarks>
        public abstract DsPrePayment GetEntity(int sysNo);

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns></returns>
        /// <remarks>2013-09-10  朱成果 创建</remarks>
        public abstract void Delete(int sysNo);

        /// <summary>
        /// 根据分销商编号获取存款信息
        /// </summary>
        /// <param name="dealerSysNo">分销商编号</param>
        /// <returns>数据实体</returns>
        /// <remarks>2013-09-10  朱成果 创建</remarks>
        public abstract DsPrePayment GetEntityByDealerSysNo(int dealerSysNo);

        /// <summary>
        /// 完成商城订单，更新预付款状态
        /// </summary>
        /// <param name="hytorderid">商城订单</param>
        /// <remarks>2014-04-22  朱成果 创建</remarks>
        public abstract void CompleteOrderPrePayment(int hytorderid);
        /// <summary>
        /// 更新付款单数值
        /// </summary>
        /// <param name="SysNo"></param>
        /// <param name="Value"></param>
        /// <remarks>2016-1-7 王耀发  创建</remarks>
        public abstract void UpdatePaymentValue(int SysNo, decimal Value);
        /// <summary>
        /// 更新付款单数值
        /// </summary>
        /// <param name="SysNo"></param>
        /// <param name="Value"></param>
        /// <remarks>2016-1-7 王耀发  创建</remarks>
        public abstract void UpdatePaymentValueConfirm(int SysNo, decimal Value);
    }
}
