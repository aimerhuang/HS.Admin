using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Hyt.Model;
using Hyt.DataAccess.Promotion;

namespace Hyt.BLL.Promotion
{
    /// <summary>
    /// 优惠券日志业务
    /// </summary>
    /// <remarks>2013-12-27 苟治国 创建</remarks>
    public class SpCouponReceiveLogBo : BOBase<SpCouponReceiveLogBo>
    {
        /// <summary>
        /// 查看优惠券日志
        /// </summary>
        /// <param name="sysNo">优惠券日志编号</param>
        /// <returns>优惠券日志</returns>
        /// <remarks>2013-12-27 苟治国 创建</remarks>
        public Model.SpCouponReceiveLog GetModel(int sysNo)
        {
            return ISpCouponReceiveLogDao.Instance.GetModel(sysNo);
        }

        /// <summary>
        /// 插入优惠券日志
        /// </summary>
        /// <param name="model">优惠券日志实体</param>
        /// <returns>受影响行</returns>
        /// <remarks>2013-12-27 苟治国 创建</remarks>
        public int Insert(Model.SpCouponReceiveLog model)
        {
            return ISpCouponReceiveLogDao.Instance.Insert(model);
        }

        /// <summary>
        /// 判断客户在时间段内是否领取优惠券
        /// </summary>
        /// <param name="customerSysNo">客户编号</param>
        /// <param name="benginDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <returns>是否领取</returns>
        /// <remarks>2013-12-31 黄波 创建</remarks>
        public bool HasGet(int customerSysNo, DateTime benginDate, DateTime endDate)
        {
            return ISpCouponReceiveLogDao.Instance.HasGet(customerSysNo, benginDate, endDate);
        }

        /// <summary>
        /// 判断客户在某天内是否领取优惠券
        /// </summary>
        /// <param name="customerSysNo">客户编号</param>
        /// <param name="subjectCode">主题代码</param>
        /// <param name="nowDate">当前时间</param>
        /// <returns>是否领取</returns>
        /// <remarks>2013-12-31 黄波 创建</remarks>
        public bool HasGet(int customerSysNo, string subjectCode, DateTime nowDate)
        {
            return ISpCouponReceiveLogDao.Instance.HasGet(customerSysNo, subjectCode, DateTime.Parse(nowDate.ToString("yyyy-MM-dd 0:0:0")), DateTime.Parse(nowDate.AddDays(1).ToString("yyyy-MM-dd 0:0:0")));
        }

        /// <summary>
        /// 更新优惠券日志
        /// </summary>
        /// <param name="model">优惠券日志实体</param>
        /// <returns>受影响行</returns>
        /// <remarks>2013-12-27 苟治国 创建</remarks>
        public int Update(Model.SpCouponReceiveLog model)
        {
            return ISpCouponReceiveLogDao.Instance.Update(model);
        }

        /// <summary>
        /// 删除优惠券日志
        /// </summary>
        /// <param name="sysNo">优惠券日志编号</param>
        /// <returns>成功返回true,失败返回false</returns>
        /// <remarks>2013-12-27 苟治国 创建</remarks>
        public bool Delete(int sysNo)
        {
            return ISpCouponReceiveLogDao.Instance.Delete(sysNo);
        }
    }
}
