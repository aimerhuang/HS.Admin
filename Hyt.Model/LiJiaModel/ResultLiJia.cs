using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Hyt.Model.LiJiaModel
{
    /// <summary>
    /// 利嘉返回数据
    /// </summary>
    /// <remarks>
    /// 2017-05-18 罗勤尧 生成
    /// </remarks>
    public class ResultLiJia
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        [DataMember(Name = "Success")]
        public bool Success { get; set; }
        /// <summary>
        /// accessToken
        /// </summary>
        [DataMember(Name = "Message")]
        public string Message { get; set; }
        /// <summary>
        /// ERP订单号
        /// </summary>
        [DataMember(Name = "OrderNo")]
        public string OrderNo { get; set; }

        /// <summary>
        /// 会员id
        /// </summary>
        [DataMember(Name = "MemberId")]
        public int MemberId { get; set; }
        /// <summary>
        /// 商品分类id
        /// </summary>
        [DataMember(Name = "CategoryId")]
        public int CategoryId { get; set; }
        ///// <summary>
        ///// 上级商品分类id
        ///// </summary>
        //[DataMember(Name = "ParentCategoryId")]
        //public int ParentCategoryId { get; set; }
        ///// <summary>
        ///// 商品分类名称
        ///// </summary>
        //[DataMember(Name = "CategoryName")]
        //public string CategoryName { get; set; }
        /// <summary>
        /// 会员编码
        /// </summary>
        [DataMember(Name = "MemberCode")]
        public string MemberCode { get; set; }

        /// <summary>
        /// 查询结果总记录
        /// </summary>
        [DataMember(Name = "total")]
        public int total { get; set; }

        /// <summary>
        /// 数据集
        /// </summary>
        [DataMember(Name = "rows")]
        public string rows { get; set; }

        #region 新增品牌返回扩展
        /// <summary>
        /// 品牌id
        /// </summary>
        [DataMember(Name = "BrandId")]
        public int BrandId { get; set; }
        /// <summary>
        /// 品牌编码
        /// </summary>
        [DataMember(Name = "BrandCode")]
        public string BrandCode { get; set; }
        #endregion

        #region 新增仓库返回扩展
       
        /// <summary>
        /// ERP仓库编码
        /// </summary>
        [DataMember(Name = "StoreCode")]
        public string StoreCode { get; set; }
        #endregion

        #region 入库返回扩展

        /// <summary>
        ///ERP入库单号
        /// </summary>
        [DataMember(Name = "PurchaseInboundOrderNo")]
        public string PurchaseInboundOrderNo { get; set; }
        #endregion
    }
}
