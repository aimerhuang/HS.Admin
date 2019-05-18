using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Model.Manual;

namespace Hyt.Model.Transfer
{
    /// <summary>
    /// 用户组扩展
    /// </summary>
    /// <remarks>2013-08-01 朱成果 创建</remarks> 
    public class CBSyUserGroup : SyUserGroup
    {
        /// <summary>
        /// 用户组对应菜单，权限
        /// </summary>
        List<SyUserGroupMenu> GroupMeuns { get; set; }
        /// <summary>
        /// 用户组角色
        /// </summary>
        List<SyUserGroupRole> GroupRoles { get; set; }
    }
}
