using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Base;
using Hyt.Model;
using Hyt.Model.Parameter;

namespace Hyt.DataAccess.Promotion
{
    /// <summary>
    /// 团购
    /// </summary>
    /// <remarks>2013-08-20 朱家宏 创建</remarks>
    public abstract class IGsGroupShoppingItemDao : DaoBase<IGsGroupShoppingItemDao>
    {
        /// <summary>
        /// 插入团购商品表
        /// </summary>
        /// <param name="entity">团购商品表实体</param>
        /// <returns>团购商品表实体（带编号）</returns>
        /// <remarks>2013-08-21 余勇  创建</remarks>
        public abstract GsGroupShoppingItem InsertEntity(GsGroupShoppingItem entity);

        /// <summary>
        /// 获取团购明细
        /// </summary>
        /// <returns>团购明细</returns>
        /// <remarks>2013-09-01 吴文强 创建</remarks>
        public abstract GsGroupShoppingItem GetEntity(int sysNo);

        /// <summary>
        /// 获取团购商品表
        /// </summary>
        /// <returns>团购商品列表</returns>
        /// <remarks>2013-08-22 余勇 创建</remarks>
        public abstract IList<GsGroupShoppingItem> GetItem(int sysNo);

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <remarks>2013-08-21  余勇 创建</remarks>
        public abstract void Delete(int sysNo);
    }
}
