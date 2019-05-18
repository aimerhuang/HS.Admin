using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Manual
{
    /// <summary>
    /// 角色菜单,权限
    /// </summary>
    /// <remarks>2013-08-07 余勇 创建</remarks>
    public class SyRoleMenuPrivilege
    {
        /// <summary>
        /// 角色编号
        /// </summary>
        public int RoleSysNo { get; set; }
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
