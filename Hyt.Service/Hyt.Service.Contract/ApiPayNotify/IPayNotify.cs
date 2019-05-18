using Hyt.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Xml.Serialization;

namespace Hyt.Service.Contract.ApiPayNotify
{
    #region 易宝支付回调参数
    /// <summary>
    /// 异步回执
    /// </summary>
    /// <remarks>2016-1-1 杨浩 创建</remarks>
    [DataContract]
    public class NotifyResult
    {
        /// <summary>
        /// 支付流水号
        /// </summary>
        [DataMember(Name = "paySerialNumber")]
        public string PaySerialNumber { get; set; }
        /// <summary>
        /// 海关报关信息
        /// </summary>
        [DataMember(Name = "customsInfos")]
        public List<CustomsInfos> CustomsInfos { get; set; }

    }
    /// <summary>
    /// 海关报关信息
    /// </summary>
    /// <remarks>2016-1-1 杨浩 创建</remarks>
    [DataContract]
    public class CustomsInfos
    {
        /// <summary>
        /// 海关报关状态
        /// </summary>
        [DataMember(Name = "status")]
        public string Status { get; set; }
    }
    #endregion
    /// <summary>
    /// 支付异步回调
    /// </summary>
    [ServiceContract]
    public interface IPayNotify
    {
        /// <summary>
        /// 易宝支付海关报关异步回执
        /// </summary>
        /// <param name="stream">http请求上下文信息</param>
        /// <returns></returns>
        /// <remarks>2015-10-12 杨浩 创建</remarks>
        [OperationContractAttribute(Name = "EhkingNotify")]
        [WebInvoke(UriTemplate = "EhkingNotify", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        string EhkingNotifyReceipt(Stream stream);
        /// <summary>
        /// 易宝支付海关报关异步回执
        /// </summary>
        /// <param name="nameValuePair"></param>
        /// <returns></returns>
        /// <remarks>2015-12-31 杨浩 创建</remarks>     
        [WebInvoke(UriTemplate = "EhkingNotifyReceipt", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Result EhkingNotifyReceipt(NotifyResult notifyResult);

        /// <summary>
        /// 易扫购订单异步回调
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        /// <remarks>2016-5-8 杨浩 创建</remarks>
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        string EhkingScannNotifyReceipt(Stream stream);
    }
}