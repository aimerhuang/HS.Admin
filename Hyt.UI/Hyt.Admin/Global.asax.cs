using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Hyt.BLL.Base;
using Hyt.Service.Task.Core;
using Hyt.BLL.Logistics;
using Hyt.DataAccess.MallSeller;

namespace Hyt.Admin
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            #region 初始化系统运行时

            DataProviderBo.Set(
                Activator.CreateInstance(Type.GetType("Hyt.DataAccess.Oracle.DataProvider,Hyt.DataAccess.Oracle")));

            //升级数据库
            BLL.Sys.SyUpgradeTheDatabaseBo.Upgrade();

            var enableTask = bool.Parse(ConfigurationManager.AppSettings["EnableTask"]);
            if (enableTask)
            {
                TaskManager.Instance.Init(); 
            }

            #endregion
        }

        protected void Application_End()
        {
            #region 分析Application Pool 退出/重启的原因
#if !DEBUG
            try
            {
                HttpRuntime runtime = (HttpRuntime) typeof (System.Web.HttpRuntime).InvokeMember(
                    "_theRuntime", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, null,
                    null
                                                        );

                if (runtime == null)
                    return;

                string shutDownMessage = (string) runtime.GetType().InvokeMember(
                    "_shutDownMessage", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetField, null,
                    runtime, null
                                                      );

                string shutDownStack = (string) runtime.GetType().InvokeMember(
                    "_shutDownStack", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetField, null,
                    runtime, null
                                                    );

                Hyt.Util.Log.LogManager.Instance.WriteLog("ASP.NET应用程序关闭。\r\n关闭消息：\r\n" + shutDownMessage + "堆栈跟踪：\r\n" +
                                                          shutDownStack);
            }
            catch
            {
                Hyt.Util.Log.LogManager.Instance.WriteLog("ASP.NET应用程序关闭。\r\n关闭原因：\r\n" +
                                                          HostingEnvironment.ShutdownReason);
            }
#endif

            #endregion
        }

        #region DEBUG

        //#if DEBUG //DEBUG模式下启用
        //        protected void Application_Error(object sender, EventArgs e)
        //        {
        //            #region 记录错误日志

        //            Exception LastError = Server.GetLastError();
        //            if (LastError != null)
        //            {

        //                Exception ex = LastError.GetBaseException();
        //                if (ex as HttpException != null)
        //                {
        //                    if (((HttpException)ex).GetHttpCode() == 404)
        //                    {
        //                        //转向404页面
        //                        Response.Write("404");
        //                        return;
        //                    }
        //                }
        //                Server.ClearError();
        //                //string errorID = BLL.LogBO.Instance.WriteLog(ex);
        //                if (ex is HttpRequestValidationException)
        //                {
        //                    Response.Write(string.Empty);
        //                }
        //                else
        //                {
        //                    Response.Write("<b>错误ID</b>：" + "" + "<br/><br/><b>错误信息</b>:" + ex.Message +
        //                                   "<br/><br/><b>错误堆栈</b>：" + ex.StackTrace);
        //                }
        //                Response.End();
        //            }

        //            #endregion
        //        }
        //#endif

        #endregion

       
    }
}