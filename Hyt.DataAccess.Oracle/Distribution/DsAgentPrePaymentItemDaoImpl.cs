using System;
using Hyt.Model;
using System.Collections.Generic;
using Hyt.DataAccess.Base;
using Hyt.DataAccess.Distribution;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;

namespace Hyt.DataAccess.Oracle.Distribution
{
    /// <summary>
    /// 代理商预存款来往明细数据层
    /// </summary>
    /// <remarks> 2016-04-18 刘伟豪 创建 </remarks>
    public class DsAgentPrePaymentItemDaoImpl : IDsAgentPrePaymentItemDao
    {
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pager">分页实体</param>
        /// <param name="filter">查询条件</param>
        /// <remarks> 2016-04-18 刘伟豪 创建</remarks>
        public override void Search(ref Pager<CBDsAgentPrePaymentItem> pager, ParaDsAgentPrePaymentItemFilter filter)
        {
            string sqlWhere = "1=1";
            string sql = @"(Select 
                                 A.*, C.Name AgentName
                            FROM 
                                 DsAgentPrePaymentItem A
                                 Inner Join DsAgentPrePayment B On A.AgentPrePaymentSysNo = B.SysNo
                                 Inner Join DsAgent C On B.AgentSysNo = C.SysNo
                            WHERE 
                                 (@0=-1 Or A.Status = @0)
                                 And (@1=-1 Or A.Source = @1)
                                 And (@2 Is Null Or A.CreatedDate >= @2)
                                 And (@3 Is Null Or A.CreatedDate <= @3)
                                 And (@4 Is Null Or C.SysNo=@4)
                                 And " + sqlWhere + ") tb";

            var dataList = Context.Select<CBDsAgentPrePaymentItem>("tb.*").From(sql);
            var dataCount = Context.Select<int>("count(1)").From(sql);
            if (filter.EndTime.HasValue)
            {
                var date = Convert.ToDateTime(filter.EndTime.Value);
                filter.EndTime = date.AddDays(1);
            }
            var paras = new object[]
                {
                    filter.Status,
                    filter.Source,
                    filter.StartTime,
                    filter.EndTime,
                    filter.SysNo,
                };
            dataList.Parameters(paras);
            dataCount.Parameters(paras);

            pager.Rows = dataList.OrderBy(@"tb.CreatedDate Desc,tb.SysNo Desc").Paging(pager.CurrentPage, pager.PageSize).QueryMany();
            pager.TotalRows = dataCount.QuerySingle();
        }

        /// <summary>
        /// 新建代理商预存款来往明细
        /// </summary>
        /// <param name="model">代理商预存款来往明细实体</param>
        /// <remarks> 2016-04-18 刘伟豪 创建</remarks>
        public override int Create(DsAgentPrePaymentItem model)
        {
            model.SysNo = Context.Insert("DsAgentPrePaymentItem", model)
                                        .AutoMap(o => o.SysNo)
                                        .ExecuteReturnLastId<int>("SysNo");
            return model.SysNo;
        }
    }
}