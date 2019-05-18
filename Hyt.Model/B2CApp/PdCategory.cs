using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.B2CApp
{
    /// <summary>
    /// 分类信息
    /// </summary>
    /// <remarks>2014-1-21 黄波 创建</remarks>
    public class ParentClassApp
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        public int SysNo { get; set; }

        /// <summary>
        /// 图标URL地址
        /// </summary>
        public string ImgUrl { get; set; }

        /// <summary>
        /// 分类标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 简介
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// 子分类列表
        /// </summary>
        public IList<SubClassApp> Items { get; set; }
    }

    /// <summary>
    /// 子分类信息
    /// </summary>
    /// <remarks>2014-1-21 黄波 创建</remarks>
    public class SubClassApp
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        public int SysNo { get; set; }

        /// <summary>
        /// 分类名称
        /// </summary>
        public string Title { get; set; }
    }
}
