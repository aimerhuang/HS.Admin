using Hyt.BLL.Log;
using Hyt.DataAccess.Sys;
using Hyt.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Model.WorkflowStatus;

namespace Hyt.BLL.Sys
{
    /// <summary>
    /// 任务池分配
    /// </summary>
    /// <remarks>2013-06-21 余勇 创建</remarks>
    public class SyJobDispatcherBo : BOBase<SyJobDispatcherBo>
    {

        /// <summary>
        /// 插入自动分配记录
        /// </summary>
        /// <param name="model">自动分配实体</param>
        /// <returns>返回系统编号</returns>
        /// <remarks>2013-06-21 余勇 创建</remarks>
        public int InsertJobDispatcher(SyJobDispatcher model)
        {
            return ISyJobDispatcher.Instance.InsertJobDispatcher(model);
        }

        /// <summary>
        /// 通过用户编号跟任务类型获取订单分配信息
        /// </summary>
        /// <param name="userSysNo">用户编号</param>
        /// <param name="taskType">任务类型</param>
        /// <returns>返回任务分配实体</returns>
        /// <remarks>2013-06-21 余勇 创建</remarks>
        public SyJobDispatcher GetJobDispatcher(int userSysNo, int taskType)
        {
           return ISyJobDispatcher.Instance.GetJobDispatcher(userSysNo, taskType);
        }

        /// <summary>
        /// 更新自动分配状态
        /// </summary>
        /// <param name="model">自动分配任务实体</param>
        /// <returns>返回系统编号</returns>
        /// <remarks>2013-06-21 余勇 创建</remarks>
        public  int UpdateStatus(SyJobDispatcher model)
        {
            return ISyJobDispatcher.Instance.UpdateStatus(model);
        }

        /// <summary>
        /// 更新自动分配状态及数量
        /// </summary>
        /// <param name="model">自动分配任务实体</param>
        /// <returns>返回系统编号</returns>
        /// <remarks>2013-06-21 余勇 创建</remarks>
        public  int UpdateStatusQuantity(SyJobDispatcher model)
        {
            return ISyJobDispatcher.Instance.UpdateStatusQuantity(model);
        }

        /// <summary>
        /// 通过SysNo删除该记录
        /// </summary>
        /// <param name="sysNo">自动分配编号</param>
        /// <returns>影响行数</returns>
        /// <remarks>2013-06-19 余勇 创建</remarks>
        public int DeleteBySysNo(int sysNo)
        {
            return ISyJobDispatcher.Instance.DeleteBySysNo(sysNo);
        }

        /// <summary>
        /// 获取已分配过任务的用户列表
        /// </summary>
        /// <returns>已分配过任务的用户列表</returns>
        /// <remarks>2013-10-08 朱家宏 创建</remarks>
        public IList<SyUser> GetDispatchedUsers()
        {
            return ISyJobDispatcher.Instance.SelectDistinctUsers();
        }

        /// <summary>
        ///  通过用户编号删除
        /// </summary>
        /// <param name="userSysNo">用户编号</param>
        /// <returns>t:成功，f:失败</returns>
        /// <remarks>2013-10-08 朱家宏 创建</remarks>
        public bool RemoveByUserSysNo(int userSysNo)
        {
            return ISyJobDispatcher.Instance.DeleteByUserSysNo(userSysNo) > 0;
        }

        /// <summary>
        /// 获取所有
        /// </summary>
        /// <returns>list</returns>
        /// <remarks>2013-10-08 朱家宏 创建</remarks>
        public IList<SyJobDispatcher> GetAll()
        {
            return ISyJobDispatcher.Instance.SelectAll();
        }

        /// <summary>
        /// 写入任务日志
        /// </summary>
        /// <param name="message">日志内容</param>
        /// <param name="targetSysno">系统编号</param>
        /// <param name="userIp">ip</param>
        /// <param name="operatorSysno">操作人</param>
        public void WriteJobLog(string message, int targetSysno, string userIp, int operatorSysno)
        {
            SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Info, LogStatus.系统日志来源.后台, message, LogStatus.系统日志目标类型.任务池, targetSysno, null, userIp, operatorSysno);
        }
    }
}
