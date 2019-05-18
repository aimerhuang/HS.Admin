using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Distribution;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;

namespace Hyt.DataAccess.Oracle.Distribution
{
    /// <summary>
    /// 分销商App信息维护数据访问层
    /// </summary>
    /// <remarks>
    /// 2014-05-06 余勇 创建
    /// </remarks>
    public class DsDealerAppDaoImpl : IDsDealerAppDao
    {
        #region 操作

        /// <summary>
        /// 创建分销商App
        /// </summary>
        /// <param name="model">分销商App实体</param>
        /// <returns>新加的系统编号</returns>
        /// <remarks>
        /// 2014-05-06 余勇 创建
        /// </remarks>
        public override int Create(DsDealerApp model)
        {
            return Context.Insert("DsDealerApp", model)
                         .AutoMap(x => x.SysNo)
                         .ExecuteReturnLastId<int>("SysNo");
        }

        /// <summary>
        /// 修改分销商App
        /// </summary>
        /// <param name="model">分销商App实体</param>
        /// <returns>受影响的行数</returns>
        /// <remarks>
        /// 2014-05-06 余勇 创建
        /// </remarks>
        public override int Update(DsDealerApp model)
        {
            return Context.Update("DsDealerApp", model)
                          .AutoMap(x => x.SysNo)
                          .Where(x => x.SysNo)
                          .Execute();
        }

        /// <summary>
        /// 分销商App状态更新
        /// </summary>
        /// <param name="sysNo">分销商App系统编号</param>
        /// <param name="status">分销商App状态</param>
        /// <returns>受影响的行数</returns>
        /// <remarks>
        /// 2014-05-06 余勇 创建
        /// </remarks>
        public override int UpdateStatus(int sysNo, DistributionStatus.分销商App状态 status)
        {
            return Context.Sql("update DsDealerApp set status=@0 where sysno=@1", (int)status,sysNo)
                           .Execute();
        }
        #endregion

        #region 查询
        /// <summary>
        /// 通过过滤条件获取分销商App列表
        /// </summary>
        /// <param name="filter">过滤条件</param>
        /// <returns>分销商App列表</returns>
        /// 2014-05-06 余勇 创建
        public override Pager<CBDsDealerApp> GetDealerList(ParaDsDealerAppFilter filter)
        {
            const string sql = @"(SELECT A.*
	                                ,B.MallName
                                FROM DsDealerApp A
                                LEFT JOIN DsMallType B ON A.MallTypeSysNo = B.SysNo
                                    ) tb";

            var dataList = Context.Select<CBDsDealerApp>("tb.*").From(sql);
            var dataCount = Context.Select<int>("count(1)").From(sql);

            var pager = new Pager<CBDsDealerApp>()
            {
                Rows = dataList.OrderBy(@"tb.SysNo desc").Paging(filter.CurrentPage, filter.PageSize).QueryMany(),
                TotalRows = dataCount.QuerySingle()
            };
            return pager;
        }

        /// <summary>
        /// 根据系统编号获取分销商App信息
        /// </summary>
        /// <param name="sysNo">分销商App系统编号</param>
        /// <returns>分销商App信息</returns>
        /// <remarks>
        /// 2014-05-06 余勇 创建
        /// </remarks>
        public override DsDealerApp GetDsDealerApp(int sysNo)
        {
            const string sql = @"SELECT *
                                FROM DsDealerApp 
                                where sysno=@0";
            return Context.Sql(sql, sysNo)
                          .QuerySingle<DsDealerApp>();
        }

        /// <summary>
        /// 用于更新检查分销商AppKey不重复，查询分销商App信息
        /// </summary>
        /// <param name="sysNo">用户系统编号</param>
        /// <param name="appKey">要排除的分销商AppKey</param>
        /// <returns>分销商App信息列表</returns>
        /// <remarks> 
        /// 2013-09-05 郑荣华 创建 
        /// </remarks>   
        public override IList<CBDsDealerApp> GetDsDealerAppList(int sysNo, string appKey)
        {
            const string sql = @"select t.* from DsDealerApp t where t.SysNo<>@0 and t.AppKey=@1";
            return Context.Sql(sql, sysNo, appKey)
                          .QueryMany<CBDsDealerApp>();
        }

        /// <summary>
        /// 通过分销商城类型系统编号获取AppKey列表
        /// </summary>
        /// <param name="mallType">mallType</param>
        /// <returns>分销商App信息列表</returns>
        /// <remarks> 
        /// 2014-07-24 余勇 创建 
        /// </remarks>   
        public override IList<CBDsDealerApp> GetListByMallType(int mallType)
        {
            const string sql = @"select t.* from DsDealerApp t where t.Status=1 and t.MallTypeSysNo=@0";
            return Context.Sql(sql, mallType)
                          .QueryMany<CBDsDealerApp>();
        }
        #endregion
    }
}
