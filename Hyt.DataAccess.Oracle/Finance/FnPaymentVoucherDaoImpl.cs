using System;
using System.Collections.Generic;
using Hyt.DataAccess.Finance;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;

namespace Hyt.DataAccess.Oracle.Finance
{

    /// <summary>
    /// 付款单dao
    /// </summary>
    /// <remarks>
    /// 2013-07-12 何方 创建
    /// </remarks>
    public class FnPaymentVoucherDaoImpl : IFnPaymentVoucherDao
    {
        /// <summary>
        /// 创建付款单
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>付款单系统编号</returns>
        /// <remarks>
        /// 2013/7/12 何方 创建
        /// </remarks>
        public override int Insert(FnPaymentVoucher model)
        {
            var sysNo = Context.Insert("FnPaymentVoucher", model)
                                      .AutoMap(o => o.SysNo)
                                      .ExecuteReturnLastId<int>("SysNo");
            return sysNo;
        }

        /// <summary>
        /// 获取付款单详情(不包括明细)
        /// </summary>
        /// <param name="sysNo">付款单编号</param>
        /// <returns>付款单详情(不包括明细)</returns>
        /// <remarks>2013-7-15 朱成果 创建 </remarks>
        public override CBFnPaymentVoucher GetEntity(int sysNo)
        {
            return Context.Sql("select * from FnPaymentVoucher where SysNo=@SysNo")
                    .Parameter("SysNo", sysNo)
                    .QuerySingle<CBFnPaymentVoucher>();
        }

        /// <summary>
        /// 根据单据来源获取实体
        /// </summary>
        /// <param name="source">单据来源</param>
        /// <param name="sourceSysNo">单据编号</param>
        /// <returns>根据单据来源获取实体</returns>
        /// <remarks>2013-11-8 朱成果 创建 </remarks>
        public override FnPaymentVoucher GetEntity(int source, int sourceSysNo)
        {
            return Context.Sql("select * from FnPaymentVoucher where source=@source and sourceSysNo=@sourceSysNo")
                  .Parameter("source", source)
                  .Parameter("sourceSysNo", sourceSysNo)
                  .QuerySingle<FnPaymentVoucher>();

        }

        /// <summary>
        /// 创建付款单明细
        /// </summary>
        /// <param name="model">付款单明细</param>
        /// <returns>付款单明细系统编号</returns>
        /// <remarks>
        /// 2013-7-12 何方 创建
        /// 2013-07-19 朱成果 修改
        /// </remarks>
        public override int InsertItem(FnPaymentVoucherItem model)
        {
            var sysNo = Context.Insert("FnPaymentVoucherItem", model)
                                        .AutoMap(o => o.SysNo)
                                        .ExecuteReturnLastId<int>("SysNo");
            return sysNo;
        }

        /// <summary>
        /// 以TransactionSysNo获取实体
        /// </summary>
        /// <param name="transactionSysNo">事务编号</param>
        /// <returns>付款单</returns>
        /// <remarks>2013-7-17 朱家宏 创建</remarks>
        public override FnPaymentVoucher GetEntityByTransactionSysNo(string transactionSysNo)
        {
            return Context.Sql("select * from FnPaymentVoucher where transactionSysNo=@transactionSysNo")
                    .Parameter("transactionSysNo", transactionSysNo)
                    .QuerySingle<FnPaymentVoucher>();
        }

        /// <summary>
        /// 根据单据来源获取实体
        /// </summary>
        /// <param name="source">单据来源</param>
        /// <param name="sourceSysNo">单据编号</param>
        /// <returns>付款单</returns>
        /// <remarks>2013-7-25 朱家宏 创建 </remarks>
        public override CBFnPaymentVoucher GetEntityByVoucherSource(int source, int sourceSysNo)
        {
            return Context.Sql("select * from FnPaymentVoucher where source=@source and sourceSysNo=@sourceSysNo")
                    .Parameter("source", source)
                    .Parameter("sourceSysNo", sourceSysNo)
                    .QuerySingle<CBFnPaymentVoucher>();
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="filter">查询参数</param>
        /// <returns>分页</returns>
        /// <remarks>2013-07-19 朱家宏 创建</remarks>
        /// <remarks>2013-07-24 黄志勇 修改</remarks>
        public override Pager<CBPaymentVoucher> GetAll(ParaVoucherFilter filter)
        {
            const string sql =
                   @"(select a.*
                            from FnPaymentVoucher a
                    where   
                            (@0 is null or a.Source=@0) and                       --单据来源
                            (@1 is null or a.Status=@1) and                       --状态
                            (@2 is null or a.CreatedDate>=@2) and           --创建日期(起)
                            (@3 is null or a.CreatedDate<@3) and                --创建日期(止) 
                            (@4 is null or a.SourceSysNo=@4)            --单据来源编号 
                    ) tb";

            //查询日期上限+1
            filter.EndDate = filter.EndDate == null ? (DateTime?)null : filter.EndDate.Value.AddDays(1);

            var paras = new object[]
                {
                    filter.Source,   //   filter.Source,
                    filter.Status,    //  filter.Status,
                    filter.BeginDate,  // filter.BeginDate,
                    filter.EndDate,    // filter.EndDate,
                    filter.SourceSysNo//, filter.SourceSysNo
                };

            var dataList = Context.Select<CBPaymentVoucher>("tb.*").From(sql);
            var dataCount = Context.Select<int>("count(0)").From(sql);

            dataList.Parameters(paras);
            dataCount.Parameters(paras);

            var pager = new Pager<CBPaymentVoucher>
            {
                PageSize = filter.PageSize,
                CurrentPage = filter.Id
            };

            pager.TotalRows = dataCount.QuerySingle();
            pager.Rows = dataList.OrderBy("tb.SysNo desc").Paging(pager.CurrentPage, filter.PageSize).QueryMany();

            return pager;
        }

        /// <summary>
        /// 获取付款明细
        /// </summary>
        /// <param name="paymentVoucherSysNo">付款单编号</param>
        /// <returns>付款明细</returns>
        /// <remarks>2013-7-19 朱成果 创建 </remarks>
        public override List<FnPaymentVoucherItem> GetVoucherItems(int paymentVoucherSysNo)
        {
            return Context.Sql("select * from FnPaymentVoucherItem where PaymentVoucherSysNo=@PaymentVoucherSysNo order by CreatedDate desc")
                .Parameter("PaymentVoucherSysNo", paymentVoucherSysNo).QueryMany<FnPaymentVoucherItem>();
        }

        /// <summary>
        /// 获取付款单明细
        /// </summary>
        /// <param name="sysNo">明细编号</param>
        /// <returns>付款单明细</returns>
        /// <remarks>2013-7-22 朱成果 创建 </remarks>
        public override FnPaymentVoucherItem GetVoucherItem(int sysNo)
        {
            return Context.Sql("select * from FnPaymentVoucherItem where SysNo=@SysNo")
                  .Parameter("SysNo", sysNo)
             .QuerySingle<FnPaymentVoucherItem>();
        }

        /// <summary>
        /// 更新付款单
        /// </summary>
        /// <param name="entity">付款单</param>
        /// <returns></returns>
        /// <remarks>2013-07-22  黄志勇 创建</remarks>
        public override void UpdateVoucher(FnPaymentVoucher entity)
        {
            Context.Update("FnPaymentVoucher", entity)
               .AutoMap(o => o.SysNo)
               .Where("SysNo", entity.SysNo)
               .Execute();
        }

        /// <summary>
        /// 更新付款单明细
        /// </summary>
        /// <param name="entity">付款单明细</param>
        /// <returns></returns>
        /// <remarks>2013-07-22  朱成果 创建</remarks>
        public override void UpdateVoucherItem(FnPaymentVoucherItem entity)
        {
            Context.Update("FnPaymentVoucherItem", entity)
               .AutoMap(o => o.SysNo)
               .Where("SysNo", entity.SysNo)
               .Execute();
        }
    }
}
