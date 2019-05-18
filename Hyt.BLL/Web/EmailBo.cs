using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Extra.EDM;
using System.Web;
using Hyt.Util;

namespace Hyt.BLL.Web
{
    /// <summary>
    /// 邮件发送
    /// </summary>
    /// <remarks>2013－08-09 苟治国 创建</remarks>
    public class EmailBo : BOBase<EmailBo>
    {
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="receiver">收件人</param>
        /// <param name="subject">主题</param>
        /// <param name="body">邮件内容</param>
        /// <returns>
        /// 发送结果
        /// </returns>
        /// <remarks>
        /// 2013/4/12 何方 创建
        /// </remarks>
        public EdmResult Send(string receiver, string subject, string body)
        {
            return Send(receiver, subject, body, EmailType.Notification);
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="receiver">收件人</param>
        /// <param name="subject">主题</param>
        /// <param name="body">邮件内容</param>
        /// <param name="emailType">邮件类型</param>
        /// <returns>
        /// 发送结果
        /// </returns>
        /// <remarks>
        /// 2013/4/12 何方 创建
        /// </remarks>
        public EdmResult Send(string receiver, string subject, string body, EmailType emailType)
        {
            var sendResult = EdmProviderFactory.CreateProvider().Send(receiver, subject, body, emailType);
            //SaveToDb(receiver, subject, body, emailType, sendResult.Status);
            return sendResult;
        }

        /// <summary>
        /// 使用模板文件发送邮件
        /// </summary>
        /// <param name="receiver">收件人</param>
        /// <param name="subject">主题</param>
        /// <param name="templatePath">模板</param>
        /// <param name="values">邮件内容</param>
        /// <returns>发送结果</returns>
        /// <remarks>
        /// 2013/4/12 何方 创建
        /// </remarks>
        public EdmResult SendByTemplateFile(string receiver, string subject, string templatePath, Dictionary<string, string> values)
        {
            templatePath = HttpContext.Current.Server.MapPath("\\MailTemplates" + "\\" + templatePath);
            string template;
            try
            {
                template = EmailTemplate.GetContent(templatePath, values);
            }
            catch (Exception ex)
            {
                return new EdmResult { Status = 0, Message = "邮件模板失败模板" + ex.Message };
            }
            var sendResult = SendByTemplate(receiver, subject, template, values);
            return sendResult;
        }

        /// <summary>
        /// 使用模板发送邮件
        /// </summary>
        /// <param name="receiver">收件人</param>
        /// <param name="subject">主题</param>
        /// <param name="template">模板</param>
        /// <param name="values">邮件内容</param>
        /// <returns>发送结果</returns>
        /// <remarks>
        /// 2013/4/12 何方 创建
        /// </remarks>
        public EdmResult SendByTemplate(string receiver, string subject, string template, Dictionary<string, string> values)
        {
            var emailbody = "";
            if (values != null)
            {
                foreach (string key in values.Keys)
                {
                    template = template.Replace(key, values[key].ToString());
                }
            }
            emailbody = template;
            var sendResult = Send(receiver, subject, emailbody, EmailType.Notification);
            return sendResult;
        }
    }
}
