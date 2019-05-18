using System.Transactions;
using Hyt.BLL.Authentication;
using Hyt.BLL.Log;
using Hyt.BLL.MallSeller;
using Hyt.BLL.Order;
using Hyt.BLL.Sys;
using Hyt.Infrastructure.Pager;
using Hyt.Model;
using Hyt.Model.Generated;
using Hyt.Model.WorkflowStatus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Hyt.Model.SystemPredefined;
using Hyt.Util;

namespace Hyt.Admin.Controllers
{
    /// <summary>
    /// 工作池
    /// </summary>
    /// <remarks>
    /// 2013-06-13 余勇 创建
    /// 2013-06-13 罗雄伟 重构文件名、类名
    /// </remarks>
    public class JobPoolController : BaseController
    {
        #region 任务池业务
        /// <summary>
        /// 任务池列表查询
        /// </summary>
        /// <returns>任务池列表</returns>
        /// <remarks>2013-06-13 余勇 创建</remarks>
        [Privilege(PrivilegeCode.SY1010101)]
        public ActionResult Index()
        {
            if (CurrentUser.PrivilegeList.HasPrivilege(PrivilegeCode.SY1010801))
            {
                ViewBag.Admin = true;
            }
            else
            {
                ViewBag.Admin = false;
            }
            return View();
        }

        /// <summary>
        /// 我的任务池查询
        /// </summary>
        /// <returns>我的任务池查询页面</returns>
        /// <remarks>2013-10-16 余勇 创建</remarks>
        [Privilege(PrivilegeCode.SY1018101)]
        public ActionResult MyJob()
        {
            return View();
        }

        /// <summary>
        /// 我的任务池查询
        /// </summary>
        /// <param name="taskType">任务类型</param>
        /// <returns>我的任务池查询页面</returns>
        /// <remarks>2013-10-16 余勇 创建</remarks>
        [Privilege(PrivilegeCode.SY1018101)]
        public ActionResult LoadMyJobs(int taskType)
        {
            ViewBag.TaskType = taskType;
            return View("_MyJob");
        }

        /// <summary>
        /// 任务池分配
        /// </summary>
        /// <param name="sysNos">任务池编号字符串，多个任务池编号用逗号(,)分隔</param>
        /// <param name="userSysNo">指定客服编号</param>
        /// <returns>Result对象</returns>
        /// <remarks>2013-06-14 余勇 创建</remarks>
        [HttpPost]
        [Privilege(PrivilegeCode.SY1010201)]
        public ActionResult AssignJobs(string sysNos, int userSysNo)
        {
            var result = new Result();
            if (!string.IsNullOrEmpty(sysNos) && userSysNo > 0)
            {
                Hyt.BLL.Sys.SyJobPoolManageBo.Instance.AssignJobs(sysNos, userSysNo, CurrentUser.Base.SysNo);
            }
            else
            {
                result.StatusCode = -1;
                result.Message = "尚未选择任务分配对象和分配人员！";
            }
            return Json(result);
        }

        /// <summary>
        /// 设置优先级
        /// </summary>
        /// <param name="sysNo">任务编号</param>
        /// <param name="priority">优先级</param>
        /// <returns>Result对象</returns>
        /// <remarks>2014-01-13 余勇 创建</remarks>
        [HttpPost]
        [Privilege(PrivilegeCode.SY1010201, PrivilegeCode.SY1018201)]
        public ActionResult ChangePriority(int sysNo, int priority)
        {
            var result = new Result<string>();
            if (sysNo > 0 && priority >= 0)
            {
                Hyt.BLL.Sys.SyJobPoolManageBo.Instance.UpdatePriority(sysNo, priority);
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Info, LogStatus.系统日志来源.后台, "设置优先级，任务号:" + sysNo,
                                   LogStatus.系统日志目标类型.任务池, 0, null, null, CurrentUser.Base.SysNo);

                result.Data = "blue";
                result.Status = true;
            }
            else
            {
                result.Message = "任务编号或优先级不能为空！";
            }
            return Json(result);
        }

        /// <summary>
        /// 加载客服人员
        /// </summary>
        /// <param name="taskType">任务类型编号</param>
        /// <returns>客服人员选择页</returns>
        /// <remarks>2013-06-19 余勇 创建</remarks> 
        [HttpPost]
        [Privilege(PrivilegeCode.SY1010201)]
        public ActionResult LoadJobPoolUsers(int taskType)
        {
            ViewBag.SysUsers = Hyt.BLL.Sys.SyJobPoolManageBo.Instance.GetJobUserByUserGroup(taskType, UserGroup.客服组);
            return PartialView("_AssignUser");
        }

        /// <summary>
        /// 获取已分配客服人员键值对
        /// </summary>
        /// <param name="taskType">任务类型编号</param>
        /// <returns>客服人员键值对</returns>
        /// <remarks>2013-07-03 余勇 创建</remarks>
        [Privilege(PrivilegeCode.SY1010101)]
        public JsonResult LoadAssignedUser(int taskType)
        {
            return Json(Hyt.BLL.Sys.SyJobPoolManageBo.Instance.GetAssignedUsers(taskType), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 任务池申领
        /// </summary>
        /// <param name="sysNos">任务池编号字符串，多个任务池编号用逗号(,)分隔</param>
        /// <param name="taskType">任务对象类型</param>
        /// <returns>Result对象</returns>
        /// <remarks>2013-06-14 余勇 创建</remarks>
        [HttpPost]
        [Privilege(PrivilegeCode.SY1010201)]
        public ActionResult ApplyJobs(string sysNos, int taskType)
        {
            var result = new Result();
            if (!string.IsNullOrEmpty(sysNos) && taskType > 0)
            {
                int userSysNo = CurrentUser.Base.SysNo;
                var jobNum = SyJobPoolManageBo.Instance.GetJobsNumByUser(userSysNo, taskType);
                //获取自动任务配置
                SyJobDispatcher model = Hyt.BLL.Sys.SyJobDispatcherBo.Instance.GetJobDispatcher(CurrentUser.Base.SysNo, taskType) ?? new SyJobDispatcher();
                var maxTaskQuantity = model.MaxTaskQuantity > 0 ? model.MaxTaskQuantity : 10;
                if (jobNum >= maxTaskQuantity)
                {
                    result.StatusCode = -2;
                    result.Message = "最多申领" + maxTaskQuantity + "个任务，您的任务数已满，请处理后再来申领！";
                }
                else
                {
                    string[] ids = sysNos.Split(',');
                    var jobs = maxTaskQuantity - jobNum;
                    if (jobs > 0)
                    {
                        sysNos = ids.Take(jobs).ToArray().Join(",");
                    }
                    try
                    {
                        Hyt.BLL.Sys.SyJobPoolManageBo.Instance.AssignJobs(sysNos, userSysNo, userSysNo);
                        result.Message = "您成功申领" + sysNos.Split(',').Length + "个任务！";
                    }
                    catch (Exception ex)
                    {
                        SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "任务池申领错误:" + sysNos,
                                  LogStatus.系统日志目标类型.任务池, 0, null, null, userSysNo);
                    }
                }

            }
            else
            {
                result.StatusCode = -1;
                result.Message = "尚未选择任务对象！";
            }
            return Json(result);
        }

        /// <summary>
        /// 查询任务池
        /// </summary>
        /// <param name="model">传入的实体参数</param>
        /// <returns>返回查询列表</returns>
        /// <remarks>2013-06-18 余勇 创建</remarks>
        [Privilege(PrivilegeCode.SY1010101)]
        public ActionResult Search(CBSyJobPool model)
        {
            var pager = new Pager<CBSyJobPool>
                {
                    PageFilter =
                        {
                            Status = model.Status,
                            ExecutorSysNo = model.ExecutorSysNo,
                            TaskSysNo = model.TaskSysNo,
                            TaskType = model.TaskType,
                            Sort = model.Sort,
                            SelectedAgentSysNo = model.AgentSysNo,
                            SelectedDealerSysNo = model.DealerSysNo
                        }
                };
            //当前用户对应仓库权限，2016-6-13 王耀发 创建
            pager.PageFilter.HasAllWarehouse = SyUserGroupBo.Instance.IsHasAllWarehouse(CurrentUser.Base.SysNo);
            pager.PageFilter.Warehouses = CurrentUser.Warehouses;

            //当前用户对应分销商，2016-2-16 王耀发 创建
            if (CurrentUser.IsBindDealer)
            {
                int DealerSysNo = CurrentUser.Dealer.SysNo;
                pager.PageFilter.DealerSysNo = DealerSysNo;
                pager.PageFilter.IsBindDealer = CurrentUser.IsBindDealer;
            }
            //是否绑定所有经销商
            pager.PageFilter.IsBindAllDealer = CurrentUser.IsBindAllDealer;
            pager.PageFilter.DealerCreatedBy = CurrentUser.Base.SysNo;

            pager.CurrentPage = model.id;
            Hyt.BLL.Sys.SyJobPoolManageBo.Instance.GetJobSpoolList(pager);

            var list = new PagedList<CBSyJobPool>();
            list.TData = pager.Rows;
            list.CurrentPageIndex = pager.CurrentPage;
            list.TotalItemCount = pager.TotalRows;
            list.PageSize = pager.PageSize;
            return PartialView("_AjaxPager", list);
        }

        /// <summary>
        /// 收回任务分配
        /// </summary>
        /// <param name="sysNos">任务号字符串，多个任务编号用逗号(,)分隔</param>
        /// <returns>Result对象</returns>
        /// <remarks>2013-06-14 余勇 创建</remarks>
        [HttpPost]
        [Privilege(PrivilegeCode.SY1010201, PrivilegeCode.SY1018201)]
        public ActionResult ReleaseJobs(string sysNos)
        {

            var result = new Result();
            if (!string.IsNullOrEmpty(sysNos))
            {
                //查看任务状态，如任务处于审核状态则任务不能收回
                string[] ids = sysNos.Split(',');
                string sysNo = string.Empty;
                foreach (string id in ids)
                {
                    try
                    {
                        int status = Hyt.BLL.Sys.SyJobPoolManageBo.Instance.GetJobStatus(int.Parse(id));
                        if (status != (int)SystemStatus.任务池状态.待处理)
                        {
                            sysNo = id;
                            break;
                        }
                    }
                    catch (Exception ex)
                    {
                        SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "收回任务分配GetJobStatus错误:" + ex.Message + "，任务编号:" + id,
                              LogStatus.系统日志目标类型.任务池, 0, null, null, CurrentUser.Base.SysNo);
                    }
                }
                //是否存在处理中的任务
                if (!string.IsNullOrEmpty(sysNo))
                {
                    try
                    {
                        result.StatusCode = -1;
                        SyJobPool job = Hyt.BLL.Sys.SyJobPoolManageBo.Instance.GetJob(int.Parse(sysNo));
                        var taskSysNo = job != null ? job.TaskSysNo : 0;
                        result.Message = "任务正在处理中不能回收，任务对象编号：" + taskSysNo;
                    }
                    catch (Exception ex)
                    {
                        SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "收回任务分配GetJob错误:" + ex.Message + "，任务编号:" + sysNo,
                              LogStatus.系统日志目标类型.任务池, 0, null, null, CurrentUser.Base.SysNo);
                    }
                }
                else
                {
                    try
                    {
                        bool succ = Hyt.BLL.Sys.SyJobPoolManageBo.Instance.ReleaseJob(sysNos, CurrentUser.Base.SysNo);

                    }
                    catch (Exception ex)
                    {
                        SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "收回任务分配错误:" + ex.Message + "，任务编号:" + sysNos,
                                LogStatus.系统日志目标类型.任务池, 0, null, null, CurrentUser.Base.SysNo);
                    }

                }
            }
            else
            {
                result.StatusCode = -1;
                result.Message = "尚未选择任务！";
            }
            return Json(result);
        }

        /// <summary>
        /// 解锁任务
        /// </summary>
        /// <param name="sysNos">任务号数组</param>
        /// <returns>Result对象</returns>
        /// <remarks>2013-06-14 余勇 创建</remarks>
        [HttpPost]
        [Privilege(PrivilegeCode.SY1010201)]
        public ActionResult UnLockJobs(int[] sysNos)
        {

            var result = new Result();
            if (sysNos != null && sysNos.Length > 0)
            {
                var sysNo = 0;
                foreach (var id in sysNos)
                {
                    int status = Hyt.BLL.Sys.SyJobPoolManageBo.Instance.GetJobStatus(id);
                    if (status != (int)SystemStatus.任务池状态.已锁定)
                    {
                        sysNo = id;
                        break;
                    }
                }
                //是否存在未锁定的任务
                if (sysNo > 0)
                {
                    var job = Hyt.BLL.Sys.SyJobPoolManageBo.Instance.GetJob(sysNo);
                    result.Message = string.Format("任务{0}未锁定，不能解锁", job.TaskSysNo);
                }
                else
                {
                    Hyt.BLL.Sys.SyJobPoolManageBo.Instance.UnLockJob(sysNos, CurrentUser.Base.SysNo);
                    result.Status = true;
                    result.Message = "解锁成功!";
                }
            }
            else
            {
                result.Message = "尚未选择任务！";
            }
            return Json(result);
        }

        /// <summary>
        /// 锁定任务
        /// </summary>
        /// <param name="sysNos">任务号数组</param>
        /// <param name="unLockState">解锁状态</param>
        /// <param name="unLockDate">解锁日期</param>
        /// <param name="remarks">备注</param>
        /// <returns>Result对象</returns>
        /// <remarks>2014-06-16 余勇 创建</remarks>
        [HttpPost]
        [Privilege(PrivilegeCode.SY1010201)]
        public ActionResult LockJobs(int[] sysNos, int unLockState, DateTime? unLockDate, string remarks)
        {
            var result = new Result();
            if (sysNos != null && sysNos.Length > 0)
            {
                int sysNo = 0;
                foreach (var id in sysNos)
                {
                    int status = Hyt.BLL.Sys.SyJobPoolManageBo.Instance.GetJobStatus(id);
                    if (status == (int)SystemStatus.任务池状态.已锁定)
                    {
                        sysNo = id;
                        break;
                    }
                }
                //是否存在已锁定的任务
                if (sysNo > 0)
                {
                    result.StatusCode = -1;
                    SyJobPool job = Hyt.BLL.Sys.SyJobPoolManageBo.Instance.GetJob(sysNo);
                    result.Message = string.Format("任务{0}不能重复锁定", job.TaskSysNo);
                }
                else
                {
                    Hyt.BLL.Sys.SyJobPoolManageBo.Instance.LockJob(sysNos, unLockState, unLockDate, remarks,
                        CurrentUser.Base.SysNo);
                    result.Status = true;
                    result.Message = "锁定成功!";
                }
            }
            else
            {
                result.Message = "尚未选择任务！";
            }
            return Json(result);
        }
        #endregion

        #region 任务分配

        /// <summary>
        /// 启用/暂停任务自动分配
        /// </summary>
        /// <param name="taskType">任务类型</param>
        /// <param name="status">状态值</param>
        /// <returns>Result对象</returns>
        /// <remarks>2013-06-21 余勇 创建</remarks>
        [HttpPost]
        [Privilege(PrivilegeCode.SY1010201, PrivilegeCode.SY1018201)]
        public ActionResult JobDispatcher(int taskType, int status)
        {
            var result = new Result();
            SyJobDispatcher model = Hyt.BLL.Sys.SyJobDispatcherBo.Instance.GetJobDispatcher(CurrentUser.Base.SysNo, taskType);
            if (model != null)
            {
                var stat = status == 1 ? 0 : 1; //状态为1表示开启，为0关闭
                model.Status = stat;
                Hyt.BLL.Sys.SyJobDispatcherBo.Instance.UpdateStatus(model);
                result.StatusCode = model.Status;
                #region 日志 朱成果 2013/12/20
                try
                {
                    string strDes = "(我的工作)" + Hyt.Util.EnumUtil.GetDescription(typeof(Hyt.Model.WorkflowStatus.SystemStatus.任务对象类型), taskType);
                    if (model.Status == 1)
                    {
                        strDes += ":开启";
                    }
                    else
                    {
                        strDes += ":关闭";
                    }
                    Hyt.BLL.Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台, strDes, LogStatus.系统日志目标类型.任务池, CurrentUser.Base.SysNo,
                                            CurrentUser.Base.SysNo);
                }
                catch { }
                #endregion
            }
            else
            {
                InsertJobDispatcher(taskType);
                result.StatusCode = 1;
            }

            return Json(result);
        }

        #endregion

        #region 设置任务执行人
        /// <summary>
        /// 任务执行人设置
        /// </summary>
        /// <returns>视图</returns>
        /// <remarks>2013-10-08 朱家宏 创建</remarks>
        [Privilege(PrivilegeCode.SY1010201)]
        public ActionResult JobDispatcherSelector()
        {
            return View();
        }

        /// <summary>
        /// 任务执行人查询
        /// </summary>
        /// <returns>视图</returns>
        /// <remarks>2013-10-10 朱家宏 创建</remarks>
        [Privilege(PrivilegeCode.SY1010201)]
        public ActionResult SearchJobDispatcher()
        {
            var users = BLL.Sys.SyJobDispatcherBo.Instance.GetDispatchedUsers().ToList();
            var dic = EnumUtil.ToDictionary(typeof(SystemStatus.任务对象类型));
            ViewBag.JobTypes = dic;
            ViewBag.Jobs = BLL.Sys.SyJobDispatcherBo.Instance.GetAll();

            return PartialView("_JobDispatcherSelector", users);
        }

        /// <summary>
        /// 获取单个任务设置
        /// </summary>
        /// <param name="userSysNo">用户编号</param>
        /// <param name="taskType">任务类型</param>
        /// <param name="jobs">任务列表</param>
        /// <returns>model</returns>
        /// <remarks>2013-10-09 朱家宏 创建</remarks>
        [Privilege(PrivilegeCode.SY1010201)]
        public static SyJobDispatcher GetTaskInfo(int userSysNo, int taskType, List<SyJobDispatcher> jobs)
        {
            var job = jobs.SingleOrDefault(o => o.UserSysNo == userSysNo && o.TaskType == taskType);
            return job;
        }


        /// <summary>
        /// 保存任务执行人设置
        /// </summary>
        /// <param name="userSysNo">用户编号</param>
        /// <param name="sets">设置</param>
        /// <param name="prioritys">优先级</param>
        /// <returns>json</returns>
        /// <remarks>2013-10-09 朱家宏 创建</remarks>
        [Privilege(PrivilegeCode.SY1010201)]
        public JsonResult SaveJobDispatcher(int userSysNo, List<SyJobDispatcher> sets, string prioritys)
        {

            var r = new Result { Status = false, StatusCode = 0, Message = "操作失败" };

            if (sets != null && sets.Any())
            {
                foreach (var item in sets)
                {
                    SyJobDispatcher model = Hyt.BLL.Sys.SyJobDispatcherBo.Instance.GetJobDispatcher(userSysNo, item.TaskType);

                    if (model == null)
                    {
                        model = new SyJobDispatcher
                        {
                            TaskType = item.TaskType,
                            Status = item.Status,    //界面上传入客服工作状态
                            UserSysNo = userSysNo,
                            MaxTaskQuantity = item.MaxTaskQuantity,
                            Prioritys = prioritys
                        };
                        Hyt.BLL.Sys.SyJobDispatcherBo.Instance.InsertJobDispatcher(model);
                    }
                    else
                    {
                        model.Status = item.Status;//界面上传入客服工作状态
                        model.MaxTaskQuantity = item.MaxTaskQuantity;
                        model.Prioritys = prioritys;
                        SyJobDispatcherBo.Instance.UpdateStatusQuantity(model);
                    }
                    r.Status = true;
                }
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Info, LogStatus.系统日志来源.后台, "修改客服工作量设置，被修改人：" + SyUserBo.Instance.GetUserName(userSysNo),
                                  LogStatus.系统日志目标类型.任务池, userSysNo, null, null, CurrentUser.Base.SysNo);
            }

            if (r.Status)
            {
                r.Message = "操作成功";
            }
            return Json(r, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 删除任务执行人设置
        /// </summary>
        /// <param name="userSysNo">用户编号</param>
        /// <returns>json</returns>
        /// <remarks>2013-10-09 朱家宏 创建</remarks>
        [Privilege(PrivilegeCode.SY1010201)]
        public JsonResult RemoveJobDispatcher(int userSysNo)
        {
            var r = new Result();
            if (BLL.Sys.SyJobDispatcherBo.Instance.RemoveByUserSysNo(userSysNo))
            {
                r.Status = true;
                r.Message = "删除成功。";
            }
            else
            {
                r.Status = false;
                r.Message = "删除失败。";
            }
            return Json(r, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 重置任务池
        /// </summary>
        /// <returns>操作是否成功</returns>
        /// <remarks>2014-02-28 余勇 创建</remarks>
        [Privilege(PrivilegeCode.SY1010201)]
        public JsonResult ResetJobDispatcher()
        {
            var result = new Result();
            try
            {
                AutoAssignJob.Instance.Reset();
                result.Status = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "重置任务池错误",
                                LogStatus.系统日志目标类型.任务池, 0, null, null, CurrentUser.Base.SysNo);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 以默认值插入自动分配记录
        /// </summary>
        /// <param name="taskType">任务类型</param>
        /// <returns>自动分配实体</returns>
        /// <remarks>2013-10-09 朱家宏 创建</remarks>
        private SyJobDispatcher InsertJobDispatcher(int taskType)
        {
            var model = new SyJobDispatcher
                {
                    TaskType = taskType,
                    Status = 1,
                    UserSysNo = CurrentUser.Base.SysNo,
                    MaxTaskQuantity = 0
                };
            Hyt.BLL.Sys.SyJobDispatcherBo.Instance.InsertJobDispatcher(model);
            return model;
        }

        #region 修改任务执行者
        /// <summary>
        /// 修改任务执行者
        /// </summary>
        /// <param name="taskType">任务对象类型</param>
        /// <param name="taskSysNo">任务对象编号</param>
        /// <returns>json</returns>
        /// <remarks>2013-06-21 余勇 创建</remarks>
        [Privilege(PrivilegeCode.SY1010801, PrivilegeCode.SY1010201, PrivilegeCode.SO1003601)]
        [HttpGet]
        public ActionResult UpdateExecutorSysNo(int taskType, int taskSysNo)
        {
            var i = 0;
            var job = SyJobPoolManageBo.Instance.GetByTask(taskSysNo, taskType);
            if (job != null)
            {
                i = BLL.Sys.SyJobPoolManageBo.Instance.UpdateExecutorSysNo(taskType, taskSysNo, CurrentUser.Base.SysNo);
                SyJobDispatcherBo.Instance.WriteJobLog(EnumUtil.GetDescription(typeof(SystemStatus.任务对象类型), taskType) + "被" + CurrentUser.Base.UserName + "强制执行,任务对象编号:" + taskSysNo + ",原执行人是：" + SyUserBo.Instance.GetUserName(job.ExecutorSysNo), taskSysNo, null, CurrentUser.Base.SysNo);
            }
            else
            {
                i = SyJobPoolManageBo.Instance.AssignJobByTaskType(taskType, taskSysNo, CurrentUser.Base.SysNo);
                SyJobDispatcherBo.Instance.WriteJobLog(EnumUtil.GetDescription(typeof(SystemStatus.任务对象类型), taskType) + "被" + CurrentUser.Base.UserName + "强制执行,任务对象编号:" + taskSysNo + ",原执行人为空", taskSysNo, null, CurrentUser.Base.SysNo);
            }
            var model = SoOrderBo.Instance.GetEntity(taskSysNo);
            if (model != null)
            {
                SoOrderBo.Instance.UpdateOnlineStatusByOrderID(model.SysNo, model.OnlineStatus);//目的是刷新时间戳
            }
            return Json(i, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #endregion

        #region 任务池优先级
        /// <summary>
        /// 任务池优先级列表查询
        /// </summary>
        /// <returns>任务池优先级列表</returns>
        /// <remarks>2014-02-28 余勇 创建</remarks>
        [Privilege(PrivilegeCode.SP1010101)]
        public ActionResult JobPriority()
        {
            return View();
        }

        /// <summary>
        /// 新增编辑任务池优先级
        /// </summary>
        /// <param name="id">任务池优先级编号</param>
        /// <returns>视图</returns>
        /// <remarks>2014-02-28 余勇 创建</remarks>
        [Privilege(PrivilegeCode.SP1010201)]
        public ActionResult JobPriorityCreate(int? id)
        {
            SyJobPoolPriority model = new SyJobPoolPriority();
            if (id.HasValue)
            {
                model = Hyt.BLL.Sys.SyJobPoolPriorityBo.Instance.Get(id.Value);
            }
            return View(model);
        }

        /// <summary>
        /// 任务池优先级查询
        /// </summary>
        /// <param name="id">任务池优先级编号</param>
        /// <returns>视图</returns>
        /// <remarks>2014-02-28 余勇 创建</remarks>
        [Privilege(PrivilegeCode.SP1010101)]
        public ActionResult SearchJobPriority()
        {
            var list = SyJobPoolPriorityBo.Instance.SelectAll();

            return PartialView("_JobPriority", list);
        }

        /// <summary>
        /// 保存任务池优先级
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns>操作是否成功</returns>
        /// <remarks>2014-02-28 余勇 创建</remarks>
        [Privilege(PrivilegeCode.SP1010201)]
        public JsonResult SaveJobPriority(SyJobPoolPriority model)
        {
            var result = new Result();
            try
            {
                if (model.SysNo > 0) //修改
                {
                    var entity = Hyt.BLL.Sys.SyJobPoolPriorityBo.Instance.Get(model.SysNo);

                    if (!string.IsNullOrEmpty(model.PriorityCode))
                    {
                        var priorityModel = Hyt.BLL.Sys.SyJobPoolPriorityBo.Instance.GetByPriorityCode(model.PriorityCode);
                        if (priorityModel != null)
                        {
                            if (priorityModel.SysNo != model.SysNo)
                            {
                                result.Message = "该优先级编码已存在，不能重复命名";
                                return Json(result, JsonRequestBehavior.AllowGet);
                            }
                        }
                    }

                    var priority = Hyt.BLL.Sys.SyJobPoolPriorityBo.Instance.GetByPriority(model.Priority);
                    if (priority != null && priority.SysNo != model.SysNo)
                    {
                        result.Message = "该优先级已存在，请重新设置";
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }

                    //当优先级编码改变时判断：如为系统内置编码则不允许修改
                    if (!string.IsNullOrEmpty(entity.PriorityCode) && !string.Equals(entity.PriorityCode, model.PriorityCode) && SyJobPoolPriorityBo.Instance.IsSysPriorityCode(entity.PriorityCode.Trim()))
                    {
                        result.Message = "该优先级编码为系统内置编码，不能修改";
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                    entity.Priority = model.Priority;
                    entity.PriorityDescription = model.PriorityDescription;
                    entity.PriorityCode = model.PriorityCode;
                    result = Hyt.BLL.Sys.SyJobPoolPriorityBo.Instance.Update(entity);
                    SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Info, LogStatus.系统日志来源.后台, "修改任务池优先级，系统编号:" + model.SysNo,
                                             LogStatus.系统日志目标类型.任务池, 0, null, null, CurrentUser.Base.SysNo);
                }
                else //新增
                {
                    if (!string.IsNullOrEmpty(model.PriorityCode))
                    {
                        var priorityModel = Hyt.BLL.Sys.SyJobPoolPriorityBo.Instance.GetByPriorityCode(model.PriorityCode);
                        if (priorityModel != null)
                        {
                            result.Message = "该优先级编码已存在，不能重复命名";
                            return Json(result, JsonRequestBehavior.AllowGet);
                        }
                    }
                    var priority = Hyt.BLL.Sys.SyJobPoolPriorityBo.Instance.GetByPriority(model.Priority);
                    if (priority != null)
                    {
                        result.Message = "该优先级已存在，请重新设置";
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                    result = Hyt.BLL.Sys.SyJobPoolPriorityBo.Instance.Insert(model);
                    SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Info, LogStatus.系统日志来源.后台, "新建任务池优先级",
                                             LogStatus.系统日志目标类型.任务池, 0, null, null, CurrentUser.Base.SysNo);

                }
                result.Status = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "保存任务池优先级错误:" + model.SysNo,
                                LogStatus.系统日志目标类型.任务池, 0, null, null, CurrentUser.Base.SysNo);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 删除任务池优先级
        /// </summary>
        /// <param name="id">系统编号</param>
        /// <returns>操作是否成功</returns>
        /// <remarks>2014-02-28 余勇 创建</remarks>
        [Privilege(PrivilegeCode.SP1010401)]
        public JsonResult DeleteJobPriority(int id)
        {
            var result = new Result();
            try
            {
                if (id > 0)
                {
                    var entity = Hyt.BLL.Sys.SyJobPoolPriorityBo.Instance.Get(id);
                    //当优先级编码改变时判断：如为系统内置编码则不允许修改
                    if (!string.IsNullOrEmpty(entity.PriorityCode) && SyJobPoolPriorityBo.Instance.IsSysPriorityCode(entity.PriorityCode.Trim()))
                    {
                        result.Message = "该优先级为系统内置，不能删除";
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }

                    result = Hyt.BLL.Sys.SyJobPoolPriorityBo.Instance.Delete(id);
                    SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Info, LogStatus.系统日志来源.后台,
                                             "删除任务池优先级，系统编号:" + id,
                                             LogStatus.系统日志目标类型.任务池, 0, null, null, CurrentUser.Base.SysNo);

                    result.Status = true;
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "删除任务池优先级错误:" + id,
                                LogStatus.系统日志目标类型.任务池, 0, null, null, CurrentUser.Base.SysNo);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 百城达未处理订单
        /// <summary>
        /// 百城达未处理订单
        /// </summary>
        /// <returns>百城达未处理订单页面</returns>
        /// <remarks>2014-08-05 余勇 创建</remarks>
        [Privilege(PrivilegeCode.SY1010901)]
        public ActionResult SyJobSmsConfig(CBSyJobPool model)
        {
            var hours = 3;
            var configModel = SyJobSmsConfigBo.Instance.GetFirst();
            if (configModel != null)
                hours = configModel.MaxDealTime;

            var list = SyJobPoolManageBo.Instance.GetDealingOverTimeSyJobs(hours);
            int pagecount = list.Count;
            if (pagecount > model.id * model.PageSize)
            {
                list = list.Skip(model.id * model.PageSize).Take(model.PageSize).ToList();
            }

            ViewBag.PageIndex = model.id;
            return View(list);
        }

        /// <summary>
        /// 查询百城达未处理订单
        /// </summary>
        /// <param name="model">传入的实体参数</param>
        /// <returns>返回查询列表</returns>
        /// <remarks>2014-08-05 余勇 创建</remarks>
        [Privilege(PrivilegeCode.SY1010901)]
        public ActionResult SearchSmsConfig(CBSyJobPool model)
        {
            model.id = model.id > 0 ? model.id : 1;
            model.PageSize = 10;
            var configModel = SyJobSmsConfigBo.Instance.GetFirst();
            var hours = configModel != null ? configModel.MaxDealTime : 3;
            var pager = Hyt.BLL.Sys.SyJobPoolManageBo.Instance.GetOverTimeSyJobsList(hours, model);
            var list = new PagedList<CBSyJobPool>();
            list.TData = pager.Rows;
            list.CurrentPageIndex = pager.CurrentPage;
            list.TotalItemCount = pager.TotalRows;
            list.PageSize = pager.PageSize;
            return PartialView("_SyJobSmsConfig", list);
        }

        /// <summary>
        /// 加载客服人员
        /// </summary>
        /// <param name="taskType">任务类型编号</param>
        /// <returns>客服人员</returns>
        /// <remarks>2014-08-05 余勇 创建</remarks>
        [HttpPost]
        [Privilege(PrivilegeCode.SY1010901)]
        public ActionResult GetJobUserByUserGroup(int taskType)
        {
            return Json(Hyt.BLL.Sys.SyJobPoolManageBo.Instance.GetJobUserByUserGroup(taskType, UserGroup.客服组), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 编辑任务池短信提醒设置
        /// </summary>
        /// <returns>执行结果</returns>
        /// <remarks>2014-08-05 余勇 创建</remarks>
        [Privilege(PrivilegeCode.SY1010901)]
        public ActionResult GetSmsConfig()
        {
            var result = new Result<SyJobSmsConfig>();
            try
            {
                var list = SyJobSmsConfigBo.Instance.GetAll();
                if (list != null && list.Count>0)
                {
                    result.Status = true;
                    result.Data = list.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "任务池短信提醒设置错误",
                                LogStatus.系统日志目标类型.任务池, 0, null, null, CurrentUser.Base.SysNo);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 任务池短信提醒设置保存
        /// </summary>
        /// <returns>执行结果</returns>
        /// <remarks>2014-08-05 余勇 创建</remarks>
        [Privilege(PrivilegeCode.SY1010901)]
        public ActionResult SaveSmsConfig(SyJobSmsConfig model)
        {
            var result = new Result();
            try
            {
                if (model.SysNo > 0)
                {
                    SyJobSmsConfigBo.Instance.Update(model);
                }
                else
                {
                    SyJobSmsConfigBo.Instance.Create(model);
                }
                result.Status = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "任务池短信提醒设置错误",
                                LogStatus.系统日志目标类型.任务池, 0, null, null, CurrentUser.Base.SysNo);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
