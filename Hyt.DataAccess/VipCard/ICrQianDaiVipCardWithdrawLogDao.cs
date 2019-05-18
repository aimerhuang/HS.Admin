using Hyt.DataAccess.Base;
using Hyt.Model;
using Hyt.Model.VipCard.QianDai;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.VipCard
{
    /// <summary>
    /// 会员卡提现日志
    /// </summary>
    /// <remarks>2017-03-31 杨浩 创建</remarks>
    public abstract class ICrQianDaiVipCardWithdrawLogDao : DaoBase<ICrQianDaiVipCardWithdrawLogDao>
    {
        /// <summary>
        /// 创建
        /// </summary>
        /// <returns>会员信息</returns>
        /// <remarks>2017-03-31 杨浩 创建</remarks>
        public abstract CrQianDaiVipCardWithdrawLog CreateCrQianDaiVipCardWithdrawLog(CrQianDaiVipCardWithdrawLog model);
        /// <summary>
        /// 获取提现日志
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns></returns>
        /// <remarks>2017-03-31 杨浩 创建</remarks>
        public abstract CrQianDaiVipCardWithdrawLog GetModel(int sysNo);
        /// <summary>
        /// 获取提现日志
        /// </summary>
        /// <param name="rechargeNo ">流水号</param>
        /// <returns></returns>
        /// <remarks>2017-03-31 杨浩 创建</remarks>
        public abstract CrQianDaiVipCardWithdrawLog GetQianDaiVipCardWithdraweLogByWithdrawNo(string rechargeNo);
 
        /// <summary>
        /// 更新提现日志
        /// </summary>
        /// <param name="model"></param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2017-03-31 杨浩 创建</remarks>
        public abstract int Update(CrQianDaiVipCardWithdrawLog model);

        /// <summary>
        /// 根据条件获取提现日志列表
        /// </summary>
        /// <param name="pager">提现日志查询条件</param>
        /// <returns>提现日志列表</returns>
        /// <remarks>2017-03-31 杨浩 创建</remarks>
        public abstract Pager<CrQianDaiVipCardWithdrawLog> Seach(Pager<CrQianDaiVipCardWithdrawLog> pager);

    }
}
