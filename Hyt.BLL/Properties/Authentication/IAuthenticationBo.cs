using System;
using System.Web;
using System.Web.Security;
using Hyt.BLL.Sys;
using Hyt.Model;

namespace Hyt.BLL.Authentication
{
    /// <summary>
    /// 身份验证
    /// </summary>
    /// <remarks>2013-06-24 吴文强 创建</remarks>
    public interface IAuthenticationBo
    {
        /// <summary>
        /// 登录认证
        /// </summary>
        /// <param name="user">用户对象</param>
        /// <param name="isPersistent">如果票证将存储在持久性 Cookie 中（跨浏览器会话保存），则为 true；否则为 false。如果该票证存储在 URL 中，将忽略此值。</param>
        /// <returns></returns>
        /// <remarks>2013-06-24 吴文强 创建</remarks>
        void Ticket(SyUser user, bool isPersistent);

        /// <summary>
        /// 登出认证
        /// </summary>
        /// <returns></returns>
        /// <remarks>2013-06-24 吴文强 创建</remarks>
        void Logout();

        /// <summary>
        /// 获取登录用户对象
        /// </summary>
        /// <returns>返回通过认证的用户对象</returns>
        /// <remarks>2013-06-24 吴文强 创建</remarks>
        SyUser GetAuthenticatedUser();
    }
}
