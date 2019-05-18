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
    public class QianDaiVipCardRechargeLogBo : BOBase<QianDaiVipCardRechargeLogBo>
    {
        /// <summary>
        /// 创建
        /// </summary>
        /// <returns>会员信息</returns>
        /// <remarks>2017-03-31 杨浩 创建</remarks>
        public  CrQianDaiVipCardRechargeLog CreateCrQianDaiVipCardRechargeLog(CrQianDaiVipCardRechargeLog model)
        {
            return ICrQianDaiVipCardRechargeLogDao.Instance.CreateCrQianDaiVipCardRechargeLog(model);
        }
        /// <summary>
        /// 获取充值日志
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns></returns>
        /// <remarks>2017-03-31 杨浩 创建</remarks>
        public  CrQianDaiVipCardRechargeLog GetModel(int sysNo)
        {
            return ICrQianDaiVipCardRechargeLogDao.Instance.GetModel(sysNo);
        }
        /// <summary>
        /// 获取充值日志
        /// </summary>
        /// <param name="rechargeNo ">充值流水号</param>
        /// <returns></returns>
        /// <remarks>2017-03-31 杨浩 创建</remarks>
        public CrQianDaiVipCardRechargeLog GetQianDaiVipCardRechargeLogByRechargeNo(string rechargeNo)
        {
            return ICrQianDaiVipCardRechargeLogDao.Instance.GetQianDaiVipCardRechargeLogByRechargeNo(rechargeNo);
        }

        /// <summary>
        /// 更新充值日志
        /// </summary>
        /// <param name="model"></param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2017-03-31 杨浩 创建</remarks>
        public  int Update(CrQianDaiVipCardRechargeLog model)
        {
            return ICrQianDaiVipCardRechargeLogDao.Instance.Update(model);
        }

        /// <summary>
        /// 根据条件获取充值日志列表
        /// </summary>
        /// <param name="pager">充值日志查询条件</param>
        /// <returns>充值日志列表</returns>
        /// <remarks>2017-03-31 杨浩 创建</remarks>
        public PagedList<CrQianDaiVipCardRechargeLog> Seach(Pager<CrQianDaiVipCardRechargeLog> pager)
        {
            var list = new PagedList<CrQianDaiVipCardRechargeLog>();
            pager.PageSize = list.PageSize;
            return ICrQianDaiVipCardRechargeLogDao.Instance.Seach(pager).Map();
        }
    }
}
