using System.Collections.Generic;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Web;
using Hyt.Model;
using Hyt.Model.B2CApp;
using Hyt.Model.WorkflowStatus;
using Hyt.Service.Contract.Base;

namespace Hyt.Service.Contract.B2CApp
{
    /// <summary>
    /// 基础性接口
    /// </summary>
    /// <remarks> 2013-7-8 杨浩 创建</remarks>
    [ServiceKnownType(typeof(Result))]
    [ServiceContract]
    public interface ICommon : IBaseServiceContract
    {
        /// <summary>
        /// 获取版本更新
        /// </summary>
        /// <returns></returns>
        /// <remarks> 2013-7-5 杨浩 创建 </remarks>
        [CustomOperationBehavior(false)]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Result<Version> GetVersion(AppEnum.AppType type);

        /// <summary>
        /// 更新用户网络类型
        /// </summary>
        /// <returns></returns>
        /// <remarks> 2013-7-15 杨浩 创建</remarks>
        [CustomOperationBehavior(false)]
        [WebInvoke(Method = "PUT", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        Result UpdateNetworkType(string account, AppEnum.NetworkType networkType);

        /// <summary>
        /// 获取帮助信息
        /// </summary>
        /// <returns></returns>
        /// <remarks> 2013-7-15 杨浩 创建</remarks>
        [CustomOperationBehavior(false)]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Result<IList<FeArticle>> GetHelps();

        /// <summary>
        /// 获取反馈类型
        /// </summary>
        /// <param name="source">来源:商城(10),IphoneApp(20),AndroidApp(30)</param>
        /// <returns></returns>
        /// <remarks> 2013-7-15 杨浩 创建 </remarks>
        [CustomOperationBehavior(false)]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Result<IList<FeedbackType>> GetFeedbackType(CustomerStatus.意见反馈类型来源 source);

        /// <summary>
        /// 新增反馈信息
        /// </summary>
        /// <returns></returns>
        /// <remarks> 2013-7-15 杨浩 创建 </remarks>
        [CustomOperationBehavior(false)]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        Result AddFeedback(Feedback feedback);

        /// <summary>
        /// 获取省市区
        /// </summary>
        /// <param name="parentSysNo">区域上级系统号</param>
        /// <returns></returns>
        /// <remarks> 2013-7-15 杨浩 创建 </remarks>
        [CustomOperationBehavior(false)]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        Result<IList<BsArea>> GetArea(int parentSysNo);

        /// <summary>
        /// 获取支付宝配置信息
        /// </summary>
        /// <returns></returns>
        /// <remarks> 2013-7-15 杨浩 创建 </remarks>
        [CustomOperationBehavior(true)]
        [WebGet(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        Result<AlipayConfig> GetAlipayConfig();

        /// <summary>
        /// 获取附近门店信息
        /// </summary>
        /// <param name="longitude">经度</param>
        /// <param name="latitude">纬度</param>
        /// <returns></returns>
        /// <remarks> 2013-7-15 杨浩 创建 </remarks>
        [WebGet(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        [CustomOperationBehavior(false)]
        ResultPager<IList<NWarehouse>> GetNearbyStores(double longitude, double latitude);

        /// <summary>
        /// 获取启动屏时的广告
        /// </summary>
        /// <param name="type">平台类型：IphoneApp = 31,AndroidApp = 32</param>
        /// <returns></returns>
        /// <remarks> 2014-01-09 杨浩 创建 </remarks>
        [WebGet(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        [CustomOperationBehavior(false)]
        Result<string> GetStartAd(int type);

        /// <summary>
        /// 手机端广告
        /// </summary>
        /// <param name="type">手机端平台类型：IphoneApp = 31,AndroidApp = 32</param>
        /// <param name="storeId">手机端平台门店编号</param>
        /// <returns></returns>
        /// <remarks> 2016-07-13 杨云奕 创建 </remarks>
       [CustomOperationBehavior(false)]
       [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        List<Model.FeAdvertItem> GetStartAdvertItems(string type, int storeId);

        /// <summary>
        /// 手机端广告
        /// </summary>
        /// <param name="type">手机端平台类型：IphoneApp = 31,AndroidApp = 32</param>
        /// <param name="storeId">手机端平台门店编号</param>
        /// <param name="number">数量</param>
        /// <returns></returns>
        /// <remarks> 2016-07-13 杨云奕 创建 </remarks>
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        [CustomOperationBehavior(false)]
        List<Model.FeAdvertItem> GetStartAdvertItemsByThrie(string type, int storeId,int number);

        /// <summary>
        /// 获取商品数据
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="customerLevel">等级</param>
        /// <returns>2016-07-19 杨云奕 创建</returns>
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        [CustomOperationBehavior(false)]
        List<Model.PdProductIndex> GetSaleGoodsPdProductIndex(string type, int customerLevel, int storeId, int totalNum);
    }
}
