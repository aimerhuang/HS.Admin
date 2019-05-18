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
    ///分销商城类型维护业务层
    /// </summary>
    /// <remarks>
    /// 2013-09-04 郑荣华 创建
    /// </remarks>
    public abstract class IDsMallTypeDao : DaoBase<IDsMallTypeDao>
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
        public abstract int Create(DsMallType model);

        /// <summary>
        /// 修改分销商城类型
        /// </summary>
        /// <param name="model">分销商城类型实体</param>
        /// <returns>受影响的行数</returns>
        /// <remarks>
        /// 2013-09-04 郑荣华 创建
        /// </remarks>
        public abstract int Update(DsMallType model);

        /// <summary>
        /// 分销商城类型状态更新
        /// </summary>
        /// <param name="sysNo">分销商城类型系统编号</param>
        /// <param name="status">分销商城类型状态</param>
        /// <returns>受影响的行数</returns>
        /// <remarks>
        /// 2013-09-04 郑荣华 创建
        /// </remarks>
        public abstract int UpdateStatus(int sysNo, DistributionStatus.商城类型状态 status);

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
        public abstract DsMallType GetDsMallType(string mallCode);

        /// <summary>
        /// 获取分销商城类型信息
        /// </summary>
        /// <param name="sysNo">分销商城类型系统编号</param>
        /// <returns>分销商城类型信息</returns>
        /// <remarks>
        /// 2013-09-04 郑荣华 创建
        /// </remarks>
        public abstract DsMallType GetDsMallType(int sysNo);

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
        public abstract IList<DsMallType> GetDsMallTypeList(string mallName, int? isPreDeposit, int? status);

        #endregion
    }
}
