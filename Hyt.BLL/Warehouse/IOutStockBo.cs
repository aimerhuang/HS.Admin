using System.Collections.Generic;
using Hyt.Infrastructure.Pager;
using Hyt.Model;

namespace Hyt.BLL.Warehouse
{
    public interface IStockOutBo
    {

        /// <summary>
        /// 根据过滤条件查询出库单
        /// </summary>
        /// <param name="condition">过滤条件</param>
        /// <param name="pageSize">页大小</param>
        /// <returns>出库单实体列表</returns>
        /// <remarks>2013-07-04 周唐炬 创建</remarks>
        PagedList<CBWhStockOut> SearchFilter(StockOutSearchCondition condition, int pageSize);

        /// <summary>
        /// 创建出库单
        /// </summary>
        /// <param name="stockOut">出库单实体.</param>
        /// <returns>出库单系统编号</returns>
        /// <remarks>
        /// 2013/6/26 何方 创建
        /// </remarks>
        int CreateStockOut( WhStockOut stockOut);
        /// <summary>
        /// </summary>
        /// <param name="stockOut">出库单实体.</param>
        /// <param name="items">出库单明细列表.</param>
        /// <returns>出库单系统编号</returns>
        /// <remarks>
        /// 2013/6/26 何方 创建
        /// </remarks>
        int CreateStockOut(WhStockOut stockOut,IList<WhStockOutItem> items);

        /// <summary>
        /// 添加出库单明细
        /// </summary>
        /// <param name="stockOutSysNo">出库单系统编号.</param>
        /// <param name="item">出库单明细实体.</param>
        /// <returns>出库单明细系统编号</returns>
        /// <remarks>
        /// 2013/6/26 何方 创建
        /// </remarks>
        int AddStockOutItem(int stockOutSysNo, WhStockOutItem item);

        /// <summary>
        /// </summary>
        /// <param name="stockOutSysNo">出库单系统编号.</param>
        /// <param name="status">出库单状态</param>
        /// <remarks>
        /// 2013/6/27 何方 创建
        /// </remarks>
        void UpdateStockOutStatus(int stockOutSysNo,Model.WorkflowStatus.WarehouseStatus.出库单状态 status,int userSysNo);

 
        /// <summary>
        ///     查询所有具有相同商品编号，商品数量的出库单
        /// </summary>
        /// <returns>出库单实体列表</returns>
        /// <remarks>2013-06-08 周瑜 创建</remarks>
        IList<CBWhStockOut> GetSingleBatchDO();

        /// <summary>
        ///     获取出库单的详细信息
        /// </summary>
        /// <param name="sysno">出库单系统编号</param>
        /// <returns>出库单详细信息实体</returns>
        /// <remarks>2013-06-25 周瑜 创建</remarks>
        CBWhStockOut GetStockOutInfo(int sysno);

        /// <summary>
        ///     根据输入条件查询出库单
        /// </summary>
        /// <param name="condition">搜索条件</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <returns>出库单实体列表</returns>
        /// <remarks>2013-06-08 周瑜 创建</remarks>
        PagedList<CBWhStockOut> Search(StockOutSearchCondition condition, int pageIndex, int pageSize);

        /// <summary>
        ///  修改出库单
        /// </summary>
        /// <param name="model">用于修改出库单的实体</param>
        /// <returns>出库单实体</returns>
        /// <remarks>2013-06-08 周瑜 创建</remarks>
        void UpdateStockOut(WhStockOut model);
 

        /// <summary>
        ///     打印拣货单
        /// </summary>
        /// <param name="model">出库单实体</param>
        /// <remarks>2013-06-08 周瑜 创建</remarks>
        void PrintPickingOrder(WhStockOut model);

        /// <summary>
        ///     打印装箱单
        /// </summary>
        /// <param name="model">出库单实体</param>
        /// <remarks>2013-06-08 周瑜 创建</remarks>
        void PrintPackageOrder(WhStockOut model);

        /// <summary>
        ///     打印发票
        /// </summary>
        /// <param name="model">出库单实体</param>
        /// <remarks>2013-06-08 周瑜 创建</remarks>
        void PrintInvoice(WhStockOut model);

        /// <summary>
        ///     打印运单
        /// </summary>
        /// <param name="model"></param>
        void PrintWaybill(WhStockOut model);

  

        /// <summary>
        ///     修改库存
        /// </summary>
        /// <param name="model">出库单实体</param>
        /// <remarks>2013-06-08 周瑜 创建</remarks>
        void UpdateInventory(WhStockOut model);

        /// <summary>
        ///     记录出库日志
        /// </summary>
        /// <remarks>2013-06-08 周瑜 创建</remarks>
        void OutStockLog();

        /// <summary>
        ///   出库
        /// </summary>
        /// <param name="model">出库单实体</param>
        /// <remarks>2013-06-08 周瑜 创建</remarks>
        void OutStock(WhStockOut model);

        /// <summary>
        ///     新增出库单
        /// </summary>
        /// <param name="model">用于新增出库单的实体</param>
        /// <remarks>2013-06-08 周瑜 创建</remarks>
        void Insert(WhStockOut model);

        /// <summary>
        ///     逻辑删除出库单中的商品
        /// </summary>
        /// <param name="items">将要删除的商品列表</param>
        /// <remarks>2013-06-08 周瑜 创建</remarks>
        void RemoveItem(IList<WhStockOutItem> items);

        /// <summary>
        ///     根据主键获取出库单
        /// </summary>
        /// <param name="outStockSysNo">出库单系统编号.</param>
        /// <returns>出库单实体</returns>
        /// <remarks>
        ///     2013/6/18 何方 创建
        /// </remarks>
        WhStockOut Get(int outStockSysNo);

        /// <summary>
        ///     根据订单编号获取出库单列表
        /// </summary>
        /// <param name="orderId">订单编号</param>
        /// <returns>出库单列表</returns>
        /// <remarks>2013-06-25 朱成果 创建</remarks>
        IList<WhStockOut> GetWhStockOutListByOrderID(int orderId);

        /// <summary>
        /// 作废出库单
        /// </summary>
        /// <param name="sysNo">出库单系统编号</param>
        /// <param name="userSysNo">用户系统编号</param>
        /// <param name="text">出库单作废原因</param>
        /// <returns>成功返回true,失败返回false</returns>
        /// <remarks>2013-07-01 周瑜 创建</remarks>
        Result CancelStockOut(int sysNo, int userSysNo, string text);

        /// <summary>
        /// 根据系统编号集合获取出库单列表
        /// </summary>
        /// <param name="sysNos">出库单系统编号数组</param>
        /// <returns>出库单列表</returns>
        /// <remarks>2013-07-04 沈强 创建</remarks>
        IList<WhStockOut> GetWhStockOutListBySysNos(int[] sysNos);
    }
}