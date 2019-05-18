
using System.Collections.Generic;
using Hyt.DataAccess.Base;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
namespace Hyt.DataAccess.Promotion
{
    /// <summary>
    /// 优惠卡关联
    /// </summary>
    /// <remarks>2014-01-08  朱成果 创建</remarks>
    public abstract class ISpCouponCardAssociateDao : DaoBase<ISpCouponCardAssociateDao>
    {

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>新增记录编号</returns>
        /// <remarks>2014-01-08  朱成果 创建</remarks>
        public abstract int Insert(SpCouponCardAssociate entity);

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns></returns>
        /// <remarks>2014-01-08  朱成果 创建</remarks>
        public abstract void Update(SpCouponCardAssociate entity);

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns>数据实体</returns>
        /// <remarks>2014-01-08  朱成果 创建</remarks>
        public abstract SpCouponCardAssociate GetEntity(int sysNo);

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns></returns>
        /// <remarks>2014-01-08  朱成果 创建</remarks>
        public abstract void Delete(int sysNo);

        /// <summary>
        /// 根据优惠卡类型编号删除
        /// </summary>
        /// <param name="cardTypeSysNo">优惠卡类型编号</param>
        /// <remarks>2014-01-08  朱成果 创建</remarks>
        public abstract void DeleteByCardTypeSysNo(int cardTypeSysNo);

        /// <summary>
        /// 通过优惠卡类型编号获取数据
        /// </summary>
        /// <param name="cardTypeSysNo">优惠卡类型编号</param>
        /// <returns>列表</returns>
        /// <remarks>2014-01-08  朱家宏 创建</remarks>
        public abstract IList<SpCouponCardAssociate> GetAllByCardTypeSysNo(int cardTypeSysNo);

        /// <summary>
        /// 获取优惠卡类型关联的优惠券信息
        /// </summary>
        /// <param name="cardTypeSysNo">优惠卡类型编号</param>
        /// <returns>列表</returns>
        /// <remarks>2014-01-09  朱成果 创建</remarks>
        public abstract List<CBSpCouponCardAssociate> GetListByCardTypeSysNo(int cardTypeSysNo);
    }
}
