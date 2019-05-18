using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using Hyt.Model;
using Hyt.Model.Transfer;

namespace Hyt.BLL.Authentication
{
    /// <summary>
    /// 前台身份验证
    /// </summary>
    /// <remarks>
    /// 2013-7-9 杨浩 创建
    /// </remarks>
    public class CustomerAuthenticationBo : BOBase<CustomerAuthenticationBo>
    {
        /// <summary>
        /// 系统登录
        /// </summary>
        /// <param name="account">账户</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        /// <remarks>
        /// 2013-6-26 杨浩 创建
        /// </remarks>
        public Result<CBCrCustomer> Login(string account, string password)
        {
            var result = new Result<CBCrCustomer>();

            //var customer = Hyt.BLL.CRM.CrCustomerBo.Instance.SSOGetCustomerByLogin(account, password);
            //if (customer == null)
            //{
            //    return new Result<CBCrCustomer>()
            //        {
            //            Status = false,
            //            Message = "账户名或密码错误"
            //        };
            //}

            var cbCustomer = BLL.CRM.CrCustomerBo.Instance.GetCrCustomer(account);
            if (cbCustomer != null && Hyt.Util.EncryptionUtil.VerifyCiphetrextWithMd5AndSalt(password,cbCustomer.Password))
            {
                if (cbCustomer.Status == 1)
                {
                    //更新登录IP、登录时间
                    Hyt.BLL.Web.CrCustomerBo.Instance.UpdateDateTimeAndIp(cbCustomer.SysNo, Hyt.Util.WebUtil.GetUserIp());
                    //写入认证信息
                    Ticket(cbCustomer, false);
                    result.Status = true;
                    result.Data = cbCustomer;
                    result.Message = "登录成功!";
                }
                else
                {
                    result.Status = false;
                    result.Message = "您的账户已被系统锁定，请联系客服！"; 
                }

            }
            else
            {
                result.Status = false;
                result.Message = "账户名或密码错误";
            }
            return result;
        }

        /// <summary>
        /// 登出认证
        /// </summary>
        /// <remarks>2013-06-26 杨浩 创建</remarks>
        public void Logout()
        {
            FormsAuthentication.SignOut();
        }

        /// <summary>
        /// 创建票证
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="isPersistent"></param>
        /// <remarks>2013-06-26 杨浩 创建</remarks>
        private void Ticket(CrCustomer customer, bool isPersistent)
        {
            var now = DateTime.Now;

            var ticket = new FormsAuthenticationTicket(
                2,
                customer.SysNo.ToString(),
                now,
                now.Add(FormsAuthentication.Timeout),
                isPersistent,
                customer.Account,
                FormsAuthentication.FormsCookiePath);

            var encryptedTicket = FormsAuthentication.Encrypt(ticket);

            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
            cookie.HttpOnly = true;
            if (ticket.IsPersistent)
            {
                cookie.Expires = ticket.Expiration;
            }
            cookie.Secure = FormsAuthentication.RequireSSL;
            cookie.Path = FormsAuthentication.FormsCookiePath;
            if (FormsAuthentication.CookieDomain != null)
            {
                cookie.Domain = FormsAuthentication.CookieDomain;
            }

            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        /// <summary>
        /// 客户是否登录
        /// </summary>
        /// <remarks>
        /// 2013-6-26 杨浩 创建
        /// </remarks>
        public bool IsLogin
        {
            get
            {
                return UserSysNo > 0;
            }
        }

        /// <summary>
        /// 当前登录客户的客户编号
        /// </summary>
        /// <returns>客户编号</returns>
        /// <remarks>
        /// 2013-6-26 杨浩 创建
        /// </remarks>
        public int CustomerSysNo
        {
            get
            {
                return UserSysNo;
            }
        }

        /// <summary>
        /// 当前登录客户的客户编号
        /// </summary>
        /// <returns>客户编号</returns>
        /// <remarks>
        /// 2013-6-26 杨浩 创建
        /// </remarks>
        private int UserSysNo
        {
            get
            {
                if (!(HttpContext.Current.User.Identity is FormsIdentity))
                    return 0;
                var formsIdentity = (FormsIdentity)HttpContext.Current.User.Identity;
                int sysNo = int.Parse(formsIdentity.Name);
                return sysNo;
            }
        }

        /// <summary>
        /// 当前用户
        /// </summary>
        public CrCustomer CurrentUser
        {
            get
            {
                return Hyt.BLL.CRM.CrCustomerBo.Instance.GetCrCustomerItem(UserSysNo);
            }
        }
    }
}
