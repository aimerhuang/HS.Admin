using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Sys;
using Hyt.Model.WorkflowStatus;

namespace Hyt.BLL.Sys
{
    /// <summary>
    /// 订单池任务自动分配
    /// </summary>
    /// <remarks>
    /// 2013-12-5 杨文兵 创建 
    /// </remarks>
    public class AutoAssignJob
    {
        #region singleton

        private static AutoAssignJob instance;

        private AutoAssignJob() { }

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <remarks>
        /// 2013-12-5 杨文兵 创建 
        /// </remarks>
        public static AutoAssignJob Instance
        {
            get
            {
                return Nested.Instance;
            }
        }

        /// <summary>
        /// 单例私有类
        /// </summary>
        class Nested
        {
            static Nested() { }
            internal static readonly AutoAssignJob Instance = new AutoAssignJob();
        }

        #endregion

        /// <summary>
        /// 任务分配是否正在执行
        /// </summary>
        private bool inExecuting = false;

        /// <summary>
        /// 工作量平均分配 最后重置时间 1小时重置一次
        /// </summary>
        private DateTime lastResetTime = DateTime.Now;

        /// <summary>
        /// 自动任务处理记录
        /// </summary>
        private IList<AutoAssignJobModel> autoAssignJobModelList = new List<AutoAssignJobModel>();

        /// <summary>
        /// 回调委托
        /// </summary>
        private Action<Model.SyJobPool, int> action = delegate(Model.SyJobPool syJobPool, int executorSysNo) { };

        /// <summary>
        /// 启用客服(任务执行者)优先级
        /// </summary>
        private bool enabledExecutorPriority = false;


        /// <summary>
        /// 开始执行任务分配
        /// </summary>
        /// <param name="waitingForAssignJobList">等待分配的任务列表.</param>
        /// <param name="jobExecutorList">任务执行者列表.</param>
        /// <param name="action">该委托接收二个参数  1.任务实体  2.任务执行者sysno</param>
        /// <returns>空</returns>
        /// <remarks>2013-12-5 杨文兵 创建 
        /// 2014-5-4 杨文兵 增加breakFlag标识，用于判断如果连续3个任务没有找到合适的客服，则跳出任务分配。(取消此功能)
        /// </remarks>
        public void Execute(IList<Model.SyJobPool> waitingForAssignJobList, IList<Model.SyJobDispatcher> jobExecutorList, Action<Model.SyJobPool, int> action)
        {
            if (inExecuting == true) return;
            if (waitingForAssignJobList == null || jobExecutorList == null) return;

            //保持1小时之内分配给客服的工作量平均
            if (DateTime.Now.AddHours(-1) >= lastResetTime)
            {
                this.Reset();
            }

            int executorSysNo = 0;

            //int breakFlag = 0;


            waitingForAssignJobList = waitingForAssignJobList.OrderBy(p => p.CreatedDate).OrderByDescending(p => p.Priority).ToList();

            //查询出待处理任务的所有任务类型
            IList<int> taskTypeList = new List<int>();
            foreach (var syJobPool in waitingForAssignJobList)
            {
                if (taskTypeList.Contains(syJobPool.TaskType) == false) {
                    taskTypeList.Add(syJobPool.TaskType);
                }
            }

            //待处理的任务 按任务类型分组执行。如果某一分组中的任务没有找到合适的客服则跳出循环，各分组不彼此影响
            foreach (var taskType in taskTypeList) {

                foreach (var syJobPool in waitingForAssignJobList)
                {
                    if (syJobPool.TaskType != taskType) continue;

                    executorSysNo = GetTaskExecutorSysNo(syJobPool, jobExecutorList);
                    if (executorSysNo > 0)
                    {
                        action(syJobPool, executorSysNo);
                        this.WriteAssignLog(executorSysNo, syJobPool.TaskType);
                        //if (breakFlag > 0) breakFlag--;
                    }
                    else
                    {
                        break;
                        //breakFlag++;
                    }
                    //if (breakFlag >= 3) break;
                }

            }

            inExecuting = false;
        }

        /// <summary>
        /// 写分配日志记录
        /// </summary>
        /// <param name="executorSysNo">执行人</param>
        /// <param name="taskType">任务类型</param>
        /// <returns>空</returns>
        /// <remarks>2013-12-5 杨文兵 创建 </remarks>
        private void WriteAssignLog(int executorSysNo, int taskType)
        {
            var logModel = autoAssignJobModelList.FirstOrDefault(p => p.ExecutorSysNo == executorSysNo && p.TaskType == taskType);
            if (logModel == null)
            {
                autoAssignJobModelList.Add(new AutoAssignJobModel()
                {
                    ExecutorSysNo = executorSysNo,
                    TaskType = taskType,
                    HandledNum = 1
                });
            }
            else
            {
                logModel.HandledNum++;
            }
        }

        /// <summary>
        /// 获取任务最合适的执行客服sysno
        /// </summary>
        /// <param name="task">任务.</param>
        /// <param name="jobExecutorList">任务执行者列表.</param>
        /// <returns>
        /// 最合适的执行客服sysno
        /// </returns>
        /// <remarks>
        /// 2013-12-5 杨文兵 创建
        /// </remarks>
        private int GetTaskExecutorSysNo(Model.SyJobPool task, IList<Model.SyJobDispatcher> jobExecutorList)
        {
            if (jobExecutorList == null) return 0;
            if (task == null) return 0;
            if (task.Priority <= 2) task.Priority = 2; //处理旧数据：没有优先级设置默认优先级为2 增强兼容性

            var executorList = jobExecutorList.Where(p => p.TaskType == task.TaskType && p.Status == 1).OrderByDescending(p => p.MaxTaskQuantity);
            if (executorList == null || executorList.Count() < 1) return 0;

            foreach (var executor in executorList)
            {
                //executor.Prioritys
                if (autoAssignJobModelList.Count(p => p.ExecutorSysNo == executor.UserSysNo && p.TaskType == task.TaskType) < 1)
                {
                    var prioritys = executor.Prioritys != null
                                        ? executor.Prioritys.Split(';').ToList()
                                        : new List<string>();
                    autoAssignJobModelList.Add(new AutoAssignJobModel()
                    {
                        ExecutorSysNo = executor.UserSysNo,
                        TaskType = task.TaskType,
                        HandledNum = 0,
                        Priorities = prioritys
                    });
                }
            }

            int waitedQuqntity = 0;
            int maxTaskQuantity = 0;
            Model.SyJobDispatcher currentExecutor = null;

            var taskAutoAssignJobModelList = autoAssignJobModelList.Where(p => p.TaskType == task.TaskType ).OrderBy(p => p.HandledNum).ToList();
            if (enabledExecutorPriority == true) {
                //启用客服能处理的任务优先级配置
                taskAutoAssignJobModelList = taskAutoAssignJobModelList.Where(p => p.Priorities.Contains(task.Priority.ToString())).ToList();
            }

            foreach (var executor in taskAutoAssignJobModelList)
            {
                waitedQuqntity = SyJobPoolManageBo.Instance.GetJobsNumByUser(executor.ExecutorSysNo, task.TaskType);
                currentExecutor = executorList.FirstOrDefault(p => p.UserSysNo == executor.ExecutorSysNo && p.TaskType == task.TaskType);
                maxTaskQuantity = currentExecutor != null ? currentExecutor.MaxTaskQuantity : 0;
                //如果"执行者待处理的任务数量" >= "执行者的最大任务数" 则选择下一个执行者
                if (waitedQuqntity >= maxTaskQuantity) continue;
                return executor.ExecutorSysNo;
            }
            return 0;
        }

        /// <summary>
        /// 重置
        /// </summary>
        public void Reset()
        {
            this.autoAssignJobModelList.Clear();
            lastResetTime = DateTime.Now;
        }

    }

    /// <summary>
    /// 自动任务处理Model
    /// 
    /// </summary>
    public class AutoAssignJobModel
    {
        private IList<string> _priorities = new List<string>();

        /// <summary>
        /// 执行人SysNo
        /// </summary>
        public int ExecutorSysNo { get; set; }

        /// <summary>
        /// 任务类型
        /// </summary>
        public int TaskType { get; set; }

        /// <summary>
        /// 1小时内 处理数量合计
        /// </summary>
        public int HandledNum { get; set; }

        /// <summary>
        /// 能处理的优先级 
        /// </summary>
        public IList<string> Priorities
        {
            get { return _priorities; }
            set { _priorities = value; }
        }

    }
}
