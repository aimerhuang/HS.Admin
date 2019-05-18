using Hyt.DataAccess.RMA;
using Hyt.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Oracle.RMA
{
    /// <summary>
    /// 小商品换货
    /// </summary>
    /// <remarks>2014-11-18  朱成果 创建</remarks>
    public class RcNoReturnExchangeDaoImpl : IRcNoReturnExchangeDao 
    {
        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>新增记录编号</returns>
        /// <remarks>2014-11-18  朱成果 创建</remarks>
        public override  int Insert(RcNoReturnExchange entity)
        {
            entity.SysNo = Context.Insert("RcNoReturnExchange", entity)
                                        .AutoMap(o => o.SysNo)
                                        .ExecuteReturnLastId<int>("SysNo");
            return entity.SysNo;
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns></returns>
        /// <remarks>2014-11-18  朱成果 创建</remarks>
        public override void Update(RcNoReturnExchange entity)
        {
            Context.Update("RcNoReturnExchange", entity)
                  .AutoMap(o => o.SysNo)
                  .Where("SysNo", entity.SysNo)
                  .Execute();
        }

        /// <summary>
        /// 获取小商品退换货详情
        /// </summary>
        /// <param name="rmaid">退换货编号</param>
        /// <returns></returns>
        /// <remarks>2014-11-18  朱成果 创建</remarks>
        public override RcNoReturnExchange GetModelByRmaID(int rmaid)
        {
            return Context.Sql("select * from RcNoReturnExchange where ReturnSysNo=@rmaid")
                   .Parameter("rmaid", rmaid)
              .QuerySingle<RcNoReturnExchange>();
        }
    }
}
