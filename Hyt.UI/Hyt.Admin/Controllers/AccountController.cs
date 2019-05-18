using System;
using System.Web.Mvc;
using Hyt.BLL.Authentication;
using Hyt.BLL.Log;
using Hyt.BLL.Sys;
using Hyt.Model;
using Hyt.Model.SystemPredefined;
using Hyt.Model.WorkflowStatus;
using Hyt.Util;
using Hyt.Util.Validator.Rule;
using Extra.SMS.Mai;
using System.IO;
using System.Xml;
using System.Configuration;

namespace Hyt.Admin.Controllers
{
    /// <summary>
    /// 用户账户控制器
    /// </summary>
    /// <remarks>2014-1-8 黄波 创建</remarks>
    [CustomActionFilter(false)]
    public class AccountController : BaseController
    {

        private readonly Grand.Framework.Service.ServiceConfig _serverconfig = ConfigurationManager.GetSection("ServiceConfig") as Grand.Framework.Service.ServiceConfig;
        /// <summary>
        /// SSO登录
        /// </summary>
        /// <remarks>2014-10-14 杨浩 创建</remarks>  
        //[HttpGet]
        //public ActionResult Login()
        //{
        //    if (CurrentUser != null)
        //    {
        //        //清缓存
        //        Hyt.Infrastructure.Memory.MemoryProvider.Default.Remove(string.Format(Hyt.Infrastructure.Memory.KeyConstant.SyUser, CurrentUser.Base.SysNo));
        //    }
        //    return Grand.EAI.SSO.Core.AuthManager.Instance.InitAuth(this._serverconfig.AppKey, this._serverconfig.AppSecret, null);
        //}
        #region ==杨浩==

        #region =====登录页=====
        /// <summary>
        /// 登录
        /// </summary>
        /// <returns>登录页面</returns>
        /// <remarks>2013-6-5 杨浩 创建</remarks>
        [HttpGet]
        public ActionResult Login()
        {
            Session["LOGIN_STAMP"] = DateTime.Now.Ticks;
            ViewBag.Stamp = Session["LOGIN_STAMP"];
            return View();
        }

        /// <summary>
        /// 系统登录
        /// </summary>
        /// <param name="account">账户</param>
        /// <param name="password">密码</param>
        /// <param name="verifycode">验证码</param>
        /// <returns>返回登录验证结果</returns>
        /// <remarks>
        /// 2013-6-26 杨浩 添加
        /// 2013-11-14 苟治国 修改
        /// 2014-04-30 朱家宏 增加登录日志
        /// </remarks>
        [HttpPost]
        public JsonResult Login(string account, string password, string verifycode)
        {                   
            var result = new Model.Result<SyUser>();
            if (Session["verifycode"] == null)
            {
                result.Message = "验证码超时!";
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            if (Session["verifycode"].ToString().ToLower() != verifycode.ToLower() || string.IsNullOrEmpty(verifycode))
            {
                result.Message = "验证码输入错误!";
                return Json(result, JsonRequestBehavior.AllowGet);
            }

            result = AdminAuthenticationBo.Instance.Login(account, password);
            if (result.Status)
            {
                RecordLoginUserName(account);
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Info, LogStatus.系统日志来源.后台, "用户成功登录。账户名:" + account,
                    LogStatus.系统日志目标类型.登录, result.Data.SysNo, null, null, result.Data.SysNo);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    
        /// <summary>
        /// 系统登录
        /// </summary>
        /// <param name="account">账户</param>
        /// <param name="password">密码</param>
        /// <param name="verifycode">验证码</param>
        /// <returns>返回登录验证结果</returns>
        /// <remarks>
        /// 2013-11-14 苟治国 创建
        /// 2014-04-30 朱家宏 增加登录日志
        /// </remarks>
        public JsonResult DoLogin(string account, string password, string verifycode)
        {
            var result = new Model.Result<SyUser>();
            if (Session["verifycode"] == null)
            {
                result.Message = "验证码超时!";
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            if (Session["verifycode"].ToString().ToLower() != verifycode.ToLower() || string.IsNullOrEmpty(verifycode))
            {
                result.Message = "验证码输入错误!";
                return Json(result, JsonRequestBehavior.AllowGet);
            }

            result = AdminAuthenticationBo.Instance.Login(account, password);
            if (result.Status)
            {
                RecordLoginUserName(account);
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Info, LogStatus.系统日志来源.后台, "用户成功登录。账户名:" + account,
                    LogStatus.系统日志目标类型.登录, result.Data.SysNo, null, null, result.Data.SysNo);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// ajax验证验证码
        /// </summary>
        /// <param name="verifyCode">验证码</param>
        /// <returns>返回是否相等</returns>
        /// <remarks>2010-10-31 邵斌 创建</remarks>
        [HttpPost]
        public JsonResult CheckVerifyCode(string verifyCode)
        {
            var result = new Model.Result();

            //默认验证码错误
            result.Message = "验证码输入错误!";
            result.Status = false;
            result.StatusCode = -1;

            //判断验证码是否为空
            if (!string.IsNullOrWhiteSpace(verifyCode) && Session["verifycode"] != null)
            {
                //判断验证码是否和Session中验证码相等
                if (Session["verifycode"].ToString().ToLower() == verifyCode.ToLower())
                {
                    //验证码正确，设置返回信息
                    result.Message = "";
                    result.Status = true;
                    result.StatusCode = 1;
                }
            }

            return Json(result.Status, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// 登出认证
        /// </summary>
        /// <returns>Login页面</returns>
        /// <remarks>
        /// 2013-6-26 杨浩 添加
        /// 2014-04-30 朱家宏 增加日志
        /// </remarks>
        public ActionResult Logout()
        {
            #region 非SSO模式
            AdminAuthenticationBo.Instance.Logout();
            try
            {
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Info, LogStatus.系统日志来源.后台,
                    "用户退出系统。账户名:" + CurrentUser.Base.Account,
                    LogStatus.系统日志目标类型.登录, CurrentUser.Base.SysNo, null, null, CurrentUser.Base.SysNo);
            }
            catch { }
            return View("Login");
            #endregion

            #region sso模式
            //AdminAuthenticationBo.Instance.Logout();      
            //return Grand.EAI.SSO.Core.AuthManager.Instance.SignOutAll(_serverconfig.AppKey, _serverconfig.AppSecret);
            #endregion
        }

        /*
        /// <summary>
        /// Ajax登录
        /// </summary>
        /// <param name="account">用户账号</param>
        /// <param name="password">密码</param>
        /// <param name="verifyCode">验证码</param>
        /// <param name="stamp">时间戳</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Login(string account, string password, string verifyCode,string stamp)
        {
            var result = new Result();

            result.Message = "";

            if (Session["verifycode"] == null)
            {
                result.Message += "验证码超时!";
            }
            else
            {
                if (Session["verifycode"].ToString().ToLower() != verifyCode.ToLower() || string.IsNullOrEmpty(verifyCode))
                {
                    result.Message += "验证码输入错误!";
                }
                result = AdminAuthenticationBo.Instance.Login(account, password);
                if (result != null)
                {
                    result = new Result();
                }
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

         * */
        #endregion

        #region =====Mini登录框=====
        /// <summary>
        /// Mini登录框
        /// </summary>
        /// <returns>Mini登录页面</returns>
        /// <remarks>
        /// 2013-6-15 黄波 创建
        /// </remarks>
        [HttpGet]
        public ActionResult MiniLogin()
        {
            return View();
        }
        #endregion

        #endregion

        #region 忘记密码
        /// <summary>
        /// 找回密码
        /// </summary>
        /// <returns>找回密码视图</returns>
        /// <remarks>2014-1-8 黄波 创建</remarks>
        public ActionResult FindUserPassword()
        {
            return View();
        }

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="account">账号</param>
        /// <param name="verifycode">验证码</param>
        /// <returns>密码是否重置成功</returns>
        /// <remarks>2014-1-8 黄波 创建</remarks>
        public JsonResult SetUserPassWord(string account, string verifycode)
        {
            //验证数据是否合法
            Hyt.Util.Validator.VResult vResult =
                Hyt.Util.Validator.VHelper.ValidatorRule(new Hyt.Util.Validator.Rule.Rule_NotAllowNull(account, ""),
                                                         new Rule_NotAllowNull(verifycode, "")
                                                         );
            //数据格式不合法提示
            if (!vResult.IsPass)
                return Json(new { Status = false, Message = vResult.Message });

            if (Session["verifycode"] == null)
                return Json(new { Status = false, Message = "图片验证码过期,重新获取验证吗" });
            else
            {
                if (Session["verifycode"].ToString().ToLower() != verifycode.ToLower())
                    return Json(new { Status = false, Message = "图片验证码错误" });
            }
            //验证账号是否存在
            Hyt.Model.SyUser model = BLL.Sys.SyUserBo.Instance.GetSyUser(account);
            if (model != null)
            {
                if (string.IsNullOrEmpty(model.MobilePhoneNumber))
                    return Json(new { Status = false, Message = "您还没有设置手机号！" });
                else
                {
                    string password = Hyt.Util.WebUtil.Number(6, true);
                    model.Password = Hyt.Util.EncryptionUtil.EncryptWithMd5AndSalt(password);
                    string msg = "{0}您好！您的新密码是:{1}。服务热线：4006840668【任你比商城】";
                    if (Hyt.BLL.Web.CrCustomerBo.Instance.SendSyUserPassword(model.MobilePhoneNumber, string.Format(msg, model.Account, password)))
                    {
                        BLL.Sys.SyUserBo.Instance.UpdateSyUser(model);
                        return Json(new { Status = false, Message = "新密码已发送到您的手机，请查收！" });
                    }
                    else
                        return Json(new { Status = false, Message = "发送失败，请连接管理员！" });
                }

            }
            else
                return Json(new { Status = false, Message = "账户不存在！" });
        }
        #endregion

        #region 修改密码
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <returns>视图</returns>
        /// <remarks>2013-11-4 黄志勇 创建</remarks>
        [HttpGet]
        [CustomActionFilter(true)]
        [Privilege(PrivilegeCode.CM1005155)]
        public ActionResult ModifyPwd()
        {
            var model = SyUserBo.Instance.GetSyUser(CurrentUser.Base.SysNo);
            return View(model);
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="oldPassword">旧密码（未加密）</param>
        /// <param name="newPassword">新密码（未加密）</param>
        /// <returns>操作是否成功</returns>
        /// <remarks>2013-11-15 黄志勇 创建</remarks>
        [HttpPost]
        [CustomActionFilter(true)]
        [Privilege(PrivilegeCode.CM1005155)]
        public ActionResult ModifyPassword(string oldPassword, string newPassword)
        {
            var result = new Result() {Status = false};
            try
            {
                var syUser = SyUserBo.Instance.GetSyUser(CurrentUser.Base.SysNo);
                if (Util.EncryptionUtil.VerifyCiphetrextWithMd5AndSalt(oldPassword, syUser.Password))
                {
                    syUser.Password = Util.EncryptionUtil.EncryptWithMd5AndSalt
                        (newPassword);
                    syUser.LastUpdateBy = CurrentUser.Base.SysNo;
                    syUser.LastUpdateDate = DateTime.Now;
                    SyUserBo.Instance.UpdateSyUser(syUser);
                    result.Status = true;
                }
                else
                {
                    result.Message = "旧密码错误!";
                }
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Info, LogStatus.系统日志来源.后台, "修改密码",
                                         LogStatus.系统日志目标类型.用户, CurrentUser.Base.SysNo, null, WebUtil.GetUserIp(),
                                         CurrentUser.Base.SysNo);


                #region 修改密码同步到sso企业用户
                //修改密码同步到sso企业用户 谭显锋 添加
                //int ssoid = SySsoUserAssociationBo.Instance.GetSsoIdByUserSysNo(syUser.SysNo);
                //if (ssoid > 0)//如果是企业用户，则更新密码到企业用户
                //{
                 //   var enterpriseUser = SySsoUserAssociationBo.Instance.GetSsoUserInfoBySsoUserId(ssoid);
                  // SySsoUserAssociationBo.Instance.EnterpriseUserEditPassword(enterpriseUser.EnterpriseNO, enterpriseUser.Id, oldPassword, newPassword);
                //    result.Status = true;
                //}

                #endregion

            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "修改密码:" + ex.Message,
                                         LogStatus.系统日志目标类型.用户, CurrentUser.Base.SysNo, null, WebUtil.GetUserIp(),
                                         CurrentUser.Base.SysNo);

            }
            return Json(result, JsonRequestBehavior.DenyGet);
        }

        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <param name="userName">用户姓名</param>
        /// <param name="mobilePhoneNumber">用户手机</param>
        /// <param name="emailAddress">电子邮箱</param>
        /// <returns>操作是否成功</returns>
        /// <remarks>2013-12-5 黄志勇 创建</remarks>
        [HttpPost]
        [CustomActionFilter(true)]
        [Privilege(PrivilegeCode.CM1005155)]
        public ActionResult ModifyUserInfo(string userName, string mobilePhoneNumber, string emailAddress)
        {
            var result = new Result() { Status = false };
            try
            {
                var syUser = SyUserBo.Instance.GetSyUser(CurrentUser.Base.SysNo);
                syUser.UserName = userName.Trim();
                syUser.MobilePhoneNumber = mobilePhoneNumber.Trim();
                syUser.EmailAddress = emailAddress.Trim();
                syUser.LastUpdateBy = CurrentUser.Base.SysNo;
                syUser.LastUpdateDate = DateTime.Now;
                SyUserBo.Instance.UpdateSyUser(syUser);
                result.Status = true;

                #region 如果是企业用户，则更新用户信息到企业用户 
                //int ssoid = SySsoUserAssociationBo.Instance.GetSsoIdByUserSysNo(syUser.SysNo);
                //if (ssoid > 0) //如果是企业用户，则更新用户信息到企业用户
                //{
                //    SySsoUserAssociationBo.Instance.EnterpriseUserEdit(syUser); //同时更新用户信息到企业用户 
                //}
                #endregion

                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Info, LogStatus.系统日志来源.后台, "修改用户信息",
                                         LogStatus.系统日志目标类型.用户, CurrentUser.Base.SysNo, null, WebUtil.GetUserIp(),
                                         CurrentUser.Base.SysNo);
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "修改用户信息:" + ex.Message,
                                         LogStatus.系统日志目标类型.用户, CurrentUser.Base.SysNo, null, WebUtil.GetUserIp(),
                                         CurrentUser.Base.SysNo);
            }
            return Json(result, JsonRequestBehavior.DenyGet);
        }
        #endregion
        /// <summary>
        /// 供应链接口调用测试
        /// </summary>
        /// <remarks>2016-3-14 17:35 刘伟豪 创建</remarks>  
        public string testSupply()
        {
            //var result = new Result<string>();
            //result = BLL.ApiFactory.ApiProviderFactory.GetSupplyInstance((int)Model.CommonEnum.供应链代码.客比邻).GetGoodsList(new BLL.ApiSupply.ParaSupplyProductFilter() {PageSize=10 });

            //return result.Data;

            //var order = BLL.Order.SoOrderBo.Instance.GetEntity(6374);
            var result = new Result();
            //result = BLL.ApiFactory.ApiProviderFactory.GetPayInstance((int)Model.CommonEnum.PayCode.通联支付).ApplyToCustoms(order);
            //result = BLL.ApiFactory.ApiProviderFactory.GetPayInstance((int)Model.CommonEnum.PayCode.通联支付).CustomsQuery(6374);

            return result.Message;
            /*
                <CustomsMessage>
                    <payTransactionNo>201410171651019604</payTransactionNo>
                    <orderNo>2014101783124</orderNo>
                    <status>C01</status>
                    <msg>入库成功</msg>
                    <ciqStatus>10</ciqStatus>
                    <ciqMsg>入库成功</ciqMsg>
                    <sendtime>20150713170131</sendtime>
                    <sign>4E2BB4166FD74A0DB163573324B49DB9</sign>
                </CustomsMessage>
             */

            //var url = "http://demo.singingwhale.cn/OnlinePay/TLPayCustomsAsync";

            //var xml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><CustomsMessage><payTransactionNo>201410171651019604</payTransactionNo><orderNo>2014101783124</orderNo><status>C01</status><msg>入库成功</msg><ciqStatus>10</ciqStatus><ciqMsg>入库成功</ciqMsg><sendtime>20150713170131</sendtime><sign>4E2BB4166FD74A0DB163573324B49DB9</sign></CustomsMessage>";

            //System.Text.Encoding myEncode = System.Text.Encoding.GetEncoding("UTF-8"); 
            //byte[] data = System.Text.Encoding.UTF8.GetBytes(xml);

            //System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(url);
            //request.Method = "POST";
            //request.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
            //request.ContentLength = data.Length;
            
            //try
            //{
            //    using (System.IO.Stream reqStream = request.GetRequestStream())
            //    {
            //        reqStream.Write(data, 0, data.Length);
            //    }
            //    using (System.Net.WebResponse res = request.GetResponse())
            //    {
            //        using (System.IO.StreamReader sr = new System.IO.StreamReader(res.GetResponseStream(), myEncode))
            //        {
            //            string strResult = sr.ReadToEnd();
            //            result.Message = strResult;
            //        }
            //    }
            //}
            //catch (System.Net.WebException ex)
            //{
            //    return "无法连接到服务器\r\n错误信息：" + ex.Message;
            //}  

            //return result.Message;
        }
    }
}