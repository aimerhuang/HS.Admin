using System;
using System.Web.Security;
using System.Web.Configuration;
using System.Configuration;
using System.Collections.Generic;
using System.Data;
using Extra.EDM.com.focussend.app;

namespace Extra.EDM.FocusSend
{
    /// <summary>
    /// 邮件服务
    /// </summary>
    /// <remarks>2014-1-14 黄波 创建</remarks>
    internal class EdmProvider : IEdmProvider
    {
        /// <summary>
        /// 提示消息
        /// </summary>
        private string result = "未连接上邮件接口服务,请查看网络连接!";

        /// <summary>
        /// 批量发送邮件
        /// </summary>
        /// <param name="table">主题</param>
        /// <param name="subject">主题</param>
        /// <param name="body">内容</param>
        /// <param name="type">邮件类型</param>
        /// <returns>发送结果</returns>
        /// <remarks>2014-1-14 黄波 创建</remarks>
        public EdmResult BatchSend(DataTable table, string subject, string body, EmailType type)
        {
            FocusSendWebService service = new FocusSendWebService();
            FocusUser user = GetFocusUser(type);
            EmailSenderInfo sender = EmailSender.GetSenderByType(type);

            FocusEmail email = new FocusEmail();
            email.Body = body;
            email.IsBodyHtml = true;

            FocusTask task = new FocusTask();
            task.TaskName = "BatchTask:" + DateTime.Now;
            task.SenderEmail = sender.Email;
            task.SenderName = sender.Name;
            task.SendDate = DateTime.Now;
            task.Subject = subject;

            List<FocusReceiver> list = new List<FocusReceiver>();
            FocusReceiver receiver;

            foreach (DataRow dr in table.Rows)
            {
                receiver = new FocusReceiver();
                receiver.Email = dr["Email"].ToString();
                list.Add(receiver);
            }

            try
            {
                result = service.BatchSend(user, email, task, list.ToArray());
            }
            catch { }
            return ConvertResult(result);
        }

        /// <summary>
        /// 批量发送邮件，但发给每一个人的邮件内容都不一样
        /// </summary>
        /// <param name="table">邮件内容</param>
        /// <returns>发送结果</returns>
        /// <remarks>2014-1-14 黄波 创建</remarks>
        public EdmResult BatchSend(DataTable table)
        {
            FocusUser user = GetFocusUser(EmailType.Notification);
            EmailSenderInfo sender = EmailSender.GetSenderByType(EmailType.Notification);

            FocusTask task = new FocusTask();
            task.TaskName = "BatchSendPersonal:" + DateTime.Now;
            task.SenderEmail = sender.Email;
            task.SenderName = sender.Name;
            task.SendDate = DateTime.Now;
            task.Subject = "";

            List<FocusEmail> emailList = new List<FocusEmail>();
            FocusEmail fEmail;

            List<FocusReceiver> receiverList = new List<FocusReceiver>();
            FocusReceiver fReceiver;

            foreach (DataRow dr in table.Rows)
            {
                fEmail = new FocusEmail();
                fReceiver = new FocusReceiver();

                fEmail.Subject = dr["subject"].ToString();
                fEmail.Body = dr["body"].ToString();
                emailList.Add(fEmail);

                fReceiver.Email = dr["email"].ToString();
                receiverList.Add(fReceiver);
            }

            FocusSendWebService service = new FocusSendWebService();

            try
            {
                result = service.BatchSendPersonal(user, task, emailList.ToArray(), receiverList.ToArray());
            }
            catch { }
            return ConvertResult(result);
        }

        /// <summary>
        /// 发送单封邮件
        /// </summary>
        /// <param name="email">邮件地址</param>
        /// <param name="subject">主题</param>
        /// <param name="body">内容</param>
        /// <param name="type">邮件类型</param>
        /// <returns>发送结果</returns>
        /// <remarks>2014-1-14 黄波 创建</remarks>
        public EdmResult Send(string email, string subject, string body, EmailType type)
        {
            FocusSendWebService webService = new FocusSendWebService();
            FocusUser user = GetFocusUser(type);

            FocusEmail focusEmail = new FocusEmail();
            focusEmail.Body = body;
            focusEmail.IsBodyHtml = true;
            FocusReceiver receiver = new FocusReceiver();
            receiver.Email = email;

            try
            {
                result = webService.SendOne(user, focusEmail, subject, receiver);
            }
            catch { }
            return ConvertResult(result);
        }

        /// <summary>
        /// 获取用户账户信息
        /// </summary>
        /// <param name="type">邮件类型</param>
        /// <returns>用户账户信息</returns>
        /// <remarks>2014-1-14 黄波 创建</remarks>
        private FocusUser GetFocusUser(EmailType type)
        {
            FocusUser user = new FocusUser();
            EmailSenderInfo sender = EmailSender.GetSenderByType(type);

            user.Email = sender.Email;
            user.Password = FormsAuthentication.HashPasswordForStoringInConfigFile("hytHYTedm", FormsAuthPasswordFormat.SHA1.ToString("G"));

            return user;
        }

        /// <summary>
        /// 将字符串转换为邮件发送结果
        /// </summary>
        /// <param name="stringResult">结果字符串</param>
        /// <returns>发送结果</returns>
        /// <remarks>2014-1-14 黄波 创建</remarks>
        private EdmResult ConvertResult(string stringResult)
        {
            EdmResult result = new EdmResult();
            switch (stringResult)
            {
                case "success": result.Status = EdmResultStatus.Success; result.Message = "邮件发送成功"; break;
                default: result.Status = EdmResultStatus.Failue; result.Message = stringResult; break;
            }
            return result;
        }
    }
}
