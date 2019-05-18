using Hyt.DataAccess.Stores;
using Hyt.Model;
using Hyt.Model.Transfer;
using Hyt.Model.Parameter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Oracle.Stores
{
    /// <summary>
    /// 分销上保证金订单
    /// </summary>
    /// <remarks>2016-5-15 杨浩 创建</remarks>
    public class DsDealerBailOrderDaoImpl : IDsDealerBailOrderDao
    {
        /// <summary>
        /// 根据客户编号获取保证金订单详情
        /// </summary>
        /// <param name="customerSysNo">客户编号</param>
        /// <returns></returns>
        public override DsDealerBailOrder GetDsDealerBailOrder(int customerSysNo)
        {
            return Context
                .Sql(string.Format("select * from DsDealerBailOrder where CustomerSysNo={0}",customerSysNo))
                .QuerySingle<DsDealerBailOrder>();
        }
        /// <summary>
        /// 获取保证金订单详情
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns></returns>
        public override DsDealerBailOrder GetModel(int sysNo)
        {
            return Context
               .Sql(string.Format("select * from DsDealerBailOrder where sysNo={0}", sysNo))
               .QuerySingle<DsDealerBailOrder>();
        }
        /// <summary>
        /// 更新保证金订单
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public override int Update(DsDealerBailOrder model)
        {
            var row = Context.Update<DsDealerBailOrder>("DsDealerBailOrder", model)
               .AutoMap(x => x.SysNo)
               .Execute();

            return row;
        }
        /// <summary>
        /// 创建保证金订单
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public override int Create(DsDealerBailOrder model)
        {
            var sysNo = Context.Insert<DsDealerBailOrder>("DsDealerBailOrder", model)
                 .AutoMap(x => x.SysNo)
                 .ExecuteReturnLastId<int>("SysNo");

            return sysNo;
        }
        /// <summary>
        /// 更新保证金订单状态
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <param name="statusType">状态类型（0：状态 1:支付状态）</param>
        /// <param name="status">状态值</param>
        /// <returns></returns>
        public override int UpdateStatus(int sysNo, int statusType, int status)
        {
            string setStr = "set";
            if (statusType == 1)
            {
                setStr += " PayStatus="+status;
            }
            else
            {
                setStr += " Status=" + status;
            }
            return Context
               .Sql(string.Format("update DsDealerBailOrder  {1}  where sysNo={0}", sysNo,setStr))
               .Execute();
        }
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="filter">查询参数</param>
        /// <returns>分页</returns>
        public override Pager<CBDsDealerBailOrder> Query(ParaDsDealerBailOrderFilter filter)
        {
            var where = "1=1";

            //判断是否绑定所有分销商
            if (!filter.IsBindAllDealer)
            {
                //判断是否绑定分销商
                if (filter.IsBindDealer)
                {
                    where += " and dea.SysNo = " + filter.DealerSysNo;
                }
                else
                {
                    where += " and dea.CreatedBy = " + filter.DealerCreatedBy;
                }
            }
            if (filter.SelectedAgentSysNo != -1)
            {
                if (filter.SelectedDealerSysNo != -1)
                {
                    where += " and dea.SysNo = " + filter.SelectedDealerSysNo;
                }
                else
                {
                    where += " and dea.CreatedBy = " + filter.SelectedAgentSysNo;
                }
            }

            string sql = @"(select a.*,(select SysNo from fnreceiptvoucher where Source = 20 and SourceSysNo = a.SysNo) as FnReceiptVoucherSysNo,dea.DealerName 
                    from DsDealerBailOrder a 
                    left join CrCustomer c on a.CustomerSysNo = c.SysNo 
                    left join DsDealer dea on c.DealerSysNo = dea.SysNo
                    where " + where + " and ((@0 is null or charindex(a.ContactName,@1)>0) or  (@2 is null or charindex(a.ContactWay,@3)>0) )) tb";

            var dataList = Context.Select<CBDsDealerBailOrder>("tb.*").From(sql);
            var dataCount = Context.Select<int>("count(1)").From(sql);

            var paras = new object[]
                {
                    filter.ContactName,filter.ContactName,
                    filter.ContactWay,filter.ContactWay
                };
            dataList.Parameters(paras);
            dataCount.Parameters(paras);

            var pager = new Pager<CBDsDealerBailOrder>
            {
                CurrentPage = filter.Id,
                PageSize = filter.PageSize
            };
            var totalRows = dataCount.QuerySingle();
            var rows = dataList.OrderBy("tb.CreateDate desc").Paging(pager.CurrentPage, pager.PageSize).QueryMany();

            pager.TotalRows = totalRows;
            pager.Rows = rows;

            return pager;

        }
    }
}
