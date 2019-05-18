using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Extra.SMS.Xml;
using System.Collections;
using System.Security.Cryptography;
using Newtonsoft.Json.Linq;

namespace Extra.SMS.Alidayu
{
    /// <summary>
    /// 阿里大鱼
    /// </summary>
    /// <remarks>2016-4-12 陈海裕 添加</remarks>
    internal class SmsProvider : ISmsProvider
    {
        private static string AppKey = "23343797";
        private static string AppSecret = "28d5949de7cff9c34be4cd90ab888ee7";

        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="mobile">手机号(13811290000;15210950000)</param>
        /// <param name="msg">消息</param>
        /// <param name="sendTime">定时</param>
        /// <returns>执行结果</returns>
        public SmsResult Send(string mobile, string msg, DateTime? sendTime)
        {
            var smsResult = new SmsResult();
            return smsResult;
        }

        public SmsResult Send(string mobile, string msg, DateTime? sendTime,string smsTemplateCode)
        {
            var smsResult = new SmsResult();

            string smsModelName = "";
            string smsModelId = "";
            string smsParams = "";

            switch (smsModelName)
            {
                case "用户注册验证码":
                    smsModelId = "SMS_7456196"; // 用户注册验证码
                    smsParams = "{\"code\":\"code\",\"product\":\"product\"}"; // 验证码${code}，您正在注册成为${product}用户，感谢您的支持！
                    break;
                case "身份验证验证码":
                    smsModelId = "SMS_7456200"; // 身份验证验证码
                    smsParams = "{\"code\":\"code\",\"product\":\"product\"}"; // 验证码${code}，您正在进行${product}身份验证，打死不要告诉别人哦！
                    break;
                case "修改密码验证码":
                    smsModelId = "SMS_7456194"; // 修改密码验证码
                    smsParams = "{\"code\":\"code\",\"product\":\"product\"}"; // 验证码${code}，您正在尝试修改${product}登录密码，请妥善保管账户信息。
                    break;
            }

            if (smsModelId == "" || smsParams=="")
            {
                smsResult.Status = SmsResultStatus.Failue;
                smsResult.RowCount = 0;
                return smsResult;
            }

            //alibaba.aliqin.fc.sms.num.send
            //http://gw.api.taobao.com/router/rest
            string method = "alibaba.aliqin.fc.sms.num.send";
            string result = "";

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("method", method);
            parameters.Add("app_key", AppKey);
            parameters.Add("timestamp", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            parameters.Add("format", "json");
            parameters.Add("v", "2.0");
            parameters.Add("sign_method", "md5");
            parameters.Add("sms_type", "normal");
            parameters.Add("sms_free_sign_name", "爱勤全球购");
            parameters.Add("sms_param", "{\"customer\":\"chen\"}");
            parameters.Add("rec_num", mobile);
            parameters.Add("sms_template_code", smsTemplateCode);
            parameters.Add("sign", ComputeSignature(parameters));
            string tempStr = "";
            foreach (var i in parameters)
            {
                tempStr += i.Key + "=" + i.Value + "&";
            }
            if (tempStr.Length > 0)
            {
                tempStr = tempStr.Substring(0, tempStr.Length - 1);
            }
            UTF8Encoding encoding = new UTF8Encoding();
            byte[] postData = encoding.GetBytes(tempStr);

            result = PostRequest("http://gw.api.taobao.com/router/rest", postData);

            
            var _result = JObject.Parse(result);
            if (_result.Property(method) != null && _result[method]["result"]["success"].ToString() == "True")
            {
                smsResult.Status = SmsResultStatus.Success;
                smsResult.RowCount = 1;
                return smsResult;
            }
            else
            {
                smsResult.Status = SmsResultStatus.Failue;
                smsResult.RowCount = 0;
                return smsResult;
            }
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
            return GetResult(result);
        }
        /// <summary>
        /// 查询余额
        /// </summary>
        /// <returns></returns>
        public string Balance()
        {
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
                result.Status = SmsResultStatus.Success;
                result.RowCount = 1;
            }
            else
            {
                result.Status = SmsResultStatus.Failue;
                result.RowCount = 0;
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

        private string ComputeSignature(Dictionary<string, string> parameters)
        {
            parameters = parameters.OrderBy(o => o.Key).ToDictionary(k => k.Key, v => v.Value);

            string signStr = "";

            foreach (var item in parameters)
            {
                signStr += item.Key + item.Value;
            }

            Byte[] bytes = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(AppSecret + signStr + AppSecret));

            string ret = "";
            for (int i = 0; i < bytes.Length; i++)
            {
                ret += Convert.ToString(bytes[i], 16).PadLeft(2, '0');
            }

            return ret.PadLeft(32, '0').ToUpper();
        }
    }
}
