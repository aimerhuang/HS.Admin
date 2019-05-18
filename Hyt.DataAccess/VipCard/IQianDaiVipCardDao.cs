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
    /// 钱袋宝会员卡
    /// </summary>
    /// <remarks>2017-03-31 杨浩 创建</remarks>
    public abstract class IQianDaiVipCardDao : DaoBase<IQianDaiVipCardDao>
    {
        /// <summary>
        /// 获取会员详细信息
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>会员信息</returns>
        /// <remarks>2017-03-31 杨浩 创建</remarks>
        public abstract CrQianDaiVipCard CreateVipCard(CrQianDaiVipCard model);
        /// <summary>
        /// 获取会员详细信息
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>会员信息</returns>
        /// <remarks>2017-03-31 杨浩 创建</remarks>
        public abstract CrQianDaiVipCard GetModel(int sysNo);
        /// <summary>
        /// 获取会员详细信息
        /// </summary>
        /// <param name="CustomerSysNo">客户系统编号</param>
        /// <returns>会员信息</returns>
        /// <remarks>2017-03-31 杨浩 创建</remarks>
        public abstract CrQianDaiVipCard GetVipCardByCustomerSysNo(int customerSysNo);
        /// <summary>
        /// 获取会员详细信息
        /// </summary>
        /// <param name="cardId">客钱袋宝会员卡编号</param>
        /// <returns>会员信息</returns>
        /// <remarks>2017-03-31 杨浩 创建</remarks>
        public abstract CrQianDaiVipCard GetVipCardByCardId(int cardId);
        /// <summary>
        /// 更新会员
        /// </summary>
        /// <param name="model"></param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2017-03-31 杨浩 创建</remarks>
        public abstract int Update(CrQianDaiVipCard model);

        /// <summary>
        /// 根据条件获取会员列表
        /// </summary>
        /// <param name="pager">会员查询条件</param>
        /// <returns>会员列表</returns>
        /// <remarks>2017-03-31 杨浩 创建</remarks>
        public abstract Pager<CrQianDaiVipCard> Seach(Pager<CrQianDaiVipCard> pager);
        /// <summary>
        /// 获取会员卡信息列表
        /// </summary>
        /// <param name="customerSysNos">客户系统编号多个逗号分隔</param>
        /// <returns></returns>
        /// <remarks>2017-04-01 杨浩 创建</remarks>
        public abstract IList<CrQianDaiVipCard> GetList(string customerSysNos);
    }
}
