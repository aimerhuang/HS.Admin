using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Model;
using Hyt.Model.WorkflowStatus;
using Hyt.DataAccess.Distribution;

namespace Hyt.BLL.Distribution
{
    /// <summary>
    /// 分销商等级息等级维护业务层
    /// </summary>
    /// <remarks>
    /// 2013-09-04 郑荣华 创建
    /// </remarks>
    public class DsDealerLevelBo : BOBase<DsDealerLevelBo>
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
        public int Create(DsDealerLevel model)
        {
            var sysno = IDsDealerLevelDao.Instance.Create(model);
            Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台, "创建分销商等级", LogStatus.系统日志目标类型.分销商等级, sysno);
            return sysno;
        }

        /// <summary>
        /// 修改分销商等级
        /// </summary>
        /// <param name="model">分销商等级实体</param>
        /// <returns>受影响的行数</returns>
        /// <remarks>
        /// 2013-09-04 郑荣华 创建
        /// </remarks>
        public int Update(DsDealerLevel model)
        {
            var r = IDsDealerLevelDao.Instance.Update(model);
            if (r > 0)
                Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台, "修改分销商等级", LogStatus.系统日志目标类型.分销商等级, model.SysNo);
            return r;
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
        public int UpdateStatus(int sysNo, DistributionStatus.分销商等级状态 status, int lastUpdateBy, DateTime lastUpdateDate)
        {
            var r = IDsDealerLevelDao.Instance.UpdateStatus(sysNo, status, lastUpdateBy, lastUpdateDate);
            if (r > 0)
                Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台, "修改分销商等级状态", LogStatus.系统日志目标类型.分销商等级, sysNo);
            return r;
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
        public DsDealerLevel GetDsDealerLevel(string levelName)
        {
            return IDsDealerLevelDao.Instance.GetDsDealerLevel(levelName);
        }

        /// <summary>
        /// 获取分销商等级信息 
        /// </summary>
        /// <param name="sysNo">等级系统编号</param>
        /// <returns>分销商等级息信息</returns>
        /// <remarks>
        /// 2013-09-04 郑荣华 创建 作等级名称重复性检查
        /// </remarks>
        public DsDealerLevel GetDsDealerLevel(int sysNo)
        {
            return IDsDealerLevelDao.Instance.GetDsDealerLevel(sysNo);
        }

        /// <summary>
        /// 查询所有分销商等级
        /// </summary>
        /// <param></param>
        /// <returns>分销商等级列表</returns>
        /// <remarks> 
        /// 2013-09-04 郑荣华 创建 
        /// </remarks>      
        public IList<DsDealerLevel> GetDsDealerLevelList()
        {
            return IDsDealerLevelDao.Instance.GetDsDealerLevelList();
        }

        /// <summary>
        /// 查询分销商等级
        /// </summary>
        /// <param name="status">分销商等级状态</param>
        /// <returns>分销商等级列表</returns>
        /// <remarks> 
        /// 2013-11-04 郑荣华 创建 
        /// </remarks>      
        public IList<DsDealerLevel> GetDsDealerLevelList(DistributionStatus.分销商等级状态 status)
        {
            return IDsDealerLevelDao.Instance.GetDsDealerLevelList(status);
        }
        /// <summary>
        /// 获取分销商等级信息 
        /// </summary>
        /// <param name="DsDealerSysNo">分销商系统编号</param>
        /// <returns>分销商等级息信息</returns>
        /// <remarks>
        /// 2015-12-17 王耀发 创建 
        public DsDealerLevel GetLevelByDealerSysNo(int DsDealerSysNo)
        {
            return IDsDealerLevelDao.Instance.GetLevelByDealerSysNo(DsDealerSysNo);
        }

        /// <summary>
        /// 获取分销商等级信息
        /// </summary>
        /// <param name="DsDealerSysNo">分销商系统编号</param>
        /// <returns>2016-09-06 罗远康 创建 </returns>
        public DsDealerLevel GetDealerLevelByDealerSysNo(int DsDealerSysNo)
        {
            return IDsDealerLevelDao.Instance.GetDealerLevelByDealerSysNo(DsDealerSysNo);
        }
        #endregion

    }
}
