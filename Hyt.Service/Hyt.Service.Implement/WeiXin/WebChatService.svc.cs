using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text;
using System.IO;
using Hyt.Model;
using Hyt.Service.Contract.WeiXin;
using Hyt.BLL.Weixin;
using Hyt.Service.Contract.WeiXin.Model;
using Hyt.Util.Extension;
using System.Net;
using Newtonsoft.Json.Linq;
using Hyt.Infrastructure.Memory;
using Newtonsoft.Json;
using Hyt.Infrastructure.Caching;
using Hyt.Util;
using System.Threading;
using Hyt.BLL.ScheduledEvents;
using Hyt.Model.Transfer;
namespace Hyt.Service.Implement.WeiXin
{
    /// <summary>
    /// 微信服务 
    /// </summary>
    /// <remarks>2015-1-9 杨浩 创建</remarks>
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class WebChatService :IWebChatService
    {
        private static readonly object _lockThis = new object();
        private static readonly List<int> _dealerSysNolist = new List<int>();
        static Timer eventTimer;
        /// <summary>
        /// 当前分销商编号
        /// </summary>
        /// <remarks>2015-1-9 杨浩 创建</remarks>
        private int DealerSysNo { get; set; }

        /// <summary>
        /// 当前分销商
        /// </summary>
        /// <remarks>2016-5-28 杨浩 创建</remarks>
        private CBDsDealer CurrentDealer
        {
            get
            {
                var dealerInfo = BLL.Distribution.DsDealerBo.Instance.GetDsDealer(DealerSysNo);
                try
                {
                    int dealerId = 0;

                    if (dealerInfo.AppID == "" || string.IsNullOrEmpty(dealerInfo.AppID) || dealerInfo.AppSecret == "" || string.IsNullOrEmpty(dealerInfo.AppSecret))
                    {
                        dealerInfo = BLL.Distribution.DsDealerBo.Instance.GetDsDealer(dealerId);
                    }
                }
                catch (Exception)
                {
                    
                    throw;
                }
                
                return dealerInfo;
            }
        }
        /// <summary>
        /// 微信授权码
        /// </summary>
        /// <remarks>2015-1-9 杨浩 创建</remarks>
        private string AccessToken
        {
            get
            {
                return MemoryProvider.Default.Get<string>(string.Format(KeyConstant.WeixinAccessToken_,DealerSysNo),100,()=>
                {
                    var dealerInfo = CurrentDealer;   

                    var apiUrl = string.Format("https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}"
                                       , dealerInfo.AppID, dealerInfo.AppSecret);
                    
                    string token = "";
                    try
                    {
                        var req =Hyt.Util.WebUtil.GetWebRequest(apiUrl, "GET");
                        var webResponse = (HttpWebResponse)req.GetResponse();
                        var stream = new StreamReader(webResponse.GetResponseStream());
                        var jsonStr = stream.ReadToEnd();
                        var jsonObject = JObject.Parse(jsonStr);
                        if (jsonObject.Property("access_token")!= null)
                        {
                            token = jsonObject["access_token"].ToString();
                            //StartUpEvent("ClearAccessTokenEvent", DealerSysNo);
                        }
                        else
                        {
                            string json = JsonConvert.SerializeObject(jsonObject);
                            BLL.Log.LocalLogBo.Instance.Write("获取微信授权码错误：\r\n" + json + "\r\n" + "dealerInfo=" + JsonConvert.SerializeObject(dealerInfo), "WeiXinAccessTokenLog");
                            //TODO:返回错误
                        }
                    }
                    catch (Exception ex)
                    {
                        BLL.Log.LocalLogBo.Instance.Write("获取微信授权码错误：\r\n" + ex.Message + "\r\n", "WeiXinAccessTokenLog");
                        //TODO:异常
                    }
                    return token;
                });
            }
        }
        /// <summary>
        /// 微信分销js签名
        /// </summary>
        /// <remarks>2015-1-9 杨浩 创建</remarks>
        private string JsTicket
        {
            get
            {
                RemoveWeiXinCache(DealerSysNo);
                return MemoryProvider.Default.Get<string>(string.Format(KeyConstant.WeixinJsTicket_,DealerSysNo),100,() =>
                {
                    string ticke = "";
                    string apiUrl = string.Format("https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token={0}&type=jsapi", AccessToken);
                    try
                    {
                        var req = Hyt.Util.WebUtil.GetWebRequest(apiUrl, "GET");
                        var webResponse = (HttpWebResponse)req.GetResponse();
                        var stream = new StreamReader(webResponse.GetResponseStream());
                        var jsonStr = stream.ReadToEnd();
                        var jsonObject = JObject.Parse(jsonStr);
                        string errcode = "";
                        if (jsonObject.Property("errcode") != null)
                            errcode = jsonObject["errcode"].ToString();
                        if (jsonObject.Property("ticket") != null)//errcode
                        {
                            ticke=jsonObject["ticket"].ToString();
                            //StartUpEvent("ClearJsTicketEvent",DealerSysNo);
                           
                        }
                        else
                        {
                            string errmsg = jsonObject["errmsg"].ToString();
                            //string json=JsonConvert.SerializeObject(jsonObject);
                            //BLL.Log.LocalLogBo.Instance.Write("获取微信Js签名错误：\r\n" + json + "\r\n", "WeiXinJsTicketLog");
                            //TODO:返回错误
                        }
                        RemoveWeiXinCache(DealerSysNo, errcode);
                    }
                    catch(Exception ex)
                    {
                        BLL.Log.LocalLogBo.Instance.Write("获取微信Js签名异常：\r\n" + ex.Message+ "\r\n", "WeiXinJsTicketLog");
                        //TODO:异常
                    }
                    return ticke;
                }); 
            }
        }
        /// <summary>
        /// 启动和重置最后更新时间
        /// </summary>
        /// <param name="key">事件key</param>
        /// <param name="dealerSysNo">经销商编号</param>
        /// <remarks>2016-5-4 杨浩 创建</remarks>
        private void StartUpEvent(string key, int dealerSysNo)
        {
            if (eventTimer == null)
            {
                eventTimer = new Timer((sender) => {
                    try
                    {
                        EventManager.Execute();
                    }
                    catch
                    {
                        EventLogs.WriteFailedLog("Failed ScheduledEventCallBack");
                    }

                }, null, 60000, EventManager.TimerMinutesInterval * 60000);
            }
            else
            {
                EventManager.UpdateTimeByKeyAndDealerSysNo(key, dealerSysNo);
            }
        }
   
        #region 创建微信菜单
        /// <summary>
        /// 创建微信菜单
        /// 1、自定义菜单最多包括3个一级菜单，每个一级菜单最多包含5个二级菜单。
        /// 2、一级菜单最多4个汉字，二级菜单最多7个汉字，多出来的部分将会以“...”代替。（一个汉字两个字节长度）
        /// 3、创建自定义菜单后，由于微信客户端缓存，需要24小时微信客户端才会展现出来。测试时可以尝试取消关注公众账号后再次关注，则可以看到创建后的效果。
        /// </summary>
        /// <param name="dealerSysNo">经销商编号</param>
        /// <remarks>2015-1-9 杨浩 创建</remarks>
        public Result CreateMenu(int dealerSysNo)
        {
            DealerSysNo = dealerSysNo;
            var result = new Result()
            {
                Status = false
            };

            try
            {
                #region 微信菜单读取

              

                if (CurrentDealer.SysNo != dealerSysNo)
                {
                    result.Status = false;
                    result.Message = "当前分销商没有配置微信开发者接口";
                    return result;
                }  
            
                RemoveWeiXinCache(dealerSysNo);

                var menus = MkCustomizeMenuBo.Instance.GetAllMkCustomizeMenuList(dealerSysNo);
                if (menus == null || (menus != null && menus.Count() == 0))
                {
                    result.Status = false;
                    result.Message = "请先添加微信菜单！";
                    return result;
                }

                StringBuilder sb = new StringBuilder();

                //BLL.Log.LocalLogBo.Instance.Write(DealerSysNo+"|"+JsonConvert.SerializeObject(menus),"CreateMenuLog");
                //一级菜单
                var rootMenus = menus.Where(x => x.Pid == 0).OrderBy(x => x.Order);
                int i = 0;
                sb.Append("{\"button\":[");
                foreach (var m in rootMenus)
                {
                    //获得当前菜单的子菜单
                    var childs = menus.Where(x => x.Pid == m.SysNo).OrderBy(x=>x.Order);

                    int childCount =childs==null?0:childs.Count();
                   
                    i++;
                    if (i > 3)
                        break;
                    if (i > 1)
                        sb.Append(",");

                    sb.Append("{");
                    sb.Append("\"name\":\"" + m.Name.Replace("\"", "”").SubString(8, "") + "\",");

                    if (childCount > 0)
                        sb.Append("\"sub_button\":[");
                    else
                    {
                        sb.Append("\"type\":\"" + m.Type + "\",");//view 或 click
                        sb.Append("\"" + (m.Type == "view" ? "url" : "key") + "\":\"" + (m.Type == "view" ? (m.Url??"") : (m.Key??"")).ToString() + "\"");
                    }
                   
                    int j = 0;
                   
                        foreach (var m1 in childs) //循环子菜单
                        {
                          
                            j++;
                            if (j > 5)
                                break;
                            if (j > 1)
                                sb.Append(",");

                            sb.Append("{");
                            sb.Append("\"name\":\"" + m1.Name.Replace("\"", "”").SubString(14, "") + "\",");
                            sb.Append("\"type\":\"" + m1.Type + "\",");

                            sb.Append("\"" + (m1.Type == "view" ? "url" : "key") + "\":\"" + (m1.Type == "view" ? m1.Url :(m1.Key??"")).ToString()+ "\"");
                            sb.Append("}");

                          
                        }
                      
                        if (childCount > 0)
                            sb.Append("]"); 
                       

                    sb.Append("}");
                }
                sb.Append("]}");
                #endregion

                BLL.Log.LocalLogBo.Instance.Write(sb.ToString()+"--------->\r\n", "CreateMenuLog");

                #region 提交菜单数据到微信
                //api请求
                var apiUrl = "https://api.weixin.qq.com/cgi-bin/menu/create?access_token=" + AccessToken;
                var req = Hyt.Util.WebUtil.GetWebRequest(apiUrl, "POST");

                byte[] postData = Encoding.UTF8.GetBytes(sb.ToString());
                System.IO.Stream reqStream = req.GetRequestStream();
                reqStream.Write(postData, 0, postData.Length);
                reqStream.Close();

                var webResponse = (HttpWebResponse)req.GetResponse();
                var stream = new StreamReader(webResponse.GetResponseStream());
                var jsonStr = stream.ReadToEnd();
                var jsonObject = JObject.Parse(jsonStr);
                if (jsonObject["errcode"].ToString() == "0")
                {
                    result.Status = true;
                    result.StatusCode = 1;
                    result.Message = "创建自定义菜单成功";
                }
                else
                {
                    result.Status = false;
                    result.StatusCode = 0;
                    RemoveWeiXinCache(dealerSysNo, jsonObject["errcode"].ToString());                   
                    result.Message = "错误代码：" + jsonObject["errcode"].ToString() +" 错误消息："+ jsonObject["errmsg"].ToString();

                }
                #endregion
            }
            catch(Exception ex)
            {
                result.Status = false;
                result.StatusCode = -1;
                result.Message = "创建自定义菜单失败！";
                BLL.Log.LocalLogBo.Instance.Write(ex.Message, "CreateMenuLog");
            }
            return result;
        }
        #endregion
        /// <summary>
        /// 获取微信授权码
        /// </summary>
        /// <param name="dealerSysNo">经销商编号</param>
        /// <remarks>2015-1-9 杨浩 创建</remarks>
        public Result GetAccessToken(int dealerSysNo)
        {
            DealerSysNo = dealerSysNo;

            //BLL.Log.LocalLogBo.Instance.Write(dealerSysNo.ToString()+"\r\n", "GetAccessTokenLog");
            var result = new Result()
            {
                Status = false
            };

            string accessToken =AccessToken;
            if (accessToken != "")
            {
                result.Status = true;
                result.Message = accessToken;
            }
          
            return result;
        }

        /// <summary>
        /// 获取微信分享签名
        /// </summary>
        /// <param name="dealerSysNo">经销商编号</param>
        /// <remarks>2015-1-9 杨浩 创建</remarks>
        public Result GetJsTicket(int dealerSysNo)
        {
            DealerSysNo = dealerSysNo;
            var result = new Result()
            {
                Status = false
            };

            string jsTicket = JsTicket;
            if (jsTicket != "")
            {
                result.Status = true;
                result.Message = jsTicket;
            }
            return result;
        }
        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="key">缓存键值</param>
        /// <remarks>2016-1-14 杨浩 创建</remarks>
        public Result RemoveCache(string key)
        {
            var result = new Result()
            {
                Status = false
            };

            try
            {
                MemoryProvider.Default.Remove(key);        
                result.Status = true;
                result.Message = "删除成功！";
            }
            catch (Exception ex){result.Message = ex.Message;}           
            return result;
        }
        /// <summary>
        /// 重置微信缓存
        /// </summary>
        /// <param name="dealerSysNo">店铺编号</param>
        /// <remarks>2016-3-22 杨浩 创建</remarks>
        public Result ResetWeiXinCache(int dealerSysNo)
        {
            var result = RemoveWeiXinCache(dealerSysNo);        
            return result;
        }

        /// <summary>
        /// 判断指定错误代码移除微信全局缓存
        /// </summary>
        /// <param name="dealerSysNo">经销商编号</param>
        /// <param name="errcode">错误代码</param>
        /// <remarks>2016-5-1 杨浩 创建</remarks>
        public Result RemoveWeiXinCache(int dealerSysNo,string errcode = "")
        {
            DealerSysNo = dealerSysNo;
            var result = new Result()
            {
                Status = true
            };

            if (errcode == "" || errcode == "42001" || errcode == "40001")
            {
                try
                {
                    //lock (_lockThis)
                    //{
                    //    //判断是否有锁
                    //    if (_dealerSysNolist.Contains(dealerSysNo))
                    //        return new Result { Status = false, Message = "正在删除中,请稍后再试..." };
                    //    //加锁
                    //    _dealerSysNolist.Add(dealerSysNo);
                    //}


                    //测试是否AccessToken有效，无效则清除缓存
                    var _result = GetCallBackIp(dealerSysNo);
                    if (!_result.Status)
                    {
                        //清除微信全局缓存
                        RemoveCache(string.Format(KeyConstant.WeixinAccessToken_, dealerSysNo));
                        //CacheManager.RemoveCache(CacheKeys.Items.WeixinAccessToken_, dealerSysNo.ToString());

                        //RemoveCache(string.Format(KeyConstant.WeixinJsTicket_, dealerSysNo));
                        //CacheManager.RemoveCache(CacheKeys.Items.WeixinJsTicket_, dealerSysNo.ToString());
                    }

                    //_dealerSysNolist.Remove(dealerSysNo);                                            
                    result.Status = true;
                    result.Message = "删除成功！";
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
        /// 发送模板消息
        /// </summary>
        /// <param name="dealerSysNo">经销商编号</param>
        /// <param name="data">模板字符</param>
        /// <remarks>2016-4-14 杨浩 创建</remarks>
        public Result SendTemplateMessage(int dealerSysNo, string data)
        {

            DealerSysNo = dealerSysNo;

            var result = new Result()
            {
                Status = false
            };

            try
            {
                
                string apiUrl = "https://api.weixin.qq.com/cgi-bin/message/template/send?access_token=" + AccessToken;

                HttpWebRequest req = (HttpWebRequest)WebRequest.CreateDefault(new Uri(apiUrl));

                req.ServicePoint.Expect100Continue = false;
                req.Method = "POST";
                req.KeepAlive = true;

                byte[] postData = Encoding.UTF8.GetBytes(data);
                System.IO.Stream reqStream = req.GetRequestStream();
                reqStream.Write(postData, 0, postData.Length);
                reqStream.Close();

                var webResponse = (HttpWebResponse)req.GetResponse();
                var stream = new StreamReader(webResponse.GetResponseStream());
                var jsonStr = stream.ReadToEnd();
                var jsonObject = JObject.Parse(jsonStr);

                if (jsonObject["errcode"].ToString() != "0")
                {
                    RemoveWeiXinCache(DealerSysNo,jsonObject["errcode"].ToString());
                    result.Status = false;
                    result.Message = jsonObject["errmsg"].ToString();                   
                }          

            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = ex.Message;
            }

            return result;
        }

        /// <summary>
        /// 发送模板消息
        /// </summary>
        /// <param name="dealerSysNo">经销商编号</param>
        /// <param name="data">模板字符</param>
        /// <remarks>2016-4-14 杨浩 创建</remarks>
        public Result SendMessage(int dealerSysNo, string data)
        {

            DealerSysNo = dealerSysNo;

            var result = new Result()
            {
                Status = false
            };
            result.StatusCode = dealerSysNo;
            result.Message = "无数据";

            return result;
        }


        /// <summary>
        /// 上传图片
        /// </summary>
        /// <param name="list">表单信息列表</param>
        /// <param name="dealerSysNo">经销商编号</param>
        /// <param name="fileData">图片字节数组</param>
        /// <returns></returns>
        /// <remarks>2016-4-16 杨浩 创建</remarks>     
        public Result UploadImage(int dealerSysNo,byte[] fileData)
        {
            DealerSysNo = dealerSysNo;

            List<FormItem> list = new List<FormItem>();
            Stream imgStream = new MemoryStream(fileData);
            FormItem fi = new FormItem()
            {
                Name = "",
                FileStream = imgStream,
                ParamType = ParamType.File,
                Value = DateTime.Now.ToString("yyyyMMddmmss") + ".png"
            };
            list.Add(fi);

            var result = new Result<string>()
            {
                Status = false
            };

            try
            {
                string apiUrl = "https://api.weixin.qq.com/cgi-bin/media/upload?access_token=" + AccessToken + "&type=image";

                string jsonStr = Hyt.Util.WebUtil.PostFormData(list, apiUrl);

                var jsonObject = JObject.Parse(jsonStr);

                if (jsonObject["errcode"].ToString() != "0")
                {
                    result.Message = jsonObject["errmsg"].ToString();
                }
                else
                {
                    result.Status=true;
                    result.Message = jsonObject["media_id"].ToString();
                    result.Data = jsonObject["created_at"].ToString();
                }
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = ex.Message;
            }          
            return result;
        }
        /// <summary>
        /// 创建二维码
        /// </summary>
        /// <param name="dealerSysNo">经销商编号</param>
        /// <param name="actionName"> 1:永久 2:临时</param>
        /// <param name="sceneId">场景ID（整数类型）</param>
        /// <param name="sceneStr">场景ID字符串</param>
        /// <returns></returns>
        /// <remarks>2016-4-16 杨浩 创建</remarks>     
        public Result CreateQrcode(int dealerSysNo, string actionName, string sceneStr, int sceneId)
        {
            DealerSysNo = dealerSysNo;

            var result = new Result()
            {
                Status = false
            };

            try
            {
                string apiUrl = "https://api.weixin.qq.com/cgi-bin/qrcode/create?access_token=" + AccessToken;

                //二维码类型，QR_SCENE为临时,QR_LIMIT_SCENE为永久,QR_LIMIT_STR_SCENE为永久的字符串参数值
                //string data = "{\"action_name\": \"QR_LIMIT_SCENE\",\"expire_seconds\":2592000, \"action_info\": {\"scene\": {\"scene_id\":\"0825\",\"scene_str\": \"0256uuuu\"}}}";

                string data = "{\"action_name\": \"QR_LIMIT_STR_SCENE\",\"action_info\": {\"scene\": {\"scene_str\": \"" + sceneStr + "\"}}}";
                if (actionName == "2")
                {
                    data = "{\"action_name\": \"QR_SCENE\",\"expire_seconds\":2592000, \"action_info\": {\"scene\": {\"scene_id\":\"" + sceneId + "\"}}}";
                }
                HttpWebRequest req = (HttpWebRequest)WebRequest.CreateDefault(new Uri(apiUrl));

                req.ServicePoint.Expect100Continue = false;
                req.Method = "POST";
                req.KeepAlive = true;

                byte[] postData = Encoding.UTF8.GetBytes(data);
                System.IO.Stream reqStream = req.GetRequestStream();
                reqStream.Write(postData, 0, postData.Length);
                reqStream.Close();

                var webResponse = (HttpWebResponse)req.GetResponse();
                var stream = new StreamReader(webResponse.GetResponseStream());
                var jsonStr = stream.ReadToEnd();
                var jsonObject = JObject.Parse(jsonStr);

                if (jsonObject.Property("errcode") != null && jsonObject["errcode"].ToString() != "0")
                {
                    result.Status = false;
                    result.Message = jsonObject["errmsg"].ToString();
                }
                else
                {
                    string url = jsonObject["url"].ToString();
                    result.Status = true;
                    result.Message = url;
                }
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = ex.Message;
            }

            return result;
        }

        /// <summary>
        /// 获取微信服务器IP地址
        /// </summary>
        /// <param name="dealerSysNo">经销商编号</param>
        /// <returns></returns>
        /// <remarks>2016-5-1 杨浩 创建</remarks>
        public Result GetCallBackIp(int dealerSysNo)
        {
            DealerSysNo = dealerSysNo;

            var result = new Result()
            {
                Status = false
            };

            try
            {
                string apiUrl = "https://api.weixin.qq.com/cgi-bin/getcallbackip?access_token=" + AccessToken;
                
                var req = Hyt.Util.WebUtil.GetWebRequest(apiUrl, "GET");
                var webResponse = (HttpWebResponse)req.GetResponse();
                var stream = new StreamReader(webResponse.GetResponseStream());
                var jsonStr = stream.ReadToEnd();


                BLL.Log.LocalLogBo.Instance.Write(jsonStr, "GetCallBackIpLog");
                var jsonObject = JObject.Parse(jsonStr);
                
                if (jsonObject.Property("ip_list") != null)
                {
                    result.Status = true;
                    result.Message = jsonObject["ip_list"].ToString();
                }
                else
                {              
                    string errcode = jsonObject["errcode"].ToString();
                    string errmsg = jsonObject["errmsg"].ToString();
                    result.Status = false;
                    result.Message = errmsg;
                    //RemoveWeiXinCache(dealerSysNo,errcode);
                    //TODO:返回错误
                }
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = ex.Message;
            }
            return result;
        }
    }
}
