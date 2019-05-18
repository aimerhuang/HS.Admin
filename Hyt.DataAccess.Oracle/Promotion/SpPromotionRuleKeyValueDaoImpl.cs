

using Hyt.Model;
using System.Collections.Generic;
using Hyt.DataAccess.Base;
using Hyt.DataAccess.Promotion;
namespace Hyt.DataAccess.Oracle.Promotion
{
    /// <summary>
    /// 促销规则值
    /// </summary>
    /// <remarks>2013-08-21  朱成果 创建</remarks>
    public class SpPromotionRuleKeyValueDaoImpl : ISpPromotionRuleKeyValueDao
    {

        #region 数据记录增，删，改，查
        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>新增记录编号</returns>
        /// <remarks>2013-08-21  朱成果 创建</remarks>
        public override int Insert(SpPromotionRuleKeyValue entity)
        {
            entity.SysNo = Context.Insert("SpPromotionRuleKeyValue", entity)
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
        public override void Update(SpPromotionRuleKeyValue entity)
        {

            Context.Update("SpPromotionRuleKeyValue", entity)
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
        public override SpPromotionRuleKeyValue GetEntity(int sysNo)
        {

            return Context.Sql("select * from SpPromotionRuleKeyValue where SysNo=@SysNo")
                   .Parameter("SysNo", sysNo)
              .QuerySingle<SpPromotionRuleKeyValue>();
        }

        /// <summary>
        /// 获取促销规则值列表
        /// </summary>
        /// <param name="promotionSysNo">促销系统编号</param>
        /// <returns></returns>
        /// <remarks>2013-08-21  朱成果 创建</remarks>
        public override List<SpPromotionRuleKeyValue> GetListByPromotionSysNo(int promotionSysNo)
        {
            return Context.Sql("select * from SpPromotionRuleKeyValue where PromotionSysNo=@PromotionSysNo")
                .Parameter("PromotionSysNo", promotionSysNo)
                .QueryMany<SpPromotionRuleKeyValue>();
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns></returns>
        /// <remarks>2013-08-21  朱成果 创建</remarks>
        public override void Delete(int sysNo)
        {
            Context.Sql("Delete from SpPromotionRuleKeyValue where SysNo=@SysNo")
                 .Parameter("SysNo", sysNo)
            .Execute();
        }

        /// <summary>
        /// 根据促销系统编号删除数据
        /// </summary>
        /// <param name="promotionSysNo">促销系统编号</param>
        /// <returns></returns>
        /// <remarks>2013-08-21  朱成果 创建</remarks>
        public override void DeleteByPromotionSysNo(int promotionSysNo)
        {
            Context.Sql("Delete from SpPromotionRuleKeyValue where PromotionSysNo=@PromotionSysNo")
               .Parameter("PromotionSysNo", promotionSysNo)
          .Execute();
        }
        #endregion

    }
}
