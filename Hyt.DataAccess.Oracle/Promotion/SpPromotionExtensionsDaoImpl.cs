using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Promotion;

namespace Hyt.DataAccess.Oracle.Promotion
{
    /// <summary>
    /// 促销扩展数据
    /// </summary>
    /// <remarks>2014-03-05 吴文强 创建</remarks>
    public class SpPromotionExtensionsDaoImpl : ISpPromotionExtensionsDao
    {
        /// <summary>
        /// 促销在一定时间范围内使用的次数
        /// </summary>
        /// <param name="customerSysno">客户系统编号</param>
        /// <param name="startTime">查询开始时间</param>
        /// <param name="promotionSysNo">促销系统编号</param>
        /// <returns>促销使用次数</returns>
        /// <remarks>2014-03-05 吴文强 创建</remarks>
        public override int UsedPromotionNum(int customerSysno, DateTime startTime, int promotionSysNo)
        {
            const string strSql = @"
                                select count(1) 
                                from SoOrderItem oi
                                 left join SoOrder o
                                    on oi.ordersysno = o.sysno
                                where o.customersysno = :customersysno
                                  and oi.usedpromotions is not null
                                  and o.CreateDate >= :CreateDate
                                  and exists (select 1 from splitstr(oi.usedpromotions,';') tmp where tmp.col = :promotionSysNo)
                                ";

            var result = Context.Sql(strSql)
                         .Parameter("customersysno", customerSysno)
                         .Parameter("CreateDate", startTime)
                         .Parameter("promotionSysNo", promotionSysNo.ToString())
                         .QuerySingle<int>();

            return result;
        }
    }
}
