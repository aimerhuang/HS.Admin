using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using Hyt.Model;
using Hyt.Model.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;
using Hyt.Util;

namespace Hyt.BLL.ApiSupply.HaiTun
{
    /// <summary>
    /// 海豚供应链接口
    /// </summary>
    /// <remarks>
    /// 2016-2-23 杨浩 创建
    /// 2016-4-6 陈海裕 修改
    /// 2016-5-20 刘伟豪 修改
    /// </remarks>
    public class HaiTunProvider : ISupplyProvider
    {
        #region 属性字段
        public override CommonEnum.供应链代码 Code
        {
            get { return CommonEnum.供应链代码.海豚; }
        }

        protected override SupplyInfo Config
        {
            get { return Hyt.BLL.Config.Config.Instance.GetSupplyConfig().SupplyList.FirstOrDefault(s => s.Key == Code.ToString()); }
        }

        private static object lockHelper = new object();

        public HaiTunProvider() { }
        #endregion

        #region 商品管理
        /// <summary>
        /// 获取所有商品
        /// </summary>
        /// <remarks>
        /// 2016-4-6 陈海裕 创建
        /// 2016-5-20 刘伟豪 修改
        /// </remarks>
        public override Result<string> GetGoodsList(ParaSupplyProductFilter paraFilte = null)
        {
            Result<string> result = new Result<string>()
            {
                Status = false,
                StatusCode = 0,
                Message = "未知错误"
            };

            //防止并发操作
            lock (lockHelper)
            {
                try
                {
                    string responseStr = Post("getStocks");
                    var back = JObject.Parse(responseStr);
                    result = GetResult(back);

                    if (result.Status)
                    {
                        var pIndex = 0;
                        var pSize = 10;
                        var pCount = 1;

                        var totalNum = back["data"].Children().Count();
                        pCount = totalNum % pSize > 0 ? totalNum / pSize + 1 : totalNum / pSize;

                        Array arrList = Array.CreateInstance(typeof(string), totalNum);
                        var i = 0;
                        var stockObj = JObject.Parse(back["data"].ToString());
                        foreach (KeyValuePair<string, JToken> keyValuePair in stockObj)
                        {
                            arrList.SetValue(keyValuePair.Key, i);
                            i++;
                        }

                        //循环获取商品详情，接口只提供一次获取10个
                        do
                        {
                            var aLength = pIndex + 1 < pCount ? pSize : totalNum % pSize;
                            var idxMax = (pIndex + 1) * pSize > totalNum ? totalNum : (pIndex + 1) * pSize;
                            Array arr = Array.CreateInstance(typeof(string), aLength);
                            var j = 0;
                            for (var idx = pIndex * pSize; idx < idxMax; idx++)
                            {
                                arr.SetValue(arrList.GetValue(idx), j);
                                j++;
                            }

                            result = GetGoodsDetail(JsonConvert.SerializeObject(new { sku = arr }));

                            pIndex++;
                        }
                        while (pIndex < pCount);
                    }
                }
                catch (Exception ex)
                {
                    result.Message = ex.Message;
                    BLL.Log.SysLog.Instance.Error(LogStatus.系统日志来源.后台, this.Code + "供应链，获取所有商品：" + ex.Message, ex);
                }
                return result;
            }
        }

        /// <summary>
        /// 获取指定商品
        /// </summary>
        /// <param name="skuid">海豚商品sku</param>
        /// <remarks>
        /// 2016-5-20 刘伟豪 创建
        /// </remarks>
        public override Result<string> GetGoodsSku(string skuid)
        {
            Result<string> result = new Result<string>()
            {
                Status = false,
                StatusCode = 0,
                Message = "未知错误"
            };

            //防止并发操作
            lock (lockHelper)
            {
                try
                {
                    var skus = skuid.Split(',');
                    if (skus.Length > 0)
                    {
                        var pIndex = 0;
                        var pSize = 10;
                        var pCount = 1;

                        var totalNum = skus.Length;
                        pCount = totalNum % pSize > 0 ? totalNum / pSize + 1 : totalNum / pSize;

                        Array arrList = Array.CreateInstance(typeof(string), totalNum);
                        var i = 0;
                        foreach (var sku in skus)
                        {
                            arrList.SetValue(sku, i);
                            i++;
                        }

                        //循环获取商品详情，接口只提供一次获取10个
                        do
                        {
                            var aLength = pIndex + 1 < pCount ? pSize : totalNum % pSize;
                            var idxMax = (pIndex + 1) * pSize > totalNum ? totalNum : (pIndex + 1) * pSize;
                            Array arr = Array.CreateInstance(typeof(string), aLength);
                            var j = 0;
                            for (var idx = pIndex * pSize; idx < idxMax; idx++)
                            {
                                arr.SetValue(arrList.GetValue(idx), j);
                                j++;
                            }

                            result = GetGoodsDetail(JsonConvert.SerializeObject(new { sku = arr }));

                            pIndex++;
                        }
                        while (pIndex < pCount);
                    }
                }
                catch (Exception ex)
                {
                    result.Message = ex.Message;
                    BLL.Log.SysLog.Instance.Error(LogStatus.系统日志来源.后台, this.Code + "供应链，获取指定商品：" + ex.Message, ex);
                }
                return result;
            }
        }

        /// <summary>
        /// 获取商品详情
        /// </summary>
        /// <param name="postJson">{sku:["AMAV001","AMCB009"]}</param>
        /// <remarks>
        /// 2016-5-23 刘伟豪 创建
        /// </remarks>
        private Result<string> GetGoodsDetail(string postJson = "")
        {
            Result<string> result = new Result<string>()
            {
                Status = false,
                StatusCode = 0,
                Message = "未知错误"
            };

            //防止并发操作
            lock (lockHelper)
            {
                try
                {
                    string responseStr = Post("getProductDetail", postJson);
                    var back = JObject.Parse(responseStr);
                    result = GetResult(back);
                    if (result.Status)
                    {
                        var products = Hyt.BLL.Supply.ScProductBo.Instance.GetScProductList((int)Code);
                        var goodsList = back["data"];

                        foreach (var good in goodsList)
                        {
                            var g = good.First();
                            var _product = products.FirstOrDefault(x => x.SKU == g["skuCode"].ToString());
                            ScProduct product = new ScProduct();
                            if (_product != null)
                            {
                                product = _product;
                            }

                            product.SKU = g["skuCode"].ToString();
                            product.SupplyCode = (int)Code;
                            product.ProductName = g["productName"].ToString();
                            product.Receipt = JsonConvert.SerializeObject(g);
                            product.Status = 1;
                            product.Price = decimal.Parse(g["price"].ToString());

                            var detail = g["detail"];
                            foreach (var d in detail)
                            {
                                if (d["proName"].ToString().Trim() == "品牌")
                                {
                                    product.Brands = d["proValue"].ToString().Trim();
                                }
                                if (d["proName"].ToString().Trim() == "原产地")
                                {
                                    product.Cradle = d["proValue"].ToString().Trim();
                                }
                            }

                            if (_product == null)
                            {
                                product.SysNo = BLL.Supply.ScProductBo.Instance.AddScProduct(product);
                                products.Add(product);
                            }
                            else
                            {
                                BLL.Supply.ScProductBo.Instance.UpdateScProduct(product);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    result.Message = ex.Message;
                    BLL.Log.SysLog.Instance.Error(LogStatus.系统日志来源.后台, this.Code + "供应链，获取商品详情：" + ex.Message, ex);
                }
                return result;
            }
        }

        /// <summary>
        /// 批量入库
        /// </summary>
        /// <param name="sysNos">商品系统编号，逗号分隔</param>
        /// <returns></returns>
        /// <remarks>
        /// 2016-5-25 刘伟豪 创建
        /// </remarks> 
        public override Result<string> StockInSupplyProduct(string sysNos)
        {
            var result = new Result<string>()
            {
                Status = false,
                StatusCode = -1,
                Message = "未知错误"
            };

            //防止多人同时操作
            lock (lockHelper)
            {
                try
                {
                    //是否有仓库
                    WhWarehouse Warehouse = BLL.Warehouse.WhWarehouseBo.Instance.GetAllWarehouseList().FirstOrDefault(w => w.Supply == (int)Code && w.Status == (int)Model.WorkflowStatus.WarehouseStatus.仓库状态.启用);
                    if (Warehouse == null)
                    {
                        result.Message = string.Format("尚未创建{0}专用仓库，如果已创建请更新后台缓存！", Code.ToString());
                        return result;
                    }

                    bool isSuccess = false;
                    foreach (var c in sysNos.Split(','))
                    {
                        int id = int.Parse(c);
                        int UserSysNo = Hyt.BLL.Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo;
                        ScProduct Product = BLL.Supply.ScProductBo.Instance.GetScProductInfo(id);

                        //获取库存，没库存将不执行入库操作
                        var stock = GetProSkuStock(Product.SKU);
                        if (stock == 0m)
                            continue;

                        //未入库的商品可入库
                        if (Product.ProductSysNo == 0)
                        {
                            using (var tran = new System.Transactions.TransactionScope())
                            {
                                var json = JObject.Parse(Product.Receipt);
                                PdProduct PdData = new PdProduct();

                                //绑定品牌
                                var brandSysNo = 0;
                                var brandName = string.IsNullOrWhiteSpace(Product.Brands) ? "其他" : Product.Brands;
                                PdBrand PdBrand = BLL.Product.PdBrandBo.Instance.GetEntityByName(brandName);
                                brandSysNo = PdBrand != null ? PdBrand.SysNo : DataAccess.Product.IPdBrandDao.Instance.Create(new PdBrand() { Name = brandName, DisplayOrder = 0, Status = 1 });
                                PdData.BrandSysNo = brandSysNo;

                                //绑定原产地
                                var orginSysNo = 0;
                                var orginName = string.IsNullOrWhiteSpace(Product.Cradle) ? "其他" : Product.Cradle;
                                Origin Origin = BLL.Basic.OriginBo.Instance.GetEntityByName(orginName);
                                orginSysNo = Origin != null ? Origin.SysNo : DataAccess.Basic.IOriginDao.Instance.Insert(new Origin() { Origin_Name = orginName, CreatedBy = UserSysNo, CreatedDate = DateTime.Now, LastUpdateBy = UserSysNo, LastUpdateDate = DateTime.Now });
                                PdData.OriginSysNo = orginSysNo;

                                PdData.ErpCode = PdData.Barcode = Product.SKU;
                                PdData.EasName = string.Format("({0}产品){1}", Code.ToString(), Product.ProductName);
                                PdData.ProductName = PdData.SeoTitle = PdData.SeoKeyword = PdData.SeoDescription = Product.ProductName;
                                PdData.ViewCount = PdData.DisplayOrder = 0;

                                var weight = 0.00m;
                                foreach (var d in json["detail"])
                                {
                                    if (d["proName"].ToString().Trim() == "规格")
                                    {
                                        var w = d["proValue"].ToString().Trim();
                                        if (w.IndexOf("g") > -1)
                                        {
                                            if (w.IndexOf("kg") > -1)
                                            {
                                                decimal.TryParse(w.Replace("kg", ""), out weight);
                                            }
                                            else
                                            {
                                                if (decimal.TryParse(w.Replace("g", ""), out weight))
                                                    weight = weight * 0.001m;
                                            }
                                        }
                                    }
                                }
                                PdData.GrosWeight = weight;

                                PdData.NetWeight = 0.00m;
                                PdData.SalesMeasurementUnit = "kg";
                                PdData.ValueUnit = "CNY";
                                PdData.VolumeUnit = "m";
                                PdData.Tax = PdData.Freight = PdData.VolumeValue = PdData.Rate = PdData.PriceRate = PdData.PriceValue = PdData.DealerPriceValue = PdData.TradePrice = 0.00m;
                                PdData.CostPrice = Product.Price;
                                PdData.FreightFlag = "Y";

                                PdData.AgentSysNo = 1;
                                PdData.DealerSysNo = 0;

                                //各状态
                                PdData.ProductType = (int)Model.WorkflowStatus.ProductStatus.商品类型.普通商品;
                                PdData.CanFrontEndOrder = (int)Model.WorkflowStatus.ProductStatus.商品是否前台下单.是;
                                PdData.IsFrontDisplay = (int)Model.WorkflowStatus.ProductStatus.前台显示.是;
                                PdData.Status = (int)Model.WorkflowStatus.ProductStatus.商品状态.下架;

                                PdData.CreatedBy = PdData.LastUpdateBy = UserSysNo;
                                PdData.CreatedDate = PdData.LastUpdateDate = PdData.Stamp = DateTime.Now;

                                //创建商品
                                int productSysNo = BLL.Product.PdProductBo.Instance.CreateProduct(PdData);
                                if (productSysNo > 0)
                                {
                                    //创建商品基础价
                                    PdPrice BasicModel = new PdPrice();
                                    BasicModel.ProductSysNo = productSysNo;
                                    BasicModel.Price = Product.Price;
                                    BasicModel.PriceSource = (int)Model.WorkflowStatus.ProductStatus.产品价格来源.基础价格;
                                    BasicModel.SourceSysNo = 0;
                                    BasicModel.Status = (int)Model.WorkflowStatus.ProductStatus.产品价格状态.有效;
                                    Hyt.BLL.Product.PdPriceBo.Instance.Create(BasicModel);
                                    //创建商品会员价
                                    PdPrice SaleModel = new PdPrice();
                                    SaleModel.ProductSysNo = productSysNo;
                                    SaleModel.Price = Product.Price;
                                    SaleModel.PriceSource = (int)Model.WorkflowStatus.ProductStatus.产品价格来源.会员等级价;
                                    SaleModel.SourceSysNo = 1;
                                    SaleModel.Status = (int)Model.WorkflowStatus.ProductStatus.产品价格状态.有效;
                                    Hyt.BLL.Product.PdPriceBo.Instance.Create(SaleModel);
                                    //更新商品临时表的ProductSysNo
                                    Hyt.BLL.Supply.ScProductBo.Instance.UpdateProductSysNo(id, productSysNo);

                                    //创建商品库存
                                    Hyt.BLL.Supply.ScProductBo.Instance.ProCreateSupplyStock((int)Code, productSysNo, stock, UserSysNo);
                                }
                                tran.Complete();
                                isSuccess = true;
                            }
                        }
                        else
                        { 
                            //更新平台商品

                        }
                    }
                    //是否成功
                    if (isSuccess)
                    {
                        result.Message = "入库成功！";
                        result.Status = true;
                        result.StatusCode = 1;
                    }
                    else
                    {
                        result.Message = this.Code + "供应链没有对应库存";
                    }
                }
                catch (Exception ex)
                {
                    result.Message = ex.Message;
                    BLL.Log.SysLog.Instance.Error(LogStatus.系统日志来源.后台, this.Code + "供应链，入库操作：" + ex.Message, ex);
                }
                return result;
            }
        }

        /// <summary>
        /// 获取库存
        /// </summary>
        /// <param name="sku">海豚skuCode</param>
        /// <remarks> 
        /// 2016-5-25 刘伟豪 创建
        /// </remarks>
        private decimal GetProSkuStock(string sku)
        {
            var stock = 0m;

            try
            {
                string responseStr = Post("getStocks", JsonConvert.SerializeObject(new { sku = new string[1] { sku } }));
                var back = JObject.Parse(responseStr);
                var result = GetResult(back);

                if (result.Status)
                {
                    var stockObj = back["data"][sku];
                    decimal.TryParse(stockObj["physi"].ToString(), out stock);
                }
            }
            catch (Exception ex)
            {
                BLL.Log.SysLog.Instance.Error(LogStatus.系统日志来源.后台, this.Code + "供应链，入库前获取库存操作：" + ex.Message, ex);
                stock = 0m;
            }
            return stock;
        }
        #endregion

        public override Result<string> CheckOrder(int orderSysNo)
        {
            throw new NotImplementedException();
        }

        public override Result<string> CancelOrder(int orderSysNo)
        {
            throw new NotImplementedException();
        }

        public override Result<string> GetShipping()
        {
            throw new NotImplementedException();
        }

        public override Result<string> GetAllGoodsSku()
        {
            throw new NotImplementedException();
        }

        #region 订单管理
        /// <summary>
        /// 推送供应链订单
        /// </summary>
        /// <param name="orderSysNo"></param>
        /// <returns></returns>
        /// <remarks>
        /// 2016-4-15 陈海裕 创建
        /// 2016-5-20 刘伟豪 修改
        /// </remarks>
        public override Result<string> SendOrder(int orderSysNo)
        {
            //防止并发操作
            lock (lockHelper)
            {
                // type: pushOrderDataInfo
                var result = new Result<string>()
                {
                    Status = false,
                    StatusCode = 0,
                    Message = "向" + this.Code + "供应链推送订单失败"
                };

                if (orderSysNo <= 0)
                {
                    return result;
                }

                // json格式的post数据
                string jsonData = "";
                try
                {
                    SoOrder order = BLL.Order.SoOrderBo.Instance.GetEntity(orderSysNo);
                    if (order == null)
                    {
                        result.Message = "该订单不存在";
                        return result;
                    }
                    order.ReceiveAddress = BLL.Order.SoOrderBo.Instance.GetOrderReceiveAddress(order.ReceiveAddressSysNo);
                    order.OrderItemList = DataAccess.Order.ISoOrderItemDao.Instance.GetOrderItemsByOrderSysNo(order.SysNo);
                    ParaVoucherFilter voucherFilter = new ParaVoucherFilter();
                    voucherFilter.SourceSysNo = order.SysNo;
                    CBFnReceiptVoucher recVoucher = BLL.Finance.FinanceBo.Instance.GetReceiptVouchers(voucherFilter).Rows.FirstOrDefault();
                    recVoucher.VoucherItems = BLL.Finance.FnReceiptVoucherBo.Instance.GetReceiptVoucherItem(recVoucher.SysNo);
                    // 收货人 区 市 省
                    BsArea receiverDistrict = BLL.Basic.BasicAreaBo.Instance.GetArea(order.ReceiveAddress.AreaSysNo);
                    BsArea receiverCity = BLL.Basic.BasicAreaBo.Instance.GetArea(receiverDistrict.ParentSysNo);
                    BsArea receiverProvince = BLL.Basic.BasicAreaBo.Instance.GetArea(receiverCity.ParentSysNo);

                    DsDealer dealer = BLL.Distribution.DsDealerBo.Instance.GetDsDealer(order.DealerSysNo);

                    HaiTunOrder newOrder = new HaiTunOrder();
                    newOrder.address = TConvert.ToString(order.ReceiveAddress.StreetAddress);
                    newOrder.city = TConvert.ToString(receiverCity.AreaName.Trim());
                    newOrder.consignee = TConvert.ToString(order.ReceiveAddress.Name);
                    newOrder.consumerNote = TConvert.ToString(order.Remarks);
                    newOrder.country = "中国";
                    newOrder.district = TConvert.ToString(receiverDistrict.AreaName.Trim());
                    newOrder.idCardNumber = TConvert.ToString(order.ReceiveAddress.IDCardNo);
                    newOrder.isCheck = "no";
                    newOrder.mobile = TConvert.ToString(order.ReceiveAddress.MobilePhoneNumber);
                    newOrder.moneyPaid = TConvert.ToString(order.OrderAmount);
                    newOrder.orderAmount = TConvert.ToString(order.OrderAmount);
                    newOrder.orderSn = TConvert.ToString(order.SysNo);
                    newOrder.paymentAccount = "htdolphin@163.com"; // 固定值
                    newOrder.paymentInfoIdCardNumber = TConvert.ToString(order.ReceiveAddress.IDCardNo);
                    newOrder.paymentInfoMethod = "支付宝";
                    newOrder.paymentInfoName = TConvert.ToString(order.ReceiveAddress.Name);
                    newOrder.paymentInfoNumber = TConvert.ToString(recVoucher.VoucherItems[0].VoucherNo);
                    newOrder.province = TConvert.ToString(receiverProvince.AreaName);
                    newOrder.shippingFee = TConvert.ToString(order.FreightAmount);
                    newOrder.siteName = TConvert.ToString(dealer.ErpName);
                    newOrder.siteType = "商城";
                    newOrder.tel = TConvert.ToString(order.ReceiveAddress.PhoneNumber);
                    newOrder.zipcode = TConvert.ToString(order.ReceiveAddress.ZipCode);
                    newOrder.items = new List<HaiTunOrderItem>();
                    HaiTunOrderItem haitunItem = new HaiTunOrderItem();
                    foreach (var item in order.OrderItemList)
                    {
#if DEBUG
                        haitunItem.goodsName = "【广州保税 全国包邮】澳大利亚Swisse 奶蓟草护肝片肝脏排毒120粒 【2件起发】";
                        haitunItem.goodsPrice = "100";
                        haitunItem.goodsSn = "AUSW003";
                        haitunItem.quantity = "1";
                        newOrder.items.Add(haitunItem);
                        break;
#else
                    PdProductStock productStock = BLL.Warehouse.PdProductStockBo.Instance.GetEntityByWP(order.DefaultWarehouseSysNo, item.ProductSysNo);
                    var supplyProduct = BLL.Supply.ScProductBo.Instance.GetScProductInfo(productStock.ProductSku, (int)this.Code);
                    if (supplyProduct != null)
                    {
                        haitunItem.goodsName = TConvert.ToString(supplyProduct.ProductName);
                        haitunItem.goodsPrice = TConvert.ToString(item.SalesUnitPrice);
                        haitunItem.goodsSn = TConvert.ToString(supplyProduct.SKU);
                        haitunItem.quantity = TConvert.ToString(item.Quantity);
                        newOrder.items.Add(haitunItem);
                    }
                    else
                    {
                        result.Message = "商品" + item.ProductSysNo + "对应的供应链商品不存在";
                        return result;
                    }
#endif
                    }

                    jsonData = Util.Serialization.JsonUtil.ToJson2(new List<HaiTunOrder>() { newOrder });

                    var back = Post("pushOrderDataInfo", jsonData);
                    result = GetResult(JObject.Parse(back));
                    if (result.Status)
                    {

                    }
                }
                catch (Exception ex)
                {
                    result.Message = ex.Message;
                    BLL.Log.SysLog.Instance.Error(LogStatus.系统日志来源.后台, this.Code + "供应链订单推送：" + ex.Message, ex);
                }

                return result;
            }
        }

        /// <summary>
        /// 海豚订单
        /// </summary>
        private class HaiTunOrder
        {
            public string consignee { get; set; }
            public string country { get; set; }
            public string province { get; set; }
            public string city { get; set; }
            public string district { get; set; }
            public string address { get; set; }
            public string zipcode { get; set; }
            /// <summary>
            /// 座机(可以与mobile二选一)
            /// </summary>
            public string tel { get; set; }
            public string mobile { get; set; }
            public string orderSn { get; set; }
            public string idCardNumber { get; set; }
            /// <summary>
            /// 店铺类型,如:线下,淘宝等等
            /// </summary>
            public string siteType { get; set; }
            /// <summary>
            /// 店铺名称,打印面单时的内容
            /// </summary>
            public string siteName { get; set; }
            /// <summary>
            /// 备注
            /// </summary>
            public string consumerNote { get; set; }
            /// <summary>
            /// 运费金额
            /// </summary>
            public string shippingFee { get; set; }
            /// <summary>
            /// 订单金额
            /// </summary>
            public string orderAmount { get; set; }
            /// <summary>
            /// 支付金额
            /// </summary>
            public string moneyPaid { get; set; }
            /// <summary>
            /// 支付人姓名
            /// </summary>
            public string paymentInfoName { get; set; }
            /// <summary>
            /// 支付人身份证
            /// </summary>
            public string paymentInfoIdCardNumber { get; set; }
            /// <summary>
            /// 支付类型(字符类型，参考支付类型表)
            /// </summary>
            public string paymentInfoMethod { get; set; }
            /// <summary>
            /// 支付流水号
            /// </summary>
            public string paymentInfoNumber { get; set; }
            /// <summary>
            /// htdolphin@163.com固定值
            /// </summary>
            public string paymentAccount { get; set; }
            public List<HaiTunOrderItem> items { get; set; }
            /// <summary>
            /// "yes"自动通知发货,默认"no"
            /// </summary>
            public string isCheck { get; set; }
        }

        /// <summary>
        /// 海豚订单商品
        /// </summary>
        private class HaiTunOrderItem
        {
            public string quantity { get; set; }
            public string goodsName { get; set; }
            public string goodsSn { get; set; }
            public string goodsPrice { get; set; }
        }
        #endregion

        #region 私有方法
        /// <summary>
        /// 检验返回结果
        /// </summary>
        /// <param name="back"></param>
        /// <remarks>
        /// 2016-4-6 陈海裕 创建
        /// 2016-5-20 刘伟豪 修改
        /// </remarks>
        private Result<string> GetResult(JObject back)
        {
            var result = new Result<string>()
            {
                Status = false,
                StatusCode = 0,
                Message = "未知错误"
            };

            try
            {
                if (back.Property("status") != null)
                {
                    int stat = 0;
                    int.TryParse(back["status"].ToString(), out stat);
                    if (stat == (int)Model.WorkflowStatus.SupplyStatus.海豚接口返回状态.响应成功)
                    {
                        result.Status = true;
                        result.Message = "响应成功";
                        result.Data = back.ToString();
                    }
                    else
                    {
                        result.Message = Hyt.Util.EnumUtil.GetDescription(typeof(Model.WorkflowStatus.SupplyStatus.海豚接口返回状态), stat) + "," + stat;
                    }
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return result;
        }

        /// <summary>
        /// Post
        /// </summary>
        /// <param name="type">调用方法</param>
        /// <param name="postJson">json数据</param>
        /// <remarks>
        /// 2016-5-20 刘伟豪 创建
        /// </remarks>
        private string Post(string type, string postJson = "")
        {
            Dictionary<string, string> getData = new Dictionary<string, string>();
            var tokenObj = JObject.Parse(GetToken());
            //getData.Add("debug", "on");
            getData.Add("type", type);
            getData.Add("name", tokenObj["data"]["name"].ToString());
            getData.Add("time", base.GetTimeStamp());

            var back = GetResponse(getData, tokenObj["data"]["key"].ToString(), postJson);

            return back;
        }

        /// <summary>
        /// 获取令牌
        /// </summary>
        /// <remarks>
        /// 2016-4-6 陈海裕 创建
        /// 2016-5-20 刘伟豪 修改
        /// </remarks>
        private string GetToken()
        {
            Dictionary<string, string> sourData = new Dictionary<string, string>();
            //sourData.Add("debug", "on");
            sourData.Add("type", "token");
            sourData.Add("name", Config.Account);
            sourData.Add("time", base.GetTimeStamp());

            string back = GetResponse(sourData, Config.Secert);

            return back;
        }

        /// <summary>
        /// 传递请求并接受返回结果，get传方法及验证，post传数据
        /// </summary>
        /// <param name="getData">get参数</param>
        /// <param name="key">密钥</param>
        /// <param name="postJson">post参数（json）</param>
        /// <remarks>
        /// 2016-4-6 陈海裕 创建
        /// 2016-5-20 刘伟豪 修改
        /// </remarks>
        private string GetResponse(Dictionary<string, string> getData, string key, string postJson = "")
        {
            string requestUrl = Config.GatewayUrl + "?";
            string md5string = "";
            string postData = Convert.ToBase64String(EncryptRC4(Encoding.UTF8.GetBytes(postJson), key));
            foreach (var i in getData.OrderBy(o => o.Key).ToDictionary(k => k.Key, v => v.Value))
            {
                md5string += i.Value;
            }
            md5string += key + postData;
            string md5Code = base.Encrypt_MD5(md5string);

            getData.Add("md5", md5Code);

            foreach (var i in getData)
            {
                requestUrl += i.Key + "=" + i.Value + "&";
            }
            if (requestUrl.Length > 0)
            {
                requestUrl = requestUrl.Substring(0, requestUrl.Length - 1);
            }

            byte[] data = Encoding.UTF8.GetBytes("data=" + postData);

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(requestUrl);
            request.Method = "POST";
            request.KeepAlive = true;
            request.Timeout = 300000;
            request.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
            request.ContentLength = data.Length;
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/47.0.2526.111 Safari/537.36";
            using (Stream stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }
            string respStr = "";
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    respStr = reader.ReadToEnd();
                }
            }

            respStr = Encoding.UTF8.GetString(EncryptRC4(Convert.FromBase64String(respStr), key));

            return respStr;
        }

        private Byte[] EncryptRC4(Byte[] data, string pass)
        {
            if (data == null || pass == null) return null;
            Byte[] output = new Byte[data.Length];
            Int64 i = 0;
            Int64 j = 0;
            Byte[] mBox = GetKey(Encoding.UTF8.GetBytes(pass), 256);

            // 加密
            for (Int64 offset = 0; offset < data.Length; offset++)
            {
                i = (i + 1) % mBox.Length;
                j = (j + mBox[i]) % mBox.Length;
                Byte temp = mBox[i];
                mBox[i] = mBox[j];
                mBox[j] = temp;
                Byte a = data[offset];
                //Byte b = mBox[(mBox[i] + mBox[j] % mBox.Length) % mBox.Length];
                // mBox[j] 一定比 mBox.Length 小，不需要在取模
                Byte b = mBox[(mBox[i] + mBox[j]) % mBox.Length];
                output[offset] = (Byte)((Int32)a ^ (Int32)b);
            }

            return output;
        }

        private Byte[] GetKey(Byte[] pass, Int32 kLen)
        {
            Byte[] mBox = new Byte[kLen];

            for (Int64 i = 0; i < kLen; i++)
            {
                mBox[i] = (Byte)i;
            }
            Int64 j = 0;
            for (Int64 i = 0; i < kLen; i++)
            {
                j = (j + mBox[i] + pass[i % pass.Length]) % kLen;
                Byte temp = mBox[i];
                mBox[i] = mBox[j];
                mBox[j] = temp;
            }
            return mBox;
        }
        #endregion
    }
}