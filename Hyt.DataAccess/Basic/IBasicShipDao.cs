using System.Collections.Generic;
using Hyt.DataAccess.Base;
using Hyt.Model;

namespace Hyt.DataAccess.BaseInfo
{
    /// <summary>
    /// 配送方式
    /// </summary>
    /// <remarks>2013-06-09 黄志勇 创建</remarks>
    public abstract class IBasicShipDao : DaoBase<IBasicShipDao>
    {
        /// <summary>
        /// 获取所有配送方式
        /// </summary>
        /// <returns>配送方式</returns>
        /// <remarks>
        /// 2013-06-09 黄志勇 创建
        /// </remarks>
        public abstract IList<LgDeliveryType> LoadAllDeliveryType();

        /// <summary>
        /// 获取配送方式信息
        /// </summary>
        /// <param name="sysNo">配送方式编号</param>
        /// <returns>配送方式信息</returns>
        /// <remarks> 2013-07-18 朱成果 创建 </remarks>
        public abstract LgDeliveryType GetEntity(int sysNo);
    }
}
