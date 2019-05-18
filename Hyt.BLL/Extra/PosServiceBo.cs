using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Grand.Platform.Api.Contract;
using Hyt.Model;
using Grand.Platform.Api.Contract.DataContract;

namespace Hyt.BLL.Extras
{
    public class PosServiceBo : BOBase<PosServiceBo>
    {
        static object lockObject = new object();

        /// <summary>
        /// 从POS系统获取更新商品价格
        /// </summary>
        /// <returns></returns>
        /// <remarks>2016-05-19 陈海裕 创建</remarks>
        public Result UpdateProductPricesFormPOS()
        {
            lock (lockObject)
            {
                Result result = new Result();
                string ErrorProduct = "";
                try
                {
                    IList<Grand.Platform.Api.Contract.Model.BranchPrice> branchPrices = null;
                    using (var service = new Pisen.Framework.Service.Proxy.ServiceProxy<IPosService>())
                    {
                        var response = service.Channel.GetBranchPriceList();
                        branchPrices = response.BranchPrices;
                    }

                    if (branchPrices != null)
                    {
                        List<int> sysNoList = new List<int>();
                        foreach (var item in branchPrices)
                        {
                            ErrorProduct = "ERPCODE:" + item.item_no;
                            List<Model.PdPrice> tempList = new List<Model.PdPrice>();
                            Model.PdPrice price = null;

                            //price.ProductSysNo = Convert.ToInt32(item.item_no);
                            if (item.base_price1 != 0)
                            {
                                price = new Model.PdPrice();
                                price.Price = item.base_price1;
                                price.SourceSysNo = 0;
                                tempList.Add(price);
                            }

                            if (item.base_price2 != 0)
                            {
                                price = new Model.PdPrice();
                                price.Price = item.base_price2;
                                price.SourceSysNo = 1;
                                tempList.Add(price);
                            }

                            if (item.base_price3 != 0)
                            {
                                price = new Model.PdPrice();
                                price.Price = item.base_price3;
                                price.SourceSysNo = 6;
                                tempList.Add(price);
                            }

                            if (item.base_price4 != 0)
                            {
                                price = new Model.PdPrice();
                                price.Price = item.base_price4;
                                price.SourceSysNo = 7;
                                tempList.Add(price);
                            }

                            if (item.base_price5 != 0)
                            {
                                price = new Model.PdPrice();
                                price.Price = item.base_price5;
                                price.SourceSysNo = 8;
                                tempList.Add(price);
                            }

                            if (item.base_price6 != 0)
                            {
                                price = new Model.PdPrice();
                                price.Price = item.base_price6;
                                price.SourceSysNo = 9;
                                tempList.Add(price);
                            }

                            if (tempList.Count > 0)
                            {
                                PdProduct tempProduct = BLL.Product.PdProductBo.Instance.GetProductByErpCode(item.item_no);
                                if (tempProduct == null)
                                {
                                    continue;
                                }
                                IList<PdPrice> tempPriceList = BLL.Product.PdPriceBo.Instance.GetProductPrice(tempProduct.SysNo);
                                PdPrice proPrice = new PdPrice();
                                foreach (var pri in tempList)
                                {
                                    proPrice = (PdPrice)tempPriceList.Where(p => p.SourceSysNo == pri.SourceSysNo).FirstOrDefault();
                                    if ((proPrice.Price != pri.Price) && !sysNoList.Contains(tempProduct.SysNo))
                                    {
                                        sysNoList.Add(tempProduct.SysNo);
                                        proPrice.Price = pri.Price;
                                        BLL.Product.PdPriceBo.Instance.Update(proPrice);
                                    }
                                }
                            }
                        }

                        // 更新索引
                        for (int i = 0; i < sysNoList.Count; i++)
                        {
                            //缓存清理
                            Hyt.BLL.Cache.DeleteCache.ProductInfo(sysNoList[i]);
                            //更新索引
                            BLL.Web.ProductIndexBo.Instance.UpdateProductIndex(sysNoList[i]);
                        }
                    }

                    BLL.Log.SysLog.Instance.Info(Model.WorkflowStatus.LogStatus.系统日志来源.后台, "从Pos系统获取更新商品价格成功", Model.WorkflowStatus.LogStatus.系统日志目标类型.商品基本信息, 0);

                    result.Status = true;
                }
                catch (Exception ex)
                {
                    BLL.Log.SysLog.Instance.Error(Model.WorkflowStatus.LogStatus.系统日志来源.后台, "从Pos系统获取更新商品价格异常:UpdateProductPricesFormPOS;" + ErrorProduct, ex);
                }

                return result;
            }
        }

        /// <summary>
        /// 从Pos系统根据仓库编码和产品编码中获取产品库存
        /// </summary>
        /// <param name="houseErpCode">仓库编码</param>
        /// <param name="productErpCode">产品库存</param>
        /// <returns></returns>
        /// <remarks>2016-5-18 杨浩 创建</remarks>
        public int GetProductStockByErpCodeAndWarehouseCode(string houseErpCode, string productErpCode)
        {
            int stock = 0;
            try
            {
                using (var service = new Pisen.Framework.Service.Proxy.ServiceProxy<IPosService>())
                {
                    var response = service.Channel.GetProductStock(houseErpCode, productErpCode);
                    stock = response;
                }
            }
            catch (Exception ex)
            {
                BLL.Log.SysLog.Instance.Error(Model.WorkflowStatus.LogStatus.系统日志来源.前台, "从Pos系统根据仓库编码和产品编码中获取产品库存请求服务异常:GetProductStockByErpCodeAndWarehouseCode", ex);
            }
            return stock;
        }
        /// <summary>
        /// 减库存
        /// </summary>
        /// <param name="request">请求数据集合</param>
        /// <returns></returns>
        /// <remarks>2016-5-17 杨浩 创建</remarks>
        public bool LessProductStock(LessProductStockRequest request)
        {
            try
            {
                using (var service = new Pisen.Framework.Service.Proxy.ServiceProxy<IPosService>())
                {
                    var response = service.Channel.LessProductStock(request);

                    return response.IsError == false;
                }
            }
            catch (Exception ex)
            {
                BLL.Log.SysLog.Instance.Error(Model.WorkflowStatus.LogStatus.系统日志来源.前台, "减库请求服务异常:LessProductStock", ex);
                return false;
            }

        }

        /// <summary>
        /// 退货入库
        /// </summary>
        /// <param name="model">入库单</param>
        /// <remarks>2016-10-31 杨浩 创建</remarks>
        public void ReturnsIntoWarehouse(WhStockIn model)
        {
            var result = new Result();
            try
            {
                var warehouse = BLL.Warehouse.WhWarehouseBo.Instance.GetWarehouse(model.WarehouseSysNo);

                var request = new Grand.Platform.Api.Contract.DataContract.LessProductStockRequest();

                Grand.Platform.Api.Contract.Model.SheetMaster sheetMaster = new Grand.Platform.Api.Contract.Model.SheetMaster();
                sheetMaster.trans_no = "00";
                sheetMaster.branch_no = warehouse.ErpCode;
                sheetMaster.oper_date = DateTime.Now;
                sheetMaster.sheet_amt = 0;
                sheetMaster.db_no = "+";
                IList<Grand.Platform.Api.Contract.Model.SheetDetail> sheetDetailList = new List<Grand.Platform.Api.Contract.Model.SheetDetail>();
                Grand.Platform.Api.Contract.Model.SheetDetail sheetDetail = null;
                foreach (var item in model.ItemList)
                {
                    sheetDetail = new Grand.Platform.Api.Contract.Model.SheetDetail();
                    var tempProduct = BLL.Product.PdProductBo.Instance.GetProduct(item.ProductSysNo);
                    sheetDetail.item_no = tempProduct.ErpCode;
                    sheetDetail.real_qty = item.RealStockInQuantity;
                    sheetDetail.large_qty = item.RealStockInQuantity;
                    sheetDetail.valid_price =0;
                    sheetDetail.sale_price = 0;
                    sheetDetail.sub_amt =0;
                    sheetDetailList.Add(sheetDetail);
                }
                request.SheetMaster = sheetMaster;
                request.SheetDetails = sheetDetailList;

                string json = Util.Serialization.JsonUtil.ToJson2(request);

                result.Status = this.LessProductStock(request);
                if (result.Status == false)
                {
                    //var queObj = DataAccess.Extra.IPosServiceDao.Instance.GetEntityByOrderSysNo(orderSysNo);
                    //if (queObj == null)
                    //{
                    //    // 扣减库存失败，添加到队列表
                    //    ReducedPOSInventoryQueue model1 = new ReducedPOSInventoryQueue();
                    //    model.OrderSysNo = orderSysNo;
                    //    model.JsonData = json;
                    //    this.AddToReducedPOSInventoryQueue(model);
                    //}

                    BLL.Log.SysLog.Instance.Error(Model.WorkflowStatus.LogStatus.系统日志来源.前台, "POS系统加库存失败:ReturnsIntoWarehouse", new Exception());
                }
            }
            catch (Exception ex)
            {
                //// 扣减库存失败，添加到队列表
                //ReducedPOSInventoryQueue model1 = new ReducedPOSInventoryQueue();
                //model.OrderSysNo = orderSysNo;
                //model.Remark = ex.ToString();
                //this.AddToReducedPOSInventoryQueue(model);

                BLL.Log.SysLog.Instance.Error(Model.WorkflowStatus.LogStatus.系统日志来源.前台, "POS系统加库存失败:ReturnsIntoWarehouse", ex);
            }
        }

        /// <summary>
        /// POS系统减库存（维嘉项目）
        /// </summary>
        /// <param name="orderSysNo"></param>
        /// <param name="orderAmount"></param>
        /// <param name="db_no">单据标示</param>
        /// <remarks>2016-05-20 陈海裕 创建</remarks>
        public void ReducedPOSInventory(int orderSysNo, decimal orderAmount, string db_no=null)
        {
            Result result = new Result();
            try
            {
                SoOrder order = BLL.Order.SoOrderBo.Instance.GetEntity(orderSysNo);
                IList<SoOrderItem> orderItems = BLL.Order.SoOrderBo.Instance.GetOrderItemsByOrderId(orderSysNo);
                CBWhWarehouse warehouse = BLL.Warehouse.WhWarehouseBo.Instance.GetWarehouse(order.DefaultWarehouseSysNo);

                Grand.Platform.Api.Contract.DataContract.LessProductStockRequest request = new Grand.Platform.Api.Contract.DataContract.LessProductStockRequest();

                Grand.Platform.Api.Contract.Model.SheetMaster sheetMaster = new Grand.Platform.Api.Contract.Model.SheetMaster();
                sheetMaster.trans_no = "00";
                sheetMaster.branch_no = warehouse.ErpCode;
                sheetMaster.oper_date = DateTime.Now;
                sheetMaster.sheet_amt = orderAmount;
                sheetMaster.db_no = db_no;
                IList<Grand.Platform.Api.Contract.Model.SheetDetail> sheetDetailList = new List<Grand.Platform.Api.Contract.Model.SheetDetail>();
                Grand.Platform.Api.Contract.Model.SheetDetail sheetDetail = null;
                foreach (var item in orderItems)
                {
                    sheetDetail = new Grand.Platform.Api.Contract.Model.SheetDetail();
                    var tempProduct = BLL.Product.PdProductBo.Instance.GetProduct(item.ProductSysNo);
                    sheetDetail.item_no = tempProduct.ErpCode;
                    sheetDetail.real_qty = item.Quantity;
                    sheetDetail.large_qty = item.Quantity;
                    sheetDetail.valid_price = item.SalesUnitPrice;
                    sheetDetail.sale_price = item.OriginalPrice;
                    sheetDetail.sub_amt = item.SalesAmount;
                    sheetDetailList.Add(sheetDetail);
                }
                request.SheetMaster = sheetMaster;
                request.SheetDetails = sheetDetailList;

                string json = Util.Serialization.JsonUtil.ToJson2(request);

                result.Status = this.LessProductStock(request);
                if (result.Status == false)
                {
                    var queObj = DataAccess.Extra.IPosServiceDao.Instance.GetEntityByOrderSysNo(orderSysNo);
                    if (queObj == null)
                    {
                        // 扣减库存失败，添加到队列表
                        ReducedPOSInventoryQueue model = new ReducedPOSInventoryQueue();
                        model.OrderSysNo = orderSysNo;
                        model.JsonData = json;
                        this.AddToReducedPOSInventoryQueue(model);
                    }

                    BLL.Log.SysLog.Instance.Error(Model.WorkflowStatus.LogStatus.系统日志来源.前台, "POS系统减库存失败:ReducedPOSInventory", new Exception());
                }
            }
            catch (Exception ex)
            {
                // 扣减库存失败，添加到队列表
                ReducedPOSInventoryQueue model = new ReducedPOSInventoryQueue();
                model.OrderSysNo = orderSysNo;
                model.Remark = ex.ToString();
                this.AddToReducedPOSInventoryQueue(model);

                BLL.Log.SysLog.Instance.Error(Model.WorkflowStatus.LogStatus.系统日志来源.前台, "POS系统减库存失败:ReducedPOSInventory", ex);
            }
        }

        /// <summary>
        /// 根据商品ERP编码获取POS系统仓库信息列表
        /// </summary>
        /// <param name="productErpCode"></param>
        /// <returns></returns>
        /// <remarks>2016-05-20 陈海裕 创建</remarks>
        public List<WhWarehouse> GetWarehouseStockByProductErpCode(string productErpCode)
        {
            //List<WhWarehouse> warehouseList = new List<WhWarehouse>();
            //try
            //{
            //    IList<BranchStock> branchStocks = new List<BranchStock>();
            //    using (var service = new Pisen.Framework.Service.Proxy.ServiceProxy<IPosService>())
            //    {
            //        branchStocks = service.Channel.GetWarehouseStockByProductErpCode(productErpCode).BranchStocks;
            //    }
            //    if (branchStocks != null)
            //    {
            //        WhWarehouse warehouse = null;
            //        foreach (var item in branchStocks)
            //        {
            //            warehouse = new WhWarehouse();
            //            warehouse.WarehouseName = item.branch_name;
            //            //warehouse.StockQuantity = item.stock_qty;
            //            warehouse.ErpCode = item.branch_no;
            //            warehouseList.Add(warehouse);
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    BLL.Log.SysLog.Instance.Error(Model.WorkflowStatus.LogStatus.系统日志来源.前台, "根据商品ERP编码获取POS系统仓库信息列表:GetWarehouseStockByProductErpCode", ex);
            //}

            //return warehouseList;

            throw new NotImplementedException();
        }

        /// <summary>
        /// 减库存失败的订单将存放在此表中，等待再次扣减库存
        /// </summary>
        /// <param name="entity"></param>
        /// <remarks>2016-05-24 陈海裕 创建</remarks>
        public void AddToReducedPOSInventoryQueue(ReducedPOSInventoryQueue entity)
        {
            entity.CreateDate = DateTime.Now;
            DataAccess.Extra.IPosServiceDao.Instance.AddToReducedPOSInventoryQueue(entity);
        }
    }
}
