using System.Collections.Generic;
using Hyt.DataAccess.Sys;
using Hyt.Model;

namespace Hyt.DataAccess.Oracle.Sys
{
    /// <summary>
    /// 菜单权限
    /// </summary>
    /// <remarks>2013-08-01 朱家宏 创建</remarks>
    public class SyMenuPrivilegeDaoImpl : ISyMenuPrivilegeDao
    {
        /// <summary>
        /// insert
        /// </summary>
        /// <param name="model">model</param>
        /// <returns>sysNo</returns>
        /// <remarks>2013-08-01 朱家宏 创建</remarks>
        public override int Insert(SyMenuPrivilege model)
        {
            return Context.Insert("SyMenuPrivilege", model)
                                        .AutoMap(o => o.SysNo)
                                        .ExecuteReturnLastId<int>("SysNo");
        }

        /// <summary>
        /// delete
        /// </summary>
        /// <param name="sysNo">sysNo</param>
        /// <returns>boolean</returns>
        /// <remarks>2013-08-01 朱家宏 创建</remarks>
        public override bool Delete(int sysNo)
        {
            var r = Context.Delete("SyMenuPrivilege").Where("SysNo", sysNo).Execute();
            return r > 0;
        }

        /// <summary>
        /// 通过菜单sysNo删除菜单权限
        /// </summary>
        /// <param name="menuSysNo">菜单sysNo</param>
        /// <returns>boolean</returns>
        /// <remarks>2013-08-01 朱家宏 创建</remarks>
        public override bool DeleteByMenuSysNo(int menuSysNo)
        {
            var r = Context.Delete("SyMenuPrivilege").Where("menuSysNo", menuSysNo).Execute();
            return r > 0;
        }

        /// <summary>
        /// 通过权限编号查询
        /// </summary>
        /// <param name="privilegeSysNo">权限编号</param>
        /// <returns>列表</returns>
        /// <remarks>2013-08-05 朱家宏 创建</remarks>
        public override IList<SyMenuPrivilege> SelectAllByPrivilegeSysNo(int privilegeSysNo)
        {
            const string sql = @"select * from SyMenuPrivilege where privilegeSysNo=@0 order by LastUpdateDate desc";
            return Context.Sql(sql, privilegeSysNo).QueryMany<SyMenuPrivilege>();
        }
    }
}
