using Hyt.Model;
using System.Collections.Generic;
using Hyt.DataAccess.Base;
using Hyt.DataAccess.Sys;
namespace Hyt.DataAccess.Oracle.Sys
{
    /// <summary>
    /// 用户组
    /// </summary>
    /// <remarks>2013-07-30  朱成果 创建</remarks>
    public class SyUserGroupDaoImpl : ISyUserGroupDao
    {

        #region 数据记录增，删，改，查
        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>新增记录编号</returns>
        /// <remarks>2013-07-30  朱成果 创建</remarks>
        public override int Insert(SyUserGroup entity)
        {
            entity.SysNo = Context.Insert("SyUserGroup", entity)
                                        .AutoMap(o => o.SysNo)
                                        .ExecuteReturnLastId<int>("SysNo");
            return entity.SysNo;
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns></returns>
        /// <remarks>2013-07-30  朱成果 创建</remarks>
        public override void Update(SyUserGroup entity)
        {

            Context.Update("SyUserGroup", entity)
                   .AutoMap(o => o.SysNo,o=>o.CreatedDate)
                   .Where("SysNo", entity.SysNo)
                   .Execute();
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns>数据实体</returns>
        /// <remarks>2013-07-30  朱成果 创建</remarks>
        public override SyUserGroup GetEntity(int sysNo)
        {

            return Context.Sql("select * from SyUserGroup where SysNo=@SysNo")
                   .Parameter("SysNo", sysNo)
              .QuerySingle<SyUserGroup>();
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns></returns>
        /// <remarks>2013-07-30  朱成果 创建</remarks>
        public override void Delete(int sysNo)
        {
            Context.Sql("Delete from SyUserGroup where SysNo=@SysNo")
                 .Parameter("SysNo", sysNo)
            .Execute();
        }
        #endregion

        /// <summary>
        ///根据用户组名获取用户组
        /// </summary>
        /// <param name="groupName">用户组名</param>
        /// <returns>用户组</returns>
        /// <remarks>2013-08-01  朱成果 创建</remarks>
        public override SyUserGroup GetByGroupName(string groupName)
        {
            return Context.Sql("select * from SyUserGroup where GroupName=@GroupName")
                  .Parameter("GroupName", groupName)
             .QuerySingle<SyUserGroup>();
        }

        /// <summary>
        /// 获取全部用户组
        /// </summary>
        /// <returns>用户组列表</returns>
        /// <remarks>2013-08-05 黄志勇 创建</remarks>
        public override IList<SyUserGroup> GetAllSyGroup()
        {
            return Context.Sql("select * from SyUserGroup order by LASTUPDATEDATE desc").QueryMany<SyUserGroup>();
        }

        /// <summary>
        /// 检查当前用户组是否在使用
        /// </summary>
        /// <param name="sysNo">用户组编号</param>
        /// <returns></returns>
        /// <remarks>2013-08-05  朱成果 创建</remarks> 
        public override bool IsBeingUsed(int sysNo)
        {
            //            string sqlStr= @"select 1 as p from dual
            //where exists(select 1 from SyGroupUser where GroupSysNo=:GroupSysNo)
            //or exists(select 1 from SyPermission where SourceSysNo=:SourceSysNo and Source=:Source)";
            //          var i=  Context.Sql(sqlStr)
            //                .Parameter("GroupSysNo", sysNo)
            //                .Parameter("SourceSysNo", sysNo)
            //                .Parameter("Source", (int)Hyt.Model.WorkflowStatus.SystemStatus.授权来源.用户组)
            //                .QuerySingle<int>();
            //          return i == 1;

            //string sqlStr = @"select 1 as p from dual
            //where exists(select 1 from SyGroupUser where GroupSysNo=@GroupSysNo)";
            //var i = Context.Sql(sqlStr)
            //      .Parameter("GroupSysNo", sysNo)
            //      .QuerySingle<int>();

            int TotalRows = Context.Select<int>("count(sysno)")
                               .From(@"SyGroupUser")
                              .Where(@"(GroupSysNo = @GroupSysNo)")
                              .Parameter("GroupSysNo", sysNo)
                              .QuerySingle();
            return TotalRows > 0;

        }
    }
}
