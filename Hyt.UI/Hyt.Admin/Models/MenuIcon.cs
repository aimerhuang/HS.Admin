using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hyt.Admin.Models
{
    /// <summary>
    /// 菜单图标
    /// </summary>
    /// <remarks>2013－08-09 朱家宏 创建</remarks>
    public class MenuIcon
    {
        /// <summary>
        /// 文件名称
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 文件大小
        /// </summary>
        public long FileSize { get; set; }

        /// <summary>
        /// 图标名
        /// </summary>
        public string IconName
        {
            get { return FileName; }
        }
    }
}