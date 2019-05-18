using Hyt.DataAccess.Base;
using Hyt.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.RMA
{
    /// <summary>
    /// 小商品退换货
    /// </summary>
    /// <remarks>2014-11-18  朱成果 创建</remarks>
    public  abstract class IRcNoReturnExchangeDao : DaoBase<IRcNoReturnExchangeDao>
    {
        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>新增记录编号</returns>
        /// <remarks>2014-11-18  朱成果 创建</remarks>
        public abstract int Insert(RcNoReturnExchange entity);

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns></returns>
        /// <remarks>2014-11-18  朱成果 创建</remarks>
        public abstract void Update(RcNoReturnExchange entity);

        /// <summary>
        /// 获取小商品退换货详情
        /// </summary>
        /// <param name="rmaid">退换货编号</param>
        /// <returns></returns>
        /// <remarks>2014-11-18  朱成果 创建</remarks>
        public abstract RcNoReturnExchange GetModelByRmaID(int rmaid);
    }
}
