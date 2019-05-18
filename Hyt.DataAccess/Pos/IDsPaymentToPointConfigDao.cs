using Hyt.DataAccess.Base;
using Hyt.Model.Pos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Pos
{
    /// <summary>
    /// 积分配置
    /// </summary>
    public abstract class IDsPaymentToPointConfigDao : DaoBase<IDsPaymentToPointConfigDao>
    {
        // <summary>
        /// 添加数据
        /// </summary>
        /// <param name="cardMod"></param>
        /// <returns></returns>
        public abstract int Insert(DsPaymentToPointConfig cardMod);
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="cardMod"></param>
        public abstract void Update(DsPaymentToPointConfig cardMod);
        /// <summary>
        /// 获取会员卡信息
        /// </summary>
        /// <param name="cardSysNo"></param>
        /// <returns></returns>
        public abstract DsPaymentToPointConfig GetDsPaymentToPointConfigBySysNo(int dsSysNo);

    }
}
