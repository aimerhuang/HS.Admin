using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Sys;
using Hyt.Model;
namespace Hyt.DataAccess.Oracle.Sys
{

    /// <summary>
    /// 自动分配系统任务
    /// </summary>
    /// <remarks> 
    /// 2013-06-21 余勇 创建
    /// </remarks>
    public class SyJobDispatcherImpl : ISyJobDispatcher
    {
        /// <summary>
        /// 插入自动分配记录
        /// </summary>
        /// <param name="model">自动分配实体</param>
        /// <returns>返回系统编号</returns>
        /// <remarks>2013-06-21 余勇 创建</remarks>
        public override int InsertJobDispatcher(SyJobDispatcher model)
        {
            return Context.Insert("SyJobDispatcher", model)
                        .AutoMap(x => x.SysNo)
                        .ExecuteReturnLastId<int>("SysNo");
        }

        /// <summary>
        /// 通过用户编号跟任务类型获取订单分配信息
        /// </summary>
        /// <param name="userSysNo">用户编号</param>
        /// <param name="taskType">任务类型</param>
        /// <returns>返回任务分配实体</returns>
        /// <remarks>2013-06-21 余勇 创建</remarks>
        public override SyJobDispatcher GetJobDispatcher(int userSysNo, int taskType)
        {
            return Context.Sql(@"SELECT * FROM SyJobDispatcher WHERE UserSysNo=@UserSysNo AND TaskType=@TaskType")
                .Parameter("UserSysNo", userSysNo)
                .Parameter("TaskType", taskType).QuerySingle<SyJobDispatcher>();
        }

        /// <summary>
        /// 更新自动分配状态
        /// </summary>
        /// <param name="model">自动分配任务实体</param>
        /// <returns>返回系统编号</returns>
        /// <remarks>2013-06-21 余勇 创建</remarks>
        public override int UpdateStatus(SyJobDispatcher model)
        {
            return Context.Update("SyJobDispatcher")
                                .Column("Status", model.Status)
                                .Where("SysNo", model.SysNo)
                                .Execute();
        }

        /// <summary>
        /// 更新自动分配状态及数量
        /// </summary>
        /// <param name="model">自动分配任务实体</param>
        /// <returns>返回系统编号</returns>
        /// <remarks>2013-06-21 余勇 创建</remarks>
        public override int UpdateStatusQuantity(SyJobDispatcher model)
        {
            return Context.Update("SyJobDispatcher")
                                .Column("MaxTaskQuantity", model.MaxTaskQuantity)
                                .Column("Status", model.Status)
                                .Column("Prioritys", model.Prioritys)
                                .Where("SysNo", model.SysNo)
                                .Execute();
        }

        /// <summary>
        /// 通过SysNo删除该记录
        /// </summary>
        /// <param name="sysNo">自动分配编号</param>
        /// <returns>影响行数</returns>
        /// <remarks>2013-06-19 余勇 创建</remarks>
        public override int DeleteBySysNo(int sysNo)
        {
            return Context.Sql(@"DELETE SyJobDispatcher WHERE SysNo=@SysNo")
                .Parameter("SysNo", sysNo)
                .Execute();
        }

        /// <summary>
        /// 获取所有记录
        /// </summary>
        /// <returns>list</returns>
        /// <remarks>2013-10-08 朱家宏 创建</remarks>
        public override IList<SyJobDispatcher> SelectAll()
        {
            const string sql = @"select * from SyJobDispatcher order by sysNo desc";
            return Context.Sql(sql).QueryMany<SyJobDispatcher>();
        }

        /// <summary>
        /// 通过userSysNo删除记录
        /// </summary>
        /// <param name="userSysNo">用户编号</param>
        /// <returns>影响行数</returns>
        /// <remarks>2013-10-08 朱家宏 创建</remarks>
        public override int DeleteByUserSysNo(int userSysNo)
        {
            return Context.Sql(@"DELETE SyJobDispatcher WHERE userSysNo=@userSysNo")
                .Parameter("userSysNo", userSysNo)
                .Execute();
        }

        /// <summary>
        /// 获取不同用户编号的数据
        /// </summary>
        /// <returns>不同用户编号列表</returns>
        /// <remarks>2013-10-08 朱家宏 创建</remarks>
        public override IList<SyUser> SelectDistinctUsers()
        {
            const string sql = @"select * from syUser a where exists (select distinct t.usersysno from syjobdispatcher t where a.sysno=t.usersysno )";
            return Context.Sql(sql).QueryMany<SyUser>();
        }
    }
}
