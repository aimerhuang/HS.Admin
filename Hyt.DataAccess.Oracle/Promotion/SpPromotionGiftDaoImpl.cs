using Hyt.Model;
using System.Collections.Generic;
using Hyt.DataAccess.Base;
using Hyt.DataAccess.Promotion;
using Hyt.Model.Transfer;

namespace Hyt.DataAccess.Oracle.Promotion
{
    /// <summary>
    /// 促销赠品
    /// </summary>
    /// <remarks>2013-08-21  朱成果 创建</remarks>
    public class SpPromotionGiftDaoImpl : ISpPromotionGiftDao
    {

        #region 数据记录增，删，改，查
        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>新增记录编号</returns>
        /// <remarks>2013-08-21  朱成果 创建</remarks>
        public override int Insert(SpPromotionGift entity)
        {
            entity.SysNo = Context.Insert("SpPromotionGift", entity)
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
        public override void Update(SpPromotionGift entity)
        {

            Context.Update("SpPromotionGift", entity)
                   .AutoMap(o => o.SysNo)
                   .Where("SysNo", entity.SysNo)
                   .Execute();
        }

        /// <summary>
        /// 根据促销系统编号更新已销售数量
        /// </summary>
        /// <param name="promotionSysNo">团购系统编号</param>
        /// <param name="productSysNo">团购系统编号</param>
        /// <param name="quantity">已团数量</param>
        /// <returns></returns>
        /// <remarks>2013-10-09 吴文强 创建</remarks>
        public override void UpdateUsedSaleQuantity(int promotionSysNo,int productSysNo, int quantity)
        {
            const string strSql = @"
                            update SpPromotionGift 
                            set UsedSaleQuantity = UsedSaleQuantity + @quantity 
                            where promotionSysNo = @promotionSysNo
                              and productSysNo = @productSysNo
                            ";
            Context.Sql(strSql)
                         .Parameter("quantity", quantity)
                         .Parameter("promotionSysNo", promotionSysNo)
                         .Parameter("productSysNo", productSysNo)
                         .Execute();
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns>数据实体</returns>
        /// <remarks>2013-08-21  朱成果 创建</remarks>
        public override SpPromotionGift GetEntity(int sysNo)
        {

            return Context.Sql("select * from SpPromotionGift where SysNo=@SysNo")
                   .Parameter("SysNo", sysNo)
              .QuerySingle<SpPromotionGift>();
        }

        /// <summary>
        /// 获取赠品列表
        /// </summary>
        /// <param name="promotionSysNo">促销系统编号</param>
        /// <returns>赠品列表</returns>
        /// <remarks>2013-08-21  朱成果 创建</remarks>
        public override List<CBSpPromotionGift> GetListByPromotionSysNo(int promotionSysNo)
        {

            return Context.Sql("select * from SpPromotionGift where PromotionSysNo=@PromotionSysNo")
                  .Parameter("PromotionSysNo", promotionSysNo)
            .QueryMany<CBSpPromotionGift>();
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns></returns>
        /// <remarks>2013-08-21  朱成果 创建</remarks>
        public override void Delete(int sysNo)
        {
            Context.Sql("Delete from SpPromotionGift where SysNo=@SysNo")
                 .Parameter("SysNo", sysNo)
            .Execute();
        }

        /// <summary>
        /// 根据促销系统编号删除赠品
        /// </summary>
        /// <param name="promotionSysNo">促销系统编号</param>
        /// <returns></returns>
        /// <remarks>2013-08-21  朱成果 创建</remarks>
        public override void DeleteByPromotionSysNo(int promotionSysNo)
        {
            Context.Sql("Delete from SpPromotionGift where PromotionSysNo=@PromotionSysNo")
               .Parameter("PromotionSysNo", promotionSysNo)
          .Execute();
        }
        #endregion

    }
}
