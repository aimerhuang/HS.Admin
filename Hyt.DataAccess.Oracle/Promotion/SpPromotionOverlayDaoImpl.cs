using Hyt.Model;
using System.Collections.Generic;
using Hyt.DataAccess.Base;
using Hyt.DataAccess.Promotion;
namespace Hyt.DataAccess.Oracle.Promotion
{
    /// <summary>
    /// 促销叠加
    /// </summary>
    /// <remarks>2013-08-21  朱成果 创建</remarks>
    public class SpPromotionOverlayDaoImpl : ISpPromotionOverlayDao
    {

        #region 数据记录增，删，改，查
        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>新增记录编号</returns>
        /// <remarks>2013-08-21  朱成果 创建</remarks>
        public override int Insert(SpPromotionOverlay entity)
        {
            entity.SysNo = Context.Insert("SpPromotionOverlay", entity)
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
        public override void Update(SpPromotionOverlay entity)
        {

            Context.Update("SpPromotionOverlay", entity)
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
        public override SpPromotionOverlay GetEntity(int sysNo)
        {

            return Context.Sql("select * from SpPromotionOverlay where SysNo=@SysNo")
                   .Parameter("SysNo", sysNo)
              .QuerySingle<SpPromotionOverlay>();
        }

        /// <summary>
        /// 获取促销叠加
        /// </summary>
        /// <param name="promotionSysNo">促销系统编号</param>
        /// <returns>促销叠加列表</returns>
        /// <remarks>2013-08-21  朱成果 创建</remarks>
        public override List<SpPromotionOverlay> GetListByPromotionSysNo(int promotionSysNo)
        {
            return Context.Sql("select * from SpPromotionOverlay where OverlayCode in (select OverlayCode from SpPromotionOverlay where PromotionSysNo=@PromotionSysNo)")
                  .Parameter("PromotionSysNo", promotionSysNo)
             .QueryMany<SpPromotionOverlay>();
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns></returns>
        /// <remarks>2013-08-21  朱成果 创建</remarks>
        public override void Delete(int sysNo)
        {
            Context.Sql("Delete from SpPromotionOverlay where SysNo=@SysNo")
                 .Parameter("SysNo", sysNo)
            .Execute();
        }

        /// <summary>
        /// 根据促销系统编号删除数据
        /// </summary>
        /// <param name="promotionSysNo">促销系统编号</param>
        /// <returns></returns>
        /// <remarks>2013-08-21  余勇 创建</remarks>
        public override void DeleteByPromotionSysNo(int promotionSysNo)
        {
            Context.Sql("Delete from SpPromotionOverlay where OverlayCode=@PromotionSysNo")
               .Parameter("PromotionSysNo", promotionSysNo)
          .Execute();
        }
        #endregion

    }
}
