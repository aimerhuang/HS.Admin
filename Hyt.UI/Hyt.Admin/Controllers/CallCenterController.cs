using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Hyt.BLL.Authentication;
using Hyt.BLL.CRM;
using Hyt.BLL.Log;
using Hyt.Model.Parameter;
using Hyt.Model.SystemPredefined;
using Hyt.Model.WorkflowStatus;
using Hyt.Util;

namespace Hyt.Admin.Controllers
{
    /// <summary>
    /// 呼叫中心
    /// </summary>
    /// <remarks>2013-8-8 黄伟 创建</remarks> 
    [CustomActionFilter(false)]
    public class CallCenterController : BaseController
    {
        /// <summary>
        /// private key used to vailidate the data 
        /// </summary>
        private const string Key = "15ba577241cf21c237d80d3695a98596";

        //
        // GET: /CallingCener/
        /// <summary>
        /// 呼叫中心入口
        /// </summary>
        /// <param name="phone">ParaCallCenterIndex to string</param>
        /// <param name="sso_data">sso_data</param>
        /// <param name="sso_auth">sso_auth</param>
        /// <param name="callrecid">callrecid</param>
        /// <returns>呼叫中心-客户信息</returns>
        /// <remarks>2013-8-8 黄伟 创建</remarks>
        [Privilege(PrivilegeCode.CC1001101)]
        //[Privilege(PrivilegeCode.FN1005201)]
        [HttpGet]
        public ActionResult Index(string phone, string sso_data, string sso_auth, string callrecid)
        {
            //return View(new ParaCallCenterIndex
            //{
            //    Phone = "13683459249",
            //    SSO_data = new SSOData{Name="admin",OperatorId = "1"}
            //    //SSO_auth = sso_auth,
            //    //SSO_data = model,
            //    //Callrecid = callrecid
            //});

            //var str = "phone=18965017897&sso_auth=79d9cfc395bb99f05dad903eb8b55366&sso_data=%257B%2522clgNumber%2522%253A%25228010%2522%252C%2522company%2522%253A%2522pskj%2522%252C%2522crmaccount%2522%253Anull%252C%2522crmpwd%2522%253Anull%252C%2522groupid%2522%253A%25221%2522%252C%2522name%2522%253A%25228017%2522%252C%2522operatorid%2522%253A%25228017%2522%252C%2522password%2522%253A%25228017%2522%252C%2522ts%2522%253A%25221376361153%2522%257D&callrecid=00018629416751546&calloutid=0&area=%E7%A6%8F%E5%BB%BA%E7%9C%81%E7%A6%8F%E5%B7%9E%E5%B8%82&accnum=888888&ivrnode=1&websid= 7777";
            //var test =HttpUtility.UrlDecode(str);
            ////var test1 = HttpUtility.UrlDecodeToBytes(str);
            //var test2 = HttpUtility.UrlDecode(test);
            SSOData model;
            try
            {
                model = new JavaScriptSerializer().Deserialize<SSOData>(HttpUtility.UrlDecode(sso_data));
            }
            catch (Exception ex)
            {
                SysLog.Instance.Error(LogStatus.系统日志来源.后台, ex.Message + "\n\rQueryString:" + Request.QueryString,
                                      LogStatus.系统日志目标类型.用户, 0, ex);
                throw;
            }
            //log
            var ip = HttpContext.Request.ServerVariables["REMOTE_ADDR"];
            //if(!ip.StartsWith("192.168.10"))
            //SysLog.Instance.Info(LogStatus.系统日志来源.后台, Request.QueryString.ToString(),LogStatus.系统日志目标类型.用户,0, ip);//marked for ip recorded
            /* authencation 
             *sso_auth 认证参数。统一小写。MD5(key+sso_data)
             * MD5(key+sso_data)==model.SSO_auth
            */
            //var hashCode = EncryptionUtil.MD5Encrypt(Key + HttpUtility.UrlDecode(sso_data));
            // var hashCode = EncryptionUtil.MD5Encrypt(Key + HttpUtility.UrlEncode(sso_data));
            var hashCode = EncryptionUtil.EncryptWithMd5(Key + HttpUtility.UrlEncode(sso_data));

            if (sso_auth == hashCode)
            {
                if (model.Name.ToLower() == "admin")
                {
                    Response.Charset = Encoding.UTF8.WebName;
                    Response.StatusCode = 401;
                    Response.Write("<h1> admin 用户被拒绝</h1>");
                    Response.End();
                    SysLog.Instance.Warn(LogStatus.系统日志来源.后台,
                                        "呼叫中心登录:" + model.Name + " 拒绝" ,
                                        LogStatus.系统日志目标类型.用户, 0, -1);
                    return null;
                }

                var result = AdminAuthenticationBo.Instance.Login(model.Name);
                //var result = AdminAuthenticationBo.Instance.Login("admin"); //admin to demo

                if (!result.Status)
                {
                    SysLog.Instance.Warn(LogStatus.系统日志来源.后台,
                                         "呼叫中心登录:" + model.Name + " " +result.Status+","+ result.Message,
                                         LogStatus.系统日志目标类型.用户,0,-1);

                    //Response.Write("<script>alert('" + result .Message+ "!');</script>");
                    Response.Charset = Encoding.UTF8.WebName;
                    Response.StatusCode = 401;
                    Response.Write("<h1>" + result.Message + "</h1>");
                    Response.End();
                    return null;
                }
                else
                {
                    SysLog.Instance.Info(LogStatus.系统日志来源.后台,
                                         "呼叫中心登录:" + model.Name + "(" + result.Data.UserName + ")" + " " +
                                         result.Message,
                                         LogStatus.系统日志目标类型.用户, result.Data.SysNo,
                                         result.Data.SysNo);
                }
            }
            else
            {
                //not authorized
                //return Redirect("/Shared/Error404");
                //return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
                //Response.Write("<script>alert('" + result .Message+ "!');</script>");
                SysLog.Instance.Warn(LogStatus.系统日志来源.后台,
                                     "SSO_auth消息验证失败hashcode:" + hashCode + "\n\rQueryString:" + Request.QueryString,
                                     LogStatus.系统日志目标类型.用户,-1,-1);
                Response.Write(string.Format("<h1>SSO_auth消息验证失败sso_auth:{0} hashcode:{1}</h1>", sso_auth, hashCode));
                Response.End();
                 return null;
            }

            //retrieve the data needed
            //sysno from crcustomer by phone--which is the account in customer
            var cus=CrCustomerBo.Instance.GetCrCustomer(phone);
            if (cus != null)
            {
                ViewBag.cusSysNo = cus.SysNo;
                ViewBag.cusAccount = model.Name;
            }

            return View(new ParaCallCenterIndex
                {
                    Phone = phone,
                    SSO_auth = sso_auth,
                    SSO_data = model,
                    Callrecid = callrecid
                });
        }

        /// <summary>
        /// 根据手机号获取客户信息
        /// </summary>
        /// <param name="phone">客户手机号</param>
        /// <returns>返回客户信息</returns>
        /// <remarks>2013-08-06 沈强 创建</remarks>
        [Privilege(PrivilegeCode.CC1001101)]
        public ActionResult Customer(string phone)
        {
            var meberInfoUrl = "/CRM/CrCustomer";
            var c = BLL.CRM.CrCustomerBo.Instance.GetCrCustomer(phone);
            if (c != null)
            {
                meberInfoUrl = "/CRM/CrCustomerEditDetail?sysno=" + c.SysNo;
            }
            return new RedirectResult(meberInfoUrl);
        }
    }
}