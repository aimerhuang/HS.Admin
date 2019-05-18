using System.Collections.Generic;
using Hyt.DataAccess.Base;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.WorkflowStatus;
using Hyt.Model.Transfer;

namespace Hyt.DataAccess.RMA
{
    /// <summary>
    /// 退换货
    /// </summary>
    /// <remarks>2013-07-11 朱成果 创建</remarks>
    public abstract class IRcReturnDao:DaoBase<IRcReturnDao>
    {
        /// <summary>
        /// 插入退换货数据
        /// </summary>
        /// <param name="entity">退换货实体</param>
        /// <returns>返回新的编号</returns>
        /// <remarks>2013-07-11 朱成果 创建</remarks>
        public abstract int Insert(RcReturn entity);

        /// <summary>
        /// 获取当前订单待处理的退货单数量（不包括，作废和已完成的退换货单)
        /// </summary>
        /// <param name="orderID">订单编号</param>
        /// <returns></returns>
        /// <remarks>2013-07-16 朱成果 创建</remarks>
        public abstract int GetDealWithCount(int orderID);

        /// <summary>
        /// 获取当前订单待处理的退货单（不包括，作废和已完成的退换货单)
        /// </summary>
        /// <param name="orderID">订单编号</param>
        /// <returns></returns>
        /// <remarks>2013-07-16 朱成果 创建</remarks>
        public abstract RcReturn GetDealWithRMA(int orderID);

        /// <summary>
        /// 获取当前订单待审核的退货单
        /// </summary>
        /// <param name="orderID">订单编号</param>
        /// <returns>获取当前订单待审核的退货单</returns>
        /// <remarks>2014-04-24 余勇 创建</remarks>
        public abstract RcReturn GetPendWithReturn(int orderID);

        /// <summary>
        /// 更新退换货数据
        /// </summary>
        /// <param name="entity">退换货实体</param>
        /// <returns></returns>
        /// <remarks>2013-07-11 朱成果 创建</remarks>
        public abstract void Update(RcReturn entity);

        /// <summary>
        /// 获取退换货单实体
        /// </summary>
        /// <param name="sysNo">退换货单编号</param>
        /// <returns>退换货单实体</returns>
        /// <remarks>2013-07-11 朱成果 创建</remarks>
        public abstract CBRcReturn GetEntity(int sysNo);

        /// <summary>
        /// 退换货单分页查询
        /// </summary>
        /// <param name="filter">筛选条件</param>
        /// <returns>可分页的退换货表</returns>
        /// <remarks>2013-07-11 朱家宏 创建</remarks>
        public abstract Pager<CBRcReturn> GetAll(ParaRmaFilter filter);

        /// <summary>
        /// 根据出库单明细系统编号获取退换货申请单
        /// </summary>
        /// <param name="stockOutItemSysNo">出库单明细系统编号</param>
        /// <param name="sourceType">退换货申请单来源</param>
        /// <returns></returns>
        /// <remarks>
        /// 2013/8/21 何方 创建
        /// </remarks>
        public abstract IList<RcReturn> Get(int stockOutItemSysNo, RmaStatus.退换货申请单来源? sourceType);

        #region 退货金额计算使用

        /// <summary>
        /// 获取有效销售订单明细
        /// 订单明细数量不包含退货(退换货类型为退货并审核通过)数量
        /// </summary>
        /// <param name="orderSysNo">订单编号</param>
        /// <returns>有效销售订单明细</returns>
        /// <remarks>2013-09-13 吴文强 创建</remarks>
        public abstract IList<SoOrderItem> GetValidSalesOrderItem(int orderSysNo);

        /// <summary>
        /// 获取退货已退回的惠源币
        /// 退换货状态：=待退款 or 已完成
        /// </summary>
        /// <param name="orderSysNo">订单号</param>
        /// <returns>退货已退回的惠源币</returns>
        /// <remarks>2013-10-29 吴文强 创建</remarks>
        public abstract decimal GetReturnCoin(int orderSysNo);

        #endregion

        /// <summary>
        /// 转门店
        /// </summary>
        /// <param name="sysNo">退款单编号</param>
        /// <param name="handleDepartment">处理部门</param> 
        /// <returns>影响行数</returns> 
        /// <remarks>2013-06-19 余勇 创建</remarks>
        public abstract int UpdateRcReturnToShop(int sysNo, int handleDepartment);

        /// <summary>
        /// 退换货单统计
        /// </summary>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="filter">分销商权限信息</param>
        /// <param name="infomation">信息对象</param>
        /// <returns>统计数据</returns>
        /// <remarks>2013-09-26 邵斌 创建</remarks>
        public abstract CBDefaultPageCountInfo GetRMATotalInformation(System.DateTime startTime, System.DateTime endTime, ParaIsDealerFilter filter, ref CBDefaultPageCountInfo infomation);

        /// <summary>
        /// 退换货单统计
        /// </summary>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="filter">分销商权限信息</param>
        /// <param name="infomation">信息对象</param>
        /// <returns>统计数据</returns>
        /// <remarks>2013-09-26 邵斌 创建</remarks>
        public abstract CBDefaultPageCountInfo GetTodayRMATotalInformation(System.DateTime startTime,
                                                                           System.DateTime endTime,
                                                                           ParaIsDealerFilter filter,
                                                                           ref CBDefaultPageCountInfo infomation);
        /// <summary>
        /// 通过订单编号获得退换货列表
        /// </summary>
        /// <param name="orderSysNo">订单编号</param>
        /// <returns>退换货列表</returns>
        /// <remarks>
        /// 2013-12-06 余勇 创建
        /// </remarks>
        public abstract List<CBRcReturn> GetRmaReturnListByOrderSysNo(int orderSysNo);

        /// <summary>
        /// 获取还货订单编号
        /// </summary>
        /// <param name="rmaid">退换货编号</param>
        /// <returns></returns>
        /// <remarks>
        /// 2014-06-17 朱成果 创建
        /// </remarks>
        public abstract int GetRMAOrderSysNo(int rmaid);
        /// <summary>
        /// 退换货返利扣除列表
        /// </summary>
        /// <param name="orderSysNo">订单编号</param>
        /// <returns></returns>
        /// <remarks>2016-1-6 杨浩 创建</remarks>
        public abstract List<CBReurnDeductRebates> GetReurnDeductRebates(int orderSysNo);
    }
}
