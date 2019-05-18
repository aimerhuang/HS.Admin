using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using Extra.Erp;
using Hyt.BLL.Authentication;
using Hyt.BLL.Basic;
using Hyt.BLL.Finance;
using Hyt.BLL.Log;
using Hyt.BLL.MallSeller;
using Hyt.DataAccess.CRM;
using Hyt.DataAccess.Order;
using Hyt.DataAccess.Warehouse;
using Hyt.Infrastructure.Memory;
using Hyt.Infrastructure.Pager;
using Hyt.Model;
using Hyt.Model.B2CApp;
using Hyt.Model.Parameter;
using Hyt.Model.SystemPredefined;
using Hyt.Util;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;
using Extra.Erp.Model.Sale;
using Hyt.BLL.Sys;
using PdProductBo = Hyt.BLL.Product.PdProductBo;
using SoOrderBo = Hyt.BLL.Order.SoOrderBo;
using Hyt.BLL.Distribution;
using Hyt.BLL.QJT;
using Hyt.Infrastructure.Communication;
using Extra.Erp.Model;
using System.Data;
using Extra.Erp.Kis;
using Hyt.Model.Generated;
using Grand.Platform.Wms.Contract.DataContract;
using Hyt.Util.Serialization;
using System.Text.RegularExpressions;
namespace Hyt.BLL.Warehouse
{

    /// <summary>
    /// 仓库业务处理
    /// </summary>
    /// <remarks>2013-06-26 吴文强 更新</remarks>
    public partial class WhWarehouseBo : BOBase<WhWarehouseBo>, IWhWarehouseBo
    {

        /// <summary>
        /// 库存查询
        /// </summary>
        /// <param name="storageOrgNumber"></param>
        /// <param name="erpCode">商品编码</param>
        /// <param name="erpWarehouseSysNo">仓库编号</param>
        /// <param name="warehouseSysNo"></param>
        /// <returns>库存</returns>
        ///  <remarks>2017-06-28 罗勤尧 创建</remarks>
        public Hyt.Model.Result<IList<Hyt.Model.Inventory>> GetInventory(string[] erpCode, string erpWarehouseSysNo)
        {
            //return new Result<IList<Inventory>> { Data = null, Message = Model.EasConstant.EAS_MESSAGE_CLOSE, Status = false };
            var data = GetKisStock(erpCode, erpWarehouseSysNo);
            return data;
        }
        /// <summary>
        /// 获取金蝶库存
        /// </summary>
        /// <param name="storageOrgNumber">组织结构编码</param>
        /// <param name="erpCode">erp商品编码</param>
        /// <param name="erpWarehouseSysNo">EAS仓库编号</param>
        /// <param name="warehouseSysNo"></param>
        /// <returns>eas库存</returns>
        /// <remarks>2017-06-28 罗勤尧 创建</remarks>
        public Hyt.Model.Result<IList<Hyt.Model.Inventory>> GetKisStock(string[] erpCode, string erpWarehouseSysNo)
        {
            try
            {
                var request = new Grand.Platform.Api.Contract.DataContract.WebInventoryRequest()
                {
                    FNumbers = string.Join(",", erpCode),
                    FStockID = erpWarehouseSysNo,
                };
                using (var service = new Pisen.Framework.Service.Proxy.ServiceProxy<Grand.Platform.Api.Contract.IErpService>())
                {
                    var response = service.Channel.WebInventory(request);
                    //var result = RequestApi<IList<KisInventory>>("/base/InvList", request.ToJson());
                    if ((!response.IsError && response.InventoryList == null) || (!response.IsError && response.InventoryList != null && response.InventoryList.Count <= 0) || response.IsError)
                    {
                        return new Hyt.Model.Result<IList<Hyt.Model.Inventory>>
                        {
                            Data = null,
                            Status = false,
                            Message = string.Format("没有找到商品数据。仓库:{0},商品:{1}", erpWarehouseSysNo, string.Join(";", erpCode))
                        };
                    }


                    var data = (from item in response.InventoryList
                                select new Hyt.Model.Inventory
                                {
                                    #region
                                    //StorageOrgNumber = item.Element("StorageOrgNumber").Value,
                                    //StorageOrgName = item.Element("StorageOrgName").Value,
                                    //MeasureunitNumber = item.Element("MeasureunitNumber").Value
                                    #endregion
                                    MaterialNumber = item.FNumber,
                                    MaterialName = item.FName,
                                    WarehouseNumber = item.FStockID,
                                    WarehouseName = item.FStockName,
                                    Quantity = (int)item.FQty,
                                }).ToList();
                    return new Hyt.Model.Result<IList<Hyt.Model.Inventory>>
                    {
                        Status = !response.IsError,
                        StatusCode = int.Parse(response.ErrCode),
                        Message = response.ErrMsg,
                        Data = data
                    };

                }
            }
            catch (Exception e)
            {
                return new Hyt.Model.Result<IList<Hyt.Model.Inventory>>
                {
                    Data = null,
                    Status = false,
                    Message =
                        string.Format("{0}。仓库:{1},商品:{2}", e.Message, erpWarehouseSysNo, string.Join(";", erpCode) + ",kis:" + e.Message)
                };
            }
        }
        /// <summary>
        /// 减平台仓库库存（不减金蝶库存）
        /// </summary>
        /// <param name="type">类型（-1:出库减库存 1:作废出库单回滚库存）</param>
        /// <param name="warehouseSysNo">仓库系统编号</param>
        /// <param name="stockOutItem">出库单列表</param>
        /// <param name="stockInItem">入库单列表</param>
        /// <returns></returns>
        /// <remarks>2017-06-29 杨浩 创建</remarks>
        public Hyt.Model.Result ReduceStock(int type, int warehouseSysNo, IList<WhStockOutItem> stockOutItem, IList<WhStockInItem> stockInItem = null)
        {
            var result = new Hyt.Model.Result();
            var warehouseInfo = BLL.Warehouse.WhWarehouseBo.Instance.GetWarehouseEntity(warehouseSysNo);
            using (var service = new Pisen.Framework.Service.Proxy.ServiceProxy<Grand.Platform.Wms.Contract.IWarehouseService>())
            {
                List<Grand.Platform.Wms.Contract.Model.ProductStock> stocks = null;
                if (stockInItem == null)
                {
                    stocks = stockOutItem.Select(x => new Grand.Platform.Wms.Contract.Model.ProductStock()
                    {
                        ProductErpCode = x.ProductErpCode,
                        WarehouseErpCode = warehouseInfo.ErpCode,
                        StockQuantity = type * x.ProductQuantity
                    }).ToList();
                }
                else
                {
                    stocks = stockInItem.Select(x => new Grand.Platform.Wms.Contract.Model.ProductStock()
                    {
                        ProductErpCode = x.ProductErpCode,
                        WarehouseErpCode = warehouseInfo.ErpCode,
                        StockQuantity = type * x.RealStockInQuantity

                    }).ToList();

                }

                var request = new ProductStockModifyRequset()
                {
                    ProductStockList = stocks
                };

                var response = service.Channel.ModifyProductStock(request);
                result.Status = !response.IsError;
                result.Message = response.ErrMsg;
            }

            return result;
        }

        /// <summary>
        /// 获取仓库物流运费模板关联列表
        /// </summary>
        /// <param name="warehouseSysNo">仓库编号</param>
        /// <returns></returns>
        /// <remarks>2015-11-20 杨浩 创建</remarks>
        public List<WhWarehouseDeliveryType> GetWarehouseDeliveryTypeList(int warehouseSysNo)
        {
            return IWhWarehouseDao.Instance.GetWarehouseDeliveryTypeList(warehouseSysNo);
        }

        public List<WhWarehouse> GetWhWarehouseList()
        {
            return IWhWarehouseDao.Instance.GetWhWarehouseList();
        }

        /// <summary>
        /// 更新仓库配送方式关联运费模板
        /// </summary>
        /// <param name="warehouseSysNo">仓库系统编号</param>
        /// <param name="deliveryTypeSysNo">配送方式系统编号</param>
        /// <param name="freightModuleSysNo">运费模板系统编号</param>
        /// <returns></returns>
        /// <remarks>2015-11-21 杨浩 创建</remarks>
        public bool UpdateWarehouseDeliveryTypeAssoFreightModule(int warehouseSysNo, int deliveryTypeSysNo, int freightModuleSysNo)
        {
            return IWhWarehouseDao.Instance.UpdateWarehouseDeliveryTypeAssoFreightModule(warehouseSysNo, deliveryTypeSysNo, freightModuleSysNo) > 0;
        }

        /// <summary>
        /// 更新仓库利嘉返回对应仓库编号
        /// </summary>
        /// <param name="warehouseSysNo">仓库系统编号</param>
        /// <param name="LiJiaStoreCode">利嘉返回对应仓库编号</param>
        /// <returns></returns>
        /// <remarks>2017-05-25 罗勤尧 创建</remarks>
        public bool UpdateLiJiaStoreCode(int warehouseSysNo, string LiJiaStoreCode)
        {
            return IWhWarehouseDao.Instance.UpdateLiJiaStoreCode(warehouseSysNo, LiJiaStoreCode) > 0;
        }
        /// <summary>
        /// 获取仓库物流运费模板关联
        /// </summary>
        /// <param name="warehouseSysNo">仓库编号</param>
        /// <param name="deliveryTypeSysNo">物流编号</param>
        /// <returns></returns>
        /// <remarks>2015-11-20 杨浩 创建</remarks>
        public WhWarehouseDeliveryType GetWarehouseDeliveryType(int warehouseSysNo, int deliveryTypeSysNo)
        {
            return IWhWarehouseDao.Instance.GetWarehouseDeliveryType(warehouseSysNo, deliveryTypeSysNo);
        }

        #region Erp商品库存接口

        /// <summary>
        /// 获取分销商，分销订单号
        /// </summary>
        /// <param name="order">订单</param>
        /// <returns>分销订单号</returns>
        private string GetDsOrderSysNo(SoOrder order)
        {
            var orderSysNo = order.SysNo.ToString();
            if (order.OrderSource == OrderStatus.销售单来源.分销商升舱.GetHashCode())
            {
                var dsOrders = DsOrderBo.Instance.GetEntityByHytOrderID(order.SysNo);
                if (dsOrders == null || dsOrders.Count == 0)
                {
                    throw new Exception(string.Format("找不到系统编号为{0}的分销商商城", order.OrderSourceSysNo));
                }
                orderSysNo = string.Join(";", dsOrders.Select(o => o.MallOrderId));
            }
            return orderSysNo;
        }
        /// <summary>
        /// 更新仓库库存
        /// </summary>
        /// <param name="stockoutSysNo">出库单号</param>
        /// <remarks>2016-5-25 杨浩 创建</remarks>
        public void UpdateWarehouseProductStock(int stockoutSysNo)
        {
            try
            {
                var stockout = WhWarehouseBo.Instance.Get(stockoutSysNo);
                if (stockout == null)
                {
                    throw new HytException(string.Format("找不到编号为:{0}的出库单", stockoutSysNo));
                }
                var warehouse = GetWarehouse(stockout.WarehouseSysNo);
                string organizationCode = string.Empty;//组织机构代码
                //if (warehouse != null)
                //{
                //    var oraganization = OrganizationBo.Instance.GetOrganization(warehouse.SysNo);
                //    if (oraganization != null)
                //    {
                //        organizationCode = oraganization.Code;
                //    }
                //}
                //创建Eas出库单摘要，商城订单时使用商城订单号，分销商时使用分销商订单号
                var orderSysNo = stockout.OrderSysNO.ToString();
                //事务编号
                var transactionSysNo = stockout.TransactionSysNo;
                var order = SoOrderBo.Instance.GetEntity(stockout.OrderSysNO);
                if (order == null)
                {
                    throw new Exception(string.Format("找不到编号为:{0}的订单", stockout.OrderSysNO));
                }
                //获取分销商，分销订单号
                //orderSysNo = GetDsOrderSysNo(order);
                //是否为RMA换货下单
                bool isRma = order.OrderSource == (int)Model.WorkflowStatus.OrderStatus.销售单来源.RMA下单;
                IList<SoReturnOrderItem> soReturnOrderItem = new List<SoReturnOrderItem>();
                if (isRma)
                {
                    soReturnOrderItem = RMA.RmaBo.Instance.GetSoReturnOrderItem(order.TransactionSysNo);
                    var rma = Hyt.BLL.RMA.RmaBo.Instance.GetRcReturnEntity(order.OrderSourceSysNo);
                    if (rma != null)
                    {
                        orderSysNo = rma.OrderSysNo.ToString();
                        var rmaOrder = SoOrderBo.Instance.GetEntity(rma.OrderSysNo);
                        transactionSysNo = rmaOrder.TransactionSysNo;
                        orderSysNo = GetDsOrderSysNo(rmaOrder);
                    }
                }
                string deliveryTypeName = Hyt.BLL.Logistics.DeliveryTypeBo.Instance.GetDeliveryType(stockout.DeliveryTypeSysNo).DeliveryTypeName;
                string account = BLL.CRM.CrCustomerBo.Instance.GetModel(order.CustomerSysNo).Account;
                //获取分销商erp编码
                var customerErpCode = GetErpCustomerCode(stockout.OrderSysNO);
                var query = new List<SaleInfo>();

                foreach (var product in stockout.Items.Where(x => x.Status == WarehouseStatus.出库单明细状态.有效.GetHashCode()))
                {

                    BLL.Warehouse.PdProductStockBo.Instance.UpdateShopStockQuantity(stockout.WarehouseSysNo, product.ProductSysNo, product.ProductQuantity);
                    //var erpCode = PdProductBo.Instance.GetProductErpCode(product.ProductSysNo);
                    //如果为换货下单取原销售金额
                    //decimal rmaAmount = 0;
                    //if (isRma)
                    //{
                    //    var model = soReturnOrderItem.SingleOrDefault(x => x.OrderItemSysNo == product.OrderItemSysNo);
                    //    if (model != null)
                    //        rmaAmount = model.FromStockOutItemAmount * ((decimal)model.OrderItemQuantity / model.FromStockOutItemQuantity);
                    //}
                    //query.Add(
                    //    new SaleInfo
                    //    {
                    //        ErpCode = erpCode,
                    //        Quantity = product.ProductQuantity,
                    //        WarehouseNumber = warehouse.ErpCode,
                    //        WarehouseSysNo = stockout.WarehouseSysNo,
                    //        OrganizationCode = organizationCode,
                    //        Amount = isRma ? (rmaAmount) : product.RealSalesAmount, //2013-11-26 吴文强 修改为商品实际销售金额合计
                    //        DiscountAmount = 0,
                    //        IsPresent = isRma ? 0 : (product.RealSalesAmount == 0 ? 1 : 0),
                    //        //2013-11-26 吴文强 价格为0的商品，传入Eas为赠品
                    //        Remark = account + "," + deliveryTypeName + (isRma ? "(换货)" : "")
                    //    });
                }

                //var client = EasProviderFactory.CreateProvider();
                //client.SaleOutStock(query, customerErpCode, orderSysNo, transactionSysNo);
                //SysLog.Instance.Info(LogStatus.系统日志来源.后台, "配送修改Eas库存", LogStatus.系统日志目标类型.EAS, stockoutSysNo, Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo);
            }
            catch (Exception ex)
            {
                SysLog.Instance.Error(LogStatus.系统日志来源.后台, "配送修改Eas库存" + stockoutSysNo, LogStatus.系统日志目标类型.EAS, stockoutSysNo, ex);
            }
        }
        /// <summary>
        /// 根据仓库编号获取仓库所属企业的ERP编号
        /// </summary>
        /// <param name="wareHouseSysNo">仓库编号</param>
        /// <returns></returns>
        /// <remarks>2015-02-09 黄波 创建</remarks>
        /// <remarks>2015-02-10 杨浩 修改</remarks>
        public string GetWarehouseErpCodeForWarehouseSysNo(int wareHouseSysNo)
        {
            var resultValue = string.Empty;

            var warehouse = WhWarehouseBo.Instance.GetWarehouse(wareHouseSysNo);
            if (warehouse.IsSelfSupport == (int)WarehouseStatus.是否自营.是)
            {
                resultValue = Extra.Erp.Model.EasConstant.HytCustomer;
            }
            else
            {
                resultValue = Hyt.BLL.Distribution.DsDealerBo.Instance.GetDsDealer(DsDealerWharehouseBo.Instance.GetByWarehousSysNo(wareHouseSysNo).DealerSysNo).ErpCode;
            }

            return resultValue;

        }
        /// <summary>
        /// 明细金额为0的商品获取原单价
        /// </summary>
        /// <param name="orderitemsysno">订单明细编号</param>
        /// <returns></returns>
        /// <remarks>2015-11-23 杨浩 创建</remarks>
        public decimal GetOriginalPrice(int orderitemsysno)
        {
            decimal p = 0;
            var soi = SoOrderBo.Instance.GetOrderItem(orderitemsysno);
            if (soi != null)
            {
                if (soi.OriginalPrice > 0)//原单价大于0
                {
                    p = soi.OriginalPrice;
                }
                else
                {
                    var pr = Hyt.BLL.Product.PdPriceBo.Instance.GetProductPrice(soi.ProductSysNo).Where(m => m.Status == Hyt.Model.WorkflowStatus.ProductStatus.产品价格状态.有效.GetHashCode() && m.PriceSource == (int)ProductStatus.产品价格来源.会员等级价 && m.SourceSysNo == CustomerLevel.初级).FirstOrDefault();
                    if (pr != null)
                    {
                        p = pr.Price;
                    }
                }
            }
            return p;
        }
        /// <summary>
        /// 销售出库修改EAS库存
        /// </summary>
        /// <param name="stockoutSysNo">出库单系统编号.</param>
        /// <returns></returns>
        /// <remarks>
        /// 2013-06-26 吴文强 创建
        /// 2013-11-15 何方 实现
        /// </remarks>
        /// <remarks>2015-5-4 杨浩 添加加盟商补货</remarks>
        public void UpdateErpProductNumber(int stockoutSysNo, Extra.Erp.Model.ErpBillSource.单据来源 erpsouce = Extra.Erp.Model.ErpBillSource.单据来源.配送)
        {
            try
            {
                var stockout = WhWarehouseBo.Instance.Get(stockoutSysNo);
                if (stockout == null)
                {
                    throw new HytException(string.Format("找不到编号为:{0}的出库单", stockoutSysNo));
                }
                var warehouse = GetWarehouse(stockout.WarehouseSysNo);
                if (erpsouce == Extra.Erp.Model.ErpBillSource.单据来源.结算签收 && (warehouse == null || warehouse.IsSelfSupport == (int)WarehouseStatus.是否自营.是))
                {
                    return; //自营仓库不做结算同步处理,非自营的在做结算同步 2015-02-11 朱成果
                }
                var diaohuo = TransferCargoBo.Instance.GetTransferCargo(stockoutSysNo);
                if (erpsouce == Extra.Erp.Model.ErpBillSource.单据来源.调货)
                {
                    if (diaohuo == null || diaohuo.Status != Hyt.Model.WorkflowStatus.WarehouseStatus.调货状态.已确认.GetHashCode())
                    {
                        return;
                    }
                    if (warehouse == null || warehouse.IsSelfSupport == (int)WarehouseStatus.是否自营.是)
                    {
                        return;//自营调货不处理
                    }

                }
                else//调货EAS修改 2016/04/11 朱成果
                {
                    if (diaohuo != null && diaohuo.Status == Hyt.Model.WorkflowStatus.WarehouseStatus.调货状态.已确认.GetHashCode())
                    {
                        return;//已经调货了，不在同步EAS单据
                    }
                }

                string organizationCode = string.Empty;//组织机构代码
                if (warehouse != null)
                {
                    var oraganization = OrganizationBo.Instance.GetOrganization(warehouse.SysNo);
                    if (oraganization != null)
                    {
                        organizationCode = oraganization.Code;
                    }
                }

                //创建Eas出库单摘要，当日达订单时使用当日达订单号，经销商时使用经销商订单号
                var orderSysNo = stockout.OrderSysNO.ToString();
                //事务编号
                var transactionSysNo = stockout.TransactionSysNo;
                var order = SoOrderBo.Instance.GetEntity(stockout.OrderSysNO);
                if (order == null)
                {
                    throw new Exception(string.Format("找不到编号为:{0}的订单", stockout.OrderSysNO));
                }
                //获取经销商，分销订单号
                orderSysNo = GetDsOrderSysNo(order);
                //是否为RMA换货下单
                bool isRma = order.OrderSource == (int)Model.WorkflowStatus.OrderStatus.销售单来源.RMA下单;
                bool issmallrma = false;//是否是小商品换货
                IList<SoReturnOrderItem> soReturnOrderItem = new List<SoReturnOrderItem>();
                //获取经销商erp编码
                var customerErpCode = GetErpCustomerCode(stockout.OrderSysNO);//送货客户
                if (isRma)
                {
                    soReturnOrderItem = RMA.RmaBo.Instance.GetSoReturnOrderItem(order.TransactionSysNo);
                    var rma = Hyt.BLL.RMA.RmaBo.Instance.GetRcReturnEntity(order.OrderSourceSysNo);
                    if (rma != null)
                    {
                        customerErpCode = GetErpCustomerCode(rma.OrderSysNo);//换货为原送货客户编号
                        orderSysNo = rma.OrderSysNo.ToString();
                        var rmaOrder = SoOrderBo.Instance.GetEntity(rma.OrderSysNo);
                        transactionSysNo = rmaOrder.TransactionSysNo;
                        orderSysNo = GetDsOrderSysNo(rmaOrder);
                        if (Hyt.BLL.RMA.RmaBo.Instance.CheckSmallProductRma(rma.SysNo)) //小商品换货
                        {
                            issmallrma = true;
                        }
                    }
                }

                string deliveryTypeName = Hyt.BLL.Logistics.DeliveryTypeBo.Instance.GetDeliveryType(stockout.DeliveryTypeSysNo).DeliveryTypeName;
                string account = BLL.CRM.CrCustomerBo.Instance.GetModel(order.CustomerSysNo).Account;
                var query = new List<SaleInfo>();

                string _deliveryCustomer, _receivableCustomer;
                bool ISBH = ((erpsouce == Extra.Erp.Model.ErpBillSource.单据来源.结算签收 || erpsouce == Extra.Erp.Model.ErpBillSource.单据来源.调货) && SoOrderBo.Instance.IsNeedReplenishment(stockout.OrderSysNO, stockout.WarehouseSysNo));//是否补货
                ///是否是加盟商自销
                bool ISIMSData = (erpsouce == Extra.Erp.Model.ErpBillSource.单据来源.结算签收 && warehouse != null && warehouse.IsSelfSupport == (int)WarehouseStatus.是否自营.否);//是否是加盟商自销
                foreach (var product in stockout.Items.Where(x => x.Status == WarehouseStatus.出库单明细状态.有效.GetHashCode()))
                {
                    var erpCode = PdProductBo.Instance.GetProductErpCode(product.ProductSysNo);
                    //如果为换货下单取原销售金额
                    decimal rmaAmount = 0;
                    if (isRma && !issmallrma)//小商品换货金额为0
                    {
                        var model = soReturnOrderItem.SingleOrDefault(x => x.OrderItemSysNo == product.OrderItemSysNo);
                        if (model != null)
                            rmaAmount = model.FromStockOutItemAmount * ((decimal)product.ProductQuantity / model.FromStockOutItemQuantity);
                    }

                    var r = SoOrderBo.Instance.CheckOrderItemForTieinSales(product.OrderItemSysNo);//获取相关搭配销售信息
                    _deliveryCustomer = GetWarehouseErpCodeForWarehouseSysNo(stockout.WarehouseSysNo);
                    _receivableCustomer = SoOrderBo.Instance.GetOrderEnterpriseForERPCode(stockout.OrderSysNO);
                    string orderremark = string.Empty;//订单对内备注
                    if (order.CustomerSysNo.ToString() == System.Configuration.ConfigurationManager.AppSettings["PisenMarketCustomerID"])
                    {
                        //只同步市场部下单的对内备注
                        orderremark = order.InternalRemarks;
                        if (!String.IsNullOrEmpty(orderremark))
                        {
                            orderremark += ",";
                        }
                    }
                    var queryitem = new SaleInfo
                    {
                        Imeis = QJTStockOutImeiBo.Instance.GetImeiByStockOutItemSysNo(product.SysNo).Select(m => m.Imei).ToList(),//千机网串码
                        ItemID = product.SysNo,
                        SalesUnitPrice = product.OriginalPrice,
                        ErpCode = erpCode,
                        Quantity = product.ProductQuantity,
                        WarehouseNumber = warehouse.ErpCode,
                        WarehouseSysNo = stockout.WarehouseSysNo,
                        OrganizationCode = organizationCode,
                        Amount = isRma ? (rmaAmount) : product.RealSalesAmount, //2013-11-26 吴文强 修改为商品实际销售金额合计
                        DiscountAmount = 0,
                        IsPresent = issmallrma ? 1 : (isRma ? 0 : (product.RealSalesAmount == 0 ? 1 : 0)),//2014-12-31 小商品换货默认当作Eas赠品
                        //2013-11-26 吴文强 价格为0的商品，传入Eas为赠品
                        //2014-12-22 朱成果 传入搭配销售名称
                        Remark = orderremark + account + "," + deliveryTypeName + (isRma ? (issmallrma ? "(小商品换货)" : "(换货)") : "") + (r.Status ? "(" + r.Data + ")" : ""),
                        DeliveryCustomer = _deliveryCustomer,
                        ReceivableCustomer = _receivableCustomer,
                        //如果订单支付方式未到付,则收款客户是送货方,如果是预付,收款人则是订单创建方
                        ReceiptCustomer = (PaymentTypeBo.Instance.GetEntity(order.PayTypeSysNo).PaymentType == Hyt.Model.WorkflowStatus.BasicStatus.支付方式类型.到付.GetHashCode()) ? _deliveryCustomer : _receivableCustomer
                    };

                    #region 积分同步备注逻辑

                    //2015-7-8 林涛 添加积分同步备注逻辑，
                    //如果是积分下订单，摘要记录使用积分情况 
                    if (order.OrderSource == OrderStatus.销售单来源.积分商城下单.GetHashCode())
                    {
                        //var response = new GetPointOrderAssociationResponse();
                        //using (var iclient = new ServiceProxy<IPointShopService>())
                        //{
                        //    response = iclient.Channel.GetPointOrderAssociation(new GetPointOrderAssociationRequest() { OrderID = order.SysNo });
                        //}
                        //if (response.IsError)
                        //{
                        //    throw new Exception(response.ErrMsg);
                        //}
                        ////“普通购物：5元500分”
                        //var orderItems = SoOrderBo.Instance.GetOrderItemsByOrderId(stockout.OrderSysNO);
                        //if (orderItems == null)
                        //{
                        //    throw new Exception("错误的订单号");
                        //}
                        //int quantity = orderItems.First().Quantity;
                        //string msg = ";积分商城：" + (order.ProductDiscountAmount / quantity * queryitem.Quantity).ToString() + "元 " +
                        //             Math.Abs(response.PointOrder.UsedPoint / quantity * queryitem.Quantity).ToString() + "积分";
                        //queryitem.Remark = queryitem.Remark + msg;
                    }

                    #endregion
                    queryitem.IsPresent = queryitem.Amount == 0 ? 1 : 0;//2015-02-05 任何0单价商品都应该默认勾选为赠品
                    if (queryitem.Amount == 0)//TASK #3883 销售金额为0的商品EAS同步金额和折扣
                    {
                        if (isRma)
                        {
                            queryitem.Amount = GetOriginalPrice(product.OrderItemSysNo) * product.ProductQuantity;
                            queryitem.DiscountAmount = queryitem.Amount - rmaAmount;
                            queryitem.IsPresent = 0;
                        }
                        else
                        {
                            queryitem.Amount = GetOriginalPrice(product.OrderItemSysNo) * product.ProductQuantity;
                            queryitem.DiscountAmount = queryitem.Amount - product.RealSalesAmount;
                            queryitem.IsPresent = 0;
                        }
                    }
                    query.Add(queryitem);
                }

                //var client = EasProviderFactory.CreateProvider();
                //client.SaleOutStock(query, customerErpCode, orderSysNo, transactionSysNo, erpsouce);
                //SysLog.Instance.Info(LogStatus.系统日志来源.后台, "配送修改Eas库存", LogStatus.系统日志目标类型.EAS, stockoutSysNo, Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo);
                #region 加盟商补货 2015-5-4 谭显锋 添加
                if (ISBH)
                {

                    var replenishModel = new WhReplenish
                    {
                        Identifier = transactionSysNo,
                        Remarks = stockout.OrderSysNO + " " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                        WarehouseSysNo = stockout.WarehouseSysNo,
                        EnterpriseCode = GetWarehouseErpCodeForWarehouseSysNo(stockout.WarehouseSysNo),//订货客户
                        OutStockNo = stockout.SysNo,
                        WarehouseErpCpde = warehouse.ErpCode
                    };
                    if (erpsouce == Extra.Erp.Model.ErpBillSource.单据来源.调货)
                    {
                        replenishModel.Remarks = stockout.OrderSysNO + "(调货)" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        replenishModel.EnterpriseCode = GetParentDsDealerErpCode(stockout.WarehouseSysNo);//调货的订货客户是上一级加盟商 2016/04/15 梅风仙
                    }
                    replenishModel.type = 补货类型.补货;

                    Decimal sumMoney = 0;
                    List<WhReplenishItem> lwitems = new List<WhReplenishItem>();
                    foreach (var q in query)
                    {
                        var p = new WhReplenishItem()
                        {
                            Amount = q.Amount,//新增
                            DiscountAmount = q.DiscountAmount,//新增
                            IsPresent = q.IsPresent,//新增
                            ErpCode = q.ErpCode,
                            Quantity = q.Quantity,
                            UnitPrice = Math.Round(q.Amount / q.Quantity, 2),
                            Remarks = orderSysNo,//第三方订单号
                            DeliveryCustomer = GetParentDsDealerErpCode(stockout.WarehouseSysNo),//送货客户有上级就取上级经销商
                            ReceiveCustomer = customerErpCode,//应收客户
                            PaymentCustomer = customerErpCode//收款客户
                        };
                        #region 积分同步备注逻辑

                        //2015-7-8 林涛 添加积分同步备注逻辑，
                        //如果是积分下订单，摘要记录使用积分情况 
                        if (order.OrderSource == OrderStatus.销售单来源.积分商城下单.GetHashCode())
                        {
                            //var response = new GetPointOrderAssociationResponse();
                            //using (var iclient = new ServiceProxy<IPointShopService>())
                            //{
                            //    response = iclient.Channel.GetPointOrderAssociation(new GetPointOrderAssociationRequest() { OrderID = order.SysNo });
                            //}
                            //if (response.IsError)
                            //{
                            //    throw new Exception(response.ErrMsg);
                            //}
                            ////“普通购物：5元500分”
                            //var orderItems = SoOrderBo.Instance.GetOrderItemsByOrderId(stockout.OrderSysNO);
                            //if (orderItems == null)
                            //{
                            //    throw new Exception("错误的订单号");
                            //}
                            //int quantity = orderItems.First().Quantity;
                            //string msg = ";积分商城：" + (order.ProductDiscountAmount / quantity * q.Quantity).ToString() + "元 " +
                            //             Math.Abs(response.PointOrder.UsedPoint / quantity * q.Quantity).ToString() + "积分";
                            //p.Remarks = p.Remarks + msg;
                        }

                        #endregion
                        sumMoney += (q.Amount - q.DiscountAmount);
                        lwitems.Add(p);
                    }
                    replenishModel.Amount = sumMoney;
                    //EasProviderFactory.CreateProvider().SubmitReplenish(replenishModel, lwitems);
                }
                #endregion
                else if (ISIMSData && erpsouce != Extra.Erp.Model.ErpBillSource.单据来源.调货)
                {
                    #region 加盟商自销
                    //var ss = new Extra.Erp.Model.ImportStatus()
                    //{
                    //    dataMd5 = null,
                    //    isAgain = false,
                    //    enableEas = true,
                    //    isData = false,
                    //    isSave = false,
                    //};
                    //Extra.Erp.IMSEntity.TpcStockInOutData dtinfo = new Extra.Erp.IMSEntity.TpcStockInOutData();
                    //dtinfo.Identifier = transactionSysNo;
                    //dtinfo.InterfaceType = 接口类型.IMS出入库;
                    //dtinfo.WarehouseNo = warehouse.SysNo;
                    //dtinfo.RequestData = new Extra.Erp.IMSEntity.TpcStockInOutRequest()
                    //{
                    //    EasEnterpriseCode = GetWarehouseErpCodeForWarehouseSysNo(stockout.WarehouseSysNo),
                    //    InOCurrencyutType = "人民币",
                    //    InOutType = "21",
                    //    OrderCode = orderSysNo,
                    //    OrderDate = DateTime.Now,
                    //    Remarks = orderSysNo,
                    //    Warehouse = warehouse.ErpCode,
                    //    ExchangeValue = 1,
                    //    TaxType = "20"
                    //};
                    //dtinfo.RequestData.ItemList = new List<Extra.Erp.IMSEntity.TpcStockInOutItem>();
                    //foreach (var q in query)
                    //{
                    //    dtinfo.RequestData.ItemList.Add(new Extra.Erp.IMSEntity.TpcStockInOutItem()
                    //    {
                    //        ErpCode = q.ErpCode,
                    //        IsPresent = q.IsPresent,
                    //        ItemId = q.ItemID.HasValue ? q.ItemID.ToString() : string.Empty,
                    //        UnitPrice = q.SalesUnitPrice ?? 0,
                    //        Quantity = q.Quantity,
                    //        Remarks = q.Remark,
                    //        OriginalCurrency = (q.SalesUnitPrice ?? 0) * q.Quantity - (q.Amount - q.DiscountAmount)
                    //    });
                    //}
                    //ImsHelper.SyncTpcStockInOut(dtinfo, ss);
                    #endregion
                }
            }
            catch (Exception ex)
            {
                SysLog.Instance.Error(LogStatus.系统日志来源.后台, "配送修改Eas库存" + stockoutSysNo, LogStatus.系统日志目标类型.EAS, stockoutSysNo, ex);
            }
        }
        /// <summary>
        /// 根据仓库编号获取上级经销商仓库所属企业的ERP编号
        /// </summary>
        /// <param name="wareHouseSysNo">仓库编号</param>
        /// <returns>如果有上级经销商，返回上级经销商企业的ERP编号，否则返回经销商企业的ERP编号</returns>
        /// <remarks>2015-5-4 杨浩 创建</remarks>
        public string GetParentDsDealerErpCode(int wareHouseSysNo)
        {
            var resultValue = string.Empty;
            var dealerWarehous = DsDealerWharehouseBo.Instance.GetByWarehousSysNo(wareHouseSysNo);
            if (dealerWarehous != null)
            {
                var dealerModel = DsDealerBo.Instance.GetDsDealer(dealerWarehous.DealerSysNo); ;
                //if (dealerModel != null && dealerModel.ParentId > 0)
                //{
                //    dealerModel = DsDealerBo.Instance.GetDsDealer(dealerModel.ParentId);
                //}
                if (dealerModel != null)
                {
                    resultValue = dealerModel.ErpCode;
                }
            }
            return resultValue;
        }
        /// <summary>
        /// 销售出库修改EAS库存
        /// </summary>
        /// <param name="stockoutSysNo">出库单系统编号.</param>
        /// <returns></returns>
        /// <remarks>
        /// 2013-06-26 吴文强 创建
        /// 2013-11-15 何方 实现
        /// </remarks>
        public void UpdateErpProductNumber(int stockoutSysNo)
        {
            try
            {
                var stockout = WhWarehouseBo.Instance.Get(stockoutSysNo);
                if (stockout == null)
                {
                    throw new HytException(string.Format("找不到编号为:{0}的出库单", stockoutSysNo));
                }
                var warehouse = GetWarehouse(stockout.WarehouseSysNo);
                string organizationCode = string.Empty;//组织机构代码
                //if (warehouse != null)
                //{
                //    var oraganization = OrganizationBo.Instance.GetOrganization(warehouse.SysNo);
                //    if (oraganization != null)
                //    {
                //        organizationCode = oraganization.Code;
                //    }
                //}
                //创建Eas出库单摘要，商城订单时使用商城订单号，分销商时使用分销商订单号
                var orderSysNo = stockout.OrderSysNO.ToString();
                //事务编号
                var transactionSysNo = stockout.TransactionSysNo;
                var order = SoOrderBo.Instance.GetEntity(stockout.OrderSysNO);
                if (order == null)
                {
                    throw new Exception(string.Format("找不到编号为:{0}的订单", stockout.OrderSysNO));
                }
                var dealerInfo = BLL.Distribution.DsDealerBo.Instance.GetDsDealer(order.DealerSysNo);
                organizationCode = dealerInfo.CreatedBy.ToString();

                //获取分销商，分销订单号
                orderSysNo = GetDsOrderSysNo(order);
                //是否为RMA换货下单
                bool isRma = order.OrderSource == (int)Model.WorkflowStatus.OrderStatus.销售单来源.RMA下单;
                IList<SoReturnOrderItem> soReturnOrderItem = new List<SoReturnOrderItem>();
                if (isRma)
                {
                    soReturnOrderItem = RMA.RmaBo.Instance.GetSoReturnOrderItem(order.TransactionSysNo);
                    var rma = Hyt.BLL.RMA.RmaBo.Instance.GetRcReturnEntity(order.OrderSourceSysNo);
                    if (rma != null)
                    {
                        orderSysNo = rma.OrderSysNo.ToString();
                        var rmaOrder = SoOrderBo.Instance.GetEntity(rma.OrderSysNo);
                        transactionSysNo = rmaOrder.TransactionSysNo;
                        orderSysNo = GetDsOrderSysNo(rmaOrder);
                    }
                }
                string deliveryTypeName = Hyt.BLL.Logistics.DeliveryTypeBo.Instance.GetDeliveryType(stockout.DeliveryTypeSysNo).DeliveryTypeName;
                string account = BLL.CRM.CrCustomerBo.Instance.GetModel(order.CustomerSysNo).Account;
                //获取分销商erp编码
                var customerErpCode = GetErpCustomerCode(stockout.OrderSysNO);
                var query = new List<SaleInfo>();

                foreach (var product in stockout.Items.Where(x => x.Status == WarehouseStatus.出库单明细状态.有效.GetHashCode()))
                {
                    var erpCode = PdProductBo.Instance.GetProductErpCode(product.ProductSysNo);
                    //如果为换货下单取原销售金额
                    decimal rmaAmount = 0;
                    if (isRma)
                    {
                        var model = soReturnOrderItem.SingleOrDefault(x => x.OrderItemSysNo == product.OrderItemSysNo);
                        //if (model != null)
                        //    rmaAmount = model.FromStockOutItemAmount * ((decimal)model.OrderItemQuantity / model.FromStockOutItemQuantity);

                        //2017-12-01 修改 退换货的实际销售金额=退换货出库商品数*（原出库单实际销售金额/来源出库单商品数）
                        if (model != null)
                            rmaAmount = product.ProductQuantity * (model.FromStockOutItemAmount / model.FromStockOutItemQuantity);
                    }
                    query.Add(
                        new SaleInfo
                        {
                            ErpCode = erpCode,
                            Quantity = product.ProductQuantity,
                            WarehouseNumber = warehouse.ErpCode,
                            WarehouseSysNo = stockout.WarehouseSysNo,
                            OrganizationCode = organizationCode,
                            Amount = isRma ? (rmaAmount) : product.RealSalesAmount, //2013-11-26 吴文强 修改为商品实际销售金额合计
                            DiscountAmount = 0,
                            IsPresent = isRma ? 0 : (product.RealSalesAmount == 0 ? 1 : 0),
                            //2013-11-26 吴文强 价格为0的商品，传入Eas为赠品
                            Remark = account + "," + deliveryTypeName + (isRma ? "(换货)" : ""),
                            ItemID = stockoutSysNo,
                            DeliveryCustomer = dealerInfo.ErpCode
                        });
                }

                var client = KisProviderFactory.CreateProvider();
                client.SaleOutStock(query, customerErpCode, orderSysNo, transactionSysNo);
                //SysLog.Instance.Info(LogStatus.系统日志来源.后台, "配送修改Eas库存", LogStatus.系统日志目标类型.EAS, stockoutSysNo, Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo);
            }
            catch (Exception ex)
            {
                SysLog.Instance.Error(LogStatus.系统日志来源.后台, "配送修改Eas库存" + stockoutSysNo, LogStatus.系统日志目标类型.EAS, stockoutSysNo, ex);
            }
        }

        #endregion

        #region 仓库基本信息

        /// <summary>
        /// 所有地区
        /// </summary>
        /// <param></param>
        /// <returns>返回所有地区</returns>
        /// <remarks>2013-06-24 周唐炬 创建</remarks>
        private List<BsArea> AreaAll
        {
            get { return BasicAreaBo.Instance.GetAll(); }
        }

        /// <summary>
        ///根据地区信息获取仓库信息
        /// </summary>
        /// <param name="areaSysNo">地区信息</param>
        /// <param name="warehouseType">仓库类型</param>
        /// <param name="deliveryType">配送方式</param>
        /// <returns></returns>
        /// <remarks> 
        /// 2013-06-18 朱成果 创建
        /// </remarks>
        public IList<WhWarehouse> GetWhWarehouseListByArea(int areaSysNo, int? warehouseType, int? deliveryType = null)
        {
            return IWhWarehouseDao.Instance.GetWhWarehouseListByArea(areaSysNo, warehouseType, deliveryType);
        }
        /// <summary>
        /// 根据地区信息获取仓库信息
        /// </summary>
        /// <param name="areaSysNo">地区信息</param>
        /// <param name="warehouseType">仓库类型</param>
        /// <param name="deliveryType">配送方式</param>
        /// <returns>匹配仓库数据列表</returns>
        /// <remarks> 
        /// 2013-06-18 朱成果 创建
        /// </remarks>
        public IList<WhWarehouse> GetWhWarehouseListByDeliveryType(int deliveryType)
        {
            return IWhWarehouseDao.Instance.GetWhWarehouseListByDeliveryType(deliveryType);
        }

        /// <summary>
        /// 根据地区、仓库类型、取件方式获取仓库信息
        /// </summary>
        /// <param name="areaSysNo">地区系统编号</param>
        /// <param name="warehouseType">仓库类型</param>
        /// <param name="pickupType">取件方式编号</param>
        /// <returns>返回仓库列表</returns>
        /// <remarks>2013-09-13 周唐炬 创建</remarks>
        public IList<WhWarehouse> GetWhWarehouseList(int areaSysNo, int? warehouseType, int pickupType)
        {
            return IWhWarehouseDao.Instance.GetWhWarehouseList(areaSysNo, warehouseType, pickupType);
        }

        /// <summary>
        ///根据地区信息获取仓库信息
        /// </summary>
        /// <param name="area">地区信息</param>
        /// <returns></returns>
        /// <remarks> 
        /// 2013-09-11 杨晗 创建
        /// </remarks>
        public IList<WhWarehouse> GetWhWarehouseListByArea(List<int> area)
        {
            return IWhWarehouseDao.Instance.GetWhWarehouseListByArea(area);
        }

        /// <summary>
        /// 获取所有仓库信息
        /// </summary>
        /// <returns></returns>
        /// <remarks>2013-06-18 朱成果 创建</remarks>
        /// <remarks>2013-11-05 杨浩 添加缓存</remarks>
        public IList<WhWarehouse> GetAllWarehouseList()
        {
            var list = MemoryProvider.Default.Get(KeyConstant.WarehouseList, () => IWhWarehouseDao.Instance.GetAllWarehouseList());
            return list;
        }

        /// <summary>
        /// 获取仓库详情
        /// </summary>
        /// <param name="sysNo">仓库编号</param>
        /// <remarks> 
        /// 2013-06-18 朱成果 创建
        /// 2014-5-13 杨文兵 仓库数据加入缓存处理
        /// </remarks>
        public WhWarehouse GetWarehouseEntity(int sysNo)
        {
            var cacheKey = string.Format("CACHE_WHWAREHOUSE_{0}", sysNo);
            return MemoryProvider.Default.Get<WhWarehouse>(cacheKey, 60 * 24, () =>
            {
                return IWhWarehouseDao.Instance.GetWarehouseEntity(sysNo);
            }, CachePolicy.Absolute);
        }
        /// <summary>
        /// 获取实际仓库库存
        /// </summary>
        /// <param name="sysNo"></param>
        /// <param name="whsysNo"></param>
        /// <returns></returns>
        public string GetStockQuantity(int sysNo, int whsysNo)
        {
            return IWhWarehouseDao.Instance.GetStockQuantity(sysNo, whsysNo);
        }
        /// <summary>
        /// 获取仓库详情
        /// </summary>
        /// <param name="sysNo">仓库名称</param>
        /// <returns>返回仓库详情</returns>
        /// <remarks> 2015-12-15 王耀发 创建</remarks>
        public static WhWarehouse GetWarehouseByName(string BackWarehouseName)
        {
            return IWhWarehouseDao.Instance.GetWarehouseByName(BackWarehouseName);
        }
        /// <summary>
        /// 获取仓库详情
        /// </summary>
        /// <param name="sysNo">仓库编号</param>
        /// <returns></returns>
        /// <remarks>2015-08-06 王耀发 创建</remarks>
        public static WhWarehouse GetEntity(int sysNo)
        {
            return IWhWarehouseDao.Instance.GetWarehouseEntity(sysNo);
        }
        /// <summary>
        /// 获取仓库名称
        /// </summary>
        /// <param name="sysNo">仓库编号</param>
        /// <returns>仓库名称</returns>
        /// <remarks>2013-08-13 周唐炬 创建</remarks>
        public string GetWarehouseName(int sysNo)
        {
            var wareHouse = GetWarehouseEntity(sysNo);
            return wareHouse == null ? "未知仓库" : wareHouse.BackWarehouseName;
        }
        /// <summary>
        /// 获取仓库名称和仓库是否自营
        /// </summary>
        /// <param name="sysNo">仓库编号</param>
        /// <returns>仓库名称</returns>
        /// <remarks>2015-01-04 杨浩 创建</remarks>
        public string GetWarehouseNameWithIsSelfSupport(int sysNo)
        {
            var wareHouse = GetWarehouseEntity(sysNo);
            if (wareHouse != null)
            {
                if (wareHouse.IsSelfSupport == 1)
                {
                    return wareHouse.WarehouseName + "(自营)";
                }
                else
                {
                    return wareHouse.WarehouseName + "(非自营)";
                }
            }
            return "未知仓库";
        }
        #endregion

        #region 仓库配送员

        /// <summary>
        /// 获取仓库下面的配送员
        /// </summary>
        /// <param name="warehouseSysNo">仓库系统编号</param>
        /// <returns>配送员信息</returns>
        /// <remarks> 
        /// 2013-06-21 郑荣华 创建
        /// </remarks>
        public IList<SyUser> GetWhDeliveryUser(int warehouseSysNo)
        {
            var list = IWhWarehouseDao.Instance.GetWhDeliveryUser(warehouseSysNo);
            return list;
        }

        /// <summary>
        /// 获取仓库下面是否允许借货的配送员
        /// </summary>
        /// <param name="warehouseSysNo">仓库系统编号</param>
        /// <param name="isDelivery">是否允许借货</param>
        /// <returns>配送员字典</returns>
        /// <remarks> 
        /// 2013-07-03 沈强 创建
        /// </remarks>
        public IList<SyUser> GetWhDeliveryUser(int warehouseSysNo, Model.WorkflowStatus.LogisticsStatus.配送员是否允许配送 isDelivery)
        {
            var list = IWhWarehouseDao.Instance.GetWhDeliveryUser(warehouseSysNo, isDelivery);
            return list;
        }

        /// <summary>
        /// 获取未录入信用信息的仓库下面的配送员
        /// </summary>
        /// <param name="warehouseSysNo">仓库系统编号</param>
        /// <returns>配送员字典</returns>
        /// <remarks> 
        /// 2013-06-21 郑荣华 创建
        /// </remarks>
        public Dictionary<int, string> GetWhDeliveryUserDictForCredit(int warehouseSysNo)
        {
            var dic = new Dictionary<int, string>();
            var list = IWhWarehouseDao.Instance.GetWhDeliveryUserForCredit(warehouseSysNo);
            list.ForEach(p => dic.Add(p.SysNo, p.UserName));
            return dic;
        }
        #endregion

        #region 搜索仓库 何方

        /// <summary>
        /// 搜索仓库
        /// </summary>
        /// <param name="keyword">关键词.</param>
        /// <param name="areaNoCheck">地区节点是否可被选中</param>
        /// <param name="isRma">是否Rma仓</param>
        /// <param name="isSelfSupport">是否自营</param>
        /// <returns>ZTree列表</returns>
        /// <remarks>
        /// 2013-6-21 何方 创建
        /// 2013-06-24 周唐炬 修改查询业务,节点递归
        /// 2013-10-22 郑荣华 增加只能查询有权限的仓库
        /// 2013-10-24 黄志勇 筛选Rma仓库
        /// </remarks>
        public IList<ZTreeNode> SearchWharehouseNew(string keyword, bool areaNoCheck, bool isRma = false, int? isSelfSupport = null)
        {
            List<ZTreeNode> nodes = null;
            var list = new List<BsArea>();
            //检查查询条件
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                keyword = keyword.ToLower(); //全部小写
                //子节点
                var areas = BasicAreaBo.Instance.QueryRecursive(keyword);
                if (areas != null && areas.Any())
                {
                    list.AddRange(areas);
                    //递归父级
                    var parentAreas = BasicAreaBo.Instance.QueryRecursive(keyword, true);
                    list.AddRange(parentAreas);
                    //去重
                    list = list.Distinct(new Compare()).ToList();
                }
            }
            else
            {
                list = AreaAll;
            }
            if (list.Any())
            {
                nodes = (from BsArea b in list
                         select
                             new ZTreeNode
                             {
                                 id = b.SysNo,
                                 pId = b.ParentSysNo,
                                 name = b.AreaName,
                                 open = true,
                                 nodetype = 0,
                                 nocheck = areaNoCheck,
                             }).ToList();
            }
            if (nodes != null && nodes.Any())
            {
                nodes = AddWarehouse(nodes, isRma, isSelfSupport); //补充仓库
                nodes = RemoveNoWarehouse(nodes); //删除不含仓库的节点
                nodes = nodes.OrderBy(x => x.id).ToList();
            }
            return nodes;
        }

        /// <summary>
        /// 地区比较,节点去重复使用
        /// </summary>
        class Compare : IEqualityComparer<BsArea>
        {
            public bool Equals(BsArea x, BsArea y)
            {
                return x.SysNo == y.SysNo;
            }
            public int GetHashCode(BsArea codeh)
            {
                return codeh.ToString().GetHashCode();
            }
        }

        #region Private

        /// <summary>
        /// 补充子节点
        /// </summary>
        /// <param name="nodes">原有地区节点列表</param>
        /// <param name="init">是否初始化，true：不执行</param>
        /// <returns>补充后的地区列表</returns>
        /// <remarks>
        /// 2013-07-01 何方 创建
        /// </remarks>
        private List<ZTreeNode> AddParent(List<ZTreeNode> nodes, bool init)
        {
            if (init)
                return nodes;
            //缺少的父节点id
            var pids = new List<int>();
            //(from n in nodes where n.pid > 0 && nodes.All(x => x.id != n.pid) select n.pid).ToList();

            //查找所有所有父节点不在列表中的节点的父节点id
            foreach (var n in nodes)
            {
                if (n.pId > 0 && !nodes.Any(x => x.id == n.pId))
                {
                    pids.Add(n.pId);
                }
            }

            if (pids.Count > 0)
            {
                var pNodes = (from BsArea a in this.AreaAll
                              where pids.Contains(a.SysNo)
                              select
                                  new ZTreeNode
                                      {
                                          id = a.SysNo,
                                          pId = a.ParentSysNo,
                                          name = a.AreaName,
                                          open = true,
                                          nodetype = 0
                                      }).ToList();
                //补充父节点
                nodes.AddRange(pNodes);
                AddParent(nodes, init);
            }
            return nodes;
        }

        /// <summary>
        /// 补充节点
        /// </summary>
        /// <param name="nodes">原有地区节点列表</param>
        /// <param name="init">是否初始化，true：不执行</param>
        /// <returns>补充后的地区列表</returns>
        /// <remarks>
        /// 2013-07-01 何方 创建
        /// </remarks>
        private List<ZTreeNode> AddChild(List<ZTreeNode> nodes, bool init)
        {
            if (init)
                return nodes;
            var cids = new List<int>();
            //查找所有叶子节点(不含有子节点的节点)id
            foreach (var n in nodes)
            {
                if (!nodes.Any(x => x.pId == n.id))
                {
                    cids.Add(n.id);
                }
            }

            //从所有地区表中查找叶子节点的子节点
            var cnodes = (from BsArea a in this.AreaAll
                          where cids.Contains(a.ParentSysNo)
                          select
                              new ZTreeNode
                                  {
                                      id = a.SysNo,
                                      pId = a.ParentSysNo,
                                      name = a.AreaName,
                                      open = true,
                                      nodetype = 0
                                  }).ToList();

            if (cnodes.Any())
            {
                //补充子节点
                nodes.AddRange(cnodes);
                AddChild(nodes, init);
            }

            //没有子节点可以添加
            return nodes;
        }

        /// <summary>
        /// 删除不含仓库的节点
        /// </summary>
        /// <param name="nodes">The nodes.</param>
        /// <returns></returns>
        /// <remarks>
        /// 2013-07-01 何方 创建
        /// </remarks>
        private List<ZTreeNode> RemoveNoWarehouse(List<ZTreeNode> nodes)
        {
            //查找所有非仓库叶子节点(不含有子节点的节点)
            var delNotes = (from n in nodes
                            where !nodes.Any(x => x.pId == n.id) && n.nodetype == 0
                            select n).ToList();
            if (delNotes.Any())
            {
                foreach (var n in delNotes)
                {
                    nodes.Remove(n);

                }
                RemoveNoWarehouse(nodes);
            }
            return nodes;
        }
        #endregion

        /// <summary>
        /// 补充仓库节点
        /// </summary>
        /// <param name="nodes">原有地区节点列表</param>
        /// <param name="isRma">是否Rma仓</param>
        /// <param name="isSelfSupport">是否自营</param>
        /// <returns>补充后的地区列表(含仓库)</returns>
        /// <remarks>
        /// 2013-07-01 何方 创建
        /// 2013-10-22 郑荣华 增加只能查有权限的仓库
        /// 2013-10-24 黄志勇 筛选Rma仓库
        /// 2013-11-05 杨浩 添加 缓存，移除userSysNo参数
        /// </remarks>
        private List<ZTreeNode> AddWarehouse(List<ZTreeNode> nodes, bool isRma, int? isSelfSupport = null)
        {
            var listWh = Authentication.AdminAuthenticationBo.Instance.Current.Warehouses;
            var warehouse = (from item in listWh
                             where nodes.Any(x => x.id == item.AreaSysNo && (!isRma || !string.IsNullOrWhiteSpace(item.ErpRmaCode)))
                             && (!isSelfSupport.HasValue || item.IsSelfSupport == isSelfSupport.Value)
                             select new ZTreeNode
                                 {
                                     id = -item.SysNo,
                                     pId = item.AreaSysNo,
                                     name = item.BackWarehouseName,
                                     open = true,
                                     nodetype = 1,
                                     nocheck = false
                                 }).ToList();

            nodes.AddRange(warehouse);
            return nodes;
        }

        #endregion

        #region 用户与仓库

        /// <summary>
        /// 获取配送员仓库
        /// </summary>
        /// <param name="deliveryUserSysNo">配送员系统编号</param>
        /// <returns>仓库系统编号</returns>
        /// <remarks>2013-08-07 周唐炬 创建</remarks>
        public int GetDeliveryUserWarehouseSysNo(int deliveryUserSysNo)
        {
            return IWhWarehouseDao.Instance.GetDeliveryUserWarehouseSysNo(deliveryUserSysNo);
        }

        /// <summary>
        /// 获取用户有可管理的所有仓库
        /// </summary>
        /// <param name="userSysNo">用户系统编号.</param>
        /// <returns>仓库集合</returns>
        /// <remarks>
        /// 2013/7/3 何方 创建
        /// </remarks>
        public IList<WhWarehouse> GetUserWarehouseList(int userSysNo)
        {
            return IWhWarehouseDao.Instance.GetUserWarehuoseList(userSysNo);
        }

        /// <summary>
        /// 获取多个仓库的配送员
        /// </summary>
        /// <param name="warehouseSysNos">The warehouse sys nos.</param>
        /// <returns>用户集合</returns>
        /// <remarks>
        /// 2013/7/3 何方 创建
        /// </remarks>
        public IList<SyUser> GetDeliveryUserList(IList<int> warehouseSysNos)
        {
            return IWhWarehouseDao.Instance.GetDeliveryUserList(warehouseSysNos);
        }

        #endregion

        #region 仓库快递方式维护

        #region 操作
        /// <summary>
        /// 添加仓库快递方式
        /// </summary>
        /// <param name="model">仓库快递方式实体</param>        
        /// <returns>是否成功</returns>
        /// <remarks> 
        /// 2013-07-09 郑荣华 创建
        /// </remarks>
        public bool CreateWareHouseDeliveryType(WhWarehouseDeliveryType model)
        {
            var r = IWhWarehouseDao.Instance.CreateWareHouseDeliveryType(model);
            Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台, "创建仓库快递方式", LogStatus.系统日志目标类型.仓库快递方式, r);
            //清除缓存
            MemoryProvider.Default.Remove(KeyConstant.WhWarehouseDeliveryTypeList);
            return r > 0;
        }

        /// <summary>
        /// 获取所有的仓库快递方式
        /// </summary>
        /// <returns>仓库快递方式列表</returns>
        /// <remarks> 
        /// 2014-05-14 朱成果 创建
        /// </remarks>
        public List<WhWarehouseDeliveryType> GetWhWarehouseDeliveryTypeList()
        {
            var list = MemoryProvider.Default.Get<List<WhWarehouseDeliveryType>>(KeyConstant.WhWarehouseDeliveryTypeList, () => IWhWarehouseDao.Instance.GetWhWarehouseDeliveryTypeList());
            return list;
        }

        /// <summary>
        /// 删除仓库快递方式
        /// </summary>
        /// <param name="sysNo">要删除的仓库快递方式系统编号</param>
        /// <returns>是否删除成功</returns>
        /// <remarks> 
        /// 2013-07-09 郑荣华 创建
        /// </remarks>
        public bool DeleteWareHouseDeliveryType(int sysNo)
        {
            Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台, "删除仓库快递方式", LogStatus.系统日志目标类型.仓库快递方式, sysNo);
            //清除缓存
            MemoryProvider.Default.Remove(KeyConstant.WhWarehouseDeliveryTypeList);
            return IWhWarehouseDao.Instance.DeleteWareHouseDeliveryType(sysNo) > 0;
        }

        /// <summary>
        /// 删除仓库快递方式
        /// </summary>
        /// <param name="whSysNo">仓库系统编号</param>
        /// <param name="deliveryTypeSysNo">快递方式系统编号</param>
        /// <returns>受影响的行数</returns>
        /// <remarks> 
        /// 2013-07-12 郑荣华 创建
        /// </remarks>
        public bool DeleteWareHouseDeliveryType(int whSysNo, int deliveryTypeSysNo)
        {
            Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台, "删除仓库快递方式（仓库+快递）", LogStatus.系统日志目标类型.仓库快递方式, whSysNo);
            //清除缓存
            MemoryProvider.Default.Remove(KeyConstant.WhWarehouseDeliveryTypeList);
            return IWhWarehouseDao.Instance.DeleteWareHouseDeliveryType(whSysNo, deliveryTypeSysNo) > 0;
        }
        #endregion

        #region 查询

        /// <summary>
        /// 获取仓库快递方式列表信息
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <returns>仓库快递方式列表信息</returns>
        /// <remarks>
        /// 2013-07-09 郑荣华 创建
        /// </remarks>
        public IList<CBWhWarehouseDeliveryType> GetLgDeliveryType(ParaWhDeliveryTypeFilter filter)
        {
            return IWhWarehouseDao.Instance.GetLgDeliveryType(filter);
        }

        #endregion

        #endregion

        #region 仓库取件方式
        /// <summary>
        /// 仓库取件方式
        /// </summary>
        /// <param name="warehouseSysNo">仓库编号</param>
        /// <returns>取件方式列表</returns>
        /// <remarks> 2013-07-11 朱成果 创建</remarks>
        public IList<LgPickupType> GetPickupTypeListByWarehouse(int warehouseSysNo)
        {
            return IWhWarehouseDao.Instance.GetPickupTypeListByWarehouse(warehouseSysNo);
        }

        /// <summary>
        /// 添加仓库取件方式
        /// </summary>
        /// <param name="model">仓库取件方式实体</param>        
        /// <returns>添加后的系统编号</returns>
        /// <remarks> 
        /// 2013-08-28 郑荣华 创建
        /// </remarks>
        public int CreateWareHousePickUpType(WhWarehousePickupType model)
        {
            var r = IWhWarehouseDao.Instance.CreateWareHousePickUpType(model);
            Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台, "创建仓库取件方式", LogStatus.系统日志目标类型.仓库取件方式, r);
            return r;
        }

        /// <summary>
        /// 删除仓库取件方式
        /// </summary>
        /// <param name="sysNo">要删除的仓库取件方式系统编号</param>
        /// <returns>是否删除成功</returns>
        /// <remarks> 
        /// 2013-08-28 郑荣华 创建
        /// </remarks>
        public bool DeleteWareHousePickUpType(int sysNo)
        {
            Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台, "删除仓库取件方式", LogStatus.系统日志目标类型.仓库取件方式, sysNo);
            return IWhWarehouseDao.Instance.DeleteWareHousePickUpType(sysNo) > 0;
        }

        /// <summary>
        /// 删除仓库取件方式
        /// </summary>
        /// <param name="whSysNo">仓库系统编号</param>
        /// <param name="pickUpTypeSysNo">取件方式系统编号</param>
        /// <returns>是否删除成功</returns>
        /// <remarks> 
        /// 2013-08-28 郑荣华 创建
        /// </remarks>
        public bool DeleteWareHousePickUpType(int whSysNo, int pickUpTypeSysNo)
        {
            Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台, "删除仓库取件方式（仓库+取件）", LogStatus.系统日志目标类型.仓库取件方式, whSysNo);
            return IWhWarehouseDao.Instance.DeleteWareHousePickUpType(whSysNo, pickUpTypeSysNo) > 0;
        }
        #endregion

        #region 地图相关仓库
        /// <summary>
        /// 获取门店
        /// </summary>
        /// <param name="lat">纬度</param>
        /// <param name="lng">经度</param>
        /// <param name="maxAngle">相差最大角度</param>
        /// <returns>门店列表(包括距离km)</returns>
        /// <remarks>
        /// 2013-09-11 郑荣华 创建
        /// </remarks>
        public IList<CBWhWarehouse> GetWarehouseByMap(double lat, double lng, double maxAngle)
        {
            var list = GetWarehouseByMap(lat, lng, maxAngle, maxAngle);
            foreach (var item in list)
            {
                item.Distance = MapUtil.GetDistance(lat, lng, item.Latitude, item.Longitude);
            }
            list = list.OrderBy(item => item.Distance).ToList();
            return list;
        }

        /// <summary>
        /// 获取门店
        /// </summary>
        /// <param name="lat">纬度</param>
        /// <param name="lng">经度</param>
        /// <param name="maxLatAngle">纬度相差最大角度</param>
        /// <param name="maxLngAngle">经度相差最大角度</param>
        /// <returns>门店列表</returns>
        /// <remarks>
        /// 2013-09-11 郑荣华 创建
        /// </remarks>
        private static IList<CBWhWarehouse> GetWarehouseByMap(double lat, double lng, double maxLatAngle, double maxLngAngle)
        {
            return IWhWarehouseDao.Instance.GetWarehouseByMap(lat, lng, maxLatAngle, maxLngAngle);
        }

        #endregion

        #region 其它

        /// <summary>
        /// 获取借货单明细中的商品
        /// </summary>
        /// <param name="deliverymanSysNo">配送员系统编号</param>
        /// <param name="userGrade">会员等级系统编号</param>
        /// <returns>借货单中的商品</returns>
        /// <remarks>2013-07-11 沈强 创建</remarks>
        public IList<CBPdProductJson> GetProductLendGoods(int deliverymanSysNo, int? userGrade)
        {
            IList<CBPdProductJson> cbPdProductJsons = IWhWarehouseDao.Instance.GetProductLendGoods(deliverymanSysNo,
                                                                                                   userGrade);
            return cbPdProductJsons;
        }

        /// <summary>
        /// 通过配送员系统编号获取借货单商品列表
        /// </summary>
        /// <param name="deliveryUserSysNo">配送员系统编号</param>
        /// <param name="customerSysNo">会员系统编号</param>
        /// <param name="productLendStatus">借货单状态</param>
        /// <param name="priceSource">产品价格来源</param>
        /// <returns>取借货单商品列表</returns>
        /// <remarks>2013-07-11 周唐炬 创建</remarks>
        public IList<CBPdProductJson> GetProductLendItmeList(int deliveryUserSysNo,
            int customerSysNo,
            WarehouseStatus.借货单状态 productLendStatus,
            ProductStatus.产品价格来源 priceSource)
        {
            var customer = ICrCustomerDao.Instance.GetCrCustomerItem(customerSysNo);
            var level = 1; //会员默认等级
            if (null != customer)
            {
                level = customer.LevelSysNo; //加入会员等级
            }
            return IWhWarehouseDao.Instance.GetProductLendGoods(deliveryUserSysNo, level, null, productLendStatus, priceSource);
        }

        /// <summary>
        /// 通过配送员系统编号获取借货单商品列表
        /// </summary>
        /// <param name="deliveryUserSysNo">配送员系统编号</param>
        /// <param name="warehouseSysNo">仓库系统编号</param>
        /// <param name="productLendStatus">借货单状态</param>
        /// <param name="priceSource">产品价格来源</param>
        /// <returns>取借货单商品列表</returns>
        /// <remarks>2013-07-11 周唐炬 创建</remarks>
        public IList<CBPdProductJson> GetProductLendItmeList(int deliveryUserSysNo, int? warehouseSysNo, WarehouseStatus.借货单状态 productLendStatus, ProductStatus.产品价格来源 priceSource)
        {
            return IWhWarehouseDao.Instance.GetProductLendGoods(deliveryUserSysNo, null, warehouseSysNo, productLendStatus, priceSource);
        }
        #endregion


        /// <summary>
        /// 快速搜索
        /// </summary>
        /// <param name="condition">搜索条件实体</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="userSysNo">当前用户编号</param>
        /// <returns>符合搜索条件的实体集合</returns>
        /// <remarks>2016-5-27 杨浩 创建</remarks>
        public PagedList<WhWarehouse> QuickSearch(WarehouseSearchCondition condition, int pageIndex, int pageSize, int? userSysNo = null)
        {
            var model = new PagedList<WhWarehouse>();
            if (condition == null)
            {
                return model;
            }

            var result = IWhWarehouseDao.Instance.QuickSearch(condition, pageIndex, pageSize, userSysNo);
            model.TData = result.Rows;
            model.PageSize = pageSize;
            model.TotalItemCount = result.TotalRows;
            model.CurrentPageIndex = pageIndex;
            return model;
        }

        /// <summary>
        /// 快速搜索
        /// </summary>
        /// <param name="condition">搜索条件实体</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <returns>符合搜索条件的实体集合</returns>
        /// <remarks>2013-08-12 周瑜 创建</remarks>
        public PagedList<CBWhWarehouse> QuickSearch(WarehouseSearchCondition condition, int pageIndex, int pageSize)
        {
            var model = new PagedList<CBWhWarehouse>();
            if (condition == null)
            {
                return model;
            }

            var result = IWhWarehouseDao.Instance.QuickSearch(condition, pageIndex, pageSize);
            model.TData = result.Rows;
            model.PageSize = pageSize;
            model.TotalItemCount = result.TotalRows;
            model.CurrentPageIndex = pageIndex;
            return model;
        }

        /// <summary>
        /// 新增仓库
        /// </summary>
        /// <param name="warehouse">仓库实体</param>
        /// <returns></returns>
        /// <remarks>2013-08-12 周瑜 创建</remarks>
        public int Insert(WhWarehouse warehouse)
        {
            SysLog.Instance.Info(LogStatus.系统日志来源.后台, "新增仓库", LogStatus.系统日志目标类型.仓库, warehouse.SysNo, (Authentication.AdminAuthenticationBo.Instance.Current == null ? 12 : Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo));
            int id = IWhWarehouseDao.Instance.Insert(warehouse);
            //清缓存
            MemoryProvider.Default.Remove(KeyConstant.WarehouseList);
            return id;
        }

        /// <summary>
        /// 修改仓库信息
        /// </summary>
        /// <param name="warehouse">仓库实体</param>
        /// <returns></returns>
        /// <remarks>2013-08-12 周瑜 创建
        /// 2014-5-13 杨文兵 增加清除缓存代码
        /// </remarks>
        public int Update(WhWarehouse warehouse)
        {
            SysLog.Instance.Info(LogStatus.系统日志来源.后台, "修改仓库信息", LogStatus.系统日志目标类型.仓库, warehouse.SysNo, (Authentication.AdminAuthenticationBo.Instance.Current == null ? 12 : Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo));
            int id = IWhWarehouseDao.Instance.Update(warehouse);
            //清缓存
            MemoryProvider.Default.Remove(KeyConstant.WarehouseList);


            // 2014-5-13 add by ywb
            var cacheKey = string.Format("CACHE_WHWAREHOUSE_{0}", warehouse.SysNo);
            var cacheKey1 = string.Format("CACHE_WHWAREHOUSE_{0}_CB", warehouse.SysNo);
            MemoryProvider.Default.Remove(cacheKey);
            MemoryProvider.Default.Remove(cacheKey1);
            //更新当前用户所能操作的所有仓库 余勇 2014-09-11
            var hasAllWarehouse = SyUserGroupBo.Instance.IsHasAllWarehouse((Authentication.AdminAuthenticationBo.Instance.Current == null ? 12 : Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo));
            AdminAuthenticationBo.Instance.Current.Warehouses = hasAllWarehouse
              ? WhWarehouseBo.Instance.GetAllWarehouseList()
              : WhWarehouseBo.Instance.GetUserWarehouseList((Authentication.AdminAuthenticationBo.Instance.Current == null ? 12 : Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo));

            return id;
        }

        /// <summary>
        /// 修改仓库状态
        /// </summary>
        /// <param name="warehouse">仓库实体</param>
        /// <returns></returns>
        /// <remarks>2013-08-12 周瑜 创建
        /// 2014-5-13 杨文兵 增加清除缓存代码
        /// </remarks>
        public int UpdateStatus(WhWarehouse warehouse)
        {
            SysLog.Instance.Info(LogStatus.系统日志来源.后台, "修改仓库状态", LogStatus.系统日志目标类型.仓库, warehouse.SysNo, (Authentication.AdminAuthenticationBo.Instance.Current == null ? 12 : Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo));
            int id = IWhWarehouseDao.Instance.UpdateStatus(warehouse);
            //清缓存
            MemoryProvider.Default.Remove(KeyConstant.WarehouseList);

            // 2014-5-13 add by ywb
            var cacheKey = string.Format("CACHE_WHWAREHOUSE_{0}", warehouse.SysNo);
            var cacheKey1 = string.Format("CACHE_WHWAREHOUSE_{0}_CB", warehouse.SysNo);
            MemoryProvider.Default.Remove(cacheKey);
            MemoryProvider.Default.Remove(cacheKey1);

            return id;
        }

        /// <summary>
        /// 获取仓库详情
        /// </summary>
        /// <param name="sysno">仓库编号</param>
        /// <remarks> 
        /// 2013-08-07 周瑜 创建
        /// 2014-5-13 杨文兵 将此方法加入缓存
        /// </remarks>
        public CBWhWarehouse GetWarehouse(int sysno)
        {
            /* 使用了CB前缀的对象，导致无法使用 GetWarehouseEntity()方法的缓存结果
             * 所以将数据加入另一个缓存Key （服务器目前可用内存比较大，所以采用此方法）             
             * 规则:缓存key中使用了_CB后缀，表示缓存的是CB前缀的对象，这些代码以后需要重构。
            */
            var cacheKey = string.Format("CACHE_WHWAREHOUSE_{0}_CB", sysno);
            return MemoryProvider.Default.Get<CBWhWarehouse>(cacheKey, 60 * 24, () =>
            {
                return IWhWarehouseDao.Instance.GetWarehouse(sysno);
            }, CachePolicy.Absolute);

        }

        /// <summary>
        /// 查询覆盖该地区的所有仓库
        /// </summary>
        /// <param name="areaSysNo">覆盖地区系统编号</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <returns>返回地区仓库分页列表</returns>
        /// <remarks>2013-08-14 周瑜 创建</remarks>
        public PagedList<CBAreaWarehouse> GetWarehouseForArea(int? areaSysNo, int pageIndex, int pageSize)
        {
            var model = new PagedList<CBAreaWarehouse>();
            var areaSysNos = new List<int>();

            if (areaSysNo.HasValue)
            {
                var area = BasicAreaBo.Instance.GetArea(areaSysNo.Value);
                if (area == null)
                {
                    throw new Exception("地区:" + areaSysNo.Value + "不存在");
                }
                if (area.AreaLevel == 3)//区级
                {
                    areaSysNos.Add(areaSysNo.Value);
                }
                if (area.AreaLevel == 2)//市级
                {
                    areaSysNos.AddRange(BasicAreaBo.Instance.GetAreaList(areaSysNo.Value).Select(x => x.SysNo));
                }
                if (area.AreaLevel == 1)//升级
                {
                    var citys = BasicAreaBo.Instance.GetAreaList(areaSysNo.Value);
                    foreach (var c in citys)
                    {
                        areaSysNos.AddRange(BasicAreaBo.Instance.GetAreaList(c.SysNo).Select(x => x.SysNo));
                    }
                }
            }

            var result = IWhWarehouseDao.Instance.GetWarehouseForArea(areaSysNos.ToArray(), pageIndex, pageSize);
            model.TData = result.Rows;
            model.PageSize = pageSize;
            model.TotalItemCount = result.TotalRows;
            model.CurrentPageIndex = pageIndex;

            return model;
        }

        /// <summary>
        /// 获取支持该服务地区的对应的仓库
        /// </summary>
        /// <param name="supportArea">支持地区</param>
        /// <param name="warehouseType">仓库类型</param>
        /// <returns></returns>
        /// <remarks>2013-08-28 朱成果 创建</remarks>
        public WhWarehouse GetWhWarehouseBySupportArea(int supportArea, int? warehouseType)
        {
            return Hyt.DataAccess.Warehouse.IWhWarehouseDao.Instance.GetWhWarehouseBySupportArea(supportArea, warehouseType).FirstOrDefault();
        }

        /// <summary>
        /// 根据服务地区,仓库类型,支持的物流类型获取仓库
        /// </summary>
        /// <param name="supportArea">服务覆盖地区.</param>
        /// <param name="warehouseType">类型,门店,长裤.</param>
        /// <param name="deliveryType">配送方式</param>
        /// <param name="status">仓库状态: 禁用/启用</param>
        /// <returns></returns>
        /// <remarks>
        /// 2013/9/16 何方 定义
        /// </remarks>
        public IList<WhWarehouse> GetWhWareHouse(int? supportArea = null, WarehouseStatus.仓库类型? warehouseType = null, int? deliveryType = null, WarehouseStatus.仓库状态? status = null)
        {
            return IWhWarehouseDao.Instance.GetWhWareHouse(supportArea, warehouseType, deliveryType, status);
        }

        /// <summary>
        /// 根据过滤条件查询出库单
        /// </summary>
        /// <param name="condition">过滤条件</param>
        /// <param name="pageSize">页大小</param>
        /// <returns>出库单实体列表</returns>
        /// <remarks>2013-07-04 周唐炬 创建</remarks>
        public PagedList<CBWhStockOut> SearchFilter(StockOutSearchCondition condition, int pageSize)
        {
            PagedList<CBWhStockOut> model = null;
            if (condition != null)
            {
                model = new PagedList<CBWhStockOut>();
                var result = IOutStockDao.Instance.SearchFilter(condition, pageSize);

                if (result.Rows != null)
                {
                    foreach (var t in result.Rows)
                    {
                        var soReceiveAddress = SoOrderBo.Instance.GetOrderReceiveAddress(t.ReceiveAddressSysNo);
                        if (soReceiveAddress != null)
                        {
                            t.ReceiverName = soReceiveAddress.Name;
                            t.District = BasicAreaBo.Instance.GetAreaFullName(soReceiveAddress.AreaSysNo);  // 2014-03-05 唐文均 修改 返回实体增加地址的值
                            t.StreetAddress = soReceiveAddress.StreetAddress;
                        }
                        else
                        {
                            t.ReceiverName = "找不到系统编号为：" + t.ReceiveAddressSysNo + "的收件地址";
                            t.StreetAddress = "未找到该收件地址";
                        }
                    }
                }

                model.TData = result.Rows;
                model.TotalItemCount = result.TotalRows;
                model.CurrentPageIndex = condition.CurrentPage;
            }
            return model;
        }

        /// <summary>
        ///     查询所有具有相同商品编号，商品数量的出库单
        /// </summary>
        /// <returns>出库单实体列表</returns>
        /// <remarks>2013-06-08 周瑜 创建</remarks>
        public IList<CBWhStockOut> GetSingleBatchDO()
        {
            var conditon = new StockOutSearchCondition
            {
                EndDate = DateTime.Now,
                Status = WarehouseStatus.出库单状态.待出库.GetHashCode(),
                //WarehouseSysNo = 1 //todo: change to the warehouse belong to the login user
            };
            var masters = IOutStockDao.Instance.Search(conditon, 1, Int32.MaxValue);
            IList<CBWhStockOut> returnList = new List<CBWhStockOut>();
            for (int i = 0; i < masters.Rows.Count; i++)
            {
                var d = masters.Rows.Where(x => x.Equals(masters.Rows[i])).ToList();
                //如果相同商品的出库单数量小于2， 即只存在一个出库单，则无批量拣货的意义
                if (d.Count() < 2)
                {
                    continue;
                }
                foreach (var b in d)
                {
                    if (returnList.All(m => m.SysNo != b.SysNo))
                    {
                        b.GroupId = i;
                        //Console.WriteLine("sysno:" + b.SysNo + " groupId:" + b.GroupId);
                        //foreach (var item in b.Items)
                        //{
                        //    Console.WriteLine("item:" + item.ProductSysNo + " qty:" + item.ProductQuantity);
                        //}
                        //Console.WriteLine("-----------------------------------------------------");
                        returnList.Add(b);
                    }
                }
            }

            #region retrunList 数据样本
            /*
-----------------------------------------------------
sysno:46 groupId:21
item:2 qty:20
-----------------------------------------------------
sysno:48 groupId:21
item:2 qty:20
-----------------------------------------------------
sysno:56 groupId:29
item:3 qty:50
-----------------------------------------------------
sysno:57 groupId:29
item:3 qty:50
-----------------------------------------------------
sysno:84 groupId:36
item:1 qty:1
item:2 qty:1
item:3 qty:1
item:4 qty:1
-----------------------------------------------------
sysno:103 groupId:36
item:2 qty:1
item:3 qty:1
item:1 qty:1
item:4 qty:1
-----------------------------------------------------
             */
            #endregion
            return returnList;
        }

        /// <summary>
        ///     获取出库单的详细信息
        /// </summary>
        /// <param name="sysno">出库单系统编号</param>
        /// <returns>出库单详细信息实体</returns>
        /// <remarks>2013-06-25 周瑜 创建</remarks>
        /// <remarks>2014-04-14 余勇 修改</remarks>
        public CBWhStockOut GetStockOutInfo(int sysno)
        {
            CBWhStockOut cbWhStockOut = null;
            cbWhStockOut = IOutStockDao.Instance.GetStockOutInfo(sysno);

            if (cbWhStockOut != null)
            {
                #region 获取订单信息 （和支付方式 郑荣华 2013-11-28）

                SoOrder soOrder = SoOrderBo.Instance.GetEntity(cbWhStockOut.OrderSysNO);
                if (soOrder != null)
                {
                    cbWhStockOut.SoRemarks = soOrder.Remarks;
                    cbWhStockOut.SalesType = soOrder.SalesType;
                    cbWhStockOut.FreightAmount = soOrder.FreightAmount;
                    cbWhStockOut.CustomerSysNo = soOrder.CustomerSysNo;
                    cbWhStockOut.SoInternalRemarks = soOrder.InternalRemarks;
                    var paytype = PaymentTypeBo.Instance.GetEntity(soOrder.PayTypeSysNo);
                    cbWhStockOut.PaymentName = paytype == null ? "第三方平台支付方式" : paytype.PaymentName;
                    cbWhStockOut.OrderSource = EnumUtil.GetDescription(typeof(OrderStatus.销售单来源), soOrder.OrderSource);
                }

                #endregion

                #region 获取仓库名称

                cbWhStockOut.WarehouseName = GetWarehouseName(cbWhStockOut.WarehouseSysNo);

                #endregion

                #region 获取配送类型

                CBLgDeliveryType cbLgDelivery =
                    Logistics.DeliveryTypeBo.Instance.GetDeliveryType(cbWhStockOut.DeliveryTypeSysNo);
                cbWhStockOut.DeliveryTypeName = cbLgDelivery.DeliveryTypeName;
                cbWhStockOut.IsThirdpartyExpress = cbLgDelivery.IsThirdPartyExpress;

                #endregion

                #region 获取收货地址信息

                SoReceiveAddress address =
                    SoOrderBo.Instance.GetOrderReceiveAddress(cbWhStockOut.ReceiveAddressSysNo);
                if (address != null)
                {
                    cbWhStockOut.StreetAddress = address.StreetAddress;
                    cbWhStockOut.PhoneNumber = address.PhoneNumber;
                    cbWhStockOut.MobilePhoneNumber = address.MobilePhoneNumber;
                    cbWhStockOut.ZipCode = address.ZipCode;
                    cbWhStockOut.ReceiverName = address.Name;
                    #region 获取地区

                    cbWhStockOut.District = BasicAreaBo.Instance.GetAreaFullName(address.AreaSysNo);

                    #endregion
                }

                #endregion

                #region 获取发票信息

                FnInvoice fnInvoice = FnInvoiceBo.Instance.GetFnInvoice(cbWhStockOut.InvoiceSysNo); // DataAccess.Order.IFnInvoiceDao.Instance.GetFnInvoice(cbWhStockOut.InvoiceSysNo);
                if (fnInvoice != null)
                {
                    FnInvoiceType fnInvoiceType = FnInvoiceTypeBo.Instance.GetModel(fnInvoice.InvoiceTypeSysNo);
                    cbWhStockOut.InvoiceTypeSysNo = fnInvoice.InvoiceTypeSysNo;
                    cbWhStockOut.InvoiceTitle = fnInvoice.InvoiceTitle;
                    cbWhStockOut.InvoiceStatus = fnInvoice.Status;
                    cbWhStockOut.InvoiceCode = fnInvoice.InvoiceCode;
                    cbWhStockOut.InvoiceNo = fnInvoice.InvoiceNo;
                    cbWhStockOut.InvoiceAmount = fnInvoice.InvoiceAmount;
                    cbWhStockOut.InvoiceRemarks = fnInvoice.InvoiceRemarks;
                    cbWhStockOut.InvoiceTypeName = fnInvoiceType.Name;
                }

                #endregion
            }

            return cbWhStockOut;
        }

        /// <summary>
        ///  出库单快速搜索
        /// </summary>
        /// <param name="status">出库单系统编号</param>
        /// <param name="isInvoice">出库单是否开具发票</param>
        /// <param name="sysno">出库单系统编号</param>
        /// <param name="deliverySysNo">配送方式系统编号</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="warehouseSysNo">仓库系统编号</param>
        /// <param name="userSysNos">当前用户系统编号</param>
        /// <param name="isHasAllWarehouse">是否拥有所有仓库</param>
        /// <returns>出库单实体列表</returns>
        /// <remarks>2013-06-25 周瑜 创建</remarks>
        public PagedList<CBWhStockOut> QuickSearch(int? status, int? isInvoice, string sysno, int? deliverySysNo, int pageIndex, int pageSize, int? warehouseSysNo, int userSysNos, bool isHasAllWarehouse)
        {

            var model = new PagedList<CBWhStockOut>();
            var result = IOutStockDao.Instance.QuickSearch(status, isInvoice, deliverySysNo, sysno, pageIndex, pageSize, warehouseSysNo, userSysNos, isHasAllWarehouse);
            model.TData = result.Rows;
            model.PageSize = pageSize;
            model.TotalItemCount = result.TotalRows;
            model.CurrentPageIndex = pageIndex;
            return model;
        }

        /// <summary>
        ///     根据输入条件查询出库单
        /// </summary>
        /// <param name="condition">搜索条件</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="userSysNo">当前用户系统编号</param>
        /// <param name="isHasAllWarehouse">是否拥有所有仓库</param>
        /// <returns>出库单实体列表</returns>
        /// <remarks>2013-06-08 周瑜 创建</remarks>
        public PagedList<CBWhStockOut> Search(StockOutSearchCondition condition, int pageIndex, int pageSize, int userSysNo, bool isHasAllWarehouse)
        {
            if (condition == null)
            {
                return null;
            }

            var model = new PagedList<CBWhStockOut>();
            var result = IOutStockDao.Instance.Search(condition, pageIndex, pageSize, userSysNo, isHasAllWarehouse);

            if (result.Rows != null)
            {
                foreach (var t in result.Rows)
                {
                    var soReceiveAddress = SoOrderBo.Instance.GetOrderReceiveAddress(t.ReceiveAddressSysNo);
                    if (soReceiveAddress != null)
                    {
                        t.ReceiverName = soReceiveAddress.Name;
                    }
                    else
                    {
                        t.ReceiverName = "找不到系统编号为：" + t.ReceiveAddressSysNo + "的收件地址";
                    }

                    // 获取签收图片的真实全路径 2014-03-06 唐文均
                    string urlSignImage = string.Empty;
                    if (!string.IsNullOrWhiteSpace(t.SignImage))
                    {
                        urlSignImage = string.Format("{0}App\\Logistics\\{1}\\{2}\\{3}", Config.Config.Instance.GetAttachmentConfig().FileServer, t.SignImage.Substring(0, 1), t.SignImage.Substring(1, 2), t.SignImage);
                    }
                    t.SignImage = urlSignImage;

                    // 获取订单来源 2014-03-07 唐文均
                    var orderEnt = SoOrderBo.Instance.GetEntity(t.OrderSysNO);
                    if (orderEnt != null)
                    {
                        t.OrderSource = EnumUtil.GetDescription(typeof(OrderStatus.销售单来源), orderEnt.OrderSource);
                    }
                }
            }

            model.TData = result.Rows;
            model.PageSize = pageSize;
            model.TotalItemCount = result.TotalRows;
            model.CurrentPageIndex = pageIndex;
            return model;
        }

        /// <summary>
        ///     修改出库单
        /// </summary>
        /// <param name="model">用于修改出库单的实体</param>
        /// <returns>出库单实体列表</returns>
        /// <remarks>2013-06-08 周瑜 创建</remarks>
        public void UpdateStockOut(WhStockOut model)
        {
            //SysLog.Instance.Info(LogStatus.系统日志来源.后台, "修改出库单", LogStatus.系统日志目标类型.出库单, model.SysNo, Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo);
            IOutStockDao.Instance.Update(model);
        }
        /// <summary>
        ///     修改出库单备注
        /// </summary>
        /// <param name="model">用于修改出库单的实体</param>
        /// <returns>出库单实体列表</returns>
        /// <remarks>罗勤尧 创建</remarks>
        public void UpdateStockOutRemarks(int SysNo, string Remarks)
        {
            //SysLog.Instance.Info(LogStatus.系统日志来源.后台, "修改出库单", LogStatus.系统日志目标类型.出库单, model.SysNo, Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo);
            IOutStockDao.Instance.UpdateRemarks(SysNo, Remarks);
        }
        /// <summary>
        /// 通过状态修改出库单（用于事务处理）
        /// </summary>
        /// <param name="model">用于修改出库单的实体</param>
        /// <param name="status">状态</param>
        /// <returns>返回受影响的行数</returns>
        /// <remarks>2014-08-01 余勇 创建</remarks>
        public int UpdateStockOutByStatus(WhStockOut model, int status)
        {
            return IOutStockDao.Instance.UpdateStockOutByStatus(model, status);
        }

        /// <summary>
        /// 修改出库单退货数量
        /// </summary>
        /// <param name="stockOutSysNo">出库单系统编号.</param>
        /// <param name="returnQuantity">退货数量.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">退货商品:数量超过了出库数量</exception>
        /// <remarks>
        /// 2013/7/11 何方 创建
        /// </remarks>
        public void UpdateStockOutItemReturnQuantity(int stockOutSysNo, Dictionary<int, int> returnQuantity)
        {
            SysLog.Instance.Info(LogStatus.系统日志来源.后台, "修改出库单退货数量", LogStatus.系统日志目标类型.出库单, stockOutSysNo, (Authentication.AdminAuthenticationBo.Instance.Current == null ? 12 : Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo));
            var stockOutItem = GetWhStockOutItemList(stockOutSysNo);
            foreach (var orderItemSysNo in returnQuantity.Keys)
            {
                foreach (var item in stockOutItem)
                {
                    if (item.OrderItemSysNo == orderItemSysNo)
                    {
                        item.ReturnQuantity += returnQuantity[orderItemSysNo];
                        if (item.ProductQuantity < item.ReturnQuantity)
                        {
                            throw new Exception("退货商品:数量超过了出库数量");
                        }
                        IOutStockDao.Instance.UpdateOutItem(item);
                    }
                }
            }

        }

        /// <summary>
        /// 更新出库单明细
        /// </summary>
        /// <param name="item">出库单明细实体</param>
        /// <returns></returns>
        /// <remarks>2013-07-15 朱成果 创建</remarks>
        public void UpdateOutItem(WhStockOutItem item)
        {
            IOutStockDao.Instance.UpdateOutItem(item);
        }

        /// <summary>
        ///     打印拣货单
        /// </summary>
        /// <param name="model">出库单实体</param>
        /// <remarks>2013-06-08 周瑜 创建</remarks>
        public void PrintPickingOrder(WhStockOut model)
        {
        }

        /// <summary>
        ///     打印装箱单
        /// </summary>
        /// <param name="model">出库单实体</param>
        /// <remarks>2013-06-08 周瑜 创建</remarks>
        public void PrintPackageOrder(WhStockOut model)
        {
        }

        /// <summary>
        ///     打印发票
        /// </summary>
        /// <param name="model">出库单实体</param>
        /// <remarks>2013-06-08 周瑜 创建</remarks>
        public void PrintInvoice(WhStockOut model)
        {
            if (model.InvoiceSysNo > 0)
            {

            }
        }

        /// <summary>
        ///     打印运单
        /// </summary>
        /// <param name="model"></param>
        public void PrintWaybill(WhStockOut model)
        {
        }

        /// <summary>
        ///     修改订单状态
        /// </summary>
        /// <param name="sysNo">出库单系统系统编号</param>
        /// <param name="userSysNo">用户编号</param>
        /// <remarks>2013-06-08 周瑜 创建</remarks>
        public void UpdateSOStatus(int sysNo, int userSysNo, string reason = null)
        {
            SysLog.Instance.Info(LogStatus.系统日志来源.后台, "修改订单状态", LogStatus.系统日志目标类型.出库单, sysNo, (Authentication.AdminAuthenticationBo.Instance.Current == null ? 12 : Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo));
            var so = new SoOrderBo();
            var u = new SyUserBo();
            var user = u.GetSyUser(userSysNo);
            so.UpdateSoStatusForSotckOutCancel(sysNo, user, reason);
        }

        /// <summary>
        ///  出库
        /// </summary>
        /// <param name="model">出库单实体</param>
        /// <remarks>2013-06-08 周瑜 创建</remarks>
        public void OutStock(WhStockOut model)
        {
            //用于保存分批出库时，创建的出库单
            WhStockOut newStockOut = null;

            //如果出库单中的商品明细与已扫描的商品明细以及数量不匹配
            //那么就要进行分批出库
            //得到未扫描的以及扫描数量不足的商品列表

            //var deliveryType = DeliveryTypeBo.Instance.GetDeliveryType(model.DeliveryTypeSysNo);
            var order = SoOrderBo.Instance.GetEntity(model.OrderSysNO);
            #region 拆分出库单

            var items =
                model.Items.Where(item => !item.IsScaned || item.ProductQuantity != item.ScanedQuantity).ToList();
            if (items.Count > 0)
            {
                //得到数量不匹配的商品列表
                newStockOut = new WhStockOut
                    {
                        TransactionSysNo = model.TransactionSysNo,
                        WarehouseSysNo = model.WarehouseSysNo,
                        OrderSysNO = model.OrderSysNO,
                        ReceiveAddressSysNo = model.ReceiveAddressSysNo,
                        IsCOD = model.IsCOD,

                        //经吴文强确认所有出库单的应收金额都为订单里总金额现金支付部分
                        Receivable = model.Receivable == 0 ? 0 : order.CashPay,
                        SignTime = DateTime.Now.AddDays(1),//新建出库单， 无签收日期
                        IsPrintedPackageCover = WarehouseStatus.是否已经打印包裹单.否.GetHashCode(),
                        IsPrintedPickupCover = WarehouseStatus.是否已经打印拣货单.否.GetHashCode(),
                        CustomerMessage = model.CustomerMessage,
                        Remarks = "创建新出库单,来源:拆分出库单：" + model.SysNo,
                        Status = (int)WarehouseStatus.出库单状态.待出库,
                        DeliveryRemarks = string.IsNullOrEmpty(model.DeliveryRemarks) ? "" : model.DeliveryRemarks,
                        DeliveryTime = string.IsNullOrEmpty(model.DeliveryTime) ? "" : model.DeliveryTime,
                        DeliveryTypeSysNo = model.DeliveryTypeSysNo,
                        ContactBeforeDelivery = model.ContactBeforeDelivery,
                        //发票由第一张出库单开具, 其余分批出库单不需要再开具发票
                        InvoiceSysNo = 0,
                        PickUpDate = DateTime.Now,
                        StockOutDate = DateTime.Now,
                        Stamp = DateTime.Now,
                        CreatedDate = DateTime.Now,
                        CreatedBy = model.LastUpdateBy,
                        LastUpdateDate = DateTime.Now,
                        LastUpdateBy = model.LastUpdateBy,
                        Items = new List<WhStockOutItem> { }
                    };

                foreach (var item in items)
                {
                    var newItem = IOutStockDao.Instance.GetStockOutItem(item.SysNo);
                    //设置金额为未出库的金额
                    newItem.RealSalesAmount = item.RealSalesAmount - Math.Round(item.RealSalesAmount * (item.ScanedQuantity / (decimal)item.ProductQuantity), 2);
                    //对于新建的出库单, 将商品数量置为未扫描数量
                    newItem.ProductQuantity = item.ProductQuantity - item.ScanedQuantity;
                    newStockOut.Items.Add(newItem);
                }
                newStockOut.StockOutAmount = items.Sum(x => x.RealSalesAmount);
                //拆分出库,逻辑删除未出库的商品,添加到新的出库单中
                RemoveItem(items.Where(m => m.IsScaned == false).ToList());
                //Insert(doMaster);

                newStockOut.SysNo = IOutStockDao.Instance.Insert(newStockOut);

                SysLog.Instance.Info(LogStatus.系统日志来源.后台, "创建新出库单,来源:拆分出库单：" + model.SysNo, LogStatus.系统日志目标类型.出库单, newStockOut.SysNo,

                                     (Authentication.AdminAuthenticationBo.Instance.Current == null ? 12 : Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo));

                SoOrderBo.Instance.WriteSoTransactionLog(newStockOut.TransactionSysNo, "拆分出库,创建新出库单单号：" + newStockOut.SysNo,

                    (Authentication.AdminAuthenticationBo.Instance.Current == null ? "系统管理员" : Authentication.AdminAuthenticationBo.Instance.Current.Base
                                                                       .UserName));
            }

            #endregion

            if (newStockOut == null)
            {
                SysLog.Instance.Info(LogStatus.系统日志来源.后台, "出库", LogStatus.系统日志目标类型.出库单, model.SysNo,
                                 (Authentication.AdminAuthenticationBo.Instance.Current == null ? 12 : Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo));
            }
            else
            {
                SysLog.Instance.Info(LogStatus.系统日志来源.后台, "分批出库，拆分后的出库单号为：" + newStockOut.SysNo, LogStatus.系统日志目标类型.出库单, model.SysNo,
                                 (Authentication.AdminAuthenticationBo.Instance.Current == null ? 12 : Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo));
                model.Remarks = "分批出库，拆分后的出库单号为：" + newStockOut.SysNo;
            }
            #region 修改原出库单中的商品数量
            //本次出库的出库单明细
            var stockOutitems = model.Items.Where(item => item.IsScaned && item.ScanedQuantity > 0).ToList();
            foreach (var item in stockOutitems)
            {
                //设置金额为实际出库单金额
                item.RealSalesAmount = Math.Round(item.RealSalesAmount * (item.ScanedQuantity / (decimal)item.ProductQuantity), 2);
                item.ProductQuantity = item.ScanedQuantity;
                item.LastUpdateBy = model.LastUpdateBy;
                item.LastUpdateDate = DateTime.Now;

            }
            IOutStockDao.Instance.UpdateItems(stockOutitems);
            #endregion

            model.Status = WarehouseStatus.出库单状态.待拣货.GetHashCode();
            model.Receivable = model.Receivable == 0 ? 0 : order.CashPay;
            //model.StockOutAmount = model.Items.Where(x => x.Status != WarehouseStatus.出库单明细状态.无效.GetHashCode()).Sum(m => m.RealSalesAmount);
            model.StockOutAmount = stockOutitems.Sum(m => m.RealSalesAmount);

            //model.StockOutBy = Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo;
            //model.StockOutDate = DateTime.Now;
            UpdateStockOut(model);
            #region 2014-04-09 朱成果 不再出库单出库修改库存
            //UpdateErpProductNumber(model.SysNo); //修改ERP库存
            try
            {
                // Extra.Erp.Control.ERPSellOutStockControl controlBll = new Extra.Erp.Control.ERPSellOutStockControl();
                // Extra.Erp.Model.Result result = controlBll.ErpSellOutStockAdd(model.SysNo);
            }
            catch { }

            #endregion



            //结束事务
        }

        /// <summary>
        /// 新增出库单
        /// </summary>
        /// <param name="model">用于新增出库单的实体</param>
        /// <remarks>2013-06-08 周瑜 创建</remarks>
        public void Insert(WhStockOut model)
        {
            SysLog.Instance.Info(LogStatus.系统日志来源.后台, "新增出库单", LogStatus.系统日志目标类型.出库单, model.SysNo, (Authentication.AdminAuthenticationBo.Instance.Current == null ? 12 : Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo));
            IOutStockDao.Instance.Insert(model);
        }

        /// <summary>
        ///    拆分出库逻辑删除出库单中的商品,添加到新的出库单中
        /// </summary>
        /// <param name="items">将要删除的商品列表</param>
        /// <remarks>2013-06-08 周瑜 创建</remarks>
        public void RemoveItem(IList<WhStockOutItem> items)
        {
            if (items == null || items.Count == 0)
            {
                return;
            }
            SysLog.Instance.Info(LogStatus.系统日志来源.后台, "拆分出库逻辑删除出库单中的商品,添加到新的出库单中", LogStatus.系统日志目标类型.出库单, items[0].StockOutSysNo, (Authentication.AdminAuthenticationBo.Instance.Current == null ? 12 : Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo));
            IOutStockDao.Instance.RemoveItem(items);
        }

        /// <summary>
        /// 作废出库单
        /// </summary>
        /// <param name="sysNo">出库单系统编号</param>
        /// <param name="userSysNo">用户系统编号</param>
        /// <param name="text">出库单作废原因</param>
        /// <returns>成功返回true,失败返回false</returns>
        /// <remarks>2013-07-01 周瑜 创建</remarks>
        /// <remarks>2013-11-04 沈强 修改（添加创建入库单业务）</remarks>
        public Hyt.Model.Result CancelStockOut(int sysNo, int userSysNo, string text)
        {

            var resultObj = new Hyt.Model.Result { StatusCode = -1, Status = false };
            //获取出库单对象
            var stockOut = Get(sysNo);

            if (stockOut == null)
            {
                resultObj.Message = string.Format("不存在出库单号为:{0}的出库单", sysNo);
                return resultObj;
            }

            if (!Authentication.AdminAuthenticationBo.Instance.HasWareHousePrivilege(stockOut.WarehouseSysNo))
            {
                throw new Exception("用户没有编号为" + stockOut.WarehouseSysNo + "仓库的权限");
            }

            if (string.IsNullOrWhiteSpace(text))
            {
                resultObj.Message = "请输入作废原因！";
                return resultObj;
            }
            string tmp = stockOut.Remarks + ";（" + DateTime.Now.ToString("yyyy-MM-dd HH:mm") + ")" + sysNo +
                         "出库单作废原因：" +
                         text;
            if (tmp.Length >= 200)
            {
                resultObj.Message = "作废原因字数太多，请减少相应字数！";
                return resultObj;
            }

            if (!(stockOut.Status == WarehouseStatus.出库单状态.待出库.GetHashCode()
                  || stockOut.Status == WarehouseStatus.出库单状态.待打包.GetHashCode()
                  || stockOut.Status == WarehouseStatus.出库单状态.待拣货.GetHashCode()
                  || stockOut.Status == WarehouseStatus.出库单状态.待配送.GetHashCode()
                 ))
            {
                resultObj.Message = string.Format("出库单不处于{0}{1}{2}{3}状态,不能作废", WarehouseStatus.出库单状态.待出库,
                                                  WarehouseStatus.出库单状态.待打包, WarehouseStatus.出库单状态.待拣货,
                                                  WarehouseStatus.出库单状态.待配送);
                return resultObj;
            }



            #region 已经不再出库单出库的时候减库存，也不作废生成入库单,改在配送单生成和作废的时候进行 2014-04-09 朱成果

            //if (stockOut.Status != WarehouseStatus.出库单状态.待出库.GetHashCode())
            //{
            //    var whStockIn = new WhStockIn
            //        {
            //            WarehouseSysNo = stockOut.WarehouseSysNo,
            //            CreatedBy = userSysNo,
            //            CreatedDate = DateTime.Now,
            //            DeliveryType = (int)WarehouseStatus.入库物流方式.作废出库,
            //            IsPrinted = 0,
            //            SourceSysNO = stockOut.SysNo,
            //            SourceType = (int)LogisticsStatus.配送单据类型.出库单,
            //            Status = (int)WarehouseStatus.入库单状态.待入库,
            //            Remarks = "出库单" + sysNo + "作废"
            //        };

            //    var whStockInItems = new List<WhStockInItem>();
            //    foreach (var item in stockOut.Items)
            //    {
            //        if (item.Status == WarehouseStatus.出库单明细状态.有效.GetHashCode())
            //        {
            //            var wh = new WhStockInItem
            //                {
            //                    CreatedBy = userSysNo,
            //                    CreatedDate = DateTime.Now,
            //                    ProductName = item.ProductName,
            //                    ProductSysNo = item.ProductSysNo,
            //                    StockInQuantity = item.ProductQuantity
            //                };
            //            whStockInItems.Add(wh);
            //        }
            //    }
            //    whStockIn.ItemList = whStockInItems;
            //    //创建入库单
            //    InStockBo.Instance.CreateStockIn(whStockIn);
            //}

            #endregion

            #region 发票信息

            if (stockOut.InvoiceSysNo > 0)
            {
                var invoice = InvoiceBo.Instance.GetModel(stockOut.InvoiceSysNo);
                if (invoice != null)
                {
                    invoice.Status = FinanceStatus.发票状态.待开票.GetHashCode();
                    invoice.InvoiceCode = invoice.InvoiceNo = string.Empty;
                    SoOrderBo.Instance.UpdateOrderInvoice(invoice);
                }
            }

            #endregion


            #region 回滚平台库存
            if (stockOut.Status != WarehouseStatus.出库单状态.待出库.GetHashCode())
            {
                //平台仓库库存
                var _result = WhWarehouseBo.Instance.ReduceStock(1, stockOut.WarehouseSysNo, stockOut.Items);
            }
            #endregion

            #region 更新出库
            stockOut.Remarks = tmp;
            stockOut.Status = (int)WarehouseStatus.出库单状态.作废;
            IOutStockDao.Instance.Update(stockOut); //应先更新出库单状态（朱成果)
            #endregion

            #region 更新订单
            UpdateSOStatus(stockOut.SysNo, userSysNo, text); //让后更新订单状态
            //写订单池记录
            SyJobPoolPublishBo.Instance.OrderWaitStockOut(stockOut.OrderSysNO, null, null, null, null, SyJobPoolPriorityBo.Instance.GetPriorityByCode(JobPriorityCode.PC040.ToString()));
            SyJobDispatcherBo.Instance.WriteJobLog(string.Format("作废出库单创建出库审核任务，销售单编号:{0}", stockOut.OrderSysNO), stockOut.OrderSysNO, null, userSysNo);
            #endregion



            resultObj.Status = true;
            resultObj.Message = string.Format("出库单{0}作废成功.", sysNo);


            SysLog.Instance.Info(LogStatus.系统日志来源.后台, "作废" + sysNo + "出库单", LogStatus.系统日志目标类型.出库单, sysNo, userSysNo);

            return resultObj;
        }

        /// <summary>
        ///     根据主键获取出库单
        /// </summary>
        /// <param name="outStockSysNo">出库单系统编号.</param>
        /// <returns>出库单实体</returns>
        /// <remarks>
        ///     2013/6/18 何方 创建
        /// </remarks>
        public WhStockOut Get(int outStockSysNo)
        {
            return IOutStockDao.Instance.GetModel(outStockSysNo);
        }
        /// <summary>
        /// 获取出库单详情
        /// </summary>
        /// <param name="stockOutSysno">出库单系统编号</param>
        /// <returns></returns>
        /// <remarks>2017-10-24 杨浩 创建</remarks>
        public WhStockOut GetSimpleInfo(int stockOutSysno)
        {
            return IOutStockDao.Instance.GetSimpleInfo(stockOutSysno);
        }
        /// <summary>
        /// 根据事务编号获取出库单
        /// </summary>
        /// <param name="transactionSysNo">事务编号</param>
        /// <returns>出库单实体</returns>
        /// <remarks>2013-07-29 周瑜 创建</remarks>
        public IList<WhStockOut> GetModelByTransactionSysNo(string transactionSysNo)
        {
            return IOutStockDao.Instance.GetModelByTransactionSysNo(transactionSysNo);
        }
        #region 销售单创建出库单相关

        /// <summary>
        ///     根据订单编号获取出库单列表
        /// </summary>
        /// <param name="orderId">订单编号</param>
        /// <returns>出库单列表</returns>
        /// <param name="onlyComplate">只读完成的定点</param>
        /// <remarks>2013-06-25 朱成果 创建</remarks>
        public IList<WhStockOut> GetWhStockOutListByOrderID(int orderId, bool onlyComplate = false)
        {
            return IOutStockDao.Instance.GetWhStockOutListByOrderID(orderId, onlyComplate);
        }

        #endregion

        /// <summary>
        /// 更新订单所有出库单状态
        /// </summary>
        /// <param name="orderSysNo">订单系统编号.</param>
        /// <param name="status">出库单状态.</param>
        /// <param name="operatorSysNo">The user sys no.</param>
        /// <returns></returns>
        /// <remarks>
        /// 2013/7/11 何方 创建
        /// </remarks>
        public void UpdateOrderStockOutStatus(int orderSysNo, WarehouseStatus.出库单状态 status, int operatorSysNo)
        {
            SysLog.Instance.Info(LogStatus.系统日志来源.后台, "更新订单所有出库单状态为 " + status, LogStatus.系统日志目标类型.出库单, orderSysNo, (Authentication.AdminAuthenticationBo.Instance.Current == null ? 12 : Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo));
            var stockOutList = GetWhStockOutListByOrderID(orderSysNo);
            foreach (var item in stockOutList)
            {
                UpdateStockOutStatus(item.SysNo, status, operatorSysNo);
            }

        }

        /// <summary>
        /// 更新出库单状态
        /// </summary>
        /// <param name="stockOutSysNo">出库单系统编号.</param>
        /// <param name="status">出库单状态.</param>
        /// <param name="operatorSysNo">The user sys no.</param>
        /// <remarks>
        /// 2013/7/11 何方 创建
        /// </remarks>
        public void UpdateStockOutStatus(int stockOutSysNo, WarehouseStatus.出库单状态 status, int operatorSysNo)
        {
            var stockOut = Get(stockOutSysNo);

            if (stockOut == null)
            {
                throw new Exception("不存在系统编号为" + stockOutSysNo + "的出库单");
            }

            stockOut.Status = (int)status;//设置状态
            stockOut.LastUpdateBy = operatorSysNo;
            stockOut.LastUpdateDate = DateTime.Now;

            /*签收时间 黄伟
            */
            if (status == WarehouseStatus.出库单状态.已签收 || status == WarehouseStatus.出库单状态.部分退货 || status == WarehouseStatus.出库单状态.拒收)
            {
                stockOut.SignTime = stockOut.LastUpdateDate;
            }

            switch (status)
            {
                case WarehouseStatus.出库单状态.配送中:
                    break;
                case WarehouseStatus.出库单状态.拒收:

                    //分批出库/配送, 如果是是第一张出库单拒收

                    //if (stockOut.Receivable>0)
                    //{
                    //    //SoOrderBo.Instance.DeclinedOrder(stockOut.OrderSysNO, SyUserBo.Instance.GetSyUser(operatorSysNo));
                    //}
                    //else
                    //{
                    //    //否则
                    //    
                    //}

                    break;
                case WarehouseStatus.出库单状态.已签收:
                    SyUser syUser = SyUserBo.Instance.GetSyUser(operatorSysNo);
                    SoOrderBo.Instance.UpdateSoStatusForSotckOutSign(stockOut, syUser == null ? new SyUser() : syUser);

                    #region 拆单出库,第一单签收以后其他出库单应收金额修改为0;

                    if (
                        GetWhStockOutListByOrderID(stockOut.OrderSysNO).All(x => x.Status != WarehouseStatus.出库单状态.已签收.GetHashCode()))
                    {
                        UpdateOtherStockOut(stockOut);
                    }

                    #endregion

                    //出库单签收--签收出库单的时候需要改订单OnlineStatus状态为“已签收”或者“部分签收” --huangwei 11-08
                    SoOrderBo.Instance.UpdateOnlineStatusByOrderID(stockOut.OrderSysNO, "已签收");

                    break;
                case WarehouseStatus.出库单状态.作废:
                    //需要调用订单的接口通知订单处理
                    SoOrderBo.Instance.UpdateSoStatusForSotckOutCancel(stockOutSysNo, SyUserBo.Instance.GetSyUser(operatorSysNo));
                    break;
            }

            UpdateStockOut(stockOut);
            //Update(Warehouse);
            if (status != WarehouseStatus.出库单状态.拒收)
            {
                SyUser syUser = SyUserBo.Instance.GetSyUser(operatorSysNo);
                SoOrderBo.Instance.SynchronousOrderStatus(stockOut.OrderSysNO, syUser == null ? new SyUser() : syUser);
            }


            //记录日志
            Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台,
                                     string.Format("将出库单{0}状态修改为{1}", stockOut.SysNo, status), LogStatus.系统日志目标类型.出库单, stockOut.SysNo, null, "", operatorSysNo);
        }

        /// <summary>
        /// 根据系统编号集合获取出库单列表
        /// </summary>
        /// <param name="sysNos">出库单系统编号数组</param>
        /// <returns>出库单列表</returns>
        /// <remarks>2013-07-04 沈强 创建</remarks>
        public IList<WhStockOut> GetWhStockOutListBySysNos(int[] sysNos)
        {
            return IOutStockDao.Instance.GetWhStockOutListBySysNos(sysNos);
        }

        /// <summary>
        /// 根据系统编号集合获取出库单及收货人列表
        /// </summary>
        /// <param name="sysNos">出库单系统编号集合</param>
        /// <returns>出库单及收货人列表</returns>
        /// <remarks>2013-07-09 黄伟 创建</remarks>
        public IList<CBWhStockOut> GetWhStockOutList(int[] sysNos)
        {
            return IOutStockDao.Instance.GetWhStockOutList(sysNos);
        }

        /// <summary>
        /// 根据出库单系统编号集合获取出库单明细列表
        /// </summary>
        /// <param name="sysNo">出库单系统编号</param>
        /// <returns>出库单明细列表</returns>
        /// <remarks>2013-07-09 黄伟 创建</remarks>
        public IList<WhStockOutItem> GetWhStockOutItemList(int sysNo)
        {
            return IOutStockDao.Instance.GetWhStockOutItemList(sysNo);
        }

        public static IList<WhStockOutItem> GetWhStockOutItemByOut(int sysNo)
        {
            return IOutStockDao.Instance.GetWhStockOutItemList(sysNo);
        }

        /// <summary>
        /// 获取出库单明细
        /// </summary>
        /// <param name="sysNo">出库单明细编号</param>
        /// <returns>出库单明细</returns>
        /// <remarks>2013-12-04 yangheyu 创建</remarks>
        public WhStockOutItem GetWhStockOutItem(int sysNo)
        {
            return IOutStockDao.Instance.GetWhStockOutItem(sysNo);
        }

        /// <summary>
        /// 根据销售单明细编号获取有效的出库单明细列表
        /// </summary>
        /// <param name="orderitemsysno">销售单明细编号</param>
        /// <returns>出库单明细列表</returns>
        /// <remarks>2013-11-22 吴文强 创建</remarks>
        public IList<WhStockOutItem> GetStockOutItems(int[] orderitemsysno)
        {
            return IOutStockDao.Instance.GetStockOutItems(orderitemsysno);
        }

        /// <summary>
        /// 根据出库单系统编号获取相应订单
        /// </summary>
        /// <param name="stockOutSysNo">出库单系统编号</param>
        /// <returns>订单实体</returns>
        /// <remarks>2013-07-11 黄伟 创建</remarks>
        public SoOrder GetSoOrder(int stockOutSysNo)
        {
            return IOutStockDao.Instance.GetSoOrder(stockOutSysNo);
        }

        /// <summary>
        /// 货到付款,拆单出库,第一单签收以后其他出库单应收金额修改为0
        /// </summary>
        /// <param name="stockOut">货到付款,拆单库出库,第一个配送的出库单.</param>
        /// <returns></returns>
        /// <remarks>
        /// 2013-08-28 何方 创建
        /// </remarks>
        private void UpdateOtherStockOut(WhStockOut stockOut)
        {
            if (stockOut.IsCOD == LogisticsStatus.是否到付.否.GetHashCode())
                return;
            //marked by huangwei 2013-11-21 bugid 2409 
            //if (stockOut.Receivable <= 0)
            //{
            //    throw new Exception(string.Format("货到付款,拆单出库,第一个配送的出库单:{0}应收金额不能为0", stockOut.SysNo));

            //}
            SysLog.Instance.Info(LogStatus.系统日志来源.后台, "货到付款,拆单出库", LogStatus.系统日志目标类型.出库单, stockOut.SysNo, (Authentication.AdminAuthenticationBo.Instance.Current == null ? 12 : Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo));

            var orderStockOutList = GetWhStockOutListByOrderID(stockOut.OrderSysNO);
            if (orderStockOutList.Count > 1)
            {

                var stockOutList = orderStockOutList.Where(
                    x => x.SysNo != stockOut.SysNo);//其他出库单
                var whStockOuts = stockOutList as WhStockOut[] ?? stockOutList.ToArray();
                if (whStockOuts.Any())
                {
                    foreach (var s in whStockOuts)
                    {
                        s.Receivable = 0;
                        IOutStockDao.Instance.Update(s);
                    }
                }

            }

        }

        /// <summary>
        /// 获取可退换列表
        /// </summary>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <returns></returns>
        /// <remarks>2013-09-12 沈强 创建</remarks>
        public IList<ReturnDetail> GetReturnsList(int customerSysNo)
        {
            var returnDetails =
                IOutStockDao.Instance.GetReturnDetailByCustomerSysNoAndStatus(customerSysNo,
                                                                              (int)WarehouseStatus.出库单状态.已签收);

            foreach (var returnDetail in returnDetails)
            {
                var productReturns = new List<ProductReturn>();

                var whStockOutItems = IOutStockDao.Instance.GetWhStockOutItemList(returnDetail.StockOutSysNo);
                foreach (var whStockOutItem in whStockOutItems)
                {
                    var productReturn = new ProductReturn
                    {
                        MaxReturnQuantity = whStockOutItem.ProductQuantity - whStockOutItem.ReturnQuantity,
                        Price = whStockOutItem.RealSalesAmount,
                        ProductName = whStockOutItem.ProductName,
                        ProductSysNo = whStockOutItem.ProductSysNo,
                        RmaQuantity = whStockOutItem.ReturnQuantity,
                        StockOutItemSysNo = whStockOutItem.SysNo,
                        Thumbnail = Web.ProductImageBo.Instance.GetProductImagePath(Web.ProductThumbnailType.Image120, whStockOutItem.ProductSysNo)
                    };

                    productReturns.Add(productReturn);
                }

                returnDetail.ProductReturns = productReturns;
            }

            return returnDetails;
        }

        /// <summary>
        /// 获取可退换商品
        /// </summary>
        /// <param name="stockOutSysNo">出库单编号</param>
        /// <returns>返回退货明细</returns>
        /// <remarks></remarks>
        public ReturnDetail GetReturnInfo(int stockOutSysNo)
        {
            var returnDetail =
                IOutStockDao.Instance.GetReturnDetailByStockOutSysNo(stockOutSysNo);

            var productReturns = new List<ProductReturn>();

            var whStockOutItems = IOutStockDao.Instance.GetWhStockOutItemList(returnDetail.StockOutSysNo);
            foreach (var whStockOutItem in whStockOutItems)
            {
                var productReturn = new ProductReturn
                {
                    MaxReturnQuantity = whStockOutItem.ProductQuantity - whStockOutItem.ReturnQuantity,
                    Price = whStockOutItem.RealSalesAmount,
                    ProductName = whStockOutItem.ProductName,
                    ProductSysNo = whStockOutItem.ProductSysNo,
                    //RmaQuantity = whStockOutItem.ReturnQuantity,
                    StockOutItemSysNo = whStockOutItem.SysNo,
                    Thumbnail = Web.ProductImageBo.Instance.GetProductImagePath(Web.ProductThumbnailType.Image120, whStockOutItem.ProductSysNo)
                };

                productReturns.Add(productReturn);
            }

            returnDetail.ProductReturns = productReturns.Where(x => x.MaxReturnQuantity > 0).ToList();

            return returnDetail;
        }

        /// <summary>
        /// 出库单查询
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页长度</param>
        /// <returns>返回出库单列表</returns>
        /// <remarks>2013-10-25 周瑜 创建</remarks>
        public PagedList<CBWhStockOut> Search(StockOutSearchCondition condition, int pageIndex, int pageSize)
        {
            if (condition == null)
            {
                return null;
            }

            var model = new PagedList<CBWhStockOut>();
            var result = IOutStockDao.Instance.Search(condition, pageIndex, pageSize);
            model.TData = result.Rows;
            model.PageSize = pageSize;
            model.TotalItemCount = result.TotalRows;
            model.CurrentPageIndex = pageIndex;
            return model;
        }

        #region 商品库存 2013-11-06 何方

        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="warehouseSysNo">The warehouse sys no.</param>
        /// <param name="productSysNos">The product sys nos.</param>
        /// <returns></returns>
        /// <remarks>
        /// 2013/11/6 何方 创建
        /// </remarks>
        public List<dynamic> GetGetInventory(int warehouseSysNo, int[] productSysNos)
        {
            var result = new List<dynamic> { };
            var productErpcode = new Dictionary<string, int> { };
            var oraganizationCode = "";
            //创建商品erpcode集合
            foreach (var productSysNo in productSysNos.Distinct())
            {
                productErpcode.Add(PdProductBo.Instance.GetProductErpCode(productSysNo), productSysNo);

            }
            //获取加盟商商品编号
            var warehouse = GetWarehouse(warehouseSysNo);
            if (warehouse == null)
            {
                throw new Exception(string.Format("编号为{0}的仓库不存在", warehouseSysNo));
            }
            var oraganization = Hyt.BLL.Basic.OrganizationBo.Instance.GetOrganization(warehouse.SysNo);
            if (oraganization != null)
                oraganizationCode = oraganization.Code;
            var inventory = KisProviderFactory.CreateProvider().GetInventory(oraganizationCode, productErpcode.Keys.ToArray(), warehouse.ErpCode, warehouse.SysNo);
            if (!inventory.Status)
            {
                throw new Exception(inventory.Message);
            }

            foreach (var i in inventory.Data)
            {

                result.Add(new { productSysNo = productErpcode[i.MaterialNumber], Quantity = i.Quantity });

            }

            return result;
        }

        #endregion

        /// <summary>
        /// 获取仓库by erpcode
        /// </summary>
        /// <param name="erpCode">erpCode</param>
        /// <returns>WhWarehouse</returns>
        /// <remarks>2013-11-13 huangwei 创建</remarks>
        public WhWarehouse GetWhWareHouseByErpCode(string erpCode)
        {
            return IWhWarehouseDao.Instance.GetWhWareHouseByErpCode(erpCode);

        }

        /// <summary>
        /// 查询订单销售单来源EAS(ERP)编号
        /// </summary>
        /// <param name="orderSysno">订单系统编号.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">
        /// </exception>
        /// <remarks>
        /// 2013-11-20 何方 创建
        /// </remarks>
        public string GetErpCustomerCode(int orderSysno)
        {
            var customerErpCode = Extra.Erp.Model.EasConstant.HytCustomer;
            var order = SoOrderBo.Instance.GetEntity(orderSysno);
            if (order == null)
            {
                throw new Exception(string.Format("找不到编号为:{0}的订单", orderSysno));
            }
            if (order.OrderSource == Model.WorkflowStatus.OrderStatus.销售单来源.分销商升舱.GetHashCode())
            {
                //var mall = DsEasBo.Instance.Get(order.OrderSourceSysNo);
                //if (mall == null)
                //{
                //    throw new Exception(string.Format("找不到系统编号为{0}的分销商商城", order.OrderSourceSysNo));
                //}
                //customerErpCode = mall.Code;
            }
            return customerErpCode;
        }

        #region 提货码及验证码分页查询
        /// <summary>
        /// 提货码及验证码分页查询
        /// </summary>
        /// <param name="pager">分页列表</param>
        /// <param name="filter">筛选条件</param>
        /// <returns></returns>
        /// <remarks>2013-12-3 余勇 创建</remarks>
        public void GetPickUpSmsList(ref Pager<CBWhPickUpCode> pager, ParaWhPickUpCodeFilter filter)
        {
            IWhPickUpCodeDao.Instance.GetPickUpSmsList(ref pager, filter);
        }
        #endregion

        #region 出库单批量出库相关
        /// <summary>
        /// 获取有权限仓库对应的所有的第三方快递
        /// </summary>
        /// <param name="user">用户信息</param>
        /// <returns></returns>
        /// <remarks>2014-07-10 朱成果 创建</remarks>
        public IList<LgDeliveryType> GetThirdPartyExpress(SyUser user)
        {
            IList<LgDeliveryType> lst = lst = Hyt.BLL.Logistics.DeliveryTypeBo.Instance.GetSubLgDeliveryTypeList(Hyt.Model.SystemPredefined.DeliveryType.第三方快递);//所有第三方快递
            var hasAllWarehouse = SyUserGroupBo.Instance.IsHasAllWarehouse(user.SysNo);
            if (!hasAllWarehouse)
            {
                var wlst = GetUserWarehouseList(user.SysNo);//所有用户有权限管理的仓库
                if (wlst != null)
                {
                    var ids = GetWhWarehouseDeliveryTypeList().Where(m => wlst.Any(y => y.SysNo == m.WarehouseSysNo)).Select(x => x.DeliveryTypeSysNo).ToList();//
                    lst = lst.Where(m => ids.Contains(m.SysNo)).ToList();
                }
            }
            return lst;
        }

        /// <summary>
        /// 查询第三方配送的出库单(不开票,待出库)
        /// </summary>
        /// <param name="stockOutStatus">出库单状态</param>
        /// <param name="userSysNo">用户编号</param>
        /// <param name="isHasAllWarehouse">是否拥有所有仓库的权限</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页面大小</param>
        /// <returns></returns>
        /// <remarks>2014-07-10 朱成果 创建</remarks>
        public PagedList<CBWhStockOut> SearchThirdPartyStockOut(int stockOutStatus, int userSysNo, int pageIndex, int pageSize, int orderSysNo, int warehouseSysNo, string sort, string sortBy)
        {
            var model = new PagedList<CBWhStockOut>();
            var hasAllWarehouse = SyUserGroupBo.Instance.IsHasAllWarehouse(userSysNo);
            var result = Hyt.DataAccess.Warehouse.IWhWarehouseDao.Instance.SearchThirdPartyStockOut(stockOutStatus, userSysNo, hasAllWarehouse, pageIndex, pageSize, orderSysNo, warehouseSysNo, sort, sortBy);
            model.TData = result.Rows;
            model.PageSize = pageSize;
            model.TotalItemCount = result.TotalRows;
            model.CurrentPageIndex = pageIndex;
            return model;
        }


        /// <summary>
        /// 查询当日达配送的出库单(不开票,待出库)
        /// </summary>
        /// <param name="stockOutStatus">出库单状态</param>
        /// <param name="userSysNo">用户编号</param>
        /// <param name="isHasAllWarehouse">是否拥有所有仓库的权限</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页面大小</param>
        /// <returns></returns>
        /// <remarks>2014-07-10 朱成果 创建</remarks>
        public PagedList<CBWhStockOut> SearchDRDStockOut(int stockOutStatus, int userSysNo, int pageIndex, int pageSize)
        {
            var model = new PagedList<CBWhStockOut>();
            var hasAllWarehouse = SyUserGroupBo.Instance.IsHasAllWarehouse(userSysNo);
            var result = Hyt.DataAccess.Warehouse.IWhWarehouseDao.Instance.SearchDRDStockOut(stockOutStatus, userSysNo, hasAllWarehouse, pageIndex, pageSize);
            model.TData = result.Rows;
            model.PageSize = pageSize;
            model.TotalItemCount = result.TotalRows;
            model.CurrentPageIndex = pageIndex;
            return model;
        }
        #endregion

        #region 检查仓库是否存在

        /// <summary>
        /// 检查仓库名称是否存在
        /// </summary>
        /// <param name="condition">搜索条件实体</param>
        /// <returns></returns>
        /// <remarks>
        /// 2015-8-8 陈海裕 创建
        /// </remarks>
        public bool CheckWarehouseName(WarehouseSearchCondition condition)
        {
            if (condition == null)
            {
                return false;
            }

            return IWhWarehouseDao.Instance.CheckWarehouseName(condition);
        }

        #endregion


        /// <summary>
        /// 外部接口调用，自动出库
        /// </summary>
        /// <param name="model"></param>
        /// <param name="user"></param>
        /// 王耀发 2015-09-22 创建
        public void OutStockApi(WhStockOut model, SyUser user)
        {
            //用于保存分批出库时，创建的出库单
            WhStockOut newStockOut = null;

            //如果出库单中的商品明细与已扫描的商品明细以及数量不匹配
            //那么就要进行分批出库
            //得到未扫描的以及扫描数量不足的商品列表

            //var deliveryType = DeliveryTypeBo.Instance.GetDeliveryType(model.DeliveryTypeSysNo);
            var order = SoOrderBo.Instance.GetEntity(model.OrderSysNO);
            #region 拆分出库单
            var items =
                model.Items.Where(item => !item.IsScaned || item.ProductQuantity != item.ScanedQuantity).ToList();
            if (items.Count > 0)
            {
                //得到数量不匹配的商品列表
                newStockOut = new WhStockOut
                {
                    TransactionSysNo = model.TransactionSysNo,
                    WarehouseSysNo = model.WarehouseSysNo,
                    OrderSysNO = model.OrderSysNO,
                    ReceiveAddressSysNo = model.ReceiveAddressSysNo,
                    IsCOD = model.IsCOD,

                    //经吴文强确认所有出库单的应收金额都为订单里总金额现金支付部分
                    Receivable = model.Receivable == 0 ? 0 : order.CashPay,
                    //SignTime = DateTime.Now.AddDays(1),//新建出库单， 无签收日期
                    IsPrintedPackageCover = WarehouseStatus.是否已经打印包裹单.否.GetHashCode(),
                    IsPrintedPickupCover = WarehouseStatus.是否已经打印拣货单.否.GetHashCode(),
                    CustomerMessage = model.CustomerMessage,
                    Remarks = "创建新出库单,来源:拆分出库单：" + model.SysNo,
                    Status = (int)WarehouseStatus.出库单状态.待出库,
                    DeliveryRemarks = string.IsNullOrEmpty(model.DeliveryRemarks) ? "" : model.DeliveryRemarks,
                    DeliveryTime = string.IsNullOrEmpty(model.DeliveryTime) ? "" : model.DeliveryTime,
                    DeliveryTypeSysNo = model.DeliveryTypeSysNo,
                    ContactBeforeDelivery = model.ContactBeforeDelivery,
                    //发票由第一张出库单开具, 其余分批出库单不需要再开具发票
                    InvoiceSysNo = 0,
                    CreatedDate = DateTime.Now,
                    CreatedBy = model.LastUpdateBy,
                    Items = new List<WhStockOutItem> { }
                };

                foreach (var item in items)
                {
                    var newItem = IOutStockDao.Instance.GetStockOutItem(item.SysNo);
                    //设置金额为未出库的金额
                    newItem.RealSalesAmount = item.RealSalesAmount - Math.Round(item.RealSalesAmount * (item.ScanedQuantity / (decimal)item.ProductQuantity), 2);
                    //对于新建的出库单, 将商品数量置为未扫描数量
                    newItem.ProductQuantity = item.ProductQuantity - item.ScanedQuantity;
                    newStockOut.Items.Add(newItem);
                }
                newStockOut.StockOutAmount = items.Sum(x => x.RealSalesAmount);
                //拆分出库,逻辑删除未出库的商品,添加到新的出库单中
                RemoveItem(items.Where(m => m.IsScaned == false).ToList());
                newStockOut.SysNo = IOutStockDao.Instance.Insert(newStockOut);

                SysLog.Instance.Info(LogStatus.系统日志来源.后台, "创建新出库单,来源:拆分出库单：" + model.SysNo, LogStatus.系统日志目标类型.出库单, newStockOut.SysNo, user.SysNo);

                SoOrderBo.Instance.WriteSoTransactionLog(newStockOut.TransactionSysNo, "拆分出库,创建新出库单单号：" + newStockOut.SysNo, user.UserName);
            }
            #endregion

            if (newStockOut == null)
            {
                SysLog.Instance.Info(LogStatus.系统日志来源.后台, "出库", LogStatus.系统日志目标类型.出库单, model.SysNo, user.SysNo);
            }
            else
            {
                SysLog.Instance.Info(LogStatus.系统日志来源.后台, "分批出库，拆分后的出库单号为：" + newStockOut.SysNo, LogStatus.系统日志目标类型.出库单, model.SysNo, user.SysNo);
                model.Remarks = "分批出库，拆分后的出库单号为：" + newStockOut.SysNo;
            }
            #region 修改原出库单中的商品数量
            //本次出库的出库单明细
            var stockOutitems = model.Items.Where(item => item.IsScaned && item.ScanedQuantity > 0).ToList();
            foreach (var item in stockOutitems)
            {
                //设置金额为实际出库单金额
                item.RealSalesAmount = Math.Round(item.RealSalesAmount * (item.ScanedQuantity / (decimal)item.ProductQuantity), 2);
                item.ProductQuantity = item.ScanedQuantity;
                item.LastUpdateBy = model.LastUpdateBy;
                item.LastUpdateDate = DateTime.Now;

            }
            IOutStockDao.Instance.UpdateItems(stockOutitems);
            #endregion

            model.Status = WarehouseStatus.出库单状态.待配送.GetHashCode();
            model.Receivable = model.Receivable == 0 ? 0 : order.CashPay;
            model.StockOutAmount = stockOutitems.Sum(m => m.RealSalesAmount);
            UpdateStockOut(model);
            //结束事务
        }

        #region 商品导出

        /// <summary>
        /// 查询导出商品库存列表
        /// </summary>
        /// <param name="sysNos"></param>
        /// <returns></returns>
        /// <remarks>2015-12-30 王耀发 创建</remarks>
        public void ExportProductStocks(string warehouseSysNo, List<int> productStockSysNos, string userIp, int operatorSysno)
        {
            try
            {
                // 查询商品
                List<CBOutputPdProductStocks> exportProducts = BLL.Warehouse.WhWarehouseBo.Instance.GetExportProductStockList(warehouseSysNo, productStockSysNos);

                var fileName = string.Format("商品({0})", DateTime.Now.ToString("yyyyMMddHHmmss"));

                /*
                 * 仓库名称
                 * 商品编码
                 * 后台显示名称
                 * 条形码
                 * 海关备案号
                 * 采购价格
                 * 库存数量
                 */

                //导出Excel，并设置表头列名
                Util.ExcelUtil.Export<CBOutputPdProductStocks>(exportProducts,
                    new List<string> { "仓库名称", "商品编码", "后台显示名称", "条形码", "海关备案号", "采购价格", "库存数量", "会员价格" },
                    fileName);

                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Info, LogStatus.系统日志来源.后台, "商品导出excel",
                                         LogStatus.系统日志目标类型.ExcelExporting, 0, null, userIp, operatorSysno);
            }
            catch (Exception ex)
            {

                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "商品导出excel",
                                         LogStatus.系统日志目标类型.ExcelExporting, 0, ex, userIp, operatorSysno);

            }
        }

        /// <summary>
        /// 查询导出商品库存列表
        /// </summary>
        /// <param name="sysNos"></param>
        /// <returns></returns>
        /// <remarks>2015-12-30 王耀发 创建</remarks>
        public void ExportProductStocks(int warehouseSysNo, IList<CBPdProductStockList> productStockSysNos, string userIp, int operatorSysno)
        {
            try
            {
                IList<PdCategory> CategoryList = Hyt.BLL.Product.PdCategoryBo.Instance.GetAllCategory();
                WhWarehouse warehouseMod = WhWarehouseBo.Instance.GetWarehouseEntity(warehouseSysNo);
                // 查询商品
                List<CBOutputPdProductStocks> exportProducts = new List<CBOutputPdProductStocks>();
                foreach (CBPdProductStockList productStock in productStockSysNos)
                {
                    if (warehouseMod.BackWarehouseName == productStock.BackWarehouseName)
                    {
                        string category1 = "";
                        string category2 = "";
                        string category3 = "";

                        string[] categoryList = productStock.PdCategorySysNos.Split(',');
                        for (int i = 1; i < categoryList.Length - 1; i++)
                        {
                            string value = "";
                            if (!string.IsNullOrEmpty(categoryList[i].Trim()))
                            {
                                value = CategoryList.First(p => p.SysNo == Convert.ToInt32(categoryList[i])).CategoryName;
                            }
                            if (i == 1)
                            {
                                category1 = value;
                            }
                            else if (i == 2)
                            {
                                category2 = value;
                            }
                            else if (i == 3)
                            {
                                category3 = value;
                            }
                        }
                        CBOutputPdProductStocks mod = new CBOutputPdProductStocks()
                        {
                            采购价格 = productStock.CostPrice,
                            仓库名称 = productStock.BackWarehouseName,
                            商品编码 = productStock.ErpCode,
                            后台显示名称 = productStock.EasName,
                            分类内容1 = category1,
                            分类内容2 = category2,
                            分类内容3 = category3,
                            条形码 = productStock.Barcode,
                            库存数量 = productStock.StockQuantity,
                            会员价格 = productStock.Price,
                            异议库存 = productStock.DetailStockQuantity
                        };
                        exportProducts.Add(mod);
                    }

                }

                var fileName = string.Format("商品({0})", DateTime.Now.ToString("yyyyMMddHHmmss"));

                /*
                 * 仓库名称
                 * 商品编码
                 * 后台显示名称
                 * 条形码
                 * 海关备案号
                 * 采购价格
                 * 库存数量
                 */

                //导出Excel，并设置表头列名
                Util.ExcelUtil.Export<CBOutputPdProductStocks>(exportProducts,
                    new List<string> { "仓库名称", "商品编码", "后台显示名称", "一级分类", "二级分类", "三级分类", "条形码", "海关备案号", "采购价格", "库存数量", "会员价格", "异议库存" },
                    fileName);

                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Info, LogStatus.系统日志来源.后台, "商品导出excel",
                                         LogStatus.系统日志目标类型.ExcelExporting, 0, null, userIp, operatorSysno);
            }
            catch (Exception ex)
            {

                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "商品导出excel",
                                         LogStatus.系统日志目标类型.ExcelExporting, 0, ex, userIp, operatorSysno);

            }
        }
        /// <summary>
        /// 查询导出商品库存列表 2仓库数据合并计算
        /// </summary>
        /// <param name="sysNos"></param>
        /// <returns></returns>
        /// <remarks>2017-1-10 杨云奕 创建</remarks>
        public void ExportProductStocks2(int warehouseSysNo1, IList<CBPdProductStockList> productStockSysNos1, int warehouseSysNo2, IList<CBPdProductStockList> productStockSysNos2, string userIp, int operatorSysno)
        {
            try
            {
                IList<PdCategory> CategoryList = Hyt.BLL.Product.PdCategoryBo.Instance.GetAllCategory();
                // 查询商品
                List<CBOutputPdProductStocks2> exportProducts = new List<CBOutputPdProductStocks2>();

                WhWarehouse warehouseMod = WhWarehouseBo.Instance.GetWarehouseEntity(warehouseSysNo1);
                foreach (CBPdProductStockList productStock in productStockSysNos1)
                {
                    if (warehouseMod.BackWarehouseName == productStock.BackWarehouseName)
                    {
                        string category1 = "";
                        string category2 = "";
                        string category3 = "";

                        string[] categoryList = productStock.PdCategorySysNos.Split(',');
                        for (int i = 1; i < categoryList.Length - 1; i++)
                        {
                            string value = "";
                            if (!string.IsNullOrEmpty(categoryList[i].Trim()))
                            {
                                value = CategoryList.First(p => p.SysNo == Convert.ToInt32(categoryList[i])).CategoryName;
                            }
                            if (i == 1)
                            {
                                category1 = value;
                            }
                            else if (i == 2)
                            {
                                category2 = value;
                            }
                            else if (i == 3)
                            {
                                category3 = value;
                            }
                        }
                        var mod = new CBOutputPdProductStocks2()
                        {
                            商品编码 = productStock.ErpCode,
                            后台显示名称 = productStock.EasName,
                            分类内容1 = category1,
                            分类内容2 = category2,
                            分类内容3 = category3,
                            条形码 = productStock.Barcode,
                            仓库1 = productStock.StockQuantity,
                            仓库2 = 0,
                            库存数量 = productStock.StockQuantity,
                            会员价格 = productStock.Price,

                        };
                        exportProducts.Add(mod);
                    }

                }
                WhWarehouse warehouseMod2 = WhWarehouseBo.Instance.GetWarehouseEntity(warehouseSysNo2);
                ////销售单列表
                List<CBSoOrder> orderList = Hyt.BLL.Order.SoOrderBo.Instance.GetAllOrderByDateTime(new DateTime(2017, 1, 1, 0, 0, 0), new DateTime(2116, 12, 09, 0, 0, 0));
                ///库存变动明细所有集合
                List<Model.Generated.WhWarehouseChangeLog> logList = WhWarehouseChangeLogBo.Instance.WarehouseLogList(warehouseSysNo2);
                foreach (var mod in orderList)
                {
                    if (mod.PayStatus == 20 && mod.Status > 0)
                    {
                        IList<SoOrderItem> detailList = Hyt.BLL.Order.SoOrderBo.Instance.GetOrderItemsByOrderId(mod.SysNo);
                        foreach (var detailMod in detailList)
                        {
                            WhWarehouseChangeLog logMod = logList.Find(p => p.BusinessTypes == "网络订单出库" && p.ProSysNo.ToString() == detailMod.ProductSysNo.ToString() && p.LogData.IndexOf(mod.OrderNo) != -1);
                            if (logMod == null)
                            {
                                Hyt.Model.Generated.WhWarehouseChangeLog log1 = new Hyt.Model.Generated.WhWarehouseChangeLog()
                                {
                                    WareSysNo = mod.DefaultWarehouseSysNo,
                                    ProSysNo = detailMod.ProductSysNo,
                                    ChageDate = mod.CreateDate,
                                    CreateDate = DateTime.Now,
                                    ChangeQuantity = Convert.ToInt32(detailMod.Quantity) * -1,
                                    BusinessTypes = "网络订单出库",
                                    LogData = "出库单号：" + mod.OrderNo
                                };
                                WhWarehouseChangeLogBo.Instance.CreateMod(log1);
                                logList.Add(log1);
                            }
                        }
                    }
                }


                foreach (var mod1 in productStockSysNos2)
                {
                    List<Model.Generated.WhWarehouseChangeLog> tempLogList = logList.Where(p => p.ProSysNo == mod1.PdProductSysNo).ToList();
                    int total = 0;
                    string txt = "";
                    int indx = 1;
                    foreach (var mod in tempLogList)
                    {
                        if (mod.BusinessTypes.IndexOf("入库") != -1 || mod.BusinessTypes.IndexOf("出库") != -1)
                        {
                            total += mod.ChangeQuantity;
                            mod.Quantity = total;
                        }
                        else if (mod.BusinessTypes.IndexOf("盘点单") != -1 || mod.BusinessTypes.IndexOf("手动修改") != -1 || mod.BusinessTypes.IndexOf("导EXCEL库存") != -1)
                        {
                            total = mod.ChangeQuantity;
                            mod.Quantity = total;
                        }

                        txt += "\n" + indx + "、" + mod.BusinessTypes + "  " + mod.LogData + "  数量：" + mod.ChangeQuantity + "  库存数：" + total;
                        indx++;
                    }

                    mod1.DetailStockQuantity = total.ToString();
                    if (total == mod1.StockQuantity)
                    {
                        mod1.DetailStockQuantity = "库存和记录一致";
                    }
                    else
                    {


                        mod1.DetailStockQuantity += " \n明细" + txt;

                    }
                }


                foreach (CBPdProductStockList productStock in productStockSysNos2)
                {
                    if (warehouseMod2.BackWarehouseName == productStock.BackWarehouseName)
                    {
                        string category1 = "";
                        string category2 = "";
                        string category3 = "";

                        string[] categoryList = productStock.PdCategorySysNos.Split(',');
                        for (int i = 1; i < categoryList.Length - 1; i++)
                        {
                            string value = "";
                            if (!string.IsNullOrEmpty(categoryList[i].Trim()))
                            {
                                value = CategoryList.First(p => p.SysNo == Convert.ToInt32(categoryList[i])).CategoryName;
                            }
                            if (i == 1)
                            {
                                category1 = value;
                            }
                            else if (i == 2)
                            {
                                category2 = value;
                            }
                            else if (i == 3)
                            {
                                category3 = value;
                            }
                        }

                        CBOutputPdProductStocks2 stick2 = exportProducts.Find(p => p.商品编码 == productStock.ErpCode);
                        if (stick2 == null)
                        {
                            var mod = new CBOutputPdProductStocks2()
                            {
                                商品编码 = productStock.ErpCode,
                                后台显示名称 = productStock.EasName,
                                分类内容1 = category1,
                                分类内容2 = category2,
                                分类内容3 = category3,
                                条形码 = productStock.Barcode,
                                仓库1 = 0,
                                仓库2 = productStock.StockQuantity,
                                库存异议 = productStock.DetailStockQuantity,
                                库存数量 = productStock.StockQuantity,
                                会员价格 = productStock.Price,

                            };
                            exportProducts.Add(mod);
                        }
                        else
                        {
                            stick2.库存异议 = productStock.DetailStockQuantity;
                            stick2.仓库2 = productStock.StockQuantity;
                            stick2.库存数量 = stick2.仓库2 + stick2.仓库1;
                        }
                    }
                }

                var fileName = string.Format("商品({0})", DateTime.Now.ToString("yyyyMMddHHmmss"));

                /*
                 * 仓库名称
                 * 商品编码
                 * 后台显示名称
                 * 条形码
                 * 海关备案号
                 * 采购价格
                 * 库存数量
                 */

                //导出Excel，并设置表头列名
                Util.ExcelUtil.Export<CBOutputPdProductStocks2>(exportProducts,
                    new List<string> { "商品编码", "后台显示名称", "一级分类", "二级分类", "三级分类", "条形码", "海关备案号", "采购价格", "会员价格", warehouseMod.BackWarehouseName, warehouseMod2.BackWarehouseName, warehouseMod2.BackWarehouseName + "库存异议", "总库存数" },
                    fileName);

                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Info, LogStatus.系统日志来源.后台, "商品导出excel",
                                         LogStatus.系统日志目标类型.ExcelExporting, 0, null, userIp, operatorSysno);
            }
            catch (Exception ex)
            {

                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "商品导出excel",
                                         LogStatus.系统日志目标类型.ExcelExporting, 0, ex, userIp, operatorSysno);

            }
        }
        /// <summary>
        /// 查询导出商品库存列表
        /// </summary>
        /// <param name="sysNos"></param>
        /// <returns></returns>
        /// <remarks>2015-12-30 王耀发 创建</remarks>
        public void ExportProductStocks(string warehouseSysNo, string userIp, int operatorSysno, bool bAlarm = false)
        {
            try
            {
                // 查询商品
                List<CBOutputPdProductAlarmStocks> exportProducts = BLL.Warehouse.WhWarehouseBo.Instance.GetExportProductStockList(warehouseSysNo, bAlarm);

                var fileName = string.Format("商品({0})", DateTime.Now.ToString("yyyyMMddHHmmss"));

                /*
                 * 仓库名称
                 * 商品编码
                 * 后台显示名称
                 * 条形码
                 * 海关备案号
                 * 采购价格
                 * 库存数量
                 */

                //导出Excel，并设置表头列名
                Util.ExcelUtil.Export<CBOutputPdProductAlarmStocks>(exportProducts,
                    new List<string> { "商品编码", "商品名称", "仓库名称", "库存数量", "上限", "下限" },
                    fileName);

                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Info, LogStatus.系统日志来源.后台, "商品导出excel",
                                         LogStatus.系统日志目标类型.ExcelExporting, 0, null, userIp, operatorSysno);
            }
            catch (Exception ex)
            {

                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "商品导出excel",
                                         LogStatus.系统日志目标类型.ExcelExporting, 0, ex, userIp, operatorSysno);

            }
        }

        /// <summary>
        /// 查询导出商品库存列表
        /// </summary>
        /// <param name="sysNos"></param>
        /// <returns></returns>
        /// <remarks>2015-12-30 王耀发 创建</remarks>
        public void ExportProductStocksYS(string warehouseSysNo, List<int> productStockSysNos, string userIp, int operatorSysno)
        {
            try
            {
                // 查询商品
                List<CBOutputPdProductStocksYS> exportProducts = BLL.Warehouse.WhWarehouseBo.Instance.GetExportProductStockListYS(warehouseSysNo, productStockSysNos);

                var fileName = string.Format("商品({0})", DateTime.Now.ToString("yyyyMMddHHmmss"));

                /*
                 * 仓库名称
                 * 商品编码
                 * 后台显示名称
                 * 条形码
                 * SKU
                 * 海关备案号
                 * 采购价格
                 * 库存数量
                 */

                //导出Excel，并设置表头列名
                Util.ExcelUtil.Export<CBOutputPdProductStocksYS>(exportProducts,
                    new List<string> { "仓库名称", "商品编码", "后台显示名称", "条形码", "商品SKU", "海关备案号", "采购价格", "库存数量" },
                    fileName);

                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Info, LogStatus.系统日志来源.后台, "商品导出excel",
                                         LogStatus.系统日志目标类型.ExcelExporting, 0, null, userIp, operatorSysno);
            }
            catch (Exception ex)
            {

                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "商品导出excel",
                                         LogStatus.系统日志目标类型.ExcelExporting, 0, ex, userIp, operatorSysno);

            }
        }
        /// <summary>
        /// 查询导出商品库存列表
        /// </summary>
        /// <param name="sysNos"></param>
        /// <returns></returns>
        /// <remarks>2015-12-30 王耀发 创建</remarks>
        public List<CBOutputPdProductStocks> GetExportProductStockList(string warehouseSysNo, List<int> sysNos)
        {
            return DataAccess.Warehouse.IPdProductStockDao.Instance.GetExportProductStockList(warehouseSysNo, sysNos);
        }
        /// <summary>
        /// 导出商品列表
        /// </summary>
        /// <param name="warehouseSysNo"></param>
        /// <returns></returns>
        public List<CBOutputPdProductAlarmStocks> GetExportProductStockList(string warehouseSysNo, bool bAlarm = false)
        {
            return DataAccess.Warehouse.IPdProductStockDao.Instance.GetExportProductStockList(warehouseSysNo, bAlarm);
        }
        public List<CBOutputPdProductStocksYS> GetExportProductStockListYS(string warehouseSysNo, List<int> sysNos)
        {
            return DataAccess.Warehouse.IPdProductStockDao.Instance.GetExportProductStockListYS(warehouseSysNo, sysNos);
        }
        /// <summary>
        /// 获取对应仓库列表
        /// 王耀发 2016-1-23 创建
        /// </summary>
        /// <returns></returns>
        public List<WhWarehouse> GetWhWareHouseList()
        {
            return IWhWarehouseDao.Instance.GetWhWareHouseList();
        }

        /// <summary>
        /// 是否关联过仓库
        /// </summary>
        /// <param name="DealerSysNo"></param>
        /// <returns></returns>
        public int ExitWarehouse(int DealerSysNo)
        {
            return IWhWarehouseDao.Instance.ExitWarehouse(DealerSysNo);
        }
        #endregion

        #region 库存提醒导入
        /// <summary>
        /// import cols mapping between DB and excel
        /// </summary>
        /// <remarks>2015-09-10 王耀发 创建</remarks>
        private static readonly Dictionary<string, string> DicColsMapping = new Dictionary<string, string>
            {
                {"ErpCode", "商品编码"},
                {"ProductName", "商品名称"},
                {"WarehouseName", "仓库名称"},
                {"Upperlimit", "上限"},
                {"Lowerlimit","下限"}
            };
        public Model.Result ImportAlarmExcel(System.IO.Stream stream, int operatorSysno)
        {
            var result = new Model.Result();

            DataTable dt = null;

            var cols = DicColsMapping.Select(p => p.Value).ToArray();
            var existlst = new List<WhInventoryAlarm>();
            try
            {
                dt = ExcelUtil.ImportExcel(stream, cols);
            }
            catch (Exception ex)
            {
                //exception happened,some not caughted
                result.Message = string.Format("数据导入错误,请选择正确的excel文件");
                result.Status = false;
                return result;
            }
            if (dt == null)
            {
                //not all the cols mapped             
                result.Message = string.Format("请选择正确的excel文件!");
                result.Status = false;
                return result;
            }

            IList<WhWarehouse> allWarehouseList = WhWarehouseBo.Instance.GetAllWarehouseList();
            IList<PdProductIndex> allPdProductList = PdProductBo.Instance.GetAllProduct();
            IList<PdProductStock> allStockList = PdProductStockBo.Instance.GetAllStockList();
            List<WhInventoryAlarm> allAlarmList = WhInventoryAlarmBo.Instance.GetAllAlarm();
            string msg = "";
            for (var i = 0; i < dt.Rows.Count; i++)
            {
                int excelRow = i + 2;

                for (var j = 0; j < dt.Columns.Count; j++)
                {
                    if ((dt.Rows[i][j] == null || string.IsNullOrEmpty(dt.Rows[i][j].ToString().Trim())))
                    {
                        result.Message = string.Format("excel表第{0}行第{1}列数据不能有空值", excelRow, (j + 1));
                        result.Status = false;
                        return result;
                    }
                }
                //商品编号
                var ErpCode = dt.Rows[i][DicColsMapping["ErpCode"]].ToString().Trim();
                //条形码
                var ProductName = dt.Rows[i][DicColsMapping["ProductName"]].ToString().Trim();
                //条形码
                var WarehouseName = dt.Rows[i][DicColsMapping["WarehouseName"]].ToString().Trim();
                //条形码
                var Upperlimit = dt.Rows[i][DicColsMapping["Upperlimit"]].ToString().Trim();
                //条形码
                var Lowerlimit = dt.Rows[i][DicColsMapping["Lowerlimit"]].ToString().Trim();

                List<WhWarehouse> warehouseList = allWarehouseList.Where(p => p.BackWarehouseName == WarehouseName).ToList();
                WhWarehouse warehouse;
                if (warehouseList.Count > 0)
                {
                    warehouse = warehouseList[0];
                }
                else
                {
                    result.Message = string.Format("不存在当前仓库{0}，请核实后再上传<br/>", WarehouseName);
                    result.Status = false;
                    return result;
                }
                List<PdProductIndex> productList = allPdProductList.Where(p => p.ErpCode == ErpCode).ToList();
                PdProductIndex pdproductIndex;
                if (productList.Count > 0)
                {
                    pdproductIndex = productList[0];
                }
                else
                {
                    msg += string.Format("无法在商品档案中查询到商品{0}，请核实后再上传<br/>", "(" + ErpCode + ")" + ProductName);
                    continue;
                }

                List<PdProductStock> stockList = allStockList.Where(p => p.WarehouseSysNo == warehouse.SysNo && p.PdProductSysNo == pdproductIndex.SysNo).ToList();
                if (stockList.Count > 0)
                {
                    WhInventoryAlarm alarmMod = allAlarmList.Find(p => p.ProductStockSysNo == stockList[0].SysNo);
                    WhInventoryAlarm alarm = new WhInventoryAlarm()
                    {
                        CreatedBy = 1,
                        CreatedDate = DateTime.Now,
                        LastUpdateBy = 1,
                        LastUpdateDate = DateTime.Now,
                        ProductStockSysNo = stockList[0].SysNo,
                        Lowerlimit = Convert.ToInt32(Lowerlimit),
                        Upperlimit = Convert.ToInt32(Upperlimit),
                        SysNo = (alarmMod == null ? 0 : alarmMod.SysNo)
                    };
                    if (alarm.SysNo == 0)
                    {
                        alarm.SysNo = WhInventoryAlarmBo.Instance.InserMod(alarm);
                        allAlarmList.Add(alarm);
                    }
                    else
                    {
                        WhInventoryAlarmBo.Instance.UpdateMod(alarm);
                    }
                }
                else
                {
                    msg += string.Format("仓库{0},无法查询到商品{1}，请核实后再上传<br/>", WarehouseName, "(" + ErpCode + ")" + ProductName);
                }
            }



            msg += "";
            if (string.IsNullOrEmpty(msg.Trim()))
            {
                msg = "导入成功！";
            }
            result.Message = msg;
            result.Status = true;
            return result;
        }
        #endregion

        #region 快递单号导入

        /// <summary>
        /// 导入快递单号
        /// </summary>
        /// <param name="stream">导入的excel stream format</param>
        /// <param name="operatorSysno">操作人员编号</param>
        /// <returns></returns>
        /// <remarks>2017-10-04 杨浩 创建</remarks>
        public Hyt.Model.Result ImportExpress(System.IO.Stream stream)
        {
            int operatorSysno = 0;

            var result = new Hyt.Model.Result<int>
            {
                Message = "成功导入",
                Status = true
            };

            #region execl数据填充

            string operatorName = SyUserBo.Instance.GetUserName(operatorSysno);

            System.Data.DataTable dt = null;
            var cols = new string[] { "订单编号", "快递单号" };
            try
            {
                string colsErr;
                dt = ExcelUtil.ImportExcel(stream, cols, out colsErr);
                if (!colsErr.IsNullOrEmpty())
                {
                    return new Hyt.Model.Result
                       {
                           Message = string.Format(colsErr),
                           Status = false
                       };
                }
            }
            catch (Exception ex)
            {
                //exception happened,some not caughted
                return new Hyt.Model.Result
                {
                    Message = string.Format("数据导入错误,请选择正确的excel文件，Exception:" + ex.Message),
                    Status = false
                };
            }

            if (dt == null)
            {
                //not all the cols mapped
                return new Hyt.Model.Result
                {
                    Message = string.Format("请选择正确的excel文件!"),
                    Status = false
                };
            }
            #endregion

            #region 处理数据

            for (var i = 0; i < dt.Rows.Count; i++)
            {

                string orderNo = dt.Rows[i]["订单编号"].ToString().Trim();
                if (string.IsNullOrWhiteSpace(orderNo))
                {
                    result.Message = string.Format("第" + (i + 1) + "行中的订单号不能为空！");
                    result.Status = false;
                    return result;
                }

                int outStockId = 0;
                if (orderNo.Length < 6 || !int.TryParse(orderNo.Substring(5), out outStockId))
                {
                    result.Message = string.Format("第" + (i + 1) + "行中的订单号格式不正确！");
                    result.Status = false;
                    return result;
                }

                string expressNo = dt.Rows[i]["快递单号"].ToString().Trim();
                if (string.IsNullOrWhiteSpace(expressNo))
                {
                    result.Message = string.Format("第" + (i + 1) + "行中的快递单号不能为空！");
                    result.Status = false;
                    return result;
                }
                else
                {
                    if (Regex.IsMatch(expressNo, @"[\u4e00-\u9fbb]"))//使用正则表达式匹配是否含有中文
                    {
                        result.Message = string.Format("第" + (i + 1) + "行中的快递单号不能包含中文！");
                        result.Status = false;
                        return result;
                    }
                }

                var outStockInfo = BLL.Warehouse.WhWarehouseBo.Instance.GetSimpleInfo(outStockId);
                if (outStockInfo == null)
                {
                    result.Message = string.Format("第" + (i + 1) + "行中的订单号【" + orderNo + "】在系统不存在！");
                    result.Status = false;
                    return result;
                }

                try
                {
                    result = BLL.Logistics.DeliveryTypeBo.Instance.BatchCreateLgDelivery(outStockInfo.WarehouseSysNo, outStockInfo.DeliveryTypeSysNo, 10, outStockId, expressNo);
                }
                catch (Exception ex)
                {
                    result.Message = "第" + (i + 1) + "行" + ex.Message;
                    return result;
                }
                if (!result.Status)
                {
                    result.Message = "第" + (i + 1) + "行" + result.Message;
                    return result;
                }
            }
            #endregion


            return result;

        }
        #endregion
        /// <summary>
        /// 出库单推送金蝶
        /// </summary>
        /// <param name="stockoutSysNo"></param>
        /// <remarks>2018-02-03 杨浩  创建</remarks>
        public void WhStockOutToErp(int stockoutSysNo)
        {
            var stockout = WhWarehouseBo.Instance.Get(stockoutSysNo);
            if (stockout == null)
                throw new HytException(string.Format("找不到编号为:{0}的出库单", stockoutSysNo));

            string head = string.Format("XSCK{0}T{1}", stockoutSysNo, stockout.OrderSysNO.ToString().PadLeft(10, '0'));
            string flowNo = "T" + stockout.OrderSysNO.ToString().PadLeft(10, '0');
            var dataList = Hyt.DataAccess.Sys.IEasDao.Instance.GetDataByFlowIdentify((int)接口类型.销售出库退货, flowNo);

            if (dataList == null || dataList.Count <= 0)
                UpdateErpProductNumber(stockoutSysNo);
            else
            {
                var stockList = new List<int>();
                foreach (var data in dataList)
                {
                    var sale = data.Data.ToObject<SaleInfoWraper>();

                    List<SaleInfo> model = sale.Model;
                    var saleInfo = model.FirstOrDefault();

                    int _stockoutSysno = saleInfo.ItemID ?? 0;

                    stockList.Add(_stockoutSysno);

                }

                if (!stockList.Where(x => stockoutSysNo == x).Any())
                {
                    UpdateErpProductNumber(stockoutSysNo);
                }
            }

        }

    }

}
