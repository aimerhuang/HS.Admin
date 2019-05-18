using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Model;
using Hyt.DataAccess.Sys;

namespace Hyt.BLL.Sys
{
    public class SyPermissionBo:BOBase<SyPermissionBo>
    {
        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>新增记录编号</returns>
        /// <remarks>2013-08-08  朱成果 创建</remarks>
        public int Insert(SyPermission entity)
        {
            return ISyPermissionDao.Instance.Insert(entity);
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="source">来源:系统用户(10),用户组(20)</param>
        /// <param name="sourceSysNo">来源编号</param>
        /// <param name="target">>目标:菜单(10),角色(20),权限(30)</param>
        /// <param name="targetSysNo">目标编号</param>
        /// <remarks>2013-08-08  黄志勇 创建</remarks>
        public void Delete(int source, int sourceSysNo, int target, int targetSysNo)
        {
            ISyPermissionDao.Instance.Delete(source, sourceSysNo, target, targetSysNo);
        }
    }
}
