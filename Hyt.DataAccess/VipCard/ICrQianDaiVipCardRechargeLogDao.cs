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
    /// 会员卡充值日志
    /// </summary>
    /// <remarks>2017-03-31 杨浩 创建</remarks>
    public abstract class ICrQianDaiVipCardRechargeLogDao : DaoBase<ICrQianDaiVipCardRechargeLogDao>
    {
        /// <summary>
        /// 创建
        /// </summary>
        /// <returns>会员信息</returns>
        /// <remarks>2017-03-31 杨浩 创建</remarks>
        public abstract CrQianDaiVipCardRechargeLog CreateCrQianDaiVipCardRechargeLog(CrQianDaiVipCardRechargeLog model);
        /// <summary>
        /// 获取充值日志
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns></returns>
        /// <remarks>2017-03-31 杨浩 创建</remarks>
        public abstract CrQianDaiVipCardRechargeLog GetModel(int sysNo);
        /// <summary>
        /// 获取充值日志
        /// </summary>
        /// <param name="rechargeNo ">充值流水号</param>
        /// <returns></returns>
        /// <remarks>2017-03-31 杨浩 创建</remarks>
        public abstract CrQianDaiVipCardRechargeLog GetQianDaiVipCardRechargeLogByRechargeNo(string rechargeNo);
 
        /// <summary>
        /// 更新充值日志
        /// </summary>
        /// <param name="model"></param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2017-03-31 杨浩 创建</remarks>
        public abstract int Update(CrQianDaiVipCardRechargeLog model);

        /// <summary>
        /// 根据条件获取充值日志列表
        /// </summary>
        /// <param name="pager">充值日志查询条件</param>
        /// <returns>充值日志列表</returns>
        /// <remarks>2017-03-31 杨浩 创建</remarks>
        public abstract Pager<CrQianDaiVipCardRechargeLog> Seach(Pager<CrQianDaiVipCardRechargeLog> pager);

    }
}
