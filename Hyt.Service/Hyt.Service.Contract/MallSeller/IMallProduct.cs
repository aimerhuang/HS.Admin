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

namespace Hyt.Service.Contract.MallSeller
{
    /// <summary>
    /// 商城商品编码维护接口
    /// </summary>
    /// <remarks>2013-8-28 陶辉 创建</remarks>
    [ServiceKnownType(typeof(Result))]
    [ServiceContract]
    public interface IMallProduct
    {
        /// <summary>
        /// 搜索商城商品信息
        /// </summary>
        /// <param name="keyword">商品名称关键字</param>
        /// <returns>商品列表</returns>
        /// <remarks>2013-8-28 陶辉 创建</remarks>
        //[WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        [CustomOperationBehavior(false)]
        [OperationContract]
        Result<List<UpGradeProduct>> GetProductList(string keyword);
        
        /// <summary>
        /// 匹配商品编码
        /// </summary>
        /// <param name="dealerSysNo">分销商系统编号</param>
        /// <param name="mallProductCode">商城商品编码</param>
        /// <param name="mallProductAttrs">商城商品属性，多属性用英文半角分号隔开</param>
        /// <param name="productId">商城商品编号</param>
        /// <returns>处理结果</returns>
        /// <remarks>2013-8-28 陶辉 创建</remarks>
        //[WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        [CustomOperationBehavior(false)]
        [OperationContract]
        Result MapProductCode(int dealerSysNo,string mallProductCode, string mallProductAttrs, string hytProductCode);
        
        /// <summary>
        /// 获取分销商所有已匹配的商品编码
        /// </summary>
        /// <param name="param">查询参数</param>
        /// <returns>商品列表</returns>
        /// <remarks>2013-8-28 陶辉 创建</remarks>
        //[WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        [CustomOperationBehavior(false)]
        [OperationContract]
        Result<PagedList<UpGradeProduct>> GetMapProductList(MapProductParameters param);


        /// <summary>
        /// 根据第三方商品编码和商品属性匹配商城商品编码
        /// </summary>
        /// <param name="dealerSysNo">分销商系统编号</param>
        /// <param name="mallProductCode">第三方商品编码</param>
        /// <param name="mallProductAttrs">第三方商品属性</param>
        /// <returns>商城商品编码</returns>
        [CustomOperationBehavior(false)]
        [OperationContract]
        Result<List<string>> GetHytProductCode(int dealerSysNo, string mallProductCode, string mallProductAttrs);
        /// <summary>
        /// 获取产品分销商价格
        /// </summary>
        /// <param name="dealerSysNo">分销商编号</param>
        /// <param name="hytProductID">产品编号</param>
        /// <returns></returns>
        /// <remarks>2013-09-11 朱成果 创建</remarks> 
        [CustomOperationBehavior(false)]
        [OperationContract]
        Result<decimal> GetHytPrice(int dealerSysNo, int hytProductID );
        /// <summary>
        /// 获取关联的商城产品详情
        /// </summary>
        /// <param name="dealerMallSysNo">分销商商城系统编号</param>
        /// <param name="mallProductId">商城商品编码</param>
        /// <returns></returns>
        /// <remarks>2013-09-13  朱成果 创建</remarks>
        [CustomOperationBehavior(false)]
        [OperationContract]
        Result<UpGradeOrderItem> MapHytProduct(int dealerMallSysNo, string mallProductId);


    }

    #region 已匹配商品关联关系查询参数

    /// <summary>
    /// 已匹配商品关联关系查询参数
    /// </summary>
    /// <remarks>2013-09-03 陶辉 创建</remarks>
    public class MapProductParameters
    {
        /// <summary>
        /// 分销商编号
        /// </summary>
        public int DealerSysNo { get; set; }

        /// <summary>
        /// 第三方商品编码
        /// </summary>
        public string MallProductCode { get; set; }

        /// <summary>
        /// 第三方商品名称
        /// </summary>
        public string MallProductName { get; set; }

        /// <summary>
        /// 当前页码
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 每页显示数量
        /// </summary>
        public int PageSize { get; set; }
    }

    #endregion
}
