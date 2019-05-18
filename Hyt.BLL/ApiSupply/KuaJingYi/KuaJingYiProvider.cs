using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Security.Cryptography;
using Hyt.Model;
using Hyt.Model.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Hyt.BLL.ApiSupply.KuaJingYi
{
    /// <summary>
    /// 跨境翼供应链接口
    /// </summary>
    /// <remarks> Create By 刘伟豪 2016-3-8 </remarks>

    public class KuaJingYiProvider : ISupplyProvider
    {
        #region 属性字段
        public override CommonEnum.供应链代码 Code
        {
            get { return CommonEnum.供应链代码.跨境翼; }
        }
        protected override SupplyInfo Config
        {
            get { return Hyt.BLL.Config.Config.Instance.GetSupplyConfig().SupplyList.FirstOrDefault(s => s.Key == Code.ToString()); }
        }
        private static object lockHelper = new object();
        public KuaJingYiProvider() { }

        #endregion

        #region 商品管理
        /// <summary>
        /// 获取所有商品
        /// </summary>
        /// <remarks>2016-3-8 刘伟豪 实现</remarks>
        public override Result<string> GetGoodsList(ParaSupplyProductFilter paraFilte = null)
        {
            var result = new Result<string>()
            {
                Status = false,
                StatusCode = 0,
                Message = ""
            };

            var pIndex = 1;
            var pSize = 50;
            var pCount = 1;
            //防止并发操作
            lock (lockHelper)
            {
                //循环获取所有分页
                do
                {
                    var postData = new Dictionary<string, string>();
                    postData.Add("appKey", Config.Account);
                    postData.Add("proSale", "0"); //0：销售中，1：下架，2：全部（只获取在售）

                    //分页获取
                    postData.Add("pageIndex", pIndex.ToString());
                    postData.Add("pageSize", pSize.ToString());

                    var responseStr = Post("IOpenAPI.GetProducts", postData);
                    var back = JObject.Parse(responseStr);
                    result = GetResult(back);

                    if (result.Status)
                    {
                        try
                        {
                            var SumNum = int.Parse(back["SumNum"].ToString());
                            pCount = SumNum % pSize > 0 ? SumNum / pSize + 1 : SumNum / pSize;

                            var productList = Hyt.BLL.Supply.ScProductBo.Instance.GetScProductList((int)Code);

                            var goodsList = back["Result"];
                            foreach (var good in goodsList)
                            {
                                var specList = good["ProductSpec"];
                                foreach (var spec in specList)
                                {
                                    var _product = productList.FirstOrDefault(p => p.SKU == spec["ProSkuNo"].ToString());
                                    ScProduct product = new ScProduct();
                                    if (_product != null)
                                    {
                                        product = _product;
                                    }

                                    //商品名
                                    product.ProductName = good["ProTitle"].ToString();
                                    if (!string.IsNullOrWhiteSpace(spec["ProColorName"].ToString()))
                                    {
                                        if (spec["ProColorName"].ToString().Trim() != "无" && spec["ProSizesName"].ToString().Trim() != "无")
                                            product.ProductName += string.Format("( {0}:{1} )", spec["ProColorName"].ToString(), spec["ProSizesName"].ToString());
                                    }
                                    //销售状态  0：销售中，1：下架，2：全部
                                    var stat = int.Parse(good["ProSale"].ToString());
                                    product.Status = stat == 0 ? 1 : 2;
                                    product.SKU = spec["ProSkuNo"].ToString();
                                    product.SupplyCode = (int)Code;
                                    product.Receipt = JsonConvert.SerializeObject(good);
                                    product.Price = decimal.Parse(good["ProFxPrice"].ToString());
                                    product.Brands = good["ProClass"].ToString();

                                    if (_product == null)
                                    {
                                        product.SysNo = BLL.Supply.ScProductBo.Instance.AddScProduct(product);
                                        productList.Add(product);
                                    }
                                    else
                                    {
                                        if (Hyt.BLL.Supply.ScProductBo.Instance.UpdateScProduct(product) && product.ProductSysNo > 0)
                                        {
                                            StockInSupplyProduct(product.SysNo.ToString());
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            result.Status = false;
                            result.StatusCode = 0;
                            result.Message = ex.Message;
                        }
                    }
                    pIndex++;
                }
                while (pIndex <= pCount);
                return result;
            }
        }

        /// <summary>
        /// 获取个别商品
        /// </summary>
        /// <remarks> 2016-5-4 刘伟豪 实现 </remarks>
        public override Result<string> GetGoodsSku(string skuids)
        {
            var result = new Result<string>()
            {
                Status = false,
                StatusCode = -1,
                Message = "未知错误"
            };

            //防止并发操作
            lock (lockHelper)
            {
                try
                {
                    foreach (string skuid in skuids.Split(','))
                    {
                        Dictionary<string, string> postData = new Dictionary<string, string>();
                        postData.Add("appKey", Config.Account);
                        postData.Add("proNo", skuid);
                        string responsestr = Post("IOpenAPI.GetProducts", postData);
                        result.Data = responsestr;
                        var back = JObject.Parse(responsestr);
                        result = GetResult(back);

                        if (result.Status)
                        {
                            var goodsList = back["Result"];
                            foreach (var good in goodsList)
                            {
                                var specList = good["ProductSpec"];
                                foreach (var spec in specList)
                                {
                                    ScProduct product = BLL.Supply.ScProductBo.Instance.GetScProductInfo(spec["ProSkuNo"].ToString(), (int)Code);
                                    if (product != null)
                                    {
                                        //商品名
                                        product.ProductName = good["ProTitle"].ToString();
                                        if (!string.IsNullOrWhiteSpace(spec["ProColorName"].ToString()))
                                        {
                                            if (spec["ProColorName"].ToString().Trim() != "无" && spec["ProSizesName"].ToString().Trim() != "无")
                                                product.ProductName += string.Format("( {0}:{1} )", spec["ProColorName"].ToString(), spec["ProSizesName"].ToString());
                                        }
                                        //销售状态  0：销售中，1：下架，2：全部
                                        var stat = int.Parse(good["ProSale"].ToString());
                                        product.Status = stat == 0 ? 1 : 2;
                                        product.SKU = spec["ProSkuNo"].ToString();
                                        product.SupplyCode = (int)Code;
                                        product.Receipt = JsonConvert.SerializeObject(good);
                                        product.Price = decimal.Parse(good["ProFxPrice"].ToString());
                                        product.Brands = good["ProClass"].ToString();

                                        if (Hyt.BLL.Supply.ScProductBo.Instance.UpdateScProduct(product) && product.ProductSysNo > 0)
                                        {
                                            StockInSupplyProduct(product.SysNo.ToString());
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    result.Message = ex.Message;
                    result.Status = false;
                    result.StatusCode = -2;
                }
                return result;
            }
        }

        /// <summary>
        /// 商品入库到平台
        /// </summary>
        /// <remarks> 2016-5-4 刘伟豪 实现 </remarks>
        public override Result<string> StockInSupplyProduct(string sysNos)
        {
            var result = new Result<string>()
            {
                Status = false,
                StatusCode = -1,
                Message = "未知错误"
            };

            lock (lockHelper)//防止多人同时操作造成产品重复添加
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

                    foreach (var c in sysNos.Split(','))
                    {
                        int id = int.Parse(c);
                        int UserSysNo = Hyt.BLL.Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo;
                        ScProduct Product = BLL.Supply.ScProductBo.Instance.GetScProductInfo(id);
                        //未入库的商品可入库
                        if (Product.ProductSysNo == 0)
                        {
                            using (var tran = new System.Transactions.TransactionScope())
                            {
                                var json = JObject.Parse(Product.Receipt);
                                PdProduct PdData = new PdProduct();

                                //绑定品牌
                                var brandSysNo = 0;
                                var brandName = json["ProClass"].ToString().Trim();
                                PdBrand PdBrand = BLL.Product.PdBrandBo.Instance.GetEntityByName(brandName);
                                brandSysNo = PdBrand != null ? PdBrand.SysNo : DataAccess.Product.IPdBrandDao.Instance.Create(new PdBrand() { Name = brandName, DisplayOrder = 0, Status = (int)Model.WorkflowStatus.ProductStatus.品牌状态.启用 });
                                PdData.BrandSysNo = brandSysNo;

                                //绑定原产地
                                var orginSysNo = 0;
                                var orginName = "其他";
                                Origin Origin = BLL.Basic.OriginBo.Instance.GetEntityByName(orginName);
                                orginSysNo = Origin != null ? Origin.SysNo : DataAccess.Basic.IOriginDao.Instance.Insert(new Origin() { Origin_Name = orginName, CreatedBy = UserSysNo, CreatedDate = DateTime.Now, LastUpdateBy = UserSysNo, LastUpdateDate = DateTime.Now });
                                PdData.OriginSysNo = orginSysNo;

                                PdData.ErpCode = PdData.Barcode = Product.SKU;
                                PdData.EasName = string.Format("({0}产品){1}", Code.ToString(), Product.ProductName);
                                PdData.ProductName = PdData.SeoTitle = PdData.SeoKeyword = PdData.SeoDescription = Product.ProductName;
                                PdData.ViewCount = PdData.DisplayOrder = 0;
                                var weight = 0.00m;
                                decimal.TryParse(json["ProWeight"].ToString(), out weight);
                                PdData.GrosWeight = weight * 0.001m;
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
                                    var stock = GetProSkuStock(Product.SKU);
                                    Hyt.BLL.Supply.ScProductBo.Instance.ProCreateSupplyStock((int)Code, productSysNo, stock, UserSysNo);
                                }
                                tran.Complete();
                            }
                        }
                        else
                        {
                            var PdData = BLL.Product.PdProductBo.Instance.GetProductNoCache(Product.ProductSysNo);
                            var stock = GetProSkuStock(Product.SKU);
                            Hyt.BLL.Supply.ScProductBo.Instance.ProCreateSupplyStock((int)Code, Product.ProductSysNo, stock, UserSysNo);
                        }
                    }
                    result.Message = "入库成功！";
                    result.Status = true;
                    result.StatusCode = 1;
                }
                catch (Exception ex)
                {
                    result.Message = ex.Message;
                    result.Status = false;
                    result.StatusCode = -2;
                }

                return result;
            }
        }

        /// <summary>
        /// 获取商品库存
        /// </summary>
        /// <param name="proSkuNo"></param>
        /// <remarks> 2016-5-5 刘伟豪 创建</remarks>
        private decimal GetProSkuStock(string proSkuNo)
        {
            var stock = 0m;

            Dictionary<string, string> postData = new Dictionary<string, string>();
            postData.Add("appKey", Config.Account);
            postData.Add("proSkuNo", proSkuNo);
            string responsestr = Post("IOpenAPI.GetProductSkuInfo", postData);
            var back = JObject.Parse(responsestr);
            var result = GetResult(back);

            if (result.Status)
            {
                var stockInfo = back["Result"];
                foreach (var s in stockInfo)
                {
                    if (s["ProSkuNo"].ToString() == proSkuNo)
                        decimal.TryParse(s["ProCount"].ToString(), out stock);
                }
            }

            return stock;
        }

        /// <summary>
        /// 获取所有类别
        /// </summary>
        /// <remarks>2016-3-8 刘伟豪 创建</remarks>
        public override Result<string> GetProClass()
        {
            var postData = new Dictionary<string, string>();
            postData.Add("appKey", Config.Account);

            var back = Post("IOpenAPI.GetProClass", postData);
            var result = GetResult(JObject.Parse(back));
            return result;
        }
        public override Result<string> GetAllGoodsSku()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region 订单管理
        public override Result<string> GetShipping()
        {
            throw new NotImplementedException();
        }

        public override Result<string> CancelOrder(int orderSysNo)
        {
            throw new NotImplementedException();
        }

        public override Result<string> CheckOrder(int orderSysNo)
        {
            var result = new Result<string>()
            {
                Status = false,
                StatusCode = 0,
                Message = "该订单未推送"
            };

            lock (lockHelper)//防止多人同时操作造成产品重复添加
            {
                try
                {
                    var orderReturn = BLL.Order.SendOrderReturnBo.Instance.GetEntityByOrderSysNo(orderSysNo);
                    if (orderReturn != null)
                    {
                        var postData = new Dictionary<string, string>();
                        postData.Add("appKey", Config.Account);
                        postData.Add("orderNo", orderReturn.soOrderSysNo.ToString());

                        var responseStr = Post("IOpenAPI.GetOrder", postData);
                        var back = JObject.Parse(responseStr);
                        result = GetResult(back);
                        if (result.Status)
                        {
                            
                        }
                    }
                }
                catch (Exception ex)
                {
                    result.Message = ex.Message;
                    Log.SysLog.Instance.Error(Model.WorkflowStatus.LogStatus.系统日志来源.后台, this.Code + "订单查询：" + ex.Message, ex);
                }

                return result;
            }
        }

        public override Result<string> SendOrder(int orderSysNo)
        {
            var result = new Result<string>()
            {
                Status = false,
                StatusCode = 0,
                Message = ""
            };

            var order = BLL.Order.SoOrderBo.Instance.GetEntity(orderSysNo);
            if (order == null)
            {
                result.Message = "该订单不存在";
            }
            else
            {
                try
                {
                    order.ReceiveAddress = BLL.Order.SoOrderBo.Instance.GetOrderReceiveAddress(order.ReceiveAddressSysNo);
                    order.OrderItemList = DataAccess.Order.ISoOrderItemDao.Instance.GetOrderItemsByOrderSysNo(order.SysNo);

                    // 收货人 区 市 省
                    BsArea receiverDistrict = BLL.Basic.BasicAreaBo.Instance.GetArea(order.ReceiveAddress.AreaSysNo);
                    BsArea receiverCity = BLL.Basic.BasicAreaBo.Instance.GetArea(receiverDistrict.ParentSysNo);
                    BsArea receiverProvince = BLL.Basic.BasicAreaBo.Instance.GetArea(receiverCity.ParentSysNo);
                    var provice = receiverProvince.AreaName.Replace("省", "").Replace("市", "");

                    var postData = new Dictionary<string, string>();
                    postData.Add("appKey", Config.Account);
                    postData.Add("orderNo", order.SysNo.ToString());//网店订单编号[必填]
                    postData.Add("userName", order.CustomerSysNo.ToString());//买家ID[可填]
                    postData.Add("uName", order.ReceiveAddress.Name);//收件人姓名[必填]
                    postData.Add("province", provice);//省份[必填]
                    postData.Add("city", receiverCity.AreaName);//城市[必填]
                    postData.Add("district", receiverDistrict.AreaName);//区域[必填]
                    postData.Add("address", provice + " " + receiverCity.AreaName + " " + receiverDistrict.AreaName + " " + order.ReceiveAddress.StreetAddress);//地址[必填]
                    postData.Add("postcode", "123456");//邮编[必填]
                    postData.Add("mobiTel", order.ReceiveAddress.MobilePhoneNumber);//手机号码[可填][注：手机号码和电话号码至少填一项]
                    //postData.Add("phone", order.ReceiveAddress.PhoneNumber);//电话号码[可填][注：手机号码和电话号码至少填一项]
                    postData.Add("cRemark", order.ReceiveAddress.IDCardNo);//身份信息
                    //postData.Add("oRemark", order.InternalRemarks);//卖家备注[可填]
                    postData.Add("oSumPrice", order.OrderAmount.ToString());//实付订单总金额[必填]
                    //postData.Add("expFee", order.FreightAmount.ToString());//实付订单运费[可填][默认为0]
                    postData.Add("expCod", "0");//是否货到付款[必填][1：货到付款]
                    //postData.Add("codFee", "0");//货到付款手续费[可填][默认为0]
                    //postData.Add("expCodFee", "0");//货到付款代收运费[可填][默认为0]
                    //postData.Add("payTime", "2015-04-20 09:44:44");//订单支付日期
                    //postData.Add("paymententerprise", "");//订单支付企业
                    //postData.Add("paymentno", "");//订单支付流水号

                    List<Api_OrderProInfo> postOrderItemList = new List<Api_OrderProInfo>();
                    foreach (var item in order.OrderItemList)
                    {
                        var scProduct = BLL.Supply.ScProductBo.Instance.GetScProductInfo(item.ProductSysNo, (int)Code);
                        if (scProduct != null)
                        {
                            var scObj = JObject.Parse(scProduct.Receipt);
                            var model = new Api_OrderProInfo()
                            {
                                proNo = scObj["ProNo"].ToString(),
                                proSku = scProduct.SKU,
                                proTitle = scProduct.ProductName,
                                proCount = item.Quantity.ToString(),
                                proPrice = item.SalesUnitPrice.ToString()
                            };
                            postOrderItemList.Add(model);
                        }
                    }
                    postData.Add("OrderPro", JsonConvert.SerializeObject(postOrderItemList));

                    var responseStr = Post("IOpenAPI.AddOrder", postData);
                    var back = JObject.Parse(responseStr);
                    result = GetResult(back);
                    if (result.Status)
                    {
                        try
                        {
                            using (var tran = new System.Transactions.TransactionScope())
                            {
                                SendOrderReturn m = new SendOrderReturn();
                                m.OverseaCarrier = "";
                                m.OverseaTrackingNo = "";
                                m.soOrderSysNo = orderSysNo;
                                m.Code = "1";
                                m.Msg = JsonConvert.SerializeObject(back);
                                m.OrderNo = back["Result"].ToString();
                                Hyt.BLL.Order.SoOrderBo.Instance.InsertSendOrderReturn(m, Hyt.BLL.Authentication.AdminAuthenticationBo.Instance.Current.Base);
                                //更新订单推送状态
                                Hyt.BLL.Order.SoOrderBo.UpdateOrderSendStatus(orderSysNo, (int)Hyt.Model.WorkflowStatus.OrderStatus.销售单推送状态.已推送);
                                result.Message = "推送成功";
                                tran.Complete();
                            }
                        }
                        catch (Exception ex)
                        {
                            result.Status = false;
                            result.Message = ex.Message;
                            BLL.Log.SysLog.Instance.Error(Model.WorkflowStatus.LogStatus.系统日志来源.后台, this.Code + "订单推送：" + ex.Message, ex);
                        }
                    }
                }
                catch (Exception ex)
                {
                    result.Message = ex.Message;
                    BLL.Log.SysLog.Instance.Error(Model.WorkflowStatus.LogStatus.系统日志来源.后台, this.Code + "订单推送：" + ex.Message, ex);
                }
            }
            return result;
        }

        /// <summary>
        /// 订单项
        /// </summary>
        /// <remarks> 2016-5-5 刘伟豪 创建</remarks>
        private class Api_OrderProInfo
        {
            /// <summary>
            /// 商品货号
            /// </summary>
            public string proNo { get; set; }
            /// <summary>
            /// 商品标题
            /// </summary>
            public string proTitle { get; set; }
            /// <summary>
            /// 商品数量
            /// </summary>
            public string proCount { get; set; }
            /// <summary>
            /// 商品价格
            /// </summary>
            public string proPrice { get; set; }
            /// <summary>
            /// 商品sku
            /// </summary>
            public string proSku { get; set; }
        }
        #endregion

        #region 私有方法
        /// <summary>
        /// 检验获取结果
        /// </summary>
        /// <param name="back"></param>
        /// <remarks>2016-3-18 刘伟豪 创建</remarks>
        private Result<string> GetResult(JObject back)
        {
            var result = new Result<string>
            {
                Status = false,
                StatusCode = 0,
                Message = "未知错误"
            };

            try
            {
                // 101：成功，102：失败，103：系统异常，104：找不到相关数据
                if (back.Property("Code") != null)
                {
                    result.Message = back["Message"].ToString();
                    if (back["Code"].ToString() == "101")
                    {
                        result.Status = true;
                        result.StatusCode = 1;
                        result.Data = back.ToString();
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
        /// <param name="postData"></param>
        /// <remarks>2016-3-8 刘伟豪 创建</remarks>
        private string Post(string method, IDictionary<string, string> postData, string _format = "json")
        {
            StringBuilder pStr = new StringBuilder();

            pStr.Append("user").Append("=").Append(Uri.EscapeDataString(Config.Account));
            pStr.Append("&").Append("method").Append("=").Append(Uri.EscapeDataString(method));
            if (!string.IsNullOrEmpty(_format))
            {
                pStr.Append("&").Append("format").Append("=").Append(Uri.EscapeDataString(_format));
            }

            string str = method + Config.Secert;
            foreach (var item in postData)
            {
                str += item.Key;
                str += item.Value;

                pStr.Append("&").Append(item.Key).Append("=").Append(Uri.EscapeDataString(item.Value));
            }
            string strAsc = Asc(str.Replace(" ", "").ToLower());
            string token = Encrypt_MD5(strAsc);

            pStr.Append("&").Append("token").Append("=").Append(Uri.EscapeDataString(token));

            var strResult = GetResponse(Config.GatewayUrl, pStr.ToString());
            return strResult;
        }

        private bool CheckValidationResult(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificate, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors errors)
        {
            return true; //总是接受     
        }
        /// <summary>
        /// 调用接口传递参数
        /// </summary>
        /// <param name="url">网关地址</param>
        /// <param name="param">传递参数</param>
        /// <param name="Method">传递方式：Post/Get</param>
        /// <remarks> Create By Lwh 2016-3-8 </remarks>
        private string GetResponse(string url, string param)
        {
            byte[] postData = Encoding.UTF8.GetBytes(param);
            ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(CheckValidationResult);
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url);
            req.Method = "POST";
            req.KeepAlive = true;
            req.Timeout = 300000;
            req.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
            req.ContentLength = postData.Length;
            req.Headers.Add("api-version", "2.0");

            Stream reqStream = req.GetRequestStream();

            reqStream.Write(postData, 0, postData.Length);

            reqStream.Close();

            HttpWebResponse rsp = null;
            try
            {
                rsp = (HttpWebResponse)req.GetResponse();
            }
            catch (WebException ex)
            {
                rsp = (HttpWebResponse)ex.Response;
            }

            Stream stream = rsp.GetResponseStream();

            StreamReader sr = new StreamReader(stream, Encoding.GetEncoding("utf-8"));
            string result = sr.ReadToEnd();
            sr.Close();
            stream.Close();

            return result;
        }
        #endregion

        
    }
}