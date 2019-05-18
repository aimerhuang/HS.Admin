using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extra.EDM
{
    /// <summary>
    /// 发送邮件
    /// </summary>
    /// <remarks>
    /// 2014-01-13 何方 创建
    /// </remarks>
    public static class EmailSender
    {
        private static EmailSenderInfo notificationSender = null;
        private static EmailSenderInfo advertisementSender = null;

        /// <summary>
        /// 构造函数
        /// </summary>
        static EmailSender()
        {
            notificationSender = new EmailSenderInfo()
            {
                Email = "pisenhyt@163.com",
                Name = "品胜商城"
            };

            advertisementSender = new EmailSenderInfo()
            {
                Email = "huiyuanti@163.com",
                Name = "品胜商城"
            };
        }

        /// <summary>
        /// 获取不懂发送sender
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        /// <remarks>
        /// 2014-01-13 何方 创建
        /// </remarks>
        public static EmailSenderInfo GetSenderByType(EmailType type)
        {
            if (type == EmailType.Notification) return notificationSender;
            if (type == EmailType.Advertisement) return advertisementSender;

            return null;
        }
    }

    /// <summary>
    /// 发件人
    /// </summary>
    /// <remarks>
    /// 2014-01-21 何方 创建
    /// </remarks>
    public class EmailSenderInfo
    {
        /// <summary>
        /// 发件人邮箱
        /// </summary>
        /// <value>
        /// 发件人邮箱
        /// </value>
        public string Email { get; set; }
        /// <summary>
        /// 发件人显示名称
        /// </summary>
        /// <value>
        /// 发件人显示名称
        /// </value>
        public string Name { get; set; }
    }
}
