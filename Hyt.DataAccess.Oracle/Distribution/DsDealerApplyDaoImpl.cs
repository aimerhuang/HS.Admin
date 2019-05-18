using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Distribution;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;

namespace Hyt.DataAccess.Oracle.Distribution
{
    /// <summary>
    /// 分销商信息维护数据访问层
    /// </summary>
    /// <remarks>
    /// 2013-09-04 郑荣华 创建
    /// </remarks>
    public class DsDealerApplyDaoImpl : IDsDealerApplyDao
    {
        /// <summary>
        /// 查询分销商申请信息
        /// </summary>
        /// <param name="filter">查询参数实体</param>
        /// <returns>分销商申请信息列表</returns>
        /// <remarks> 
        /// 2016-04-16 王耀发 创建 
        /// </remarks>   
        public override void GetDsDealerApplyList(ref Pager<CBDsDealerApply> pager, ParaDsDealerApplyFilter filter)
        {
            using (var context = Context.UseSharedConnection(true))
            {
                const string sqlSelect = @"t.*,s.UserName as HandlerName";

                const string sqlFrom = @"DsDealerApply t left join SyUser s on t.HandlerSysNo = s.SysNo ";
                string sqlWhere = "1=1";


                sqlWhere += @" and ((@ContactName is null or charindex(t.ContactName,@ContactName)>0)              
                or (@ContactWay is null or charindex(t.ContactWay,@ContactWay)>0))";

                #region sqlcount

                string sqlCount = @" select count(1) from DsDealerApply t where " + sqlWhere;

                pager.TotalRows = context.Sql(sqlCount)
                                         .Parameter("ContactName", filter.ContactName)
                                         .Parameter("ContactWay", filter.ContactWay)
                                         .QuerySingle<int>();
                #endregion

                pager.Rows = context.Select<CBDsDealerApply>(sqlSelect)
                                    .From(sqlFrom)
                                    .Where(sqlWhere)
                                    .Parameter("ContactName", filter.ContactName)
                                    .Parameter("ContactWay", filter.ContactWay)
                                    .OrderBy("t.sysno desc")
                                    .Paging(pager.CurrentPage, pager.PageSize)
                                    .QueryMany();
            }
        }

        /// <summary>
        /// 更新分销商申请状态
        /// </summary>
        /// <param name="soID">订单编号</param>
        /// <param name="NsStatus">状态</param>
        /// <returns></returns>
        /// <remarks>2016-4-6 王耀发 创建</remarks>
        public override void UpdateStatus(int SysNo, int Status, int HandlerSysNo)
        {
            Context.Update("DsDealerApply")
                .Column("Status", Status)
                .Column("HandlerSysNo", HandlerSysNo)
                .Where("SysNo", SysNo).Execute();
        }
        /// <summary>
        /// 领取活动商品记录
        /// </summary>
        /// <param name="pager"></param>
        /// <param name="filter"></param>
        public override void GetDealerReceiveProductList(ref Pager<CBDsReceiveProduct> pager, ParaReceiveProductFilter filter, string ContactKey)
        {
            using (var context = Context.UseSharedConnection(true))
            {
                const string sqlSelect = @"re.*,cu.NickName as CustomerName,cu.Account,pr.ProductName,de.DealerName";

                const string sqlFrom = @"SoReceiveProduct re  left join CrCustomer cu on re.CustomerSysNo=cu.sysno
                                                              left join PdProduct pr on re.ProductSysNo=pr.sysno
                                                              left join DsDealer de on re.DealerSysNo=de.sysno";
                string sqlWhere = " 1=1 ";

                if (ContactKey != null)
                {
                    sqlWhere += " and cu.NickName like'%" + ContactKey + "%' or pr.ProductName like'%" + ContactKey + "%'";
                }
                #region sqlcount

                string sqlCount = @" select count(1) from SoReceiveProduct  re
                                     left join CrCustomer cu on re.CustomerSysNo=cu.sysno
                                     left join PdProduct pr on re.ProductSysNo=pr.sysno
                                     left join DsDealer de on re.DealerSysNo=de.sysno where " + sqlWhere;

                pager.TotalRows = context.Sql(sqlCount)
                                         .QuerySingle<int>();
                #endregion

                pager.Rows = context.Select<CBDsReceiveProduct>(sqlSelect)
                                    .From(sqlFrom)
                                    .Where(sqlWhere)
                                    .OrderBy("re.sysno desc")
                                    .Paging(pager.CurrentPage, pager.PageSize)
                                    .QueryMany();
            }
        }
    }
}
