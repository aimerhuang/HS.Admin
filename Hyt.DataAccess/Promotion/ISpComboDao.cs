using System.Collections.Generic;
using Hyt.DataAccess.Base;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;

namespace Hyt.DataAccess.Promotion
{
    /// <summary>
    /// 组合套餐
    /// </summary>
    /// <remarks>2013-08-21  朱成果 创建</remarks>
    public abstract class ISpComboDao : DaoBase<ISpComboDao>
    {

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>新增记录编号</returns>
        /// <remarks>2013-08-21  朱成果 创建</remarks>
        public abstract int Insert(SpCombo entity);
        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entity"数据表实体</param>
        /// <returns>数据表实体（带编号）</returns>
        /// <remarks>2016-1-15 王耀发 创建</remarks>
        public abstract SpCombo InsertEntity(SpCombo entity);
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns></returns>
        /// <remarks>2013-08-21  朱成果 创建</remarks>
        public abstract void Update(SpCombo entity);


        /// <summary>
        /// 根据组合套餐系统编号更新已销售数
        /// </summary>
        /// <param name="sysNo">组合套餐系统编号</param>
        /// <param name="quantity">使用套餐数量</param>
        /// <returns></returns>
        /// <remarks>2013-10-09 吴文强 创建</remarks>
        public abstract void UpdateSaleQuantity(int sysNo, int quantity);

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns>数据实体</returns>
        /// <remarks>2013-08-21  朱成果 创建</remarks>
        public abstract SpCombo GetEntity(int sysNo);

        /// <summary>
        /// 获取促销对应的组合套餐列表
        /// </summary>
        /// <param name="promotionSysNo">促销系统编号</param>
        /// <returns>促销对应的组合套餐列表</returns>
        /// <remarks>2013-08-21  朱成果 创建</remarks>
        public abstract List<SpCombo> GetListByPromotionSysNo(int promotionSysNo);

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns></returns>
        /// <remarks>2013-08-21  朱成果 创建</remarks>
        public abstract void Delete(int sysNo);


        /// <summary>
        /// 根据组主商品系统编号获取组合套餐信息
        /// </summary>
        /// <param name="productSysNo">组合套餐明细系统编号</param>
        /// <returns>组合套餐集合</returns>
        /// <remarks>2013-09-06 吴文强 创建</remarks>
        public abstract IList<SpCombo> GetComboByMasterProductSysNo(int productSysNo);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="filter">查询参数</param>
        /// <returns>分页</returns>
        /// <remarks>2016-1-15 王耀发 创建</remarks>
        public abstract Pager<CBSpCombo> Query(ParaSpComboFilter filter);
    }
}
