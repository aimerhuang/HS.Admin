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
    /// 代理商数据访问层
    /// </summary>
    /// <remarks> 2016-04-13 刘伟豪 创建 </remarks>
    public class DsAgentDaoImpl : IDsAgentDao
    {
        /// <summary>
        /// 分页查询代理商
        /// </summary>
        /// <param name="pager">分页</param>
        /// <param name="filter">查询条件</param>
        /// <remarks> 2016-04-13 刘伟豪 创建 </remarks>
        public override void Search(ref Pager<CBDsAgent> pager, ParaDsAgentFilter filter)
        {
            using (var _context = Context.UseSharedConnection(true))
            {
                var sqlWhere = " 1=1 ";

                if (filter.SysNo > 0)
                    sqlWhere += " and Ag.SysNo=@SysNo ";

                if (filter.LevelSysNo > 0)
                    sqlWhere += " and Ag.LevelSysNo=@LevelSysNo ";

                if (!string.IsNullOrWhiteSpace(filter.Name))
                    sqlWhere += " and Ag.Name like @Name";

                if (!string.IsNullOrWhiteSpace(filter.Contact))
                    sqlWhere += " and Ag.Contact like @Contact";

                if (!string.IsNullOrWhiteSpace(filter.MobilePhoneNumber))
                    sqlWhere += " and Ag.MobilePhoneNumber like @MobilePhoneNumber";

                if (filter.Status > -1)
                    sqlWhere += " and Ag.Status=@Status ";

                if (filter.StartTime != null)
                    sqlWhere += " and Ag.CreatedDate >= @StartTime";

                if (filter.EndTime != null)
                    sqlWhere += " and Ag.CreatedDate <= @EndTime";

                if (filter.UpdateDateStartTime != null)
                    sqlWhere += " and Ag.LastUpdateDate >= @UpdateDateStartTime";

                if (filter.UpdateDateEndTime != null)
                    sqlWhere += " and  Ag.LastUpdateDate <= @UpdateDateEndTime";

                if (!string.IsNullOrWhiteSpace(filter.KeyWord))
                    sqlWhere += " and (Ag.Name like @KeyWord or Us.Account like @KeyWord)";

                if (!string.IsNullOrWhiteSpace(filter.Account))
                    sqlWhere += " and Us.Account like @Account";

                pager.Rows = _context.Select<CBDsAgent>(" Ag.*,Lv.LevelName,Us.Account,dbo.func_getaereapath(Ag.AreaSysNo) AreaAllName,Ap.TotalPrestoreAmount,Ap.AvailableAmount,Ap.FrozenAmount,Ap.SysNo As AgentPrePaymentSysNo ")
                                     .From(@"DsAgent Ag
                                             Left Join DsDealerLevel Lv On Ag.LevelSysNo=Lv.SysNo
                                             Left Join SyUser Us On Ag.UserSysNo=Us.SysNo
                                             Left Join DsAgentPrePayment Ap On Ag.SysNo=Ap.AgentSysNo")
                                     .Where(sqlWhere)
                                     .Parameter("SysNo", filter.SysNo)
                                     .Parameter("LevelSysNo", filter.LevelSysNo)
                                     .Parameter("Name", "%" + filter.Name + "%")
                                     .Parameter("Contact", "%" + filter.Contact + "%")
                                     .Parameter("MobilePhoneNumber", "%" + filter.MobilePhoneNumber + "%")
                                     .Parameter("Status", filter.Status)
                                     .Parameter("StartTime", filter.StartTime)
                                     .Parameter("EndTime", filter.EndTime)
                                     .Parameter("UpdateDateStartTime", filter.UpdateDateStartTime)
                                     .Parameter("UpdateDateEndTime", filter.UpdateDateEndTime)
                                     .Parameter("KeyWord", "%" + filter.KeyWord + "%")
                                     .Parameter("Account", "%" + filter.Account + "%")
                                     .OrderBy(" Ag.LastUpdateDate Desc,Ag.SysNo Desc ")
                                     .Paging(pager.CurrentPage, pager.PageSize)
                                     .QueryMany();

                pager.TotalRows = _context.Select<int>(" count(0) ")
                                     .From(@"DsAgent Ag
                                             Left Join DsDealerLevel Lv On Ag.LevelSysNo=Lv.SysNo
                                             Left Join SyUser Us On Ag.UserSysNo=Us.SysNo
                                             Left Join DsAgentPrePayment Ap On Ag.SysNo=Ap.AgentSysNo")
                                     .Where(sqlWhere)
                                     .Parameter("SysNo", filter.SysNo)
                                     .Parameter("LevelSysNo", filter.LevelSysNo)
                                     .Parameter("Name", "%" + filter.Name + "%")
                                     .Parameter("Contact", "%" + filter.Contact + "%")
                                     .Parameter("MobilePhoneNumber", "%" + filter.MobilePhoneNumber + "%")
                                     .Parameter("Status", filter.Status)
                                     .Parameter("StartTime", filter.StartTime)
                                     .Parameter("EndTime", filter.EndTime)
                                     .Parameter("UpdateDateStartTime", filter.UpdateDateStartTime)
                                     .Parameter("UpdateDateEndTime", filter.UpdateDateEndTime)
                                     .Parameter("KeyWord", "%" + filter.KeyWord + "%")
                                     .Parameter("Account", "%" + filter.Account + "%")
                                     .QuerySingle();
            }
        }

        /// <summary>
        /// 分页查询代理商资金列表
        /// </summary>
        /// <param name="pager">分页</param>
        /// <param name="filter">查询条件</param>
        /// <remarks> 2016-04-18 刘伟豪 创建 </remarks>
        public override void SeachAgentPrePayment(ref Pager<CBDsAgent> pager, ParaDsAgentFilter filter)
        {
            string sqlWhere = " 1=1 ";
            if (!filter.IsBindAllDealer)
            {
                if (filter.IsAgent)
                {
                    sqlWhere += " And Ag.UserSysNo=@4";
                }
            }
            string sql = @"(Select Ag.*
                                  ,Lv.LevelName
                                  ,Us.Account
                                  ,dbo.func_getaereapath(Ag.AreaSysNo) AreaAllName
                                  ,Ap.TotalPrestoreAmount
                                  ,Ap.AvailableAmount
                                  ,Ap.FrozenAmount
                                  ,Ap.SysNo As AgentPrePaymentSysNo
                             From
                                  DsAgent Ag 
                                  Left Join DsDealerLevel Lv On Ag.LevelSysNo=Lv.SysNo 
                                  Left Join SyUser Us On Ag.UserSysNo=Us.SysNo
                                  Left Join DsAgentPrePayment Ap On Ag.SysNo=Ap.AgentSysNo
                             Where
                                  (@0=0 Or Ag.AreaSysNo=@0)
                                  And (@1=-1 Or Ag.Status=@1)
                                  And (@2=0 Or Ag.LevelSysNo=@2)
                                  And (@3='' Or @3 Is Null Or CHARINDEX(@3,Ag.Name) > 0)
                                  And " + sqlWhere + ") tb";

            var dataList = Context.Select<CBDsAgent>("tb.*").From(sql);
            var dataCount = Context.Select<int>("count(0)").From(sql);
            var paras = new object[] { 
                filter.AreaSysNo,
                filter.Status,
                filter.LevelSysNo,
                filter.Name,
                filter.CurrentUserSysNo
            };

            dataList = dataList.Parameters(paras);
            dataCount = dataCount.Parameters(paras);

            pager.Rows = dataList.OrderBy("tb.LastUpdateDate Desc,tb.SysNo Desc").Paging(pager.CurrentPage, pager.PageSize).QueryMany();
            pager.TotalRows = dataCount.QuerySingle();
        }

        /// <summary>
        /// 用于更新检查用户系统编号不重复，查询代理商信息，修改时用
        /// </summary>
        /// <param name="userSysNo">用户系统编号</param>
        /// <param name="sysNo">要排除的代理商系统编号</param>
        /// <remarks> 2016-04-14 刘伟豪 创建 </remarks>
        public override IList<CBDsAgent> List(int userSysNo, int sysNo)
        {
            const string sql = @"Select Ag.*,
	                                    Lv.LevelName,
	                                    Us.Account,
	                                    dbo.func_getaereapath(Ag.AreaSysNo) AreaAllName,
	                                    Ap.TotalPrestoreAmount,Ap.AvailableAmount,Ap.FrozenAmount,Ap.SysNo As DsAgentPrePaymentSysNo
                                 From DsAgent Ag
                                 Left Join DsDealerLevel Lv On Ag.LevelSysNo=Lv.SysNo
                                 Left Join SyUser Us On Ag.UserSysNo=Us.SysNo
                                 Left Join DsAgentPrePayment Ap On Ag.SysNo=Ap.AgentSysNo
                                 Where Ag.UserSysNo=@0 And Ag.SysNo<>@1";

            return Context.Sql(sql, userSysNo, sysNo)
                          .QueryMany<CBDsAgent>();
        }

        /// <summary>
        /// 用于新建检查用户系统编号不重复，查询代理商信息，新增时用
        /// </summary>
        /// <param name="userSysNo">用户系统编号</param>
        /// <remarks> 2016-04-14 刘伟豪 创建 </remarks>
        public override IList<CBDsAgent> List(int userSysNo)
        {
            const string sql = @"Select Ag.*,
	                                    Lv.LevelName,
	                                    Us.Account,
	                                    dbo.func_getaereapath(Ag.AreaSysNo) AreaAllName,
	                                    Ap.TotalPrestoreAmount,Ap.AvailableAmount,Ap.FrozenAmount,Ap.SysNo As DsAgentPrePaymentSysNo
                                 From DsAgent Ag
                                 Left Join DsDealerLevel Lv On Ag.LevelSysNo=Lv.SysNo
                                 Left Join SyUser Us On Ag.UserSysNo=Us.SysNo
                                 Left Join DsAgentPrePayment Ap On Ag.SysNo=Ap.AgentSysNo
                                 Where Ag.UserSysNo=@0";

            return Context.Sql(sql, userSysNo)
                          .QueryMany<CBDsAgent>();
        }

        /// <summary>
        /// 根据名称查询代理商列表，用于判断名称唯一性，新增时用
        /// </summary>
        /// <param name="agentName">代理商名称</param>
        /// <remarks> 2016-04-14 刘伟豪 创建 </remarks>
        public override IList<CBDsAgent> List(string agentName)
        {
            const string sql = @"Select Ag.*,
	                                    Lv.LevelName,
	                                    Us.Account,
	                                    dbo.func_getaereapath(Ag.AreaSysNo) AreaAllName,
	                                    Ap.TotalPrestoreAmount,Ap.AvailableAmount,Ap.FrozenAmount,Ap.SysNo As DsAgentPrePaymentSysNo
                                 From DsAgent Ag
                                 Left Join DsDealerLevel Lv On Ag.LevelSysNo=Lv.SysNo
                                 Left Join SyUser Us On Ag.UserSysNo=Us.SysNo
                                 Left Join DsAgentPrePayment Ap On Ag.SysNo=Ap.AgentSysNo
                                 Where Ag.Name=@0";

            return Context.Sql(sql, agentName)
                          .QueryMany<CBDsAgent>();
        }

        /// <summary>
        /// 根据名称查询代理商列表，用于判断名称唯一性，修改时用
        /// </summary>
        /// <param name="agentName">代理商名称</param>
        /// <param name="sysNo">要排除的代理商系统编号</param>
        /// <remarks> 2016-04-15 刘伟豪 创建 </remarks>
        public override IList<CBDsAgent> List(string agentName, int sysNo)
        {
            const string sql = @"Select Ag.*,
	                                    Lv.LevelName,
	                                    Us.Account,
	                                    dbo.func_getaereapath(Ag.AreaSysNo) AreaAllName,
	                                    Ap.TotalPrestoreAmount,Ap.AvailableAmount,Ap.FrozenAmount,Ap.SysNo As DsAgentPrePaymentSysNo
                                 From DsAgent Ag
                                 Left Join DsDealerLevel Lv On Ag.LevelSysNo=Lv.SysNo
                                 Left Join SyUser Us On Ag.UserSysNo=Us.SysNo
                                 Left Join DsAgentPrePayment Ap On Ag.SysNo=Ap.AgentSysNo
                                 Where Ag.Name=@0 And Ag.SysNo<>@1";

            return Context.Sql(sql, agentName, sysNo)
                          .QueryMany<CBDsAgent>();
        }

        /// <summary>
        /// 新建代理商
        /// </summary>
        /// <param name="model">代理商实体</param>
        /// <remarks> 2016-04-14 刘伟豪 创建 </remarks>
        public override int Create(DsAgent model)
        {
            return Context.Insert("DsAgent", model)
                         .AutoMap(x => x.SysNo)
                         .ExecuteReturnLastId<int>("SysNo");
        }

        /// <summary>
        /// 获取代理商实体
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <remarks> 2016-04-14 刘伟豪 创建 </remarks>
        public override CBDsAgent GetDsAgent(int sysNo)
        {
            const string sql = @"Select Ag.*,
	                                    Lv.LevelName,
	                                    Us.Account,
	                                    dbo.func_getaereapath(Ag.AreaSysNo) AreaAllName,
	                                    Ap.TotalPrestoreAmount,Ap.AvailableAmount,Ap.FrozenAmount,Ap.SysNo As DsAgentPrePaymentSysNo
                                 From DsAgent Ag
                                 Left Join DsDealerLevel Lv On Ag.LevelSysNo=Lv.SysNo
                                 Left Join SyUser Us On Ag.UserSysNo=Us.SysNo
                                 Left Join DsAgentPrePayment Ap On Ag.SysNo=Ap.AgentSysNo
                                 Where Ag.SysNo=@0";
            return Context.Sql(sql, sysNo)
                          .QuerySingle<CBDsAgent>();
        }

        /// <summary>
        /// 根据管理员系统编号，获取代理商实体
        /// </summary>
        /// <param name="userSysNo">管理员系统编号</param>
        /// <remarks> 2016-04-15 刘伟豪 创建 </remarks>
        public override CBDsAgent GetDsAgentByUserSysNo(int userSysNo)
        {
            const string sql = @"Select Ag.*,
	                                    Lv.LevelName,
	                                    Us.Account,
	                                    dbo.func_getaereapath(Ag.AreaSysNo) AreaAllName,
	                                    Ap.TotalPrestoreAmount,Ap.AvailableAmount,Ap.FrozenAmount,Ap.SysNo As DsAgentPrePaymentSysNo
                                 From DsAgent Ag
                                 Left Join DsDealerLevel Lv On Ag.LevelSysNo=Lv.SysNo
                                 Left Join SyUser Us On Ag.UserSysNo=Us.SysNo
                                 Left Join DsAgentPrePayment Ap On Ag.SysNo=Ap.AgentSysNo
                                 Where Ag.UserSysNo=@0";
            return Context.Sql(sql, userSysNo)
                          .QuerySingle<CBDsAgent>();
        }

        /// <summary>
        /// 修改代理商
        /// </summary>
        /// <param name="model">代理商实体</param>
        /// <remarks> 2016-04-15 刘伟豪 创建 </remarks>
        public override bool Update(DsAgent model)
        {
            int rows = Context.Update("DsAgent", model)
                          .AutoMap(x => x.SysNo, x => x.CreatedDate, x => x.CreatedBy)
                          .Where(x => x.SysNo)
                          .Execute();
            return rows > 0;
        }

        /// <summary>
        /// 修改代理商时，同时统一更新下级分销商等级
        /// </summary>
        /// <param name="agentSysNo">代理商系统编号</param>
        /// <param name="levelSysNo">分销商等级编号</param>
        /// <remarks> 2016-04-15 刘伟豪 创建 </remarks>
        public override bool UpdateDealersLevel(int agentSysNo, int levelSysNo)
        {
            int r = Context.Sql("update DsDealer set LevelSysNo=@0 where CreatedBy=@1", levelSysNo, agentSysNo)
                           .Execute();
            return r > 0;
        }

        /// <summary>
        /// 代理商状态更新，同时修改关联账号状态,状态枚举相同
        /// </summary>
        /// <param name="sysNo">代理商系统编号</param>
        /// <param name="status">代理商状态</param>
        /// <param name="lastUpdateBy">最后更新人</param>
        /// <remarks> 2016-04-15 刘伟豪 创建 </remarks>
        public override bool UpdateStatus(int sysNo, DistributionStatus.代理商状态 status, int lastUpdateBy)
        {
            int r = Context.Sql("update DsAgent set status=@0,lastupdateby=@1,lastupdatedate=@2 where sysno=@3", (int)status, lastUpdateBy, DateTime.Now, sysNo)
                           .Execute();
            return r > 0;
        }
    }
}