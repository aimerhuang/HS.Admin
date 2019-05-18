using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Hyt.Util.Serialization;

namespace Hyt.Util.JPush
{
    /// <summary>
    /// 激光推送封装
    /// </summary>
    /// <remarks>2014-01-17 邵斌 创建</remarks>
    public class JPushHelper
    {
        private const string serverUrl = "http://api.jpush.cn:8800/v2/push";
        private string _masterSecret = "3026d842d3da9a22726e816a";     //验证串
        private string _appkeys = "29e1b05260f2c1b7051afd9e";          //appID
        private int _keepLiveDay = 1;          //信息保存服务器时间 单位：天

        /// <summary>
        /// 是否启用调试状态
        /// </summary>
        /// <remarks>2014-01-17 邵斌 创建</remarks>
        public bool EnabledDebugger { get; set; }

        /// <summary>
        /// 消息停留天数 单位：天
        /// </summary>
        /// <remarks>2014-01-17 邵斌 创建</remarks>
        public int KeepLiveDay
        {
            get { return _keepLiveDay; }
            set { _keepLiveDay = value; }
        }

        /// <summary>
        /// 默认构造函数
        /// </summary>
        /// <returns>创建推送对象</returns>
        /// <remarks>2014-01-17 邵斌 创建</remarks>
        public JPushHelper()
        {
            EnabledDebugger = false;
        }

        /// <summary>
        /// 默认构造函数
        /// </summary>
        /// <param name="masterSecret">加密验证串</param>
        /// <param name="appkeys">App编号</param>
        /// <returns>创建推送对象</returns>
        /// <remarks>2014-01-17 邵斌 创建</remarks>
        public JPushHelper(string masterSecret, string appkeys)
        {
            _masterSecret = masterSecret;
            _appkeys = appkeys;
        }

        /// <summary>
        /// 发送推送请求
        /// </summary>
        /// <param name="url">推送REST API 地址</param>
        /// <param name="param">推送参数内容</param>
        /// <returns>返回回馈结果</returns>
        /// <remarks>2014-01-17 邵斌 创建</remarks>
        private PushResult RequestPost(string url = "", string param = "")
        {
            //检查参数
            if (string.IsNullOrEmpty(url) || string.IsNullOrEmpty(param))
            {

                return new PushResult() { Code = -1, Message = "参数错误" };
            }

            //============
            //  发送过程
            //============
            //创建发送对象和参数设置
            var client = WebRequest.Create(url);
            client.Method = "post";                                     //采用POST方式发送
            client.ContentType = "application/x-www-form-urlencoded";   //内容头信息

            byte[] buffer = null;                                       //发送内容缓存

            //发送参数内容
            if (param != null)
            {
                buffer = Encoding.UTF8.GetBytes(param);             //读出参数
                client.ContentLength = buffer.Length;
                Stream newStream = client.GetRequestStream();
                newStream.Write(buffer, 0, buffer.Length);
                newStream.Flush();
                newStream.Close();
            }
            else
            {
                client.ContentLength = 0;
            }

            //============
            //  接收过程
            //============
            string re = "";
            try
            {
                WebResponse result = client.GetResponse();
                Stream receiveStream = result.GetResponseStream();

                byte[] readBuffer = new byte[512];
                int bytes = receiveStream.Read(readBuffer, 0, 512);

                re = "";

                while (bytes > 0)
                {
                    Encoding encoding = Encoding.GetEncoding("gb2312");
                    re += encoding.GetString(readBuffer, 0, bytes);
                    bytes = receiveStream.Read(readBuffer, 0, 512);
                }
            }

            catch (Exception e)
            {
                re = e.Message;
            }

            return ErrorCode(re);

        }

        /// <summary>
        /// 广播方式推送
        /// </summary>
        /// <param name="request">发送请求对象参数</param>
        /// <returns>返回 true：发送成功 false：发送失败</returns>
        /// <remarks>2014-01-17 邵斌 创建</remarks>
        public PushResult Send(JPushRequest request)
        {
            string platFormString = (request.Platform == PlatformEnum.All) ? "android,ios" : request.Platform.ToString().ToLower();
            var url = "http://api.jpush.cn:8800/sendmsg/v2/sendmsg";
            System.Text.StringBuilder param = new StringBuilder();
            param.Append("&sendno=" + request.SendNo);
            param.Append("&app_key=" + _appkeys);
            param.Append("&receiver_type=" + request.ReceiveType);
            param.Append("&receiver_value=" + request.ReceiverValue);
            param.Append("&verification_code=" + EncodeToMd5(string.Format("{0}{1}{2}{3}", request.SendNo, request.ReceiveType, request.ReceiverValue, _masterSecret)));
            param.Append("&msg_type=" + request.MsgType);
            param.Append("&msg_content=" + request.MsgContent);
            param.Append("&platform=" + platFormString);
            if (EnabledDebugger)
            {
                param.Append("&apns_production=" + 0);
            }
            else
            {
                param.Append("&apns_production=" + 1);
            }

            param.Append("&time_to_live=" + ((request.TimeToLive == 0) ? KeepLiveDay : request.TimeToLive) * 86400);        //推送消息安秒计算，一天秒数：60*60*24=86400
            param.Append("&send_description=" + request.Description);

            return RequestPost(serverUrl, param.ToString());
        }

        /// <summary>
        /// 广播方式推送
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <param name="title">标题</param>
        /// <param name="appType">客户端类型</param>
        /// <param name="content">推送内容</param>
        /// <param name="serviceType">内容参数：服务服务类型</param>
        /// <param name="parameter">参数</param>
        /// <returns>返回 true：发送成功 false：发送失败</returns>
        /// <remarks>2014-01-17 邵斌 创建</remarks>
        public PushResult Send(int sysNo, string title, PlatformEnum appType, string content, int serviceType, string parameter)
        {
            string jsonContent = "";
            if (appType == PlatformEnum.Ios)
            {
                jsonContent = Utility.ConvertObjectToIosContentString(sysNo, title, content, serviceType, parameter);
            }
            else
            {
                jsonContent = Utility.ConvertObjectToContentString(sysNo, title, serviceType, content, parameter);
            }

            //极光推送请求对象
            JPushRequest jRequest = new JPushRequest()
            {
                SendNo = sysNo,
                MsgContent = jsonContent,
                MsgType = 1,
                ReceiveType = 4,
                ReceiverValue = "",
                Description = "推送管理"
            };

            jRequest.Platform = appType; 


            return Send(jRequest);
        }

        /// <summary>
        /// 对单个用户推送消息
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <param name="title">标题</param>
        /// <param name="customerySysNo">接收人系统编号</param>
        /// <param name="appType">客户端类型</param>
        /// <param name="content">推送内容</param>
        /// <param name="serviceType">内容参数：服务服务类型</param>
        /// <param name="parameter">参数</param>
        /// <param name="timeToLive">推送消息在推送服务器保留时间（单位：天）</param>
        /// <param name="description">此处推送描述</param>
        /// <returns>返回 true：发送成功 false：发送失败</returns>
        /// <remarks>2014-01-17 邵斌 创建</remarks>
        public PushResult SendToSingle(int sysNo, string title, int customerySysNo, PlatformEnum appType, string content, int serviceType, string parameter, int timeToLive, string description)
        {
            JPushRequest request = new JPushRequest();
            request.SendNo = sysNo;
            request.ReceiveType = 2;
            request.ReceiverValue = customerySysNo.ToString();
            request.MsgType = 1;
            request.MsgContent = Utility.ConvertObjectToContentString(sysNo, title, serviceType, content, parameter);
            request.Platform = appType;
            request.TimeToLive = timeToLive;
            request.Description = description;

            return Send(request);
        }

        /// <summary>
        /// 对多个用户推送消息
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <param name="title">标题</param>
        /// <param name="customerList">接收人系统编号列表(用户系统编号集合)</param>
        /// <param name="appType">客户端类型</param>
        /// <param name="content">推送内容</param>
        /// <param name="parameter">参数</param>
        /// <param name="timeToLive">推送消息在推送服务器保留时间（单位：天）</param>
        /// <param name="description">此处推送描述</param>
        /// <returns>返回 true：发送成功 false：发送失败</returns>
        /// <remarks>2014-01-17 邵斌 创建</remarks>
        public PushResult SendToGroup(int sysNo, string title, IList<int> customerList, PlatformEnum appType, string content, string parameter, int timeToLive, string description)
        {
            JPushRequest request = new JPushRequest();
            request.SendNo = sysNo;
            request.ReceiveType = 3;
            request.ReceiverValue = customerList.Join(",");
            request.MsgType = 1;
            request.MsgContent = Utility.ConvertObjectToContentString(sysNo, title, 3, content, parameter);

            request.Platform = appType;
            request.TimeToLive = timeToLive;
            request.Description = description;

            return Send(request);
        }

        /// <summary>
        /// md5加密
        /// </summary>
        /// <param name="eString">发送内容</param>
        /// <returns>加密后的字符串</returns>
        /// <remarks>2014-01-17 邵斌 创建</remarks>
        public string EncodeToMd5(string eString)
        {
            byte[] src = Encoding.Convert(Encoding.Default, Encoding.UTF8, Encoding.Default.GetBytes(eString));
            return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(Encoding.UTF8.GetString(src), "md5");
        }

        /// <summary>
        /// 建返回信息转换成返回结果对象
        /// </summary>
        /// <param name="response">返回结果</param>
        /// <returns></returns>
        private PushResult ErrorCode(string response)
        {
            JPushResponse responseObj = response.ToObject<JPushResponse>();

            //返回结果集
            /*
            {"errcode":1007,"errmsg":"IMEI value is not correct"}
            {"sendno":"13","msg_id":"1738689308","errcode":0,"errmsg":"Succeed"}
             */

            /*
            错误码	错误描述
            0	    调用成功
            10	    系统内部错误
            1001	只支持 HTTP Post 方法，不支持 Get 方法
            1002	缺少了必须的参数
            1003	参数值不合法
            1004	verification_code 验证失败
            1005	消息体太大。
            1006	用户名或密码错误
            1007	receiver_value 参数 非法
            1008	appkey参数非法
            1010	msg_content 不合法
            1011	没有满足条件的推送目标
                    如果群发：则此应用还没有一个客户端用户注册。请检查 SDK 集成是否正常。
                    如果是推送给某别名或者标签：则此别名或者标签还没有在任何客户端SDK提交设置成功。
            1012	iOS 不支持推送自定义消息。只有 Android 支持推送自定义消息。
            1013	content-type 只支持 application/x-www-form-urlencoded
             */
            PushResult result = new PushResult();

            if (responseObj.sendno > 0 && responseObj.errcode == 0 && responseObj.errmsg.ToLower() == "succeed")
            {
                result.Code = 0;
                result.Message = "发送成功";
                return result;
            }

            switch (responseObj.errcode)
            {
                case 0:
                    result.Message = "调用成功";
                    break;
                case 10:
                    result.Message = "系统内部错误";
                    break;
                case 1001:
                    result.Message = "只支持 HTTP Post 方法，不支持 Get 方法";
                    break;
                case 1002:
                    result.Message = "缺少了必须的参数";
                    break;
                case 1003:
                    result.Message = "参数值不合法";
                    break;
                case 1004:
                    result.Message = "verification_code 验证失败";
                    break;
                case 1005:
                    result.Message = "消息体太大";
                    break;
                case 1006:
                    result.Message = "用户名或密码错误";
                    break;
                case 1007:
                    result.Message = "receiver_value 参数 非法";
                    break;
                case 1008:
                    result.Message = "appkey参数非法";
                    break;
                case 1010:
                    result.Message = "msg_content 不合法";
                    break;
                case 1011:
                    result.Message = "没有满足条件的推送目标";
                    break;
                case 1012:
                    result.Message = "iOS 不支持推送自定义消息。只有 Android 支持推送自定义消息。";
                    break;
                case 1013:
                    result.Message = "content-type 只支持 application/x-www-form-urlencoded";
                    break;
                default:
                    result.Message = "调用失败";
                    break;
            }

            result.Code = responseObj.errcode;

            return result;

        }
    }
}
