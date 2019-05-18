using Hyt.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace Hyt.Service.Contract.B2CApp
{

    /// <summary>
    /// 订单
    /// </summary>
    /// <remarks>2016-9-7 杨浩 创建</remarks>
    [ServiceKnownType(typeof(Result))]
    [ServiceContract]
    public interface IOrders
    {
        /// <summary>
        /// 添加订单
        /// </summary>
        /// <param name="data">订单数据</param>
        /// <returns></returns>
        /// <remarks>2016-9-7 杨浩 创建</remarks>
       [WebInvoke(Method = "POST",ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
       Result<string> AddOrder(Stream stream);
       /// <summary>
       /// 获取物流状态
       /// </summary>
       /// <param name="orderNo">订单编号</param>
       /// <returns></returns>
       /// <remarks>2016-09-09 杨浩 创建</remarks>
       [WebInvoke(Method = "GET", UriTemplate = "/GetLogisticsTracking/?orderNo={orderNo}", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
       Result<string> GetLogisticsTracking(string orderNo);

    }
}
