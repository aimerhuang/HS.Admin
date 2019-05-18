

using Hyt.Model;
using System.Collections.Generic;
using Hyt.DataAccess.Base;
using Hyt.DataAccess.Promotion;
using Hyt.Model.Transfer;
namespace Hyt.DataAccess.Oracle.Promotion
{
    /// <summary>
    /// 优惠卡关联
    /// </summary>
    /// <remarks>2014-01-08  朱成果 创建</remarks>
    public class SpCouponCardAssociateDaoImpl : ISpCouponCardAssociateDao
    {
        #region 数据记录增，删，改，查
        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>新增记录编号</returns>
        /// <remarks>2014-01-08  朱成果 创建</remarks>
        public override int Insert(SpCouponCardAssociate entity)
        {
            entity.SysNo = Context.Insert("SpCouponCardAssociate", entity)
                                        .AutoMap(o => o.SysNo)
                                        .ExecuteReturnLastId<int>("SysNo");
            return entity.SysNo;
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns></returns>
        /// <remarks>2014-01-08  朱成果 创建</remarks>
        public override void Update(SpCouponCardAssociate entity)
        {

            Context.Update("SpCouponCardAssociate", entity)
                   .AutoMap(o => o.SysNo)
                   .Where("SysNo", entity.SysNo)
                   .Execute();
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns>数据实体</returns>
        /// <remarks>2014-01-08  朱成果 创建</remarks>
        public override SpCouponCardAssociate GetEntity(int sysNo)
        {

            return Context.Sql("select * from SpCouponCardAssociate where SysNo=@SysNo")
                   .Parameter("SysNo", sysNo)
              .QuerySingle<SpCouponCardAssociate>();
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns></returns>
        /// <remarks>2014-01-08  朱成果 创建</remarks>
        public override void Delete(int sysNo)
        {
            Context.Sql("Delete from SpCouponCardAssociate where SysNo=@SysNo")
                 .Parameter("SysNo", sysNo)
            .Execute();
        }

        /// <summary>
        /// 通过优惠卡类型编号获取数据
        /// </summary>
        /// <param name="cardTypeSysNo">优惠卡类型编号</param>
        /// <returns>列表</returns>
        /// <remarks>2014-01-08  朱家宏 创建</remarks>
        public override IList<SpCouponCardAssociate> GetAllByCardTypeSysNo(int cardTypeSysNo)
        {
            var items =
                Context.Select<SpCouponCardAssociate>("*")
                       .From("SpCouponCardAssociate")
                       .Where("cardTypeSysNo=@cardTypeSysNo")
                       .Parameter("cardTypeSysNo", cardTypeSysNo)
                       .QueryMany();
            return items;
        }

        #endregion

        /// <summary>
        /// 根据优惠卡类型编号删除
        /// </summary>
        /// <param name="cardTypeSysNo">优惠卡类型编号</param>
        /// <returns></returns>
        /// <remarks>2014-01-08  朱成果 创建</remarks>
        public override void DeleteByCardTypeSysNo(int cardTypeSysNo)
        {
            string sql = "delete from SpCouponCardAssociate where CardTypeSysNo=@CardTypeSysNo";
            Context.Sql(sql).Parameter("CardTypeSysNo", cardTypeSysNo).Execute();

        }

        /// <summary>
        /// 获取优惠卡类型关联的优惠券信息
        /// </summary>
        /// <param name="cardTypeSysNo">优惠卡类型编号</param>
        /// <returns>列表</returns>
        /// <remarks>2014-01-09  朱成果 创建</remarks>
        public override List<CBSpCouponCardAssociate> GetListByCardTypeSysNo(int cardTypeSysNo)
        {

            string sql = @"select t0.*,t1.couponcode,t1.couponamount,t1.description
                                from SpCouponCardAssociate t0
                                inner join SpCoupon t1 
                                on t0.couponsysno=t1.sysno 
                                where  t0.cardtypesysno=@cardTypeSysNo";
            return Context.Sql(sql).Parameter("cardTypeSysNo", cardTypeSysNo).QueryMany<CBSpCouponCardAssociate>();
        }

    }
}
