using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using Hyt.BLL.Extras;
using Hyt.BLL.Log;
using Hyt.BLL.MallSeller;
using Hyt.BLL.Order;
using Hyt.DataAccess.Order;
using Hyt.DataAccess.Sys;
using Hyt.Model;
using Hyt.Model.Generated;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;

namespace Hyt.BLL.Sys
{
    /// <summary>
    /// 任务管理
    /// </summary>
    /// <remarks>2013-09-27 吴文强 创建</remarks>
    public class SyJobPoolManageBo : BOBase<SyJobPoolManageBo>
    {
        /// <summary>
        /// 任务池列表查询
        /// </summary>
        /// <param name="pageFilter">订单分页查询条件实体</param>
        /// <returns>任务池列表</returns>
        /// <remarks>2013-06-13 余勇 创建</remarks>
        public void GetJobSpoolList(Pager<CBSyJobPool> pageFilter)
        {
            ISyJobPoolDao.Instance.GetJobSpoolList(pageFilter);
        }

        /// <summary>
        /// 任务池列表查询[客服订单审核10、客服订单提交出库15]
        /// </summary>
        /// <param name="pageFilter">订单分页查询条件实体</param>
        /// <returns>空</returns>
        /// <remarks>2013-11-06 苟治国 创建</remarks>
        public void GetMessages(Pager<CBSyJobPool> pageFilter)
        {
            ISyJobPoolDao.Instance.GetMessages(pageFilter);
        }

        /// <summary>
        /// 取得任务池任务实体
        /// </summary>
        /// <param name="sysNo">任务池任务编号</param>
        /// <returns>任务池任务实体</returns>
        /// <remarks>2013-06-13 余勇 创建</remarks>
        public SyJobPool GetJob(int sysNo)
        {
            return ISyJobPoolDao.Instance.Get(sysNo);
        }

        /// <summary>
        /// 修改任务池
        /// </summary>
        /// <param name="model">任务池实体</param>
        /// <returns>修改的任务池编号</returns>
        /// <remarks>2013-06-13 余勇 创建</remarks>
        public int Update(SyJobPool model)
        {
            return ISyJobPoolDao.Instance.Update(model);
        }

        /// <summary>
        ///  分配/申领单个订单
        /// </summary>
        /// <param name="sysNo">任务池编号</param>
        /// <param name="userSysNo">分配客服编号</param>
        /// <param name="assignerSysNo">分配人编号</param>
        /// <returns>任务池编号</returns>
        /// <remarks>2013-06-13 余勇 创建</remarks>
        public int AssignJob(int sysNo, int userSysNo, int assignerSysNo)
        {
            var res = 0;
            var model = ISyJobPoolDao.Instance.Get(sysNo);
            if (model != null)
            {
                model.ExecutorSysNo = userSysNo;
                model.AssignDate = DateTime.Now;
                model.AssignerSysNo = assignerSysNo;
                model.Status = (int)SystemStatus.任务池状态.待处理;
                res = SyJobPoolManageBo.Instance.Update(model);
            }
            //取消记录日志 余勇 2014-10-08
            //else
            //{
            //    SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, string.Format("分配任务池错误:编号为{0}的任务池实体为空", sysNo),
            //                   LogStatus.系统日志目标类型.用户, 0, null);
            //}
            return res;

        }

        /// <summary>
        ///  分配/申领多个订单
        /// </summary>
        /// <param name="sysNos">任务号字符串，多个任务编号用逗号(,)分隔</param>
        /// <param name="userSysNo">分配客服编号</param>
        /// <param name="assignerSysNo">分配人编号</param>
        /// <returns>是否成功</returns>
        /// <remarks>2013-06-13 余勇 创建</remarks>
        public bool AssignJobs(string sysNos, int userSysNo, int assignerSysNo)
        {
            bool res = false;
            if (!string.IsNullOrEmpty(sysNos) && userSysNo > 0)
            {
                string[] ids = sysNos.Split(',');
                foreach (string id in ids)
                {
                    if (!string.IsNullOrEmpty(id))
                    {
                        AssignJob(int.Parse(id), userSysNo, assignerSysNo);
                        var model = ISyJobPoolDao.Instance.Get(int.Parse(id));
                        if (model != null)
                        {
                            SyJobDispatcherBo.Instance.WriteJobLog(string.Format("{0}分配{1}任务{2}给{3}",
                                SyUserBo.Instance.GetUserName(assignerSysNo),
                                Hyt.Util.EnumUtil.GetDescription(
                                    typeof(Hyt.Model.WorkflowStatus.SystemStatus.任务对象类型),
                                    model.TaskType), model.TaskSysNo, SyUserBo.Instance.GetUserName(userSysNo)), model.TaskSysNo, null, assignerSysNo);
                        }
                    }
                }
                res = true;
            }
            return res;
        }

        /// <summary>
        /// 解除申领的订单
        /// </summary>
        /// <param name="sysNos">任务池编号字符串，多个任务池编号用逗号(,)分隔</param>
        /// <param name="userSysno">用户编号</param>
        /// <returns>是否成功</returns>
        /// <remarks>2013-06-13 余勇 创建</remarks>
        public bool ReleaseJob(string sysNos, int userSysno)
        {
            bool res = false;
            if (!string.IsNullOrEmpty(sysNos))
            {
                string[] ids = sysNos.Split(',');
                foreach (string id in ids)
                {
                    if (!string.IsNullOrEmpty(id))
                    {

                        var model = ISyJobPoolDao.Instance.Get(int.Parse(id));
                        model.ExecutorSysNo = 0;
                        model.AssignDate = DateTime.Now;
                        model.Status = (int)SystemStatus.任务池状态.待分配;
                        int rownum = SyJobPoolManageBo.Instance.Update(model);
                        SyJobDispatcherBo.Instance.WriteJobLog(string.Format("{0}解除申领的订单{1}",
                            SyUserBo.Instance.GetUserName(userSysno),
                            model.TaskSysNo), int.Parse(id), null, userSysno);


                    }
                }
                res = true;
            }
            return res;
        }

        /// <summary>
        /// 解锁订单
        /// </summary>
        /// <param name="sysNos">任务池编号数组</param>
        /// <param name="userId">操作用户编号</param>
        /// <returns>是否成功</returns>
        /// <remarks>2014-06-16 余勇 创建</remarks>
        public bool UnLockJob(int[] sysNos, int userId)
        {
            var res = false;
            if (sysNos != null && sysNos.Length > 0)
            {
                foreach (var id in sysNos)
                {
                    if (id > 0)
                    {
                        int taskSysNo = 0;
                        SyJobPool job = SyJobPoolManageBo.Instance.GetJob(id);
                        if (job != null)
                        {
                            //如果任务执行者不为空，修改状态为待处理
                            if (job.ExecutorSysNo > 0)
                            {
                                job.Status = (int)SystemStatus.任务池状态.待处理;
                            }
                            else
                            {
                                job.Status = (int)SystemStatus.任务池状态.待分配;
                            }
                            Update(job);
                            taskSysNo = job.TaskSysNo;
                        }
                        //删除锁定任务
                        SyLockJobBo.Instance.Delete(id);
                        SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Info, LogStatus.系统日志来源.后台, "解锁成功，任务对象编号:" + taskSysNo,
                            LogStatus.系统日志目标类型.任务池, id, null, null, userId);
                    }
                }
                res = true;
            }
            return res;
        }

        /// <summary>
        /// 锁定订单
        /// </summary>
        /// <param name="sysNos">任务池编号数组</param>
        /// <param name="unLockState">是否自动解锁</param>
        /// <param name="remarks">备注</param>
        /// <param name="unLockDate">解锁日期</param>
        /// <param name="userId">操作用户编号</param>
        /// <returns>是否成功</returns>
        /// <remarks>2013-06-19 余勇 创建</remarks>
        public bool LockJob(int[] sysNos, int unLockState, DateTime? unLockDate, string remarks, int userId)
        {
            var res = false;
            if (sysNos != null && sysNos.Length > 0)
            {
                foreach (var id in sysNos)
                {
                    if (id > 0)
                    {
                        int taskSysNo = 0;
                        SyJobPool job = SyJobPoolManageBo.Instance.GetJob(id);
                        if (job != null)
                        {
                            //string remark = remarks;
                            //if (unLockState > 0 && unLockDate != null)
                            //{
                            //    remark = remarks +
                            //             string.Format("(自动解锁时间:'{0}')", unLockDate.Value.ToString("yyyy-MM-dd HH:mm"));
                            //}
                            job.Remarks = remarks;
                            job.Status = (int)SystemStatus.任务池状态.已锁定;
                            Update(job);
                            taskSysNo = job.TaskSysNo;
                        }
                        //先删除任务
                        SyLockJobBo.Instance.Delete(id);
                        //插入锁定任务表
                        var model = new SyLockJob
                        {
                            JobPoolSysNo = id,
                            UnLockState = unLockState,
                            CreatedBy = userId,
                            CreatedDate = DateTime.Now
                        };
                        if (unLockState > 0 && unLockDate != null)
                        {
                            model.UnLockDate = unLockDate.Value;
                        }
                        SyLockJobBo.Instance.Insert(model);
                        SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Info, LogStatus.系统日志来源.后台, "锁定任务，任务对象编号:" + taskSysNo,
                                LogStatus.系统日志目标类型.任务池, id, null, null, userId);
                    }
                }
                res = true;
            }
            return res;
        }

        /// <summary>
        /// 取得订单状态
        /// </summary>
        /// <param name="sysNo">任务池系统编号</param>
        /// <returns>订单状态值</returns>
        /// <remarks>2013-6-19 余勇 创建</remarks>
        public int GetJobStatus(int sysNo)
        {
            return ISyJobPoolDao.Instance.GetJobStatus(sysNo);
        }

        /// <summary>
        /// 根据任务类型获取客服人员键值对
        /// </summary>
        /// <param name="taskType">任务类型</param>
        /// <param name="groupSysNo">客服组编号</param>
        /// <returns>客服人员键值对</returns>
        /// <remarks>2013-10-11 余勇 创建</remarks>
        public List<CBSyJobPoolUsers> GetJobUserByUserGroup(int taskType, int groupSysNo)
        {
            return ISyJobPoolDao.Instance.GetJobUserByUserGroup(taskType, groupSysNo);
        }

        /// <summary>
        /// 修改任务池优先级
        /// </summary>
        /// <param name="sysNo">任务池编号</param>
        /// <param name="priority">任务池优先级</param> 
        /// <returns>修改的任务池编号</returns>
        /// <remarks>2014-01-13 余勇 创建</remarks>
        public int UpdatePriority(int sysNo, int priority)
        {
            return ISyJobPoolDao.Instance.UpdatePriority(sysNo, priority);
        }

        /// <summary>
        /// 获取已分配客服人员键值对
        /// </summary>
        /// <param name="taskType">任务类型编号</param>
        /// <returns>客服人员键值对</returns>
        /// <remarks>2013-07-03 余勇 创建</remarks>
        public List<CBSyJobPoolUsers> GetAssignedUsers(int taskType)
        {
            return ISyJobPoolDao.Instance.GetAssignedUsers(taskType);
        }

        /// <summary>
        /// 根据订单号及任务类型取得任务池实体
        /// </summary>
        /// <param name="soSysNo">任务编号</param>
        /// <param name="jobType">任务类型</param>
        /// <returns>任务池实体</returns>
        /// <remarks>2013-06-2-20 余勇 创建</remarks>
        public SyJobPool GetByTask(int soSysNo, int jobType)
        {
            return ISyJobPoolDao.Instance.GetByTask(soSysNo, jobType);
        }

        /// <summary>
        /// 任务池列表查询[客服订单审核10、客服订单提交出库15,通知10][待分配、待处理、处理中、已锁定]
        /// <param name="filter">订单分页查询条件实体</param>
        /// </summary>
        /// <returns>空</returns>
        /// <remarks>2014-02-28 苟治国 创建</remarks>
        public void GetMessageList(Pager<CBSyJobPool> filter)
        {
            ISyJobPoolDao.Instance.GetMessageList(filter);
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
        public int GetIsNewOrders(bool IsBindAllDealer, bool IsBindDealer, int DealerSysNo, int DealerCreatedBy, int ExecutorSysNo, int TaskType = 10)
        {
            return ISyJobPoolDao.Instance.GetIsNewOrders(IsBindAllDealer, IsBindDealer, DealerSysNo, DealerCreatedBy, ExecutorSysNo, TaskType);
        }

        /// <summary>
        /// 删除任务池记录
        /// </summary>
        /// <param name="taskSysNo">任务池编号</param>
        /// <param name="taskType">任务类型</param>
        /// <returns>删除的任务池编号</returns>
        /// <remarks>2013-09-28 余勇 创建</remarks>
        public int DeleteJobPool(int taskSysNo, int taskType)
        {
            return ISyJobPoolDao.Instance.Delete(taskSysNo, taskType);
        }

        /// <summary>
        ///  删除任务池记录
        /// </summary>
        /// <param name="sysNo">任务系统编号</param>
        /// <returns>删除的任务池编号</returns>
        /// <remarks>2014-02-20 邵斌 创建</remarks>
        public int DeleteJobPool(int sysNo)
        {
            return ISyJobPoolDao.Instance.Delete(sysNo);

        }

        /// <summary>
        /// 修改任务池执行者
        /// </summary>
        /// <param name="taskType">任务对象类型</param>
        /// <param name="taskSysNo">任务对象编号</param>
        /// <param name="executorSysNo">ExecutorSysNo</param>
        /// <returns>受影响行数</returns>
        /// <remarks>2013-10-09 黄志勇 创建</remarks>
        public int UpdateExecutorSysNo(int taskType, int taskSysNo, int executorSysNo)
        {
            return ISyJobPoolDao.Instance.UpdateExecutorSysNo(taskType, taskSysNo, executorSysNo);
        }

        /// <summary>
        /// 获取用户待处理任务数
        /// </summary>
        /// <param name="executorSysNo">用户编号</param>
        /// <param name="taskType">任务类型</param>
        /// <returns>用户待处理任务数</returns>
        /// <remarks>2013-09-28 余勇 创建</remarks>
        public int GetJobsNumByUser(int executorSysNo, int taskType)
        {
            return ISyJobPoolDao.Instance.GetJobsNumByUser(executorSysNo, taskType);
        }

        /// <summary>
        /// 任务池自动分配
        /// </summary>
        /// <returns>空</returns>
        /// <remarks>2013-09-28 余勇 创建</remarks>
        public void AutoAssignJob()
        {
            var list = ISyJobPoolDao.Instance.GetSyJobs();

            var executorList = SyJobDispatcherBo.Instance.GetAll().Where(p => p.Status == 1).ToList();

            Hyt.BLL.Sys.AutoAssignJob.Instance.Execute(list, executorList, (syJobPool, executorSysNo) =>
                {
                    if (syJobPool != null)
                    {
                        AssignJob(syJobPool.SysNo, executorSysNo, 0);
                        SyJobDispatcherBo.Instance.WriteJobLog(string.Format("任务池自动分配任务给{0}，任务对象类型:{1}，任务对象编号:{2}",
                            SyUserBo.Instance.GetUserName(executorSysNo),
                            Hyt.Util.EnumUtil.GetDescription(
                                typeof(Hyt.Model.WorkflowStatus.SystemStatus.任务对象类型),
                                syJobPool.TaskType), syJobPool.TaskSysNo), syJobPool.TaskSysNo, null, 0);
                    }
                    else
                    {
                        SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "定时分配任务池错误:任务池实体为空",
                                LogStatus.系统日志目标类型.用户, 0, null);
                    }
                });

            //已关闭自动分配的客服待处理订单(排除客服自己创建的订单)
            var dealingList = ISyJobPoolDao.Instance.GetDealingSyJobs();
            foreach (var syJobPool in dealingList)
            {
                if (syJobPool.TaskType != (int)SystemStatus.任务对象类型.客服订单审核 &&
                    syJobPool.TaskType != (int)SystemStatus.任务对象类型.客服订单提交出库) continue;
                syJobPool.ExecutorSysNo = 0;
                syJobPool.Status = (int)SystemStatus.任务池状态.待分配;
                syJobPool.AssignDate = DateTime.Now;
                Update(syJobPool);

                SyJobDispatcherBo.Instance.WriteJobLog(string.Format("自动回收分配的客服待处理订单，任务对象类型:{0}，任务对象编号:{1}",
                            Hyt.Util.EnumUtil.GetDescription(
                                typeof(Hyt.Model.WorkflowStatus.SystemStatus.任务对象类型),
                                syJobPool.TaskType), syJobPool.TaskSysNo), syJobPool.TaskSysNo, null, 0);
            }

            //将超过解锁日期的锁定任务解锁 余勇 增加 2014-06-17
            var lockJobList = SyLockJobBo.Instance.GetOverTimeLockJobs();
            if (lockJobList != null && lockJobList.Count > 0)
            {
                UnLockJob(lockJobList.ToArray(), 0);
            }

            #region 发现百城当日达的订单，如果在几小时之内还没有被处理就给管理人发一条短信
            //从短信配置表中取得未处理提醒时间、短信接受人，最近处理时间
            var configModel = SyJobSmsConfigBo.Instance.GetFirst();
            if (configModel != null)
            {
                var userSysNo = configModel.ReceiveSysNo;
                var hours = configModel.MaxDealTime;
                var lastSendTime = configModel.LastSendTime;
                var dealingJobs = GetDealingOverTimeSyJobs(hours);
                if (dealingJobs != null && dealingJobs.Count > 0)
                {
                    var msg = "温馨提示:您有订单超过{0}小时未处理,请及时办理！";
                    //最近处理时间超过未处理提醒时间就发送短信给管理员
                    if (lastSendTime.ToString("yyyy-MM-dd") == "0001-01-01" || (DateTime.Now - lastSendTime).TotalHours >= hours)
                    {
                        //发送短信
                        AdminSmsBO.Instance.Send(userSysNo, string.Format(msg, hours), null);
                        //写入最后发送时间
                        configModel.LastSendTime = DateTime.Now;
                        SyJobSmsConfigBo.Instance.Update(configModel);
                    }
                }
            }
            #endregion
        }

        /// <summary>
        /// 通过任务编号及类型分配任务给指定人
        /// </summary>
        /// <param name="taskType">任务类型</param>
        /// <param name="taskSysNo">任务编号</param>
        /// <param name="executorSysNo">任务执行人</param>
        /// <returns>任务编号</returns>
        /// <remarks>2013-09-28 余勇 创建</remarks>
        public int AssignJobByTaskType(int taskType, int taskSysNo, int executorSysNo)
        {
            SyJobPool job = null;
            switch (taskType)
            {
                case (int)SystemStatus.任务对象类型.客服订单审核:
                    job = SyJobPoolPublishBo.Instance.OrderAuditBySysNo(taskSysNo, executorSysNo);
                    break;
                case (int)SystemStatus.任务对象类型.客服订单提交出库:
                    job = SyJobPoolPublishBo.Instance.OrderWaitStockOut(taskSysNo, "", "", executorSysNo);
                    break;
                case (int)SystemStatus.任务对象类型.商品评论审核:
                    job = SyJobPoolPublishBo.Instance.ProductCommentAudit(taskSysNo, "", "", executorSysNo);
                    break;
                case (int)SystemStatus.任务对象类型.商品评论回复审核:
                    job = SyJobPoolPublishBo.Instance.ProductCommentReplayAudit(taskSysNo, "", "", executorSysNo);
                    break;
                case (int)SystemStatus.任务对象类型.商品晒单审核:
                    job = SyJobPoolPublishBo.Instance.ProductShareAudit(taskSysNo, "", "", executorSysNo);
                    break;
            }
            return job != null ? job.SysNo : 0;
        }

        /// <summary>
        /// 任务池处理
        /// </summary>
        /// <param name="taskType">任务类型</param>
        /// <param name="taskSysNo">任务系统编号</param>
        /// <param name="currentSysNo">当前用户编号</param>
        /// <returns>任务池</returns>
        /// <remarks>2013-11-6 黄志勇 修改</remarks>
        public CBProcJobPool ProcJobPool(int taskType, int taskSysNo, int currentSysNo)
        {
            var cBProcJobPool = new CBProcJobPool();
            SyUser user = null;
            var jobPool = Instance.GetByTask(taskSysNo, taskType);
            if (jobPool != null)
            {
                if (jobPool.Status == (int)SystemStatus.任务池状态.已锁定)
                {
                    cBProcJobPool.IsLocked = true;
                }
                user = SyUserBo.Instance.GetSyUser(jobPool.ExecutorSysNo);
            }
            int executorSysNo = user != null ? user.SysNo : 0;
            cBProcJobPool.IsDisabled = taskType > 0 && executorSysNo > 0 && executorSysNo != currentSysNo;  //当前用户不是任务执行者，需禁用
            return cBProcJobPool;
        }

        /// <summary>
        /// 几小时内当日达未处理订单
        /// </summary>
        /// <param name="hours">几小时之类未处理</param>
        /// <returns>当日达未处理订单列表</returns>
        /// <remarks>2014-08-05 余勇 创建</remarks>
        public List<CBSyJobPool> GetDealingOverTimeSyJobs(int hours)
        {
            return ISyJobPoolDao.Instance.GetDealingOverTimeSyJobs(hours);
        }

        /// <summary>
        /// 分页查询当日达未处理订单
        /// </summary>
        /// <param name="hours">几小时之类未处理</param>
        /// <param name="model">订单池查询实体类</param>
        /// <returns>当日达未处理订单列表</returns>
        /// <remarks>2014-08-05 余勇 创建</remarks>
        public Pager<CBSyJobPool> GetOverTimeSyJobsList(int hours, CBSyJobPool model)
        {
            return ISyJobPoolDao.Instance.GetOverTimeSyJobsList(hours, model);
        }
    }
}
