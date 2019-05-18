using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Hyt.Model.Parameter;
using Hyt.Model.SystemPredefined;
using Hyt.Service.Task.Core;

namespace Hyt.Admin.Controllers
{
    /// <summary>
    /// 任务计划管理
    /// </summary>
    /// <remarks>2013-10-9 杨浩 添加</remarks>
    public class TaskController : BaseController
    {
        /// <summary>
        /// 任务计划
        /// </summary>
        /// <returns>任务列表页</returns>
        /// <remarks>2013-10-9 杨浩 添加</remarks>
        public ActionResult Index()
        {
            var task = Hyt.BLL.Sys.SyTaskBo.Instance.GetAll();
            return View(task);
        }

        /// <summary>
        /// 编辑/新增 页面显示
        /// </summary>
        /// <param name="sysNo">任务系统编号</param>
        /// <returns>编辑/新增 页面</returns>
        /// <remarks>2013-10-16 杨浩 添加</remarks>
        [Privilege(PrivilegeCode.SY1011201)]
        public ActionResult TaskInfo(int? sysNo)
        {
            var model = new ParaSyTask();
            model.Config = new Model.SyTaskConfig();
            //获取实现了ITask的任务类型名称
            var typeList = TaskManager.Instance.GetType()
                                      .Assembly.GetExportedTypes()
                                      .Where(x => x.GetInterface("ITask") != null)
                                      .ToList();
            var types = new Dictionary<Type, string>();
            foreach (var t in typeList)
            {
                var p = t.GetCustomAttributes(typeof (DescriptionAttribute), false);
                var s = p.Any() ? ((DescriptionAttribute) (p[0])).Description : "无描述";
                types.Add(t, s);
            }
            ViewBag.Types = types;
            if (sysNo != null)
            {
                var task = Hyt.BLL.Sys.SyTaskBo.Instance.GetTask(sysNo ?? 0);
                if (task.CreateTime == default(DateTime))
                    task.CreateTime = DateTime.Now;
                model.Config = task;
                switch (task.Timetype)
                {
                    case (int)TaskTimeType.Interval:
                        if (!string.IsNullOrEmpty(task.TimeQuantum))
                        {
                            var timeQuantum = task.TimeQuantum.Split(',');
                            model.StartTimeQuantum = new TimeSpan(long.Parse(timeQuantum[0]));
                            model.EndTimeQuantum = new TimeSpan(long.Parse(timeQuantum[1]));
                        }
                        model.IntervalTime = Int64.Parse(task.ExecuteTime) /(10000000);
                        break;
                    case (int)TaskTimeType.Day:
                        model.DayExecuteTime = new TimeSpan(long.Parse(task.ExecuteTime));
                        break;
                    case (int)TaskTimeType.Week:
                        model.DayOfWeekExecuteTime = new TimeSpan(long.Parse(task.ExecuteTime));
                        break;
                    case (int)TaskTimeType.Month:
                        var time = new TimeSpan(long.Parse(task.ExecuteTime));
                        model.MonthHmsExecuteTime = new TimeSpan(time.Hours, time.Minutes, time.Seconds);
                        model.MonthOfDayExecuteTime = time.Days;
                        break;
                }
            }
            else
                model.EndTimeQuantum = new TimeSpan(23, 59, 59);
            return View(model);
        }

        /// <summary>
        /// 添加/更新
        /// </summary>
        /// <param name="model">任务数据</param>
        /// <returns>Result</returns>
        /// <remarks>2013-10-16 杨浩 添加</remarks>
        [HttpPost]
        [Privilege(PrivilegeCode.SY1011201)]
        public JsonResult AddTaskInfo(ParaSyTask model)
        {
           
            var result = new Model.Result
            {
                Message="任务添加成功", 
                Status = true
            };
            try
            {
                //时间转换为刻度数
                switch (model.Config.Timetype)
                {
                    case (int)TaskTimeType.Interval:
                        //是否设置了间隔任务的时间段
                        if (model.StartTimeQuantum != default(TimeSpan) && model.EndTimeQuantum != default(TimeSpan))
                        {
                            model.Config.TimeQuantum = model.StartTimeQuantum.Ticks + "," +
                                                       model.EndTimeQuantum.Ticks;
                        }
                        model.Config.ExecuteTime = (10000000 * model.IntervalTime).ToString();
                        break;
                    case (int)TaskTimeType.Day:
                        model.Config.ExecuteTime = model.DayExecuteTime.Ticks.ToString();
                        break;
                    case (int)TaskTimeType.Week:
                        model.Config.ExecuteTime = model.DayOfWeekExecuteTime.Ticks.ToString();
                        break;
                    case (int)TaskTimeType.Month:
                        model.Config.ExecuteTime =
                            model.MonthHmsExecuteTime.Add(new TimeSpan(model.MonthOfDayExecuteTime, 0, 0, 0))
                                 .Ticks.ToString();
                        break;
                }

                model.Config.LastExecuteTime = model.Config.LastExecuteTime == System.DateTime.MinValue ? (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue : model.Config.LastExecuteTime;
                //添加
                if (model.Config.SysNo == 0)
                {
                    model.Config.CreateTime = DateTime.Now;
                    model.Config.Status = 1;
                    model.Config.StartTime = model.Config.StartTime == System.DateTime.MinValue ? (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue : model.Config.StartTime;
                    model.Config.EndTime = model.Config.EndTime == System.DateTime.MinValue ? (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue : model.Config.EndTime;
                    Hyt.BLL.Sys.SyTaskBo.Instance.Add(model.Config);
                }
                //更新
                else
                {
                    result.Message = "任务更新成功";
                    Hyt.BLL.Sys.SyTaskBo.Instance.UpdateTask(model.Config);
                }
                TaskManager.Instance.Reload();
            }
            catch(Exception e)
            {
                result.Message = e.Message;
                result.Status = false;
                //TODO:记录错误
            }
            return Json(result);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="sysNo">任务计划编号</param>
        /// <param name="status">状态</param>
        /// <returns>Result</returns>
        /// <remarks>2013-10-16 杨浩 添加</remarks>
        [HttpPost]
        [Privilege(PrivilegeCode.SY1011701)]
        public JsonResult UpdateStatus(int sysNo, int status)
        {
            var task = Hyt.BLL.Sys.SyTaskBo.Instance.GetTask(sysNo);
            task.Status = status;
            Hyt.BLL.Sys.SyTaskBo.Instance.UpdateTask(task);
            TaskManager.Instance.Reload();
            return Json(new Model.Result {Status = true});
        }

        /// <summary>
        /// 查看任务执行日志
        /// </summary>
        /// <param name="sysNo">任务计划编号</param>
        /// <param name="id">currentPage</param>
        /// <param name="pageSize"></param>
        /// <returns>日志页面</returns>
        /// <remarks>2013-10-16 杨浩 添加</remarks>
        [Privilege(PrivilegeCode.SY1011201)]
        public ActionResult TaskLog(int sysNo, int? id, int? pageSize)
        {
            var model = Hyt.BLL.Sys.SyTaskBo.Instance.GetTaskLogs(sysNo, id ?? 1, pageSize ?? 10);

            if (Request.IsAjaxRequest())
            {
                return PartialView("_TaskLogPager", model);
            }

            return View(model);
        }

        /// <summary>
        /// 获取触发器提示信息
        /// </summary>
        /// <param name="task">任务对象</param>
        /// <returns>触发器提示信息</returns>
        /// <remarks>2013-10-16 杨浩 添加</remarks>
        public static string GetTips(Model.SyTaskConfig task)
        {
            var timeType = (TaskTimeType) task.Timetype;
            string tips = string.Empty;
            switch (timeType)
            {
                case TaskTimeType.Interval:
                    tips = string.Format("间隔执行:每间隔 {0}秒 执行一次", Int64.Parse(task.ExecuteTime) / (10000000));
                    break;
                case TaskTimeType.Day:
                    tips = string.Format("按天执行:在每天的 {0} 时间点执行", new TimeSpan(long.Parse(task.ExecuteTime)));
                    break;
                case TaskTimeType.Week:
                    tips = string.Format("按周执行:在每周的第 {0}天的{1} 时间点执行", task.DayOfWeek, new TimeSpan(long.Parse(task.ExecuteTime)));
                    break;
                case TaskTimeType.Month:
                    var time = new TimeSpan(long.Parse(task.ExecuteTime));
                    var monthHmsExecuteTime = new TimeSpan(time.Hours, time.Minutes, time.Seconds);
                    var monthOfDayExecuteTime = time.Days;
                    tips = string.Format("按月执行:在{0}月的第{1}天的 {2} 时间点执行", task.Month, monthOfDayExecuteTime, monthHmsExecuteTime);
                    break;
            }
            return tips;
        }
    }
}
