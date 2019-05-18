using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Model;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;
using Hyt.Model.Parameter;

namespace Hyt.DataAccess.Warehouse
{
    /// <summary>
    /// 库存信息
    /// </summary>
    /// <param name="filter">库存信息</param>
    /// <returns>返回库存信息</returns>
    /// <remarks>2015-08-27 王耀发 创建</remarks>
    public abstract class IPdProductStockDao : Hyt.DataAccess.Base.DaoBase<IPdProductStockDao>
    {

        /// <summary>
        /// 检查产品指定仓库库存是否足够减（包含锁定库存）
        /// </summary>
        /// <param name="warehouseSysNo">仓库系统编号</param>
        /// <param name="productSysNo">产品系统编号</param>
        /// <param name="qty">待减的数量</param>
        /// <returns></returns>
        /// <remarks>2018-01-17 杨浩 创建</remarks>
        public abstract bool HasProductStock(int warehouseSysNo, int productSysNo, int qty);
         /// <summary>
        /// 库存查询
        /// </summary>
        /// <param name="erpCode">商品编码</param>
        /// <param name="warehouseSysNo">仓库编号</param>
        /// <returns></returns>
        /// <remarks>2016-8-8 杨浩 添加</remarks>
        public abstract IList<Inventory> GetInventory(string[] erpCode, int warehouseSysNo);

        /// <summary>
        /// 获取产品所在的仓库列表
        /// </summary>
        /// <param name="productSysNo">产品系统编号</param>
        /// <returns>仓库列表</returns>
        /// <remarks>2015-9-9 杨浩 创建</remarks>
        public abstract IList<PdProductStock> GetProductStockListByProductSysNo(int productSysNo);
        /// <summary>
        /// 获取产品所在的仓库列表
        /// </summary>
        /// <param name="productSysNo">产品系统编号</param>
        /// <returns>仓库列表</returns>
        /// <remarks>2016-6-3 王耀发 创建</remarks>
        public abstract IList<PdProductStockList> GetProStockListByProductSysNo(int productSysNo);
        /// <summary>
        /// 获取运费模板列表
        /// </summary>
        /// <param name="sysNo">运费模板系统编号</param>
        /// <returns>运费模板列表</returns>
        /// <remarks>2015-08-06 王耀发 创建</remarks>
        public abstract Pager<CBPdProductStockList> GetPdProductStockList(ParaProductStockFilter filter);


      



        /// <summary>
        /// 获得仓库产品详情
        /// </summary>
        /// <param name="WarehouseSysNo">仓库编号</param>
        /// <param name="PdProductSysNo">产品编号</param>
        /// <returns></returns>
        /// <remarks>
        /// 2015-08-06 王耀发 创建
        /// 2016-1-1 杨浩 添加注释
        /// </remarks>
        public abstract PdProductStock GetEntityByWP(int WarehouseSysNo, int PdProductSysNo);

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>新增记录编号</returns>
        /// <remarks>2015-08-06 王耀发 创建</remarks>
        public abstract int Insert(PdProductStock entity);

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>修改记录编号</returns>
        /// <remarks>2015-08-21  王耀发 创建</remarks>
        public abstract int Update(PdProductStock entity);

        /// <summary>
        /// 获得商品库存，根据商品编号字符串
        /// </summary>
        /// <param name="warehouseSysNo"></param>
        /// <param name="productSysNos"></param>
        /// <returns></returns>
        public abstract IList<PdProductStock> GetPdProductStockList(int warehouseSysNo, int[] productSysNos);

        /// <summary>
        /// 更新库存数量
        /// </summary>
        /// <param name="WarehouseSysNo">仓库编号</param>
        /// <param name="PdProductSysNo">产品编号</param>
        /// <param name="Quantity">商品数</param>
        /// <remarks>2016-5-25 杨浩 添加注释并将返回值改为影响行数</remarks>
        public abstract int UpdateStockQuantity(int WarehouseSysNo, int PdProductSysNo, decimal Quantity);

        /// <summary>
        /// 更新锁定库存数，
        /// </summary>
        /// <param name="warehouseSysNo"></param>
        /// <param name="productSysNo"></param>
        /// <param name="quantity"></param>
        /// <remarks>2017-9-08 罗勤尧 创建</remarks>
        public abstract void UpdateLockStockQuantity(int warehouseSysNo, int productSysNo, int quantity);

        /// <summary>
        /// 释放锁定库存数，
        /// </summary>
        /// <param name="warehouseSysNo"></param>
        /// <param name="productSysNo"></param>
        /// <param name="quantity"></param>
        /// <remarks>2017-9-08 罗勤尧 创建</remarks>
        public abstract void RollbackLockStockQuantity(int warehouseSysNo, int productSysNo, int quantity);
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns>删除数据编号</returns>
        /// <remarks>2016-08-1  罗远康 创建</remarks>
        public abstract int Delete(int sysNo);
        /// <summary>
        /// 获取所有库存信息
        /// </summary>
        /// <returns>库存信息集合</returns>
        /// <remarks>2015-12-15 王耀发 创建</remarks>
        public abstract IList<PdProductStock> GetAllProductStock();

        /// <summary>
        /// 获取当前仓库对应的库存记录
        /// </summary>
        /// <param name="WarehouseSysNo"></param>
        /// <returns></returns>
        /// <remarks>2015-12-15 王耀发 创建</remarks>
        public abstract IList<PdProductStock> GetProStockByWarehouseSysNo(int WarehouseSysNo);

        /// <summary>
        /// 新增商品库存信息
        /// </summary>
        /// <param name="models">商品信息列表</param>
        /// <returns>空</returns>
        /// <remarks>2015-09-10 王耀发 创建</remarks>
        public abstract void CreateExcelProductStock(List<PdProductStock> models);
        public abstract void CreateExcelProductStockYS(List<PdProductStock> models);
        /// <summary>
        /// 更新商品库存信息
        /// </summary>
        /// <param name="models">商品信息列表</param>
        /// <returns>空</returns>
        /// <remarks>2015-09-10 王耀发 创建</remarks>
        public abstract void UpdateExcelProductStock(List<PdProductStock> models);

        /// <summary>
        /// 更新商品库存日期信息
        /// </summary>
        /// <param name="models">商品信息列表</param>
        /// <returns>空</returns>
        /// <remarks>2017-07-10 罗勤尧 创建</remarks>
        public abstract void UpdateExcelProductStockDate(List<PdProductStock> models);
        public abstract void UpdateExcelProductStockYS(List<PdProductStock> models);
        /// <summary>
        /// 查询导出商品库存列表
        /// </summary>
        /// <param name="sysNos"></param>
        /// <returns></returns>
        /// <remarks>2015-12-30 王耀发 创建</remarks>
        public abstract List<CBOutputPdProductStocks> GetExportProductStockList(string warehouseSysNo, List<int> sysNos);
        public abstract List<CBOutputPdProductAlarmStocks> GetExportProductStockList(string warehouseSysNo, bool bAlarm = false);
        public abstract List<CBOutputPdProductStocksYS> GetExportProductStockListYS(string warehouseSysNo, List<int> sysNos);

        /// <summary>
        /// 更新商品库存信息
        /// </summary>
        /// <param name="models">商品信息列表</param>
        /// <returns>空</returns>
        /// <remarks>2015-09-10 王耀发 创建</remarks>
        public abstract void UpdateProductStockInfo(PdProductStock model);

        /// <summary>
        /// 获取库存商品明细
        /// </summary>
        /// <param name="warehouseNo"></param>
        /// <returns></returns>
        public abstract List<PdProductStock> GetPdProductStockList(int warehouseNo);

        /// <summary>
        /// 查分销商绑定库存商品列表的分页数据列表
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        ///<remarks>
        /// 2016-03-14 杨云奕 添加
        /// </remarks>
        public abstract Pager<PdProductStockList> DoDealerPdProductStockDetailQuery(ParaProductStockFilter filter);

        /// <summary>
        /// 通过自动编号获取库存数据
        /// </summary>
        /// <param name="SysNo"></param>
        /// <returns></returns>
        public abstract PdProductStock GetPdProductStockBySysNo(int SysNo);

        public abstract IList<PdProductStock> GetAllStockList(int warehouseSysNo, IList<int> proSysNos);
        /// <summary>
        /// 同步库存
        /// </summary>
        /// <param name="warehouseSysNo">仓库系统编号</param>
        /// <param name="productSysNo">产品系统编号</param>
        /// <param name="quantity">库存数</param>
        /// <returns></returns>
        /// <remarks>2017-1-11 杨浩 创建</remarks>
        public abstract int SynchronizeStock(int warehouseSysNo, int productSysNo, int quantity);
        /// <summary>
        /// 获取无效库存的产品系统编号列表
        /// </summary>
        /// <returns></returns>
        /// <remarks>2017-3-23 杨浩 创建</remarks>
        public abstract IList<int> GetInvalidInventoryProductSysNoList();


        /// <summary>
        /// 获取运费模板列表
        /// </summary>
        /// <param name="sysNo">运费模板系统编号</param>
        /// <returns>运费模板列表</returns>
        /// <remarks>2017-08-08 吴琨 创建</remarks>
        public abstract List<CBPdProductStockList> GetPdProductStockListData(string WhInventoryStr, string whereStr, string BrandSysNo, string PdCategoryId);


        /// <summary>
        /// 根据商品编号获取条码
        /// </summary>
        /// <param name="Code"></param>
        /// <returns></returns>
        public abstract string GetBarcode(string Code);
      
    }
}

