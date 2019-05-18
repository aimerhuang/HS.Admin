using System.Collections.Generic;
using Hyt.Model;
using Hyt.Model.WorkflowStatus;
using Hyt.DataAccess.Distribution;

namespace Hyt.BLL.Distribution
{
    /// <summary>
    ///分销商城类型维护业务层
    /// </summary>
    /// <remarks>
    /// 2013-09-04 郑荣华 创建
    /// </remarks>
    public class DsMallTypeBo : BOBase<DsMallTypeBo>
    {

        #region 操作

        /// <summary>
        /// 创建分销商城类型
        /// </summary>
        /// <param name="model">分销商城类型实体</param>
        /// <returns>新加的系统编号</returns>
        /// <remarks>
        /// 2013-09-04 郑荣华 创建
        /// </remarks>
        public int Create(DsMallType model)
        {
            var r = IDsMallTypeDao.Instance.Create(model);
            Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台, "创建分销商商城类型", LogStatus.系统日志目标类型.分销商商城类型, r);
            return r;
        }

        /// <summary>
        /// 修改分销商城类型
        /// </summary>
        /// <param name="model">分销商城类型实体</param>
        /// <returns>受影响的行数</returns>
        /// <remarks>
        /// 2013-09-04 郑荣华 创建
        /// </remarks>
        public int Update(DsMallType model)
        {
            var r = IDsMallTypeDao.Instance.Update(model);
            if (r > 0)
                Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台, "修改分销商商城类型", LogStatus.系统日志目标类型.分销商商城类型, model.SysNo);
            return r;
        }

        /// <summary>
        /// 分销商城类型状态更新
        /// </summary>
        /// <param name="sysNo">分销商城类型系统编号</param>
        /// <param name="status">分销商城类型状态</param>       
        /// <returns>受影响的行数</returns>
        /// <remarks>
        /// 2013-09-04 郑荣华 创建
        /// </remarks>
        public int UpdateStatus(int sysNo, DistributionStatus.商城类型状态 status)
        {
            var r = IDsMallTypeDao.Instance.UpdateStatus(sysNo, status);
            if (r > 0)
                Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台, "修改分销商商城类型状态", LogStatus.系统日志目标类型.分销商商城类型, sysNo);
            return r;
        }
        #endregion

        #region 查询

        /// <summary>
        /// 获取分销商城类型信息
        /// </summary>
        /// <param name="mallCode">分销商城类型代号</param>
        /// <returns>分销商城类型信息</returns>
        /// <remarks>
        /// 2013-09-04 郑荣华 创建 可用于重复性检查 代号唯一
        /// </remarks>
        public DsMallType GetDsMallType(string mallCode)
        {
            return IDsMallTypeDao.Instance.GetDsMallType(mallCode);
        }

        /// <summary>
        /// 获取分销商城类型信息
        /// </summary>
        /// <param name="sysNo">分销商城类型系统编号</param>
        /// <returns>分销商城类型信息</returns>
        /// <remarks>
        /// 2013-09-04 郑荣华 创建
        /// </remarks>
        public DsMallType GetDsMallType(int sysNo)
        {
            return IDsMallTypeDao.Instance.GetDsMallType(sysNo);
        }

        /// <summary>
        /// 查询分销商城类型
        /// </summary>
        /// <param name="mallName">名称</param>
        /// <param name="isPreDeposit">是否使用预存款</param>
        /// <param name="status">状态</param>
        /// <returns>分销商城类型列表</returns>
        /// <remarks>
        /// 2013-09-04 郑荣华 创建
        /// </remarks>
        public IList<DsMallType> GetDsMallTypeList(string mallName, int? isPreDeposit, int? status)
        {
            return IDsMallTypeDao.Instance.GetDsMallTypeList(mallName, isPreDeposit, status);
        }

        /// <summary>
        /// 查询所有分销商城类型
        /// </summary>
        /// <returns>分销商城类型列表</returns>
        /// <remarks>
        /// 2013-09-04 郑荣华 创建
        /// </remarks>
        public IList<DsMallType> GetDsMallTypeList()
        {
            return IDsMallTypeDao.Instance.GetDsMallTypeList(null, null, null);
        }
        #endregion
    }
}
