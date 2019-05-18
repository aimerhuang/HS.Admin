using System.Web.Mvc;
using Hyt.BLL.Log;
using Hyt.Model;
using System;
using Hyt.Model.WorkflowStatus;

namespace Hyt.Admin
{
    /// <summary>
    /// 所有Controller继承此基类
    /// </summary>
    /// <remarks>2013-6-24 杨浩 创建</remarks>
    [CustomActionFilter(true)]
    public class BaseController : Controller
    {
        #region 异常处理
        /// <summary>
        /// 异常处理
        /// </summary>
        /// <param name="filterContext">异常内容</param>
        /// <returns>异常提示信息</returns>
        /// <remarks>2013-11-10  黄波 重构</remarks>
        protected override void OnException(ExceptionContext filterContext)
        {
            //TODO:此处实现异常记录
            var exception = filterContext.Exception;
            var exceptionResult = new Result<Exception>
            {
                Message = exception.Message,
                Status = false,
                StatusCode = 0,
                Data = exception
            };

            #region 异常记录 yhy Add

            var fileName = @"D:\pisen\hyt\log\tpc";
            var values = filterContext.Controller.ControllerContext.RouteData.Values;
            var controller = values["controller"].ToString().ToLower();
            var action = values["action"].ToString().ToLower();
            Hyt.Util.Log.LogManager.Instance.WriteLog(fileName, @"日志类型:系统错误" +
                                                "\r\nActionName:" + action + ",ControllerName:" + controller +
                                                "\r\n错误消息：" + exception.Message +
                                                "\r\n错误堆栈：" + exception.StackTrace +
                                                (Request != null ? ("\r\n来源地址：" + (Request.UrlReferrer != null ? Request.UrlReferrer.AbsolutePath : "无") + "\r\n异常地址：" + Request.Url + "\r\n用户IP:" + Request.UserHostAddress + "\r\n登录用户:" + (CurrentUser != null ? CurrentUser.Base.Account : "未登录") + "\r\n用户代理:" + Request.UserAgent) : "")
                                                );

            SysLog.Instance.Error(LogStatus.系统日志来源.后台, exception.Message, LogStatus.系统日志目标类型.未捕获异常, 0, exception,
                                  (Request != null ? Request.UserHostAddress : string.Empty),
                                  (CurrentUser != null ? CurrentUser.Base.SysNo : 0));
            #endregion

            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                exceptionResult.Data = null;
                filterContext.Result = Json(exceptionResult, JsonRequestBehavior.AllowGet);
            }
            else
            {
                filterContext.Result = View("Error", exceptionResult);
            }
            filterContext.ExceptionHandled = true;
        }
        #endregion

        #region 属性

        /// <summary>
        /// 当前用户认证信息
        /// </summary>
        /// <returns>SysAuthorization</returns>
        /// <remarks> 2013-6-26 杨浩 创建</remarks>
        protected static SysAuthorization CurrentUser
        {
            get {
                if (BLL.Authentication.AdminAuthenticationBo.Instance.Current == null)
                    return new SysAuthorization() { Base = new SyUser() { SysNo = 12, UserName = "系统管理员" } };
                return BLL.Authentication.AdminAuthenticationBo.Instance.Current; 
            }
        }
        #endregion

        /// <summary>
        /// 将登录用户的用户名写入COOKIE
        /// </summary>
        /// <param name="userName">登录用户名</param>
        /// <return>void</return>
        /// <remarks>2013-10-31 黄波 创建</remarks>
        public void RecordLoginUserName(string userName)
        {
            Hyt.Util.CookieUtil.SetCookie(
                Hyt.Model.SystemPredefined.Constant.ADMIN_LOGINHISTORYUSERNAME_COOKIE
                , userName
                , DateTime.Now.AddDays(Hyt.Model.SystemPredefined.Constant.ADMIN_LOGINHISTORYUSERNAME_COOKIE_EXPIRY)
            );
        }
    }
}