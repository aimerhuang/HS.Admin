using Hyt.Model;
using Hyt.Service.Contract.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace Hyt.Service.Contract.ApiPay
{
    /// <summary>
    /// 支付接口对接
    /// </summary>
    /// <remarks>2015-12-28 杨浩 创建</remarks>
    //[ServiceKnownType(typeof(Result))]
    [ServiceContract]
    public interface IPay
    {
        /// <summary>
        /// 海关报关
        /// </summary>
        /// <param name="orderSysNo">订单编号</param>
        /// <param name="payCode">支付编码</param>
        /// <returns></returns>
        /// <remarks>2015-10-12 杨浩 创建</remarks>    
       [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
       Result ApplyToCustoms(int orderSysNo, int payCode);

       /// <summary>
       /// 海关报关查询
       /// </summary>
       /// <param name="orderSysNo">订单编号</param>
       /// <param name="payCode">支付编码</param>
       /// <returns></returns>
       /// <remarks>2015-12-31 杨浩 创建</remarks>
       [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
       Result CustomsQuery(int orderSysNo, int payCode);
    }
}
