using System;
using Hyt.DataAccess.Distribution;
using Hyt.Model;
using System.Linq;
using System.Collections.Generic;
using Hyt.DataAccess.Base;
using Hyt.Model.Transfer;

namespace Hyt.DataAccess.Oracle.Distribution
{
    /// <summary>
    /// 分销商预存款往来账明细
    /// </summary>
    /// <remarks>2013-09-10  朱成果 创建</remarks>
    public class DsPrePaymentItemDaoImpl : IDsPrePaymentItemDao
    {

        #region 数据记录增，删，改，查
        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>新增记录编号</returns>
        /// <remarks>2013-09-10  朱成果 创建</remarks>
        public override int Insert(DsPrePaymentItem entity)
        {
            entity.SysNo = Context.Insert("DsPrePaymentItem", entity)
                                        .AutoMap(o => o.SysNo)
                                        .ExecuteReturnLastId<int>("SysNo");
            return entity.SysNo;
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns></returns>
        /// <remarks>2013-09-10  朱成果 创建</remarks>
        public override void Update(DsPrePaymentItem entity)
        {

            Context.Update("DsPrePaymentItem", entity)
                   .AutoMap(o => o.SysNo)
                   .Where("SysNo", entity.SysNo)
                   .Execute();
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns>数据实体</returns>
        /// <remarks>2013-09-10  朱成果 创建</remarks>
        public override DsPrePaymentItem GetEntity(int sysNo)
        {

            return Context.Sql("select * from DsPrePaymentItem where SysNo=@SysNo")
                   .Parameter("SysNo", sysNo)
              .QuerySingle<DsPrePaymentItem>();
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns></returns>
        /// <remarks>2013-09-10  朱成果 创建</remarks>
        public override void Delete(int sysNo)
        {
            Context.Sql("Delete from DsPrePaymentItem where SysNo=@SysNo")
                 .Parameter("SysNo", sysNo)
            .Execute();
        }
        /// <summary>
        /// 更新预存款明细状态
        /// </summary>
        /// <param name="sourceSysNo">来源单据</param>
        /// <param name="source">来源编号</param>
        /// <param name="status">状态</param>
        /// <returns></returns>
        /// <remarks>2016-1-7 杨浩 创建</remarks>
        public override int UpdatePrePaymentItemStatus(int sourceSysNo,int source,int status)
        {
            return Context.Sql("update DsPrePaymentItem set [status]=@status  where SourceSysNo=@SourceSysNo and source=@source")
                 .Parameter("SourceSysNo", sourceSysNo)
                 .Parameter("source", source)
                 .Parameter("Status", status) 
                 .Execute();
        }

        #endregion

        /// <summary>
        /// 根据来源信息获取预存款明细
        /// </summary>
        /// <param name="dealerSysNo">分销商编号</param>
        /// <param name="source">来源</param>
        /// <param name="sourceSysNo">来源编号</param>
        /// <returns>预存款明细列表</returns>
        /// <remarks>2013-09-11  朱成果 创建</remarks>
        public override List<DsPrePaymentItem> GetListBySource(int dealerSysNo, int source, int sourceSysNo)
        {

            return Context.Sql(@"select t1.* from DsPrePaymentItem t1
                                    inner join DsPrePayment t2
                                    on t1.prepaymentsysno=t2.sysno
                                    where t2.dealersysno=@dealerSysNo and t1.source=@source and t1.sourcesysno=@sourceSysNo")
                 .Parameter("dealerSysNo", dealerSysNo)
                 .Parameter("source", source)
                 .Parameter("sourceSysNo", sourceSysNo)
            .QueryMany<DsPrePaymentItem>();
        }
        /// <summary>
        /// 根据来源信息获取到期返利的预存款明细
        /// </summary>
        /// <param name="source">来源</param>
        /// <param name="delayDay">到期天数</param>
        /// <param name="dealerSysNo">分销商编号</param>
        /// <param name="orderSysNo">订单系统编号</param>
        /// <returns></returns>
        /// <remarks>2015-1-8 杨浩 创建</remarks>
        public override List<DsPrePaymentItem> GetExpireListBySource(int source, int delayDay, int dealerSysNo = 0, int orderSysNo=0)
        {
            string whereStr = "";
            if (dealerSysNo > 0)
                whereStr = string.Format(" and b.DealerSysNo={0}", dealerSysNo);
            if(orderSysNo>0)
                whereStr += string.Format(" and b.SysNo={0}", orderSysNo);

            return Context.Sql(@"select pp.* from DsPrePaymentItem as pp 
                                inner join SoOrder b on pp.SourceSysNo = b.SysNo 
                                where pp.Source=@source and b.ReceivingConfirmDate<'" + DateTime.Now.AddDays(-1 * delayDay).ToString() + "' and pp.Status=10 " + whereStr)
              .Parameter("source", source)
              .QueryMany<DsPrePaymentItem>();
        }
        /// <summary>
        /// 创建分销商预存款往来账明细
        /// </summary>
        /// <param name="filter">过滤条件</param>
        /// <returns>分销商预存款往来账明细列表</returns>
        /// <remarks>2013-09-11 周唐炬 创建</remarks>
        public override Pager<CBDsPrePaymentItem> GetDsPrePaymentItemList(Model.Parameter.ParaDealerFilter filter)
        {
            string sqlWhere = "1=1";
            //判断是否绑定所有分销商
            if (!filter.IsBindAllDealer)
            {
                //判断是否绑定分销商
                if (filter.IsBindDealer)
                {
                    sqlWhere += " and C.SysNo = @7";
                }
                else
                {
                    sqlWhere += " and C.CreatedBy = @8";
                }
            }
            if (filter.SelectedDealerSysNo != -1)
            {
                sqlWhere += " and C.SysNo = @9";
            }

            //if(filter.SourceSysNo.HasValue&&filter.SourceSysNo>0)
                //sqlWhere += " and A.SourceSysNo=" + filter.SourceSysNo ;


            string sql = @"(SELECT A.*, C.DEALERNAME
                                          FROM DsPrePaymentItem A
                                         INNER JOIN DSPREPAYMENT B
                                            ON B.SYSNO = A.PREPAYMENTSYSNO
                                         INNER JOIN DsDealer C
                                            ON C.SYSNO = B.DEALERSYSNO
                                         WHERE (@0 IS NULL OR A.STATUS = @0)
                                           AND (@1 IS NULL OR A.SOURCE = @1)
                                           AND (@2 IS NULL OR A.SOURCESYSNO = @2)
                                           AND (@3 IS NULL OR charindex(C.DEALERNAME, @3) > 0)
                                           AND (@4 IS NULL OR A.CREATEDDATE >= @4)
                                           AND (@5 IS NULL OR A.CREATEDDATE <= @5)
                                           AND (@6 IS NULL OR C.SYSNO=@6)
                                           AND " + sqlWhere + ") tb";

            var dataList = Context.Select<CBDsPrePaymentItem>("tb.*").From(sql);
            var dataCount = Context.Select<int>("count(1)").From(sql);

            var dataSumIncreased = Context.Select<decimal>("sum(Increased)").From(sql);
            var dataSumDecreased = Context.Select<decimal>("sum(Decreased)").From(sql);
            var dataSumSurplus = Context.Select<decimal>("sum(Surplus)").From(sql);

            if (filter.EndTime.HasValue)
            {
                var date = Convert.ToDateTime(filter.EndTime.Value);
                filter.EndTime = date.AddDays(1);
            }
            var paras = new object[]
                {
                    filter.Status,
                    filter.Source,
                    filter.SourceSysNo,
                    filter.DealerName,
                    filter.StartTime,
                    filter.EndTime,
                    filter.SysNo,
                    filter.DealerSysNo,
                    filter.DealerCreatedBy,
                    filter.SelectedDealerSysNo
                };
            dataList.Parameters(paras);
            dataCount.Parameters(paras);

            dataSumIncreased.Parameters(paras);
            dataSumDecreased.Parameters(paras);
            dataSumSurplus.Parameters(paras);

            var pager = new Pager<CBDsPrePaymentItem>
            {
                Rows = dataList.OrderBy(@"tb.SysNo desc").Paging(filter.CurrentPage, filter.PageSize).QueryMany(),
                TotalRows = dataCount.QuerySingle()
            };

            if (pager.Rows.Any())
            {
                pager.Rows.First().TotalIncreased = dataSumIncreased.QuerySingle();
                pager.Rows.First().TotalDecreased = dataSumDecreased.QuerySingle();
                pager.Rows.First().TotalSurplus = dataSumSurplus.QuerySingle();
            }
            return pager;
        }

        /// <summary>
        /// 更新付款单明细状态
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="SendStatus"></param>
        /// <remarks>2015-09-17 王耀发  创建</remarks>
        public override void UpdatePaymentItemStatus(int SysNo, int Status)
        {
            Context.Sql("Update DsPrePaymentItem set Status=@Status where SysNo=@SysNo")
                   .Parameter("Status", Status)
                   .Parameter("SysNo", SysNo).Execute();
        }

    }
}
