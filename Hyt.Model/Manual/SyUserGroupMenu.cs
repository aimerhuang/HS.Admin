using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Manual
{
    /// <summary>
    /// 用户组菜单,权限
    /// </summary>
    /// <remarks>2013-08-02 朱成果 创建</remarks>
   public class SyUserGroupMenu
   {
       /// <summary>
       /// 用户组编号
       /// </summary>
       public int UserGroupID { get; set; }
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
