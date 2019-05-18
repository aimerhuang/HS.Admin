using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Base;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;

namespace Hyt.DataAccess.Distribution
{
    /// <summary>
    /// 分销商信息维护接口层
    /// </summary>
    /// <remarks>
    /// 2013-09-04 郑荣华 创建
    /// </remarks>
    public abstract class IDsDealerAppDao : DaoBase<IDsDealerAppDao>
    {
        #region 操作

        /// <summary>
        /// 创建分销商
        /// </summary>
        /// <param name="model">分销商实体</param>
        /// <returns>新加的系统编号</returns>
        /// <remarks>
        /// 2013-09-04 郑荣华 创建
        /// </remarks>
        public abstract int Create(DsDealerApp model);

        /// <summary>
        /// 修改分销商
        /// </summary>
        /// <param name="model">分销商实体</param>
        /// <returns>受影响的行数</returns>
        /// <remarks>
        /// 2013-09-04 郑荣华 创建
        /// </remarks>
        public abstract int Update(DsDealerApp model);

        /// <summary>
        /// 分销商状态更新
        /// </summary>
        /// <param name="sysNo">分销商系统编号</param>
        /// <param name="status">分销商状态</param>
        /// <returns>受影响的行数</returns>
        /// <remarks>
        /// 2013-09-04 郑荣华 创建
        /// </remarks>
        public abstract int UpdateStatus(int sysNo, DistributionStatus.分销商App状态 status);

        #endregion

        #region 查询
        /// <summary>
        /// 通过过滤条件获取分销商列表
        /// </summary>
        /// <param name="filter">过滤条件</param>
        /// <returns>分销商列表</returns>
        ///<remarks>2013-09-03 周唐炬 创建</remarks>
        public abstract Pager<CBDsDealerApp> GetDealerList(ParaDsDealerAppFilter filter);

        /// <summary>
        /// 根据系统编号获取分销商信息
        /// </summary>
        /// <param name="sysNo">分销商系统编号</param>
        /// <returns>分销商信息</returns>
        /// <remarks>
        /// 2013-09-04 郑荣华 创建
        /// </remarks>
        public abstract DsDealerApp GetDsDealerApp(int sysNo);

        /// <summary>
        /// 用于更新检查分销商AppKey不重复，查询分销商App信息
        /// </summary>
        /// <param name="sysNo">用户系统编号</param>
        /// <param name="appKey">要排除的分销商AppKey</param>
        /// <returns>分销商App信息列表</returns>
        /// <remarks> 
        /// 2013-09-05 郑荣华 创建 
        /// </remarks>   
        public abstract IList<CBDsDealerApp> GetDsDealerAppList(int sysNo, string appKey);

        /// <summary>
        /// 通过分销商城类型系统编号获取AppKey列表
        /// </summary>
        /// <param name="mallType">mallType</param>
        /// <returns>分销商App信息列表</returns>
        /// <remarks> 
        /// 2014-07-24 余勇 创建 
        /// </remarks>   
        public abstract IList<CBDsDealerApp> GetListByMallType(int mallType);

        #endregion
    }
}
