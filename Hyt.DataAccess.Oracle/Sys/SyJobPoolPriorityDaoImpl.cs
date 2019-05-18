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
    public class SyJobPoolPriorityDaoImpl : ISyJobPoolPriorityDao
    {
        /// <summary>
        /// 插入实体记录
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns>返回系统编号</returns>
        /// <remarks>2014-02-28 余勇 创建</remarks>
        public override int Insert(SyJobPoolPriority model)
        {
            return Context.Insert("SyJobPoolPriority")
                  .Column("Priority", model.Priority)
                  .Column("PriorityCode", model.PriorityCode)
                  .Column("PriorityDescription", model.PriorityDescription)
                  .ExecuteReturnLastId<int>("SysNo");
        }

        /// <summary>
        /// 通过优先级获取实体信息
        /// </summary>
        /// <param name="priority">优先级</param>
        /// <returns>返回实体</returns>
        /// <remarks>2014-02-28 余勇 创建</remarks>
        public override SyJobPoolPriority GetByPriority(int priority)
        {
            return Context.Sql(@"SELECT * FROM SyJobPoolPriority WHERE priority=@priority")
                .Parameter("priority", priority).QuerySingle<SyJobPoolPriority>();
        }

        /// <summary>
        /// 通过系统编号获取实体信息
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>返回实体</returns>
        /// <remarks>2014-02-28 余勇 创建</remarks>
        public override SyJobPoolPriority Get(int sysNo)
        {
            return Context.Sql(@"SELECT * FROM SyJobPoolPriority WHERE SysNo=@SysNo")
                .Parameter("SysNo", sysNo).QuerySingle<SyJobPoolPriority>();
        }

        /// <summary>
        /// 通过优先级编码获取实体信息
        /// </summary>
        /// <param name="code">优先级编码</param>
        /// <returns>返回实体</returns>
        /// <remarks>2014-02-28 余勇 创建</remarks>
        public override SyJobPoolPriority GetByPriorityCode(string code)
        {
            return Context.Sql(@"SELECT * FROM SyJobPoolPriority WHERE PriorityCode=@PriorityCode")
                   .Parameter("PriorityCode", code).QuerySingle<SyJobPoolPriority>();
        }

        /// <summary>
        /// 通过SysNo删除该记录
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>影响行数</returns>
        /// <remarks>2014-02-28 余勇 创建</remarks>
        public override int Delete(int sysNo)
        {
            return Context.Sql(@"DELETE SyJobPoolPriority WHERE SysNo=@SysNo")
              .Parameter("SysNo", sysNo)
              .Execute();
        }

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="model">实体信息</param>
        /// <returns>返回影响行数</returns>
        /// <remarks>2014-02-28 余勇 创建</remarks>
        public override int Update(SyJobPoolPriority model)
        {
           return Context.Update("SyJobPoolPriority", model)
                  .AutoMap(o => o.SysNo)
                  .Where("SysNo", model.SysNo)
                  .Execute();
        }

        /// <summary>
        /// 获取所有记录
        /// </summary>
        /// <returns>list</returns>
        /// <remarks>2014-02-28 余勇 创建</remarks>
        public override IList<SyJobPoolPriority> SelectAll()
        {
            const string sql = @"select * from SyJobPoolPriority order by Priority desc";
            return Context.Sql(sql).QueryMany<SyJobPoolPriority>();
        }
    }
}
