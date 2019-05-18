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
    public abstract class IGsSupportAreaDao : DaoBase<IGsSupportAreaDao>
    {
        /// <summary>
        /// 插入团购商品表
        /// </summary>
        /// <param name="entity">团购商品表实体</param>
        /// <returns>团购商品表实体（带编号）</returns>
        /// <remarks>2013-08-21 余勇  创建</remarks>
        public abstract GsSupportArea InsertEntity(GsSupportArea entity);

        /// <summary>
        /// 获取团购商品表
        /// </summary>
        /// <returns>团购商品表</returns>
        /// <remarks>2013-08-22 余勇 创建</remarks>
        public abstract IList<GsSupportArea> GetItem(int sysNo);

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <remarks>2013-08-21  余勇 创建</remarks>
        public abstract void Delete(int sysNo);
    }
}
