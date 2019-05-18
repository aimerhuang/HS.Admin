using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Hyt.BLL.Front;
using Hyt.BLL.Log;
using Hyt.BLL.Logistics;
using Hyt.BLL.Sys;
using Hyt.Infrastructure.Pager;
using Hyt.Model;
using Hyt.Model.SystemPredefined;
using Hyt.Model.WorkflowStatus;
using System.Web;
using Hyt.BLL.Authentication;
using Hyt.BLL.LevelPoint;
using Hyt.Infrastructure.Caching;
using Hyt.Util;
using Hyt.Admin;
using System.Web.Script.Serialization;
using Hyt.Model.Parameter;

namespace Hyt.Admin.Controllers
{
    /// <summary>
    /// 主页控制器
    /// </summary>
    /// <remarks>2014-1-8 黄波 创建</remarks>
    public class MainController : BaseController
    {
        /// <summary>
        /// 主框架
        /// </summary>
        /// <returns>主框架页面</returns>
        /// <remarks> 黄波 2013/06/05 添加</remarks>
        [CustomActionFilter(true)]
        public ActionResult Index()
        {
           // var result = Hyt.BLL.Order.SoOrderBo.Instance.ImportDsMallOrderNew();
            //主页框架关闭MiniProfiler性能分析
            ViewBag.StartHiddenMiniProfiler = "Close";
            return View(CurrentUser);
        }

        /// <summary>
        /// 添加我的菜单
        /// </summary>
        /// <param name="menuSysNo">菜单系统编号</param>
        /// <returns>返回json数据</returns>
        /// <remarks>2013-10-09  周瑜 创建</remarks>
        [Privilege(PrivilegeCode.CM1005155)]
        public JsonResult MyMenuInsert(int menuSysNo)
        {
            var result = new Result { StatusCode = -1 };
            try
            {
                var menu = new SyMyMenu
                {
                    MenuSysNo = menuSysNo,
                    UserSysNo = CurrentUser.Base.SysNo,
                    CreatedBy = CurrentUser.Base.SysNo,
                    CreatedDate = DateTime.Now
                };
                var menuCheck = SyMyMenuBo.Instance.GetMoudle(CurrentUser.Base.SysNo, menuSysNo);
                if (menuCheck == null)
                {
                    SyMyMenuBo.Instance.Insert(menu);
                    result.StatusCode = 0;
                    result.Message = "添加到我的菜单成功.";
                }
                else
                {
                    result.StatusCode = 1;
                    result.Message = "该菜单已经添加过了.";
                }
            }
            catch (Exception ex)
            {
                result.StatusCode = -1;
                result.Message = ex.Message;
            }

            return Json(result);
        }

        /// <summary>
        /// 删除我的菜单
        /// </summary>
        /// <param name="menuSysNo">菜单系统编号</param>
        /// <returns>返回json数据</returns>
        /// <remarks>2013-10-09  周瑜 创建</remarks>
        [Privilege(PrivilegeCode.CM1005155)]
        public JsonResult MyMenuDelete(int menuSysNo)
        {
            var result = new Result { StatusCode = -1 };
            try
            {
                SyMyMenuBo.Instance.Delete(CurrentUser.Base.SysNo, menuSysNo);
                result.StatusCode = 0;
                result.Message = "我的菜单删除成功.";
            }
            catch (Exception ex)
            {
                result.StatusCode = -1;
                result.Message = ex.Message;
            }

            return Json(result);
        }

        /// <summary>
        /// 后台首页
        /// </summary>
        /// <returns>后台首页</returns>
        /// <remarks>2013-09-16 苟治国 创建</remarks>
        public ActionResult Default()
        {
            //统计信息
            var filter = new ParaIsDealerFilter { };
            //当前用户对应分销商，2015-12-19 王耀发 创建
            if (CurrentUser.IsBindDealer)
            {
                filter.DealerSysNo = CurrentUser.Dealer.SysNo;
                filter.IsBindDealer = CurrentUser.IsBindDealer;
            }
            //是否绑定所有经销商
            filter.IsBindAllDealer = CurrentUser.IsBindAllDealer;
            filter.DealerCreatedBy = CurrentUser.Base.SysNo;

            ViewBag.TotalInfo = BLL.Order.SoOrderBo.Instance.GetOrderTotalInfo(filter);
            return View();
        }

        #region 订单池
        /// <summary>
        /// 查询订单池
        /// </summary>
        /// <param name="model">传入的实体参数</param>
        /// <returns>返回查询列表</returns>
        /// <remarks>2013-09-26 苟治国 创建</remarks>
        [Privilege(PrivilegeCode.CM10046602)]
        public ActionResult JobPooSearch(CBSyJobPool model)
        {
            var pager = new Pager<CBSyJobPool>
            {
                PageFilter =
                {
                    Status = (int)SystemStatus.任务池状态.待处理,
                    TaskType = 0,
                    ExecutorSysNo = CurrentUser.Base.SysNo
                },
                PageSize = 10,
                CurrentPage = model.id
            };

            //当前用户对应分销商，2015-12-19 王耀发 创建
            if (CurrentUser.IsBindDealer)
            {
                pager.PageFilter.DealerSysNo = CurrentUser.Dealer.SysNo;
                pager.PageFilter.IsBindDealer = CurrentUser.IsBindDealer;
            }
            //是否绑定所有经销商
            pager.PageFilter.IsBindAllDealer = CurrentUser.IsBindAllDealer;
            pager.PageFilter.DealerCreatedBy = CurrentUser.Base.SysNo;

            Hyt.BLL.Sys.SyJobPoolManageBo.Instance.GetMessageList(pager);

            PagedList<CBSyJobPool> list = new PagedList<CBSyJobPool>();
            list.TData = pager.Rows;
            list.CurrentPageIndex = pager.CurrentPage;
            list.TotalItemCount = pager.TotalRows;
            list.PageSize = pager.PageSize;

            //获取待回复的咨询数量
            ViewBag.QueCounts = CrCustomerQuestionBo.Instance.GetCusQuestionCounts((int)CustomerStatus.会员咨询状态.待回复);
            return PartialView("_SyMessages", list);
        }

        /// <summary>
        /// 定时请求订单池
        /// </summary>
        /// <param name="model">传入的实体参数</param>
        /// <returns>返回查询列表</returns>
        /// <remarks>2013-09-26 苟治国 创建</remarks>
        [Privilege(PrivilegeCode.CM10046602)]
        public JsonResult JobPool(CBSyJobPool model)
        {
            var pager = new Pager<CBSyJobPool>
            {
                PageFilter =
                {
                    //Status = model.Status == 0 ? 0 : model.Status,//默认2，,未处理
                    Status = (int)SystemStatus.任务池状态.待处理,
                    TaskType = 0,
                    ExecutorSysNo = CurrentUser.Base.SysNo
                },
                PageSize = 99,
                CurrentPage = model.id
            };
            //当前用户对应分销商，2015-12-19 王耀发 创建
            if (CurrentUser.IsBindDealer)
            {
                pager.PageFilter.DealerSysNo = CurrentUser.Dealer.SysNo;
                pager.PageFilter.IsBindDealer = CurrentUser.IsBindDealer;
            }
            //是否绑定所有经销商
            pager.PageFilter.IsBindAllDealer = CurrentUser.IsBindAllDealer;
            pager.PageFilter.DealerCreatedBy = CurrentUser.Base.SysNo;

            Hyt.BLL.Sys.SyJobPoolManageBo.Instance.GetMessageList(pager);

            var list = new PagedList<CBSyJobPool>
            {
                TData = pager.Rows,
                CurrentPageIndex = pager.CurrentPage,
                TotalItemCount = pager.TotalRows,
                PageSize = pager.PageSize
            };

            return Json(from item in list.TData select new { JobDescription = item.JobDescription, JobUrl = item.JobUrl, TaskType = Hyt.Util.EnumUtil.GetDescription(typeof(Hyt.Model.WorkflowStatus.SystemStatus.任务对象类型), item.TaskType), Status = Hyt.Util.EnumUtil.GetDescription(typeof(Hyt.Model.WorkflowStatus.SystemStatus.任务池状态), item.Status) });
        }
        #endregion

        #region 公司公告
        /// <summary>
        /// 获取公司公告
        /// </summary>
        /// <param name="id">索引</param>
        /// <returns>返回查询列表</returns>
        /// <remarks>2013-09-26 苟治国 创建</remarks>
        [Privilege(PrivilegeCode.CM10046601)]
        public ActionResult CompanyPostSeach(int id)
        {
            var ids = new List<int>();
            ids.Add(3);//Fearticlecategory系统编号

            var filter = new ParaArticleFilter { pageIndex = id, ids = ids, searchStaus = (int)ForeStatus.文章状态.已审, searchName = "", SelectedDealerSysNo = -1 };
            //当前用户对应分销商，2015-12-19 王耀发 创建
            if (CurrentUser.IsBindDealer)
            {
                int DealerSysNo = CurrentUser.Dealer.SysNo;
                filter.DealerSysNo = DealerSysNo;
                filter.IsBindDealer = CurrentUser.IsBindDealer;
            }
            //是否绑定所有经销商
            filter.IsBindAllDealer = CurrentUser.IsBindAllDealer;
            filter.DealerCreatedBy = CurrentUser.Base.SysNo;
            var model = FeArticleBo.Instance.Seach(filter);

            return PartialView("_CompanyPost", model);
        }
        #endregion

        /// <summary>
        /// 查看公司公告详细
        /// </summary>
        /// <param name="id">公告系统编号</param>
        /// <returns>公司公告详细视图</returns>
        /// <remarks>2013-09-26 苟治国 修改</remarks>
        [Privilege(PrivilegeCode.CM10046601)]
        public ActionResult Announcement(int? id)
        {
            var model = FeArticleBo.Instance.GetPassArticle(3);
            ViewBag.ShowSysNo = id ?? 0;
            return View(model);
        }

        /// <summary>
        /// 查看系统消息 请求订单池
        /// </summary>
        /// <param name="model">订单查询池</param>
        /// <returns>系统消息列表界面</returns>
        /// <remarks> 
        /// 2013-10-23 郑荣华 创建
        /// </remarks>
        [Privilege(PrivilegeCode.CM10046602)]
        public ActionResult SyMessagesList(CBSyJobPool model)
        {
            if (Request.IsAjaxRequest())
            {
                var pager = new Pager<CBSyJobPool>
                {
                    PageFilter =
                    {
                        Status = model.Status == 0 ? 0 : model.Status,//默认2，,未处理
                        TaskType = 0,
                        ExecutorSysNo = CurrentUser.Base.SysNo
                    },
                    CurrentPage = model.id
                };
                //当前用户对应分销商，2015-12-19 王耀发 创建
                if (CurrentUser.IsBindDealer)
                {
                    pager.PageFilter.DealerSysNo = CurrentUser.Dealer.SysNo;
                    pager.PageFilter.IsBindDealer = CurrentUser.IsBindDealer;
                }
                //是否绑定所有经销商
                pager.PageFilter.IsBindAllDealer = CurrentUser.IsBindAllDealer;
                pager.PageFilter.DealerCreatedBy = CurrentUser.Base.SysNo;

                SyJobPoolManageBo.Instance.GetMessageList(pager);
                //SyJobPoolManageBo.Instance.GetJobSpoolList(pager);

                var list = new PagedList<CBSyJobPool>
                {
                    TData = pager.Rows,
                    CurrentPageIndex = pager.CurrentPage,
                    TotalItemCount = pager.TotalRows,
                    PageSize = pager.PageSize
                };
                return PartialView("_AjaxPagerSyMessages", list);
            }

            var item = new SelectListItem() { Text = @"全部", Value = "", Selected = true };
            var statustList = new List<SelectListItem>() { item };
            Util.EnumUtil.ToListItem<SystemStatus.任务池状态>(ref statustList);
            ViewBag.Status = new SelectList(statustList, "Value", "Text");

            return View();
        }

        #region 测试页面
        [CustomActionFilter(false)]
        public ActionResult test()
        {
            System.Threading.Tasks.Task.Factory.StartNew(() =>
            {
                var file = "";
                var configFileName = "taobao.config";


                if (System.Web.HttpContext.Current != null)
                {
                    file = System.Web.HttpContext.Current.Server.MapPath("~/Config/") + configFileName + "h";
                }
                else
                {
                    file = AppDomain.CurrentDomain.BaseDirectory + "Config\\" + configFileName + "a";
                }


                SysLog.Instance.Info(LogStatus.系统日志来源.后台, file, LogStatus.系统日志目标类型.EAS, 0, 0);
            });
            return View();
        }

        public ActionResult ValidateTest()
        {
            return View();
        }
        #endregion
    }
}
