using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using Extra.EDM;
using Hyt.BLL.Log;
using Hyt.Model;
using Hyt.Model.WorkflowStatus;

namespace Hyt.BLL.Extras
{
    /// <summary>
    /// 业务邮件对象
    /// </summary>
    /// <remarks>2013-09-24 陶辉 创建</remarks>
    public class EmailBo:BOBase<EmailBo>
    {
        /// <summary>
        /// 发送一条邮件
        /// </summary>
        /// <param name="receive">收件人</param>
        /// <param name="subject">邮件主题</param>
        /// <param name="mailbody">邮件内容</param>
        /// <param name="type">类型</param>
        /// <returns>发送结果</returns>
        /// <remarks>2013-09-24 陶辉 创建</remarks>
        public EdmResult SendMail(string receive, string subject, string mailbody,EmailType type=EmailType.Notification)
        {
            var result = new EdmResult();
            if (!string.IsNullOrEmpty(receive) && !string.IsNullOrEmpty(subject) && !string.IsNullOrEmpty(mailbody))
            {
                result = EdmProviderFactory.CreateProvider().Send(receive, subject, mailbody, type);
                SaveToDb(receive, subject, mailbody,type, result);
            }
            return result;
        }

        /// <summary>
        /// 把邮件记录到数据库
        /// </summary>
        /// <param name="receive">收件人</param>
        /// <param name="subject">邮件主题</param>
        /// <param name="mailbody">邮件内容</param>
        /// <param name="type">类型</param>
        /// <param name="result">发送结果</param>
        /// <remarks>2013-09-24 陶辉 创建</remarks>
        private void SaveToDb(string receive, string subject, string mailbody,EmailType type, EdmResult result)
        {
            var model = new NcEmail()
            {
                CreatedBy = 0,
                CreatedDate = DateTime.Now,
                ErrorQuantity = result.Status == EdmResultStatus.Success ? 0 : 1,
                HandleTime = DateTime.Now,
                MailAddress = receive,
                MailSubject = subject,
                MailBody = mailbody,
                MailType = (int)type,
                Status = result.Status == EdmResultStatus.Success ? (int)Hyt.Model.WorkflowStatus.NotificationStatus.邮件发送状态.已发 : (int)Hyt.Model.WorkflowStatus.NotificationStatus.邮件发送状态.待发
            };

            Hyt.DataAccess.Notification.INcEmailDao.Instance.Create(model);
        }

        /// <summary>
        /// 发送门店自提确认备货邮件
        /// </summary>
        /// <param name="mail">接收邮件</param>
        /// <param name="customerID">客户ID</param>
        /// <param name="orderID">订单编号</param>
        /// <param name="address">自提地址</param>
        /// <param name="cellPhone">自提点联系人手机号码</param>
        /// <returns>发送结果</returns>
        /// <remarks>2013-9-24 陶辉 创建</remarks>
        public EdmResult 发送门店自提确认备货邮件(string mail, string customerID, string orderID, string address, string cellPhone)
        {

            #region subject templet

            //mail标题
            string mailsubject = customerID + "您好，您在商城的订单#" + orderID + "已经备货完成";

            //邮件模板
            string fileName = HttpContext.Current.Server.MapPath("\\MailTemplates\\MailSOTakeOneselfTemplet.htm");

            //如果没有填写email，不进行处理
            if (string.IsNullOrEmpty(mail))
                return new EdmResult() { Status = EdmResultStatus.Failue };

            FileStream aFile;
            try
            {
                aFile = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            }
            catch
            {
                return new EdmResult() { Status = EdmResultStatus.Failue };
            }

            StreamReader sr = new StreamReader(aFile, Encoding.Default);

            string templet = sr.ReadToEnd();
            sr.Close();

            #endregion

            #region body templet

            string mailBody = GetString(templet, "_begin", "_end");
            mailBody = Rep(mailBody, "_CustomerID", customerID);
            mailBody = Rep(mailBody, "_OrderID", orderID);
            mailBody = Rep(mailBody, "_Address", address);
            mailBody = Rep(mailBody, "_CellPhone", cellPhone);

            #endregion body templet

            return SendMail(mail, mailsubject, mailBody,EmailType.Notification);
        }

        /// <summary>
        /// 发送退换货单通过审核邮件
        /// </summary>
        /// <param name="mail">接收邮件</param>
        /// <param name="customerID">客户ID</param>
        /// <param name="rmaID">退换货单号</param>
        /// <param name="rmaDate">退货申请日期</param>
        /// <param name="address">退货地址</param>
        /// <param name="userName">退货联系人</param>
        /// <param name="cellPhone">退货联系方式</param>
        /// <returns>发送结果</returns>
        /// <remarks>2013-9-24 陶辉 创建</remarks>
        public EdmResult 发送退换货单通过审核邮件(string mail, string customerID, string rmaID, DateTime rmaDate, string address, string userName, string cellPhone)
        {

            #region subject templet

            //mail标题
            string mailsubject = customerID + "您好，您在品胜商城的退货单 RMA#" + rmaID + " -- 已经审核通过";

            //邮件模板
            string fileName = HttpContext.Current.Server.MapPath("\\MailTemplates\\MailRMAAuditedTemplet.htm");

            //如果没有填写email，不进行处理
            if (string.IsNullOrEmpty(mail))
                return new EdmResult() { Status = EdmResultStatus.Failue };

            FileStream aFile;
            try
            {
                aFile = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            }
            catch
            {
                return new EdmResult() { Status = EdmResultStatus.Failue };
            }

            StreamReader sr = new StreamReader(aFile, Encoding.Default);

            string templet = sr.ReadToEnd();
            sr.Close();

            #endregion

            #region body templet

            string mailBody = GetString(templet, "_begin", "_end");
            mailBody = Rep(mailBody, "_CustomerID", customerID);
            mailBody = Rep(mailBody, "_RmaDate", rmaDate.ToString());
            mailBody = Rep(mailBody, "_Address", address);
            mailBody = Rep(mailBody, "_CellPhone", cellPhone);

            #endregion body templet

            return SendMail(mail, mailsubject, mailBody,EmailType.Notification);
        }

        /// <summary>
        /// 发送退换货单未通过审核邮件
        /// </summary>
        /// <param name="mail">接收邮件</param>
        /// <param name="customerID">客户ID</param>
        /// <param name="rmaID">退换货单号</param>
        /// <param name="rmaDate">退换货申请日期</param>
        /// <returns>发送结果</returns>
        /// <remarks>2013-9-24 陶辉 创建</remarks>
        public EdmResult 发送退换货单未通过审核邮件(string mail, string customerID, string rmaID, DateTime rmaDate)
        {
            #region subject templet

            //mail标题
            string mailsubject = customerID + "您好，您在品胜商城的退货单 RMA#"+rmaID+" -- 审核不通过";

            //邮件模板
            string fileName = HttpContext.Current.Server.MapPath("\\MailTemplates\\MailRMAAbandonTemplet.htm");

            //如果没有填写email，不进行处理
            if (string.IsNullOrEmpty(mail))
                return new EdmResult() { Status = EdmResultStatus.Failue };

            FileStream aFile;
            try
            {
                aFile = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            }
            catch
            {
                return new EdmResult() { Status = EdmResultStatus.Failue };
            }

            StreamReader sr = new StreamReader(aFile, Encoding.Default);

            string templet = sr.ReadToEnd();
            sr.Close();

            #endregion

            #region body templet

            string mailBody = GetString(templet, "_begin", "_end");
            mailBody = Rep(mailBody, "_CustomerID", customerID);
            mailBody = Rep(mailBody, "_RmaDate", rmaDate.ToString());

            #endregion body templet

            return SendMail(mail, mailsubject, mailBody,EmailType.Notification);
        }

        /// <summary>
        /// 发送退款完成邮件 
        /// </summary>
        /// <param name="mail">接收邮件</param>
        /// <param name="customerID">客户ID</param>
        /// <param name="rmaID">退货单号</param>
        /// <returns>发送结果</returns>
        /// <remarks>2013-9-24 陶辉 创建</remarks>
        public EdmResult 发送退款完成邮件(string mail, string customerID, string rmaID)
        {

            #region subject templet

            //mail标题
            string mailsubject = customerID + "您好，您在品胜商城的退货单 RMA#"+rmaID+" -- 退货单已退款完成";

            //邮件模板
            string fileName = HttpContext.Current.Server.MapPath("\\MailTemplates\\MailRMARefundTemplet.htm");

            //如果没有填写email，不进行处理
            if (string.IsNullOrEmpty(mail))
                return new EdmResult() { Status = EdmResultStatus.Failue };

            FileStream aFile;
            try
            {
                aFile = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            }
            catch
            {
                return new EdmResult() { Status = EdmResultStatus.Failue };
            }

            StreamReader sr = new StreamReader(aFile, Encoding.Default);

            string templet = sr.ReadToEnd();
            sr.Close();

            #endregion

            #region body templet

            string mailBody = GetString(templet, "_begin", "_end");
            mailBody = Rep(mailBody, "_CustomerID", customerID);

            #endregion body templet

            return SendMail(mail, mailsubject, mailBody,EmailType.Notification);
        
        }

        /// <summary>
        /// 发送升舱成功邮件
        /// </summary>
        /// <param name="mail">接收邮件</param>
        /// <param name="customerID">客户ID</param>
        /// <param name="mallName">第三方商城名称</param>
        /// <param name="mallOrderID">第三方商城订单号</param>
        /// <returns>发送结果</returns>
        /// <remarks>2013-9-24 陶辉 创建 </remarks>
        public EdmResult 发送升舱成功邮件(string mail, string customerID, string mallName, string mallOrderID)
        {
            #region subject templet

            //mail标题
            string mailsubject = customerID + "您好，您在"+mallName+"的订单#"+mallOrderID+" -- 已经成功升舱为商城配送";

            //邮件模板
            string fileName = HttpContext.Current.Server.MapPath("\\MailTemplates\\MailUpGradeSucceedTemplet.htm");

            //如果没有填写email，不进行处理
            if (string.IsNullOrEmpty(mail))
                return new EdmResult() { Status = EdmResultStatus.Failue };

            FileStream aFile;
            try
            {
                aFile = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            }
            catch
            {
                return new EdmResult() { Status = EdmResultStatus.Failue };
            }

            StreamReader sr = new StreamReader(aFile, Encoding.Default);

            string templet = sr.ReadToEnd();
            sr.Close();

            #endregion

            #region body templet

            string mailBody = GetString(templet, "_begin", "_end");
            mailBody = Rep(mailBody, "_CustomerID", customerID);
            mailBody = Rep(mailBody, "_MallName", mallName);
            mailBody = Rep(mailBody, "_MallOrderID", mallOrderID);

            #endregion body templet

            return SendMail(mail, mailsubject, mailBody,EmailType.Notification);
        }

        /// <summary>
        /// 发送第三方物流发货邮件
        /// </summary>
        /// <param name="mail">接收邮件</param>
        /// <param name="customerID">客户ID</param>
        /// <param name="orderID">订单号</param>
        /// <param name="shipTypeName">配送方式</param>
        /// <param name="wayBillNo">物流单号</param>
        /// <returns>发送结果</returns>
        /// <remarks>2013-9-24 陶辉 创建 </remarks>
        public EdmResult 发送第三方物流发货邮件(string mail, string customerID, string orderID, string shipTypeName, string wayBillNo)
        {
            #region subject templet

            //mail标题
            string mailsubject = customerID + "您好，您在商城的订单#"+orderID+" -- 已经发货，请您注意查收";

            //邮件模板
            string fileName = HttpContext.Current.Server.MapPath("\\MailTemplates\\MailSODeliveryTemplet.htm");

            //如果没有填写email，不进行处理
            if (string.IsNullOrEmpty(mail))
                return new EdmResult() { Status = EdmResultStatus.Failue };

            FileStream aFile;
            try
            {
                aFile = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            }
            catch
            {
                return new EdmResult() { Status = EdmResultStatus.Failue };
            }

            StreamReader sr = new StreamReader(aFile, Encoding.Default);

            string templet = sr.ReadToEnd();
            sr.Close();

            #endregion

            #region body templet

            string mailBody = GetString(templet, "_begin", "_end");
            mailBody = Rep(mailBody, "_CustomerID", customerID);
            mailBody = Rep(mailBody, "_OrderID", orderID);
            mailBody = Rep(mailBody, "_ShipTypeName", shipTypeName);
            mailBody = Rep(mailBody, "_WayBillNo", wayBillNo);

            #endregion body templet

            return SendMail(mail, mailsubject, mailBody,EmailType.Notification);
        }

        /// <summary>
        /// 发送百城当日达发货邮件
        /// </summary>
        /// <param name="mail">接收邮件</param>
        /// <param name="customerID">客户ID</param>
        /// <param name="orderID">订单号</param>
        /// <param name="deliveryMan">配送员</param>
        /// <param name="cellPhone">配送员联系方式</param>
        /// <returns>发送结果</returns>
        /// <remarks>2013-9-24 陶辉 创建 </remarks>
        public EdmResult 发送百城当日达发货邮件(string mail, string customerID, string orderID, string deliveryMan, string cellPhone)
        {
            #region subject templet

            //mail标题
            string mailsubject = customerID + "您好，您在商城的订单#" + orderID + " -- 已经已由百城当日达发货，请您注意查收";

            //邮件模板
            string fileName = HttpContext.Current.Server.MapPath("\\MailTemplates\\MailSODeliveryOneselfTemplet.htm");

            //如果没有填写email，不进行处理
            if (string.IsNullOrEmpty(mail))
                return new EdmResult() { Status = EdmResultStatus.Failue };

            FileStream aFile;
            try
            {
                aFile = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            }
            catch
            {
                return new EdmResult() { Status = EdmResultStatus.Failue };
            }

            StreamReader sr = new StreamReader(aFile, Encoding.Default);

            string templet = sr.ReadToEnd();
            sr.Close();

            #endregion

            #region body templet

            string mailBody = GetString(templet, "_begin", "_end");
            mailBody = Rep(mailBody, "_CustomerID", customerID);
            mailBody = Rep(mailBody, "_OrderID", orderID);
            mailBody = Rep(mailBody, "_DeliveryMan", deliveryMan);
            mailBody = Rep(mailBody, "_CellPhone", cellPhone);

            #endregion body templet

            return SendMail(mail, mailsubject, mailBody,EmailType.Notification);
        }

        /// <summary>
        /// 发送换货完成邮件
        /// </summary>
        /// <param name="mail">接收邮件</param>
        /// <param name="customerID">客户ID</param>
        /// <param name="rmaID">换货单号</param>
        /// <param name="orderID">换货销售单号</param>
        /// <param name="ramDate">换货申请日期</param>
        /// <returns>发送结果</returns>
        /// <remarks>2013-9-24 陶辉 创建 </remarks>
        public EdmResult 发送换货完成邮件(string mail, string customerID, string rmaID, string orderID, string ramDate)
        {
            #region subject templet

            //mail标题
            string mailsubject = customerID + "您好，您在商城的退货单 RMA#"+rmaID+" -- 换货已处理完成";

            //邮件模板
            string fileName = Hyt.Util.WebUtil.GetMapPath("/MailTemplates/MailRMAExChangeTemplet.htm");
            //如果没有填写email，不进行处理
            if (string.IsNullOrEmpty(mail))
                return new EdmResult() { Status = EdmResultStatus.Failue };

            FileStream aFile;
            try
            {
                aFile = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            }
            catch
            {
                return new EdmResult() { Status = EdmResultStatus.Failue };
            }

            StreamReader sr = new StreamReader(aFile, Encoding.Default);

            string templet = sr.ReadToEnd();
            sr.Close();

            #endregion

            #region body templet

            string mailBody = GetString(templet, "_begin", "_end");
            mailBody = Rep(mailBody, "_CustomerID", customerID);
            mailBody = Rep(mailBody, "_OrderID", orderID);
            mailBody = Rep(mailBody, "_RamDate", ramDate.ToString());

            #endregion body templet

            return SendMail(mail, mailsubject, mailBody,EmailType.Notification);
        }

        /// <summary>
        /// 发送到货通知邮件
        /// </summary>
        /// <param name="mail">接收邮件</param>
        /// <param name="customerID">客户ID</param>
        /// <param name="product">带链接的商品名称</param>
        /// <returns>发送结果</returns>
        /// <remarks>2013-9-24 陶辉 创建 </remarks>
        public EdmResult 发送到货通知邮件(string mail, string customerID, string product)
        {
            #region subject templet

            //mail标题
            string mailsubject = customerID + "您好，您在商城关注的商品已经到货啦";

            //邮件模板
            string fileName = HttpContext.Current.Server.MapPath("\\MailTemplates\\MailArriveNoticeTemplet.htm");

            //如果没有填写email，不进行处理
            if (string.IsNullOrEmpty(mail))
                return new EdmResult() { Status = EdmResultStatus.Failue };

            FileStream aFile;
            try
            {
                aFile = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            }
            catch
            {
                return new EdmResult() { Status = EdmResultStatus.Failue };
            }

            StreamReader sr = new StreamReader(aFile, Encoding.Default);

            string templet = sr.ReadToEnd();
            sr.Close();

            #endregion

            #region body templet

            string mailBody = GetString(templet, "_begin", "_end");
            mailBody = Rep(mailBody, "_CustomerID", customerID);
            mailBody = Rep(mailBody, "_ProductName", product);

            #endregion body templet

            return SendMail(mail, mailsubject, mailBody,EmailType.Notification);
        }

        /// <summary>
        /// 发送降价通知邮件
        /// </summary>
        /// <param name="mail">接收邮件</param>
        /// <param name="customerID">客户ID</param>
        /// <param name="product">带链接的商品名称</param>
        /// <returns>发送结果</returns>
        /// <remarks>2013-9-24 陶辉 创建 </remarks>
        public EdmResult 发送降价通知邮件(string mail, string customerID, string product)
        {
            #region subject templet

            //mail标题
            string mailsubject = customerID + "您好，您在商城关注的商品已经到货啦";

            //邮件模板
            string fileName = HttpContext.Current.Server.MapPath("\\MailTemplates\\MailDepreciateNoticeTemplet.htm");

            //如果没有填写email，不进行处理
            if (string.IsNullOrEmpty(mail))
                return new EdmResult() { Status = EdmResultStatus.Failue };

            FileStream aFile;
            try
            {
                aFile = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            }
            catch
            {
                return new EdmResult() { Status = EdmResultStatus.Failue };
            }

            StreamReader sr = new StreamReader(aFile, Encoding.Default);

            string templet = sr.ReadToEnd();
            sr.Close();

            #endregion

            #region body templet

            string mailBody = GetString(templet, "_begin", "_end");
            mailBody = Rep(mailBody, "_CustomerID", customerID);
            mailBody = Rep(mailBody, "_ProductName", product);

            #endregion body templet

            return SendMail(mail, mailsubject, mailBody,EmailType.Notification);
        }

        /// <summary>
        /// 替换文本中指定字符为目标字符
        /// </summary>
        /// <param name="org">文本</param>
        /// <param name="source">指定字符</param>
        /// <param name="target">目标字符</param>
        /// <returns>替换结果文本</returns>
        /// <remarks>2013-12-31 陶辉 添加</remarks>
        private string Rep(string org, string source, string target)
        {
            return org.Replace(source, target);
        }

        /// <summary>
        /// 获取指定标签之间的文本
        /// </summary>
        /// <param name="org">文本</param>
        /// <param name="beginTag">起始标签</param>
        /// <param name="endTag">截止标签</param>
        /// <returns>文本内容</returns>
        /// <remarks>2013-12-31 陶辉 添加</remarks>
        private string GetString(string org, string beginTag, string endTag)
        {
            int beginIndex, endIndex;
            string result;
            beginIndex = org.IndexOf(beginTag, 0);
            endIndex = org.IndexOf(endTag, 0);
            if (endIndex <= beginIndex)
            {
                return "";
            }
            else
            {
                result = org.Substring(beginIndex, endIndex - beginIndex);
                result = result.Replace(beginTag, "");
                return result;
            }
        }

    }
}
