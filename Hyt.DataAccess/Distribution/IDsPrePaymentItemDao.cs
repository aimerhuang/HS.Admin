
using System.Collections.Generic;
using Hyt.DataAccess.Base;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;

namespace Hyt.DataAccess.Distribution
{
    /// <summary>
    /// 预存款明细
    /// </summary>
    /// <remarks>2013-09-10  朱成果 创建</remarks>
    public abstract class IDsPrePaymentItemDao : DaoBase<IDsPrePaymentItemDao>
    {

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>新增记录编号</returns>
        /// <remarks>2013-09-10  朱成果 创建</remarks>
        public abstract int Insert(DsPrePaymentItem entity);

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns></returns>
        /// <remarks>2013-09-10  朱成果 创建</remarks>
        public abstract void Update(DsPrePaymentItem entity);

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns>数据实体</returns>
        /// <remarks>2013-09-10  朱成果 创建</remarks>
        public abstract DsPrePaymentItem GetEntity(int sysNo);

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns></returns>
        /// <remarks>2013-09-10  朱成果 创建</remarks>
        public abstract void Delete(int sysNo);
        /// <summary>
        /// 更新预存款明细状态
        /// </summary>
        /// <param name="sourceSysNo">来源单据</param>
        /// <param name="source">来源编号</param>
        /// <param name="status">状态</param>
        /// <returns></returns>
        /// <remarks>2016-1-7 杨浩 创建</remarks>
        public abstract int UpdatePrePaymentItemStatus(int sourceSysNo, int source, int status);
        /// <summary>
        /// 根据来源信息获取预存款明细
        /// </summary>
        /// <param name="dealerSysNo">分销商编号</param>
        /// <param name="source">来源</param>
        /// <param name="sourceSysNo">来源编号</param>
        /// <returns></returns>
        /// <remarks>2013-09-11  朱成果 创建</remarks>
        public abstract List<DsPrePaymentItem> GetListBySource(int dealerSysNo, int source, int sourceSysNo);

        /// <summary>
        /// 根据来源信息获取到期返利的预存款明细
        /// </summary>
        /// <param name="source">来源</param>
        /// <param name="delayDay">到期天数</param>
        /// <param name="dealerSysNo">分销商编号</param>
        /// <param name="orderSysNo">订单系统编号</param>
        /// <returns></returns>
        /// <remarks>2015-1-8 杨浩 创建</remarks>
        public abstract List<DsPrePaymentItem> GetExpireListBySource(int source, int delayDay, int dealerSysNo = 0, int orderSysNo=0);

        /// <summary>
        /// 创建分销商预存款往来账明细
        /// </summary>
        /// <param name="filter">过滤条件</param>
        /// <returns>分销商预存款往来账明细列表</returns>
        /// <remarks>2013-09-11 周唐炬 创建</remarks>
        public abstract Pager<CBDsPrePaymentItem> GetDsPrePaymentItemList(ParaDealerFilter filter);
        /// <summary>
        /// 更新付款单明细状态
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="SendStatus"></param>
        /// <remarks>2015-09-17 王耀发  创建</remarks>
        public abstract void UpdatePaymentItemStatus(int SysNo, int Status);
    }
}
