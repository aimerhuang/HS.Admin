
using Hyt.DataAccess.Base;
using Hyt.DataAccess.Sys;
using System.Collections.Generic;
using Hyt.Model;
using System.Linq;
using Hyt.Model.WorkflowStatus;

namespace Hyt.DataAccess.Oracle.Sys
{
    /// <summary>
    /// 任务池数据访问类
    /// </summary>
    /// <remarks>2013-9-16 余勇 创建</remarks>
    public class SyJobPoolDaoImpl : ISyJobPoolDao
    {
        /// <summary>
        /// 任务池列表查询
        /// </summary>
        /// <param name="filter">订单分页查询条件实体</param>
        /// <returns>空</returns>
        /// <remarks>2013-06-13 余勇 创建</remarks>
        /// <remarks>2013-06-24 余勇 重构</remarks>
        public override void GetJobSpoolList(Pager<CBSyJobPool> filter)
        {
            string queryFields = @"c.TaskSysNo,c.JobDescription,c.JobUrl,c.TaskType,
                               b.username as ExecutorSysName,c.ExecutorSysNo,c.SysNo,c.Status,c.Priority,c.CreatedDate,c.AssignDate,c.Remarks";
            string sqlWhere = @"(@0 = 0 or c.TaskSysNo like @0)
                                      and (
                                      (@1 = 2 and (c.Status =@2 or c.Status =@3) and (@4= 0 or c.ExecutorSysNo=@4))    --已分配                                                                    
                                      or (@1 = 3 and c.Status = @5)    --已锁定
                                      or (@1 = 1 and c.Status = @6)  
                                    )
                                    and (@7 = 0 or c.TaskType = @7)";
            //2016-3-18 王耀发 修改
            //var sort = "ISNULL(c.Priority,0) desc,(case when convert(char(10),c.AssignDate,20)='1900-01-01' then SYSDATETIME()-c.CreatedDate else SYSDATETIME()-c.AssignDate end) desc";
            var sort = "ISNULL(c.Priority,0) desc,c.AssignDate desc ";
            if (filter.PageFilter.Sort == 1)
            {
                sort = "(case when convert(char(10),c.AssignDate,110)='1900-01-01' then SYSDATETIME()-c.CreatedDate else SYSDATETIME()-c.AssignDate end) desc";
            }

            string fromSql = @"SyJobPool c left join SyUser b on b.SysNo=c.ExecutorSysNo";

            //当任务类型为订单审核(10)，订单提交出库(15)加入订单表对应的分销商选择 王耀发 2016-1-28 创建
            if (filter.PageFilter.TaskType == 10 || filter.PageFilter.TaskType == 15)
            {
                fromSql += " left join SoOrder a on c.TaskSysNo = a.SysNo left join DsDealer d on a.DealerSysNo = d.SysNo";

                queryFields += ",d.DealerName";
                //是否绑定所有仓库
                if (filter.PageFilter.HasAllWarehouse)
                {
                    //判断是否绑定所有分销商
                    if (!filter.PageFilter.IsBindAllDealer)
                    {
                        //判断是否绑定分销商
                        if (filter.PageFilter.IsBindDealer)
                        {
                            sqlWhere += " and d.SysNo = " + filter.PageFilter.DealerSysNo;
                        }
                        else
                        {
                            sqlWhere += " and d.CreatedBy = " + filter.PageFilter.DealerCreatedBy;
                        }
                    }
                    if (filter.PageFilter.SelectedAgentSysNo != -1)
                    {
                        if (filter.PageFilter.SelectedDealerSysNo != -1)
                        {
                            sqlWhere += " and d.SysNo = " + filter.PageFilter.SelectedDealerSysNo;
                        }
                        else
                        {
                            sqlWhere += " and d.CreatedBy = " + filter.PageFilter.SelectedAgentSysNo;
                        }
                    }
                }
                else
                {
                    if (filter.PageFilter.Warehouses.Count > 0)
                    {
                        var wList = "";
                        foreach (var w in filter.PageFilter.Warehouses)
                        {
                            if (wList == "")
                                wList = w.SysNo.ToString();
                            else
                                wList += ',' + w.SysNo.ToString();
                        }
                        wList = "(" + wList + ")";
                        sqlWhere += " and a.DefaultWarehouseSysNo in " + wList;
                    }
                    else
                    {
                        sqlWhere += " and a.DefaultWarehouseSysNo = -1";
                    }
                }
            }
            //---------------------------------------------------------------------------------------------

            var dataCount = Context.Select<int>("count(1)")
                .From(fromSql)
                .Where(sqlWhere);
            var dataList = Context.Select<CBSyJobPool>(queryFields)
                .From(fromSql)
                .Where(sqlWhere);

            var paras = new object[]
                {
                  filter.PageFilter.TaskSysNo,
                  filter.PageFilter.Status,
                  (int)SystemStatus.任务池状态.待处理,
                  (int)SystemStatus.任务池状态.处理中,
                  filter.PageFilter.ExecutorSysNo,
                  (int)SystemStatus.任务池状态.已锁定,
                  (int)SystemStatus.任务池状态.待分配,
                  filter.PageFilter.TaskType
                };
            dataList.Parameters(paras);
            dataCount.Parameters(paras);

            filter.TotalRows = dataCount.QuerySingle();
            filter.Rows = dataList.OrderBy(sort)
                .Paging(filter.CurrentPage, filter.PageSize).QueryMany();

        }

        /// <summary>
        /// 获取用户待处理任务数
        /// </summary>
        /// <param name="executorSysNo">用户编号</param>
        /// <param name="taskType">任务类型</param>
        /// <returns>用户待处理任务数</returns>
        /// <remarks>2013-06-13 余勇 创建</remarks>
        public override int GetJobsNumByUser(int executorSysNo, int taskType)
        {
            string sqlWhere = @"(c.Status =@DealStatus or c.Status =@Dealing)
                                and c.ExecutorSysNo=@ExecutorSysNo
                                and c.TaskType = @TaskType";
            string fromSql = @"SyJobPool c";
            return Context.Select<int>("count(1)")
                    .From(fromSql)
                    .Where(sqlWhere)
                    .Parameter("DealStatus", (int)SystemStatus.任务池状态.待处理)
                    .Parameter("Dealing", (int)SystemStatus.任务池状态.处理中)
                    .Parameter("ExecutorSysNo", executorSysNo)
                    .Parameter("TaskType", taskType)
                    .QuerySingle();
        }

        /// <summary>
        /// 任务池列表查询[客服订单审核10、客服订单提交出库15,通知10][待分配、待处理、处理中、已锁定]
        /// <param name="filter">订单分页查询条件实体</param>
        /// </summary>
        /// <returns>空</returns>
        /// <remarks>2013-11-06 苟治国 创建</remarks>
        public override void GetMessageList(Pager<CBSyJobPool> filter)
        {
            string sqlWhere = " 1=1 ";
            //判断是否绑定所有分销商
            if (!filter.PageFilter.IsBindAllDealer)
            {
                //判断是否绑定分销商
                if (filter.PageFilter.IsBindDealer)
                {
                    sqlWhere += " and dea.SysNo = " + filter.PageFilter.DealerSysNo ;
                }
                else
                {
                    sqlWhere += " and dea.CreatedBy = " + filter.PageFilter.DealerCreatedBy ;
                }
            }
            string sqlTable = @"SyJobPool c left join SyUser b on b.SysNo=c.ExecutorSysNo left join SoOrder a on c.TaskSysNo = a.SysNo 
                                left join DsDealer dea on a.DealerSysNo = dea.SysNo ";
            string sqlFields = "c.TaskSysNo,c.JobDescription,c.JobUrl,c.TaskType,b.username as ExecutorSysName,c.ExecutorSysNo,c.SysNo,c.Status,c.Priority";
            sqlWhere += @"
                        and (@Status=0 or c.Status=@Status)
                        and (@TaskType=0 or (c.TaskType=@TaskType1 or c.TaskType=@TaskType2 )) 
                        and c.ExecutorSysNo=@ExecutorSysNo";
            using (var context = Context.UseSharedConnection(true))
            {
                filter.Rows = context.Select<CBSyJobPool>(sqlFields)
                                    .From(sqlTable)
                                    .Where(sqlWhere)
                                    .Parameter("Status", filter.PageFilter.Status)
                                    .Parameter("TaskType", filter.PageFilter.TaskType)
                                    .Parameter("TaskType1", (int)SystemStatus.任务对象类型.客服订单审核)
                                    .Parameter("TaskType2", (int)SystemStatus.任务对象类型.客服订单提交出库)
                                    //.Parameter("TaskType3", (int)SystemStatus.任务对象类型.通知)
                                    .Parameter("ExecutorSysNo", filter.PageFilter.ExecutorSysNo)
                                    .OrderBy("ISNULL(c.Priority,0) desc,c.SysNo")
                                    .Paging(filter.CurrentPage, filter.PageSize).QueryMany();

                filter.TotalRows = context.Select<int>("count(1)")
                                    .From(sqlTable)
                                    .Where(sqlWhere)
                                    .Parameter("Status", filter.PageFilter.Status)
                                    .Parameter("TaskType", filter.PageFilter.TaskType)
                                    .Parameter("TaskType1", (int)SystemStatus.任务对象类型.客服订单审核)
                                    .Parameter("TaskType2", (int)SystemStatus.任务对象类型.客服订单提交出库)
                                    //.Parameter("TaskType3", (int)SystemStatus.任务对象类型.通知)
                                    .Parameter("ExecutorSysNo", filter.PageFilter.ExecutorSysNo)
                                    .QuerySingle();
            }
        }
        /// <summary>
        /// 获取新订单
        /// </summary>
        /// <param name="IsBindAllDealer"></param>
        /// <param name="IsBindDealer"></param>
        /// <param name="DealerSysNo"></param>
        /// <param name="DealerCreatedBy"></param>
        /// <param name="TaskType"></param>
        /// <param name="ExecutorSysNo"></param>
        /// <returns></returns>
        public override int GetIsNewOrders(bool IsBindAllDealer, bool IsBindDealer, int DealerSysNo, int DealerCreatedBy, int ExecutorSysNo, int TaskType = 10)
        {
            string sqlWhere = " where 1=1 ";
            //判断是否绑定所有分销商
            if (!IsBindAllDealer)
            {
                //判断是否绑定分销商
                if (IsBindDealer)
                {
                    sqlWhere += " and dea.SysNo = " +DealerSysNo;
                }
                else
                {
                    sqlWhere += " and dea.CreatedBy = " + DealerCreatedBy;
                }
            }
            string sqlTable = @"select count(1) from SyJobPool c left join SyUser b on b.SysNo=c.ExecutorSysNo left join SoOrder a on c.TaskSysNo = a.SysNo 
                                left join DsDealer dea on a.DealerSysNo = dea.SysNo ";
            sqlWhere += @"and ((c.Status=20 and PayTypeSysNo<>11) or (c.Status=10 and PayTypeSysNo=11)) and (c.TaskType="+TaskType+") and c.ExecutorSysNo=" + ExecutorSysNo + "";

            return Context.Sql(sqlTable + sqlWhere).QuerySingle<int>();
        }

        /// <summary>
        /// 任务池列表查询[客服订单审核10、客服订单提交出库15]
        /// <param name="filter">订单分页查询条件实体</param>
        /// </summary>
        /// <returns>空</returns>
        /// <remarks>2013-11-06 苟治国 创建</remarks>
        public override void GetMessages(Pager<CBSyJobPool> filter)
        {
            const string queryFields = @"c.TaskSysNo,c.JobDescription,c.JobUrl,c.TaskType,
                               b.username as ExecutorSysName,c.ExecutorSysNo,c.SysNo,c.Status,c.Priority";
            const string sqlWhere = @"(@SysNo = 0 or c.TaskSysNo like @SysNo)
                                      and 
                                    (
                                      (
                                      @QueryStatus = 2 and (c.Status =@DealStatus or c.Status =@Dealing) and (@ExecutorSysNo= 0 or c.ExecutorSysNo=@ExecutorSysNo)
                                      )    --已分配                                                                    
                                      or (@QueryStatus = 3 and c.Status = 2LockStatus)    --已锁定
                                      or (@Status = 1 and c.Status = @AssignStatus)  
                                    ) and (c.TaskType = @TaskType or c.TaskType = @TaskType1 or c.TaskType = @TaskType2)";

            const string fromSql = @"SyJobPool c left join SyUser b on b.SysNo=c.ExecutorSysNo";
            filter.TotalRows = Context.Select<int>("count(1)")
                    .From(fromSql)
                    .Where(sqlWhere)
                    .Parameter("SysNo", filter.PageFilter.TaskSysNo)
                // .Parameter("SysNo", filter.PageFilter.TaskSysNo)
                    .Parameter("QueryStatus", filter.PageFilter.Status)
                    .Parameter("DealStatus", (int)SystemStatus.任务池状态.待处理)
                    .Parameter("Dealing", (int)SystemStatus.任务池状态.处理中)
                    .Parameter("ExecutorSysNo", filter.PageFilter.ExecutorSysNo)
                //.Parameter("ExecutorSysNo", filter.PageFilter.ExecutorSysNo)
                    .Parameter("QueryStatus", filter.PageFilter.Status)
                    .Parameter("LockStatus", (int)SystemStatus.任务池状态.已锁定)
                    .Parameter("Status", filter.PageFilter.Status)
                    .Parameter("AssignStatus", (int)SystemStatus.任务池状态.待分配)
                    .Parameter("TaskType", (int)SystemStatus.任务对象类型.客服订单审核)

                    .Parameter("TaskType1", (int)SystemStatus.任务对象类型.客服订单提交出库)

                    .Parameter("TaskType2", (int)SystemStatus.任务对象类型.通知)

                    .QuerySingle();
            filter.Rows = Context.Select<CBSyJobPool>(queryFields)
                    .From(fromSql)
                    .Where(sqlWhere)
                    .Parameter("SysNo", filter.PageFilter.TaskSysNo)
                //.Parameter("SysNo", filter.PageFilter.TaskSysNo)
                    .Parameter("QueryStatus", filter.PageFilter.Status)
                    .Parameter("DealStatus", (int)SystemStatus.任务池状态.待处理)
                    .Parameter("Dealing", (int)SystemStatus.任务池状态.处理中)
                    .Parameter("ExecutorSysNo", filter.PageFilter.ExecutorSysNo)
                //.Parameter("ExecutorSysNo", filter.PageFilter.ExecutorSysNo)
                    .Parameter("QueryStatus", filter.PageFilter.Status)
                    .Parameter("LockStatus", (int)SystemStatus.任务池状态.已锁定)
                    .Parameter("Status", filter.PageFilter.Status)
                    .Parameter("AssignStatus", (int)SystemStatus.任务池状态.待分配)
                    .Parameter("TaskType", (int)SystemStatus.任务对象类型.客服订单审核)

                    .Parameter("TaskType1", (int)SystemStatus.任务对象类型.客服订单提交出库)

                    .Parameter("TaskType2", (int)SystemStatus.任务对象类型.通知)

                    .OrderBy("ISNULL(c.Priority,0) desc,c.SysNo")
                    .Paging(filter.CurrentPage, filter.PageSize).QueryMany();
        }

        /// <summary>
        /// 取得任务池实体
        /// </summary>
        /// <param name="sysNo">任务池编号</param>
        /// <returns>任务池实体</returns>
        /// <remarks>2013-06-13 余勇 创建</remarks>
        public override SyJobPool Get(int sysNo)
        {
            return Context.Sql("select * from SyJobPool where SysNo=@0", sysNo)
                               .QuerySingle<SyJobPool>();
        }

        /// <summary>
        /// 根据任务编号及任务类型取得任务池实体
        /// </summary>
        /// <param name="soSysNo">任务编号</param>
        /// <param name="jobType">任务类型</param>
        /// <returns>任务池实体</returns>
        /// <remarks>2013-06-13 余勇 创建</remarks>
        public override SyJobPool GetByTask(int soSysNo, int jobType)
        {
            return Context.Sql("select * from SyJobPool where TaskSysNo=@0 and TaskType=@1", soSysNo, jobType)
                               .QuerySingle<SyJobPool>();
        }

        /// <summary>
        /// 插入任务池实体
        /// </summary>
        /// <param name="model">任务池实体</param>
        /// <returns>插入的任务池编号</returns>
        /// <remarks>2013-06-13 余勇 创建</remarks>
        public override int Insert(SyJobPool model)
        {
            return Context.Insert("SyJobPool", model)
                         .AutoMap(x => x.SysNo)
                         .ExecuteReturnLastId<int>("SysNo");
        }

        /// <summary>
        /// 修改任务池
        /// </summary>
        /// <param name="model">任务池实体</param>
        /// <returns>修改的任务池编号</returns>
        /// <remarks>2013-06-13 余勇 创建</remarks>
        public override int Update(SyJobPool model)
        {
            return Context.Update("SyJobPool", model)
                          .AutoMap(o => o.SysNo)
                          .Where("SysNo", model.SysNo).Execute();
        }

        /// <summary>
        /// 删除任务池记录
        /// </summary>
        /// <param name="sysNo">任务池编号</param>
        /// <returns>删除任务池记录</returns>
        /// <remarks>2013-06-13 余勇 创建</remarks>
        public override int Delete(int sysNo)
        {
            return Context.Delete("SyJobPool")
                               .Where("SysNo", sysNo)
                               .Execute();
        }

        /// <summary>
        /// 删除任务池记录
        /// </summary>
        /// <param name="taskSysNo">任务池编号</param>
        /// <param name="taskType">任务类型</param>
        /// <returns>修改的任务池编号</returns>
        /// <remarks>2013-06-13 余勇 创建</remarks>
        public override int Delete(int taskSysNo, int taskType)
        {
            return Context.Delete("SyJobPool")
                               .Where("TaskSysNo", taskSysNo)
                               .Where("TaskType", taskType)
                               .Execute();
        }

        /// <summary>
        /// 修改任务池状态
        /// </summary>
        /// <param name="sysNo">任务池编号</param>
        /// <param name="status">任务池状态值</param> 
        /// <returns>修改的任务池编号</returns>
        /// <remarks>2013-06-18 余勇 创建</remarks>
        public override int UpdateStatus(int sysNo, int status)
        {
            return Context.Update("SyJobPool")
                                .Column("Status", status)
                                .Where("SysNo", sysNo)
                                .Execute();
        }

        /// <summary>
        /// 修改任务池优先级
        /// </summary>
        /// <param name="sysNo">任务池编号</param>
        /// <param name="priority">任务池优先级</param> 
        /// <returns>修改的任务池编号</returns>
        /// <remarks>2014-01-13 余勇 创建</remarks>
        public override int UpdatePriority(int sysNo, int priority)
        {
            return Context.Update("SyJobPool")
                                .Column("Priority", priority)
                                .Where("SysNo", sysNo)
                                .Execute();
        }

        /// <summary>
        /// 修改任务执行者
        /// </summary>
        /// <param name="taskType">任务对象类型</param>
        /// <param name="taskSysNo">任务对象编号</param>
        /// <param name="executorSysNo">ExecutorSysNo</param>
        /// <returns>受影响行数</returns>
        /// <remarks>2013-10-09 黄志勇 创建</remarks>
        public override int UpdateExecutorSysNo(int taskType, int taskSysNo, int executorSysNo)
        {
            return Context.Update("SyJobPool")
                          .Column("ExecutorSysNo", executorSysNo)
                          .Column("Status", (int)SystemStatus.任务池状态.待处理)
                          .Where("TaskType", taskType)
                          .Where("TaskSysNo", taskSysNo)
                          .Execute();
        }

        /// <summary>
        /// 取得任务池任务状态
        /// </summary>
        /// <param name="sysNo">任务池编号</param>
        /// <returns>返回任务池订单状态值</returns>
        /// <remarks>2013-06-13 余勇 创建</remarks>
        public override int GetJobStatus(int sysNo)
        {
            return Context.Sql(@"select status from SyJobPool where SysNo=@0 ", sysNo).QuerySingle<int>();
        }

        /// <summary>
        /// 根据任务类型获取客服组人员键值对
        /// </summary>
        /// <param name="taskType">任务类型</param>
        /// <param name="groupSysNo">客服组编号</param>
        /// <returns>客服人员键值对</returns>
        /// <remarks>2013-10-11 余勇 创建</remarks>
        public override List<CBSyJobPoolUsers> GetJobUserByUserGroup(int taskType, int groupSysNo)
        {
            return Context.Sql(@"select a.SysNo,a.UserName,ISNULL((select count(1) from SyJobPool where executorsysno=a.SysNo and tasktype=(@0) and (Status =@1 or Status =@2) group by executorsysno),0) as TaskNum
                      from SyUser a  
                      where  a.Status=1 and Sysno in( select usersysno from  sygroupuser where groupsysno=@3) order by a.UserName", taskType, (int)SystemStatus.任务池状态.待处理, (int)SystemStatus.任务池状态.处理中, groupSysNo)
                          .QueryMany<CBSyJobPoolUsers>();
        }

        /// <summary>
        /// 获取已分配客服人员键值对
        /// </summary>
        /// <param name="taskType">任务类型</param>
        /// <returns>客服人员键值对</returns>
        /// <remarks>2013-07-03 余勇 创建</remarks>
        public override List<CBSyJobPoolUsers> GetAssignedUsers(int taskType)
        {
            return Context.Sql(@"select distinct a.SysNo,a.UserName
                                  from SyUser a inner join SyJobPool b on a.sysno=b.executorsysno   where b.executorsysno>0 and tasktype=(@0) and
                                   (b.Status =@1 or b.Status =@2)
                                  order by a.UserName", taskType, (int)SystemStatus.任务池状态.待处理, (int)SystemStatus.任务池状态.处理中)
                          .QueryMany<CBSyJobPoolUsers>();
        }

        /// <summary>
        /// 任务池待分配列表查询
        /// </summary>
        /// <returns>任务池待分配列表</returns>
        /// <remarks>2013-11-06 苟治国 创建</remarks>
        /// <remarks>2014-01-14 余勇 修改</remarks>
        public override List<SyJobPool> GetSyJobs()
        {
            return Context.Sql(@"select c.*
                                  from  SyJobPool c  where c.Status= (@0)
                                  ", (int)SystemStatus.任务池状态.待分配)
                        .QueryMany<SyJobPool>();

        }

        /// <summary>
        /// 已关闭自动分配的客服待处理订单
        /// </summary>
        /// <returns>任务池待分配列表</returns>
        /// <remarks>2014-03-11 余勇 创建</remarks>
        public override List<SyJobPool> GetDealingSyJobs()
        {
            return Context.Sql(@"select a.*
                                    from SyJobPool a 
                                    left join SyUser b  on b.sysno=a.executorsysno  left join syjobdispatcher c on b.sysno=c.usersysno 
                                    and c.TaskType=a.tasktype
                                    where   a.status=(@0)                       --待处理
                                        and (c.status=0 or c.status is null)    --自动任务关闭
                                        and (a.taskType=(@1) or a.tasktype=(@2))  --任务类型为订单审核或出库
                                       ", (int)SystemStatus.任务池状态.待处理,
                                      (int)SystemStatus.任务对象类型.客服订单审核,
                                      (int)SystemStatus.任务对象类型.客服订单提交出库).QueryMany<SyJobPool>();
        }

        /// <summary>
        /// 当日达未处理订单
        /// </summary>
        /// <param name="hours">几小时之类未处理</param>
        /// <returns>当日达未处理订单列表</returns>
        /// <remarks>2014-08-05 余勇 创建</remarks>
        public override List<CBSyJobPool> GetDealingOverTimeSyJobs(int hours)
        {
            return Context.Sql(@"select c.*,b.username as ExecutorSysName,b.mobilephonenumber
                                    from SyJobPool c left join SyUser b on b.SysNo=c.ExecutorSysNo
                                    where (c.Status=(@0) or c.Status=(@1))   --待处理或待分配
                                          and (c.taskType=(@2) or c.tasktype=(@3)) --任务类型为订单审核或出库
                                          and  (case when convert(char(10),c.AssignDate,110)='1900-01-01' then floor(datediff(c.AssignDate,SYSDATETIME())*24)
                                            else  floor(datediff(c.AssignDate,SYSDATETIME())*24) end) >=(@4)      --大于几小时未处理
                                       ", (int)SystemStatus.任务池状态.待处理, (int)SystemStatus.任务池状态.待分配,
                                      (int)SystemStatus.任务对象类型.客服订单审核,
                                      (int)SystemStatus.任务对象类型.客服订单提交出库,
                                      hours).QueryMany<CBSyJobPool>();
        }

        /// <summary>
        /// 分页查询当日达未处理订单
        /// </summary>
        /// <param name="hours">几小时之类未处理</param>
        /// <param name="model">订单池查询实体类</param>
        /// <returns>当日达未处理订单列表</returns>
        /// <remarks>2014-08-05 余勇 创建</remarks>
        public override Pager<CBSyJobPool> GetOverTimeSyJobsList(int hours, CBSyJobPool model)
        {
            const string sql = @"(select c.*,b.username as ExecutorSysName,b.mobilephonenumber
                                    from SyJobPool c left join SyUser b on b.SysNo=c.ExecutorSysNo
                                    where (@0 = 0 or c.TaskSysNo like @0)
                                          and(
                                              (@1 = 2 and (c.Status =@2))    --已分配                                                                    
                                              or (@1 = 1 and c.Status = @3)   --待分配
                                              or (@1 = 0 and (c.Status=@2 or c.Status=@3)) --待处理或待分配
                                             )
                                             and (c.taskType=(@4) or c.tasktype=(@5)) --任务类型为订单审核或出库
                                          and (case when convert(char(10),c.AssignDate,110)='1900-01-01' then floor(datediff(c.CreatedDate,SYSDATETIME())*24)
                                               else  floor(datediff(c.AssignDate,SYSDATETIME())*24) end) >=@6
                                    )tb";

            var dataList = Context.Select<CBSyJobPool>("tb.*").From(sql);
            var dataCount = Context.Select<int>("count(0)").From(sql);
            var paras = new object[]
                {
                    model.TaskSysNo,
                    model.Status,
                    (int)SystemStatus.任务池状态.待处理,
                    (int)SystemStatus.任务池状态.待分配,
                    (int)SystemStatus.任务对象类型.客服订单审核,
                    (int)SystemStatus.任务对象类型.客服订单提交出库,
                    hours
                };
            dataList.Parameters(paras);
            dataCount.Parameters(paras);
            var sort = "(case when convert(char(10),AssignDate,110)='1900-01-01' then SYSDATETIME()-CreatedDate else SYSDATETIME()-AssignDate end) desc";
            var pager = new Pager<CBSyJobPool>
            {
                CurrentPage = model.id,
                TotalRows = dataCount.QuerySingle(),
                Rows = dataList.OrderBy(sort).Paging(model.id, model.PageSize).QueryMany()
            };

            return pager;
        }
    }
}
