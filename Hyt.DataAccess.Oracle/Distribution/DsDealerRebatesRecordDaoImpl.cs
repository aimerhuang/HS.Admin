using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.SellBusiness;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;
using Hyt.Util;
namespace Hyt.DataAccess.Distribution
{
    /// <summary>
    /// 返利记录数据访问类
    /// </summary>
    /// <remarks>
    /// 2015-09-15 王耀发 创建
    /// </remarks>
    public class DsDealerRebatesRecordDaoImpl : IDsDealerRebatesRecordDao
    {
        
        /// <summary>
        /// 返利记录信息
        /// </summary>
        /// <param name="filter">返利记录信息</param>
        /// <returns>返回返利记录信息</returns>
        /// <remarks>2015-09-15 王耀发 创建</remarks>
        public override Pager<CBDsDealerRebatesRecord> GetDsDealerRebatesRecordList(ParaDealerRebatesRecordFilter filter)
        {
            string sqlWhere = "1=1";

            //判断是否绑定所有分销商
            if (!filter.IsBindAllDealer)
            {
                //判断是否绑定分销商
                if (filter.IsBindDealer)
                {
                    sqlWhere += " and rd.SysNo = " + filter.DealerSysNo;
                }
                else
                {
                    sqlWhere += " and rd.CreatedBy = " + filter.DealerCreatedBy;
                }
            }
            if (filter.SelectedAgentSysNo != -1)
            {
                if (filter.SelectedDealerSysNo != -1)
                {
                    sqlWhere += " and rd.SysNo = " + filter.SelectedDealerSysNo;
                }
                else
                {
                    sqlWhere += " and rd.CreatedBy = " + filter.SelectedAgentSysNo;
                }
            }
            ///订单数据不为空的时候
            if (!string.IsNullOrEmpty(filter.Condition))
            {
                sqlWhere += " and (c.Name like '%" + filter.Condition.Replace("\'", "") + "%' or b.Name like '%" + filter.Condition.Replace("\'", "") + "%' or Convert(varchar(50), a.OrderSysNo) = '" + filter.Condition.Replace("\'", "")
                    + "' or c.Account like '%" + filter.Condition.Replace("\'", "") + "%' or b.Account like '%" + filter.Condition.Replace("\'", "") + "%' ) ";
            }

            if(!string.IsNullOrEmpty(filter.RebatesType))
            {
                sqlWhere += " and ( a.Genre = '" + filter.RebatesType + "' ) ";
            }
            string sql = @"(select a.*, b.Account as RecommendAccount,b.Name as RecommendName,c.Account as ComplyAccount,c.Name as ComplyName,rd.DealerName as RDealerName  
                    from CrCustomerRebatesRecord a left join CrCustomer b on a.RecommendSysNo = b.SysNo 
                    left join CrCustomer c on a.ComplySysNo = c.SysNo
                    left join SoOrder so on a.OrderSysNo=so.SysNo 
                    left join DsDealer rd on so.DealerSysNo = rd.SysNo   
                    where
                       " + sqlWhere + " ) tb";

            var dataList = Context.Select<CBDsDealerRebatesRecord>("tb.*").From(sql);
            var dataCount = Context.Select<int>("count(1)").From(sql);

            //var paras = new object[]
            //    {
            //        filter.OrderSysNo,
            //        filter.OrderSysNo
            //    };
            //dataList.Parameters(paras);
            //dataCount.Parameters(paras);

            var pager = new Pager<CBDsDealerRebatesRecord>
            {
                CurrentPage = filter.Id,
                PageSize = filter.PageSize
            };
            var totalRows = dataCount.QuerySingle();
            var rows = dataList.OrderBy("tb.OrderSysNo desc,tb.RebatesTime desc").Paging(pager.CurrentPage, pager.PageSize).QueryMany();

            pager.TotalRows = totalRows;
            pager.Rows = rows;

            return pager;
        }
        /// <summary>
        /// 分销汇总明细
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        /// <remarks>2016-07-15 周 创建</remarks>
        public override Pager<CBCCustomerRebatesRecord> GetDealerInfoSummaryList(ParaCBCCustomerRebatesRecordFilter filter)
        {

            string sqlWhere = "1=1";

            //判断是否绑定所有分销商
            if (!filter.IsBindAllDealer)
            {
                //判断是否绑定分销商
                if (filter.IsBindDealer)
                {
                    sqlWhere += " and c.DealerSysNo = " + filter.DealerSysNo;
                }
                else
                {
                    sqlWhere += " and de.CreatedBy = " + filter.DealerCreatedBy;
                }
            }
            if (filter.SelectedAgentSysNo != -1)
            {
                if (filter.SelectedDealerSysNo != -1)
                {
                    sqlWhere += " and c.DealerSysNo = " + filter.SelectedDealerSysNo;
                }
                else
                {
                    sqlWhere += " and de.CreatedBy = " + filter.SelectedAgentSysNo;
                }
            }
            if (filter.Account != "" || filter.Account != null)
            {
                sqlWhere += "and (c.Account like '%" + filter.Account + "%' or c.Name like '%" + filter.Account + "%' or c.NickName like '%" + filter.Account + "%' or substring(Convert(char(10),c.CreatedDate,112),1,8)='" + filter.Account + "' )";
            }
            string sql = @"(select [dbo].[func_GetCrCustomerOrderCount](c.sysno) as OrderNums,
                            [dbo].[func_GetRebagesOrderCount](c.sysno) as RebagesOrderCount,
                            [dbo].[func_GetDistributionGenreCount](c.sysno,1) as DirectCount,
                            [dbo].[func_GetDistributionGenreCount](c.sysno,2) as Indirect1Count,
                            [dbo].[func_GetDistributionGenreCount](c.sysno,3) as Indirect2Count,
                            le.LevelName,le.[Direct],le.[Indirect1],le.Indirect2,de.DealerName,de.CreatedBy as DealerCreatedBy,c.* 
                            from [CrCustomer] c 

                            left join DsDealer de on c.DealerSysNo=de.[SysNo]
                            left join DsDealerLevel le on le.[SysNo]=de.LevelSysNo

                            where  c.IsSellBusiness=1  and " + sqlWhere + " ) tb";
            var dataList = Context.Select<CBCCustomerRebatesRecord>("tb.*").From(sql);
            var dataCount = Context.Select<int>("count(1)").From(sql);
            //var paras = new object[]
            //    {
            //        //filter.Account,
            //        filter.Account
            //    };
            //dataList.Parameters(paras);
            //dataCount.Parameters(paras);

            var pager = new Pager<CBCCustomerRebatesRecord>
            {
                CurrentPage = filter.Id,
                PageSize = filter.PageSize
            };
            var totalRows = dataCount.QuerySingle();
            var rows = dataList.OrderBy("RebagesOrderCount desc,OrderNums desc").Paging(pager.CurrentPage, pager.PageSize).QueryMany();

            pager.TotalRows = totalRows;
            pager.Rows = rows;

            return pager;
        }


        /// <summary>
        /// 返利记录信息
        /// </summary>
        /// <param name="filter">返利记录信息</param>
        /// <returns>返回返利记录信息</returns>
        /// <remarks>2015-09-15 王耀发 创建</remarks>
        public override IList<CBDsDealerRebatesRecord> GetDsDealerRebatesRecordView(ParaDealerRebatesRecordFilter filter)
        {
            string sqlWhere = "1=1";
            if (filter.OrderSysNo > 0)
            {
                sqlWhere += " and a.OrderSysNo = @OrderSysNo";
            }
            //判断是否绑定所有分销商
            if (!filter.IsBindAllDealer)
            {
                //判断是否绑定分销商
                if (filter.IsBindDealer)
                {
                    sqlWhere += " and rd.SysNo = @DealerSysNo";
                }
                else
                {
                    sqlWhere += " and rd.CreatedBy = @DealerCreatedBy";
                }
            }
            if (filter.SelectedDealerSysNo != -1)
            {
                sqlWhere += " and rd.SysNo = @SelectedDealerSysNo";
            }
            string sql = @"select a.*, b.Account as RecommendAccount,b.Name as RecommendName,c.Account as ComplyAccount,c.Name as ComplyName,rd.DealerName as RDealerName  
                    from CrCustomerRebatesRecord a left join CrCustomer b on a.RecommendSysNo = b.SysNo
                    left join CrCustomer c on a.ComplySysNo = c.SysNo
                    left join DsDealer rd on b.DealerSysNo = rd.SysNo
                    where " + sqlWhere;

            var items = Context.Sql(sql)
                               .Parameter("OrderSysNo", filter.OrderSysNo)
                               .Parameter("DealerSysNo", filter.DealerSysNo)
                               .Parameter("DealerCreatedBy", filter.DealerCreatedBy)
                               .Parameter("SelectedDealerSysNo", filter.SelectedDealerSysNo)
                               .QueryMany<CBDsDealerRebatesRecord>();
            return items;
        }

        #region 数据记录增，删，改，查
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns>删除记录</returns>
        /// <remarks>2015-08-30 王耀发 创建</remarks>
        public override int Delete(int sysNo)
        {
            return Context.Delete("CrCustomerRebatesRecord")
                               .Where("SysNo", sysNo)
                               .Execute();
        }
       
        #endregion

        /// <summary>
        /// 分销返利分页
        /// </summary>
        /// <param name="pager"></param>
        /// <remarks>2016-07-15 周 创建</remarks>
        public override void GeDirectOrdersList(ref Model.Pager<CBDsDealerRebatesRecord> pager)
        {
            #region sql条件

            string sqlWhere = @"a.RecommendSysNo=@RecommendSysNo ";
            if(pager.PageFilter.Genre!="0")
            {
                sqlWhere += "and a.Genre=@Genre";
            }
            #endregion

            using (var context = Context.UseSharedConnection(true))
            {
                pager.Rows = context.Select<CBDsDealerRebatesRecord>("a.*, b.Account as RecommendAccount,b.Name as RecommendName,c.Account as ComplyAccount,c.Name as ComplyName,rd.DealerName as RDealerName ")
                                    .From("CrCustomerRebatesRecord a left join CrCustomer b on a.RecommendSysNo = b.SysNo left join CrCustomer c on a.ComplySysNo = c.SysNo left join DsDealer rd on b.DealerSysNo = rd.SysNo")
                                    .Where(sqlWhere)
                                    .Parameter("RecommendSysNo", pager.PageFilter.RecommendSysNo)
                                    .Parameter("Genre", pager.PageFilter.Genre)
                                    .Paging(pager.CurrentPage, pager.PageSize)
                                    .OrderBy("a.RebatesTime desc")
                                    .QueryMany();

                pager.TotalRows = context.Select<int>("count(1)")
                                         .From("CrCustomerRebatesRecord a left join CrCustomer b on a.RecommendSysNo = b.SysNo left join CrCustomer c on a.ComplySysNo = c.SysNo left join DsDealer rd on b.DealerSysNo = rd.SysNo")
                                         .Where(sqlWhere)
                                         .Parameter("RecommendSysNo", pager.PageFilter.RecommendSysNo)
                                         .Parameter("Genre", pager.PageFilter.Genre)
                                         .QuerySingle();
            }
        }

        /// <summary>
        /// 分销团队
        /// </summary>
        /// <param name="type"></param>
        /// <param name="pager"></param>
        /// <remarks>2016-07-15 周 创建</remarks>
        public override void GetMyDistTemsList(int? typeid, ref Model.Pager<CBCCrCustomerList> pager)
        {
            string sqlWhere = " 1=1 "; //" c.SysNo<>" + pager.PageFilter.SysNo + " ";
            switch(typeid)
            {
                case 1:
                    sqlWhere += " and c.PSysNo=" + pager.PageFilter.SysNo + "";
                    break;
                case 2:
                    sqlWhere += " and c.PSysNo in(select sysno from [CrCustomer] where PSysNo=" + pager.PageFilter.SysNo + ")";
                    break;
                case 3:
                    sqlWhere += " and c.PSysNo in(select sysno from [CrCustomer] where PSysNo in(select sysno from [CrCustomer] where PSysNo=" + pager.PageFilter.SysNo + "))";
                    break;
                default:
                    sqlWhere += " and c.CustomerSysNos like '%," + pager.PageFilter.SysNo + ",%'";
                    break;
            }
            
            using (var context = Context.UseSharedConnection(true))
            {
                pager.Rows = context.Select<CBCCrCustomerList>(" c.*,[dbo].[func_GetCrCustomerOrderCount](c.sysno) as OrderNums,[dbo].[func_GetRebagesOrderCount](c.sysno) as RebagesOrderCount ")
                                    .From("CrCustomer c")
                                    .Where(sqlWhere)
                                    //.Parameter("SysNo", pager.PageFilter.SysNo)
                                    .Paging(pager.CurrentPage, pager.PageSize)
                                    .OrderBy("c.RegisterDate desc")
                                    .QueryMany();

                pager.TotalRows = context.Select<int>("count(1)")
                                         .From("CrCustomer c")
                                         .Where(sqlWhere)
                                         //.Parameter("SysNo", pager.PageFilter.SysNo)
                                         .QuerySingle();
            }
        }

    }
}
