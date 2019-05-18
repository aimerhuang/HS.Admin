using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Sys;
using Hyt.Model;
namespace Hyt.DataAccess.Oracle.Sys
{
    /// <summary>
    /// 用户仓库对应关系访问类
    /// </summary>
    /// <remarks> 
    /// 2013-06-13 郑荣华 创建
    /// </remarks>
    public class SystemUserWarehouseImpl:ISystemUserWarehouse
    {
        /// <summary>
        /// 根据仓库系统编号获取用户系统编号及名称
        /// </summary>
        /// <param name="warehouseSysNo">仓库系统编号</param>
        /// <returns>仓库下用户系统编号列表</returns>
        /// <remarks> 
        /// 2013-06-13 郑荣华 创建
        /// </remarks>
        public override IDictionary<string, string> GetSystemUserDict(int warehouseSysNo)
        {
            var list= Context.Sql("select a.userSysno,b.username from SystemUserWarehouse a inner join SystemUser b on a.usersysno=b.sysno where WarehouseSysno=@0", warehouseSysNo)
                                          .QueryMany<SyUser>();
            return  list.ToDictionary(s => s.SysNo.ToString(CultureInfo.InvariantCulture),s=>s.UserName);
        }
            
    }
}
