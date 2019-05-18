using System;
using System.Collections.Generic;
using Hyt.DataAccess.Base;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;

namespace Hyt.DataAccess.Promotion
{
    /// <summary>
    /// 优惠券
    /// </summary>
    /// <remarks>2013-08-21  朱成果 创建</remarks>
    public abstract class ISpCouponDao : DaoBase<ISpCouponDao>
    {

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>新增记录编号</returns>
        /// <remarks>2013-08-21  朱成果 创建</remarks>
        public abstract int Insert(SpCoupon entity);

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>影响的行</returns>
        /// <remarks>2013-08-21  朱成果 创建</remarks>
        public abstract int Update(SpCoupon entity);

        /// <summary>
        /// 根据优惠券代码更新优惠券已使用数量
        /// </summary>
        /// <param name="couponCode">优惠券代码</param>
        /// <returns></returns>
        /// <remarks>2013-10-09 吴文强 创建</remarks>
        public abstract void UpdateUsedQuantity(string couponCode);

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns>数据实体</returns>
        /// <remarks>2013-08-21  朱成果 创建</remarks>
        public abstract SpCoupon GetEntity(int sysNo);

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns></returns>
        /// <remarks>2013-08-21  朱成果 创建</remarks>
        public abstract void Delete(int sysNo);

        /// <summary>
        /// 分页获取优惠券
        /// </summary>
        /// <param name="filter">筛选条件</param>
        /// <returns>分页列表</returns>
        /// <remarks>2013-08-21 黄志勇 创建</remarks>
        public abstract Pager<CBSpCoupon> GetCoupon(ParaCoupon filter);

        /// <summary>
        /// 获取优惠券
        /// </summary>
        /// <param name="couponCode">优惠券代码</param>
        /// <returns></returns>
        /// <remarks>2013-08-27 黄志勇 创建</remarks>
        public abstract SpCoupon GetCoupon(string couponCode);

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns>数据实体</returns>
        /// <remarks>2013-08-27  黄志勇 创建</remarks>
        public abstract CBSpCoupon GetCoupon(int sysNo);

        /// <summary>
        /// 根据客户系统编号获取客户所有优惠券信息
        /// </summary>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="status">优惠券状态(Null:所有)</param>
        /// <param name="platformType">使用平台</param>
        /// <returns>优惠券信息集合</returns>
        /// <remarks>2013-08-30 吴文强 创建</remarks>
        public abstract IList<SpCoupon> GetCustomerCoupons(int customerSysNo, PromotionStatus.优惠券状态 status, PromotionStatus.促销使用平台[] platformType);

        /// <summary>
        /// 获取优惠卷信息分页方法
        /// </summary>
        /// <param name="pageIndex">起始页</param>
        /// <param name="pageSize">每页数量</param>
        /// <param name="customerSysNo">用户系统号</param>
        /// <param name="status">优惠券状态</param>
        /// <param name="nowTime">起始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="type">优惠券状态</param>
        /// <param name="count">总数</param>
        /// <returns>优惠券列表</returns>
        /// <remarks>2013-09-16 杨晗 创建</remarks>
        public abstract IList<SpCoupon> Seach(int pageIndex, int pageSize, int customerSysNo,
                                              PromotionStatus.优惠券状态? status,
                                              DateTime? nowTime, DateTime? endTime, int type, out int count);

        /// <summary>
        /// 根据优惠券代码获取优惠券
        /// </summary>
        /// <param name="couponCode">优惠券代码</param>
        /// <returns>优惠券信息</returns>
        /// <remarks>2014-03-28 唐永勤 创建</remarks>
        public abstract SpCoupon GetSpCouponByCouponCode(string couponCode);


        /// <summary>
        /// 根据客户系统编号获取客户所有优惠券信息(已经优惠卡号)
        /// </summary>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="status">优惠券状态(Null:所有)</param>
        /// <param name="platformType">使用平台</param>
        /// <returns>优惠券信息集合</returns>
        /// <remarks>2014-06-18 朱成果 创建</remarks>
        public abstract IList<CBSpCoupon> GetCustomerCouponsWithCard(int customerSysNo, PromotionStatus.优惠券状态 status, PromotionStatus.促销使用平台[] platformType);

    }
}
