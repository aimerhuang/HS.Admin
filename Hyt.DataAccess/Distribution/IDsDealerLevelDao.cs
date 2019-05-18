using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Base;
using Hyt.Model;
using Hyt.Model.WorkflowStatus;

namespace Hyt.DataAccess.Distribution
{
    /// <summary>
    /// 分销商等级息等级维护接口层
    /// </summary>
    /// <remarks>
    /// 2013-09-04 郑荣华 创建
    /// </remarks>
    public abstract class IDsDealerLevelDao : DaoBase<IDsDealerLevelDao>
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
        public abstract int Create(DsDealerLevel model);

        /// <summary>
        /// 修改分销商等级
        /// </summary>
        /// <param name="model">分销商等级实体</param>
        /// <returns>受影响的行数</returns>
        /// <remarks>
        /// 2013-09-04 郑荣华 创建
        /// </remarks>
        public abstract int Update(DsDealerLevel model);

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
        public abstract int UpdateStatus(int sysNo, DistributionStatus.分销商等级状态 status, int lastUpdateBy, DateTime lastUpdateDate);
        
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
        public abstract DsDealerLevel GetDsDealerLevel(string levelName);

        /// <summary>
        /// 获取分销商等级信息 
        /// </summary>
        /// <param name="sysNo">等级系统编号</param>
        /// <returns>分销商等级息信息</returns>
        /// <remarks>
        /// 2013-09-04 郑荣华 创建 作等级名称重复性检查
        /// </remarks>
        public abstract DsDealerLevel GetDsDealerLevel(int sysNo);

        /// <summary>
        /// 查询所有分销商等级
        /// </summary>
        /// <returns>分销商等级列表</returns>
        /// <remarks> 
        /// 2013-09-04 郑荣华 创建 
        /// </remarks>      
        public abstract IList<DsDealerLevel> GetDsDealerLevelList();

        /// <summary>
        /// 查询分销商等级
        /// </summary>
        /// <param name="status">分销商等级状态</param>
        /// <returns>分销商等级列表</returns>
        /// <remarks> 
        /// 2013-11-04 郑荣华 创建 
        /// </remarks>      
        public abstract IList<DsDealerLevel> GetDsDealerLevelList(DistributionStatus.分销商等级状态 status);
        /// <summary>
        /// 获取分销商等级信息 
        /// </summary>
        /// <param name="DsDealerSysNo">分销商系统编号</param>
        /// <returns>分销商等级息信息</returns>
        /// <remarks>
        /// 2015-12-17 王耀发 创建 
        /// </remarks>
        public abstract DsDealerLevel GetLevelByDealerSysNo(int DsDealerSysNo);

        /// <summary>
        /// 获取分销商等级信息
        /// </summary>
        /// <param name="DsDealerSysNo">分销商系统编号</param>
        /// <returns>2016-09-06 罗远康 创建 </returns>
        public abstract DsDealerLevel GetDealerLevelByDealerSysNo(int DsDealerSysNo);
        #endregion
    }
}
