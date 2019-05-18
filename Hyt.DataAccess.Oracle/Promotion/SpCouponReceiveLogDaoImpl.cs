using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Hyt.Model;
using Hyt.DataAccess.Promotion;

namespace Hyt.DataAccess.Oracle.Promotion
{
    /// <summary>
    /// 优惠券日志
    /// </summary>
    /// <remarks>2013-12-27 苟治国 创建</remarks>
    public class SpCouponReceiveLogDaoImpl:ISpCouponReceiveLogDao
    {
        /// <summary>
        /// 查看优惠券日志
        /// </summary>
        /// <param name="sysNo">优惠券日志编号</param>
        /// <returns>优惠券日志</returns>
        /// <remarks>2013-12-27 苟治国 创建</remarks>
        public override Model.SpCouponReceiveLog GetModel(int sysNo)
        {
            return Context.Sql(@"select * from SpCouponReceiveLog where SysNO = @SysNO", sysNo).QuerySingle<Model.SpCouponReceiveLog>();
        }

        /// <summary>
        /// 插入优惠券日志
        /// </summary>
        /// <param name="model">优惠券日志实体</param>
        /// <returns>受影响行</returns>
        /// <remarks>2013-12-27 苟治国 创建</remarks>
        public override int Insert(Model.SpCouponReceiveLog model)
        {
            var result = Context.Insert<SpCouponReceiveLog>("SpCouponReceiveLog", model)
                    .AutoMap(x => x.SysNo)
                    .ExecuteReturnLastId<int>("SysNo");
            return result;
        }

        /// <summary>
        /// 判断客户在时间段内是否领取优惠券
        /// </summary>
        /// <param name="customerSysNo">客户编号</param>
        /// <param name="benginDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <returns>是否领取</returns>
        /// <remarks>2013-12-31 苟治国 创建</remarks>
        public override bool HasGet(int customerSysNo, DateTime benginDate, DateTime endDate)
        {
            var result =Context.Sql("select * from SpCouponReceiveLog where RecipientSysNo=@RecipientSysNo and (ReceiveTime>=@benginDate and ReceiveTime<=@endDate)")
                         .Parameter("RecipientSysNo", customerSysNo)
                         .Parameter("benginDate", benginDate)
                         .Parameter("endDate", endDate)
                         .QueryMany<SpCouponReceiveLog>();
            return result.Count > 0;

        }

        /// <summary>
        /// 判断客户在时间段内是否领取特定主题的优惠券
        /// </summary>
        /// <param name="customerSysNo">客户编号</param>
        /// <param name="subjectCode">主题代码</param>
        /// <param name="benginDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <returns>是否领取</returns>
        /// <remarks>2013-12-31 苟治国 创建</remarks>
        public override bool HasGet(int customerSysNo,string subjectCode, DateTime benginDate, DateTime endDate)
        {
            var result = Context.Sql("select * from SpCouponReceiveLog where RecipientSysNo=@RecipientSysNo and (ReceiveTime>=@benginDate and ReceiveTime<=@endDate) and subjectcode=@subjectcode")
                         .Parameter("RecipientSysNo", customerSysNo)
                         .Parameter("benginDate", benginDate)
                         .Parameter("endDate", endDate)
                         .Parameter("subjectcode",subjectCode)
                         .QueryMany<SpCouponReceiveLog>();
            return result.Count > 0;

        }

        /// <summary>
        /// 更新优惠券日志
        /// </summary>
        /// <param name="model">优惠券日志实体</param>
        /// <returns>受影响行</returns>
        /// <remarks>2013-12-27 苟治国 创建</remarks>
        public override int Update(Model.SpCouponReceiveLog model)
        {
            int rowsAffected = Context.Update<Model.SpCouponReceiveLog>("SpCouponReceiveLog", model)
                          .AutoMap(x => x.SysNo)
                          .Where(x => x.SysNo)
                          .Execute();
            return rowsAffected;
        }

        /// <summary>
        /// 删除优惠券日志
        /// </summary>
        /// <param name="sysNo">优惠券日志编号</param>
        /// <returns>成功返回true,失败返回false</returns>
        /// <remarks>2013-12-27 苟治国 创建</remarks>
        public override bool Delete(int sysNo)
        {
            int rowsAffected = Context.Delete("SpCouponReceiveLog")
                          .Where("sysNo", sysNo)
                          .Execute();
            return rowsAffected > 0;
        }

        /// <summary>
        /// 判断该优惠卡是否已经绑定过该客户
        /// </summary>
        /// <param name="couponCardNo">优惠卡卡号</param>
        /// <param name="recipientSysNo">领取客户号</param>
        /// <returns>是否绑定过 t:已绑定 f:未绑定</returns>
        /// <remarks>2014-01-08 朱家宏 创建</remarks>
        public override bool HasGet(string couponCardNo, int recipientSysNo)
        {
            var result = Context.Sql("select sysNo from SpCouponReceiveLog where couponCardNo=@couponCardNo and recipientSysNo=@recipientSysNo")
                         .Parameter("couponCardNo", couponCardNo)
                         .Parameter("recipientSysNo", recipientSysNo)
                         .QueryMany<SpCouponReceiveLog>();
            return result.Count > 0;
        }

        /// <summary>
        /// 通过优惠卡号、接受人系统编号获取记录
        /// </summary>
        /// <param name="couponCardNo">优惠卡号</param>
        /// <param name="recipientSysNo">接受人系统编号</param>
        /// <returns>领取记录</returns>
        /// <remarks>2014-01-21 朱家宏 创建</remarks>
        public override IList<SpCouponReceiveLog> GetAll(string couponCardNo, int recipientSysNo)
        {
            var items =
                Context.Select<SpCouponReceiveLog>("*")
                       .From("SpCouponReceiveLog")
                       .Where("couponCardNo=@couponCardNo and recipientSysNo=@recipientSysNo")
                       .Parameter("couponCardNo", couponCardNo)
                       .Parameter("recipientSysNo", recipientSysNo)
                       .QueryMany();
            return items;
        }
    }
}
