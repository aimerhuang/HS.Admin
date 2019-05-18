using System;
using System.Collections.Generic;
using Hyt.DataAccess.Base;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;

namespace Hyt.DataAccess.MallSeller
{
    /// <summary>
    /// 分销商升舱订单数据访问接口
    /// </summary>
    /// <remarks>2013-09-03 朱家宏 创建</remarks>
    public abstract class IDsOrderDao : DaoBase<IDsOrderDao>
    {
        /// <summary>
        /// 根据开始日期获取指定状态的升舱订单
        /// </summary>
        /// <param name="startDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <param name="dearerMallSysNo">商城系统编号</param>
        /// <param name="status">订单状态</param>
        /// <returns>订单列表</returns>
        /// <reamrks>2014-04-08 黄波 创建</reamrks>
        public abstract List<CBDsOrder> GetSuccessedOrder(DateTime startDate, DateTime endDate, int dearerMallSysNo, Hyt.Model.WorkflowStatus.DistributionStatus.升舱订单状态 status);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="filter">查询参数</param>
        /// <returns>分页</returns>
        /// <remarks>2013-09-03 朱家宏 创建</remarks>
        public abstract Pager<CBDsOrder> Query(ParaDsOrderFilter filter);

        /// <summary>
        /// 查询可退换货升舱订单
        /// </summary>
        /// <param name="filter">查询参数</param>
        /// <returns>分页</returns>
        /// <remarks>2013-09-26 朱成果 创建</remarks>
        public abstract Pager<CBDsOrder> QueryForRma(ParaDsOrderFilter filter);

        /// <summary>
        /// 获取升舱订单实体
        /// </summary>
        /// <param name="mallOrderId">淘宝订单编号</param>
        /// <returns>实体</returns>
        /// <remarks>2013-09-05 朱家宏 创建</remarks>
        public abstract DsOrder SelectByMallOrderId(string mallOrderId);

        /// <summary>
        /// 根据升舱订单编号获取
        /// </summary>
        /// <param name="sysNo">升舱订单编号</param>
        /// <returns></returns>
        /// <remarks>2013-09-26 朱成果 创建</remarks>
        public abstract DsOrder SelectBySysNo(int sysNo);

        /// <summary>
        /// 获取升舱订单明细
        /// </summary>
        /// <param name="dsOrderSysNo">升舱编号</param>
        /// <returns>明细列表</returns>
        /// <remarks>2013-09-05 朱家宏 创建</remarks>
        public abstract IList<DsOrderItem> SelectItems(int dsOrderSysNo);

        /// <summary>
        /// 获取升舱订单明细关联
        /// </summary>
        /// <param name="dsOrderItemSysNo">升舱明细编号</param>
        /// <returns>明细关联列表</returns>
        /// <remarks>2013-09-05 朱家宏 创建</remarks>
        public abstract IList<DsOrderItemAssociation> SelectItemAssociations(int dsOrderItemSysNo);

        /// <summary>
        /// 根据出库单明细获取关联
        /// </summary>
        /// <param name="outStockItemNo">出库单明细编号</param>
        /// <returns>关联列表</returns>
        /// <remarks>2013-09-05 朱家宏 创建</remarks>
        public abstract List<DsOrderItemAssociation> GetDsOrderItemAssociationByOutStockItemNo(int outStockItemNo);

        /// <summary>
        /// 插入分销商升舱订单数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>新增记录编号</returns>
        /// <remarks>2013-09-06  余勇 创建</remarks>
        public abstract int InsertOrder(DsOrder entity);

        /// <summary>
        /// 更新分销商升舱订单数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>影响行数</returns>
        /// <remarks>2013-09-06  余勇 创建</remarks>
        public abstract int UpdateOrder(DsOrder entity);

        /// <summary>
        /// 删除分销商升舱订单数据
        /// </summary>
        /// <param name="sysNo">升舱编号</param>
        /// <remarks>2013-09-06  余勇 创建</remarks>
        public abstract void DeleteOrder(int sysNo);

        /// <summary>
        /// 插入分销商升舱订单明细数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>新增记录编号</returns>
        /// <remarks>2013-09-06  余勇 创建</remarks>
        public abstract int InsertOrderItem(DsOrderItem entity);

        /// <summary>
        /// 更新分销商升舱订单明细数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>影响行数</returns>
        /// <remarks>2013-09-06  余勇 创建</remarks>
        public abstract int UpdateOrderItem(DsOrderItem entity);

        /// <summary>
        /// 通过升舱编号删除分销商升舱订单明细数据
        /// </summary>
        /// <param name="dsOrderSysNo">升舱编号</param>
        /// <remarks>2013-09-06  余勇 创建</remarks>
        public abstract void DeleteItemByOrderSysNo(int dsOrderSysNo);

        /// <summary>
        /// 通过升舱明细编号删除数据
        /// </summary>
        /// <param name="dsOrderSysNo">升舱明细编号</param>
        /// <remarks>2014-08-18  朱成果 创建</remarks>
        public abstract void DeleteItemByItemSysNo(int dsorderitemsysno);


        /// <summary>
        /// 插入升舱订单明细关联数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>新增记录编号</returns>
        /// <remarks>2013-09-06  余勇 创建</remarks>
        public abstract int InsertAssociation(DsOrderItemAssociation entity);

        /// <summary>
        /// 更新升舱订单明细关联数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>影响行数</returns>
        /// <remarks>2013-09-06  余勇 创建</remarks>
        public abstract int UpdateAssociation(DsOrderItemAssociation entity);

        /// <summary>
        /// 通过升舱明细编号删除升舱订单明细关联数据
        /// </summary>
        /// <param name="dsOrderItemSysNo">升舱明细编号</param>
        /// <remarks>2013-09-06  余勇 创建</remarks>
        public abstract void DeleteAssociationByItmeSysNo(int dsOrderItemSysNo);

        /// <summary>
        /// 获取分销商升舱订单
        /// </summary>
        /// <param name="shopAccount">店铺账号</param>
        /// <param name="mallTypeSysNo">分销商城类型系统编号</param>
        /// <param name="top">取前几条</param>
        /// <param name="isFinish">升舱完成</param>
        /// <returns>分销商升舱订单列表</returns>
        /// <remarks>2013-09-10 黄志勇 创建</remarks>
        public abstract List<DsOrder> GetDsOrderInfo(string shopAccount, int mallTypeSysNo, int top, bool? isFinish);

        /// <summary>
        /// 分销商预存款主表
        /// </summary>
        /// <param name="shopAccount">店铺账号</param>
        /// <param name="mallTypeSysNo">分销商城类型系统编号</param>
        /// <returns>分销商预存款主表实体</returns>
        /// <remarks>2013-09-10 黄志勇 创建</remarks>
        public abstract DsPrePayment GetPrePayment(string shopAccount, int mallTypeSysNo);

        /// <summary>
        /// 根据商城订单事物编号获取分销商订单信息
        /// </summary>
        /// <param name="orderTransactionSysNo">订单事物编号</param>
        /// <returns>分销商订单信息列表</returns>
        /// <remarks>2013-09-12 朱成果 创建</remarks>
        public abstract List<DsOrder> GetEntityByTransactionSysNo(string orderTransactionSysNo);

        /// <summary>
        /// 根据商城订单编号获取分销商订单信息
        /// </summary>
        /// <param name="hytOrderID">商城订单编号</param>
        /// <returns>分销商订单信息列表</returns>
        /// <remarks>2013-09-12 朱成果 创建</remarks>
        public abstract List<DsOrder> GetEntityByHytOrderID(int hytOrderID);

        /// <summary>
        /// 获取商城订单对应的分销商编号
        /// </summary>
        /// <param name="hytOrderID">商城订单编号</param>
        /// <returns>商城订单对应的分销商编号</returns>
        /// <remarks>2013-09-12 朱成果 创建</remarks>
        public abstract int GetHytOrderDealerSysno(int hytOrderID);

        #region 登录
        /// <summary>
        /// 根据店铺账号、类型获取分销商商城
        /// </summary>
        /// <param name="shopAccount">店铺账号</param>
        /// <param name="mallTypeSysNo">分销商城类型系统编号</param>
        /// <returns>分销商信息</returns>
        /// <remarks>2013-09-05 黄志勇 创建</remarks>
        public abstract DsDealerMall GetDsDealerMallByShopAccount(string shopAccount, int mallTypeSysNo);

        /// <summary>
        /// 根据分销商系统编号获取授权账号绑定表
        /// </summary>
        /// <param name="dealerSysNo">分销商系统编号</param>
        /// <returns>授权账号绑定表</returns>
        /// <remarks>2013-09-13 黄志勇 创建</remarks>
        public abstract List<DsDealerMall> GetDsAuthorizations(int dealerSysNo);

        /// <summary>
        /// 通过名称得到分销商商城旗舰店
        /// </summary>
        /// <returns>分销商商城旗舰店列表</returns>
        /// <remarks>2014-05-20 余勇 创建</remarks>
        public abstract List<DsDealerMall> GetAllFlagShip();

        /// <summary>
        /// 更新分销商商城
        /// </summary>
        /// <param name="model">分销商商城</param>
        ///<returns>受影响行数</returns>
        /// <remarks>2013-09-05 黄志勇 创建</remarks>
        public abstract int UpdateDsAuthorization(DsDealerMall model);

        /// <summary>
        /// 设置登录信息
        /// </summary>
        /// <param name="shopAccount">店铺账号</param>
        /// <param name="mallTypeSysNo">分销商城类型系统编号</param>
        /// <returns>分销商授权信息</returns>
        /// <remarks>2013-9-11 黄志勇 创建</remarks>
        public abstract MallSellerAuthorization SetLoginInfo(string shopAccount, int mallTypeSysNo);

        /// <summary>
        /// 根据系统用户系统编号获取分销商商城
        /// </summary>
        /// <param name="userId">系统用户系统编号</param>
        /// <returns>分销商商城</returns>
        /// <remarks>2013-9-11 黄志勇 创建</remarks>
        public abstract DsDealerMall GetAuthorizationByUserID(int userId);
        #endregion

        #region 账户管理
        /// <summary>
        /// 获取账户信息
        /// </summary>
        /// <param name="shopAccount">店铺账号</param>
        /// <param name="mallTypeSysNo">分销商城类型系统编号</param>
        /// <returns>账户信息</returns>
        /// <remarks>2013-09-05 朱家宏 创建</remarks>
        public abstract CBAccountInfo GetAccountInfo(string shopAccount, int mallTypeSysNo);

        /// <summary>
        /// 分页查询分销商预存款往来账明细
        /// </summary>
        /// <param name="filter">查询参数</param>
        /// <returns>分页</returns>
        /// <remarks>2013-09-06 黄志勇 创建</remarks>
        public abstract Pager<DsPrePaymentItem> QueryPrePaymentItem(ParaDsPrePaymentItemFilter filter);

        /// <summary>
        /// 根据分销商系统编号获取授权账号绑定表
        /// </summary>
        /// <param name="dealerSysNo">分销商系统编号</param>
        /// <returns>分销商</returns>
        /// <remarks>2013-09-13 朱家宏 创建</remarks>
        public abstract DsDealer GetDealer(int dealerSysNo);

        #endregion

        /// <summary>
        /// 获取分销商商城
        /// </summary>
        /// <param name="value">分销商商城系统编号</param>
        /// <returns>分销商商城实体</returns>
        /// <remarks>2013-9-17 朱家宏 创建</remarks>
        public abstract DsDealerMall SelectDsDealerMall(int value);

        /// <summary>
        /// 是否已经成功升舱
        /// </summary>
        /// <param name="dealerMallSysNo">分销商商城系统编号</param>
        /// <param name="mallOrderId">商城订单号</param>
        /// <returns>ture或false是否已经成功升舱</returns>
        /// <remarks>2013-9-17 朱成果 创建</remarks>
        public abstract bool ExistsDsOrder(int dealerMallSysNo, string mallOrderId);

        /// <summary>
        /// 获取商城订单和商城类型
        /// </summary>
        /// <param name="orderTransactionSysNos">OrderTransactionSysNo</param>
        /// <returns>商城订单和商城类型列表</returns>
        /// <remarks>2013-9-11 黄志勇 创建</remarks>
        public abstract List<CBOrderMallType> GetMallType(string orderTransactionSysNos);

        /// <summary>
        /// 根据商城订单明细获取升仓明细
        /// </summary>
        /// <param name="soOrderItemSysNo">事物编号</param>
        /// <returns>获取升仓明细</returns>
        /// <remarks>2014-07-04 朱成果 创建</remarks>
        public abstract DsOrderItem GetDsOrderItemsByHytItems(int soOrderItemSysNo);
    }
}
