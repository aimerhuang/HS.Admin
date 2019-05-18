using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Instrumentation;
using System.Text;
using Hyt.DataAccess.MallSeller;
using Hyt.Infrastructure.Pager;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;
using Hyt.DataAccess.Distribution;

namespace Hyt.BLL.Distribution
{
    /// <summary>
    /// 分销商信息维护业务层
    /// </summary>
    /// <remarks>
    /// 2013-09-04 郑荣华 创建
    /// </remarks>
    public class DsDealerAppBo : BOBase<DsDealerAppBo>
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
        public  int Create(DsDealerApp model)
        {
            return IDsDealerAppDao.Instance.Create(model);
        }

        /// <summary>
        /// 修改分销商App
        /// </summary>
        /// <param name="model">分销商App实体</param>
        /// <returns>受影响的行数</returns>
        /// <remarks>
        /// 2014-05-06 余勇 创建
        /// </remarks>
        public  int Update(DsDealerApp model)
        {
            return IDsDealerAppDao.Instance.Update(model);
        }

        /// <summary>
        /// 分销商App状态更新
        /// </summary>
        /// <param name="sysNo">分销商App系统编号</param>
        /// <returns>受影响的行数</returns>
        /// <remarks>
        /// 2014-05-06 余勇 创建
        /// </remarks>
        public  int UpdateStatus(int sysNo)
        {
            var model = IDsDealerAppDao.Instance.GetDsDealerApp(sysNo);
            var changedStatus = model.Status == (int)DistributionStatus.分销商App状态.禁用
                                  ? DistributionStatus.分销商App状态.启用
                                  : DistributionStatus.分销商App状态.禁用;
            return IDsDealerAppDao.Instance.UpdateStatus(sysNo, changedStatus);
        }
        #endregion

        #region 查询
        /// <summary>
        /// 通过过滤条件获取分销商App列表
        /// </summary>
        /// <param name="filter">过滤条件</param>
        /// <returns>分销商App列表</returns>
        /// 2014-05-06 余勇 创建
        public Pager<CBDsDealerApp> GetDealerList(ParaDsDealerAppFilter filter)
        {
            return IDsDealerAppDao.Instance.GetDealerList(filter);
        }

        /// <summary>
        /// 根据系统编号获取分销商App信息
        /// </summary>
        /// <param name="sysNo">分销商App系统编号</param>
        /// <returns>分销商App信息</returns>
        /// <remarks>
        /// 2014-05-06 余勇 创建
        /// </remarks>
        public DsDealerApp GetDsDealerApp(int sysNo)
        {
            return IDsDealerAppDao.Instance.GetDsDealerApp(sysNo);
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
        public  IList<CBDsDealerApp> GetDsDealerAppList(int sysNo, string appKey)
        {
            return IDsDealerAppDao.Instance.GetDsDealerAppList(sysNo, appKey);
        }

        /// <summary>
        /// 通过分销商城类型系统编号获取AppKey列表
        /// </summary>
        /// <param name="mallType">mallType</param>
        /// <returns>分销商App信息列表</returns>
        /// <remarks> 
        /// 2014-07-24 余勇 创建 
        /// </remarks>   
        public IList<CBDsDealerApp> GetListByMallType(int mallType)
        {
            return IDsDealerAppDao.Instance.GetListByMallType(mallType);
        }

        /// <summary>
        /// 更新appkey关联数量
        /// </summary>
        /// <param name="sysNo">appKey系统编号</param>
        /// <returns>void</returns>
        /// <remarks> 
        /// 2014-07-24 余勇 创建 
        /// </remarks>   
        public void UpdateAppKeyUseNum(int sysNo)
        {
            var appModel = GetDsDealerApp(sysNo);
            if (appModel != null)
            {
                appModel.HasRelevance = DsDealerMallBo.Instance.GetAppKeyUseNum(sysNo);
                Update(appModel);
            }
        }
        #endregion
    }
}
