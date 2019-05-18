using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Manual
{
    /// <summary>
    /// 用户菜单,权限
    /// </summary>
    /// <remarks>2013-08-01 朱成果 创建</remarks>
    public class SyUserMenu
    {
        /// <summary>
        /// 用户编号
        /// </summary>
        public int UserID { get; set; }
        /// <summary>
        /// 菜单ID或者权限ID
        /// </summary>
        public int MenuID { get; set; }
        /// <summary>
        /// 类型 0=菜单 1=权限
        /// </summary>
        public int MenuType { get; set; }   
    }
}
