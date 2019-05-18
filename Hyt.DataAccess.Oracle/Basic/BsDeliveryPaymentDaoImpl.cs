using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Basic;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;

namespace Hyt.DataAccess.Oracle.Basic
{
    /// <summary>
    /// 配送方式支付方式关联（互斥）数据访问类
    /// </summary>
    /// <remarks>
    /// 2013-08-01 郑荣华 创建
    /// </remarks>
    public class BsDeliveryPaymentDaoImpl : IBsDeliveryPaymentDao
    {
        #region 操作

        /// <summary>
        /// 创建配送方式支付方式信息
        /// </summary>
        /// <param name="model">配送方式支付方式信息实体</param>
        /// <returns>创建的配送方式支付方式信息sysNo</returns>
        /// <remarks> 
        /// 2013-08-02 郑荣华 创建
        /// </remarks>
        public override int Create(BsDeliveryPayment model)
        {
            if (model.LastUpdateDate == DateTime.MinValue)
            {
                model.LastUpdateDate = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            }
            return Context.Insert("BsDeliveryPayment", model)
                     .AutoMap(x => x.SysNo)
                     .ExecuteReturnLastId<int>("SysNo");
        }

        /// <summary>
        /// 更新配送方式支付方式信息
        /// </summary>
        /// <param name="model">配送方式支付方式信息实体，根据sysno</param>
        /// <returns>受影响的行数</returns>
        /// <remarks> 
        /// 2013-08-02 郑荣华 创建
        /// </remarks>
        public override int Update(BsDeliveryPayment model)
        {
            return Context.Update("BsDeliveryPayment", model)
                     .AutoMap(x => x.SysNo, x => x.CreatedBy, x => x.CreatedDate)
                     .Where(x => x.SysNo)
                     .Execute();
        }

        /// <summary>
        /// 删除配送方式支付方式信息
        /// </summary>
        /// <param name="sysNo">要删除的配送方式支付方式信息系统编号</param>
        /// <returns>受影响的行数</returns>
        /// <remarks> 
        /// 2013-08-02 郑荣华 创建
        /// </remarks>
        public override int Delete(int sysNo)
        {
            return Context.Delete("BsDeliveryPayment")
                        .Where("SysNo", sysNo)
                        .Execute();
        }

        /// <summary>
        /// 删除配送方式支付方式关联信息
        /// </summary>
        /// <param name="paymentSysNo">支付方式系统编号</param>
        /// <param name="deliverySysNo">配送方式系统编号</param>
        /// <returns>受影响的行数</returns>
        /// <remarks> 
        /// 2013-08-02 郑荣华 创建
        /// </remarks>
        public override int Delete(int paymentSysNo, int deliverySysNo)
        {
            return Context.Delete("BsDeliveryPayment")
                          .Where("paymentSysNo", paymentSysNo)
                          .Where("deliverySysNo", deliverySysNo)
                          .Execute();
            //const string sql = @"delete from BsDeliveryPayment where paymentSysNo=@0 and deliverySysNo=@1";

            //return Context.Sql(sql, paymentSysNo, deliverySysNo)
            //              .Execute();
        }
        #endregion

        #region 查询

        /// <summary>
        /// 查询配送方式支付方式关联信息
        /// </summary>
        /// <param name="pager">配送方式支付方式关联列表分页对象</param>
        /// <param name="filter">配送方式支付方式关联查询条件</param>
        /// <returns></returns>
        /// <remarks>
        /// 2013-08-02 郑荣华 创建
        /// </remarks>
        public override void GetBsDeliveryPaymentList(ref Pager<CBBsDeliveryPayment> pager, ParaBsDeliveryPaymentFilter filter)
        {
            const string whereSql = @"(@paymentsysno is null or t.paymentsysno=@paymentsysno)
                                       and (@deliverysysno is null or t.deliverysysno=@deliverysysno )
                                       and (@parentsysno is null or b.parentsysno=@parentsysno)
                                       and (@paymenttype is null or a.paymenttype=@paymenttype)
                                       and b.parentsysno<>0 "; //去掉第一级配送方式

            const string sqlcount = @"select count(1) from BsDeliveryPayment t 
                                      left join bspaymenttype a on t.paymentsysno=a.sysno
                                      left join lgdeliverytype b on t.deliverysysno=b.sysno
                                      where " + whereSql;

            const string fromSql = @"BsDeliveryPayment t 
                                     left join bspaymenttype a on t.paymentsysno=a.sysno
                                     left join lgdeliverytype b on t.deliverysysno=b.sysno
                                     left join lgdeliverytype c on b.parentsysno=c.sysno";

            using (var context = Context.UseSharedConnection(true))
            {
                pager.TotalRows = context.Sql(sqlcount)
                                         .Parameter("paymentsysno", filter.PaymentSysNo)
                                         //.Parameter("paymentsysno", filter.PaymentSysNo)
                                         .Parameter("deliverysysno", filter.DeliverySysNo)
                                         //.Parameter("deliverysysno", filter.DeliverySysNo)
                                         .Parameter("parentsysno", filter.ParentSysNo)
                                         //.Parameter("parentsysno", filter.ParentSysNo)
                                         .Parameter("paymenttype", filter.PaymentType)
                                         //.Parameter("paymenttype", filter.PaymentType)
                                         .QuerySingle<int>();

                pager.Rows = context.Select<CBBsDeliveryPayment>(@"t.*,a.paymentname,a.paymenttype,a.isonlinepay,
                                     b.parentsysno,b.deliverytypename,c.deliverytypename parentname")
                                    .From(fromSql)
                                    .Where(whereSql)
                                    .Parameter("paymentsysno", filter.PaymentSysNo)
                                    //.Parameter("paymentsysno", filter.PaymentSysNo)
                                    .Parameter("deliverysysno", filter.DeliverySysNo)
                                    //.Parameter("deliverysysno", filter.DeliverySysNo)
                                    .Parameter("parentsysno", filter.ParentSysNo)
                                    //.Parameter("parentsysno", filter.ParentSysNo)
                                    .Parameter("paymenttype", filter.PaymentType)
                                    //.Parameter("paymenttype", filter.PaymentType)
                                    .OrderBy("t.deliverysysno,t.PaymentSysNo")
                                    .Paging(pager.CurrentPage, pager.PageSize)
                                    .QueryMany();
            }
        }

        /// <summary>
        /// 根据支付方式查询配送方式支付方式关联列表信息
        /// </summary>
        /// <param name="paymentSysNo">支付方式系统编号</param>
        /// <returns>配送方式支付方式关联列表</returns>
        /// <remarks>
        /// 2013-08-02 郑荣华 创建
        /// </remarks>
        public override IList<CBBsDeliveryPayment> GetListByPayment(int paymentSysNo)
        {
            const string sql = @"select t.*,a.paymentname,a.paymenttype,a.isonlinepay,b.parentsysno,b.deliverytypename 
                                 from BsDeliveryPayment t 
                                 left join bspaymenttype a on t.paymentsysno=a.sysno
                                 left join lgdeliverytype b on t.deliverysysno=b.sysno
                                 where t.paymentsysno=@0";

            return Context.Sql(sql, paymentSysNo)
                          .QueryMany<CBBsDeliveryPayment>();
        }

        /// <summary>
        /// 根据配送方式查询配送方式支付方式关联列表信息
        /// </summary>
        /// <param name="deliverySysNo">配送方式系统编号</param>
        /// <returns>配送方式支付方式关联列表</returns>
        /// <remarks>
        /// 2013-08-02 郑荣华 创建
        /// </remarks>
        public override IList<CBBsDeliveryPayment> GetListByDelivery(int deliverySysNo)
        {
            const string sql = @"select t.*,a.paymentname,a.paymenttype,a.isonlinepay,b.parentsysno,b.deliverytypename 
                                 from BsDeliveryPayment t 
                                 left join bspaymenttype a on t.paymentsysno=a.sysno
                                 left join lgdeliverytype b on t.deliverysysno=b.sysno
                                 where t.deliverysysno=@0";

            return Context.Sql(sql, deliverySysNo)
                          .QueryMany<CBBsDeliveryPayment>();
        }

        /// <summary>
        /// 配送和支付方式匹配条数
        /// </summary>
        /// <param name="paymentSysNo">支付方式编号</param>
        /// <param name="deliverySysNo">配送方式系统编号</param>
        /// <returns>配送和支付方式匹配条数</returns>
        /// <remarks>2013-09-12 黄志勇 创建</remarks>
        public override int GetBsDeliveryPaymentCount(int paymentSysNo, int deliverySysNo)
        {
            const string sql = @"select count(0) from BsDeliveryPayment                                  
                                 where PaymentSysNo=@paymentSysNo and DeliverySysNo=@deliverySysNo";

            return Context.Sql(sql)
                          .Parameter("paymentSysNo", paymentSysNo)
                          .Parameter("deliverySysNo", deliverySysNo)
                          .QuerySingle<int>();
        }
        #endregion
    }
}
