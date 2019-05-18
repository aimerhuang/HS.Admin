using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using Hyt.Model;
using Hyt.Model.Common;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Hyt.DataAccess.Supply;
using Hyt.BLL.Authentication;
using Hyt.BLL.Product;
using Hyt.BLL.Supply;

namespace Hyt.BLL.ApiSupply.QianhaiSail
{
    /// <summary>
    /// 前海洋行供应链接口
    /// </summary>
    /// <remarks> Create By 刘伟豪 2016-3-9 </remarks>
    public class QianhaiSailProvider : ISupplyProvider
    {
        #region 属性字段
        /// <summary>
        /// 锁对象
        /// </summary>
        private static object lockHelper = new object();
        public override CommonEnum.供应链代码 Code
        {
            get { return CommonEnum.供应链代码.前海洋行; }
        }
        protected override SupplyInfo Config
        {
            get { return Hyt.BLL.Config.Config.Instance.GetSupplyConfig().SupplyList.FirstOrDefault(s => s.Key == Code.ToString()); }
        }
        #endregion

        #region 函数
        public QianhaiSailProvider() { }
        #endregion
     
        #region 产品获取
        /// <summary>
        /// 获取商品列表
        /// </summary>
        /// <param name="paraFilte">筛选参数</param>
        /// <returns></returns>
        /// <remarks>2016-3-14 杨浩 添加注释</remarks>
        public override Result<string> GetGoodsList(ParaSupplyProductFilter paraFilte = null)
        {

            var result = new Result<string>()
            {
                Status = false,
                StatusCode =-1,
                Message = "未知错误"
            };
           
            lock (lockHelper)//防止多人同时操作造成产品重复添加
            {
                try
                {
                    string responsestr = Post(Config.GatewayUrl + "?c=vendor&a=getShopList", null);
                    result.Data = responsestr;
                    var back = JObject.Parse(responsestr);
                    result = GetResult(result, back);
                    if (result.Status)
                    {
                        var goodsList = back["data"]["list"];
                        var products = BLL.Supply.ScProductBo.Instance.GetScProductList((int)Code);

                        foreach (var g in goodsList)
                        {
                            //略过已下架，按id查询时，已下架的商品接口不返回，故略过
                            if (g["status"].ToString() == "2")
                                continue;

                            var _product = products.FirstOrDefault(x => x.SKU == g["skuid"].ToString());
                            ScProduct product = new ScProduct();
                            if (_product != null)
                            {
                                product = _product;
                            }

                            product.SKU = g["skuid"].ToString();
                            product.SupplyCode = (int)Code;
                            product.ProductName = g["goodsname"].ToString();
                            product.Receipt = JsonConvert.SerializeObject(g);
                            product.Status = int.Parse(g["status"].ToString()); //1:上架 2:下架
                            product.Tariff = g["cess"].ToString();
                            product.Price = decimal.Parse(g["price"].ToString());

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
                    result.Status = false;
                    result.StatusCode = -2;
                }
                return result;
            }            
        }
        /// <summary>
        /// 获取单个sku
        /// </summary>
        /// <returns></returns>
        /// <remarks>2016-3-18 杨浩 添加注释</remarks>
        public override Result<string> GetGoodsSku(string skuid)
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
                    Dictionary<string, string> postData = new Dictionary<string, string>();
                    postData.Add("skuid", skuid);
                    string responsestr = Post(Config.GatewayUrl + "?c=vendor&a=getSkuStore", postData);
                    result.Data = responsestr;
                    var back = JObject.Parse(responsestr);
                    result = GetResult(result, back);
                    if (result.Status)
                    {
                        var goodsList = back["data"]["list"];
                        foreach (var g in goodsList)
                        {
                            ScProduct product = BLL.Supply.ScProductBo.Instance.GetScProductInfo(g["skuid"].ToString(), (int)Code);

                            if (product != null)
                            {
                                product.SKU = g["skuid"].ToString();
                                product.SupplyCode = (int)Code;
                                product.ProductName = g["goodsname"].ToString();
                                product.Receipt = JsonConvert.SerializeObject(g);
                                //product.Status = int.Parse(g["status"].ToString()); //1:上架 2:下架
                                product.Tariff = g["cess"].ToString();
                                product.Price = decimal.Parse(g["price"].ToString());
                                BLL.Supply.ScProductBo.Instance.UpdateScProduct(product);
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
        /// 入库个别商品信息
        /// </summary>
        /// <param name="supplyCode">供应链代码</param>
        /// <param name="sysNos">商品系统编号，逗号分隔</param>
        /// <returns></returns>
        /// <remarks>2016-4-22 王耀发 创建</remarks> 
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

                    string[] sysNoArray = sysNos.Split(',');
                    int UserSysNo = AdminAuthenticationBo.Instance.Current.Base.SysNo;
                    for (var i = 0; i < sysNoArray.Length; i++)
                    {
                        int SysNo = int.Parse(sysNoArray[i]);
                        ScProduct Product = IScProductDao.Instance.GetScProductInfo(SysNo);
                        //未入库的商品可入库
                        if (Product.ProductSysNo == 0)
                        {
                            using (var tran = new System.Transactions.TransactionScope())
                            {
                                var jsonObject = JObject.Parse(Product.Receipt);
                                PdProduct PdData = new PdProduct();
                                PdData.ErpCode = jsonObject["skuid"].ToString();
                                PdData.ProductName = jsonObject["goodsname"].ToString();
                                PdData.EasName = jsonObject["goodsname"].ToString();
                                if (jsonObject["barcode"] != null)
                                    PdData.Barcode = jsonObject["barcode"].ToString();
                                if (jsonObject["cess"] != null)
                                    PdData.Tax = decimal.Parse(jsonObject["cess"].ToString());
                                PdData.CanFrontEndOrder = 1;
                                PdData.AgentSysNo = 1;
                                PdData.DealerSysNo = 0;
                                //默认品牌为其他
                                var brandSysNo = 0;
                                var brandName = "其他";
                                PdBrand PdBrand = BLL.Product.PdBrandBo.Instance.GetEntityByName(brandName);
                                brandSysNo = PdBrand != null ? PdBrand.SysNo : DataAccess.Product.IPdBrandDao.Instance.Create(new PdBrand() { Name = brandName, DisplayOrder = 0, Status = 1 });
                                PdData.BrandSysNo = brandSysNo;
                                //绑定原产地
                                var orginSysNo = 0;
                                var orginName = "其他";
                                Origin Origin = BLL.Basic.OriginBo.Instance.GetEntityByName(orginName);
                                orginSysNo = Origin != null ? Origin.SysNo : DataAccess.Basic.IOriginDao.Instance.Insert(new Origin() { Origin_Name = orginName, CreatedBy = UserSysNo, CreatedDate = DateTime.Now, LastUpdateBy = UserSysNo, LastUpdateDate = DateTime.Now });
                                PdData.OriginSysNo = orginSysNo;

                                PdData.CreatedBy = UserSysNo;
                                PdData.CreatedDate = DateTime.Now;
                                PdData.LastUpdateBy = UserSysNo;
                                PdData.LastUpdateDate = DateTime.Now;
                                //创建商品
                                int ProductSysNo = PdProductBo.Instance.CreateProduct(PdData);
                                if (ProductSysNo > 0)
                                {
                                    //创建商品基础价
                                    PdPrice BasicModel = new PdPrice();
                                    BasicModel.ProductSysNo = ProductSysNo;
                                    BasicModel.Price = decimal.Parse(jsonObject["price"].ToString());
                                    BasicModel.PriceSource = 0;
                                    BasicModel.SourceSysNo = 0;
                                    BasicModel.Status = 1;
                                    PdPriceBo.Instance.Create(BasicModel);
                                    //创建商品会员价
                                    PdPrice SaleModel = new PdPrice();
                                    SaleModel.ProductSysNo = ProductSysNo;
                                    SaleModel.Price = decimal.Parse(jsonObject["price"].ToString());
                                    SaleModel.PriceSource = 10;
                                    SaleModel.SourceSysNo = 1;
                                    SaleModel.Status = 1;
                                    PdPriceBo.Instance.Create(SaleModel);
                                    //更新商品临时表的ProductSysNo
                                    ScProductBo.Instance.UpdateProductSysNo(SysNo, ProductSysNo);
                                    //创建商品库存
                                    ScProductBo.Instance.ProCreateSupplyStock((int)Code, ProductSysNo, decimal.Parse(jsonObject["number"].ToString()), UserSysNo);
                                }
                                tran.Complete();
                            }
                        }
                        result.Status = true;
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
                        var orderStr = orderReturn.OrderNo;
                        Dictionary<string, string> postData = new Dictionary<string, string>();
                        postData.Add("order_number", orderStr);

                        string backValue = Post(Config.GatewayUrl + "?c=vendor&a=getOrderStatus", postData);
                        result.Status = true;
                        result.StatusCode = 1;
                        result.Message = backValue;
                    }
                }
                catch (Exception ex)
                {
                    result.Status = false;
                    result.StatusCode = -2;
                    result.Message = ex.Message;
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

            lock (lockHelper)//防止多人同时操作造成产品重复添加
            {
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

                        Dictionary<string, string> postData = new Dictionary<string, string>();

                        //下单商品 sku及购买数量，sku和购买数量用逗号隔开，多件商品时用分号隔开（如： test001,1;test002,1）
                        //B0230115,2;B0230131,3
                        var skuinfo = "";
                        foreach (var item in order.OrderItemList)
                        {
                            ScProduct scProduct = BLL.Supply.ScProductBo.Instance.GetScProductInfo(item.ProductSysNo, (int)Code);
                            skuinfo += string.Format("{0},{1};", scProduct.SKU, item.Quantity);
                        }
                        skuinfo = skuinfo.TrimEnd(';');

                        postData.Add("skuinfo", skuinfo);
                        postData.Add("consi_name", order.ReceiveAddress.Name);//收货人姓名
                        postData.Add("consi_phone", order.ReceiveAddress.MobilePhoneNumber);//收货人手机号码
                        postData.Add("consi_card", order.ReceiveAddress.IDCardNo);//收货人身份证
                        postData.Add("consi_provinc", receiverProvince.AreaName);//收货人所在省
                        postData.Add("consi_city", receiverCity.AreaName);//收货人所在城市
                        postData.Add("consi_county", receiverDistrict.NameAcronym);//收货人所在县
                        postData.Add("consi_address", order.ReceiveAddress.StreetAddress);//收货人所在地址
                        postData.Add("consi_freight", order.FreightAmount.ToString());//运费
                        postData.Add("consi_price", order.OrderAmount.ToString());//订单金额
                        //（非必填）torder 第三方订单号
                        //（非必填）paytype 支付方式（1：快付通，2：快钱，3：支付宝，4：微信，5：易极付，6：招商银行）
                        //（非必填）pay_tradeno 支付流水号
                        postData.Add("remark", "平台推单");//订单备注信息

                        string responsestr = Post(Config.GatewayUrl + "?c=vendor&a=addOrder", postData);
                        result.Data = responsestr;
                        var back = JObject.Parse(responsestr);
                        result = GetResult(result, back);
                        if (result.Status)
                        {
                            //返回结果保存到SendOrderReturn表中 2016-4-26 王耀发 创建
                            SendOrderReturn m = new SendOrderReturn();
                            m.soOrderSysNo = orderSysNo;
                            m.Code = "1";
                            m.Msg = responsestr;
                            m.OrderNo = back["data"]["order_number"].ToString();
                            Hyt.BLL.Order.SoOrderBo.Instance.InsertSendOrderReturn(m, AdminAuthenticationBo.Instance.Current.Base);
                            //更新订单的商检推送状态
                            Hyt.BLL.Order.SoOrderBo.UpdateOrderSendStatus(orderSysNo, (int)Hyt.Model.WorkflowStatus.OrderStatus.销售单推送状态.已推送);
                        }
                    }
                    catch (Exception ex)
                    {
                        result.Status = false;
                        result.StatusCode = -2;
                        result.Message = ex.Message;
                    }
                }
                return result;
            }
        }

        public override Result<string> GetAllGoodsSku()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region 私有方法
        /// <summary>
        /// 获取响应结果
        /// </summary>
        /// <param name="back">响应数据</param>
        /// <param name="result">返回结果</param>
        /// <returns></returns>
        /// <remarks>2016-3-18 杨浩 创建</remarks>
        private Result<string> GetResult(Result<string> result, JObject back)
        {

            if (back.Property("status") != null && back["status"].ToString() == "1")
            {
                result.Status = true;
                result.StatusCode = 1;
                result.Message = "请求成功";
            }
            else if (back.Property("status") != null && back["status"].ToString() == "2")
            {
                result.StatusCode = 2;
                result.Message = "暂无数据";
            }
            else if (back.Property("status") != null && back["status"].ToString() == "10")
            {
                result.StatusCode = 10;
                result.Message = "请求参数错误";
            }

            return result;
        }
        private string Post(string url, IDictionary<string, string> postData)
        {
            StringBuilder pStr = new StringBuilder();

            pStr.Append("key").Append("=").Append(Config.Secert).Append("&").Append("userid").Append("=").Append(Config.Account);
            if (postData != null)
            {
                foreach (var item in postData)
                {
                    pStr.Append("&").Append(item.Key).Append("=").Append(item.Value);
                }
            }

            var strResult = GetResponse(url, pStr.ToString());
            return strResult;
        }

        private string GetResponse(string url, string param)
        {
            byte[] postData = Encoding.UTF8.GetBytes(param);
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url);
            req.Method = "POST";
            req.KeepAlive = true;
            req.Timeout = 300000;
            req.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
            req.ContentLength = postData.Length;
            req.Headers.Add("api-version", "2.0");
            req.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
            req.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/47.0.2526.111 Safari/537.36";

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