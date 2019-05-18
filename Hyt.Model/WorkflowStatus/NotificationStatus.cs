using System.ComponentModel;

namespace Hyt.Model.WorkflowStatus
{
    /// <summary>
    /// 通知状态
    /// </summary>
    /// <remarks>2013-10-08 吴文强 创建</remarks>
    public class NotificationStatus
    {
        /// <summary>
        /// 短信发送状态
        /// 数据表:NcSms 字段:Status
        /// </summary>
        /// <remarks>2013-10-08 吴文强 创建</remarks>
        public enum 短信发送状态
        {
            待发 = 10,
            已发 = 20,
            作废 = -10,
        }

        /// <summary>
        /// 邮件类型
        /// 数据表:NcEmail 字段:MailType
        /// </summary>
        /// <remarks>2013-10-08 吴文强 创建</remarks>
        public enum 邮件类型
        {
            通知 = 10,
            广告 = 20,
            活动 = 30,
        }

        /// <summary>
        /// 邮件发送状态
        /// 数据表:NcEmail 字段:Status
        /// </summary>
        /// <remarks>2013-10-08 吴文强 创建</remarks>
        public enum 邮件发送状态
        {
            待发 = 10,
            已发 = 20,
            作废 = -10,
        }
    }
}
