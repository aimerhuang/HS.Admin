using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Model;
using Hyt.Model.WorkflowStatus;
using Hyt.Model.Common;
using Hyt.Model.Transfer;
namespace Hyt.BLL.ApiLogistics
{
    /// <summary>
    /// 物流接口
    /// </summary>
    /// <remarks>2015-10-12 杨浩 创建</remarks>
    public abstract class ILogisticsProvider
    {
        /// <summary>
        /// 物流配置
        /// </summary>
        /// <remarks>2016-3-8 杨浩 创建</remarks>
        protected static LogisticsConfig config = BLL.Config.Config.Instance.GetLogisticsConfig();
        /// <summary>
        /// 物流代码
        /// </summary>
        /// <remarks>2015-12-26 杨浩 创建</remarks>
        public abstract Hyt.Model.CommonEnum.物流代码 Code { get; }
        /// <summary>
        /// json特殊字符过滤
        /// </summary>
        /// <param name="jsonStr">需要过滤的字符</param>
        /// <returns></returns>
        ///<remarks>2016-3-22 杨浩 创建</remarks>
        protected string JsonStrEscape(string jsonStr)
        {
            if (jsonStr == null || string.Empty == jsonStr)
                return "";
            return
                jsonStr.Replace("\\", "\\\\")
                .Replace("\"", "\\\"")
                .Replace("\n", "")
                .Replace("\r", "");
        }
       
        /// <summary>
        /// 获取产品信息
        /// </summary>
        /// <param name="productId">商品编码</param>
        /// <returns></returns>
        /// <remarks>2016-3-10 杨浩 创建</remarks>
        public abstract Result GetProduct(string productId);
        /// <summary>
        /// 添加交易订单
        /// </summary>
        /// <param name="orderSysno">销售订单系统编号</param>
        /// <returns></returns>
        /// <remarks>2016-3-21 杨浩 创建</remarks>
        public abstract Result AddOrderTrade(int orderSysno);
        /// <summary>
        /// 查询订单运单号信息
        /// </summary>
        /// <param name="orderId">订单号</param>
        /// <returns></returns>
        /// <remarks>2016-3-22 杨浩 创建</remarks>
        public abstract Result GetOrderExpressno(string orderId);
        /// <summary>
        /// 查询订单信息
        /// </summary>
        /// <param name="orderId">订单号</param>
        /// <returns></returns>
        /// <remarks>2016-3-22 杨浩 创建</remarks>
        public abstract Result GetOrderTrade(string orderId);

        /// <summary>
        /// 查询订单物流信息
        /// </summary>
        /// <param name="orderSysNo">订单号</param>
        /// <returns></returns>
        /// <remarks>2016-04-09 陈海裕 创建</remarks>
        public abstract Result<string> GetLogisticsTracking(int orderSysNo);

        /// <summary>
        /// 取消订单
        /// </summary>
        /// <param name="orderSysNo"></param>
        /// <returns></returns>
        /// <remarks>2016-07-05 陈海裕 创建</remarks>
        public abstract Result CancelOrderTrade(int orderSysNo, string reason="");
      
    }
}
