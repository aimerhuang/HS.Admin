using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;
using Hyt.Model;
using Hyt.Model.B2CApp;
using Hyt.Service.Contract.Base;

namespace Hyt.Service.Contract
{
    /// <summary>
    /// 二维码扫描后显示的网站服务
    /// </summary>
    /// <remarks>
    /// 2013-10-24 郑荣华 创建
    /// </remarks>
    [ServiceKnownType(typeof(Result))]
    [ServiceContract]
    public interface ITwoDimensionCode : IBaseServiceContract
    {
        /// <summary>
        /// 获取app下载地址，依次为 商城Android，商城Ios，百城通
        /// </summary>
        /// <returns>最新版本列表</returns>
        /// <remarks>
        /// 2013-10-24 郑荣华 创建
        /// </remarks>
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        IList<string> GetAppUrl();

        /// <summary>
        /// 获取推荐周边产品，直接调用Product.svc的GetShake
        /// </summary>
        /// <param name="brand">手机品牌</param>
        /// <param name="type">手机型号</param>
        /// <returns>商品信息</returns>
        /// <remarks>
        /// 2013-10-12 郑荣华 创建
        /// </remarks>
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        ResultPager<IList<SimplProduct>> GetRelationProduct(string brand, string type);
    }

}
