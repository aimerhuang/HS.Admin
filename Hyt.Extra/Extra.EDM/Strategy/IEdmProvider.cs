using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Extra.EDM
{
    /// <summary>
    /// Emd接口
    /// </summary>
    /// <remarks>
    /// 2014/1/13 何方 创建
    /// </remarks>
    public interface IEdmProvider
    {
        /// <summary>
        /// 批量发送
        /// </summary>
        /// <param name="table">邮件表
        /// 列名:
        /// Email(string not null)</param>
        /// <param name="subject">邮件主题</param>
        /// <param name="body">邮件正文</param>
        /// <param name="type">邮件类型(EmailType枚举)</param>
        /// <returns>
        /// true：成功,false：失败
        /// </returns>
        /// <remarks>
        /// 2014-01-13 何方 创建
        /// </remarks>
        EdmResult BatchSend(DataTable table, string subject, string body, EmailType type);

        /// <summary>
        /// 批量发送邮件，但发给每一个人的邮件内容都不一样
        /// </summary>
        /// <param name="table">邮件表
        /// 列名:
        /// Email(string not null)
        /// Subject(string not null)
        /// Body(string not null)
        /// EmailType(string null) 值来源于EmailType枚举</param>
        /// <returns>
        /// true：成功,false：失败
        /// </returns>
        /// <remarks>
        /// 2014-01-13 何方 创建
        /// </remarks>
        EdmResult BatchSend(DataTable table);

        /// <summary>
        /// 发送单封邮件
        /// </summary>
        /// <param name="email">邮件地址</param>
        /// <param name="subject">邮件主题</param>
        /// <param name="body">邮件正文</param>
        /// <param name="type">邮件类型(EmailType枚举)</param>
        /// <returns>
        /// true：成功,false：失败
        /// </returns>
        /// <remarks>
        /// 2014-01-13 何方 创建
        /// </remarks>
        EdmResult Send(string email, string subject, string body, EmailType type);
    }
}
