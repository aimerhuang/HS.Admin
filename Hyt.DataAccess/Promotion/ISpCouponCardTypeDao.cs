
using System.Collections.Generic;
using Hyt.DataAccess.Base;
using Hyt.Model;
using Hyt.Model.Parameter;
namespace Hyt.DataAccess.Promotion
{
    /// <summary>
    /// 优惠卡类型
    /// </summary>
    /// <remarks>2014-01-08  朱成果 创建</remarks>
    public abstract class ISpCouponCardTypeDao : DaoBase<ISpCouponCardTypeDao>
    {

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>新增记录编号</returns>
        /// <remarks>2014-01-08  朱成果 创建</remarks>
        public abstract int Insert(SpCouponCardType entity);

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns></returns>
        /// <remarks>2014-01-08  朱成果 创建</remarks>
        public abstract void Update(SpCouponCardType entity);

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns>数据实体</returns>
        /// <remarks>2014-01-08  朱成果 创建</remarks>
        public abstract SpCouponCardType GetEntity(int sysNo);

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns></returns>
        /// <remarks>2014-01-08  朱成果 创建</remarks>
        public abstract void Delete(int sysNo);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="filter">筛选条件</param>
        /// <returns>分页查询数据</returns>
        /// <remarks>2014-01-08  朱成果 创建</remarks>
        public abstract Pager<SpCouponCardType> Query(ParaCouponCardType filter);

        /// <summary>
        /// 获取所有启用的优惠券卡类型
        /// </summary>
        /// <returns>优惠券卡类型集合</returns>
        /// <remarks>2014-01-08 余勇 创建</remarks>
        public abstract IList<SpCouponCardType> GetAllTypeName();

        /// <summary>
        /// 判断类型名称是否存在
        /// </summary>
        /// <param name="sysNo">类型编号</param>
        /// <param name="typeName">类型名称</param>
        /// <returns>true/false</returns>
        /// <remarks>2014-01-08  朱成果 创建</remarks>
        public abstract bool IsExistsTypeName(int sysNo,string typeName);
    }
}
