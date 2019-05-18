using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using Hyt.Infrastructure.Pager;
using Hyt.Model;
using Hyt.Model.UpGrade;
using Hyt.Service.Contract.Base;

namespace Hyt.Service.Contract.MallSeller
{
    /// <summary>
    /// 商城商品编码维护接口
    /// </summary>
    /// <remarks>2013-8-28 陶辉 创建</remarks>
    [ServiceKnownType(typeof(Result))]
    [ServiceContract]
    public interface IMallRma
    {
        /// <summary>
        /// 第三方退货单导入商城
        /// </summary>
        /// <param name="mallRma">第三方退货单实体</param>
        /// <returns>处理结果</returns>
        /// <remarks>2013-8-29 陶辉 创建</remarks>
        //[WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        [CustomOperationBehavior(false)]
        [OperationContract]
        Result ImportMallRma(UpGradeRma mallRma);

        [CustomOperationBehavior(false)]
        [OperationContract]
        Result SaveRmaImg(UpGradeRmaImage img);
        /// <summary>
        /// 获取分销商的商城退货订单列表
        /// </summary>
        /// <param name="param">查询参数</param>
        /// <returns>已申请退货订单</returns>
        /// <remarks>2013-8-28 陶辉 创建</remarks>
        //[WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        [CustomOperationBehavior(false)]
        [OperationContract]
        Result<PagedList<UpGradeRma>> GetMallRmaList(MallRmaParameters param);
        
        /// <summary>
        /// 获取商城退货单详情
        /// </summary>
        /// <param name="rmaId">商城退换货单编号</param>
        /// <returns>商城退换货单信息</returns>
        /// <remarks>2013-8-29 陶辉 创建</remarks>
        //[WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        [CustomOperationBehavior(false)]
        [OperationContract]
        Result<UpGradeRma> GetMallRma(string rmaId);

        /// <summary>
        /// 根据订单升舱编号获取可退换货数量
        /// </summary>
        /// <param name="dsOrderSysNo">订单升舱编号</param>
        /// <returns>可退货商品信息</returns>
        /// <remarks>2013-9-12 陶辉 创建</remarks>
        [CustomOperationBehavior(false)]
        [OperationContract]
        Result<UpGradeRma> BuildMallRma(int  dsOrderSysNo);
        
        /// <summary>
        /// 获取退货单处理进度
        /// </summary>
        /// <param name="rmaId">商城退换货单编号</param>
        /// <returns>退货单处理进度</returns>
        /// <remarks>2013-8-29 陶辉 创建</remarks>
        //[WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        [CustomOperationBehavior(false)]
        [OperationContract]
        Result<List<HytRmaLog>> GetMallRmaLogs(int rmaId);

        /// <summary>
        /// 获取分销商退换货单
        /// </summary>
        /// <param name="shopAccount">账户</param>
        /// <param name="mallTypeSysNo">类型</param>
        /// <param name="top">取前几条</param>
        /// <param name="isFinish">退款完成</param>
        /// <returns></returns>
        /// <remarks>2013-09-10 黄志勇 创建</remarks>
        [CustomOperationBehavior(false)]
        [OperationContract]
        Result<List<DsReturn>> GetReturnInfo(string shopAccount, int mallTypeSysNo, int top, bool? isFinish);

        /// <summary>
        /// 查询可退换货的订单
        /// </summary>
        /// <param name="param">查询参数</param>
        /// <returns>可退换货订单列表</returns>
        /// <remarks>2013-09-10 黄志勇 创建</remarks>
        [CustomOperationBehavior(false)]
        [OperationContract]
        Result<PagedList<UpGradeOrder>> GetCanRmaOrderList(MallOrderParameters param);
      
    }

    #region 已升舱订单已申请退款信息查询参数
    
    /// <summary>
    /// 已升舱订单已申请退款信息查询参数
    /// </summary>
    /// <remarks>2013-09-03 陶辉 创建</remarks>
    public class MallRmaParameters
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
        /// 第三方商品编码
        /// </summary>
        public string ProductCode { get; set; }
        
        /// <summary>
        /// 第三方订单号
        /// </summary>
        public string OrderId { get; set; }
        
        /// <summary>
        /// 商城退货单编号
        /// </summary>
        public string RmaId { get; set; }
        
        /// <summary>
        /// 第三方商品名称
        /// </summary>
        public string ProductName { get; set; }
        
        /// <summary>
        /// 第三方买家昵称
        /// </summary>
        public string BuyerName { get; set; }
        
        /// <summary>
        /// 买家申请起始时间
        /// </summary>
        public DateTime? StartDate { get; set; }
        
        /// <summary>
        /// 买家申请截止时间
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// 当前页码
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 每页显示数量
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// 当前页号
        /// </summary>
        public int Id { get; set; }
    }
   
    #endregion
}
