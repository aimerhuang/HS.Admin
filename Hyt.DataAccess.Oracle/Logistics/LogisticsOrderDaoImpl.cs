using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Logistics;
using Hyt.Model;

namespace Hyt.DataAccess.Oracle.Logistics
{
    /// <summary>
    /// 订单处理相关Dao
    /// </summary>
    /// <remarks>2013-07-16 黄伟 创建</remarks>
    public class LogisticsOrderDaoImpl : ILogisticsOrderDao
    {
        /// <summary>
        /// 模糊查询配送方式名返回系统编号
        /// </summary>
        /// <param name="name">配送方式名</param>
        /// <returns>系统编号</returns>
        /// <remarks>2013-07-16 黄伟 创建</remarks>
        public override LgDeliveryType GetDelTypeByNameLike(string name)
        {
            return Context.Sql(@"select * from lgdeliverytype d 
                                where d.deliverytypename like @name
                                and rownum=1")
                          .Parameter("name", string.Format("%{0}%", name))
                          .QuerySingle<LgDeliveryType>();
        }
    }
}
