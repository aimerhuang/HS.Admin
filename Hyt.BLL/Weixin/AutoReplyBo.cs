using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using Hyt.BLL.Union;
using Hyt.DataAccess.Weixin;
using Hyt.Model;
using Hyt.Model.Weixin;
using Hyt.Model.WorkflowStatus;

namespace Hyt.BLL.Weixin
{
    /// <summary>
    /// 微信自动回复业务类
    /// </summary>
    /// <remarks>2013-10-31 陶辉 创建</remarks>
    public class AutoReplyBo:BOBase<AutoReplyBo>
    {
        /// <summary>
        /// 接收信息处理方法
        /// </summary>
        /// <param name="postStr">接收信息</param>
        /// <returns>void</returns>
        /// <remarks>2013-10-31 陶辉 创建</remarks>
        public void Handle(string postStr)
        {
            //封装请求类
            var doc = new XmlDocument();
            doc.LoadXml(postStr);
            var rootElement = doc.DocumentElement;

            var xml = new RequestXml()
            {
                ToUserName = rootElement.SelectSingleNode("ToUserName").InnerText,
                FromUserName = rootElement.SelectSingleNode("FromUserName").InnerText,
                CreateTime = rootElement.SelectSingleNode("CreateTime").InnerText,
                MsgType = rootElement.SelectSingleNode("MsgType").InnerText
            };
            if (xml.MsgType == "text")
            {
                xml.Content = rootElement.SelectSingleNode("Content").InnerText;
            }
            else if (xml.MsgType == "event")
            {
                var selectSingleNode = rootElement.SelectSingleNode("Event");
                if (selectSingleNode != null)
                    xml.SubscribeEvent = selectSingleNode.InnerText;
            }

            #region 收集微信消息 

            //switch (xml.MsgType)
            //{
            //    case "text":
            //        xml.Content = rootElement.SelectSingleNode("Content").InnerText;
            //        break;
            //    case "location":
            //        xml.Location_X = rootElement.SelectSingleNode("Location_X").InnerText;
            //        xml.Location_Y = rootElement.SelectSingleNode("Location_Y").InnerText;
            //        xml.Scale = rootElement.SelectSingleNode("Scale").InnerText;
            //        xml.Label = rootElement.SelectSingleNode("Label").InnerText;
            //        break;
            //    case "image":
            //        xml.PicUrl = rootElement.SelectSingleNode("PicUrl").InnerText;
            //        break;
            //    case "event":
            //        xml.SubscribeEvent = rootElement.SelectSingleNode("Event").InnerText;
            //        break;
            //}

            #endregion

            //回复类型
            Reply reply = null;
            if (xml.MsgType == "event")
            {
                Hyt.Util.Log.LogManager.Instance.WriteLog("事件");
                reply = new SubscribeReply();
            }
            else
            {
                if (xml.Content.IndexOf("防伪验证")>=0)
                {
                    reply = new CheckAuthenticityReply();
                }
                else
                {
                    //自动回复
                    reply = new AutoReply();
                }
            }
            try
            {
                if (!string.IsNullOrEmpty(xml.Content))
                {
                    var question = new MkWeixinQuestion()
                    {
                        Messages = xml.Content,
                        MessagesTime = DateTime.Now,
                        ReplyerSysNo = 0,
                        Status = (int)MarketingStatus.微信咨询消息状态.未读,
                        Type = (int)MarketingStatus.微信咨询类型.咨询,
                        WeixinId = xml.FromUserName,
                        CustomerSysNo = 0
                    };
                    //question写入表
                    IMkWeixinQuestionDao.Instance.Create(question);
                }
            }
            catch (Exception)
            { 
                //TODO:
            }

            reply.WriteBackMessage(xml);
        }
    }

    /// <summary>
    /// 虚方法类
    /// </summary>
    /// <remarks>2013-10-15 陶辉 创建</remarks>
    public class Reply
    {
        /// <summary>
        /// 消息自动回复抽象方法
        /// </summary>
        /// <param name="request">回复参数</param>
        /// <remarks>2013-10-15 陶辉 创建</remarks>
        public virtual void WriteBackMessage(RequestXml request)
        {
            
        }
    }

    /// <summary>
    /// 防伪验证
    /// </summary>
    /// <remarks>2013-10-15 陶辉 创建</remarks>
    public class CheckAuthenticityReply : Reply
    {
        /// <summary>
        /// 回写防伪验证结果
        /// </summary>
        /// <param name="request">回写参数</param>
        /// <returns></returns>
        /// <remarks>2013-10-15 陶辉 创建</remarks>
        public override void WriteBackMessage(RequestXml request)
        {
            var result = WeChatBo.Instance.CheckProduct(request.Content.Split(new string[] { ":", "：" }, 
                                                                            StringSplitOptions.RemoveEmptyEntries)[1]);
            var message = result.Status ? NoHtml(result.Data.Msg) : result.Message;
            #region 数据初始化

            var resxml = string.Format(@"<xml>
                        <ToUserName><![CDATA[{0}]]></ToUserName>
                        <FromUserName><![CDATA[{1}]]></FromUserName>
                        <CreateTime>{2}</CreateTime>
                        <MsgType><![CDATA[text]]></MsgType>
                        <Content><![CDATA[{3}]]></Content>
                        <FuncFlag>1</FuncFlag>
                        </xml>", request.FromUserName, request.ToUserName, DateTime.Now.ToShortDateString(), message);

            #endregion
            try
            {
                Hyt.Util.Log.LogManager.Instance.WriteLog("防伪验证：" + resxml);
                System.Web.HttpContext.Current.Response.Write(resxml);
                System.Web.HttpContext.Current.Response.End();

                var answer = new MkWeixinQuestion()
                {
                    Messages = message,
                    MessagesTime = DateTime.Now,
                    ReplyerSysNo = 0,
                    Status = (int)MarketingStatus.微信咨询消息状态.已读,
                    Type = (int)MarketingStatus.微信咨询类型.回复,
                    WeixinId = request.FromUserName,
                    CustomerSysNo = 0
                };
                //answer写入表
                IMkWeixinQuestionDao.Instance.Create(answer);
            }
            catch (Exception)
            { 
                //TODO:
            }
            
        }

        ///   <summary>   
        ///   去除HTML标记   
        ///   </summary> 
        /// <param name="htmlstring">包括HTML的源码</param>
        /// <returns>已经去除后的文字</returns>
        ///   <remarks>2013-11-15  陶辉 创建</remarks>
        public string NoHtml(string htmlstring)
        {
            if (htmlstring == null) throw new ArgumentNullException("htmlstring");
            //删除脚本   
            htmlstring = Regex.Replace(htmlstring, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
            //删除HTML   
            htmlstring = Regex.Replace(htmlstring, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"-->", "", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"<!--.*", "", RegexOptions.IgnoreCase);

            htmlstring = Regex.Replace(htmlstring, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&(nbsp|#160);", "   ", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&#(\d+);", "", RegexOptions.IgnoreCase);

            htmlstring.Replace("<", "");
            htmlstring.Replace(">", "");
            htmlstring.Replace("\r\n", "");
            htmlstring = System.Web.HttpContext.Current.Server.HtmlEncode(htmlstring).Trim();

            return htmlstring;
        }
    }

    /// <summary>
    /// 自动回复
    /// </summary>
    /// <remarks>2013-10-15 陶辉 创建</remarks>
    public class AutoReply : Reply
    {
        /// <summary>
        /// 回写自动回复结果
        /// </summary>
        /// <param name="request">回写参数</param>
        /// <returns></returns>
        /// <remarks>2013-10-15 陶辉 创建</remarks>
        public override void WriteBackMessage(RequestXml request)
        {
            var list = WeChatBo.Instance.GetAutoReplys(request.Content);
            #region 构造数据

            //var item1 = new MkWeixinKeywordsReply()
            //{
            //    ReplyType = (int)MarketingStatus.微信关键词回复类型.文本,
            //    Content = "请稍等，待会给您回复。"
            //};
            //var item2 = new MkWeixinKeywordsReply()
            //{
            //    ReplyType = (int)MarketingStatus.微信关键词回复类型.图文,
            //    Title = "商城新品上市了，赶快下手吧",
            //    Content = @"<div style='text-align:center'><img alt="" data-pinit='registered' src='http://image.huiyuanti.com/ckimages/20130422162706.jpg' /><p>新品上市了</p></div>",
            //    Image = "http://image.huiyuanti.com/ckimages/20130422162714.jpg",
            //    Hyperlink = "http://www.huiyuanti.com/Home/Announcement/19"
            //};
            ////list.Add(item1);
            //list.Add(item2);
           

            #endregion

            #region 初始化数据

            var xmls = list.Select((q, i) => new
            {
                str = q.ReplyType == (int)MarketingStatus.微信关键词回复类型.文本 ?

                #region 文本回复内容

 string.Format(@"<xml>
                                <ToUserName><![CDATA[{0}]]></ToUserName>
                                <FromUserName><![CDATA[{1}]]></FromUserName>
                                <CreateTime>{2}</CreateTime>
                                <MsgType><![CDATA[text]]></MsgType>
                                <Content><![CDATA[{3}]]></Content>
                                <FuncFlag>1</FuncFlag>
                                </xml>", request.FromUserName, request.ToUserName, DateTime.Now.ToShortDateString(), q.Content)

                #endregion
 :
                #region 图文回复内容

 string.Format(@"<xml>
                                 <ToUserName><![CDATA[{0}]]></ToUserName>
                                 <FromUserName><![CDATA[{1}]]></FromUserName>
                                 <CreateTime>{2}</CreateTime>
                                 <MsgType><![CDATA[news]]></MsgType>
                                 <ArticleCount>1</ArticleCount>
                                 <Articles>
                                 <item>
                                 <Title><![CDATA[{3}]]></Title> 
                                 <Description><![CDATA[{4}]]></Description>
                                 <PicUrl><![CDATA[{5}]]></PicUrl>
                                 <Url><![CDATA[{6}]]></Url>
                                 </item>
                                 </Articles>
                                 </xml>", request.FromUserName, request.ToUserName, DateTime.Now.ToShortDateString(), q.Title, q.Content, q.Image, q.Hyperlink)

                #endregion
                                        ,
                                        content=q.Content

            }).ToList();

            #endregion

            try
            {
                foreach (var xml in xmls)
                {
                    System.Web.HttpContext.Current.Response.Write(xml.str);

                    var answer = new MkWeixinQuestion()
                    {
                        Messages = xml.content,
                        MessagesTime = DateTime.Now,
                        ReplyerSysNo = 1,
                        Status = (int)MarketingStatus.微信咨询消息状态.已读,
                        Type = (int)MarketingStatus.微信咨询类型.回复,
                        WeixinId = request.FromUserName,
                        CustomerSysNo = 0
                    };
                    //answer写入表
                    IMkWeixinQuestionDao.Instance.Create(answer);

                    break;
                }

                System.Web.HttpContext.Current.Response.End();
                
            }
            catch (Exception)
            { 
                //TODO:
            }
        }
    }

    /// <summary>
    /// 订阅
    /// </summary>
    /// <remarks>2013-10-15 陶辉 创建</remarks>
    public class SubscribeReply : Reply
    {
        /// <summary>
        /// 回写订阅结果
        /// </summary>
        /// <param name="request">回写参数</param>
        /// <returns></returns>
        /// <remarks>2013-10-15 陶辉 创建</remarks>
        public override void WriteBackMessage(RequestXml request)
        {
            var config = new MkWeixinConfig();

            if (config.FollowType == (int)MarketingStatus.微信消息类型.文本)
            {
                Hyt.Util.Log.LogManager.Instance.WriteLog("订阅回复开始");

                #region 初始化数据

                var resxml = string.Format(@"<xml>
                        <ToUserName><![CDATA[{0}]]></ToUserName>
                        <FromUserName><![CDATA[{1}]]></FromUserName>
                        <CreateTime>{2}</CreateTime>
                        <MsgType><![CDATA[text]]></MsgType>
                        <Content><![CDATA[{3}]]></Content>
                        <FuncFlag>1</FuncFlag>
                        </xml>", request.FromUserName, request.ToUserName, DateTime.Now.ToShortDateString(), config.FollowText);

                #endregion
                Hyt.Util.Log.LogManager.Instance.WriteLog("订阅：" + resxml);
                System.Web.HttpContext.Current.Response.Write(resxml);
                System.Web.HttpContext.Current.Response.End();
            }
        }
    }

    /// <summary>
    /// 内容异常
    /// </summary>
    /// <remarks>2013-10-15 陶辉 创建</remarks>
    public class ErrorReply : Reply
    {
        /// <summary>
        /// 回写异常结果
        /// </summary>
        /// <param name="request">回写参数</param>
        /// <returns></returns>
        /// <remarks>2013-10-15 陶辉 创建</remarks>
        public override void WriteBackMessage(RequestXml request)
        {
            var message = "抱歉，您输入的格式有误！";

            #region 初始化数据

            var resxml = string.Format(@"<xml>
                        <ToUserName><![CDATA[{0}]]></ToUserName>
                        <FromUserName><![CDATA[{1}]]></FromUserName>
                        <CreateTime>{2}</CreateTime>
                        <MsgType><![CDATA[text]]></MsgType>
                        <Content><![CDATA[{3}]]></Content>
                        <FuncFlag>1</FuncFlag>
                        </xml>", request.FromUserName, request.ToUserName, DateTime.Now.ToShortDateString(), message);

            #endregion

            try
            {
                System.Web.HttpContext.Current.Response.Write(resxml);
                System.Web.HttpContext.Current.Response.End();

                var answer = new MkWeixinQuestion()
                {
                    Messages = message,
                    MessagesTime = DateTime.Now,
                    ReplyerSysNo = 0,
                    Status = (int)MarketingStatus.微信咨询消息状态.已读,
                    Type = (int)MarketingStatus.微信咨询类型.回复,
                    WeixinId = request.FromUserName,
                    CustomerSysNo = 0
                };
                //answer写入表
                IMkWeixinQuestionDao.Instance.Create(answer);
            }
            catch (Exception)
            { 
                //TODO:
            }
        }
    }
}
