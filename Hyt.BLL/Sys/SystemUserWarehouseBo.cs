using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Model;
using Hyt.DataAccess.Sys;
namespace Hyt.BLL.Sys
{
    /// <summary>
    /// 用户仓库对应关系接口类
    /// </summary>
    /// <remarks> 
    /// 2013-06-13 郑荣华 创建
    /// </remarks>
    public class SystemUserWarehouseBo:BOBase<SystemUserWarehouseBo>
    {
        /// <summary>
        /// 根据仓库系统编号获取用户系统编号
        /// </summary>
        /// <param name="warehouseSysNo">仓库系统编号</param>
        /// <returns>仓库下用户系统编号列表</returns>
        /// <remarks> 
        /// 2013-06-13 郑荣华 创建
        /// </remarks>
        public IDictionary<string, string> GetUserSysno(int warehouseSysNo)
        {
            return ISystemUserWarehouse.Instance.GetSystemUserDict(warehouseSysNo);
        }
    }
}
