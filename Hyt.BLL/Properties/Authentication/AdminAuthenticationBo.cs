using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using Hyt.BLL.Sys;
using Hyt.BLL.Warehouse;
using Hyt.Infrastructure.Memory;
using Hyt.Model;
using Hyt.BLL.Log;
using Hyt.Model.WorkflowStatus;
using Hyt.Model.SystemPredefined;
using Hyt.BLL.Distribution;
using Hyt.Model.Transfer;
using Hyt.BLL.Transport;

namespace Hyt.BLL.Authentication
{
    /// <summary>
    /// 后台身份验证
    /// </summary>
    /// <remarks>2013-06-24 吴文强 创建</remarks>
    public class AdminAuthenticationBo : BOBase<AdminAuthenticationBo>, IAuthenticationBo
    {
        /// <summary>
        /// 登录认证信息
        /// </summary>
        /// <param name="user">用户对象</param>
        /// <param name="isPersistent">如果票证将存储在持久性 Cookie 中（跨浏览器会话保存），则为 true；否则为 false。如果该票证存储在 URL 中，将忽略此值。</param>
        /// <returns></returns>
        /// <remarks>2013-06-24 吴文强 创建</remarks>
        public void Ticket(SyUser user, bool isPersistent)
        {
            var now = DateTime.Now;

            var ticket = new FormsAuthenticationTicket(
                1,
                user.SysNo.ToString(),
                now,
                now.Add(FormsAuthentication.Timeout),
                isPersistent,
                user.Account,
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
        /// 登出认证
        /// </summary>
        /// <returns></returns>
        /// <remarks>2013-06-24 吴文强 创建</remarks>
        /// <remarks>2014-05-05 黄波 添加SESSION清除</remarks>
        public void Logout()
        {
            try
            {
                HttpContext.Current.Session.Abandon();
                FormsAuthentication.SignOut();
                //清缓存
                MemoryProvider.Default.Remove(string.Format(KeyConstant.SyUser, UserID));
                var userCacheKey = string.Format("CACHE_SYUSER_{0}", UserID);
                MemoryProvider.Default.Remove(userCacheKey);
            }
            catch { }
        }

        /// <summary>
        /// 获取登录用户对象
        /// </summary>
        /// <returns>返回通过认证的用户对象</returns>
        /// <remarks>2013-06-24 吴文强 创建</remarks>
        public SyUser GetAuthenticatedUser()
        {
            if (HttpContext.Current == null ||
                HttpContext.Current.Request == null ||
                !HttpContext.Current.Request.IsAuthenticated ||
                !(HttpContext.Current.User.Identity is FormsIdentity))
            {
                return new SyUser();
            }
            int id;
            int.TryParse(HttpContext.Current.User.Identity.Name, out id);


            var formsIdentity = (FormsIdentity)HttpContext.Current.User.Identity;
            var customer = GetAuthenticatedUserFromTicket(formsIdentity.Ticket);
            return customer;
        }

        /// <summary>
        /// 根据票证获取用户
        /// </summary>
        /// <param name="ticket">票证</param>
        /// <returns>用户对象</returns>
        /// <remarks> 2013-6-26 杨浩 创建 </remarks>
        public virtual SyUser GetAuthenticatedUserFromTicket(FormsAuthenticationTicket ticket)
        {
            if (ticket == null)
                throw new ArgumentNullException("ticket");

            var sysNo = ticket.Name;

            if (String.IsNullOrWhiteSpace(sysNo))
                return null;

            #region 非sso模式
            //将从ticket中取取的ssoId转换成syNo 
            int currentSysNo = SySsoUserAssociationBo.Instance.GetSyNoBySsoID(int.Parse(sysNo));

            if (currentSysNo < 0)
                currentSysNo = int.Parse(sysNo);

            var syUser = SyUserBo.Instance.GetSyUser(currentSysNo);
            return syUser;
            #endregion 

            #region sso模式

            ////将从ticket中取取的ssoId转换成syNo
            //int currentSysNo = SySsoUserAssociationBo.Instance.GetSyNoBySsoID(int.Parse(sysNo));
            //if (currentSysNo >= 0)
            //{
            //    var syUser = SyUserBo.Instance.GetSyUser(currentSysNo);
            //    return syUser;
            //}
            //else
            //{
            //    return new SyUser() { SysNo = -1 };
            //}
            #endregion 

           
        }

        /// <summary>
        /// 获取用户权限认证信息
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// 2013-6-26 杨浩 创建
        /// </remarks>
        private SysAuthorization GetSysAuthorization()
        {
            var syUser = GetAuthenticatedUser();
            if (syUser == null)
            {
                return null;
            }
            var hasAllWarehouse = SyUserGroupBo.Instance.IsHasAllWarehouse(syUser.SysNo);
            var hasAllDealer = SyUserGroupBo.Instance.IsHasAllDealer(syUser.SysNo);
            var DealerModel = new CBDsDealer();
            var Dealer = DsDealerBo.Instance.GetDsDealerByUserNo(syUser.SysNo);
            bool IsBindDealer;
            //判断当前用户是否存在分销商账号，没有默认为0（信营全球购账号）
            //2015-12-19 王耀发 创建
            if (Dealer == null)
            {
                DealerModel = DsDealerBo.Instance.GetDsDealer(0);
                IsBindDealer = false;
            }
            else
            {
                DealerModel = Dealer;
                IsBindDealer = true;
            }

            
           

            var info = new SysAuthorization
                {
                    Base = syUser,
                    MyMenuList = SyMyMenuBo.Instance.GetList(syUser.SysNo),
                    MenuList = SyMenuBO.Instance.GetList(syUser.SysNo),
                    PrivilegeList = SyPrivilegeBo.Instance.GetList(syUser.SysNo),
                    IsAllWarehouse=hasAllWarehouse,
                    Warehouses =
                        hasAllWarehouse
                            ? WhWarehouseBo.Instance.GetAllWarehouseList()
                            : WhWarehouseBo.Instance.GetUserWarehouseList(syUser.SysNo),
                    Dealer = DealerModel,
                    IsBindDealer = IsBindDealer,
                    Dealers =
                        hasAllDealer
                            ? DsDealerBo.Instance.GetAllDealerList()
                            : DsDealerBo.Instance.GetUserDealerList(syUser.SysNo),
                    IsBindAllDealer = hasAllDealer ? true : false,
                    IsAgent = SyUserGroupBo.Instance.GroupContainsUser(4, syUser.SysNo)
                  
                };
            return info;
        }
        /// <summary>
        /// 后去当前用户信息
        /// 王耀发 2016-3-5 创建
        /// </summary>
        /// <returns></returns>
        public SysAuthorization GetCurrentUserInfo()
        {
            return GetSysAuthorization();
        }
        /// <summary>
        /// 获取用户权限认证信息
        /// </summary>
        /// <returns>SysAuthorization</returns>
        /// <remarks>
        /// 2013-6-26 杨浩 创建
        /// </remarks>
        public SysAuthorization Current
        {
            get
            {
                if (UserID == default(int))
                    return null;
                return MemoryProvider.Default.Get(string.Format(KeyConstant.SyUser, UserID), GetSysAuthorization);
            }
        }

        /// <summary>
        /// 系统登录
        /// </summary>
        /// <param name="account">账户</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        /// <remarks>
        /// 2013-6-26 杨浩 创建
        /// </remarks>
        public Result<SyUser> Login(string account, string password)
        {
            var result = new Result<SyUser>();
            var syUser = BLL.Sys.SyUserBo.Instance.GetSyUser(account);
            if (syUser != null)
            {
                if (syUser.Status == (int)SystemStatus.系统用户状态.禁用)
                {
                    result.Status = false;
                    result.Message = "该账户已被禁用,请联系管理员！";
                    return result;
                }
                if (!Hyt.Util.EncryptionUtil.VerifyCiphetrextWithMd5AndSalt(password, syUser.Password))
                {
                    result.Status = false;
                    result.Message = "账户名或密码错误！";
                    return result;
                }
                //写入认证信息
                Ticket(syUser, false);
                result.Status = true;
                result.Data = syUser;

                //清缓存
                MemoryProvider.Default.Remove(string.Format(KeyConstant.SyUser, UserID));
            }
            else
            {
                result.Status = false;
                result.Message = "账户名不存在！";
            }
            return result;
        }

        /// <summary>
        /// 系统登录(可信登录,无需密码)
        /// </summary>
        /// <param name="account">账户</param>
        /// <returns></returns>
        /// <remarks>
        /// 2013-6-26 杨浩 创建
        /// 2013-8-5 何方 重构
        /// </remarks>
        public Result<SyUser> Login(string account)
        {
            var result = new Result<SyUser>();
            var syUser = BLL.Sys.SyUserBo.Instance.GetSyUser(account);
            if (syUser != null)
            {
                //写入认证信息
                Ticket(syUser, false);
                result.Status = true;
                result.Data = syUser;
                //_syuser = syUser;
                //_sysAuthorization = GetSysAuthorization();
            }
            else
            {
                result.Status = false;
                result.Message = "账号错误";
            }
            return result;
        }

        /// <summary>
        /// 是否登录
        /// </summary>
        /// <returns>bool</returns>
        /// <remarks>
        /// 2013-6-26 杨浩 创建
        /// </remarks>
        public bool IsLogin
        {
            get
            {
                return UserID > 0;
            }
        }

        /// <summary>
        /// 当前系统用户编号
        /// </summary>
        /// <remarks>
        /// 2013-6-26 杨浩 创建
        /// </remarks>
        private int UserID
        {
            get
            {
                if (HttpContext.Current == null)
                    return 0;
                if (!(HttpContext.Current.User.Identity is FormsIdentity))
                    return 0;
                var formsIdentity = (FormsIdentity)HttpContext.Current.User.Identity;
                int sysNo = int.Parse(formsIdentity.Name);

                //sysNo = SySsoUserAssociationBo.Instance.GetSyNoBySsoID(sysNo);//把ssoid转换成sysNo
                return sysNo;
            }
        }

        /// <summary>
        /// 判断用户是否拥有某个仓库权限
        /// </summary>
        /// <param name="wareHouseSysNo">The ware house sys no.</param>
        /// <returns></returns>
        /// <remarks>
        /// 2013-11-26 何方 创建
        /// </remarks>
        public bool HasWareHousePrivilege(int wareHouseSysNo)
        {
            var syUser = GetAuthenticatedUser();
            if (SyUserGroupBo.Instance.IsHasAllWarehouse(syUser.SysNo))
            {
                return true;
            }
            return GetSysAuthorization().Warehouses.Count(x => x.SysNo == wareHouseSysNo) > 0;
        }
    }

    /// <summary>
    /// 扩展
    /// </summary>
    /// <remarks>2013-8-19 杨浩 创建</remarks>
    public static class Extensions
    {
        /// <summary>
        /// 判断权限代码是否在权限列表中
        /// </summary>
        /// <param name="privilegeList">权限列表</param>
        /// <param name="code">权限代码</param>
        /// <returns>bool</returns>
        /// <remarks>2013-8-19 杨浩 创建</remarks>
        public static bool HasPrivilege(this IList<SyPrivilege> privilegeList, PrivilegeCode code)
        {
            var hasAction = AdminAuthenticationBo.Instance.Current.PrivilegeList.Any(x => x.Code == code.ToString());
            return hasAction;
        }
    }

}
