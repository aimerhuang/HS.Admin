using System.ServiceModel.Activation;
using System.Web;
using Hyt.Model.B2CApp;
using Hyt.Model.Transfer;
using Hyt.Service.Contract.Base;
using Hyt.Infrastructure.Memory;

namespace Hyt.Service.Implement
{
    /// <summary>
    /// 服务实现的基类
    /// </summary>
    /// <remarks> 2013-7-16 杨浩 创建 </remarks>
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class BaseService
    {
        private static Model.AttachmentConfig _config = BLL.Config.Config.Instance.GetAttachmentConfig();
        /// <summary>
        /// ftp地址
        /// </summary>
        protected readonly static string FtpImageServer = _config.FtpImageServer;
        /// <summary>
        /// ftp帐号
        /// </summary>
        protected readonly static string FtpUserName = _config.FtpUserName;
        /// <summary>
        /// ftp密码
        /// </summary>
        protected readonly static string FtpPassword = _config.FtpPassword;
        /// <summary>
        /// ftp图片服务器地址
        /// </summary>
        protected readonly static string FileServer = _config.FileServer;

        private AppEnum.NetworkType _networkType = AppEnum.NetworkType.Wifi;

        /// <summary>
        /// 当前网络类型
        /// </summary>
        /// <remarks> 2013-7-16 杨浩 创建 </remarks>
        protected AppEnum.NetworkType NetworkType 
        {
            get { return _networkType; }
            set { _networkType = value; }
        }

        /// <summary>
        /// 手机串号
        /// </summary>
        /// <returns>手机串号</returns>
        /// <remarks> 2013-7-16 杨浩 创建 </remarks>
        protected string IMEI
        {
            get 
            {
                string IMEI = HttpContext.Current.Request.Params["IMEI"] ?? HttpContext.Current.Request.Headers["IMEI"];
                
                return IMEI; 
            }
            
        }

        /// <summary>
        /// 当前用户
        /// </summary>
        /// <returns>用户</returns>
        /// <remarks> 2013-7-16 杨浩 创建 </remarks>
        protected CBCrCustomer CurrentUser
        {
            get
            {
                var customer = new Model.Transfer.CBCrCustomer
                {
                    SysNo=default(int),
                    LevelName = "初级",
                    LevelSysNo = Model.SystemPredefined.CustomerLevel.初级
                };

                if (IsLogin)
                {
                    customer = MemoryProvider.Default.Get(ContractToken.Account, () => BLL.CRM.CrCustomerBo.Instance.GetCrCustomer(ContractToken.Account));
                }
                return customer;
            }
        }

        /// <summary>
        /// 当前请求用户是否登录
        /// </summary> 
        /// <returns>bool</returns>
        /// <remarks> 2013-7-16 杨浩 创建 </remarks>
        protected bool IsLogin
        {
            get { return !string.IsNullOrEmpty(ContractToken.Account); }
        }
    }
}