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
    public abstract class IGsGroupShoppingDao : DaoBase<IGsGroupShoppingDao>
    {
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="filter">查询参数</param>
        /// <returns>分页</returns>
        /// <remarks>2013-08-20 朱家宏 创建</remarks>
        public abstract Pager<GsGroupShopping> Query(ParaGroupShoppingFilter filter);

        /// <summary>
        /// 插入团购表
        /// </summary>
        /// <param name="entity">团购表实体</param>
        /// <returns>团购表实体（带编号）</returns>
        /// <remarks>2013-08-21 余勇  创建</remarks>
        public abstract GsGroupShopping InsertEntity(GsGroupShopping entity);

        /// <summary>
        /// 获取团购的覆盖地区
        /// </summary>
        /// <param name="groupShoppingSysNo">团购编号</param>
        /// <returns>团购的覆盖地区列表</returns>
        /// <remarks>2013-08-22 余勇 创建</remarks>
        public abstract IList<BsArea> GetAreaByGroupShoppingSysNo(int groupShoppingSysNo);

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="model">数据实体</param>
        /// <returns></returns>
        /// <remarks>2013-08-21  余勇 创建</remarks>
        public abstract void Update(GsGroupShopping model);

        /// <summary>
        /// 根据团购系统编号更新已团数量
        /// </summary>
        /// <param name="sysNo">团购系统编号</param>
        /// <param name="quantity">已团数量</param>
        /// <returns></returns>
        /// <remarks>2013-10-09 吴文强 创建</remarks>
        public abstract void UpdateHaveQuantity(int sysNo, int quantity);

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns></returns>
        /// <remarks>2013-10-09 吴文强 创建</remarks>
        public abstract GsGroupShopping Get(int sysNo);

        /// <summary>
        /// 根据商品系统编号获取团购信息
        /// </summary>
        /// <param name="productSysNo">组合套餐明细系统编号</param>
        /// <returns>团购信息集合</returns>
        /// <remarks>2013-09-06 吴文强 创建</remarks>
        public abstract IList<GsGroupShopping> GetGroupShoppingByProductSysNo(int productSysNo);
    }
}
