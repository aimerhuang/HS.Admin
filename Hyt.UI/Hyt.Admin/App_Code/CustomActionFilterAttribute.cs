using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Hyt.Model;
using Hyt.Model.SystemPredefined;

namespace Hyt.Admin
{
    /// <summary>
    ///　自定义方法拦截器
    /// </summary>
    /// <remarks> 2013-6-27 杨浩 创建 </remarks>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class CustomActionFilterAttribute : ActionFilterAttribute
    {
        #region 私有常量
        /// <summary>
        /// 验证过期时执行的js脚本
        /// </summary>
        private string Script = @"UI.Dialog.close();DAO.LoginBox('{0}')";

        #endregion

        #region 公开字段
        /// <summary>
        /// 是否拦截
        /// </summary>
        public bool IsFilter { get; set; }
        #endregion

        #region 构造器
        /// <summary>
        /// CustomActionFilterAttribute
        /// </summary>
        /// <remarks> 2013-6-27 杨浩 创建 </remarks>
        public CustomActionFilterAttribute()
        {

        }

        /// <summary>
        /// 是否拦截
        /// </summary>
        /// <param name="isFilter">false:不拦截,true:拦截</param>
        /// <remarks> 2013-6-27 杨浩 创建 </remarks>
        public CustomActionFilterAttribute(bool isFilter = true)
        {
            this.IsFilter = isFilter;
        }
        #endregion

        #region 重写方法

        /// <summary>
        /// 基本验证
        /// </summary>
        /// <param name="filterContext"></param>
        /// <returns>void</returns>
        /// <remarks> 2013-6-27 杨浩 创建 </remarks> 
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            
            var currentUser = BLL.Authentication.AdminAuthenticationBo.Instance.Current;
            
            if (!IsFilter)
            {
                base.OnActionExecuting(filterContext);
                return;
            }
            //return;
            bool isLogin = BLL.Authentication.AdminAuthenticationBo.Instance.IsLogin;
            if (currentUser != null && isLogin)
            {
                if (currentUser.Base.Status == (int)Model.WorkflowStatus.SystemStatus.系统用户状态.禁用)
                {
                    filterContext.Result = new ViewResult
                    {
                        ViewName = "ErrorPrivilegeWithMessage",
                        ViewData = new ViewDataDictionary("此账号已被禁用！")
                    };
                }
                //判断是否有一般权限
                var dicPrivilege = HasAction(filterContext);

                //判断是否有菜单权限
                //bool hasMenu = false;
                var values = filterContext.Controller.ControllerContext.RouteData.Values;
                var controller = values["controller"].ToString().ToLower();
                var action = values["action"].ToString().ToLower();
                var t = ((List<SyMenu>)currentUser.MenuList);
                var hasMenu = currentUser.MenuList.FirstOrDefault(
                        m => m.MenuUrl != null && (m.Status == (int)Model.WorkflowStatus.SystemStatus.菜单状态.启用
                                                   && m.MenuUrl.ToLower().Contains(controller)
                                                   && m.MenuUrl.ToLower().Contains(action))
                        ) != null;

                if ((dicPrivilege.Count > 0 && !dicPrivilege.First().Value) || (dicPrivilege.Count == 0 && (!hasMenu)))
                {
                    filterContext.Result = new ViewResult { ViewName = "ErrorPrivilege" };
                }

            }
            else
            {
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    Script = string.Format(Script, "false");
                    filterContext.Result = new JsonResult
                        {
                            Data = new { IsLogout = true, Message = "登录超时，请重新登录", Callback = Script },
                            JsonRequestBehavior = JsonRequestBehavior.AllowGet
                        };
                }
                else
                {
                    filterContext.Result = new ViewResult { ViewName = "ErrorLogin" };

                    #region

                    //filterContext.Result = new RedirectResult("/Account/Login/");
                    //filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary(new { Controller = "Account", action = "Login" }));
                    //filterContext.Result = new HttpUnauthorizedResult();

                    #endregion
                }

            }
        }

        /// <summary>
        /// 判断控制器或动作是否有权限
        /// </summary>
        /// <param name="filterContext"></param>
        /// <returns>返回权限</returns>
        /// <remarks> 2013-6-27 杨浩 创建 </remarks>
        public Dictionary<string, bool> HasAction(ActionExecutingContext filterContext)
        {
            var currentUser = BLL.Authentication.AdminAuthenticationBo.Instance.Current;
            var result = new Dictionary<string, bool>();
            var actionPrivilege = filterContext.ActionDescriptor.GetCustomAttributes(typeof(PrivilegeAttribute), false).Cast<PrivilegeAttribute>();
            var controllerPrivilege = filterContext.ActionDescriptor.ControllerDescriptor.GetCustomAttributes(typeof(PrivilegeAttribute), false).Cast<PrivilegeAttribute>();

            var privileges = new HashSet<string>();
            if (actionPrivilege.Any())
                actionPrivilege.First().Allow.ToList().ForEach(p => privileges.Add(p.ToString()));
            if (controllerPrivilege.Any())
                controllerPrivilege.First().Allow.ToList().ForEach(p => privileges.Add(p.ToString()));

            foreach (var pr in privileges)
            {
                //if (pr == PrivilegeCode.IgnoreAction.ToString())
                //{
                //    ignoreAction = PrivilegeCode.IgnoreAction;
                //    hasPrivilege = true;
                //    break;
                //}

                if (pr == PrivilegeCode.None.ToString())
                {
                    return new Dictionary<string, bool> { { pr, true } };
                }

                if (currentUser.PrivilegeList.Any(p => p.Code == pr.ToString()))
                {
                    //ignoreAction = PrivilegeCode.IgnoreAction;
                    return new Dictionary<string, bool> { { pr, true } };
                }
            }

            if (privileges.Count > 0)
            {
                result.Add(privileges.First(), false);
            }
            return result;
        }

        #endregion
    }
}