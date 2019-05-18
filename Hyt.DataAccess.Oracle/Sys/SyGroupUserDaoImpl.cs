using Hyt.Model;
using System.Collections.Generic;
using Hyt.DataAccess.Sys;
using Hyt.Model.WorkflowStatus;

namespace Hyt.DataAccess.Oracle.Sys
{
    /// <summary>
    /// 用户对应用户组
    /// </summary>
    /// <remarks>2013-07-30  朱成果 创建</remarks>
    public class SyGroupUserDaoImpl : ISyGroupUserDao
    {

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>新增记录编号</returns>
        /// <remarks>2013-07-30  朱成果 创建</remarks>
        public override int Insert(SyGroupUser entity)
        {
            entity.SysNo = Context.Insert("SyGroupUser", entity)
                                        .AutoMap(o => o.SysNo)
                                        .ExecuteReturnLastId<int>("SysNo");
            return entity.SysNo;
        }

        /// <summary>
        /// 根据用户编号删除数据
        /// </summary>
        /// <param name="userSysNo">用户编号</param>
        /// <returns>影响的行数</returns>
        /// <remarks>2013-07-30  朱成果 创建</remarks>
        public override int DeleteByUserSysNo(int userSysNo)
        {
            return Context.Sql("delete from SyGroupUser where UserSysNo=@UserSysNo")
                           .Parameter("UserSysNo", userSysNo).Execute();
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns></returns>
        /// <remarks>2013-07-30  朱成果 创建</remarks>
        public override void Delete(int sysNo)
        {
            Context.Sql("Delete from SyGroupUser where SysNo=@SysNo")
                 .Parameter("SysNo", sysNo)
            .Execute();
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="userSysNo">用户编号</param>
        /// <param name="groupSysNo">用户组编号</param>
        /// <returns></returns>
        /// <remarks>2013-08-08  黄志勇 创建</remarks>
        public override void Delete(int userSysNo, int groupSysNo)
        {
            Context.Sql("delete from SyGroupUser where UserSysNo=@UserSysNo and GroupSysNo=@GroupSysNo")
                           .Parameter("UserSysNo", userSysNo)
                           .Parameter("GroupSysNo", groupSysNo)
                           .Execute();
        }

        /// <summary>
        /// 根据用户编号获取用户分组
        /// </summary>
        /// <param name="userSysNo">用户编号</param>
        /// <returns></returns>
        /// <remarks>2013-08-06  黄志勇 创建</remarks>
        public override IList<SyGroupUser> GetGroupUser(int userSysNo)
        {
            return Context.Sql("select * from SyGroupUser where UserSysNo=@0", userSysNo)
                          .QueryMany<SyGroupUser>();
        }

        /// <summary>
        /// 组是否包含该用户
        /// </summary>
        /// <param name="groupSysNo">组系统编号</param>
        /// <param name="userSysNo">用户系统编号</param>
        /// <returns>true:用户组存在该用户;false:用户组不存在该用户</returns>
        /// <remarks>2013-10-22 吴文强 创建</remarks>
        public override bool GroupContainsUser(int groupSysNo, int userSysNo)
        {
            string strSql = @"
                                select gu.* from SyGroupUser gu
                                  inner join SyUserGroup g on gu.groupsysno = g.sysno
                                where gu.groupsysno = @groupsysno
                                    and gu.usersysno = @usersysno 
                                    and g.status = @status
                            ";

            return Context.Sql(strSql)
                          .Parameter("groupsysno", groupSysNo)
                          .Parameter("usersysno", userSysNo)
                          .Parameter("status", SystemStatus.用户组状态.启用)
                          .QueryMany<SyGroupUser>().Count > 0;
        }

    }
}
