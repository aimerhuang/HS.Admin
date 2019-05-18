using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Model.Manual;

namespace Hyt.Model.Transfer
{
    /// <summary>
    /// 用户资料及用户权限
    /// </summary>
    /// <remarks>2013-08-01 朱成果 创建</remarks>
    public class CBSyUser:SyUser
    {
        /// <summary>
        /// 用户组
        /// </summary>
        public List<SyGroupUser> GroupUsers { get; set; }
        /// <summary>
        /// 用户仓库
        /// </summary>
        public List<SyUserWarehouse> UserWarehouses { get; set; }
        /// <summary>
        /// 用户菜单,用户权限
        /// </summary>
        public List<SyUserMenu> UserMeuns { get; set; }
        /// <summary>
        /// 用户角色
        /// </summary>
        public List<SyUserRole> UserRoles { get; set; }

     
    }
}
