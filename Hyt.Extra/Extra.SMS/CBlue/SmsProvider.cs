using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Extra.SMS.Xml;

namespace Extra.SMS.CBlue
{
    /// <summary>
    /// 创蓝										
    /// 说明
    /// <?xml version="1.0" encoding="utf-8"?>
    /// <SubmitResult xmlns="http://106.ihuyi.cn/">
    ///<code>2</code> 2:提交成功
    ///<msg>??</msg>
    ///<smsid>0</smsid>
    ///</SubmitResult>
    /// </summary>
    /// <remarks>2015-9-18 杨浩 添加</remarks>
    internal class SmsProvider : ISmsProvider
    {
        private static string sn = "jiekou-clcs-08";
        private static string pwd = "Txb654321";

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
            string content =msg;
            StringBuilder sbTemp = new StringBuilder();
            sbTemp.Append(string.Format("account={0}&pswd={1}&mobile={2}&msg=" + content + "&needstatus=true&product=&extno=", sn, pwd, mobile));

            UTF8Encoding encoding = new UTF8Encoding();
            byte[] postData = encoding.GetBytes(sbTemp.ToString());

            result = PostRequest("http://222.73.117.158/msg/HttpBatchSendSM?method=Submit", postData);
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
            string content = msg;// +"【" + dealername + "】";

            string parameter = string.Format("account={0}&pswd={1}&mobile={2}&msg=" + content + "&needstatus=true&product=&extno=", sn, pwd, mobile);

            UTF8Encoding encoding = new UTF8Encoding();
            byte[] postData = encoding.GetBytes(parameter);

            result = PostRequest("http://106.ihuyi.cn/webservice/sms.php?method=Submit", postData);
            return GetResult(result);
        }   
        /// <summary>
        /// 查询余额
        /// </summary>
        /// <returns></returns>
        public string Balance()
        {
            string parameter = string.Format("account={0}&pswd={1}", sn, pwd);
            UTF8Encoding encoding = new UTF8Encoding();
            byte[] postData = encoding.GetBytes(parameter);
            string result = PostRequest("http://222.73.117.158/msg/QueryBalance", postData);//20160126135358,0\n201356329,5953
            var _result = result.Split('\n');

            if (_result.Length>1&&_result[0].Split(',')[1] == "0")
            {
                return _result[1].Split(',')[1];
            }
            return "0";
                 
        }

        /// <summary>
        /// 将短信接口状态转换为系统状态
        /// </summary>
        /// <param name="str">短信接口状态</param>
        /// <returns>系统状态</returns>
        private SmsResult GetResult(string str)
        {
            SmsResult result = new SmsResult();

            var _result = str.Split('\n');

            if (_result.Length > 1 && _result[0].Split(',')[1] == "0")
            {
                result.Status =  SmsResultStatus.Success;
                result.RowCount = 1;
            }
            else
            {
                result.Status = SmsResultStatus.Failue;
                result.RowCount =0;
            }

            return result;
        }
        /// <summary>
        /// POST方式发送得结果
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData"></param>
        /// <returns></returns>
        private static String PostRequest(string url, byte[] postData)
        {
            string strResult = "";
            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(url);
            myRequest.Method = "POST";
            myRequest.ContentType = "application/x-www-form-urlencoded";
            myRequest.ContentLength = postData.Length;

            Stream newStream = myRequest.GetRequestStream();
            newStream.Write(postData, 0, postData.Length);
            newStream.Flush();
            newStream.Close();

            HttpWebResponse myResponse = (HttpWebResponse)myRequest.GetResponse();
            if (myResponse.StatusCode == HttpStatusCode.OK)
            {
                StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);

                strResult = reader.ReadToEnd();
            }
            else
            {
                //访问失败
            }

            return strResult;
        }

        public SmsResult BatchSend(System.Data.DataTable table, string msg)
        {
            throw new NotImplementedException();
        }

        public SmsResult BatchSend(System.Data.DataTable table)
        {
            throw new NotImplementedException();
        }
    }
}
