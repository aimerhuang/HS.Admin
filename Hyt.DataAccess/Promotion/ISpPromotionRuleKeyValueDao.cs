
using System.Collections.Generic;
using Hyt.DataAccess.Base;
using Hyt.Model;
using Hyt.Model.Parameter;
namespace Hyt.DataAccess.Promotion
{
    /// <summary>
    /// 促销规则值
    /// </summary>
    /// <remarks>2013-08-21  朱成果 创建</remarks>
    public abstract class ISpPromotionRuleKeyValueDao : DaoBase<ISpPromotionRuleKeyValueDao>
    {

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>新增记录编号</returns>
        /// <remarks>2013-08-21  朱成果 创建</remarks>
        public abstract int Insert(SpPromotionRuleKeyValue entity);

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns></returns>
        /// <remarks>2013-08-21  朱成果 创建</remarks>
        public abstract void Update(SpPromotionRuleKeyValue entity);

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns>数据实体</returns>
        /// <remarks>2013-08-21  朱成果 创建</remarks>
        public abstract SpPromotionRuleKeyValue GetEntity(int sysNo);


        /// <summary>
        /// 获取促销规则值列表
        /// </summary>
        /// <param name="promotionSysNo">促销系统编号</param>
        /// <returns>促销规则值列表</returns>
        /// <remarks>2013-08-21  朱成果 创建</remarks>
        public abstract List<SpPromotionRuleKeyValue> GetListByPromotionSysNo(int promotionSysNo);

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns></returns>
        /// <remarks>2013-08-21  朱成果 创建</remarks>
        public abstract void Delete(int sysNo);

        /// <summary>
        /// 根据促销系统编号删除数据
        /// </summary>
        /// <param name="promotionSysNo">促销系统编号</param>
        /// <returns></returns>
        /// <remarks>2013-08-21  朱成果 创建</remarks>
        public abstract void DeleteByPromotionSysNo(int promotionSysNo);

    }
}
