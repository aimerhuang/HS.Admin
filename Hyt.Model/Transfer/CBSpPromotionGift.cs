using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Hyt.Model.WorkflowStatus;

namespace Hyt.Model.Transfer
{

    /// <summary>
    /// 促销赠品
    /// </summary>
    /// <remarks>2013-09-26 吴文强 创建</remarks>
    public class CBSpPromotionGift : SpPromotionGift
    {
        /// <summary>
        /// 获取缩略图
        /// </summary>
        public Func<string> GetThumbnail { private get; set; }

        /// <summary>
        /// 赠品图标
        /// </summary>
        public string GiftIcon
        {
            //TODO:需实现
            get { return "http://image.huiyuanti.com/b2capp/cx_zp.png"; }
            set { }
        }

        /// <summary>
        /// 商品缩略图
        /// </summary>
        public string Thumbnail
        {
            get { return GetThumbnail != null ? GetThumbnail() : string.Empty;; }
            set { }
        }

        /// <summary>
        /// 基础价 余勇 添加 2014-01-21
        /// </summary>
        public decimal BasePrice { get; set; }

        /// <summary>
        /// 商品Erp编号 余勇 添加 2014-01-21
        /// </summary>
        public string ProductErpCode { get; set; } 
    }
}
