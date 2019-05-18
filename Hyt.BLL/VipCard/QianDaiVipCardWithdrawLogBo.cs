using Hyt.DataAccess.VipCard;
using Hyt.Infrastructure.Pager;
using Hyt.Model;
using Hyt.Model.VipCard.QianDai;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.BLL.VipCard
{
    /// <summary>
    /// 钱袋宝会员卡
    /// </summary>
    /// <remarks>2017-03-31 杨浩 创建</remarks>
    public class QianDaiVipCardWithdrawLogBo : BOBase<QianDaiVipCardWithdrawLogBo>
    {
        /// <summary>
        /// 创建
        /// </summary>
        /// <returns>会员信息</returns>
        /// <remarks>2017-03-31 杨浩 创建</remarks>
        public CrQianDaiVipCardWithdrawLog CreateCrQianDaiVipCardWithdrawLog(CrQianDaiVipCardWithdrawLog model)
        {
            return ICrQianDaiVipCardWithdrawLogDao.Instance.CreateCrQianDaiVipCardWithdrawLog(model);
        }
        /// <summary>
        /// 获取充值日志
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns></returns>
        /// <remarks>2017-03-31 杨浩 创建</remarks>
        public CrQianDaiVipCardWithdrawLog GetModel(int sysNo)
        {
            return ICrQianDaiVipCardWithdrawLogDao.Instance.GetModel(sysNo);
        }
        /// <summary>
        /// 获取充值日志
        /// </summary>
        /// <param name="rechargeNo ">充值流水号</param>
        /// <returns></returns>
        /// <remarks>2017-03-31 杨浩 创建</remarks>
        public CrQianDaiVipCardWithdrawLog GetQianDaiVipCardWithdrawLogByWithdrawNo(string rechargeNo)
        {
            return ICrQianDaiVipCardWithdrawLogDao.Instance.GetQianDaiVipCardWithdraweLogByWithdrawNo(rechargeNo);
        }

        /// <summary>
        /// 更新提现日志
        /// </summary>
        /// <param name="model"></param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2017-03-31 杨浩 创建</remarks>
        public int Update(CrQianDaiVipCardWithdrawLog model)
        {
            return ICrQianDaiVipCardWithdrawLogDao.Instance.Update(model);
        }

        /// <summary>
        /// 根据条件获取提现日志列表
        /// </summary>
        /// <param name="pager">充值日志查询条件</param>
        /// <returns>充值日志列表</returns>
        /// <remarks>2017-03-31 杨浩 创建</remarks>
        public PagedList<CrQianDaiVipCardWithdrawLog> Seach(Pager<CrQianDaiVipCardWithdrawLog> pager)
        {
            var list = new PagedList<CrQianDaiVipCardWithdrawLog>();
            pager.PageSize = list.PageSize;
            return ICrQianDaiVipCardWithdrawLogDao.Instance.Seach(pager).Map();
        }
    }
}
