using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Text;
using Hyt.BLL.Union;
using Hyt.DataAccess.Weixin;
using Hyt.Infrastructure.Memory;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using Hyt.Model.Weixin;
using Hyt.Model.WorkflowStatus;
using Newtonsoft.Json.Linq;
using LitJson;

namespace Hyt.BLL.Weixin
{
    /// <summary>
    /// 微信客服消息业务类
    /// </summary>
    /// <remarks>2013-10-31 陶辉 创建</remarks>
    public class CallCenterReplyBo : BOBase<CallCenterReplyBo>
    {
        #region 推送相关 WriteBackMessage
        protected string AppID = "wxd1a96e3c7fa671e3";

        protected string AppSecret = "2d85f1d6acf0c5aeb94cc6f223f50e8a";


        protected string openid_url = "https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type=authorization_code";
        /// <summary>
        /// 获取微信授权信息
        /// </summary>
        /// <returns>授权码</returns>
        /// <remarks>2013-10-31 陶辉 创建</remarks>
        public string GetAccessToken()
        {
            var token = "";
            var config = WeChatBo.Instance.GetWeixinConfig();
            var apiUrl = string.Format("https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}"
                                        , config.AppId, config.AppSecret);

            try
            {
                var req = GetWebRequest(apiUrl, "GET");
                var webResponse = (HttpWebResponse)req.GetResponse();
                var stream = new StreamReader(webResponse.GetResponseStream());
                var jsonStr = stream.ReadToEnd();
                var jsonObject = JObject.Parse(jsonStr);
                if (jsonObject.Property("access_token") != null)
                {
                    token = jsonObject["access_token"].ToString();
                }
                else
                {
                    //TODO:返回错误
                }
            }
            catch
            {
                //TODO:异常
            }
            return token;
        }
        #region js分享
        /// <summary>
        /// 获取 JS-SDK的使用权限签名
        /// </summary>
        /// <returns></returns>
        /// <remarks>2015-9-13 杨浩 创建</remarks>
        public string GetJsTicket()
        {
            string jsTicket = "";
            var config = WeChatBo.Instance.GetWeixinConfig();
            var apiUrl = string.Format("https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token={0}&type={1}"
                                      , AccessToken, "jsapi");
            try
            {
                var req = GetWebRequest(apiUrl, "GET");
                var webResponse = (HttpWebResponse)req.GetResponse();
                var stream = new StreamReader(webResponse.GetResponseStream());
                var jsonStr = stream.ReadToEnd();
                var jsonObject = JObject.Parse(jsonStr);
                if (jsonObject.Property("ticket") != null)
                {
                    jsTicket = jsonObject["ticket"].ToString();
                }
                else
                {
                    //TODO:返回错误
                }
            }
            catch
            {
                //TODO:异常
            }

            return jsTicket;

        }
        /// <summary>
        /// 将时间转换时间截
        /// </summary>
        /// <param name="time">要转换的时间</param>
        /// <remarks>2015-9-13 杨浩 创建</remarks>
        public int ConvertDateTimeInt(DateTime time)
        {
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            return (int)(time - startTime).TotalSeconds;
        }
        /// <summary>
        /// 返回随机数
        /// </summary>
        /// <param name="_length">长度</param>
        /// <remarks>2015-9-13 杨浩 创建</remarks>
        public string GetNonceStr(int _length)
        {
            string[] strs = new string[]
                                 {
                                  "a","b","c","d","e","f","g","h","i","j","k","l","m","n","o","p","q","r","s","t","u","v","w","x","y","z",
                                  "A","B","C","D","E","F","G","H","I","J","K","L","M","N","O","P","Q","R","S","T","U","V","W","X","Y","Z"
                                 };
            int strsLength = strs.Length;
            Random r = new Random();
            string nonceStr = "";
            for (int i = 0; i < _length; i++)
            {
                nonceStr += strs[r.Next(strsLength - 1)];
            }
            return nonceStr;
        }
        /// <summary>
        /// 签名算法,SHA1加密
        /// </summary>
        /// <param name="_jsapiticket">jsapi_ticket</param>
        /// <param name="_nonceStr">随机字符串(必须与wx.config中的nonceStr相同)</param>
        /// <param name="_timestamp">时间戳(必须与wx.config中的timestamp相同)</param>
        /// <param name="_url">当前网页的URL，不包含#及其后面部分(必须是调用JS接口页面的完整URL)</param>
        /// <returns></returns>
        /// <remarks>2015-9-13 杨浩 创建</remarks>
        public string GetSignature(string _jsapiticket, string _nonceStr, int _timestamp, string _url)
        {
            string string1 = string.Format("jsapi_ticket={0}&noncestr={1}&timestamp={2}&url={3}", _jsapiticket, _nonceStr, _timestamp, _url);
            //对字符串进行SHA1加密
            using (System.Security.Cryptography.HashAlgorithm iSHA = new System.Security.Cryptography.SHA1CryptoServiceProvider())
            {
                byte[] StrRes = Encoding.Default.GetBytes(string1);
                StrRes = iSHA.ComputeHash(StrRes);
                StringBuilder EnText = new StringBuilder();
                foreach (byte iByte in StrRes)
                {
                    EnText.AppendFormat("{0:x2}", iByte);
                }
                return EnText.ToString();
            }
        }
        /// <summary>
        /// 获取 JS-SDK的使用权限签名
        /// </summary>
        /// <returns></returns>
        public string JsTicket
        {
            get
            {
                return MemoryProvider.Default.Get<string>(KeyConstant.WeixinJsTicket, 100, GetJsTicket);
            }
        }
        #endregion
        /// <summary>
        /// 获取微信用户基本信息
        /// </summary>
        /// <returns>微信用户基本信息</returns>
        /// <remarks>2013-10-31 陶辉 创建</remarks>
        public WeChatUserInfo GetUserInfo(string openid)
        {
            var config = WeChatBo.Instance.GetWeixinConfig();
            var apiUrl = string.Format("https://api.weixin.qq.com/cgi-bin/user/info?access_token={0}&openid={1}"
                                        , AccessToken, openid);

            try
            {
                var req = GetWebRequest(apiUrl, "GET");
                var webResponse = (HttpWebResponse)req.GetResponse();
                var stream = new StreamReader(webResponse.GetResponseStream());
                var jsonStr = stream.ReadToEnd();

                return
                    Util.Serialization.JsonUtil.ToObject<WeChatUserInfo>(jsonStr);

            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 授权码
        /// </summary>
        public string AccessToken
        {
            get
            {
                //return GetAccessToken();
                return MemoryProvider.Default.Get<string>(KeyConstant.WeixinAccessToken, 100, GetAccessToken);
            }
        }

        /// <summary>
        /// 回复客户微信消息
        /// </summary>
        /// <param name="openID">接收方账号</param>
        /// <param name="content">回复消息内容</param>
        /// <param name="curUserSysNo">客服系统编号</param>
        /// <returns>返回结果实体</returns>
        /// <remarks>
        /// 2013-10-31 陶辉 创建
        /// 2013-11-07 郑荣华 实现数据插入
        /// </remarks>
        public Model.Result WriteBackMessage(string openID, string content, int curUserSysNo)
        {
            //消息实体
            var message = new WriteMessage()
            {
                touser = openID,
                msgtype = "text",
                text = new MessageContent()
                {
                    content = content
                }
            };
            var jsonMsg = Util.Serialization.JsonUtil.ToJson(message);//json数据包
            //api请求
            var apiUrl = "https://api.weixin.qq.com/cgi-bin/message/custom/send?access_token=" + AccessToken;
            var req = GetWebRequest(apiUrl, "POST");

            byte[] postData = Encoding.UTF8.GetBytes(jsonMsg);
            System.IO.Stream reqStream = req.GetRequestStream();
            reqStream.Write(postData, 0, postData.Length);
            reqStream.Close();

            var webResponse = (HttpWebResponse)req.GetResponse();
            var stream = new StreamReader(webResponse.GetResponseStream());
            var jsonStr = stream.ReadToEnd();
            var jsonObject = JObject.Parse(jsonStr);
            Model.Result result = null;
            try
            {
                if (jsonObject["errcode"].ToString() == "0")
                {
                    var model = new MkWeixinQuestion
                    {
                        Messages = content,
                        MessagesTime = DateTime.Now,
                        ReplyerSysNo = curUserSysNo,
                        Status = (int)MarketingStatus.微信咨询消息状态.已读,
                        Type = (int)MarketingStatus.微信咨询类型.回复,
                        WeixinId = openID,
                        CustomerSysNo = 0
                    };

                    IMkWeixinQuestionDao.Instance.Create(model);

                    result = new Model.Result()
                    {
                        Status = true,
                        StatusCode = 1,
                        Message = "回复消息成功"
                    };
                }
                else
                {
                    result = new Model.Result()
                    {
                        Status = false,
                        StatusCode = 0,
                        Message = "发送消息失败"
                    };
                }
            }
            catch
            {
                result = new Model.Result()
                {
                    Status = false,
                    StatusCode = 0,
                    Message = "发送消息失败"
                };
            }
            return result;
        }

        /// <summary>
        /// 创建自定义菜单
        /// </summary>
        /// <returns></returns>
        public Result CreateMenu()
        {
            var menu = new List<CustomizeMenu>()
            {
           
         
                            new CustomizeMenu(){
                                    type="view",
                                    name = "跨境商城",
                                    url="http://wx.nzag.cn/"
                                }
                      
                 ,  
              
                    
                            new CustomizeMenu(){
                                    type="view",
                                    name = "审批中心",
                                    url="http://wx.xxzzcs.com/weixin/GetVXAdminPageByOpenId"
                                }
                      
                  ,  
               
                    
                            new CustomizeMenu(){
                                    type="view",
                                    name = "我要分销",
                                    url="http://wx.xxzzcs.com/Member/Index"
                                }
                      
                  
               
           };

            //var menu = new ButtonMenu()
            //{
            //    button = new List<SubButtonMenu>() {
            //        new SubButtonMenu()
            //        {
            //            name="跨境商城",

            //            sub_button=new List<CustomizeMenu>()
            //            {
            //                new CustomizeMenu()
            //                {
            //                    type="view",
            //                    name="常见问题",
            //                    url="www.huiyuanti.com/Help/Index?id=19"
            //                },
            //                new CustomizeMenu()
            //                {
            //                    type="view",
            //                    name="购物流程",
            //                    url="www.huiyuanti.com/Help/Index?id=18"
            //                },
            //                new CustomizeMenu()
            //                {
            //                    type="view",
            //                    name="注册协议",
            //                    url="www.huiyuanti.com/Help/Index?id=17"
            //                }
            //            }
            //        },
            //        new SubButtonMenu()
            //        {
            //            name="物流配送",
            //            sub_button=new List<CustomizeMenu>()
            //            {
            //                new CustomizeMenu()
            //                {
            //                    type="view",
            //                    name="一小时加急送",
            //                    url="www.huiyuanti.com/Help/Index?id=33"
            //                },
            //                new CustomizeMenu()
            //                {
            //                    type="view",
            //                    name="门店自提",
            //                    url="www.huiyuanti.com/Help/Index?id=31"
            //                },
            //                new CustomizeMenu()
            //                {
            //                    type="view",
            //                    name="百城当日达",
            //                    url="www.huiyuanti.com/Help/Index?id=30"
            //                }
            //            }
            //        },
            //        new SubButtonMenu()
            //        {
            //            name="售后服务",
            //            sub_button=new List<CustomizeMenu>()
            //            {
            //                new CustomizeMenu()
            //                {
            //                    type="view",
            //                    name="防伪验证",
            //                    url="www.huiyuanti.com/Help/Index?id=38"
            //                },
            //                new CustomizeMenu()
            //                {
            //                    type="view",
            //                    name="退款说明",
            //                    url="www.huiyuanti.com/Help/Index?id=37"
            //                },
            //                new CustomizeMenu()
            //                {
            //                    type="view",
            //                    name="退换货流程",
            //                    url="www.huiyuanti.com/Help/Index?id=36"
            //                },
            //                new CustomizeMenu()
            //                {
            //                    type="view",
            //                    name="退换货政策",
            //                    url="www.huiyuanti.com/Help/Index?id=35"
            //                }
            //            }
            //        }
            //    }
            //};

            var jsonMsg = "{\"button\":" + Util.Serialization.JsonUtil.ToJson(menu) + "}";//json数据包
            //api请求
            var apiUrl = "https://api.weixin.qq.com/cgi-bin/menu/create?access_token=" + AccessToken;
            var req = GetWebRequest(apiUrl, "POST");

            byte[] postData = Encoding.UTF8.GetBytes(jsonMsg);
            System.IO.Stream reqStream = req.GetRequestStream();
            reqStream.Write(postData, 0, postData.Length);
            reqStream.Close();

            var webResponse = (HttpWebResponse)req.GetResponse();
            var stream = new StreamReader(webResponse.GetResponseStream());
            var jsonStr = stream.ReadToEnd();
            var jsonObject = JObject.Parse(jsonStr);
            Model.Result result = null;
            try
            {
                if (jsonObject["errcode"].ToString() == "0")
                {
                    result = new Model.Result()
                    {
                        Status = true,
                        StatusCode = 1,
                        Message = "创建自定义菜单成功"
                    };
                }
                else
                {
                    result = new Model.Result()
                    {
                        Status = false,
                        StatusCode = 0,
                        Message = "创建自定义菜单失败"
                    };
                }
            }
            catch
            {
                result = new Model.Result()
                {
                    Status = false,
                    StatusCode = 0,
                    Message = "创建自定义菜单成功"
                };
            }
            return result;
        }

        /// <summary>
        /// 获取请求方式
        /// </summary>
        /// <param name="url">接口地址</param>
        /// <param name="method">GET或POST</param>
        /// <returns>请求方式</returns>
        /// <remarks>2013-10-31 陶辉 创建</remarks>
        public HttpWebRequest GetWebRequest(string url, string method)
        {
            HttpWebRequest req = null;
            if (url.Contains("https"))
            {
                req = (HttpWebRequest)WebRequest.CreateDefault(new Uri(url));
            }
            else
            {
                req = (HttpWebRequest)WebRequest.Create(url);
            }

            req.ServicePoint.Expect100Continue = false;
            req.Method = method;
            req.KeepAlive = true;

            return req;
        }

        /// <summary>
        /// 回复消息实体
        /// </summary>
        /// <remarks>2013-10-31 陶辉 创建</remarks>
        public class WriteMessage
        {
            /// <summary>
            /// 接收账号
            /// </summary>
            public string touser { get; set; }

            /// <summary>
            /// 消息类型
            /// </summary>
            public string msgtype { get; set; }

            /// <summary>
            /// 消息
            /// </summary>
            public MessageContent text { get; set; }

        }

        /// <summary>
        /// 消息实体
        /// </summary>
        /// <remarks>2013-10-31 陶辉 创建</remarks>
        public class MessageContent
        {
            /// <summary>
            /// 消息内容
            /// </summary>
            public string content { get; set; }
        }


        /// <summary>
        /// 自定义主菜单
        /// </summary>
        /// <remarks>2014-01-23 陶辉 创建</remarks>
        public class ButtonMenu
        {
            /// <summary>
            /// 一级菜单数组，个数应为1~3个
            /// </summary>
            public List<SubButtonMenu> button { get; set; }

        }

        /// <summary>
        /// 自定义一级菜单
        /// </summary>
        /// <remarks>2014-01-23 陶辉 创建</remarks>
        public class CustomizeMenu
        {



            /// <summary>
            /// 菜单的响应动作类型，目前有click、view两种类型
            /// </summary>
            public string type { get; set; }

            /// <summary>
            /// 菜单标题，不超过16个字节，子菜单不超过40个字节
            /// </summary>
            public string name { get; set; }

            /// <summary>
            /// 菜单KEY值，用于消息接口推送，不超过128字节
            /// </summary>
            //public string key { get; set; }

            /// <summary>
            /// 网页链接，用户点击菜单可打开链接，不超过256字节
            /// </summary>
            public string url { get; set; }


        }

        public class SubButtonMenu
        {
            public List<CustomizeMenu> sub_button { get; set; }
            /// <summary>
            /// 菜单标题，不超过16个字节，子菜单不超过40个字节
            /// </summary>
            public string name { get; set; }
        }

        /// <summary>
        /// 自定义二级主菜单
        /// </summary>
        /// <remarks>2014-01-23 陶辉 创建</remarks>
        //public class SubButtonMenu
        //{
        //    /// <summary>
        //    /// 菜单的响应动作类型，目前有click、view两种类型
        //    /// </summary>
        //    public string type { get; set; }

        //    /// <summary>
        //    /// 菜单标题，不超过16个字节，子菜单不超过40个字节
        //    /// </summary>
        //    public string name { get; set; }

        //    /// <summary>
        //    /// 菜单KEY值，用于消息接口推送，不超过128字节
        //    /// </summary>
        //    //public string key { get; set; }

        //    /// <summary>
        //    /// 网页链接，用户点击菜单可打开链接，不超过256字节
        //    /// </summary>
        //    public string url { get; set; }
        //}

        #endregion



        public string GetOpenUrl(string redirectUri, string state)
        {
            //Game.Utils.FileManager.WriteFile(Hyt.Util.WebUtil.GetMapPath("/reurl.txt"), "https://open.weixin.qq.com/connect/oauth2/authorize?appid=" + AppID + "&redirect_uri=" + (redirectUri) + "&response_type=code&scope=snsapi_base&state=" + Hyt.Util.WebUtil.UrlEncode(state) + "#wechat_redirect");
            return "https://open.weixin.qq.com/connect/oauth2/authorize?appid=" + AppID + "&redirect_uri=" + (redirectUri) + "&response_type=code&scope=snsapi_base&state=" + Hyt.Util.WebUtil.UrlEncode(state) + "#wechat_redirect";
        }

        public string GetOpenUrl(string redirectUri, string state,string appid)
        {
            return "https://open.weixin.qq.com/connect/oauth2/authorize?appid=" + appid + "&redirect_uri=" + Hyt.Util.WebUtil.UrlEncode(redirectUri) + "&response_type=code&scope=snsapi_base&state=" + Hyt.Util.WebUtil.UrlEncode(state) + "#wechat_redirect";
        }

        public string GetOpenUrl(int DealerSysNo, string DealerHost)
        {
            string redirectUri = string.Format("http://{0}/{1}/WeiXin/Index", DealerHost, DealerSysNo);
            DsDealer dealer = Hyt.BLL.Stores.StoresBo.Instance.GetStoreById(DealerSysNo);
            string AppId = "";
            if (dealer != null)
            {
                AppId = dealer.AppID;
            }
            return GetOpenUrl(redirectUri, "1", AppId);
        }

        public string GetOpenidByCode(string code)
        {
            string url = string.Format(openid_url, AppID, AppSecret, code);
            JsonData jd = ExecuteApi(url, false);
            string openid = "";
            if (jd["errcode"].ToString() == "" || jd["errcode"].ToString() == "0")
            {
                openid = jd["result"]["openid"].ToString();
            }
            else
            {

                Hyt.BLL.Log.SysLog.Instance.Error(Model.WorkflowStatus.LogStatus.系统日志来源.外部应用, "方法 GetOpenidByCode(string code) 通过code获取openid失败 ,code:" + code + " errcode:" + jd["errcode"].ToString() + ";errmsg:" + jd["errmsg"].ToString(),
                Model.WorkflowStatus.LogStatus.系统日志目标类型.微信, 0, null, "", 0);
            }
            return openid;
        }
        public JsonData ExecuteApi(string url, bool is_check_access_token)
        {
            return ExecuteApi(url, is_check_access_token, "");
        }
        /// <summary>
        /// 执行API
        /// </summary>
        /// <param name="url">API接口url</param>
        /// <param name="is_check_access_token">是否检测access_token过期</param>
        /// <returns></returns>
        /// <remarks>2015-9-13 杨浩 创建</remarks>
        public JsonData ExecuteApi(string url, bool is_check_access_token, string post_data)
        {
            string new_url = url.Replace("[access_token]", AccessToken);
            string result = "";
            string errcode = "";
            string errmsg = "";
            ResponseWeiXin(post_data, new_url, ref result, ref errmsg, ref errcode);
            if (errcode == "42001" || errcode == "42002" || errcode == "40001" || (errmsg != "" && errmsg != "ok"))
            {
                MemoryProvider.Default.Remove(KeyConstant.WeixinAccessToken);
                new_url = url.Replace("[access_token]", AccessToken);
                errmsg = "";
                errcode = "";
                result = "";
                ResponseWeiXin(post_data, new_url, ref result, ref errmsg, ref errcode);
            }

            return JsonMapper.ToObject("{\"errcode\":\"" + errcode + "\",\"errmsg\":\"" + errmsg + "\",\"result\":" + result + "}");
        }

        /// <summary>
        /// 请求微信api
        /// </summary>
        /// <param name="post_data">post数据</param>
        /// <param name="new_url">api url</param>
        /// <param name="result">微信返回结果</param>
        /// <param name="errmsg">错误信息</param>
        /// <param name="errcode">错误代码</param>
        /// <remarks>2015-9-13 杨浩 创建</remarks>
        private void ResponseWeiXin(string post_data, string new_url, ref string result, ref string errmsg, ref string errcode)
        {
            try
            {

                if (post_data == "")
                    result = Hyt.Util.MyHttp.GetResponse(new_url);
                else
                    result = Hyt.Util.WebUtil.GetHttpWebPostJson(new_url, post_data);

                var jsonObject = JObject.Parse(result);
                if (jsonObject.Property("errcode") != null && jsonObject["errcode"].ToString() != "0" && jsonObject["errmsg"].ToString() != "ok")
                {
                    errmsg = jsonObject["errmsg"].ToString();
                    errcode = jsonObject["errcode"].ToString();
                    Hyt.BLL.Log.SysLog.Instance.Debug(Model.WorkflowStatus.LogStatus.系统日志来源.外部应用, "CallCenterReplyBo.CallCenterReplyBo.ResponseWeiXin(" + post_data + "," + new_url + "," + result + "," + errmsg + "," + errcode + "); 请求微信服务器返回值错误记录：" + result + "！",
                    Model.WorkflowStatus.LogStatus.系统日志目标类型.微信, 0, null, "", 0);
                }
                else
                {
                    errmsg = "";
                    errcode = "";
                }

            }
            catch (Exception ex)
            {
                errmsg = "请求微信服务器异常";
                errcode = "-1";
                Hyt.BLL.Log.SysLog.Instance.Error(Model.WorkflowStatus.LogStatus.系统日志来源.外部应用, "CallCenterReplyBo.CallCenterReplyBo.ResponseWeiXin(" + post_data + "," + new_url + "," + result + "," + errmsg + "," + errcode + ") 请求微信服务器异常！",
                Model.WorkflowStatus.LogStatus.系统日志目标类型.微信, 0, ex, "", 0);
            }
        }
        #region 发送模板消息
        /// <summary>
        /// 发送模板消息
        /// </summary>
        /// <param name="data">消息内容</param>
        /// <returns></returns>
        /// <remarks>2016-4-8 杨浩 创建</remarks>
        public Result SendTemplateMessage(string data)
        {
            var result = new Result()
            {
                Status = true
            };

            try
            {
                using (var service = new Hyt.Infrastructure.Communication.ServiceProxy<Hyt.Service.Contract.WeiXin.IWebChatService>())
                {
                    result = service.Channel.SendTemplateMessage(0, data);
                }

            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = ex.Message;
            }

            return result;
        }
        private static readonly object writeFile = new object();
        /// <summary>
        /// 发送模板消息
        /// </summary>
        /// <param name="data">消息内容</param>
        /// <returns></returns>
        /// <remarks>2016-4-8 杨浩 创建</remarks>
        public Result SendTemplateMessage(int dealerSysNo, string data)
        {

            var result = new Result()
            {
                Status = true
            };
            lock (writeFile)
            {
                try
                {
                    using (var service = new Hyt.Infrastructure.Communication.ServiceProxy<Hyt.Service.Contract.WeiXin.IWebChatService>())
                    {
                        result = service.Channel.SendTemplateMessage(dealerSysNo, data);
                        WriteLog("检测有无重复发送信息（Status）：" + result.Status + ",Message:" + result.Message);
                    }

                }
                catch (Exception ex)
                {
                    result.Status = false;
                    result.Message = ex.Message;
                }
            }
            return result;
        }


        /// <summary>
        /// 在本地写入错误日志
        /// </summary>
        /// <param name="exception"></param> 
        public static void WriteLog(string debugstr)
        {
            lock (writeFile)
            {
                FileStream fs = null;
                StreamWriter sw = null;

                try
                {
                    string filename = DateTime.Now.ToString("yyyy-MM-dd") + ".txt";
                    //服务器中日志目录
                    string folder = Hyt.Util.WebUtil.GetMapPath("/Log/");
                    if (!Directory.Exists(folder))
                        Directory.CreateDirectory(folder);
                    fs = new FileStream(folder + "/" + filename, System.IO.FileMode.Append, System.IO.FileAccess.Write);
                    sw = new StreamWriter(fs, Encoding.UTF8);
                    sw.WriteLine(DateTime.Now.ToString() + "     " + debugstr + "\r\n");
                }
                finally
                {
                    if (sw != null)
                    {
                        sw.Flush();
                        sw.Dispose();
                        sw = null;
                    }
                    if (fs != null)
                    {
                        //     fs.Flush();
                        fs.Dispose();
                        fs = null;
                    }
                }
            }
        }
        #endregion

    }
}
