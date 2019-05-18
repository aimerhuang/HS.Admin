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
using Hyt.Model.Generated;
using Extra.Erp.Model;
using Hyt.DataAccess.Purchase;
using Hyt.DataAccess.Product;

namespace Hyt.BLL.Warehouse
{
    /// <summary>
    /// 入库单维护Bo
    /// </summary>
    /// <remarks>2013-06-08 周唐炬 创建</remarks>
    public class InStockBo : BOBase<InStockBo>, IInStockBo
    {
        /// <summary>
        /// 插入入库单
        /// </summary>
        /// <param name="model">入库单明细</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013-06-09 周唐炬 创建</remarks>
        public int CreateStockIn(WhStockIn model)
        {
            var id = IInStockDao.Instance.InsertWhStockIn(model);
            //如果创建已经入库的入库单则调用eas入库接口
            if (model.Status == WarehouseStatus.入库单状态.已入库.GetHashCode())
            {
                EasInStock(model);
            }
            return id;
        }

        /// <summary>
        /// 退货从出库单创建入库单
        /// </summary>
        /// <param name="stockOut">出库单实体.</param>
        /// <param name="returnNumber">商品退货数量</param>
        /// <param name="operatorSysNo">操作人.</param>
        /// <param name="sourceType">来源单据类型.</param>
        /// <param name="sourceSysNo">来源单据系统编号.</param>
        /// <param name="transactionSysNo">事务编号.</param>
        /// <param name="remarks">备注.</param>
        /// <returns></returns>
        /// <remarks>
        /// 2013/7/11 何方 创建
        /// </remarks>
        public int CreateStockIn(WhStockOut stockOut, Dictionary<int, int> returnNumber, int operatorSysNo, WarehouseStatus.入库单据类型 sourceType, int sourceSysNo, string transactionSysNo = "", string remarks = "")
        {

            var stockIn = new WhStockIn
            {
                CreatedBy = operatorSysNo,
                CreatedDate = DateTime.Now,
                DeliveryType = (int)WarehouseStatus.入库物流方式.拒收,
                IsPrinted = (int)WarehouseStatus.是否已经打印拣货单.否,
                Remarks = remarks,
                SourceSysNO = sourceSysNo,
                SourceType = (int)sourceType,
                Status = (int)WarehouseStatus.入库单状态.待入库,
                TransactionSysNo = transactionSysNo,
                WarehouseSysNo = stockOut.WarehouseSysNo,
                ItemList = new List<WhStockInItem>(),
                LastUpdateBy = operatorSysNo,
                LastUpdateDate = DateTime.Now
            };
            if (returnNumber != null && returnNumber.Any())
            {
                stockIn.Status = (int)WarehouseStatus.入库单状态.待入库;
                //入库单明细
                foreach (var orderItemSysNo in returnNumber.Keys)
                {
                    var stockOutItem = stockOut.Items.SingleOrDefault(x => x.OrderItemSysNo == orderItemSysNo);
                    if (stockOutItem == null)
                    {
                        throw new Exception(string.Format("出库单:{0}不包含商品{1}", stockOut.SysNo, orderItemSysNo));
                    }

                    stockIn.ItemList.Add(new WhStockInItem
                    {
                        CreatedBy = operatorSysNo,
                        CreatedDate = DateTime.Now,
                        ProductName = stockOutItem.ProductName,
                        ProductSysNo = stockOutItem.ProductSysNo,
                        StockInQuantity = returnNumber[orderItemSysNo],//退货数量
                        RealStockInQuantity = 0,
                        LastUpdateBy = operatorSysNo,
                        LastUpdateDate = DateTime.Now
                    });
                }
            }
            return InStockBo.Instance.CreateStockIn(stockIn);
        }

        /// <summary>
        /// 更新入库单
        /// </summary>
        /// <param name="model">入库单明细</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013-06-09 周唐炬 创建</remarks>
        public int UpdateStockIn(WhStockIn model)
        {
            return IInStockDao.Instance.UpdateWhStockIn(model);
        }

        /// <summary>
        /// 获取入库单列表
        /// </summary>
        /// <param name="filter">查询参数</param>
        /// <param name="pageSize">页大小</param>
        /// <returns>返回入库单列表 Ilist</returns>
        /// <remarks>2013-06-08 周唐炬 创建</remarks>
        public PagedList<WhStockIn> GetStockInList(ParaInStockFilter filter, int pageSize)
        {
            PagedList<WhStockIn> model = null;
            if (filter != null)
            {
                model = new PagedList<WhStockIn>();
                var pager = IInStockDao.Instance.GetWhStockInList(filter, pageSize);
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
        /// 作废入库单
        /// </summary>
        /// <param name="sysNo">入库单系统编号</param>
        /// <param name="user">操作用户</param>
        /// <returns>成功返回true,失败返回false</returns>
        /// <remarks>2013-06-08 周唐炬 创建</remarks>
        /// <remarks>
        /// 2013-07-18 朱成果 添加RMA入库单作废逻辑
        /// 2016-6-20 杨浩 添加采购入库单作废逻辑
        /// </remarks>
        public bool InStockCancel(int sysNo, SyUser user)
        {
            var model = GetStockIn(sysNo);
            if (null == model) throw new HytException("入库单不存在！");
            if (!AdminAuthenticationBo.Instance.HasWareHousePrivilege(model.WarehouseSysNo))
            {
                throw new HytException("用户没有作废该条入库单的权限！");
            }
            model.Status = WarehouseStatus.入库单状态.作废.GetHashCode();
            var id = UpdateStockIn(model);
            if (id <= 0) return false;
            if (model.SourceType == (int)WarehouseStatus.入库单据类型.RMA单)
            {
                RmaBo.Instance.RMAInStockCancelCallBack(model, user);//退货单设置成待审核状态
            }
            else if (model.SourceType == (int)WarehouseStatus.入库单据类型.采购单)
            {
                PrPurchaseBo.Instance.PrInStockCancelCallBack(model, user);//采购单设置成待审核状态
            }
            return true;
        }

        /// <summary>
        /// 计算入库
        /// </summary>
        /// <param name="sysNo">入库单系统编号</param>
        /// <param name="invoiceSysNo">发票系统编号</param>
        /// <param name="itemList">入库商品</param>
        /// <param name="user">操作人</param>
        /// <returns>返回结果</returns>
        /// <remarks>
        /// 2013-06-08 周唐炬 创建
        /// 2013-07-18 朱成果 修改
        /// 2016-6-21 杨浩 增加采购单状态修改
        /// </remarks>
        public void InStockComplete(int sysNo, int? invoiceSysNo, List<WhStockInItem> itemList, SyUser user)
        {
            var model = GetStockIn(sysNo);
            if (model == null || model.Status == WarehouseStatus.入库单状态.作废.GetHashCode())
            {
                throw new HytException("入库不存在或已经作废，操作无效请检查！");
            }
            else
            {
                if (!AdminAuthenticationBo.Instance.HasWareHousePrivilege(model.WarehouseSysNo))
                {
                    throw new HytException("用户没有编号为" + model.WarehouseSysNo + "仓库的权限");
                }
                model.InvoiceSysNo = invoiceSysNo;
                model.ItemList = itemList;
                //商品入库
                ItemInStock(model, user);
                //检查商品入库情况
                var data = GetStockInItemList(model.SysNo, 1, Int32.MaxValue).TData;

                if (data != null)
                {
                    var list = data as List<WhStockInItem>;

                 

                    var status = list != null && list.Any(item => item.RealStockInQuantity < item.StockInQuantity);
                    //更新入库单状态
                    UpdateStockInStatus(model.SysNo, status ? WarehouseStatus.入库单状态.部分入库 : WarehouseStatus.入库单状态.已入库,
                                        user);

                    if (model.SourceType == (int)WarehouseStatus.入库单据类型.采购单)
                    {
                        if (status)
                            BLL.Purchase.PrPurchaseBo.Instance.UpdateStatus(model.SourceSysNO, -1, (int)PurchaseStatus.采购单入库状态.入库中, (int)PurchaseStatus.采购单状态.入库中);
                        else
                            BLL.Purchase.PrPurchaseBo.Instance.UpdateStatus(model.SourceSysNO, -1, (int)PurchaseStatus.采购单入库状态.已入库, (int)PurchaseStatus.采购单状态.已完成);
                    }

                    // 更新调拨单状态
                    if (model.SourceType == (int)WarehouseStatus.入库单据类型.调拨出库单)
                    {
                        var AAEntity = BLL.Warehouse.AtAllocationBo.Instance.GetAtAllocationEntity(model.SourceSysNO);
                        if (AAEntity != null)
                        {
                            if (!status)
                            {
                                AAEntity.Status = (int)Model.WorkflowStatus.WarehouseStatus.库存调拨单状态.已完成;
                            }
                            BLL.Warehouse.AtAllocationBo.Instance.UpdateAtAllocation(AAEntity);
                        }
                    }

                    //更新退换货信息(朱成果)
                    RmaBo.Instance.RMAInStockCallBack(model.SysNo, user, model.InvoiceSysNo.HasValue);
                }
                //已取回发票，更新发票数据
                if (model.InvoiceSysNo.HasValue)
                {
                    var invoice = InvoiceBo.Instance.GetModel(model.InvoiceSysNo.Value);
                    if (invoice != null && invoice.Status == FinanceStatus.发票状态.已开票.GetHashCode())
                    {
                        invoice.Status = FinanceStatus.发票状态.已取回.GetHashCode();
                        SoOrderBo.Instance.UpdateOrderInvoice(invoice);
                    }
                    else
                    {
                        throw new HytException("发票未开或发票信息不可用，请尝试检查订单跟发票信息！");
                    }
                }


            }
        }

        /// <summary>
        /// 自动入库
        /// </summary>
        /// <param name="sysNo">入库系统编号</param>
        /// <param name="syUserSysNo">用户系统编号</param>
        /// <returns>返回结果</returns>
        /// <remarks>2013-08-22 周唐炬 创建</remarks>
        public void AutoInStock(int sysNo, int syUserSysNo)
        {
            var model = this.GetStockIn(sysNo);
            if (null == model || model.Status == WarehouseStatus.入库单状态.作废.GetHashCode())
            {
                throw new HytException("入库不存在或已经作废，不能操作，请检查！");
            }
            else
            {
                var itemList = this.GetStockInItemList(model.SysNo);
                model.ItemList = itemList;
                foreach (var item in itemList)
                {
                    item.RealStockInQuantity = item.StockInQuantity;
                    item.LastUpdateBy = syUserSysNo;
                    item.LastUpdateDate = DateTime.Now;
                    UpdateStockInItem(item);
                }
                model.Status = (int)WarehouseStatus.入库单状态.已入库;
                model.LastUpdateBy = syUserSysNo;
                model.LastUpdateDate = DateTime.Now;
                this.UpdateStockIn(model);
                EasInStock(model);
            }
        }

        /// <summary>
        /// 商品入库
        /// </summary>
        /// <param name="model">入库单实体</param>
        /// <param name="user">操作人</param>
        /// <returns>返回操作状态(Result.StatusCode大于等于0成功,小于0失败)</returns>
        /// <remarks>2013-06-14 周唐炬 创建</remarks>
        private void ItemInStock(WhStockIn model, SyUser user)
        {
            if (model == null || model.ItemList == null || !model.ItemList.Any()) return;
            model.ItemList.ForEach(x =>
                {
                    #region 注释
                    //var data = IInStockDao.Instance.GetWhStockInItem(x.SysNo);
                    //x.SourceItemSysNo = data.SourceItemSysNo;
                    //if (data == null)
                    //{
                    //    throw new HytException(string.Format("该入库{0}无效！", x.ProductName));
                    //}
                    //var count = data.RealStockInQuantity + x.RealStockInQuantity;
                    //if (count > data.StockInQuantity)
                    //{
                    //    throw new HytException("实际商品入库数量总和超出商品入库数量，请检查！");
                    //}
                    //data.RealStockInQuantity = count;
                    //data.LastUpdateBy = user.SysNo;
                    //data.LastUpdateDate = DateTime.Now;
                    //UpdateStockInItem(data);
                    #endregion

                    var data = IInStockDao.Instance.GetWhStockInItem(x.SysNo);
                    x.SourceItemSysNo = data.SourceItemSysNo;
                    if (data == null)
                    {
                        throw new HytException(string.Format("该入库{0}无效！", x.ProductName));
                    }
                    var count = 0;
                    if (model.SourceType == 50)
                    {
                        count = data.RealStockInQuantity;
                    }
                    else
                    {
                        count = data.RealStockInQuantity + x.RealStockInQuantity;
                    }
                    if (count > data.StockInQuantity)
                    {
                        throw new HytException("实际商品入库数量总和超出商品入库数量，请检查！");
                    }
                    data.RealStockInQuantity = count;
                    data.LastUpdateBy = user.SysNo;
                    data.LastUpdateDate = DateTime.Now;
                    UpdateStockInItem(data);
                    UpdateStockInStatus(model.SysNo, WarehouseStatus.入库单状态.已入库, user);  //修改入库单状态

                });

            EasInStock(model);
        }

        #region EAS业务
        /// <summary>
        /// EAS业务入库(包含销售退货，换货，采购)
        /// </summary>
        /// <param name="model">入库单明细WhStockInItem本次入库的商品系统编号和商品数量</param>
        /// <remarks>2013-11-21 杨浩 创建</remarks>
        private static void EasInStock(WhStockIn model)
        {
            if (model == null) return;
            if (model.SourceType == WarehouseStatus.入库单据类型.RMA单.GetHashCode()
                     || model.SourceType == WarehouseStatus.入库单据类型.出库单.GetHashCode())
            {
                SaleInfoTranslations(model);
            }
            else if (model.SourceType == WarehouseStatus.入库单据类型.借货单.GetHashCode())
            {
                BorrowInfoTranslations(model);
            }
            else if (model.SourceType == WarehouseStatus.入库单据类型.采购单.GetHashCode())
            {
                PurchaseInfoTranslations(model);
            }
            else if (model.SourceType == WarehouseStatus.入库单据类型.调拨出库单.GetHashCode())
            {
                AtAllocationTranslations(model);
            }
        }

        /// <summary>
        /// 调拨单入库
        /// </summary>
        /// <param name="model"></param>
        /// <remarks>2016-07-01 陈海裕 创建</remarks>
        private static void AtAllocationTranslations(WhStockIn model)
        {

            IList<WhWarehouse> wareList = WhWarehouseBo.Instance.GetAllWarehouseList();
            var outInfo = BLL.Warehouse.WhInventoryOutBo.Instance.GetWhInventoryOut(model.SourceSysNO);//调拨出库单
            var allocationInfo = BLL.Warehouse.AtAllocationBo.Instance.GetAtAllocationEntity(outInfo.SourceSysNO);//调拨单  
            var tempInWarehouse = wareList.First(p => p.SysNo == model.WarehouseSysNo);
            //AtAllocation tempOutStock = AtAllocationBo.Instance.GetAtAllocationEntity(model.SourceSysNO);
            var tempOutWarehouse = wareList.First(p => p.SysNo == allocationInfo.OutWarehouseSysNo);

            var query = new List<TransferStockInfo>();

            if (model.ItemList != null)
            {
                foreach (var x in model.ItemList)
                {
                    var stock = new PdProductStock();
                    stock.StockQuantity = x.RealStockInQuantity;
                    stock.PdProductSysNo = x.ProductSysNo;
                    stock.WarehouseSysNo = model.WarehouseSysNo;
                    //保存仓库产品的库存数
                    BLL.Warehouse.PdProductStockBo.Instance.SaveProductStock(stock, BLL.Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo);

                    var erpCode = BLL.Product.PdProductBo.Instance.GetProductErpCode(x.ProductSysNo);
                    query.Add(new TransferStockInfo()
                    {
                        WarehouseSysNo = model.WarehouseSysNo,
                        WarehouseNumber = tempInWarehouse.ErpCode,
                        ErpCode = erpCode,
                        FSCStockID = tempOutWarehouse.ErpCode,
                        Quantity = x.RealStockInQuantity,
                        FNote = model.Remarks,
                        FFManagerID = model.CreatedBy.ToString(),
                        FDate = model.CreatedDate.ToString("yyyy-MM-dd"),
                    });
                }
                //金蝶接口                          
                string description = model.SourceSysNO + "_" + model.SysNo;
                string flowIdentify = allocationInfo.SysNo.ToString();
                var client = Extra.Erp.Kis.KisProviderFactory.CreateProvider();
                client.TransferStock(query, description, flowIdentify);
            }
        }

        /// <summary>
        /// 采购单入库事务（采购入库）
        /// </summary>
        /// <param name="model">入库实体</param>
        /// <remarks>2016-6-21 杨浩 创建</remarks>
        private static void PurchaseInfoTranslations(WhStockIn model)
        {
            if (model.SourceType != WarehouseStatus.入库单据类型.采购单.GetHashCode())
                return;

            var purchaseList = new List<PurchaseInfo>();
            var warhouseInfo = BLL.Warehouse.WhWarehouseBo.Instance.GetWarehouse(model.WarehouseSysNo);

            if (model.ItemList != null)
            {
                int enterQuantity = 0;
                var _purchaseList = BLL.Purchase.PrPurchaseBo.Instance.GetPurchaseDetailsByPurchaseSysNo(model.SourceSysNO);

                foreach (var x in model.ItemList)
                {
                    //var stock = new PdProductStock();
                    //stock.StockQuantity = x.RealStockInQuantity;
                    //stock.WarehouseSysNo = model.WarehouseSysNo;
                    //stock.PdProductSysNo = x.ProductSysNo;

                    //enterQuantity += x.RealStockInQuantity;
                    ////保存仓库产品的库存数
                    //BLL.Warehouse.PdProductStockBo.Instance.SaveProductStock(stock, 0);
                    //更新采购单详情入库数
                    BLL.Purchase.PrPurchaseDetailsBo.Instance.UpdateEnterQuantity(model.SourceSysNO, x.ProductSysNo, x.RealStockInQuantity);

                    var purchaseItemInfo = _purchaseList.Where(j => j.ProductSysNo == x.ProductSysNo).FirstOrDefault();
                    var purchaseInfo = new PurchaseInfo()
                    {
                        PurchaseQty = purchaseItemInfo.Quantity,
                        WarehouseSysNo = model.WarehouseSysNo,
                        FDate = model.CreatedDate.ToString(),
                        WarehouseNumber = warhouseInfo.ErpCode,
                        Quantity = x.RealStockInQuantity,
                        FNote = "平台入库单明细系统编号：" + x.SysNo,
                        Price = purchaseItemInfo.Money,
                        Amount = purchaseItemInfo.Money * x.RealStockInQuantity,
                        ErpCode = purchaseItemInfo.ErpCode,
                        SettleDate = model.CreatedDate.ToString(),
                    };
                    purchaseList.Add(purchaseInfo);
                }


                //更新采购单已入库数
                BLL.Purchase.PrPurchaseBo.Instance.UpdateEnterQuantity(model.SourceSysNO, enterQuantity);

                if (purchaseList.Count > 0)
                {
                    var prPurchaseInfo = IPrPurchaseDao.Instance.GetPrPurchaseInfo(model.SourceSysNO);
                    var _purchaseInfo = purchaseList[0];
                    _purchaseInfo.FSupplyID = prPurchaseInfo.ManufacturerSysNo.ToString();
                    _purchaseInfo.FMangerID = prPurchaseInfo.CreatedBy.ToString();
                    _purchaseInfo.FEmpID = prPurchaseInfo.SyUserSysNo.ToString();
                    _purchaseInfo.FDeptID = prPurchaseInfo.DepartmentSysNo.ToString();

                    string flowIdentify = model.SourceSysNO.ToString();//采购订单系统编号
                    int count = EasBo.Instance.GetEasSyncLogCount((int)Extra.Erp.Model.接口类型.采购入库退货, flowIdentify);
                    _purchaseInfo.SynchronizeCount = count;
                    purchaseList[0] = _purchaseInfo;
                    string description = model.SysNo.ToString();//入库单系统编号               
                    var client = Extra.Erp.Kis.KisProviderFactory.CreateProvider();
                    client.PurchaseInStock(purchaseList, description, flowIdentify);
                }

            }

        }
        /// <summary>
        /// 出库单作废入库,退货入库,EAS事务(销售出库单)
        /// </summary>
        /// <param name="model">入库实体</param>
        /// <remarks>2013-11-21 周唐炬 创建</remarks>
        private static void SaleInfoTranslations(WhStockIn model)
        {
            List<SaleInfo> saleInfoList = null;
            var customer = string.Empty;
            var description = string.Empty;//订单系统编号
            if (model.ItemList != null && model.ItemList.Any())
            {
                saleInfoList = new List<SaleInfo>();

                //仓库
                var warehouse = WhWarehouseBo.Instance.GetWarehouse(model.WarehouseSysNo);
                var warehouseErpCode = string.Empty;
                string organizationCode = string.Empty;//组织机构代码
                if (warehouse != null)
                {
                    warehouseErpCode = warehouse.ErpCode;
                    var oraganization = OrganizationBo.Instance.GetOrganization(warehouse.SysNo);
                    if (oraganization != null)
                    {
                        organizationCode = oraganization.Code;
                    }
                }
                var orderSysNo = 0;
                //是否为RMA换货
                bool isRma = false;
                //商城或第三方订单号
                var orderNo = string.Empty;

                SoOrder order=null;
                string deliveryCustomer = ""; //金蝶客户代码
                if (model.SourceType == WarehouseStatus.入库单据类型.RMA单.GetHashCode())
                {
                    //创建Eas出库单摘要，商城订单时使用商城订单号，分销商时使用分销商订单号
                    var rmaSysNo = model.SourceSysNO;

                    var rmaInfo = BLL.RMA.RmaBo.Instance.GetRMA(rmaSysNo);
                    if (rmaInfo == null)
                    {
                        throw new Exception(string.Format("找不到编号为:{0}的RMA单", rmaSysNo));
                    }

                    order = SoOrderBo.Instance.GetEntity(rmaInfo.OrderSysNo);

                    if (order == null)
                    {
                        throw new Exception(string.Format("找不到编号为:{0}的订单", rmaInfo.OrderSysNo));
                    }
                    var dealerInfo = BLL.Distribution.DsDealerBo.Instance.GetDsDealer(order.DealerSysNo);
                    organizationCode = dealerInfo.CreatedBy.ToString();
                    deliveryCustomer = dealerInfo.ErpCode;
                    
                }
              


                //用户账号
                string account = string.Empty;

                foreach (var x in model.ItemList)
                {                  
                    //var product = PdProductBo.Instance.GetProductErpCode(x.ProductSysNo);
                    //if (product == null) return;

                   var productErpCode = IPdProductDao.Instance.GetProductErpCode(x.ProductSysNo);
                   if (string.IsNullOrWhiteSpace(productErpCode)) return;

                    var price = decimal.Zero;

                    //取得退货单
                    if (model.SourceType == WarehouseStatus.入库单据类型.RMA单.GetHashCode())
                    {
                        var rma = RmaBo.Instance.GetRMA(model.SourceSysNO);

                        if (rma != null && rma.RMAItems != null && rma.RMAItems.Any())
                        {
                            isRma = (rma.RmaType == (int)Model.WorkflowStatus.RmaStatus.RMA类型.售后换货);
                            orderSysNo = rma.OrderSysNo;
                            //取得退货单对应商品
                            var item = rma.RMAItems.FirstOrDefault(c => c.SysNo == x.SourceItemSysNo);
                            if (item != null && item.RmaQuantity > 0)
                            {
                                if (item.RefundProductAmount < 0)//退货分摊为负，不导入
                                    continue;
                                if (rma.DeductedInvoiceAmount > 0)//如果存在扣发票金额，将其分摊到明细
                                {
                                    item.RefundProductAmount = decimal.Round(item.RefundProductAmount - (item.RefundProductAmount / rma.RefundProductAmount) * rma.DeductedInvoiceAmount, 2);
                                }

                                if (rma.RefundCoin > 0)//如果存在实退会员币，将其分摊到明细
                                {
                                    item.RefundProductAmount =decimal.Round(item.RefundProductAmount - (item.RefundProductAmount / rma.RefundProductAmount) * rma.RefundCoin, 2);
                                }

                                price = (decimal)x.RealStockInQuantity / (decimal)item.RmaQuantity * item.RefundProductAmount;//退货价为实退金额除退货数量
                            }
                            if (item != null && isRma) //如果为售后换货取原单实际销售金额
                            {
                                var whStockOut = BLL.Warehouse.WhWarehouseBo.Instance.GetWhStockOutItem(item.StockOutItemSysNo);
                                var itx = Hyt.BLL.RMA.RmaBo.Instance.GetSoReturnOrderItem(whStockOut.OrderItemSysNo);
                                if (itx != null)//换货生成的新订单再次换货
                                    price = decimal.Round(itx.FromStockOutItemAmount * ((decimal)x.RealStockInQuantity / whStockOut.ProductQuantity), 2);
                                else//原订单换货
                                    price = decimal.Round(whStockOut.RealSalesAmount * ((decimal)x.RealStockInQuantity / whStockOut.ProductQuantity), 2);
                            }
                        }
                    }
                    else if (model.SourceType == WarehouseStatus.入库单据类型.出库单.GetHashCode())
                    {
                        //出库单中商品
                        var itemList = WhWarehouseBo.Instance.GetWhStockOutItemList(model.SourceSysNO);
                        if (itemList != null && itemList.Any())
                        {
                            orderSysNo = itemList.First().OrderSysNo;
                            //customer = WhWarehouseBo.Instance.GetErpCustomerCode(itemList.First().OrderSysNo);
                            var item = itemList.FirstOrDefault(c => c.SysNo == x.SourceItemSysNo);
                            if (item != null && item.RealSalesAmount > 0 && item.ProductQuantity > 0)
                            {
                                price = decimal.Round(x.RealStockInQuantity / (decimal)item.ProductQuantity * item.RealSalesAmount, 2);
                            }
                        }
                    }

                    saleInfoList.Add(new SaleInfo()
                    {
                        ErpCode = productErpCode,
                        Quantity = x.RealStockInQuantity,
                        WarehouseNumber = warehouseErpCode,
                        WarehouseSysNo = model.WarehouseSysNo,
                        OrganizationCode = organizationCode,
                        Amount = price,
                        DiscountAmount = decimal.Zero,
                        IsPresent = isRma ? 0 : (price == 0 ? 1 : 0), //价格为0的商品，传入Eas为赠品
                        ItemID = model.SysNo,
                        DeliveryCustomer = deliveryCustomer,
                    });

                };

                //如果入库单为升舱订单就获取导入分销订单号
                //var order = SoOrderBo.Instance.GetEntity(orderSysNo);
                orderNo = orderSysNo.ToString();
                if (order != null)
                {
                    if (order.OrderSource == OrderStatus.销售单来源.分销商升舱.GetHashCode())
                    {
                        var dsOrders = Hyt.BLL.MallSeller.DsOrderBo.Instance.GetEntityByHytOrderID(orderSysNo);
                        if (dsOrders != null && dsOrders.Count > 0)
                            orderNo = string.Join(";", dsOrders.Select(o => o.MallOrderId));
                    }
                }
                customer = WhWarehouseBo.Instance.GetErpCustomerCode(orderSysNo);
                description = orderNo;
                var client =Extra.Erp.Kis.KisProviderFactory.CreateProvider();
                client.SaleInStock(saleInfoList, customer, description, order == null ? string.Empty : order.TransactionSysNo);
            }
        }
        /// <summary>
        /// 借货还货EAS事务(其他出库单)
        /// </summary>
        /// <param name="model">入库实体</param>
        /// <remarks>2013-11-21 周唐炬 创建</remarks>
        private static void BorrowInfoTranslations(WhStockIn model)
        {
            List<BorrowInfo> borrowInfoList = null;
            if (model.ItemList != null && model.ItemList.Any())
            {



                borrowInfoList = new List<BorrowInfo>();
                //仓库
                var warehouse = WhWarehouseBo.Instance.GetWarehouse(model.WarehouseSysNo);
                var warehouseErpCode = string.Empty;
                if (warehouse != null)
                {
                    warehouseErpCode = warehouse.ErpCode;
                }
                model.ItemList.ForEach(x =>
                {
                    #region 入库数写入仓库库存
                    var stock = new PdProductStock();
                    stock.StockQuantity = x.RealStockInQuantity;
                    BLL.Warehouse.PdProductStockBo.Instance.SaveProductStock(stock, BLL.Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo);

                    ///添加调拨入库记录
                    WhWarehouseChangeLog log = new WhWarehouseChangeLog()
                    {
                        WareSysNo = stock.WarehouseSysNo,
                        ProSysNo = x.ProductSysNo,
                        ChageDate = model.CreatedDate,
                        CreateDate = DateTime.Now,
                        ChangeQuantity = Convert.ToInt32(x.RealStockInQuantity),
                        BusinessTypes = "借货入库",
                        LogData = "入库单号：" + model.TransactionSysNo
                    };
                    WhWarehouseChangeLogBo.Instance.CreateMod(log);
                    #endregion

                    var product = PdProductBo.Instance.GetProduct(x.ProductSysNo);
                    if (product == null) return;
                    var price = decimal.Zero;

                    //借货单商品
                    var item = IProductLendDao.Instance.GetWhProductLendItemInfo(new ParaWhProductLendItemFilter()
                    {
                        ProductLendSysNo = model.SourceSysNO,
                        ProductSysNo = x.ProductSysNo,
                        PriceSource = (int)ProductStatus.产品价格来源.配送员进货价
                    });
                    if (item != null)
                    {
                        price = item.Price;
                    }

                    var borrowInfo = new BorrowInfo()
                        {
                            ErpCode = product.ErpCode,
                            Quantity = x.RealStockInQuantity,
                            WarehouseNumber = warehouseErpCode,
                            WarehouseSysNo = model.WarehouseSysNo,
                            Amount = price,
                            Remark = model.Remarks
                        };
                    borrowInfoList.Add(borrowInfo);
                });
            }

            //获取摘要
            var userName = string.Empty;
            var productLend = ProductLendBo.Instance.GetWhProductLend(model.SourceSysNO);
            if (productLend != null)
            {
                var syUser = SyUserBo.Instance.GetSyUser(productLend.DeliveryUserSysNo);
                if (syUser != null)
                {
                    userName = syUser.UserName;
                }
            }
            var description = string.Format("JC[{0}]-[{1}]-RK[{2}]", model.SourceSysNO, userName, model.SysNo);
            //借货调用借货入库,对应Eas其他出库单
            var client = EasProviderFactory.CreateProvider();
            client.Return(borrowInfoList, description, model.SourceSysNO.ToString());
        }
        #endregion

        /// <summary>
        /// 通过系统编号获取入库单
        /// </summary>
        /// <param name="sysNo">入库单系统编号</param>
        /// <returns>返回入库单明细,包含入库商品列表</returns>
        /// <remarks>2013-06-08 周唐炬 创建</remarks>
        public WhStockIn GetStockIn(int sysNo)
        {
            return IInStockDao.Instance.GetWhStockIn(sysNo);
        }

        /// <summary>
        /// 通过事务编号获取入库单明细
        /// </summary>
        /// <param name="transactionSysNo">事务编号</param>
        /// <returns>返回入库单明细,包含入库商品列表</returns>
        /// <remarks>2013-06-08 周唐炬 创建</remarks>
        public WhStockIn GetStockInDetailsByTransactionSysNo(string transactionSysNo)
        {
            return IInStockDao.Instance.GetWhStockInByTransactionSysNo(transactionSysNo);
        }

        /// <summary>
        /// 商品入库
        /// </summary>
        /// <param name="model">入库单明细</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013-06-08 周唐炬 创建</remarks>
        public int InsertStockInItem(WhStockInItem model)
        {
            return IInStockDao.Instance.InsertWhStockInItem(model);
        }

        /// <summary>
        /// 更新商品入库信息
        /// </summary>
        /// <param name="model">入库单明细</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013-06-08 周唐炬 创建</remarks>
        public int UpdateStockInItem(WhStockInItem model)
        {
            return IInStockDao.Instance.UpdateWhStockInItem(model);
        }

        /// <summary>
        /// 删除商品入库信息
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>成功返回true,失败返回false</returns>
        /// <remarks>2013-06-09 周唐炬 创建</remarks>
        public bool DeleteStockInItem(int sysNo)
        {
            return IInStockDao.Instance.DelWhStockInItem(sysNo);
        }
        /// <summary>
        /// 通过入库单ID获取所有商品列表
        /// </summary>
        /// <param name="stockInSysNo">入库单系统SysNO</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页大小.</param>
        /// <returns>返回入库单商品列表</returns>
        /// <remarks>2013-06-09 周唐炬 创建</remarks>
        public PagedList<WhStockInItem> GetStockInItemList(int stockInSysNo, int pageIndex, int pageSize)
        {
            var model = new PagedList<WhStockInItem>();
            var result = IInStockDao.Instance.GetWhStockInItemListByStockInSysNo(stockInSysNo, pageIndex, pageSize);
            model.Style = PagedList.StyleEnum.Mini;
            model.TData = result;
            model.TotalItemCount = IInStockDao.Instance.GetWhStockInItemListByStockInSysNoCount(stockInSysNo);
            return model;
        }

        /// <summary>
        /// 通过入库单ID获取所有商品列表
        /// </summary>
        /// <param name="stockInSysNo">入库单系统stockInSysNo</param>
        /// <returns>返回入库单商品列表</returns>
        /// <remarks>2013-06-24 郑荣华 创建</remarks>
        public List<WhStockInItem> GetStockInItemList(int stockInSysNo)
        {
            return IInStockDao.Instance.GetWhStockInItemList(stockInSysNo);
        }

        /// <summary>
        /// 更新入库单状态
        /// </summary>
        /// <param name="stockInSysNo">入库单系统编号</param>
        /// <param name="status">入库单状态</param>
        /// <param name="user">操作人</param>
        /// <returns>返回结果</returns>
        /// <remarks>2013-06-27 周唐炬 创建</remarks>
        public void UpdateStockInStatus(int stockInSysNo, WarehouseStatus.入库单状态 status, SyUser user)
        {
            var model = InStockBo.Instance.GetStockIn(stockInSysNo);
            if (null == model || model.Status == WarehouseStatus.入库单状态.作废.GetHashCode())
            {
                throw new HytException("入库不存在或已经作废，不能操作，请检查！");
            }
            else
            {
                model.Status = status.GetHashCode();
                model.LastUpdateBy = user.SysNo;
                model.LastUpdateDate = DateTime.Now;
                IInStockDao.Instance.UpdateWhStockIn(model);
                if (model.Status == (int)WarehouseStatus.入库单状态.已入库)
                {
                    //更新取件单
                    var pickup = ILgPickUpDao.Instance.GetEntityByStockIn(stockInSysNo);
                    if (pickup != null)
                    {
                        pickup.Status = (int)LogisticsStatus.取件单状态.已入库;
                        ILgPickUpDao.Instance.Update(pickup);
                    }
                    if (model.SourceType == (int)WarehouseStatus.入库单据类型.RMA单)
                    {
                        var rma = RmaBo.Instance.GetRMA(model.SourceSysNO);
                        if (rma != null && rma.RmaType == RmaStatus.RMA类型.售后换货.GetHashCode())
                        {
                            var crCustomer = CrCustomerBo.Instance.GetCrCustomerItem(rma.CustomerSysNo);
                            if (crCustomer != null)
                            {
                                Task.Factory.StartNew(
                                    () =>
                                    {
                                        Extras.SmsBO.Instance.发送换货完成短信(crCustomer.MobilePhoneNumber,
                                                                       rma.SysNo.ToString(
                                                                           CultureInfo.InvariantCulture));
                                    });
                                Task.Factory.StartNew(() =>
                                    {
                                        Extras.EmailBo.Instance.发送换货完成邮件(crCustomer.EmailAddress,
                                                                         crCustomer.SysNo.ToString(
                                                                             CultureInfo.InvariantCulture),
                                                                         rma.SysNo.ToString(
                                                                             CultureInfo.InvariantCulture),
                                                                         rma.OrderSysNo.ToString(
                                                                             CultureInfo.InvariantCulture),
                                                                         rma.CreateDate.ToString(
                                                                             CultureInfo.InvariantCulture));
                                    });
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 根据来源单据和类型获取入库单
        /// </summary>
        /// <param name="sourceType">来源类型</param>
        /// <param name="sourceNo">来源单据系统编号</param>
        /// <returns>入库单</returns>
        /// <remarks>2013-9-3 黄伟 创建</remarks>
        public WhStockIn GetStockInBySource(int sourceType, int sourceNo)
        {
            return IInStockDao.Instance.GetStockInBySource(sourceType, sourceNo);
        }


        public List<WhStockIn> GetStockInListByDate(DateTime dateTime)
        {
            return IInStockDao.Instance.GetStockInListByDate(dateTime);
        }


        /// <summary>
        /// 完成调拨
        /// </summary>
        /// <param name="model">出库单</param>
        /// <remarks>2018-01-17 杨浩 创建</remarks>
        public Hyt.Model.Result CompleteTransfer(WhInventoryOut model)
        {
            var result = new Hyt.Model.Result() { Status = false };
            var user = BLL.Authentication.AdminAuthenticationBo.Instance.Current.Base;

            #region 出库
            // 操作用户是否有相应的仓库权限
            if (!AdminAuthenticationBo.Instance.HasWareHousePrivilege(model.WarehouseSysNo))
            {
                result.Message = "用户没有编号为" + model.WarehouseSysNo + "仓库的权限";
                return result;
            }
            //出库
            var tempResult = BLL.Warehouse.AtAllocationBo.Instance.DoAAOutConfirm(model);
            if (!tempResult.Status)
                return tempResult;

            //生成入库单
            int whStockInSysNo = BLL.Logistics.LgDeliveryBo.Instance.CreateInStockByInventoryOut(model, user.SysNo, "仓库调拨出库生成入库单");
            if (whStockInSysNo <= 0)
            {
                result.Message = "生成入库单失败！";
                return result;
            }

            var temInvenOut = BLL.Warehouse.WhInventoryOutBo.Instance.GetWhInventoryOut(model.SysNo);
            //更新库存调拨单状态
            var atAllocationInfo = BLL.Warehouse.AtAllocationBo.Instance.GetAtAllocationEntity(temInvenOut.SourceSysNO);
            if (atAllocationInfo == null)
            {
                result.Message = "操作失败，调拨单不存在!";
                return result;
            }
            #endregion

            #region 入库
            var modelIn = GetStockIn(whStockInSysNo);
            if (modelIn == null || model.Status == WarehouseStatus.入库单状态.作废.GetHashCode())
                throw new HytException("入库不存在或已经作废，操作无效请检查！");

            modelIn.ItemList = GetStockInItemList(whStockInSysNo);
            if (modelIn.ItemList == null)
            {
                result.Message = "入库单明细不能为空！";
                return result;
            }

            modelIn.ItemList = modelIn.ItemList.Select(x => new WhStockInItem()
            {
                SysNo = x.SysNo,
                StockInSysNo = x.StockInSysNo,
                RealStockInQuantity = x.StockInQuantity,
                ProductSysNo = x.ProductSysNo,
                ProductName = x.ProductName,
                StockInQuantity = x.StockInQuantity,
                SourceItemSysNo = x.SourceItemSysNo,
                Remarks = x.Remarks,
                CreatedBy = x.CreatedBy,
                CreatedDate = x.CreatedDate,
                LastUpdateDate = x.LastUpdateDate,
                ProductErpCode = x.ProductErpCode,

            }).ToList();

            //商品入库
            ItemInStock(modelIn, user);
            // 更新调拨单状态
            if (modelIn.SourceType != (int)WarehouseStatus.入库单据类型.调拨出库单)
            {
                result.Message = "来源单据类型必须是调拨出库单!";
                return result;
            }

            atAllocationInfo.Status = (int)Model.WorkflowStatus.WarehouseStatus.库存调拨单状态.已完成;
            BLL.Warehouse.AtAllocationBo.Instance.UpdateAtAllocation(atAllocationInfo);
            #endregion
            result.Status = true;
            return result;
        }
    }
}
