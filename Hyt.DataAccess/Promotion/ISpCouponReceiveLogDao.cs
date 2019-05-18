using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Hyt.DataAccess.Base;
using Hyt.Model;

namespace Hyt.DataAccess.Promotion
{
    /// <summary>
    /// 优惠券日志
    /// </summary>
    /// <remarks>2013-12-27 苟治国 创建</remarks>
    public abstract class ISpCouponReceiveLogDao : DaoBase<ISpCouponReceiveLogDao>
    {
        /// <summary>
        /// 查看优惠券日志
        /// </summary>
        /// <param name="sysNo">优惠券日志编号</param>
        /// <returns>优惠券日志</returns>
        /// <remarks>2013-12-27 苟治国 创建</remarks>
        public abstract Model.SpCouponReceiveLog GetModel(int sysNo);

        /// <summary>
        /// 插入优惠券日志
        /// </summary>
        /// <param name="model">优惠券日志实体</param>
        /// <returns>受影响行</returns>
        /// <remarks>2013-12-27 苟治国 创建</remarks>
        public abstract int Insert(Model.SpCouponReceiveLog model);

        /// <summary>
        /// 判断客户在时间段内是否领取优惠券
        /// </summary>
        /// <param name="customerSysNo">客户编号</param>
        /// <param name="benginDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <returns>是否领取</returns>
        /// <remarks>2013-12-31 苟治国 创建</remarks>
        public abstract bool HasGet(int customerSysNo, DateTime benginDate, DateTime endDate);

        /// <summary>
        /// 判断客户在时间段内是否领取特定主题的优惠券
        /// </summary>
        /// <param name="customerSysNo">客户编号</param>
        /// <param name="subjectCode">主题代码</param>
        /// <param name="benginDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <returns>是否领取</returns>
        /// <remarks>2013-12-31 苟治国 创建</remarks>
        public abstract bool HasGet(int customerSysNo, string subjectCode, DateTime benginDate, DateTime endDate);

        /// <summary>
        /// 更新优惠券日志
        /// </summary>
        /// <param name="model">优惠券日志实体</param>
        /// <returns>受影响行</returns>
        /// <remarks>2013-12-27 苟治国 创建</remarks>
        public abstract int Update(Model.SpCouponReceiveLog model);

        /// <summary>
        /// 删除优惠券日志
        /// </summary>
        /// <param name="sysNo">优惠券日志编号</param>
        /// <returns>成功返回true,失败返回false</returns>
        /// <remarks>2013-12-27 苟治国 创建</remarks>
        public abstract bool Delete(int sysNo);

        /// <summary>
        /// 判断该优惠卡是否已经绑定过该客户
        /// </summary>
        /// <param name="couponCardNo">优惠卡卡号</param>
        /// <param name="recipientSysNo">领取客户号</param>
        /// <returns>是否绑定过 t:已绑定 f:未绑定</returns>
        /// <remarks>2014-01-08 朱家宏 创建</remarks>
        public abstract bool HasGet(string couponCardNo, int recipientSysNo);

        /// <summary>
        /// 通过优惠卡号、接受人系统编号获取记录
        /// </summary>
        /// <param name="couponCardNo">优惠卡号</param>
        /// <param name="recipientSysNo">接受人系统编号</param>
        /// <returns>领取记录</returns>
        /// <remarks>2014-01-21 朱家宏 创建</remarks>
        public abstract IList<SpCouponReceiveLog> GetAll(string couponCardNo, int recipientSysNo);
    }
}
