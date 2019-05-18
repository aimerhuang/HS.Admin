using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extra.UpGrade.Model
{
    /// <summary>
    /// 第三方待发货订单查询
    /// </summary>
    /// <remarks>2013-9-2 陶辉 创建</remarks>
    public class OrderParameters
    {
        #region 传入参数

        /// <summary>
        /// 订单起始时间
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// 订单截止时间
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// 商品编码
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 第三方订单号
        /// </summary>
        public string OrderID { get; set; }

        /// <summary>
        /// 买家昵称
        /// </summary>
        public string BuyerNick { get; set; }

        /// <summary>
        /// 是否买家留言
        /// </summary>
        public int HasMessage { get; set; }

        /// <summary>
        /// 是否使用旗帜
        /// </summary>
        public bool IsUseFlag { get; set; }

        /// <summary>
        /// 页码
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 每页显示数量
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 是否海外
        /// </summary>
        public bool IsAbroad { get; set; }

        /// <summary>
        /// 海外地区名称
        /// </summary>
        public string AbroadAreaName { get; set; }
        /// <summary>
        /// 海拍客回调的Xml内容
        /// </summary>
        public string Xml { get; set; }

        #endregion
    }

    /// <summary>
    /// 海外地区
    /// </summary>
    /// <remarks>2014-07-03 余勇 创建</remarks>
    public static class AbroadArea
    {
        /// <summary>
        /// 香港特别行政区
        /// </summary>
        public const string HK = "香港特别行政区";

        ///// <summary>
        ///// 澳门特别行政区
        ///// </summary>
        //public const string Macau = "澳门特别行政区";

        /// <summary>
        /// 海外地区数组
        /// </summary>
        public static string[] Areas = { HK };
        
    }
}
