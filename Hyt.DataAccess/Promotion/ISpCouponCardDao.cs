using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Base;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;

namespace Hyt.DataAccess.Promotion
{
    /// <summary>
    /// 优惠卡卡号
    /// </summary>
    /// <remarks>2014-01-08  朱家宏 创建</remarks>
    public abstract class ISpCouponCardDao : DaoBase<ISpCouponCardDao>
    {
        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>新增记录编号</returns>
        /// <remarks>2014-01-08  朱家宏 创建</remarks>
        public abstract int Insert(SpCouponCard entity);

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>空</returns>
        /// <remarks>2014-01-08  朱家宏 创建</remarks>
        public abstract void Update(SpCouponCard entity);

        /// <summary>
        /// 获取单条记录
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns>数据实体</returns>
        /// <remarks>2014-01-08  朱家宏 创建</remarks>
        public abstract SpCouponCard Get(int sysNo);

        /// <summary>
        /// 根据优惠卡号获取单条记录
        /// </summary>
        /// <param name="couponCardNo">优惠卡号码</param>
        /// <returns>数据实体</returns>
        /// <remarks>2014-01-08  朱家宏 创建</remarks>
        public abstract SpCouponCard Get(string couponCardNo);

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns>空</returns>
        /// <remarks>2014-01-08  朱家宏 创建</remarks>
        public abstract void Delete(int sysNo);


        /// <summary>
        /// 分页获取优惠券卡号
        /// </summary>
        /// <param name="filter">筛选条件</param>
        /// <returns>分页列表</returns>
        /// <remarks>2014-01-08 余勇 创建</remarks>
        public abstract Pager<CBSpCouponCard> GetCouponCard(ParaCouponCard filter);

        /// <summary>
        /// 更新优惠券卡号状态
        /// </summary>
        /// <param name="sysNo">优惠券卡编号</param>
        /// <param name="status">状态</param>
        /// <returns>返回操作行</returns>
        /// <remarks>2014-01-08 余勇 创建</remarks>
        public abstract int UpdateCouponCardStatus(int sysNo, int status);

        /// <summary>
        /// 获取所有优惠券卡号
        /// </summary>
        /// <returns>优惠券卡号集合</returns>
        /// <remarks>2014-01-08 余勇 创建</remarks>
        public abstract IList<SpCouponCard> GetAllSpCouponCard();

        /// <summary>
        /// 新增优惠券卡号
        /// </summary>
        /// <param name="models">优惠券卡号列表</param>
        /// <returns>空</returns>
        /// <remarks>2014-01-08 余勇 创建</remarks>
        public abstract void CreateSpCouponCard(List<SpCouponCard> models);

        /// <summary>
        /// 更新优惠券卡号
        /// </summary>
        /// <param name="models">优惠券卡号列表</param>
        /// <returns>空</returns>
        /// <remarks>2014-01-08 余勇 创建</remarks>
        public abstract void UpdateSpCouponCard(List<SpCouponCard> models);
    }
}
