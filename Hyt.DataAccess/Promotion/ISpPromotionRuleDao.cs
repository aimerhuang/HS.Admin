using System.Collections.Generic;
using Hyt.DataAccess.Base;
using Hyt.Model;
using Hyt.Model.Parameter;
namespace Hyt.DataAccess.Promotion
{
    /// <summary>
    /// 促销规则
    /// </summary>
    /// <remarks>2013-08-21  朱成果 创建</remarks>
    public abstract class ISpPromotionRuleDao : DaoBase<ISpPromotionRuleDao>
    {

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>新增记录编号</returns>
        /// <remarks>2013-08-21  朱成果 创建</remarks>
        public abstract int Insert(SpPromotionRule entity);

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>影响的行</returns>
        /// <remarks>2013-08-21  朱成果 创建</remarks>
        public abstract int Update(SpPromotionRule entity);

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns>数据实体</returns>
        /// <remarks>2013-08-21  朱成果 创建</remarks>
        public abstract SpPromotionRule GetEntity(int sysNo);

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns></returns>
        /// <remarks>2013-08-21  朱成果 创建</remarks>
        public abstract void Delete(int sysNo);

        /// <summary>
        /// 获取促销规则列表
        /// </summary>
        /// <param name="promotionSysNo">促销系统编号</param>
        /// <returns></returns>
        /// <remarks>2013-08-21  朱成果 创建</remarks>
        public abstract List<SpPromotionRule> GetListByPromotionSysNo(int promotionSysNo);

        /// <summary>
        /// 分页获取促销规则
        /// </summary>
        /// <param name="filter">筛选条件</param>
        /// <returns>分页列表</returns>
        /// <remarks>2013-08-21 黄志勇 创建</remarks>
        public abstract Pager<SpPromotionRule> GetPromotionRule(ParaPromotionRule filter);

        /// <summary>
        /// 判断规则名称是否存在
        /// </summary>
        /// <param name="ruleName">规则名称</param>
        /// <param name="excludesysNo">排除的规则编号</param>
        /// <returns>bool</returns>
        /// <remarks>2013-08-26 朱成果 创建</remarks>
        public abstract bool ExistsRule(string ruleName, int excludesysNo);

    }
}
