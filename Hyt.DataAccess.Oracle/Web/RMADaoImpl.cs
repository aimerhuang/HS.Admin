using Hyt.DataAccess.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Model;
using Hyt.Model.SystemPredefined;
using Hyt.Util;

namespace Hyt.DataAccess.Oracle.Web
{
    /// <summary>
    /// 退换货
    /// </summary>
    /// <remarks>2013-09-03 邵斌 创建</remarks>
    public class RMADaoImpl : IRMADao
    {
        /// <summary>
        /// 通过发票系统编号获取税率
        /// </summary>
        /// <param name="invoiceSysNo">发票系统编号</param>
        /// <returns>返回税点</returns>
        /// <remarks>2013-09-03 邵斌 创建</remarks>
        public override decimal GetRateByInvoice(int invoiceSysNo)
        {
            #region 测试SQL

            /*
            select
                percentage 
            from 
                FnInvoice fi
                inner join FnInvoiceType fit on fit.sysno = fi.invoicetypesysno
            where 
                fi.sysno=1270
              */

            return Context.Sql(@"
                    select
                        percentage 
                    from 
                        FnInvoice fi
                        inner join FnInvoiceType fit on fit.sysno = fi.invoicetypesysno
                    where 
                        fi.sysno=@0  
             ", invoiceSysNo).QuerySingle<decimal>();

            #endregion
        }

        /// <summary>
        /// 退换货申请入库
        /// </summary>
        /// <param name="returnEntity">退换货主表对象</param>
        /// <param name="items">退换货子表对象集合</param>
        /// <returns>返回 true:添加成功 false:失败</returns>
        /// <remarks>2013-09-03 邵斌 创建</remarks>
        public override bool InsertRMA(Model.RcReturn returnEntity, IList<Model.RcReturnItem> items)
        {
            int sysNo = 0;
            using (var context = Context.UseSharedConnection(true))
            {
                //插入退换货主表对象
                sysNo = context.Insert("RcReturn", returnEntity)
                                      .AutoMap(o => o.SysNo)
                                      .ExecuteReturnLastId<int>("SysNo");

                //退换货日志,只有当主表创建成功的情况下才进行写日志
                if (sysNo > 0)
                {
                    foreach (var rcReturnItem in items)
                    {
                        //插入退换货子表对象集合
                        rcReturnItem.ReturnSysNo = sysNo;
                        context.Insert("RcReturnItem", rcReturnItem)
                                       .AutoMap(o => o.SysNo)
                                       .ExecuteReturnLastId<int>("SysNo");
                    }

                    Model.RcReturnLog model = new RcReturnLog()
                        {
                            ReturnSysNo = sysNo,
                            Operator = Model.SystemPredefined.User.SystemUser,
                            LogContent = string.Format(Constant.RMA_CREATE, sysNo),
                            OperateDate = DateTime.Now
                        };
                    context.Insert("RcReturnLog", model)
                                         .AutoMap(o => o.SysNo)
                                         .ExecuteReturnLastId<int>("SysNo");
                }
                else
                {
                    //主表创建失败将返回失败。
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 退还货历史查询
        /// </summary>
        /// <param name="customer">用户系统编号</param>
        /// <param name="endTime">截止时间</param>
        /// <param name="pager">分类对象</param>
        /// <returns></returns>
        /// <remarks>2013-09-03 邵斌 创建</remarks>
        public override void Search(int customer, DateTime endTime, ref Pager<CBWebRMA> pager)
        {
            int currentPageIndex = pager.CurrentPage;
            int orderSysNo = 0;
            DateTime now = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " 23:59:59");
            endTime = DateTime.Parse(endTime.ToString("yyyy-MM-dd") + " 00:00:00");

            //判读搜索关键字是否是订单编号
            if (!int.TryParse(pager.PageFilter.SearchKeyWords, out orderSysNo))
            {
                orderSysNo = 0;
            }

            using (var context = Context.UseSharedConnection(true))
            {

                #region 测试SQL 记录条数统计

                /*
                 select 
                      count(y.returnsysno)
                    from
                      (
                        select * from 
                          (
                          --订单编号过滤
                          select  rr.sysno as returnsysno from   RcReturn rr where rr.ordersysno='0'
                          union
                          --商品名称过滤
                          select
                                 rr1.sysno as returnsysno 
                          from
                                 RcReturn rr1
                                 inner join RcReturnItem rri1 on rr1.sysno = rri1.returnsysno 
                          where
                                 regexp_charindex(productname,'ipad',1,1,0,'inm') > 0
                          group by
                                rr1.sysno) x 
                        group by returnsysno  --去重 
                      ) y --过滤完成的结果集
                 */

                #endregion

                pager.TotalRows = context.Sql(@"
                    select 
                      count(y.returnsysno)
                    from
                      (
                        select * from 
                          (
                          select
                                 rr1.sysno as returnsysno 
                          from
                                 RcReturn rr1
                                 inner join RcReturnItem rri1 on rr1.sysno = rri1.returnsysno 
                          where
                                 rr1.customersysno=@customersysno and ((rr1.ordersysno = @1) or (@2 is null or regexp_charindex(productname,@3,1,1,0,'inm') > 0)) and rr1.createdate between @enddate and @now
                          group by
                                rr1.sysno) x 
                        group by returnsysno
                      ) y              
                ").Parameter("customersysno", customer)
                       .Parameter("1", orderSysNo)
                       .Parameter("2", pager.PageFilter.SearchKeyWords)
                       .Parameter("3", pager.PageFilter.SearchKeyWords)
                       .Parameter("enddate", endTime)
                       .Parameter("now", DateTime.Now)
                                         .QuerySingle<int>();

                if (currentPageIndex > pager.TotalPages)
                {
                    pager.CurrentPage = pager.TotalPages;
                }

                #region 测试SQL 查询数据集(同时过滤订单编号和商品名称)
                /*
                select 
                  rr2.sysno as ReturnSysNo,rr2.ordersysno as OrderSysNo,rr2.RmaType as ReturnType,rr2.status as Status,rr2.createdate as ApplyDateTime
                from
                  (
                    select * from 
                      (
                      --订单编号过滤
                      select  rr.sysno as returnsysno from   RcReturn rr where rr.customersysno=1004 and rr.ordersysno='0' and rr.createdate between to_date('2013/08/04 09:40:45','yyyy/mm/dd hh24:mi:ss') and to_date('2013/08/04 09:40:45','yyyy/mm/dd hh24:mi:ss')
                      union
                      --商品名称过滤
                      select
                             rr1.sysno as returnsysno 
                      from
                             RcReturn rr1
                             inner join RcReturnItem rri1 on rr1.sysno = rri1.returnsysno 
                      where
                             rr1.customersysno=1004 and regexp_charindex(productname,'ipad',1,1,0,'inm') > 0  and rr1.createdate between to_date('2013/08/04 09:40:45','yyyy/mm/dd hh24:mi:ss') and to_date('2013/08/04 09:40:45','yyyy/mm/dd hh24:mi:ss')
                      group by
                            rr1.sysno) x 
                    group by returnsysno  --去重 
                  ) y --过滤完成的结果集
                  inner join RcReturn rr2 on rr2.sysno = y.returnsysno
                order by rr2.createdate desc
             */
                #endregion

                pager.Rows = context.Select<CBWebRMA>(
                    "rr2.sysno as ReturnSysNo,rr2.ordersysno as OrderSysNo,rr2.RmaType as ReturnType,rr2.status as Status,rr2.createdate as ApplyDateTime")
                       .From(@"
                        (
                        select * from 
                          (
                          select
                                 rr1.sysno as returnsysno 
                          from
                                 RcReturn rr1
                                 inner join RcReturnItem rri1 on rr1.sysno = rri1.returnsysno 
                          where
                                 rr1.customersysno=@customersysno and ((rr1.ordersysno = @1) or (@2 is null or regexp_charindex(productname,@3,1,1,0,'inm') > 0)) and rr1.createdate between @enddate and @now
                          group by
                                rr1.sysno) x 
                        group by returnsysno 
                      ) y 
                      inner join RcReturn rr2 on rr2.sysno = y.returnsysno
                    ")
                       .OrderBy("rr2.createdate desc")
                       .Parameter("customersysno", customer)
                       .Parameter("1", orderSysNo)
                       .Parameter("2", pager.PageFilter.SearchKeyWords)
                       .Parameter("3", pager.PageFilter.SearchKeyWords)
                       .Parameter("enddate", endTime)
                       .Parameter("now", now)
                       .Paging(pager.CurrentPage, pager.PageSize)
                       .QueryMany();

                #region 测试SQL 查询退换货商品

                /*
                    select productname,productsysno,returnsysno from RcReturnItem where returnsysno in (1101,1136,1117,1102)
                 */

                //读取所有退换货系统编号用于查询退换货的商品
                List<int> returnSysNoList = pager.Rows.Select(r => r.ReturnSysNo).ToList();

                if (returnSysNoList.Count == 0)
                    return;

                //一次将所有商品查询出来进行分拣
                IList<CBWebRMAItem> ItemsList =
                    context.Sql(@"select 
                                        ri.productname,ri.productsysno,ri.returnsysno,p.productimage as Image 
                                    from 
                                        RcReturnItem ri
                                        inner join pdproduct p on p.sysno=ri.Productsysno 
                                    where ri.returnsysno in (" + returnSysNoList.Join(",") + ")").QueryMany<CBWebRMAItem>();

                //为每个退换货单添加明细商品
                foreach (var item in pager.Rows)
                {
                    var queryItems = from ip in ItemsList
                                     where ip.ReturnSysNo.Equals(item.ReturnSysNo)
                                     select ip;
                    item.Items = queryItems.ToList();
                }

                #endregion

            }

        }
    }
}
