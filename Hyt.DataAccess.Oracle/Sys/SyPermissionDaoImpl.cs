using Hyt.Model;
using System.Collections.Generic;
using Hyt.DataAccess.Base;
using Hyt.DataAccess.Sys;
using System;
namespace Hyt.DataAccess.Oracle.Sys
{
    /// <summary>
    /// 授权
    /// </summary>
    /// <remarks>2013-08-01  朱成果 创建</remarks>
    public class SyPermissionDaoImpl : ISyPermissionDao
    {
        #region 数据记录增，删，改，查
        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>新增记录编号</returns>
        /// <remarks>2013-08-01  朱成果 创建</remarks>
        public override int Insert(SyPermission entity)
        {
            if (entity.EffectiveDate == DateTime.MinValue)
            {
                entity.EffectiveDate = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            }
            if (entity.ExpirationDate == DateTime.MinValue)
            {
                entity.ExpirationDate = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            }
            entity.SysNo = Context.Insert("SyPermission", entity)
                                        .AutoMap(o => o.SysNo)
                                        .ExecuteReturnLastId<int>("SysNo");
            return entity.SysNo;
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns></returns>
        /// <remarks>2013-08-01  朱成果 创建</remarks>
        public override void Update(SyPermission entity)
        {

            Context.Update("SyPermission", entity)
                   .AutoMap(o => o.SysNo)
                   .Where("SysNo", entity.SysNo)
                   .Execute();
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns>数据实体</returns>
        /// <remarks>2013-08-01  朱成果 创建</remarks>
        public override SyPermission GetEntity(int sysNo)
        {

            return Context.Sql("select * from SyPermission where SysNo=@SysNo")
                   .Parameter("SysNo", sysNo)
              .QuerySingle<SyPermission>();
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <remarks>2013-08-01  朱成果 创建</remarks>
        public override void Delete(int sysNo)
        {
            Context.Sql("Delete from SyPermission where SysNo=@SysNo")
                 .Parameter("SysNo", sysNo)
            .Execute();
        }
        #endregion

        /// <summary>
        /// 获取授权列表
        /// </summary>
        /// <param name="source">来源:系统用户(10),用户组(20)</param>
        /// <param name="sourceSysNo">来源编号</param>
        /// <returns>获取授权列表</returns>
        /// <remarks>2013-08-01  朱成果 创建</remarks>
        public override List<SyPermission> GetList(int source, int sourceSysNo)
        {
            return Context.Sql("select * from SyPermission where Source=@Source and SourceSysNo=@SourceSysNo")
                  .Parameter("Source", source)
                  .Parameter("SourceSysNo", sourceSysNo).QueryMany<SyPermission>();
        }

        /// <summary>
        /// 获取授权列表
        /// </summary>
        /// <param name="source">来源:系统用户(10),用户组(20)</param>
        /// <param name="sourceSysNo">来源编号</param>
        /// <param name="target">目标:菜单(10),角色(20),权限(30)</param>
        /// <returns>获取授权列表</returns>
        /// <remarks>2013-08-01  朱成果 创建</remarks>
        public override List<SyPermission> GetList(int source, int sourceSysNo, int target)
        {
            return Context.Sql("select * from SyPermission where Source=@Source and SourceSysNo=@SourceSysNo and Target=@Target")
               .Parameter("Source", source)
               .Parameter("SourceSysNo", sourceSysNo)
               .Parameter("Target", target)
               .QueryMany<SyPermission>();
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="source">来源:系统用户(10),用户组(20)</param>
        /// <param name="sourceSysNo">来源编号</param>
        /// <returns></returns>
        /// <remarks>2013-08-01  朱成果 创建</remarks>
        public override int Delete(int source, int sourceSysNo)
        {
            return Context.Sql("delete from SyPermission where Source=@Source and SourceSysNo=@SourceSysNo")
             .Parameter("Source", source)
             .Parameter("SourceSysNo", sourceSysNo).Execute();
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="source">来源:系统用户(10),用户组(20)</param>
        /// <param name="sourceSysNo">来源编号</param>
        /// <param name="target">目标:菜单(10),角色(20),权限(30)</param>
        /// <returns></returns>
        /// <remarks>2013-08-01  朱成果 创建</remarks>
        public override int Delete(int source, int sourceSysNo, int target)
        {
            return Context.Sql("delete from SyPermission where Source=@Source and SourceSysNo=@SourceSysNo and Target=@Target")
            .Parameter("Source", source)
            .Parameter("SourceSysNo", sourceSysNo)
            .Parameter("Target", target)
            .Execute();
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="source">来源:系统用户(10),用户组(20)</param>
        /// <param name="sourceSysNo">来源编号</param>
        /// <param name="target">目标:菜单(10),角色(20),权限(30)</param>
        /// <param name="targetSysNo">目标编号</param>
        /// <returns></returns>
        /// <remarks>2013-08-08  黄志勇 创建</remarks>
        public override int Delete(int source, int sourceSysNo, int target, int targetSysNo)
        {
            return Context.Sql("delete from SyPermission where Source=@Source and SourceSysNo=@SourceSysNo and Target=@Target and TargetSysNo=@TargetSysNo")
            .Parameter("Source", source)
            .Parameter("SourceSysNo", sourceSysNo)
            .Parameter("Target", target)
            .Parameter("TargetSysNo", targetSysNo)
            .Execute();
        }
    }
}
