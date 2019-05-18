using Hyt.Model;
using Hyt.Service.Contract.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace Hyt.Service.Contract.Task
{
    /// <summary>
    /// 计划任务服务
    /// </summary>
    /// <remarks>2016-5-31 杨浩 创建</remarks>
    [ServiceContract]
    public interface ITaskService : IBaseServiceContract
    {
        /// <summary>
        /// 心跳检测
        /// </summary>
        /// <returns></returns>
        /// <remarks>2016-6-2 杨浩 创建</remarks>
        [OperationContract]
        [WebGet(UriTemplate = "/", BodyStyle = WebMessageBodyStyle.Bare)]
        Result Ping();
        /// <summary>
        /// 执行订单返利操作
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/Execute/RebatesRecord", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        Result ExecuteRebatesRecord();
        /// <summary>
        /// 自动确认收货
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Result ExecuteConfirmationOfReceipt();
        /// <summary>
        /// 清理订单
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Result ExecuteOrder();
        /// <summary>
        /// 执行任务池自动分配
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Result ExecuteSyJob();

        /// <summary>
        /// 同步订单100
        /// </summary>
        /// <returns></returns>
        /// <remarks>2017-04-25 杨浩 创建</remarks>
        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Result ExecuteImportErpOrder100();
        /// <summary>
        /// 同步销售出库单到ERP
        /// </summary>
        /// <returns></returns>
        /// <remarks>2017-04-25 杨浩 创建</remarks>
        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Result ExecuteOrderToErp();

        /// <summary>
        /// 同步销售出库单到信业ERP
        /// </summary>
        /// <returns></returns>
        /// <remarks>2017-04-25 杨浩 创建</remarks>
        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Result ExecuteOrderToXinYeErp();
        /// <summary>
        /// 同步订单发货信息至第三方商城平台
        /// </summary>
        /// <returns></returns>
        /// <remarks>2017-08-31 吴琨 创建</remarks>
        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Result SynchroOrder();
        /// <summary>
        /// 同步第三方商城平台订单
        /// </summary>
        /// <returns></returns>
        /// <remarks>2017-08-31 杨浩 创建</remarks>
        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Result SynchronizeDsMallOrder();
    }
}
