using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.B2CApp
{
    /// <summary>
    /// 广告组
    /// </summary>
    /// <remarks>2013-8-6 杨浩 添加</remarks>
    public class AdvertGroup
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        public int SysNo { get; set; }

        /// <summary>
        /// 组名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 颜色值
        /// </summary>
        public string NameColor { get; set; }

        /// <summary>
        /// 广告组图标
        /// </summary>
        public string AdvertGroupIcon { get; set; }

        /// <summary>
        /// 广告项明细
        /// </summary>
        public IList<AdvertItem> Items { get; set; }
    }

    /// <summary>
    /// 广告项
    /// </summary>
    /// <remarks>2013-8-6 杨浩 添加</remarks>
    public class AdvertItem
    {
       
        /// <summary>
        /// 广告名称
        /// </summary>
        public string Name { get; set; }
       
        /// <summary>
        /// 广告图片Url
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        /// 链接
        /// </summary>
        public string LinkUrl { get; set; }

        /// <summary>
        /// 产品系统号
        /// </summary>
        public int ProductSysNo { get; set; }
    }
}
