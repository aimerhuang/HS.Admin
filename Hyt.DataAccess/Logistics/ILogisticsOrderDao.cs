using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Base;
using Hyt.Model;

namespace Hyt.DataAccess.Logistics
{
    /// <summary>
    /// 订单处理相关Dao
    /// </summary>
    /// <remarks>2013-07-16 黄伟 创建</remarks>
    public abstract class ILogisticsOrderDao : DaoBase<ILogisticsOrderDao>
    {
        /// <summary>
        /// 模糊查询配送方式名返回系统编号
        /// </summary>
        /// <param name="name">配送方式名</param>
        /// <returns>系统编号</returns>
        /// <remarks>2013-07-16 黄伟 创建</remarks>
        public abstract LgDeliveryType GetDelTypeByNameLike(string name);
    }
}
