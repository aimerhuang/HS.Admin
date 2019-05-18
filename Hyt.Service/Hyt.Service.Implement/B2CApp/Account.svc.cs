using System;
using System.Web;
using Hyt.BLL.Web;
using Hyt.Model;

using Hyt.Model.B2CApp;
using Hyt.Service.Contract.B2CApp;
using Hyt.Service.Contract.Base;
using Hyt.Util;
using Hyt.Util.Validator.Rule;
using CrCustomerBo = Hyt.BLL.CRM.CrCustomerBo;

namespace Hyt.Service.Implement.B2CApp
{
    /// <summary>
    /// 账户服务实现
    /// </summary>
    /// <remarks>
    /// 2013-7-5 杨浩 创建
    /// </remarks>
    public class Account : BaseService, IAccount
    {
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="account">账户(手机号)</param>
        /// <param name="password">密码</param>
        /// <returns>用户信息</returns>
        /// <remarks> 2013-7-5 杨浩 创建</remarks>
        public Result<Customer> Login(string account, string password)
        {
            var temp = BLL.Authentication.CustomerAuthenticationBo.Instance.Login(account, password);
            if (temp.Status == false)
            {
                return new Result<Customer>
                    {
                        Data = null,
                        Message = temp.Message,
                        Status = temp.Status,
                        StatusCode = temp.StatusCode
                    };
            }
            var customer = new Result<Customer>
                {
                    Data = new Customer
                        {
                            Account = temp.Data.Account,
                            SysNo = temp.Data.SysNo,
                            StreetAddress = temp.Data.StreetAddress,
                            NickName = temp.Data.NickName,
                            Name = temp.Data.Name,
                            LevelSysNo = temp.Data.LevelSysNo,
                            EmailAddress = temp.Data.EmailAddress,
                            LevelName = temp.Data.LevelName
                        },
                    Status = true,
                    StatusCode = 1
                };
            customer.Data.HeadImage = BLL.Web.ProductImageBo.Instance.GetHeadImagePath(ProductThumbnailType.CustomerFace, temp.Data.SysNo);
            customer.Data.AppToken = ContractToken.CreateToken(temp.Data.SysNo, account, password);
            Hyt.Infrastructure.Memory.MemoryProvider.Default.Set(account, temp.Data, int.MaxValue);
            return customer;
        }

        /// <summary>
        /// 用户登出
        /// </summary>
        /// <param name="account">账户(手机号)</param>
        /// <param name="token">token</param>
        /// <returns>状态</returns>
        /// <remarks> 2013-7-5 杨浩 创建</remarks>
        public Result LogOut(string account, string token)
        {
            ContractToken.InvalidToken(account, token);
            return new Result
                {
                    Status = true,
                    StatusCode = 1
                };
        }

        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="account">账户(手机号)</param>
        /// <param name="password">密码</param>
        /// <param name="code">安全码</param>
        /// <returns>注册结果</returns>
        /// <remarks> 
        /// 2013-7-5 杨浩 创建
        /// 2013-08-20 郑荣华 实现
        /// 2013-10-29 黄波 修改注册会员的方法
        /// </remarks>
        public Result Regist(string account, string password, string code)
        {
            //验证数据是否合法
            var vResult =
                Util.Validator.VHelper.ValidatorRule(new Rule_Number(account, ""),
                                                         new Rule_Mobile(account, ""), new Rule_StringLenth(account, 11, 11),
                                                         new Rule_NotAllowNull(password, "")
                                                         );
            //数据格式不合法提示
            if (!vResult.IsPass)
                return new Result { Message = "请检查您输入数据是否正确", Status = false, StatusCode = -1 };

            //手机是否存在
            var ssoid = BLL.CRM.CrCustomerBo.Instance.SSOGetCustomerIDByAccount(account);
            if (ssoid > 0)
                return new Result { Message = "手机号码已被注册！", Status = false, StatusCode = -1 };

            //手机验证码判断
            var phoneVerifyCode = BLL.Web.CrCustomerBo.Instance.GetRegisterPhoneVerifyCode(account);
            if (code != phoneVerifyCode)
                return new Result { Message = "手机验证码错误！", Status = false, StatusCode = -1 };

            var source = Model.WorkflowStatus.CustomerStatus.注册来源.商城AndroidApp;
            string agent = HttpContext.Current.Request.UserAgent;
            if (agent != null && (agent.Contains("iPhone") || agent.Contains("iPad") || agent.Contains("iPod")))
            {
                source = Model.WorkflowStatus.CustomerStatus.注册来源.商城IphoneApp;
            }

            int id= CrCustomerBo.Instance.RegisterFrontCustomer(
                account
                , password
                , Model.WorkflowStatus.CustomerStatus.手机状态.已验证
                , Model.WorkflowStatus.CustomerStatus.邮箱状态.已验证
                , source
             );
            try
            {
                //手机客户端注册送等级积分
                var c = Hyt.BLL.Sys.SyConfigBo.Instance.GetModel("App_Register_Integral",Model.WorkflowStatus.SystemStatus.系统配置类型.App商城配置);
                var config = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(c.Value);
                if (config != null)
                {
                    var expire = DateTime.Parse((string)config.Expire);
                    if (DateTime.Now < expire)
                    {
                        Hyt.BLL.LevelPoint.PointBo.Instance.AdjustLevelPoint(id,
                                                                             Hyt.Model.SystemPredefined.User.SystemUser,
                                                                             (int) config.Point,
                                                                             Hyt.Model.SystemPredefined.Constant
                                                                                .App_Register_Integral);
                    }
                }
            }
            catch{}

            return new Result { Message = "注册成功", Status = true, StatusCode = 1 };

        }

        /// <summary>
        /// 获取手机验证码
        /// </summary>
        /// <param name="account"></param>
        /// <returns>手机验证码</returns>
        /// <remarks> 
        /// 2013-7-5 杨浩 创建
        /// 2013-08-20 郑荣华 实现
        /// </remarks>
        public Result<string> GetSecurityCode(string account)
        {
            BLL.Web.CrCustomerBo.Instance.SendRegisterPhoneVerifyCode(account, 90);
            var phoneVerifyCode = BLL.Web.CrCustomerBo.Instance.GetRegisterPhoneVerifyCode(account);

            return new Result<string>
                {
                    Data = phoneVerifyCode,
                    Message = phoneVerifyCode != "" ? "获取验证码成功" : "验证码过期",
                    Status = phoneVerifyCode != "",
                    StatusCode = 1
                };
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="account">账户</param>
        /// <param name="oldPassword">旧密码</param>
        /// <param name="newPassword">新密码</param>
        /// <param name="code">验证码</param>
        /// <returns>修改密码结果</returns>
        /// <remarks> 
        /// 2013-7-5 杨浩 创建
        /// 2013-08-20 郑荣华 实现
        /// </remarks>
        public Result ChangePassword(string account, string oldPassword, string newPassword, string code)
        {
            //新密码数据格式是否合法
            var vResult = Util.Validator.VHelper.ValidatorRule(new Rule_NotAllowNull(newPassword, ""));
            if (!vResult.IsPass)
                return new Result { Message = "新密码数据格式错误", Status = false, StatusCode = -1 };

            //手机验证码判断
            var phoneVerifyCode = BLL.Web.CrCustomerBo.Instance.GetRegisterPhoneVerifyCode(account);
            if (code != phoneVerifyCode)
                return new Result { Message = "手机验证码错误！", Status = false, StatusCode = -1 };


            var customerID = Hyt.BLL.CRM.CrCustomerBo.Instance.SSOGetCustomerIDByAccount(account);
            if(customerID < 0)
                return new Result { Message = "修改密码失败！", Status = false, StatusCode = -1 };
            try
            {
                if (Hyt.BLL.CRM.CrCustomerBo.Instance.UpdateSSOPassword(customerID, oldPassword, newPassword))
                    //取消加密 余勇 2014-09-12
                    return new Result {Message = "修改密码成功", Status = true, StatusCode = 1};
            }
            catch (Exception ex)
            {
                return new Result { Message = ex.Message, Status = false, StatusCode = -1 };
            }
            return new Result { Message = "修改密码失败", Status = false, StatusCode = -1 };
        }

        /// <summary>
        /// 获取注册协议
        /// </summary>
        /// <returns>注册协议</returns>
        /// <remarks> 2013-9-24 杨浩 创建</remarks>
        public Result<string> GetRegistAgreement()
        {
            return new Result<string>
                {
                    Data = @"<h2 style='text-align: center'> 购物平台会员注册协议</h2><p>  欢迎您申请注册成为购物平台会员！在您注册成为购物平台会员前，请您仔细阅读本会员注册协议。</p>",
                    Status = true
                };
        }

        #region 忘记密码

        /// <summary>
        /// 账户是否存在
        /// </summary>
        /// <param name="account"></param>
        /// <returns>bool</returns>
        /// <remarks> 2013-9-24 杨浩 创建</remarks>
        public Result<bool> IsExist(string account)
        {
            var data = CrCustomerBo.Instance.GetCrCustomer(account) != null;
            return new Result<bool>
                {
                    Data = data,
                    Status = true
                };
        }

        /// <summary>
        /// 获取昵称
        /// </summary>
        /// <param name="account"></param>
        /// <returns>昵称</returns>
        /// <remarks> 2013-9-24 杨浩 创建</remarks>
        public Result<string> GetNickName(string account)
        {
            var customer = CrCustomerBo.Instance.GetCrCustomer(account);
            return new Result<string>
                {
                    Data = customer.NickName,
                    Status = true,
                    StatusCode = 1
                };
        }

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="account"></param>
        /// <param name="newPassword"></param>
        /// <param name="code"></param>
        /// <returns>状态</returns>
        /// <remarks> 2013-9-24 杨浩 创建</remarks>
        public Result ResetPassword(string account, string newPassword, string code)
        {
            //手机验证码判断
            var phoneVerifyCode = BLL.Web.CrCustomerBo.Instance.GetRegisterPhoneVerifyCode(account);
            if (code != phoneVerifyCode)
                return new Result { Message = "手机验证码错误！", Status = false, StatusCode = -1 };


            var customerID = Hyt.BLL.CRM.CrCustomerBo.Instance.SSOGetCustomerIDByAccount(account);
            if (customerID < 0)
                return new Result { Message = "重置密码失败！", Status = false, StatusCode = -1 };

            var oldPassword = Hyt.BLL.CRM.CrCustomerBo.Instance.SSOGetOldPasswordByAccount(account);
            try
            {
                if (Hyt.BLL.CRM.CrCustomerBo.Instance.UpdateSSOPassword(customerID, oldPassword, newPassword))  //取消加密 余勇 2014-09-12
                    //取消加密 余勇 2014-09-12
                    return new Result { Message = "重置密码成功", Status = true, StatusCode = 1 };
            }
            catch (Exception ex)
            {
                return new Result { Message = ex.Message, Status = false, StatusCode = -1 };
            }
            return new Result { Message = "重置密码失败", Status = false, StatusCode = -1 };
        }

        /// <summary>
        /// 验证码是否正确
        /// </summary>
        /// <param name="account">账号</param>
        /// <param name="code">验证码</param>
        /// <returns>bool</returns>
        /// <remarks> 2013-9-24 杨浩 创建</remarks>
        public Result<bool> IsVerifyCodeEnabled(string account, string code)
        {
            //手机验证码判断
            var data = false;
            var phoneVerifyCode = BLL.Web.CrCustomerBo.Instance.GetRegisterPhoneVerifyCode(account);
            if (code == phoneVerifyCode)
                data = true;
            return new Result<bool>
                {
                    Message = data ? "手机验证码正确" : "手机验证码错误",
                    Status = data,
                    StatusCode = 1,
                    Data = data
                };
        }

        #endregion
    }
}
