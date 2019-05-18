using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Extra.SMS.Xml;

namespace Extra.SMS.Mai
{
    /// <summary>
    /// 麦讯通//返回值										
    /// 说明
    /// <RetCode>Sucess or Faild</RetCode>提交成功或失败
    /// <JobID>int</JobID>   任务号，失败为0
    /// <OKPhoneCounts>int</OKPhoneCounts> 成功提交手机号码数量
    /// <StockReduced>int</StockReduced> 本次任务短信用量
    /// <ErrPhones>string</ErrPhones>  错误格式号码
    /// </summary>
    /// <remarks>2015-9-18 杨浩 添加</remarks>
    internal class MaiProvider : ISmsProvider
    {
        private static string sn = "admin";
        private static string pwd = "969396";
        private static string userid = "969396";

        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="mobile">手机号(13811290000;15210950000)</param>
        /// <param name="msg">消息</param>
        /// <param name="sendTime">定时</param>
        /// <returns>执行结果</returns>
        public SmsResult Send(string mobile, string msg, DateTime? sendTime)
        {
            string result = "";
            string content = msg+"【商城】";
            StringBuilder sbTemp = new StringBuilder();
            sbTemp.Append(string.Format("UserID={0}&account={1}&password={2}&Phones={3}&content=" + content + "&SendTime=&SendType=1&PostFixNumber=", userid,sn,pwd,mobile));
            byte[] bTemp = System.Text.Encoding.GetEncoding("UTF-8").GetBytes(sbTemp.ToString());
            result = PostRequest("http://www.mxtong.net.cn/GateWay/Services.asmx/DirectSend?", bTemp);
            return GetResult(result);
        }

        public SmsResult Send(string mobile, string msg, DateTime? sendTime, string smsTemplateCode)
        {
            return new SmsResult
            {
                RowCount = 0,
                Status = SmsResultStatus.Failue
            };
        }

        /// <summary>
        /// 各分销商发送短信
        /// </summary>
        /// <param name="mobile">手机号(13811290000;15210950000)</param>
        /// <param name="dealername">分销商</param>
        /// <param name="msg">消息</param>
        /// <param name="sendTime">定时</param>
        /// 王耀发 2016-1-18 创建
        /// <returns>执行结果</returns>
        public SmsResult DealerSend(string mobile, string dealername, string msg, DateTime? sendTime)
        {
            string result = "";
            string content = msg + "【" + dealername + "】";
            StringBuilder sbTemp = new StringBuilder();
            sbTemp.Append(string.Format("UserID={0}&account={1}&password={2}&Phones={3}&content=" + content + "&SendTime=&SendType=1&PostFixNumber=", userid, sn, pwd, mobile));
            byte[] bTemp = System.Text.Encoding.GetEncoding("UTF-8").GetBytes(sbTemp.ToString());
            result = PostRequest("http://www.mxtong.net.cn/GateWay/Services.asmx/DirectSend?", bTemp);
            return GetResult(result);
        }

        public SmsResult BatchSend(System.Data.DataTable table, string msg)
        {
            throw new NotImplementedException();
        }

        public SmsResult BatchSend(System.Data.DataTable table)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 查询余额
        /// </summary>
        /// <returns></returns>
        public string Balance()
        {
            string apiParameter = string.Format("UserID={0}&account={1}&password={2}", userid, sn, pwd);
            byte[] bTemp = System.Text.Encoding.GetEncoding("UTF-8").GetBytes(apiParameter);
            string result = PostRequest("http://www.mxtong.net.cn/Services.asmx/DirectGetStockDetails?", bTemp);
            System.Xml.XmlDocument xml = new System.Xml.XmlDocument();
            xml.LoadXml(result);
            string retCode=xml["ROOT"]["RetCode"].InnerText;

            if (retCode.ToLower() == "faild")
                return xml["ROOT"]["Message"].InnerText;           
            return xml["ROOT"]["StockRemain"].InnerText;
                 
        }

        /// <summary>
        /// 将短信接口状态转换为系统状态
        /// </summary>
        /// <param name="str">短信接口状态</param>
        /// <returns>系统状态</returns>
        private SmsResult GetResult(string str)
        {
            SmsResult result = new SmsResult();
            System.Xml.XmlDocument xml = new System.Xml.XmlDocument();
            xml.LoadXml(str);
            string retCode=xml["ROOT"]["RetCode"].InnerText;

            if (retCode.ToLower() == "faild")
            {
                result.Status = SmsResultStatus.Failue;
                result.RowCount = 0;
            }
            else
            {
                result.Status = SmsResultStatus.Success;
                result.RowCount = int.Parse(xml["ROOT"]["OKPhoneCounts"].InnerText);
            }

            return result;
        }
        //POST方式发送得结果
        private static String PostRequest(string url, byte[] bData)
        {
            System.Net.HttpWebRequest hwRequest;
            System.Net.HttpWebResponse hwResponse;

            string strResult = string.Empty;
            try
            {
                hwRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(url);
                hwRequest.Timeout = 5000;
                hwRequest.Method = "POST";
                hwRequest.ContentType = "application/x-www-form-urlencoded";
                hwRequest.ContentLength = bData.Length;

                System.IO.Stream smWrite = hwRequest.GetRequestStream();
                smWrite.Write(bData, 0, bData.Length);
                smWrite.Close();
            }
            catch (System.Exception err)
            {

                return strResult;
            }

            //get response
            try
            {
                hwResponse = (HttpWebResponse)hwRequest.GetResponse();
                StreamReader srReader = new StreamReader(hwResponse.GetResponseStream(), Encoding.ASCII);
                strResult = srReader.ReadToEnd();
                srReader.Close();
                hwResponse.Close();
            }
            catch (System.Exception err)
            {

            }

            return strResult;
        }
    }
}
