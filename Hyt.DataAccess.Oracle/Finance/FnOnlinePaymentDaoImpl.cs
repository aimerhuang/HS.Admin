using System;
using System.Linq;
using Hyt.DataAccess.Finance;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using System.Collections.Generic;

namespace Hyt.DataAccess.Oracle.Finance
{
    /// <summary>
    /// 网上支付
    /// </summary>
    /// <remarks>2013-07-16 朱家宏 创建</remarks>
    public class FnOnlinePaymentDaoImpl : IFnOnlinePaymentDao
    {
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="filter">查询参数</param>
        /// <returns>分页</returns>
        /// <remarks>2013-07-16 朱家宏 创建</remarks>
        public override Pager<CBFnOnlinePayment> GetAll(ParaOnlinePaymentFilter filter)
        {
            const string sql =
                   @"(select a.sysno,a.sourcesysno,a.amount,b.paymentname,a.voucherno,a.createddate,a.status,a.BusinessOrderSysNo,
                            0 as TotalAmount,a.CreatedBy 
                            from FnOnlinePayment a 
                            left join BsPaymentType b on a.paymenttypesysno=b.sysno
                    where   
                            (@0 is null or a.sourcesysno=@0) and                                                                                                   --OrderSysNo
                            (@1 is null or exists (select 1 from splitstr(@1,',') tmp where tmp.col = a.PaymentTypeSysNo)) and       --支付类型                                                                                         --RmaType
                            (@2 is null or a.CreatedDate>=@2) and                                                                                                    --创建日期(起)
                            (@3 is null or a.CreatedDate<@3)                                                                                                             --创建日期(止) 
                    ) tb";

            var paymentTypeSysNos = filter.PaymentTypeSysNos != null ? string.Join(",", filter.PaymentTypeSysNos) : null;

            //查询日期上限+1
            filter.EndDate = filter.EndDate == null ? (DateTime?)null : filter.EndDate.Value.AddDays(1);

            var paras = new object[]
                {
                    filter.OrderSysNo,        //  filter.OrderSysNo,
                    paymentTypeSysNos,        //  paymentTypeSysNos,
                    filter.BeginDate,         //  filter.BeginDate,
                    filter.EndDate//,             filter.EndDate
                };

            var dataList = Context.Select<CBFnOnlinePayment>("tb.*").From(sql);
            var dataCount = Context.Select<int>("count(0)").From(sql);
            var dataSum = Context.Select<decimal>("sum(amount)").From(sql);

            dataList.Parameters(paras);
            dataCount.Parameters(paras);
            dataSum.Parameters(paras);

            var pager = new Pager<CBFnOnlinePayment>
            {
                PageSize = filter.PageSize,
                CurrentPage = filter.Id
            };

            pager.TotalRows = dataCount.QuerySingle();
            pager.Rows = dataList.OrderBy("tb.CreatedDate desc").Paging(pager.CurrentPage, filter.PageSize).QueryMany();

            if (pager.Rows.Any())
            {
                pager.Rows.First().TotalAmount = dataSum.QuerySingle();
            }

            return pager;
        }

        /// <summary>
        /// 插入记录
        /// </summary>
        /// <param name="model">插入对象</param>
        /// <returns>sysNo</returns>
        /// <remarks>2013-07-16 朱家宏 创建</remarks>
        public override int Insert(FnOnlinePayment model)
        {
            if (model.OperatedDate == DateTime.MinValue)
            {
                model.OperatedDate = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            }
            var sysNo = Context.Insert("FnOnlinePayment", model)
                             .AutoMap(o => o.SysNo)
                             .ExecuteReturnLastId<int>("SysNo");
            return sysNo;
        }

        /// <summary>
        /// 获取网上支付详情
        /// </summary>
        /// <param name="voucherNo">交易凭证</param>
        /// <returns></returns>
        /// <remarks>2015-12-26 杨浩 创建</remarks>
        public override FnOnlinePayment GetOnlinePaymentByVoucherNo(string voucherNo)
        {
            return Context.Sql("select [SysNo],[Source],[SourceSysNo],[Amount],[PaymentTypeSysNo],[VoucherNo],[BusinessOrderSysNo],[Status],[CreatedBy],[CreatedDate],[Operator],[OperatedDate],[LastUpdateBy],[LastUpdateDate] from FnOnlinePayment where voucherNo=@voucherNo")
                  .Parameter("voucherNo", voucherNo)
                  .QuerySingle<FnOnlinePayment>();
        }

        /// <summary>
        /// 获取网上支付详情
        /// </summary>
        /// <param name="paymentTypeSysNo">支付类型编号</param>
        /// <param name="voucherNo">交易凭证</param>
        /// <returns></returns>
        /// <remarks>2016-09-10 杨浩 创建</remarks>
        public override FnOnlinePayment GetOnlinePaymentByVoucherNo(int paymentTypeSysNo, string voucherNo)
        {
            return Context.Sql("select [SysNo],[Source],[SourceSysNo],[Amount],[PaymentTypeSysNo],[VoucherNo],[BusinessOrderSysNo],[Status],[CreatedBy],[CreatedDate],[Operator],[OperatedDate],[LastUpdateBy],[LastUpdateDate] from FnOnlinePayment where voucherNo=@voucherNo and PaymentTypeSysNo=@PaymentTypeSysNo")
                .Parameter("voucherNo", voucherNo)
                .Parameter("PaymentTypeSysNo",paymentTypeSysNo)
                .QuerySingle<FnOnlinePayment>();
        }
        /// <summary>
        /// 获取网上支付详情
        /// </summary>
        /// <param name="SourceSysNo">源单号</param>
        /// <returns></returns>
        /// <remarks>2016-4-19 王耀发 创建</remarks>
        public override FnOnlinePayment GetOnlinePaymentBySourceSysNo(int SourceSysNo)
        {
            return Context.Sql("select [SysNo],[Source],[SourceSysNo],[Amount],[PaymentTypeSysNo],[VoucherNo],[Status],[BusinessOrderSysNo],[CreatedBy],[CreatedDate],[Operator],[OperatedDate],[LastUpdateBy],[LastUpdateDate] from FnOnlinePayment where SourceSysNo=@SourceSysNo")
                  .Parameter("SourceSysNo", SourceSysNo)
                  .QuerySingle<FnOnlinePayment>();
        }

        /// <summary>
        /// 通过订单来源和订单编号获取在线付款单数据
        /// </summary>
        /// <param name="Source">网上订单来源</param>
        /// <param name="SourceSysNo">订单编号</param>
        /// <returns>
        /// 2016-04-19 杨云奕 添加
        /// </returns>
        public override FnOnlinePayment GetOnlinePaymentBySourceSysNo(Model.WorkflowStatus.FinanceStatus.网上支付单据来源 Source, int SourceSysNo)
        {
            return Context.Sql("select * from FnOnlinePayment where Source=@Source and  SourceSysNo=@SourceSysNo ")
                  .Parameter("Source", (int)Source)
                  .Parameter("SourceSysNo", SourceSysNo)
                  .QuerySingle<FnOnlinePayment>();
        }

        /// <summary>
        /// 获取订单的在线支付记录
        /// </summary>
        /// <param name="orderSysNo"></param>
        /// <returns>sysNo</returns>
        /// <remarks>2014-05-13 何方 创建</remarks>
        public override IList<FnOnlinePayment> Get(int orderSysNo)
        {
            return Context.Sql(@"select * from FNONLINEPAYMENT t where t.source=10 and t.sourcesysno=@0  ", orderSysNo).QueryMany<FnOnlinePayment>();
        }

        /// <summary>
        /// 获取网上支付详情
        /// </summary>
        /// <param name="SourceSysNo">源单号</param>
        /// <returns></returns>
        /// <remarks>2016-4-19 王耀发 创建</remarks>
        public override CBFnOnlinePayment GetOnPaymentBySourceSysNo(int SourceSysNo)
        {
            return Context.Sql(@"select fn.*,pt.PaymentName,pt.CusPaymentCode from FnOnlinePayment fn left join bsPaymentType pt on fn.PaymentTypeSysNo = pt.SysNo where fn.SourceSysNo=@SourceSysNo")
                  .Parameter("SourceSysNo", SourceSysNo)
                  .QuerySingle<CBFnOnlinePayment>();
        }
        /// <summary>
        /// 在线支付列表
        /// </summary>
        /// <param name="SysNos">编号集合</param>
        /// <returns></returns>
        public override List<CBFnOnlinePayment> GetOnlinePaymentList(string SysNos)
        {
            string sql = " select FnOnlinePayment.*,BsPaymentType.PaymentName from FnOnlinePayment inner join BsPaymentType on FnOnlinePayment.PaymentTypeSysNo=BsPaymentType.SysNo where SourceSysNo in (" + SysNos + ") ";
            return Context.Sql(sql).QueryMany<CBFnOnlinePayment>();
        }
    }
}
