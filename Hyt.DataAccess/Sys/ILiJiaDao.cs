using Hyt.DataAccess.Base;
using Hyt.Model;
using Hyt.Model.Generated;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using System.Collections.Generic;


namespace Hyt.DataAccess.Sys
{
    public abstract class ILiJiaDao: DaoBase<ILiJiaDao>
    {
        /// <summary>
        /// 添加LiJia同步日志
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns>日志ID</returns>
        ///  <remarks>2013-10-22 黄志勇 创建</remarks>
        public abstract int LiJiaSyncLogCreate(LiJiaSyncLog model);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="filter">查询参数</param>
        /// <returns>日志分页</returns>
        /// <remarks>2013-10-22 黄志勇 创建</remarks>
        public abstract Pager<CBLiJiaSyncLog> GetList(ParaLiJiaSyncLogFilter filter);

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>日志</returns>
        /// <remarks>2013-10-23 黄志勇 创建</remarks>
        public abstract LiJiaSyncLog GetEntity(int sysNo);

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>受影响行数</returns>
        /// <remarks>2013-10-23 黄志勇 创建</remarks>
        public abstract int Update(LiJiaSyncLog entity);

        /// <summary>
        /// 数据是否已导入
        /// </summary>
        /// <param name="md5">数据md5</param>
        /// <returns>操作是否成功</returns>
        /// <remarks>2013-11-06 杨浩 创建</remarks>
        public abstract bool IsImport(string md5);

        /// <summary>
        /// 获取LiJia同步等待日志
        /// </summary>
        /// <returns></returns>
        /// <remarks>2014-5-4 杨浩 创建</remarks>
        public abstract List<LiJiaSyncLog> GetSyncWaitList(int count);
        /// <summary>
        /// 获取LiJia同步日志
        /// </summary>
        /// <param name="sysNos"></param>
        /// <returns></returns>
        /// <remarks>2014-5-4 杨浩 创建</remarks>
        public abstract IList<LiJiaSyncLog> GetList(string sysNos);
        /// <summary>
        /// 获取LiJia同步失败的日志
        /// </summary>
        /// <returns></returns>
        /// <remarks>2014-5-4 杨浩 创建</remarks>
        public abstract List<LiJiaSyncLog> GetSyncFailureList(int count);

        /// <summary>
        /// 获取LiJia同步日志关联列表
        /// </summary>
        /// <returns>分页查询</returns>
        /// <remarks>2014-4-9 杨浩 创建</remarks>
        public abstract List<LiJiaSyncLog> GetRelateList(string flowIdentify);
        /// <summary>
        /// 获取Kis单据编号
        /// </summary>
        /// <param name="flowType">流程类型</param>
        /// <param name="flowIdentify">流程编号</param>
        /// <returns></returns>
        /// <remarks>2017-05-05 杨浩 创建</remarks>
        public abstract string GetVoucherNo(string flowType, string flowIdentify);
        /// <summary>
        /// 获取没有同步金蝶的出库单
        /// </summary>
        /// <param name="warehouseSysNo">仓库系统编号</param>
        /// <returns></returns>
        /// <remarks>2017-09-28 杨浩 创建</remarks>
        public abstract IList<WhStockOut> GetNoSyncStockOutList(int warehouseSysNo);

    }
}
