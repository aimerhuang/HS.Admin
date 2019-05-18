using Hyt.DataAccess.Base;
using Hyt.Model;
using System.Collections.Generic;

namespace Hyt.DataAccess.Sys
{
    /// <summary>
    /// 任务池
    /// </summary>
    /// <remarks>2014-01-20 苟治国 追加</remarks>
    public abstract class ISyJobPoolDao : DaoBase<ISyJobPoolDao>
    {
        /// <summary>
        /// 任务池列表查询
        /// </summary>
        /// <param name="pageFilter">订单分页查询条件实体</param>
        /// <remarks>2013-06-13 余勇 创建</remarks>
        public abstract void GetJobSpoolList(Pager<CBSyJobPool> pageFilter);

        /// <summary>
        /// 任务池列表查询[客服订单审核10、客服订单提交出库15]
        /// <param name="filter">订单分页查询条件实体</param>
        /// </summary>
        /// <remarks>2013-11-06 苟治国 创建</remarks>
        public abstract void GetMessages(Pager<CBSyJobPool> filter);

        /// <summary>
        /// 任务池列表查询[客服订单审核10、客服订单提交出库15,通知10][待分配、待处理、处理中、已锁定]
        /// <param name="filter">订单分页查询条件实体</param>
        /// </summary>
        /// <returns>空</returns>
        /// <remarks>2014-02-28 苟治国 创建</remarks>
        public abstract void GetMessageList(Pager<CBSyJobPool> filter);
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
        public abstract int GetIsNewOrders(bool IsBindAllDealer, bool IsBindDealer, int DealerSysNo, int DealerCreatedBy, int ExecutorSysNo, int TaskType = 10);

        /// <summary>
        /// 取得任务池实体
        /// </summary>
        /// <param name="sysNo">任务池编号</param>
        /// <returns>任务池实体</returns>
        /// <remarks>2013-06-13 余勇 创建</remarks>
        public abstract SyJobPool Get(int sysNo);

        /// <summary>
        /// 根据订单号及任务类型取得任务池实体
        /// </summary>
        /// <param name="soSysNo">任务编号</param>
        /// <param name="jobType">任务类型</param>
        /// <returns>任务池实体</returns>
        /// <remarks>2013-06-2-20 余勇 创建</remarks>
        public abstract SyJobPool GetByTask(int soSysNo, int jobType);

        /// <summary>
        /// 插入任务池实体
        /// </summary>
        /// <param name="model">任务池实体</param>
        /// <returns>插入的任务池编号</returns>
        /// <remarks>2013-06-13 余勇 创建</remarks>
        public abstract int Insert(SyJobPool model);

        /// <summary>
        /// 修改任务池
        /// </summary>
        /// <param name="model">任务池实体</param>
        /// <returns>修改的任务池编号</returns>
        /// <remarks>2013-06-13 余勇 创建</remarks>
        public abstract int Update(SyJobPool model);

        /// <summary>
        /// 删除任务池记录
        /// </summary>
        /// <param name="sysNo">任务池编号</param>
        /// <returns>删除任务池记录</returns>
        /// <remarks>2013-06-13 余勇 创建</remarks>
        public abstract int Delete(int sysNo);

        /// <summary>
        /// 通过任务类型和编号删除任务池记录
        /// </summary>
        /// <param name="taskSysNo">任务池编号</param>
        /// <param name="taskType">任务类型</param>
        /// <returns>修改的任务池编号</returns>
        /// <remarks>2013-06-13 余勇 创建</remarks>
        public abstract int Delete(int taskSysNo, int taskType);
        
        /// <summary>
        /// 修改任务池状态
        /// </summary>
        /// <param name="sysNo">任务池编号</param>
        /// <param name="status">任务池状态值</param>
        /// <returns>修改的任务池编号</returns>
        /// <remarks>2013-06-13 余勇 创建</remarks>
        public abstract int UpdateStatus(int sysNo, int status);

        /// <summary>
        /// /// 修改任务执行者
        /// </summary>
        /// <param name="taskType">任务对象类型</param>
        /// <param name="taskSysNo">任务对象编号</param>
        /// <param name="executorSysNo">ExecutorSysNo</param>
        /// <returns>受影响行数</returns>
        /// <remarks>2013-10-09 黄志勇 创建</remarks>
        public abstract int UpdateExecutorSysNo(int taskType, int taskSysNo, int executorSysNo);

        /// <summary>
        /// 取得任务池订单状态
        /// </summary>
        /// <param name="sysNo">任务池编号</param>
        /// <returns>返回任务池订单状态值</returns>
        /// <remarks>2013-06-13 余勇 创建</remarks>
        public abstract int GetJobStatus(int sysNo);

        /// <summary>
        /// 获取已分配客服人员键值对
        /// </summary>
        /// <param name="taskType">任务类型</param>
        /// <returns>客服人员键值对</returns>
        /// <remarks>2013-07-03 余勇 创建</remarks>
        public abstract List<CBSyJobPoolUsers> GetAssignedUsers(int taskType);

        /// <summary>
        /// 任务池待分配列表查询
        /// </summary>
        /// <returns>任务池待分配列表</returns>
        /// <remarks>2013-11-08 余勇 创建</remarks>
        public abstract List<SyJobPool> GetSyJobs();

        /// <summary>
        /// 获取用户待处理任务数
        /// </summary>
        /// <param name="executorSysNo">用户编号</param>
        /// <param name="taskType">任务类型</param>
        /// <returns>用户待处理任务数</returns>
        /// <remarks>2013-11-08 余勇 创建</remarks>
        public abstract int GetJobsNumByUser(int executorSysNo, int taskType);

        /// <summary>
        /// 根据任务类型获取客服人员键值对
        /// </summary>
        /// <param name="taskType">任务类型</param>
        /// <param name="groupSysNo">客服组编号</param>
        /// <returns>客服人员键值对</returns>
        /// <remarks>2013-10-11 余勇 创建</remarks>
        public abstract List<CBSyJobPoolUsers> GetJobUserByUserGroup(int taskType, int groupSysNo);

        /// <summary>
        /// 修改任务池优先级
        /// </summary>
        /// <param name="sysNo">任务池编号</param>
        /// <param name="priority">任务池优先级</param> 
        /// <returns>修改的任务池编号</returns>
        /// <remarks>2014-01-13 余勇 创建</remarks>
        public abstract int UpdatePriority(int sysNo, int priority);

        /// <summary>
        /// 已关闭自动分配的客服待处理订单
        /// </summary>
        /// <returns>任务池待分配列表</returns>
        /// <remarks>2014-03-11 余勇 创建</remarks>
        public abstract List<SyJobPool> GetDealingSyJobs();

        /// <summary>
        /// 当日达未处理订单
        /// </summary>
        /// <param name="hours">几小时之类未处理</param>
        /// <returns>当日达未处理订单列表</returns>
        /// <remarks>2014-08-05 余勇 创建</remarks>
        public abstract List<CBSyJobPool> GetDealingOverTimeSyJobs(int hours);

        /// <summary>
        /// 分页查询当日达未处理订单
        /// </summary>
        /// <param name="hours">几小时之类未处理</param>
        /// <param name="model">订单池查询实体类</param>
        /// <returns>当日达未处理订单列表</returns>
        /// <remarks>2014-08-05 余勇 创建</remarks>
        public abstract Pager<CBSyJobPool> GetOverTimeSyJobsList(int hours, CBSyJobPool model);
    }
}