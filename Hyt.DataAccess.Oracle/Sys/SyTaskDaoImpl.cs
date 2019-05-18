using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Sys;
using Hyt.Model;

namespace Hyt.DataAccess.Oracle.Sys
{
    /// <summary>
    /// 任务计划
    /// </summary>
    /// <remarks>2013-10-15 杨浩 创建</remarks>
    public class SyTaskDaoImpl:ISyTaskDao
    {
        /// <summary>
        /// Add
        /// </summary>
        /// <param name="model">model</param>
        /// <returns>sysNo</returns>
        /// <remarks>2013-10-15 杨浩 创建</remarks>
        public override int Add(SyTaskConfig model)
        {
            return Context.Insert<SyTaskConfig>("SyTaskConfig", model)
                          .AutoMap(o => o.SysNo)
                          .ExecuteReturnLastId<int>("SysNo");
        }

        /// <summary>
        /// GetAll
        /// </summary>
        /// <returns>IList<SyTaskBo></returns>
        /// <remarks>2013-10-16 杨浩 创建</remarks>
        public override IList<SyTaskConfig> GetAll()
        {
            return Context.Sql("select * from SyTaskConfig order by createtime desc").QueryMany<SyTaskConfig>();
        }

        /// <summary>
        /// 获取任务
        /// </summary>
        /// <param name="sysNo">任务系统编号</param>
        /// <returns>SyTaskConfig</returns>
        /// <remarks>2013-10-16 杨浩 创建</remarks>
        public override SyTaskConfig GetTask(int sysNo)
        {
            return Context.Sql("select * from SyTaskConfig where sysNo=@sysNo")
                          .Parameter("sysNo", sysNo)
                          .QuerySingle<SyTaskConfig>();
        }

        /// <summary>
        /// 更新任务
        /// </summary>
        /// <param name="task">任务</param>
        /// <returns>成功行数</returns>
        /// <remarks>2013-10-16 杨浩 创建</remarks>
        public override int UpdateTask(SyTaskConfig task)
        {
            return Context.Update<SyTaskConfig>("SyTaskConfig", task)
                          .AutoMap(x => x.SysNo)
                          .Where(x => x.SysNo)
                          .Execute();
        }

        /// <summary>
        /// 查看任务执行日志
        /// </summary>
        /// <param name="sysNo">任务计划编号</param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <returns>Pager<SyTaskLog></returns>
        /// <remarks>2013-10-16 杨浩 创建</remarks>
        public override Pager<SyTaskLog> GetLogs(int sysNo, int currentPage, int pageSize)
        {
            var pager = new Pager<SyTaskLog> {};

            using (var current = Context.UseSharedConnection(true))
            {
                var count = current.Sql("select count(1) from SyTaskLog where TaskConfigSysNo=@sysNo")
                                   .Parameter("sysNo", sysNo)
                                   .QuerySingle<int>();

                var data = current.Select<SyTaskLog>("log.*")
                                  .From("SyTaskLog log")
                                  .Where("TaskConfigSysNo=@sysNo")
                                  .Parameter("sysNo", sysNo)
                                  .OrderBy("log.sysno desc")
                                  .Paging(currentPage, pageSize).QueryMany();

                pager.CurrentPage = currentPage;
                pager.Rows = data;
                pager.TotalRows = count;
            }

            return pager;

        }

        /// <summary>
        /// 添加一条任务执行日志
        /// </summary>
        /// <param name="model">任务计划日志</param>
        /// <returns>int</returns>
        /// <remarks>2013-10-16 杨浩 创建</remarks>
        public override int AddTaskLog(SyTaskLog model)
        {
            return Context.Insert<SyTaskLog>("SyTaskLog", model)
                          .AutoMap(o => o.SysNo)
                          .ExecuteReturnLastId<int>("SysNo");
        }

        /// <summary>
        /// 定时清理任务日志
        /// </summary>
        /// <param name="sysNo">任务编号</param>
        /// <returns>int</returns>
        /// <remarks>2013-10-18 杨浩 创建</remarks>
        public override void ClearTaskLog(int sysNo)
        {
            #region sql

            string sql = @"delete  from sytasklog where createtime<DATEADD(hh,-24,getdate()) and taskconfigsysno=@taskconfigsysno";

            #endregion

            Context.Sql(sql).Parameter("taskconfigsysno", sysNo).Execute();
        }

        /// <summary>
        /// 发送短信任务
        /// </summary>
        /// <param name="sendCount">发送条数</param>
        /// <returns>返回待发送Top条数</returns>
        /// <remarks>2013-10-22 苟治国 创建</remarks>
        /// <remarks>2013-10-29 黄波 修改了排序方式</remarks>
        public override IList<NcSms> GetSmsTask(int sendCount)
        {
            return Context.Sql("select NcSms.*,row_number() over(order by SysNo) as rownum from NcSms where rownum<=@rownum and Status=@Status order by Priority desc,CreatedDate asc")
                .Parameter("rownum", sendCount)
                .Parameter("Status", (int)Hyt.Model.WorkflowStatus.NotificationStatus.短信发送状态.待发)
                .QueryMany<NcSms>();
        }

        /// <summary>
        /// 更新发送短信失败状态
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <param name="status">状态</param>
        /// <returns>返回操作状态</returns>
        /// <remarks>2013-10-22 苟治国 创建</remarks>
        public override int UpdateSmsTaskStatus(int sysNo, Hyt.Model.WorkflowStatus.NotificationStatus.短信发送状态 status)
        {
            return Context.Sql("update NcSms set status=@status where sysno=@sysno")
                   .Parameter("status", sysNo)
                   .Parameter("sysno", status)
                   .Execute();
        }
    }
}
