using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Distribution;
using Hyt.Model;
using Hyt.Model.WorkflowStatus;

namespace Hyt.DataAccess.Oracle.Distribution
{
    /// <summary>
    /// 分销商等级息等级维护数据访问层
    /// </summary>
    /// <remarks>
    /// 2013-09-04 郑荣华 创建
    /// </remarks>
    public class DsDealerLevelDaoImpl : IDsDealerLevelDao
    {
        #region 操作

        /// <summary>
        /// 创建分销商等级
        /// </summary>
        /// <param name="model">分销商等级实体</param>
        /// <returns>新加的系统编号</returns>
        /// <remarks>
        /// 2013-09-04 郑荣华 创建
        /// </remarks>
        public override int Create(DsDealerLevel model)
        {
            return Context.Insert("DsDealerLevel", model)
                         .AutoMap(x => x.SysNo)
                         .ExecuteReturnLastId<int>("SysNo");
        }

        /// <summary>
        /// 修改分销商等级
        /// </summary>
        /// <param name="model">分销商等级实体</param>
        /// <returns>受影响的行数</returns>
        /// <remarks>
        /// 2013-09-04 郑荣华 创建
        /// </remarks>
        public override int Update(DsDealerLevel model)
        {
            return Context.Update("DsDealerLevel", model)
                         .AutoMap(x => x.SysNo, x => x.CreatedBy, x => x.CreatedDate)
                         .Where(x => x.SysNo)
                         .Execute();
        }

        /// <summary>
        /// 分销商等级状态更新
        /// </summary>
        /// <param name="sysNo">分销商等级系统编号</param>
        /// <param name="status">分销商等级息状态</param>
        /// <param name="lastUpdateBy">最后更新人</param>
        /// <param name="lastUpdateDate">最后更新时间</param>
        /// <returns>受影响的行数</returns>
        /// <remarks>
        /// 2013-09-04 郑荣华 创建
        /// </remarks>
        public override int UpdateStatus(int sysNo, DistributionStatus.分销商等级状态 status, int lastUpdateBy, DateTime lastUpdateDate)
        {
            return Context.Sql("update DsDealerLevel set status=@0,lastupdateby=@1,lastupdatedate=@2 where sysno=@3", (int)status, lastUpdateBy, lastUpdateDate, sysNo)
                           .Execute();
        }
        #endregion

        #region 查询

        /// <summary>
        /// 获取分销商等级信息 
        /// </summary>
        /// <param name="levelName">等级名称</param>
        /// <returns>分销商等级息信息</returns>
        /// <remarks>
        /// 2013-09-04 郑荣华 创建 作等级名称重复性检查
        /// </remarks>
        public override DsDealerLevel GetDsDealerLevel(string levelName)
        {
            const string sql = @"select t.* from DsDealerLevel t where t.levelName=@0";

            return Context.Sql(sql, levelName)
                          .QuerySingle<DsDealerLevel>();
        }

        /// <summary>
        /// 获取分销商等级信息 
        /// </summary>
        /// <param name="sysNo">等级系统编号</param>
        /// <returns>分销商等级息信息</returns>
        /// <remarks>
        /// 2013-09-04 郑荣华 创建 作等级名称重复性检查
        /// </remarks>
        public override DsDealerLevel GetDsDealerLevel(int sysNo)
        {
            const string sql = @"select t.* from DsDealerLevel t where t.sysno=@0";

            return Context.Sql(sql, sysNo)
                          .QuerySingle<DsDealerLevel>();
        }

        /// <summary>
        /// 查询所有分销商等级
        /// </summary>
        /// <returns>分销商等级列表</returns>
        /// <remarks> 
        /// 2013-09-04 郑荣华 创建 
        /// </remarks>      
        public override IList<DsDealerLevel> GetDsDealerLevelList()
        {
            const string sql = @"select t.* from DsDealerLevel t ";
            return Context.Sql(sql)
                          .QueryMany<DsDealerLevel>();
        }

        /// <summary>
        /// 查询分销商等级
        /// </summary>
        /// <param name="status">分销商等级状态</param>
        /// <returns>分销商等级列表</returns>
        /// <remarks> 
        /// 2013-11-04 郑荣华 创建 
        /// </remarks>      
        public override IList<DsDealerLevel> GetDsDealerLevelList(DistributionStatus.分销商等级状态 status)
        {
            const string sql = @"select t.* from DsDealerLevel t where status=@0";
            return Context.Sql(sql, (int)status)
                          .QueryMany<DsDealerLevel>();
        }

        /// <summary>
        /// 获取分销商等级信息 
        /// </summary>
        /// <param name="DsDealerSysNo">分销商系统编号</param>
        /// <returns>分销商等级息信息</returns>
        /// <remarks>
        /// 2015-12-17 王耀发 创建 
        /// </remarks>
        public override DsDealerLevel GetLevelByDealerSysNo(int DsDealerSysNo)
        {
            const string sql = @"select * from DsDealerLevel where SysNo = (select LevelSysNo from DsDealer where SysNo = @0)";
            return Context.Sql(sql, DsDealerSysNo)
                          .QuerySingle<DsDealerLevel>();
        }

        /// <summary>
        /// 获取分销商等级信息
        /// </summary>
        /// <param name="DsDealerSysNo">分销商系统编号</param>
        /// <returns>2016-09-06 罗远康 创建 </returns>
        public override DsDealerLevel GetDealerLevelByDealerSysNo(int DsDealerSysNo)
        {
            const string sql = @"SELECT DL.SalePriceUpper ,DL.SalePriceLower 
                                 FROM DsDealer DD LEFT JOIN DsDealerLevel DL ON DD.LevelSysNo=DL.SysNo
                                 WHERE DD.SysNo=@0";
            return Context.Sql(sql,DsDealerSysNo)
                          .QuerySingle<DsDealerLevel>();
        }
        #endregion
    }
}
