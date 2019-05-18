using Hyt.Model;
using Hyt.Service.Contract.Base;
using Hyt.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace Hyt.Service.Contract.WeiXin
{
    /// <summary>
    /// 微信服务
    /// </summary>
    /// <remarks>2016-1-9 杨浩 创建</remarks>
    [ServiceContract]
    public interface IWebChatService:IBaseServiceContract
    {
        /// <summary>
        /// 创建微信菜单
        /// </summary>
        /// <param name="dealerSysNo">经销商编号</param>
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        Result CreateMenu(int dealerSysNo);

        /// <summary>
        /// 获取微信授权码
        /// </summary>
        /// <param name="dealerSysNo">经销商编号</param>
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        Result GetAccessToken(int dealerSysNo);

        /// <summary>
        /// 获取微信分享签名
        /// </summary>
        /// <param name="dealerSysNo">经销商编号</param>
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        Result GetJsTicket(int dealerSysNo);
        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="key">缓存键值</param>
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        Result RemoveCache(string key);
        /// <summary>
        /// 重置微信缓存
        /// </summary>
        /// <param name="dealerSysNo">店铺编号</param>
        /// <remarks>2016-3-22 杨浩 创建</remarks>
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        Result ResetWeiXinCache(int dealerSysNo);
        /// <summary>
        /// 移除微信全局缓存
        /// </summary>
        /// <param name="dealerSysNo">经销商编号</param>
        /// <param name="errcode">微信接口回调错误代码</param>
        /// <returns></returns>
        /// <remarks>2016-5-1 杨浩 创建</remarks>
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        Result RemoveWeiXinCache(int dealerSysNo, string errcode = "");
        /// <summary>
        /// 发送模板消息
        /// </summary>
        /// <param name="dealerSysNo">经销商编号</param>
        /// <param name="data">模板字符</param>
        /// <remarks>2016-4-14 杨浩 创建</remarks>
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        Result SendTemplateMessage(int dealerSysNo,string data);

        /// <summary>
        /// 上传图片
        /// </summary>
        /// <param name="list">表单信息列表</param>
        /// <param name="dealerSysNo">经销商编号</param>
        /// <param name="fileData">图片字节数组</param>
        /// <returns></returns>
        /// <remarks>2016-4-16 杨浩 创建</remarks>
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        Result UploadImage(int dealerSysNo,byte[] fileData);
        /// <summary>
        /// 创建二维码
        /// </summary>
        /// <param name="dealerSysNo">经销商编号</param>
        /// <param name="actionName"> QR_LIMIT_STR_SCENE:永久 QR_LIMIT_SCENE:临时</param>
        /// <param name="sceneId">场景ID（整数类型）</param>
        /// <param name="sceneStr">场景ID字符串</param>
        /// <returns></returns>
        /// <remarks>2016-4-16 杨浩 创建</remarks>
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        Result CreateQrcode(int dealerSysNo, string actionName, string sceneStr, int sceneId);

        /// <summary>
        /// 获取微信服务器IP地址
        /// </summary>
        /// <param name="dealerSysNo">经销商编号</param>
        /// <returns></returns>
        /// <remarks>2016-5-1 杨浩 创建</remarks>
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        Result GetCallBackIp(int dealerSysNo);

        /// <summary>
        /// 发送模板消息
        /// </summary>
        /// <param name="dealerSysNo">经销商编号</param>
        /// <param name="data">模板字符</param>
        /// <remarks>2016-4-14 杨浩 创建</remarks>
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        Result SendMessage(int dealerSysNo, string data);
        
    }
}
