using System.Collections.Generic;
using Hyt.DataAccess.Base;
using Hyt.Model;
using Hyt.Model.Parameter;
namespace Hyt.DataAccess.Promotion
{
    /// <summary>
    /// 促销规则条件
    /// </summary>
    /// <remarks>2013-08-21  朱成果 创建</remarks>
    public abstract class ISpPromotionRuleConditionDao : DaoBase<ISpPromotionRuleConditionDao>
    {

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>新增记录编号</returns>
        /// <remarks>2013-08-21  朱成果 创建</remarks>
        public abstract int Insert(SpPromotionRuleCondition entity);

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns></returns>
        /// <remarks>2013-08-21  朱成果 创建</remarks>
        public abstract void Update(SpPromotionRuleCondition entity);

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns>数据实体</returns>
        /// <remarks>2013-08-21  朱成果 创建</remarks>
        public abstract SpPromotionRuleCondition GetEntity(int sysNo);
       
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns></returns>
        /// <remarks>2013-08-21  朱成果 创建</remarks>
        public abstract void Delete(int sysNo);

        /// <summary>
        /// 根据促销系统编号删除
        /// </summary>
        /// <param name="promotionSysNo">促销系统编号</param>
        /// <returns></returns>
        /// <remarks>2013-08-21  朱成果 创建</remarks>
        public abstract void DeleteByPromotionSysNo(int promotionSysNo);

    }
}
