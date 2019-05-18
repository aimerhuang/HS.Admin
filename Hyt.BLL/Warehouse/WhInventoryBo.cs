using Hyt.BLL.Log;
using Hyt.Model;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.BLL.Warehouse
{
    public class WhInventoryBo : BOBase<WhInventoryBo>
    {
        /// <summary>
        /// 创建盘点单
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <remarks>2016-06-20 陈海裕 创建</remarks>
        public WhInventory CreateWhInventory(WhInventory model)
        {
            return DataAccess.Warehouse.IWhInventoryDao.Instance.CreateWhInventory(model);
        }

        /// <summary>
        /// 分页查询盘点单列表
        /// </summary>
        /// <param name="pager"></param>
        /// <returns></returns>
        /// <remarks>2016-06-21 陈海裕 创建</remarks>
        public Pager<CBWhInventory> QueryWhInventoryPager(Pager<CBWhInventory> pager)
        {
            return DataAccess.Warehouse.IWhInventoryDao.Instance.QueryWhInventoryPager(pager);
        }

        /// <summary>
        /// 获取盘点单实体
        /// </summary>
        /// <param name="sysNo"></param>
        /// <returns></returns>
        /// <remarks>2016-06-21 陈海裕 创建</remarks>
        public WhInventory GetWhInventoryEntity(int sysNo)
        {
            return DataAccess.Warehouse.IWhInventoryDao.Instance.GetWhInventoryEntity(sysNo);
        }

        /// <summary>
        /// 获取库存盘点单商品列表（添加盘点商品用）
        /// </summary>
        /// <param name="inventorySysNo"></param>
        /// <returns></returns>
        /// <remarks>2016-06-20 陈海裕 创建</remarks>
        public List<WhInventoryDetail> GetWhInventoryProducts(int inventorySysNo)
        {
            return DataAccess.Warehouse.IWhInventoryDao.Instance.GetWhInventoryProducts(inventorySysNo);
        }

        /// <summary>
        /// 获取库存盘点单商品列表（单个）
        /// </summary>
        /// <param name="productSysNo"></param>
        /// <param name="inventorySysNo"></param>
        /// <returns></returns>
        /// <remarks>2016-07-21 陈海裕 创建</remarks>
        public WhInventoryDetail GetWhInventoryDetailEntity(int productSysNo, int inventorySysNo)
        {
            return DataAccess.Warehouse.IWhInventoryDao.Instance.GetWhInventoryDetailEntity(productSysNo, inventorySysNo);
        }

        /// <summary>
        /// 更新盘点单
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <remarks>2016-06-24 陈海裕 创建</remarks>
        public bool UpdateWhInventory(WhInventory model)
        {
            return DataAccess.Warehouse.IWhInventoryDao.Instance.UpdateWhInventory(model) > 0;
        }

        /// <summary>
        /// 添加盘点单明细
        /// </summary>
        /// <param name="inventoryDetail"></param>
        /// <returns></returns>
        /// <remarks>2016-06-20 陈海裕 创建</remarks>
        public Result AddWhInventoryDetail(WhInventoryDetail inventoryDetail)
        {
            Result result = new Result();
            if (DataAccess.Warehouse.IWhInventoryDao.Instance.AddWhInventoryDetail(inventoryDetail) > 0)
            {
                result.Status = true;
            }
            else
            {
                result.Message = "添加失败";
            }
            return result;
        }

        /// <summary>
        /// 分页查询盘点单明细
        /// </summary>
        /// <param name="pager"></param>
        /// <returns></returns>
        /// <remarks>2016-06-21 陈海裕 创建</remarks>
        public Pager<CBWhInventoryDetail> QueryWhInventoryDetailPager(Pager<CBWhInventoryDetail> pager, int whPositionSysNo = 0, string searchKeyWord = "")
        {
            return DataAccess.Warehouse.IWhInventoryDao.Instance.QueryWhInventoryDetailPager(pager, whPositionSysNo, searchKeyWord);
        }

        /// <summary>
        /// 删除盘点单商品
        /// </summary>
        /// <param name="sysNo"></param>
        /// <returns></returns>
        /// <remarks>2016-06-23 陈海裕 创建</remarks>
        public bool DeleteWhInventoryDetail(int sysNo)
        {
            return DataAccess.Warehouse.IWhInventoryDao.Instance.DeleteWhInventoryDetail(sysNo) > 0;
        }

        /// <summary>
        /// 更新盘点单商品
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <remarks>2016-06-24 陈海裕 创建</remarks>
        public bool UpdateWhInventoryDetail(WhInventoryDetail model)
        {
            return DataAccess.Warehouse.IWhInventoryDao.Instance.UpdateWhInventoryDetail(model) > 0;
        }
        /// <summary>
        /// 通过日期获取所有的盘点单数据
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        /// <remarks>2016-12-26 杨云奕 添加</remarks>
        public List<WhInventory> GetAllWhInventoryListByDate(DateTime dateTime)
        {
            return DataAccess.Warehouse.IWhInventoryDao.Instance.GetAllWhInventoryListByDate(dateTime);
        }

        #region 出库商品数据导出Excel
        public void ChuKuExportData(List<CBWhStockOut> list, string userIp, int operatorSysno)
        {
            try
            {
                // 查询商品
                List<WhStockOutOutput> outputData = new List<WhStockOutOutput>();
                foreach (var item in list)
                {
                    WhStockOutOutput model = new WhStockOutOutput();
                    model.出库单号 = "01_" + item.SysNo + "_" + item.OrderSysNO;
                    model.收货人 = item.ReceiverName;
                    model.仓库 = Hyt.BLL.Warehouse.WhWarehouseBo.Instance.GetWarehouseName(item.WarehouseSysNo);
                    model.配送方式 = Hyt.BLL.Logistics.DeliveryTypeBo.Instance.GetDeliveryTypeName(item.DeliveryTypeSysNo);
                    model.应收金额 = item.Receivable.ToString("C");
                    model.创建时间 = item.SoCreateDate.ToString("yyyy-MM-dd HH:mm");
                    model.是否开票 = item.InvoiceSysNo > 0 ? "是" : "否";
                    model.来源 = item.OrderSource;
                    model.状态 = ((WarehouseStatus.出库单状态)item.Status).ToString();
                    outputData.Add(model);
                }
                var fileName = string.Format("出库单({0})", DateTime.Now.ToString("yyyyMMddHHmmss"));
                //导出Excel，并设置表头列名
                Util.ExcelUtil.Export<WhStockOutOutput>(outputData,
                    new List<string> { "出库单号_订单号", "收货人", "仓库", "配送方式", "应收金额", "创建时间", "是否开票", "来源", "状态" },
                    fileName);
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Info, LogStatus.系统日志来源.后台, "出库单导出excel",
                                         LogStatus.系统日志目标类型.ExcelExporting, 0, null, userIp, operatorSysno);
            }
            catch (Exception ex)
            {
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "出库单导出excel",
                                         LogStatus.系统日志目标类型.ExcelExporting, 0, ex, userIp, operatorSysno);
            }
        }
        #endregion
    }
}
