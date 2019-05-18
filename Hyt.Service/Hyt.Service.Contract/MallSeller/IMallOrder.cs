using Hyt.Service.Contract.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using Hyt.Infrastructure.Pager;
using Hyt.Model.UpGrade;
using Hyt.Model;
using Hyt.Model.Transfer;

namespace Hyt.Service.Contract.MallSeller
{
    /// <summary>
    /// 已升舱订单查询接口
    /// </summary>
    /// <remarks>2013-8-28 陶辉 创建</remarks>
    [ServiceKnownType(typeof(Result))]
    [ServiceContract]
    public interface IMallOrder : IBaseServiceContract
    {
        /// <summary>
        /// 根据开始日期获取指定状态的升舱订单
        /// </summary>
        /// <param name="startDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <param name="dearerMallSysNo">商城系统编号</param>
        /// <returns></returns>
        [CustomOperationBehavior(false)]
        [OperationContract]
        List<CBDsOrder> GetSuccessedOrder(DateTime startDate,DateTime endDate,int dearerMallSysNo);

        /// <summary>
        /// 查询分销商已升舱订单
        /// </summary>
        /// <param name="param">查询参数</param>
        /// <returns>已升舱订单列表</returns>
        /// <remarks>2013-8-28 陶辉 创建</remarks>
        //[WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        [CustomOperationBehavior(false)]
        [OperationContract]
        Result<PagedList<UpGradeOrder>> GetMallOrderList(MallOrderParameters param);

        /// <summary>
        /// 获取单条升舱订单信息
        /// </summary>
        /// <param name="orderId">第三方订单编号</param>
        /// <returns>升舱订单信息</returns>
        /// <remarks>2013-8-28 陶辉 创建</remarks>
        //[WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        [CustomOperationBehavior(false)]
        [OperationContract]
        Result<UpGradeOrder> GetMallOrder(string orderId);

        /// <summary>
        /// 获取已升舱订单详情
        /// </summary>
        /// <param name="sysNo">订单升舱编号</param>
        /// <returns></returns>
        /// <remarks>2013-09-26 朱成果 创建</remarks> 
        [CustomOperationBehavior(false)]
        [OperationContract]
        Result<UpGradeOrder> GetDsOrder(int sysNo);

        /// <summary>
        /// 商城订单单条导入商城
        /// </summary>
        /// <param name="order">第三方订单实体</param>
        /// <returns>处理结果</returns>
        /// <remarks>2013-8-28 陶辉 创建</remarks>
        //[WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        [CustomOperationBehavior(false)]
        [OperationContract]
        Result ImportMallOrder(UpGradeOrder order);

        /// <summary>
        /// 商城订单合并导入商城
        /// </summary>
        /// <param name="orders">第三方订单实体列表</param>
        /// <param name="map_x">地图横坐标</param>
        /// <param name="map_y">地图纵坐标</param>
        /// <returns>处理结果</returns>
        /// <remarks>2013-8-28 陶辉 创建</remarks>
        //[WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        [CustomOperationBehavior(false)]
        [OperationContract]
        Result CombineImportMallOrder(List<UpGradeOrder> orders, double map_x = 0, double map_y = 0);

        /// <summary>
        /// 获取订单日志
        /// </summary>
        /// <param name="orderId">第三方订单号</param>
        /// <returns>物流日志</returns>
        /// <remarks>2013-8-28 陶辉 创建</remarks>
        //[WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        [CustomOperationBehavior(false)]
        [OperationContract]
        Result<List<HytOrderLog>> GetLogisticsLog(string orderId);

        /// <summary>
        /// 订单日志
        /// </summary>
        /// <param name="transactionSysNo"></param>
        /// <returns></returns>
        /// <remarks>2013-09-26 朱成果 创建</remarks>
        [CustomOperationBehavior(false)]
        [OperationContract]
        Result<List<HytOrderLog>> GetLogisticsLogByTransactionSysNo(string transactionSysNo);

        /// <summary>
        /// 获取商城所有的省份
        /// </summary>
        /// <returns>省份列表</returns>
        /// <remarks>2013-9-3 陶辉 创建</remarks>
        [CustomOperationBehavior(false)]
        [OperationContract]
        Result<List<BsArea>> GetHytProvinceList();

        /// <summary>
        /// 获取商城省份的所有城市
        /// </summary>
        /// <param name="provinceSysNo">省份编号</param>
        /// <returns>省份下面的市列表</returns>
        /// <remarks>2013－09-03 陶辉 创建</remarks>
        [CustomOperationBehavior(false)]
        [OperationContract]
        Result<List<BsArea>> GetHytCityList(int provinceSysNo);

        /// <summary>
        /// 查询商城城市下的所有区域
        /// </summary>
        /// <param name="citySysNo">城市编号</param>
        /// <returns>获取城市下面的地区列表</returns>
        /// <remarks>2013－09-03 陶辉 创建</remarks>
        [CustomOperationBehavior(false)]
        [OperationContract]
        Result<List<BsArea>> GetHytAreaList(int citySysNo);

        /// <summary>
        /// 根据地区编号，获取省市区信息
        /// </summary>
        /// <param name="sysNo">地区编号</param>
        /// <param name="cityEntity">城市信息</param>
        /// <param name="areaEntity">地区信息</param>
        /// <returns>省市区信息</returns>
        /// <remarks> 2013-09-03 陶辉 创建 </remarks>
        [CustomOperationBehavior(false)]
        [OperationContract]
        Result<BsArea> GetHytProvinceEntity(int sysNo, out BsArea cityEntity, out BsArea areaEntity);

        /// <summary>
        /// 根据省市区名称匹配商城地区编号
        /// </summary>
        /// <param name="provinceName">省</param>
        /// <param name="cityName">市</param>
        /// <param name="DistrictName">区</param>
        /// <returns>商城地区编号</returns>
        /// <remarks> 2013-09-03 陶辉 创建 </remarks>
        [CustomOperationBehavior(false)]
        [OperationContract]
        Result<int> GetDistrictSysNo(string provinceName, string cityName, string DistrictName);

        /// <summary>
        /// 获取分销商升舱订单
        /// </summary>
        /// <param name="shopAccount">店铺账号</param>
        /// <param name="mallTypeSysNo">分销商城类型系统编号</param>
        /// <param name="top">取前几条</param>
        /// <param name="isFinish">升舱完成</param>
        /// <returns></returns>
        /// <remarks>2013-09-10 黄志勇 创建</remarks>
        [CustomOperationBehavior(false)]
        [OperationContract]
        Result<List<DsOrder>> GetDsOrderInfo(string shopAccount, int mallTypeSysNo, int top, bool? isFinish);

        /// <summary>
        /// 分销商预存款主表
        /// </summary>
        /// <param name="shopAccount">店铺账号</param>
        /// <param name="mallTypeSysNo">分销商城类型系统编号</param>
        /// <returns></returns>
        /// <remarks>2013-09-10 黄志勇 创建</remarks>
        [CustomOperationBehavior(false)]
        [OperationContract]
        Result<DsPrePayment> GetPrePaymentInfo(string shopAccount, int mallTypeSysNo);

        /// <summary>
        /// 更新订单备注信息
        /// </summary>
        /// <param name="remarks">备注内容</param>
        /// <param name="auth">授权参数</param>
        /// <returns>2014-03-25</returns>
        //[CustomOperationBehavior(false)]
        //[OperationContract]
        //Result UpdateTradeRemarks(IRemarksParameters remarks, AuthorizationParameters auth);

    }

    #region 已升舱订单查询参数实体

    /// <summary>
    /// 已升舱订单查询参数实体
    /// </summary>
    /// <remarks>2013-09-03 陶辉 创建</remarks>
    public class MallOrderParameters
    {
        /// <summary>
        /// 商城类型
        /// </summary>
        public int MallType { get; set; }

        /// <summary>
        /// 分销商商城编号
        /// </summary>
        public int DealerMallSysNo { get; set; }

        /// <summary>
        /// 第三方商品编号
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// 第三方订单编号
        /// </summary>
        public string OrderID { get; set; }

        /// <summary>
        /// 第三方商品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 第三方买家昵称
        /// </summary>
        public string BuyerName { get; set; }

        /// <summary>
        /// 商城发货状态
        /// </summary>
        public int? LogisticsStatus { get; set; }

        /// <summary>
        /// 第三方订单导入时间
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// 第三方订单导入时间
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// 是否有第三方买家留言
        /// </summary>
        public bool HasMessage { get; set; }

        /// <summary>
        /// 当前页码
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 每页显示数量
        /// </summary>
        public int PageSize { get; set; }

        public int OrderState { get; set; }

        /// <summary>
        ///商城订单号
        /// </summary>
        public int? OrderSysNo { get; set; }

        /// <summary>
        /// 收货人姓名
        /// </summary>
        public string ReceiveName { get; set; }

        /// <summary>
        /// 收货人手机
        /// </summary>
        public string MobilePhoneNumber { get; set; }

    }

    #endregion

    #region 已升舱未发货订单查询实体
    /// <summary>
    /// 已升舱未发货订单查询实体
    /// </summary>
    /// <remarks>2014-04-08 黄波 创建</remarks>
    public class OrderUpGradedWaitSendParameters
    {
        /// <summary>
        /// 订单起始时间
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// 订单截至时间
        /// </summary>
        public DateTime EndDate { get; set; }
    }
    #endregion
}
