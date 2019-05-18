using System.Collections.Generic;
using Hyt.DataAccess.Sys;
using Hyt.Model;

namespace Hyt.DataAccess.Oracle.Sys
{
    /// <summary>
    /// 角色权限
    /// </summary>
    /// <remarks>2013-08-01 朱家宏 创建</remarks>
    public class SyRolePrivilegeDaoImpl : ISyRolePrivilegeDao
    {
        /// <summary>
        /// insert
        /// </summary>
        /// <param name="model">model</param>
        /// <returns>sysNo</returns>
        /// <remarks>2013-08-01 朱家宏 创建</remarks>
        public override int Insert(SyRolePrivilege model)
        {
            return Context.Insert("SyRolePrivilege", model)
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
            var r = Context.Delete("SyRolePrivilege").Where("SysNo", sysNo).Execute();
            return r > 0;
        }

        /// <summary>
        /// 通过角色编号删除角色权限
        /// </summary>
        /// <param name="roleSysNo">roleSysNo</param>
        /// <returns>boolean</returns>
        /// <remarks>2013-08-07 余勇 创建</remarks>
        public override bool DeleteByRoleSysNo(int roleSysNo)
        {
            var r = Context.Delete("SyRolePrivilege").Where("RoleSysNo", roleSysNo).Execute();
            return r > 0;
        }

        /// <summary>
        /// 通过权限编号查询
        /// </summary>
        /// <param name="privilegeSysNo">权限编号</param>
        /// <returns>列表</returns>
        /// <remarks>2013-08-05 朱家宏 创建</remarks>
        public override IList<SyRolePrivilege> SelectAllByPrivilegeSysNo(int privilegeSysNo)
        {
            const string sql = @"select * from SyRolePrivilege where privilegeSysNo=@0 order by LastUpdateDate desc";
            return Context.Sql(sql, privilegeSysNo).QueryMany<SyRolePrivilege>();
        }

        /// <summary>
        /// 通过角色编号查询所属权限
        /// </summary>
        /// <param name="roleSysNo">角色编号</param>
        /// <returns>IList</returns>
        /// <remarks>2013-08-07 余勇 创建</remarks>
        public override IList<SyRolePrivilege> GetListByRoleSysNo(int roleSysNo)
        {
            const string sql = @"select * from SyRolePrivilege where RoleSysNo=@0 order by CreatedDate desc";
            return Context.Sql(sql, roleSysNo).QueryMany<SyRolePrivilege>();
        }
    }
}
