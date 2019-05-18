using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Base;
using Hyt.Model;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;

namespace Hyt.DataAccess.LevelPoint
{
    /// <summary>
    /// 积分业务
    /// </summary>
    /// <remarks>2013-07-01 吴文强 创建</remarks>
    /// <remarks>2013-07-10 黄波 重构</remarks>
    public abstract class IPointDao : DaoBase<IPointDao>
    {
        #region 等级积分日志
        /// <summary>
        /// 查看会员等级日志最新一条记录
        /// </summary>
        /// <param name="customerSysNo">客户编号</param>
        /// <returns>等级日志</returns>
        /// <remarks>2013-11-8 苟治国 创建</remarks>
        public abstract CrLevelPointLog GetLevelPointList(int customerSysNo);

        /// <summary>
        /// 获取最后一次一条经验积分日志
        /// </summary>
        /// <param name="customerSysNo">客户编号</param>
        /// <returns>经验积分日志</returns>
        /// <remarks>2013-12-18 苟治国 创建</remarks>
        public abstract CrExperiencePointLog GetExperiencePointLog(int customerSysNo);

        /// <summary>
        /// 获取规定日期范围发内的经验积分日志列表
        /// </summary>
        /// <param name="customerSysNo">客户编号</param>
        /// <param name="beginTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns>经验积分日志列表</returns>
        /// <remarks>2013-12-18 苟治国 创建</remarks>
        public abstract IList<CrExperiencePointLog> GetExperiencePointLog(int customerSysNo, DateTime beginTime, DateTime endTime);

        /// <summary>
        /// 汇总客等级积分日志 增加积分、减少积分
        /// </summary>
        /// <param name="customerSysNo">客户编号</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns>等级积分日志</returns>
        /// <remarks>2013-11-8 苟治国 创建</remarks>
        public abstract CBCrLevelPointLog GetLevelPointLog(int customerSysNo, DateTime startTime, DateTime endTime);
        #endregion

        #region 获取实体

        /// <summary>
        /// 查看会员等级
        /// </summary>
        /// <param name="SysNo">等级编号</param>
        /// <returns>等级日志</returns>
        /// <remarks>2013-07-15 苟治国 创建</remarks>
        public abstract CBCrLevelLog GetLevelLogModel(int SysNo);

        /// <summary>
        /// 查看会员等级积分日志
        /// </summary>
        /// <param name="SysNo">等级积分日志编号</param>
        /// <returns>等级积分日志</returns>
        /// <remarks>2013-07-15 苟治国 创建</remarks>
        public abstract CBCrLevelPointLog GetLevelPointLogModel(int SysNo);

        /// <summary>
        /// 查看经验积分日志
        /// </summary>
        /// <param name="SysNo">经验积分日志编号</param>
        /// <returns>经验积分日志</returns>
        /// <remarks>2013-07-15 苟治国 创建</remarks>
        public abstract CBCrExperiencePointLog GetCrExperiencePointLogModel(int SysNo);
         /// <summary>
        /// 查看用户积分日志
        /// </summary>
        /// <param name="sysNo">积分日志编号</param>
        /// <returns>积分日志</returns>
        /// <remarks>2013-07-15 苟治国 创建</remarks>
        public abstract CrAvailablePointLog GetCrAvailablePointLogModel(int sysNo);

        /// <summary>
        /// 获取惠源币日志模型
        /// </summary>
        /// <param name="SysNo">惠源币日志系统编号</param>
        /// <returns>惠源币日志模型</returns>
        /// <remarks>2013-07-15 杨晗 创建</remarks>
        public abstract CBCrExperienceCoinLog GetCbCrExperienceCoinLog(int SysNo);
        #endregion

        #region 获取调整日志
        /// <summary>
        /// 获取惠源币日志
        /// </summary>
        /// <param name="pager">分页查询条件</param>
        /// <returns>惠源币日志</returns>
        /// <remarks>2013-07-01 吴文强 创建</remarks>
        /// <remarks>2013-07-15 杨晗 修改</remarks>
        public abstract void GetExperienceCoinLog(ref Pager<CBCrExperienceCoinLog> pager);

        /// <summary>
        /// 获取经验积分日志
        /// </summary>
        /// <param name="pager">分页查询条件</param>
        /// <returns>经验积分日志</returns>
        /// <remarks>2013-07-01 吴文强 创建</remarks>
        /// <remarks>2013-07-15 苟治国 修改</remarks>
        public abstract void GetExperiencePointLog(ref Pager<CBCrExperiencePointLog> pager);
         /// <summary>
        /// 获取用户积分日志
        /// </summary>
        /// <param name="pager">分页查询条件</param>
        /// <returns>经验积分日志</returns>
        /// <remarks>2013-07-10 黄波 创建</remarks>
        /// <remarks>2013-07-10 苟治国 修改</remarks>
        public abstract void GetCrAvailablePointLog(ref Model.Pager<CrAvailablePointLog> pager);

        /// <summary>
        /// 获取等级积分日志
        /// </summary>
        /// <param name="pager">分页查询条件</param>
        /// <returns>等级积分日志</returns>
        /// <remarks>2013-07-01 吴文强 创建</remarks>
        /// <remarks>2013-07-15 苟治国 修改</remarks>
        public abstract void GetLevelPointLog(ref Pager<CBCrLevelPointLog> pager);

        /// <summary>
        /// 获取等级日志
        /// </summary>
        /// <param name="pager">分页查询条件</param>
        /// <returns>等级日志</returns>
        /// <remarks>2013-07-01 吴文强 创建</remarks>
        /// <remarks>2013-07-15 苟治国 修改</remarks>
        public abstract void GetLevelLog(ref Model.Pager<CBCrLevelLog> pager);
        #endregion

        #region 更新用户积分相关
        /// <summary>
        /// 更新用户会员币并记录日志
        /// </summary>
        /// <param name="customerSysNo">用户编号</param>
        /// <param name="amount">调整会员币金额数(正数:增加 负数:减少)</param>
        /// <param name="model">会员币日志实体</param>
        /// <returns>void</returns>
        /// <remarks>2013-07-11 黄波 创建</remarks>
        public abstract void AdjustExperienceCoin(int customerSysNo, decimal amount,CrExperienceCoinLog model);

        /// <summary>
        /// 更新用户经验积分并记录日志
        /// </summary>
        /// <param name="customerSysNo">用户编号</param>
        /// <param name="point">调整经验积分数(正数:增加 负数:减少)</param>
        /// <param name="experiencePointLogModel">经验积分日志实体</param>
        /// <param name="availablePointLogModel">可用积分日志实体</param>
        /// <returns>void</returns>
        /// <remarks>2013-07-11 黄波 创建</remarks>
        public abstract void AdjustExperiencePoint(int customerSysNo, int point, CrExperiencePointLog experiencePointLogModel, CrAvailablePointLog availablePointLogModel);

        /// <summary>
        /// 更新用户可用积分
        /// </summary>
        /// <param name="customerSysNo">用户编号</param>
        /// <param name="point">积分数(正数:增加 负数:减少)</param>
        /// <param name="model">可用积分日志实体</param>
        /// <returns>void</returns>
        /// <remarks>2013-10-30 黄波 创建</remarks>
        public abstract void UpdateAvailablePoint(int customerSysNo, int point, CrAvailablePointLog model);

        /// <summary>
        /// 调整用户等级积分并记录日志
        /// </summary>
        /// <param name="customerSysNo">用户编号</param>
        /// <param name="point">调整等级积分数(正数:增加 负数:减少)</param>
        /// <param name="model">等级积分日志实体</param>
        /// <returns>void</returns>
        /// <remarks>2013-07-11 黄波 创建</remarks>
        public abstract void AdjustLevelPoint(int customerSysNo, int point,CrLevelPointLog model);
        #endregion

        #region 更新用户等级

        /// <summary>
        /// 更新用户等级并记录日志
        /// </summary>
        /// <param name="customerSysNo">用户编号</param>
        /// <param name="userSysNo">系统用户编号</param>
        /// <param name="changeType">等级积分日志变更类型</param>
        /// <param name="description">等级变更说明</param>
        /// <remarks>2013-07-11 黄波 创建</remarks>
        public abstract void UpdateCustomerLevel(int customerSysNo, int userSysNo, CustomerStatus.等级积分日志变更类型 changeType, string description);

        /// <summary>
        /// 更新用户等级
        /// 用于注册用户
        /// </summary>
        /// <param name="customerSysNo">用户编号</param>
        /// <remarks>2013-10-30 黄波 创建</remarks>
        public abstract void UpdateCustomerLevel(int customerSysNo);
        #endregion

        public abstract bool HasExperiencePoint(int pointType, string transactionSysNo);

        public abstract bool HasLevelPoint(int pointType, string transactionSysNo);
    }
}
