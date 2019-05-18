using System;
using System.Collections.Generic;
using Hyt.DataAccess.Base;
using Hyt.Model;
using Hyt.Model.WorkflowStatus;

namespace Hyt.DataAccess.Warehouse
{
    /// <summary>
    /// 出库
    /// </summary>
    /// <remarks>
    /// 2013/7/12 何方 创建
    /// </remarks>
    public abstract class IOutStockDao : DaoBase<IOutStockDao>
    {
        /// <summary>
        /// 根据过滤条件查询出库单
        /// </summary>
        /// <param name="condition">过滤条件</param>
        /// <param name="pageSize">页大小</param>
        /// <returns>出库单实体列表</returns>
        /// <remarks>2013-07-04 周唐炬 创建</remarks>
        public abstract Pager<CBWhStockOut> SearchFilter(StockOutSearchCondition condition, int pageSize);

        /// <summary>
        /// 根据输入条件查询出库单
        /// </summary>
        /// <param name="condition">搜索条件</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="userSysNos">当前用户系统编号</param>
        /// <param name="isHasAllWarehouse">是否拥有所有仓库</param>
        /// <returns>出库单实体列表</returns>
        /// <remarks>2013-06-08 周瑜 创建</remarks>
        public abstract Pager<CBWhStockOut> Search(StockOutSearchCondition condition, int pageIndex, int pageSize, int userSysNos, bool isHasAllWarehouse);

        /// <summary>
        /// 根据输入条件查询出库单
        /// </summary>
        /// <param name="condition">搜索条件</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <returns>出库单实体列表</returns>
        /// <remarks>2013-06-08 周瑜 创建</remarks>
        /// <remarks>2013-07-03 周唐炬 加入查询条件配送方式DeliveryTypeSysNo</remarks>
        public abstract Pager<CBWhStockOut> Search(StockOutSearchCondition condition, int pageIndex, int pageSize);

        /// <summary>
        /// 根据输入条件查询出库单
        /// </summary>
        /// <param name="status">出库单状态</param>
        /// <param name="isInvoice">是否开票</param>
        /// <param name="deliverySysNo">配送方式系统编号</param>
        /// <param name="sysno">出库单系统编号</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="warehouseSysNo">仓库系统编号</param>
        /// <param name="userSysNos">当前用户系统编号</param>
        /// <param name="isHasAllWarehouse">是否拥有所有仓库</param>
        /// <returns>发票实体列表</returns>
        /// <remarks>2013-07-10 周瑜 创建</remarks>
        public abstract Pager<CBWhStockOut> QuickSearch(int? status, int? isInvoice, int? deliverySysNo, string sysno, int pageSize, int pageIndex, int? warehouseSysNo, int userSysNos, bool isHasAllWarehouse);

        /// <summary>
        /// 修改出库单状态
        /// </summary>
        /// <param name="cbWhStockOut">用于修改出库单的实体</param>
        /// <returns>出库单实体列表</returns>
        /// <remarks>2013-06-08 周瑜 创建</remarks>
        public abstract void Update(CBWhStockOut cbWhStockOut);

        /// <summary>
        /// 修改出库单状态
        /// </summary>
        /// <param name="stockOutSysNo">出库单系统编号.</param>
        /// <param name="status">出库单状态</param>
        /// <returns></returns>
        /// <remarks>
        /// 2013-07-12 何方 创建
        /// </remarks>
        public abstract void UpdateStatus(int stockOutSysNo,  WarehouseStatus.出库单状态 status);

        /// <summary>
        /// 新增出库单
        /// </summary>
        /// <param name="model">用于新增出库单的实体</param>
        /// <returns>返回出库单系统编号</returns>
        /// <remarks>2013-06-08 周瑜 创建</remarks>
        public abstract int Insert(WhStockOut model);

        /// <summary>
        /// 根据主键获取出库单
        /// </summary>
        /// <param name="stockOutSysNo">出库单系统编号</param>
        /// <returns>出库单实体</returns>
        /// <remarks>
        /// 2013-06-18 何方 创建
        /// </remarks>
        public abstract WhStockOut GetModel(int stockOutSysNo);
        /// <summary>
        /// 获取出库详情
        /// </summary>
        /// <param name="stockOutSysno">出库单系统编号</param>
        /// <returns></returns>
        /// <remarks>2017-10-24 杨浩 创建</remarks>
        public abstract WhStockOut GetSimpleInfo(int stockOutSysno);

        /// <summary>
        /// 逻辑删除出库单中的商品
        /// </summary>
        /// <param name="items">将要删除的商品列表</param>
        /// <returns></returns>
        /// <remarks>2013-06-08 周瑜 创建</remarks>
        public abstract void RemoveItem(IList<WhStockOutItem> items);

        /// <summary>
        /// 修改出库单状态
        /// </summary>
        /// <param name="model">用于修改出库单的实体</param>
        /// <returns>返回受影响的行数</returns>
        /// <remarks>2013-06-08 周瑜 创建</remarks>
        public abstract int Update(WhStockOut model);
        /// <summary>
        /// 修改出库单备注
        /// </summary>
        /// <param name="model">用于修改出库单的实体</param>
        /// <returns>返回受影响的行数</returns>
        /// <remarks>罗勤尧 创建</remarks>
        public abstract int UpdateRemarks(int SysNo, string Remarks);

        /// <summary>
        /// 修改出库单（用于事务处理）
        /// </summary>
        /// <param name="model">用于修改出库单的实体</param>
        /// <param name="status">状态</param>
        /// <returns>返回受影响的行数</returns>
        /// <remarks>2014-08-01 余勇 创建</remarks>
        public abstract int UpdateStockOutByStatus(WhStockOut model, int status);

        /// <summary>
        /// 在出库单主表上添加一条记录
        /// </summary>
        /// <param name="model">出库实体</param>
        /// <returns>出库单编号</returns>
        /// <remarks>2013-06-25 朱成果 创建</remarks>
        public abstract int InsertMain(WhStockOut model);

        /// <summary>
        /// 在出库单明细表上添加一条记录
        /// </summary>
        /// <param name="item">出库明细</param>
        /// <returns>出库明细编号</returns>
        /// <remarks>2013-06-25 朱成果 创建</remarks>
        public abstract int InsertItem(WhStockOutItem item);

        /// <summary>
        /// 获取出库单的详细信息
        /// </summary>
        /// <param name="sysno">出库单系统编号</param>
        /// <returns>出库单详细信息实体</returns>
        /// <remarks>2013-06-25 周瑜 创建</remarks>
        public abstract CBWhStockOut GetStockOutInfo(int sysno);

        /// <summary>
        /// 根据订单编号获取出库单列表
        /// </summary>
        /// <param name="orderId">订单编号</param>
        /// <param name="onlyComplate">只读完成的定点</param>
        /// <returns>出库单列表</returns>
        /// <remarks>2013-06-25 朱成果 创建</remarks>
        public abstract IList<WhStockOut> GetWhStockOutListByOrderID(int orderId, bool onlyComplate = false);

           /// <summary>
        /// 根据订单号和产品编号获取相关的出库明细
        /// </summary>
        /// <param name="orderId">订单号</param>
        /// <param name="productid">产品编号</param>
        /// <returns></returns>
        /// <remarks>2013-08-19 朱成果 创建</remarks>
        public abstract IList<WhStockOutItem> GetWhStockOutItemList(int orderId, int productid);
        
         /// <summary>
        /// 获取出库单主表数据(不包括明细)
        /// </summary>
        /// <param name="sysNo">出库单编号</param>
        /// <returns></returns>
        /// <remarks>2013-08-19 朱成果 创建</remarks>
        public abstract WhStockOut GetEntity(int sysNo);

        /// <summary>
        /// 修改出库单明细中
        /// </summary>
        /// <param name="whStockOutItems">需要修改的出库单明细</param>
        /// <returns></returns>
        /// <remarks>
        /// 2013-08-14 何方 创建
        /// </remarks>
        public abstract void UpdateItems(IList<WhStockOutItem> whStockOutItems);

        /// <summary>
        /// 根据系统编号集合获取出库单列表
        /// </summary>
        /// <param name="SysNos">出库单系统编号数组</param>
        /// <returns>出库单列表</returns>
        /// <remarks>2013-07-04 沈强 创建</remarks>
        public abstract IList<WhStockOut> GetWhStockOutListBySysNos(int[] SysNos);

        /// <summary>
        /// 根据系统编号集合获取出库单及收货人列表
        /// </summary>
        /// <param name="sysNos">出库单系统编号集合</param>
        /// <returns>出库单及收货人列表</returns>
        /// <remarks>2013-07-09 黄伟 创建</remarks>
        public abstract IList<CBWhStockOut> GetWhStockOutList(int[] sysNos);

        /// <summary>
        /// 根据出库单系统编号获取出库单明细列表
        /// </summary>
        /// <param name="sysNo">出库单系统编号集合</param>
        /// <returns>出库单明细列表</returns>
        /// <remarks>2013-07-09 黄伟 创建</remarks>
        public abstract IList<WhStockOutItem> GetWhStockOutItemList(int sysNo);

         /// <summary>
        /// 根据销售单明细编号获取有效的出库单明细列表
        /// </summary>
        /// <param name="orderItemSysNo">销售单明细编号</param>
        /// <returns>出库单明细列表</returns>
        /// <remarks>2013-11-22 吴文强 创建</remarks>
        public abstract IList<WhStockOutItem> GetStockOutItems(int[] orderItemSysNo);

        /// <summary>
        /// 根据出库单系统编号获取相应订单
        /// </summary>
        /// <param name="stockOutSysNo">出库单系统编号</param>
        /// <returns>订单实体</returns>
        /// <remarks>2013-07-11 黄伟 创建</remarks>
        public abstract SoOrder GetSoOrder(int stockOutSysNo);

        /// <summary>
        /// 获取出库单明细
        /// </summary>
        /// <param name="sysNo">出库单明细编号</param>
        /// <returns></returns>
        /// <remarks>2013-07-15 朱成果 创建</remarks>
        public abstract WhStockOutItem GetStockOutItem(int sysNo);

         /// <summary>
        /// 获取出库单明细集合
        /// </summary>
        /// <param name="sysNos">出库单明细编号</param>
        /// <returns></returns>
        /// <remarks>2013-07-15 朱成果 创建</remarks>
        public abstract IList<WhStockOutItem> GetStockOutItem(int[] sysNos);

        /// <summary>
        /// 更新出库单明细
        /// </summary>
        /// <param name="item">出库单明细实体</param>
        /// <returns></returns>
        /// <remarks>2013-07-15 朱成果 创建</remarks>
        public abstract void UpdateOutItem(WhStockOutItem item);

        /// <summary>
        /// 根据事务编号获取出库单
        /// </summary>
        /// <param name="transactionSysNo">事务编号</param>
        /// <returns>出库单实体</returns>
        /// <remarks>2013-07-29 周瑜 创建</remarks>
        public abstract IList<WhStockOut> GetModelByTransactionSysNo(string transactionSysNo);

        /// <summary>
        /// 判断出库单是否为配送单中的第一单
        /// </summary>
        /// <param name="sysNo">出库单系统编号</param>
        /// <returns>true:是第一单 false:非第一单 </returns>
        /// <remarks>2013-07-31 周瑜创建</remarks>
        public abstract bool GetWhStockOutWithDeliveryCount(int sysNo);

        /// <summary>
        /// 根据客户系统编号与出库单状态获取退换货详细
        /// </summary>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="status">出库单状态</param>
        /// <returns>出库单集合</returns>
        /// <remarks>2013-09-12 沈强 创建</remarks>
        public abstract IList<Model.B2CApp.ReturnDetail> GetReturnDetailByCustomerSysNoAndStatus(int customerSysNo, int status);

        /// <summary>
        /// 根据客户系统编号与出库单状态获取退换货详细
        /// </summary>
        /// <param name="stockOutSysNo">出库单系统编号</param>
        /// <returns>退换货详细</returns>
        /// <remarks>2013-09-12 沈强 创建</remarks>
        public abstract Model.B2CApp.ReturnDetail GetReturnDetailByStockOutSysNo(int stockOutSysNo);

        /// <summary>
        /// 获取出库单明细
        /// </summary>
        /// <param name="sysNo">出库单明细编号</param>
        /// <returns>出库单明细</returns>
        /// <remarks>2013-12-04 yangheyu 创建</remarks>
        public abstract WhStockOutItem GetWhStockOutItem(int sysNo);
    }
}
