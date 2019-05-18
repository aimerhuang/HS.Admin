using System.Globalization;
using System.Threading.Tasks;
using Extra.Erp;
using Extra.Erp.Model.Borrowing;
using Extra.Erp.Model.Sale;
using Hyt.BLL.Authentication;
using Hyt.BLL.Basic;
using Hyt.BLL.CRM;
using Hyt.BLL.Log;
using Hyt.BLL.Order;
using Hyt.BLL.Product;
using Hyt.BLL.RMA;
using Hyt.BLL.Sys;
using Hyt.DataAccess.RMA;
using Hyt.DataAccess.Warehouse;
using Hyt.Infrastructure.Pager;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.BLL.Purchase;
using Hyt.BLL.Finance;
using Extra.Erp.Model;

namespace Hyt.BLL.Warehouse
{
    /// <summary>
    /// 插入出库单
    /// </summary>
    /// <param name="model">出库单明细</param>
    /// <returns>出库单系统编号</returns>
    /// <remarks>2016-06-24 王耀发 创建</remarks>
    public class WhInventoryOutBo : BOBase<WhInventoryOutBo>
    {
        /// <summary>
        /// 插入出库单
        /// </summary>
        /// <param name="model">出库单明细</param>
        /// <returns>出库单系统编号</returns>
        /// <remarks>2016-06-24 王耀发 创建</remarks>
        public int CreateWhInventoryOut(WhInventoryOut model)
        {
            return IWhInventoryOutDao.Instance.InsertWhInventoryOut(model);
        }

        /// <summary>
        /// 通过系统编号获取入库单明细
        /// </summary>
        /// <param name="sysNo">出库单系统编号</param>
        /// <returns>返回入库单明细,包含入库商品列表</returns>
        /// <remarks>2016-06-24 王耀发 创建</remarks>
        public WhInventoryOut GetWhInventoryOut(int sysNo)
        {
            return IWhInventoryOutDao.Instance.GetWhInventoryOut(sysNo);
        }

        /// <summary>
        /// 获取出库单列表
        /// </summary>
        /// <param name="filter">查询参数</param>
        /// <param name="pageSize">页大小</param>
        /// <returns>返回入库单列表 Ilist</returns>
        /// <remarks>2016-06-24 王耀发 创建</remarks>
        public PagedList<WhInventoryOut> GetWhInventoryOutList(ParaInventoryOutFilter filter, int pageSize)
        {
            PagedList<WhInventoryOut> model = null;
            if (filter != null)
            {
                model = new PagedList<WhInventoryOut>();
                var pager = IWhInventoryOutDao.Instance.GetWhInventoryOutList(filter, pageSize);
                if (null != pager)
                {
                    model.TData = pager.Rows;
                    model.TotalItemCount = pager.TotalRows;
                    model.CurrentPageIndex = filter.CurrentPage;
                }
            }
            return model;
        }


        /// <summary>
        /// 获取调拨出库单列表
        /// </summary>
        /// <param name="filter">查询参数</param>
        /// <param name="pageSize">页大小</param>
        /// <returns>返回入库单列表 Ilist</returns>
        /// <remarks>2016-06-24 王耀发 创建</remarks>
        public PagedList<WhInventoryOut> GetWhInventoryOutListTo(ParaInventoryOutFilter filter, int pageSize)
        {
            PagedList<WhInventoryOut> model = null;
            if (filter != null)
            {
                model = new PagedList<WhInventoryOut>();
                var pager = IWhInventoryOutDao.Instance.GetWhInventoryOutListTo(filter, pageSize);
                if (null != pager)
                {
                    model.TData = pager.Rows;
                    model.TotalItemCount = pager.TotalRows;
                    model.CurrentPageIndex = filter.CurrentPage;
                }
            }
            return model;
        }



        /// <summary>
        /// 通过出库单ID获取所有商品列表
        /// </summary>
        /// <param name="inventoryOutSysNo">出库单系统SysNO</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页大小.</param>
        /// <returns>返回出库单商品列表</returns>
        /// <remarks>2016-06-24 王耀发 创建</remarks>
        public PagedList<WhInventoryOutItem> GetInventoryOutItemList(int inventoryOutSysNo, int pageIndex, int pageSize)
        {
            var model = new PagedList<WhInventoryOutItem>();
            var result = IWhInventoryOutDao.Instance.GetWhInventoryOutItemListByInventoryOutSysNo(inventoryOutSysNo, pageIndex, pageSize);
            model.Style = PagedList.StyleEnum.Mini;
            model.TData = result;
            model.TotalItemCount = IWhInventoryOutDao.Instance.GetWhInventoryOutItemListByInventoryOutSysNoCount(inventoryOutSysNo);
            return model;
        }
        /// <summary>
        /// 计算出库
        /// </summary>
        /// <param name="sysNo">出库单系统编号</param>
        /// <param name="itemList">出库商品</param>
        /// <param name="user">操作人</param>
        /// <returns>返回结果</returns>
        /// <remarks>
        /// 2016-06-24 王耀发 创建
        /// </remarks>
        public void InventoryOutComplete(int sysNo, List<WhInventoryOutItem> itemList, SyUser user)
        {
            var model = GetWhInventoryOut(sysNo);
            if (model == null || model.Status == WarehouseStatus.采购退货出库单状态.作废.GetHashCode())
            {
                throw new HytException("出库不存在或已经作废，操作无效请检查！");
            }
            else
            {
                if (!AdminAuthenticationBo.Instance.HasWareHousePrivilege(model.WarehouseSysNo))
                {
                    throw new HytException("用户没有编号为" + model.WarehouseSysNo + "仓库的权限");
                }
                model.ItemList = itemList;
                //商品出库
                ItemInventoryOut(model, user);
                //检查商品入库情况
                var data = GetInventoryOutItemList(model.SysNo, 1, Int32.MaxValue).TData;
                if (data != null)
                {
                    var list = data as List<WhInventoryOutItem>;
                    var status = list != null && list.Any(item => item.RealStockOutQuantity < item.StockOutQuantity);
                    //更新出库单状态
                    UpdateInventoryOutStatus(model.SysNo, status ? WarehouseStatus.采购退货出库单状态.部分出库 : WarehouseStatus.采购退货出库单状态.已出库,
                                        user);
                }
            }
        }
        /// <summary>
        /// 商品出库
        /// </summary>
        /// <param name="model">出库单实体</param>
        /// <param name="user">操作人</param>
        /// <returns>返回操作状态(Result.StatusCode大于等于0成功,小于0失败)</returns>
        /// 2016-06-24 王耀发 创建
        private void ItemInventoryOut(WhInventoryOut model, SyUser user)
        {
            if (model == null || model.ItemList == null || !model.ItemList.Any()) return;

            var purchaseList = new List<PurchaseInfo>();
            var warhouseInfo = BLL.Warehouse.WhWarehouseBo.Instance.GetWarehouse(model.WarehouseSysNo);

            var purchaseReturnInfo = Hyt.DataAccess.Purchase.IPrPurchaseReturnDao.Instance.GetPrPurchaseReturn(model.SourceSysNO);//采购退货单
            var _purchaseList = BLL.Purchase.PrPurchaseBo.Instance.GetPurchaseDetailsByPurchaseSysNo(purchaseReturnInfo.PurchaseSysNo);

            model.ItemList.ForEach(x =>
            {
                var data = IWhInventoryOutDao.Instance.GetWhInventoryOutItem(x.SysNo);
                x.SourceItemSysNo = data.SourceItemSysNo;
                if (data == null)
                {
                    throw new HytException(string.Format("该出库{0}无效！", x.ProductName));
                }
                var count = data.RealStockOutQuantity + x.RealStockOutQuantity;
                if (count > data.StockOutQuantity)
                {
                    throw new HytException("实际商品出库数量总和超出商品出库数量，请检查！");
                }

                data.RealStockOutQuantity = count;
                data.LastUpdateBy = user.SysNo;
                data.LastUpdateDate = DateTime.Now;
                UpdateWhInventoryOutItem(data);
                //更新退货明细的出库商量
                PrPurchaseReturnDetailsBo.Instance.UpdateOutQuantity(model.SourceSysNO, data.ProductSysNo, count);
                //减库存操作
                Hyt.BLL.Warehouse.PdProductStockBo.Instance.UpdateStockQuantity(model.WarehouseSysNo, data.ProductSysNo, -1 * x.RealStockOutQuantity);

                var purchaseItemInfo = _purchaseList.Where(j => j.ProductSysNo == x.ProductSysNo).FirstOrDefault();
                var purchaseInfo = new PurchaseInfo()
                {
                    WarehouseSysNo = model.WarehouseSysNo,
                    FDate = model.CreatedDate.ToString(),
                    WarehouseNumber = warhouseInfo.ErpCode,
                    Quantity = data.RealStockOutQuantity,
                    FNote = "平台出库单明细系统编号：" + x.SysNo,
                    Price = purchaseItemInfo.Money,
                    Amount = purchaseItemInfo.Money * data.RealStockOutQuantity,
                    ErpCode = purchaseItemInfo.ErpCode,
                    SettleDate = model.CreatedDate.ToString(),
                };
                purchaseList.Add(purchaseInfo);

            });

            //更新退货单的出库商量
            PrPurchaseReturnBo.Instance.UpdateOutQuantity(model.SourceSysNO);

            #region 同步金蝶
            if (model.SourceType == (int)WarehouseStatus.出库单来源.采购单)
            {
                string flowIdentify = model.SourceSysNO.ToString();//采购订单系统编号
                string description = model.SysNo.ToString();//入库单号                            
                var client = Extra.Erp.Kis.KisProviderFactory.CreateProvider();
                //单据编号
                //string voucherNo=EasBo.Instance.GetVoucherNoByTlowIdentify((int)Extra.Erp.Model.接口类型.采购入库退货, purchaseReturnInfo.PurchaseSysNo.ToString());
                string voucherNo = purchaseReturnInfo.PurchaseSysNo.ToString();//采购单系统编号
                client.PurchaseOutStock(purchaseList, description, model.SourceSysNO.ToString(), voucherNo);
            }
            #endregion
        }

        /// <summary>
        /// 更新商品出库信息
        /// </summary>
        /// <param name="model">出库单明细</param>
        /// <returns>返回受影响行</returns>
        /// 2016-06-24 王耀发 创建
        public int UpdateWhInventoryOutItem(WhInventoryOutItem model)
        {
            return IWhInventoryOutDao.Instance.UpdateWhInventoryOutItem(model);
        }
        /// <summary>
        /// 更新出库单状态
        /// </summary>
        /// <param name="stockInSysNo">出库单系统编号</param>
        /// <param name="status">出库单状态</param>
        /// <param name="user">操作人</param>
        /// <returns>返回结果</returns>
        /// 2016-06-24 王耀发 创建
        public void UpdateInventoryOutStatus(int inventoryOutSysNo, WarehouseStatus.采购退货出库单状态 status, SyUser user)
        {
            var model = IWhInventoryOutDao.Instance.GetWhInventoryOut(inventoryOutSysNo);
            if (null == model || model.Status == WarehouseStatus.采购退货出库单状态.作废.GetHashCode())
            {
                throw new HytException("出库不存在或已经作废，不能操作，请检查！");
            }
            else
            {
                model.Status = status.GetHashCode();
                model.LastUpdateBy = user.SysNo;
                model.LastUpdateDate = DateTime.Now;
                IWhInventoryOutDao.Instance.UpdateWhInventoryOut(model);
            }
        }
        /// <summary>
        /// 作废出库单
        /// </summary>
        /// <param name="sysNo">出库单系统编号</param>
        /// <param name="user">操作用户</param>
        /// <returns>成功返回true,失败返回false</returns>
        /// <remarks>2016-06-27 王耀发 创建</remarks>
        public bool InventoryOutCancel(int sysNo, SyUser user)
        {
            var model = GetWhInventoryOut(sysNo);
            if (null == model) throw new HytException("出库单不存在！");
            if (!AdminAuthenticationBo.Instance.HasWareHousePrivilege(model.WarehouseSysNo))
            {
                throw new HytException("用户没有作废该条出库单的权限！");
            }
            FnReceiptVoucher ReceiptVoucher = null;
            if (model.SourceType != (int)WarehouseStatus.出库单来源.调货单)
            {
                //获取对应收款单
                ReceiptVoucher = FnReceiptVoucherBo.Instance.GetEntity((int)Hyt.Model.WorkflowStatus.FinanceStatus.收款来源类型.采购退货单, model.SourceSysNO);
                if (ReceiptVoucher.Status == (int)FinanceStatus.收款单状态.已确认)
                {
                    throw new HytException("该条出库单对应的收款单已确认，不能作废！");
                }
            }
            model.Status = WarehouseStatus.采购退货出库单状态.作废.GetHashCode();
            var id = UpdateWhInventoryOut(model);

            if (id <= 0) return false;
            if (model.SourceType != (int)WarehouseStatus.出库单来源.调货单)
            {
                //更新采购退货单状态
                Hyt.BLL.Purchase.PrPurchaseReturnBo.Instance.UpdateStatus(model.SourceSysNO, (int)PurchaseStatus.采购退货单状态.待审核);
                //更新收款单状态
                ReceiptVoucher.Status = (int)FinanceStatus.收款单状态.作废;
                ReceiptVoucher.LastUpdateBy = user.SysNo;
                ReceiptVoucher.LastUpdateDate = DateTime.Now;
                FnReceiptVoucherBo.Instance.Update(ReceiptVoucher);
            }
            return true;
        }
        /// <summary>
        /// 更新出库单
        /// </summary>
        /// <param name="model">出库单明细</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013-06-09 王耀发 创建</remarks>
        public int UpdateWhInventoryOut(WhInventoryOut model)
        {
            return IWhInventoryOutDao.Instance.UpdateWhInventoryOut(model);
        }

        public List<WhInventoryOut> GetWhInventoryOutList(DateTime dateTime)
        {
            return IWhInventoryOutDao.Instance.GetWhInventoryOutList(dateTime);
        }

        /// <summary>
        /// 根据来源单号获取入库单
        /// </summary>
        /// <param name="sysNo"></param>
        /// <returns>2018-01-16 罗熙 创建</returns>
        public WhInventoryOut GetWhInventoryOutToSourceSysNo(int sysNo)
        {
            return IWhInventoryOutDao.Instance.GetWhInventoryOutToSourceSysNo(sysNo);
        }
    }
}
