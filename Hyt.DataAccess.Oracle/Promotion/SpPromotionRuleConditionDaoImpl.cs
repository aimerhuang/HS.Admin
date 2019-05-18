using Hyt.Model;
using System.Collections.Generic;
using Hyt.DataAccess.Base;
using Hyt.DataAccess.Promotion;
namespace Hyt.DataAccess.Oracle.Promotion
{
    /// <summary>
    /// 促销规则条件
    /// </summary>
    /// <remarks>2013-08-21  朱成果 创建</remarks>
    public class SpPromotionRuleConditionDaoImpl : ISpPromotionRuleConditionDao
    {

        #region 数据记录增，删，改，查
        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>新增记录编号</returns>
        /// <remarks>2013-08-21  朱成果 创建</remarks>
        public override int Insert(SpPromotionRuleCondition entity)
        {
            entity.SysNo = Context.Insert("SpPromotionRuleCondition", entity)
                                        .AutoMap(o => o.SysNo)
                                        .ExecuteReturnLastId<int>("SysNo");
            return entity.SysNo;
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns></returns>
        /// <remarks>2013-08-21  朱成果 创建</remarks>
        public override void Update(SpPromotionRuleCondition entity)
        {

            Context.Update("SpPromotionRuleCondition", entity)
                   .AutoMap(o => o.SysNo)
                   .Where("SysNo", entity.SysNo)
                   .Execute();
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns>数据实体</returns>
        /// <remarks>2013-08-21  朱成果 创建</remarks>
        public override SpPromotionRuleCondition GetEntity(int sysNo)
        {

            return Context.Sql("select * from SpPromotionRuleCondition where SysNo=@SysNo")
                   .Parameter("SysNo", sysNo)
              .QuerySingle<SpPromotionRuleCondition>();
        }
       
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns></returns>
        /// <remarks>2013-08-21  朱成果 创建</remarks>
        public override void Delete(int sysNo)
        {
            Context.Sql("Delete from SpPromotionRuleCondition where SysNo=@SysNo")
                 .Parameter("SysNo", sysNo)
            .Execute();
        }
        /// <summary>
        /// 根据促销系统编号删除
        /// </summary>
        /// <param name="promotionSysNo">促销系统编号</param>
        /// <returns></returns>
        /// <remarks>2013-08-21  朱成果 创建</remarks>
        public override void DeleteByPromotionSysNo(int promotionSysNo)
        {

            Context.Sql("Delete from SpPromotionRuleCondition where PromotionSysNo=@PromotionSysNo")
               .Parameter("PromotionSysNo", promotionSysNo)
          .Execute();
        }
        #endregion

    }
}
