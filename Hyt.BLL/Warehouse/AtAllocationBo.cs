using Hyt.BLL.Log;
using Hyt.Model;
using Hyt.Model.Generated;
using Hyt.Model.WorkflowStatus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.BLL.Warehouse
{
    public class AtAllocationBo : BOBase<AtAllocationBo>
    {
        /// <summary>
        /// 分页查询调拨单列表
        /// </summary>
        /// <param name="pager"></param>
        /// <returns></returns>
        /// <remarks>2016-06-28 陈海裕 创建</remarks>
        public Pager<CBAtAllocation> QueryAtAllocationPager(Pager<CBAtAllocation> pager)
        {
            return DataAccess.Warehouse.IAtAllocationDao.Instance.QueryAtAllocationPager(pager);
        }

        /// <summary>
        /// 获取调拨单实体
        /// </summary>
        /// <param name="sysNo"></param>
        /// <returns></returns>
        /// <remarks>2016-06-29 陈海裕 创建</remarks>
        public AtAllocation GetAtAllocationEntity(int sysNo)
        {
            return DataAccess.Warehouse.IAtAllocationDao.Instance.GetAtAllocationEntity(sysNo);
        }

        /// <summary>
        /// 创建调拨单
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <remarks>2016-06-29 陈海裕 创建</remarks>
        public AtAllocation CreateAtAllocation(AtAllocation model)
        {
            model.CheckDate = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            model.CreatedDate = DateTime.Now;
            model.Status = (int)Model.WorkflowStatus.WarehouseStatus.库存调拨单状态.待审核;
            return DataAccess.Warehouse.IAtAllocationDao.Instance.CreateAtAllocation(model);
        }

        /// <summary>
        /// 更新调拨单
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <remarks>2016-06-30 陈海裕 创建</remarks>
        public bool UpdateAtAllocation(AtAllocation model)
        {
            return DataAccess.Warehouse.IAtAllocationDao.Instance.UpdateAtAllocation(model) > 0;
        }

        /// <summary>
        /// 新增调拨单明细
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <remarks>2016-06-29 陈海裕 创建</remarks>
        public Result AddAtAllocationItem(AtAllocationItem model)
        {
            Result result = new Result();
            if (DataAccess.Warehouse.IAtAllocationDao.Instance.AddAtAllocationItem(model) > 0)
            {
                result.Status = true;
            }
            else
            {
                result.Status = false;
                result.Message = "添加失败";
            }
            return result;
        }

        /// <summary>
        /// 获取库存调拨单商品列表（添加调拨商品用）
        /// </summary>
        /// <param name="atAllocationSysNo"></param>
        /// <returns></returns>
        /// <remarks>2016-06-29 陈海裕 创建</remarks>
        public List<AtAllocationItem> GetAtAllocationProducts(int atAllocationSysNo)
        {
            return DataAccess.Warehouse.IAtAllocationDao.Instance.GetAtAllocationProducts(atAllocationSysNo);
        }

        /// <summary>
        /// 删除调拨单明细
        /// </summary>
        /// <param name="sysNo"></param>
        /// <returns></returns>
        /// <remarks>2016-06-29 陈海裕 创建</remarks>
        public bool DeleteAtAllocationItem(int sysNo)
        {
            return DataAccess.Warehouse.IAtAllocationDao.Instance.DeleteAtAllocationItem(sysNo) > 0;
        }

        /// <summary>
        /// 分页查询调拨单明细列表
        /// </summary>
        /// <param name="pager"></param>
        /// <returns></returns>
        /// <remarks>2016-06-29 陈海裕 创建</remarks>
        public Pager<AtAllocationItem> QueryAtAllocationItemPager(Pager<AtAllocationItem> pager)
        {
            return DataAccess.Warehouse.IAtAllocationDao.Instance.QueryAtAllocationItemPager(pager);
        }

        /// <summary>
        /// 更新调拨单明细
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <remarks>2016-06-29 陈海裕 创建</remarks>
        public bool UpdateAtAllocationItem(AtAllocationItem model)
        {
            return DataAccess.Warehouse.IAtAllocationDao.Instance.UpdateAtAllocationItem(model) > 0;
        }

        /// <summary>
        /// 生成调拨出库单
        /// </summary>
        /// <param name="atAllocationSysNo"></param>
        /// <returns></returns>
        /// <remarks>2016-06-30 陈海裕 创建</remarks>
        public int CreateALCInventoryOutOrder(int atAllocationSysNo)
        {

            var atAllocationEntity = BLL.Warehouse.AtAllocationBo.Instance.GetAtAllocationEntity(atAllocationSysNo);
            var itemList = BLL.Warehouse.AtAllocationBo.Instance.GetAtAllocationProducts(atAllocationSysNo);
            if (atAllocationEntity == null || itemList == null || !itemList.Any())
                return 0;

            WhInventoryOut invenOutEntity = new WhInventoryOut();
            invenOutEntity.CreatedBy = BLL.Authentication.AdminAuthenticationBo.Instance.Current == null ? 12 : BLL.Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo;
            invenOutEntity.CreatedDate = DateTime.Now;
            invenOutEntity.DeliveryType = 0;
            invenOutEntity.IsPrinted = 0;
            invenOutEntity.LastUpdateBy = invenOutEntity.CreatedBy;
            invenOutEntity.LastUpdateDate = invenOutEntity.CreatedDate;
            invenOutEntity.Remarks = "调拨出库";
            invenOutEntity.SourceSysNO = atAllocationEntity.SysNo;
            invenOutEntity.SourceType = (int)WarehouseStatus.出库单来源.调货单;
            invenOutEntity.Stamp = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            invenOutEntity.Status = (int)WarehouseStatus.采购退货出库单状态.待出库;
            invenOutEntity.TransactionSysNo = BLL.Basic.ReceiptNumberBo.Instance.GetAllocationOutNo();
            invenOutEntity.WarehouseSysNo = atAllocationEntity.OutWarehouseSysNo;
            invenOutEntity.ItemList = new List<WhInventoryOutItem>();
            foreach (var item in itemList)
            {
                var productName = BLL.Product.PdProductBo.Instance.GetProductEasName(item.ProductSysNo);
                WhInventoryOutItem temp = new WhInventoryOutItem();
                temp.CreatedBy = invenOutEntity.CreatedBy;
                temp.CreatedDate = invenOutEntity.CreatedDate;
                temp.LastUpdateBy = invenOutEntity.CreatedBy;
                temp.LastUpdateDate = invenOutEntity.CreatedDate;
                temp.ProductName = productName;
                temp.ProductSysNo = item.ProductSysNo;
                temp.RealStockOutQuantity = item.Quantity;  //0
                temp.Remarks = "";
                temp.SourceItemSysNo = item.SysNo;
                temp.StockOutQuantity = item.Quantity;
                invenOutEntity.ItemList.Add(temp);
            }

            return WhInventoryOutBo.Instance.CreateWhInventoryOut(invenOutEntity);           
        }

        /// <summary>
        /// 调拨出库单出库
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <remarks>2016-07-01 陈海裕 创建</remarks>
        public Result DoAAOutConfirm(WhInventoryOut model)
        {
            var result = new Result() { Status = false, Message = "出库失败" };

            var wareList = WhWarehouseBo.Instance.GetAllWarehouseList();
            //出库单是否处于待出库状态
            var tempInvenOut = BLL.Warehouse.WhInventoryOutBo.Instance.GetWhInventoryOut(model.SysNo);
            if (tempInvenOut == null)
            {
                result.Message = "出库单不存在";
                return result;
            }

            model.ItemList = BLL.Warehouse.WhInventoryOutBo.Instance.GetInventoryOutItemList(model.SysNo, 1, 4000).TData.ToList();
            //检查实际出库数量
            foreach (var item in model.ItemList)
            {
                if (BLL.Warehouse.PdProductStockBo.Instance.HasProductStock(model.WarehouseSysNo, item.ProductSysNo, item.RealStockOutQuantity))
                {
                    var warehouseName = BLL.Warehouse.WhWarehouseBo.Instance.GetWarehouseName(model.WarehouseSysNo);
                    result.Message = "仓库[" + warehouseName + "]商品（" + item.ProductSysNo + "）出库数量大于库存数";
                    return result;
                }
            }

            var tempOutWarehouse = wareList.First(p => p.SysNo == model.WarehouseSysNo);
            var tempInStock = AtAllocationBo.Instance.GetAtAllocationEntity(tempInvenOut.SourceSysNO);
            var tempInWarehouse = wareList.First(p => p.SysNo == tempInStock.EnterWarehouseSysNo);
            int currentUserSysNo = BLL.Authentication.AdminAuthenticationBo.Instance.Current == null ? 12 : BLL.Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo;
            //var tempOutItemList = BLL.Warehouse.WhInventoryOutBo.Instance.GetInventoryOutItemList(model.SysNo, 1, 4000).TData;
            foreach (var item in model.ItemList)
            {
                //减库存
                BLL.Warehouse.PdProductStockBo.Instance.UpdateStockQuantity(model.WarehouseSysNo, item.ProductSysNo,item.RealStockOutQuantity);
                // 更新出库单实际出库数量RealStockOutQuantity
                //WhInventoryOutItem tempOutItem = tempOutItemList.Where(o => o.SysNo == item.SysNo).FirstOrDefault();
                item.LastUpdateBy = currentUserSysNo;
                item.LastUpdateDate = DateTime.Now;
                item.RealStockOutQuantity = item.RealStockOutQuantity;
                BLL.Warehouse.WhInventoryOutBo.Instance.UpdateWhInventoryOutItem(item);
            }

            //更新出库单状态
            tempInvenOut.LastUpdateBy = currentUserSysNo;
            tempInvenOut.LastUpdateDate = DateTime.Now;
            tempInvenOut.Status = (int)WarehouseStatus.库存调拨出库单状态.已出库;
            if (BLL.Warehouse.WhInventoryOutBo.Instance.UpdateWhInventoryOut(tempInvenOut) > 0)
            {
                result.Status = true;
                result.Message = "出库成功";
            }
            return result;
        }

        /// <summary>
        /// 检查调拨单产品是否有0的数量
        /// </summary>
        /// <param name="atAllocationSysNo">调拨单系统编号</param>
        /// <returns></returns>
        /// <remarks>2018-01-17 杨浩 创建</remarks>
        public bool ExistAtAllocationProductQtyZero(int atAllocationSysNo)
        {
            return DataAccess.Warehouse.IAtAllocationDao.Instance.ExistAtAllocationProductQtyZero(atAllocationSysNo);
        }
    }
}
