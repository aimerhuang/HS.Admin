using System.Collections.Generic;
using Hyt.DataAccess.Base;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;

namespace Hyt.DataAccess.Sys
{
    public abstract class IEasDao : DaoBase<IEasDao>
    {


        /// <summary>
        /// 获取指定接口流程编号同步次数
        /// </summary>
        /// <param name="interfaceType">接口类型</param>
        /// <param name="flowIdentify">流程编号</param>
        /// <returns></returns>
        /// <remarks>2017-1-4 杨浩 创建</remarks>
        public abstract int GetEasSyncLogCount(int interfaceType, string flowIdentify);
        /// <summary>
        /// 根据流程编号和接口类型获取单据编号
        /// </summary>
        /// <param name="interfaceType">接口类型</param>
        /// <param name="flowIdentify">流程编号</param>
        /// <returns></returns>
        /// <remarks>2017-1-4 杨浩 创建</remarks>
        public abstract string GetVoucherNoByTlowIdentify(int interfaceType, string flowIdentify);
        /// <summary>
        /// 添加Eas同步日志
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns>日志ID</returns>
        ///  <remarks>2013-10-22 黄志勇 创建</remarks>
        public abstract int EasSyncLogCreate(EasSyncLog model);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="filter">查询参数</param>
        /// <returns>日志分页</returns>
        /// <remarks>2013-10-22 黄志勇 创建</remarks>
        public abstract Pager<CBEasSyncLog> GetList(ParaEasSyncLogFilter filter);

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>日志</returns>
        /// <remarks>2013-10-23 黄志勇 创建</remarks>
        public abstract EasSyncLog GetEntity(int sysNo);

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>受影响行数</returns>
        /// <remarks>2013-10-23 黄志勇 创建</remarks>
        public abstract int Update(EasSyncLog entity);

        /// <summary>
        /// 数据是否已导入
        /// </summary>
        /// <param name="md5">数据md5</param>
        /// <returns>操作是否成功</returns>
        /// <remarks>2013-11-06 杨浩 创建</remarks>
        public abstract bool IsImport(string md5);

        /// <summary>
        /// 获取Eas同步等待日志
        /// </summary>
        /// <returns></returns>
        /// <remarks>2014-5-4 杨浩 创建</remarks>
        public abstract List<EasSyncLog> GetSyncWaitList(int count);
        /// <summary>
        /// 获取Eas同步日志
        /// </summary>
        /// <param name="sysNos"></param>
        /// <returns></returns>
        /// <remarks>2014-5-4 杨浩 创建</remarks>
        public abstract IList<EasSyncLog> GetList(string sysNos);
        /// <summary>
        /// 获取Eas同步失败的日志
        /// </summary>
        /// <returns></returns>
        /// <remarks>2014-5-4 杨浩 创建</remarks>
        public abstract List<EasSyncLog> GetSyncFailureList(int count);

        /// <summary>
        /// 获取Eas同步日志关联列表
        /// </summary>
        /// <returns>分页查询</returns>
        /// <remarks>2014-4-9 杨浩 创建</remarks>
        public abstract List<EasSyncLog> GetRelateList(string flowIdentify);
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

        /// <summary>
        /// 根据流程编号和接口类型获取推送数据
        /// </summary>
        /// <param name="interfaceType">接口类型</param>
        /// <param name="flowIdentify">流程编号</param>
        /// <returns></returns>
        /// <remarks>2017-1-4 杨浩 创建</remarks>
        public abstract List<EasSyncLog> GetDataByFlowIdentify(int interfaceType, string flowIdentify);

       
    }
}
