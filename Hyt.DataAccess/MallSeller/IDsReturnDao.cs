using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Base;
using Hyt.Model;
using Hyt.Model.Transfer;
using Hyt.Model.Parameter;

namespace Hyt.DataAccess.MallSeller
{
    /// <summary>
    /// 分销商退换货
    /// </summary>
    /// <remarks>2013-09-10 朱家宏 创建</remarks>
    public abstract class IDsReturnDao : DaoBase<IDsReturnDao>
    {
        /// <summary>
        /// 根据hyt退换货单号获取实体
        /// </summary>
        /// <param name="value">hyt退换货单号</param>
        /// <returns>分销商退换货单</returns>
        /// <remarks>2013-09-10 朱家宏 创建</remarks>
        public abstract DsReturn SelectByRmaSysNo(int value);

        /// <summary>
        /// 获取分销商退换货明细
        /// </summary>
        /// <param name="dsReturnSysNo">分销商退换货单编号</param>
        /// <returns></returns>
        /// <remarks>2013-09-10 朱家宏 创建</remarks>
        public abstract IList<DsReturnItem> SelectItems(int dsReturnSysNo);

        /// <summary>
        /// 获取分销商退换货单
        /// </summary>
        /// <param name="shopAccount">账户</param>
        /// <param name="mallTypeSysNo">类型</param>
        /// <param name="top">取前几条</param>
        /// <param name="isFinish">退款完成</param>
        /// <returns></returns>
        /// <remarks>2013-09-10 黄志勇 创建</remarks>
        public abstract List<CBDsReturn> GetReturn(string shopAccount, int mallTypeSysNo, int top, bool? isFinish);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="filter">查询参数</param>
        /// <returns>分页</returns>
        /// <remarks>2013-09-10 余勇 创建</remarks>
        public abstract Pager<CBDsReturn> Query(ParaDsReturnFilter filter);

        /// <summary>
        /// 插入主表
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns>sysNo</returns>
        /// <remarks>2013-09-12 朱家宏 创建</remarks>
        public abstract int Insert(DsReturn model);

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns></returns>
        /// <remarks>2013-09-25  朱成果 创建</remarks>
        public abstract void Update(DsReturn entity);

        /// <summary>
        /// 插入明细
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns>sysNo</returns>
        /// <remarks>2013-09-12 朱家宏 创建</remarks>
        public abstract int InsertItem(DsReturnItem model);
    }
}
