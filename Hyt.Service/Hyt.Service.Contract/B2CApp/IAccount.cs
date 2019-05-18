using System.ServiceModel;
using System.ServiceModel.Web;
using Hyt.Model;
using Hyt.Model.B2CApp;
using Hyt.Service.Contract.Base;

namespace Hyt.Service.Contract.B2CApp
{
    /// <summary>
    /// 用户登录服务契约
    /// </summary>
    /// <remarks> 2013-7-5 杨浩 创建</remarks>
    [ServiceKnownType(typeof(Result))]
    [ServiceContract]
    public interface IAccount : IBaseServiceContract
    {
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="account">账户(手机号)</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        /// <remarks> 2013-7-5 杨浩 创建</remarks>
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        [CustomOperationBehavior(false)]
        Result<Customer> Login(string account, string password);

        /// <summary>
        /// 用户登出
        /// </summary>
        /// <param name="account">账户(手机号)</param>
        /// <param name="token">token</param>
        /// <returns></returns>
        /// <remarks> 2013-7-5 杨浩 创建</remarks>
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        [CustomOperationBehavior(false)]
        Result LogOut(string account, string token);

        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="account">账户(手机号)</param>
        /// <param name="password">密码</param>
        /// <param name="code">安全码</param>
        /// <returns></returns>
        /// <remarks> 2013-7-5 杨浩 创建</remarks>
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        [CustomOperationBehavior(false)]
        Result Regist(string account, string password, string code);

        /// <summary>
        /// 获取手机验证码
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        /// <remarks> 2013-7-5 杨浩 创建</remarks>
        [WebInvoke(Method = "GET",ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        [CustomOperationBehavior(false)]
        Result<string> GetSecurityCode(string account);

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="account">账户</param>
        /// <param name="oldPassword">旧密码</param>
        /// <param name="newPassword">新密码</param>
        /// <param name="code">验证码</param>
        /// <returns></returns>
        /// <remarks> 2013-7-5 杨浩 创建</remarks>
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        [CustomOperationBehavior(false)]
        Result ChangePassword(string account,string oldPassword, string newPassword, string code);

        /// <summary>
        /// 获取注册协议
        /// </summary>
        /// <returns></returns>
        /// <remarks> 2013-9-24 杨浩 创建</remarks>
       [WebGet(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
       [CustomOperationBehavior(false)]
       Result<string> GetRegistAgreement();

       #region 忘记密码

        /// <summary>
        /// 验证码是否正确
        /// </summary>
        /// <returns></returns>
        /// <remarks> 2013-9-24 杨浩 创建</remarks>
        [WebGet(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        [CustomOperationBehavior(false)]
        Result<bool> IsVerifyCodeEnabled(string account, string code);

       /// <summary>
       /// 账号是否存在
       /// </summary>
       /// <returns></returns>
       /// <remarks> 2013-9-24 杨浩 创建</remarks>
       [WebGet(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
       [CustomOperationBehavior(false)]
       Result<bool> IsExist(string account);

       /// <summary>
       /// 获取昵称
       /// </summary>
       /// <returns></returns>
       /// <remarks> 2013-9-24 杨浩 创建</remarks>
       [WebGet(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
       [CustomOperationBehavior(false)]
       Result<string> GetNickName(string account);

       /// <summary>
       /// 重置密码
       /// </summary>
       /// <returns></returns>
       /// <remarks> 2013-9-24 杨浩 创建</remarks>
       [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
       [CustomOperationBehavior(false)]
       Result ResetPassword(string account, string newPassword, string code);

       #endregion
    }
}
