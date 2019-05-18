using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Extra.SMS;
using Hyt.BLL.Log;
using Hyt.DataAccess;
using Hyt.Model;
using Hyt.Model.WorkflowStatus;
using Hyt.Util.Validator;
using Hyt.Util.Validator.Rule;

namespace Hyt.BLL.Extras
{
    /// <summary>
    /// 后台管理手机短信业务对象
    /// </summary>
    /// <remarks>
    /// 2014-10-03 余勇 创建
    /// </remarks>
    public class AdminSmsBO : BOBase<AdminSmsBO>
    {
        #region 发送一条短信

        /// <summary>
        /// 发送一条短信
        /// 调用网关发送短信（不保存到数据库）
        /// </summary>
        /// <param name="sysNo">后台管理员编号</param>
        /// <param name="msg">70字（包含签名）一条短信，超出则按此规则分割成多条短信</param>
        /// <param name="sendTime">定时发送(精确到秒)，为空不需要定时</param>
        /// <returns>返回受影响行数</returns>
        /// <remarks>
        /// 2014-10-03 余勇 创建
        /// </remarks>
        public SmsResult Send(int sysNo, string msg, DateTime? sendTime)
        {
            var user = Hyt.BLL.Sys.SyUserBo.Instance.GetSyUser(sysNo);
            var result = new SmsResult { Status = SmsResultStatus.Success };

            if (user != null
                    && !string.IsNullOrEmpty(user.MobilePhoneNumber)
                    && !string.IsNullOrEmpty(msg)
                    && VHelper.ValidatorRule(new Rule_Mobile(user.MobilePhoneNumber)).IsPass
                    && user.MobilePhoneNumber.Length >= 11)
            {
                System.Threading.Tasks.Task.Factory.StartNew(() =>
                {
                    try
                    {
                        result = SmsProviderFactory.CreateProvider().Send(user.MobilePhoneNumber, msg + "【】", sendTime);
                        SysLog.Instance.Info(LogStatus.系统日志来源.后台,
                               msg + "(" + user.MobilePhoneNumber + ")", LogStatus.系统日志目标类型.短信咨询, 0, null, "",
                              0);
                      
                    }
                    catch (Exception ex)
                    {
                        Hyt.BLL.Log.SysLog.Instance.Error(Model.WorkflowStatus.LogStatus.系统日志来源.外部应用, "短信发送异常", ex);
                    }
                });
            }
            else
            {
                return new SmsResult
                {
                    RowCount = 0,
                    Status = SmsResultStatus.Failue
                };
            }
            return result;
        }

        #endregion
    }
}
