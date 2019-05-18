using Hyt.BLL.Authentication;
using Hyt.BLL.Product;
using Hyt.BLL.Supply;
using Hyt.DataAccess.Product;
using Hyt.DataAccess.Supply;
using Hyt.Model;
using Hyt.Model.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Hyt.BLL.ApiSupply.U1City
{
    public class U1CityProvider : ISupplyProvider
    {
        #region 属性字段
        /// <summary>
        /// 锁对象
        /// </summary>
        private static object lockHelper = new object();
        public override CommonEnum.供应链代码 Code
        {
            get { return CommonEnum.供应链代码.又一城; }
        }
        protected override SupplyInfo Config
        {
            get { return Hyt.BLL.Config.Config.Instance.GetSupplyConfig().SupplyList.FirstOrDefault(s => s.Key == Code.ToString()); }
        }
        #endregion

        #region 函数
        public U1CityProvider() { }
        #endregion

        #region 产品获取
        /// <summary>
        /// 获取商品列表
        /// </summary>
        /// <param name="paraFilte">筛选参数</param>
        /// <returns></returns>
        /// <remarks>2016-3-14 杨浩 添加注释 / 2016-11-03 周 重写</remarks>
        public override Result<string> GetGoodsList(ParaSupplyProductFilter paraFilte = null)
        {

            var result = new Result<string>()
            {
                Status = true,
                StatusCode = 1,
                Message = "获取成功"
            };

            lock (lockHelper)//防止多人同时操作造成产品重复添加
            {
                try
                {

                    result = GetAllGoodsSku();
                    if (result.Status)
                    {
                        var goodsList = result.Data;
                        var products = BLL.Supply.ScProductBo.Instance.GetScProductList((int)Code);

                        //JArray ja = (JArray)JsonConvert.DeserializeObject(goodsList);
                        List<Dto_Pro_Sku_Info> jobInfoList = JsonConvert.DeserializeObject<List<Dto_Pro_Sku_Info>>(goodsList);

                        foreach (Dto_Pro_Sku_Info g in jobInfoList)
                        {
                            var _product = products.FirstOrDefault(x => x.SKU == g.ProductSpec.FirstOrDefault().ProSkuNo);
                            ScProduct product = new ScProduct();
                            if (_product != null)
                            {
                                product = _product;
                            }
                            List<ProductSpecs> ProductSpec = g.ProductSpec;
                            foreach (var o2 in ProductSpec)
                            {
                                product.SKU = o2.ProSkuNo;
                            }
                            
                            product.SupplyCode = (int)Code;
                            product.ProductName = g.ProTitle;
                            product.Receipt = JsonConvert.SerializeObject(g);
                            product.Status = g.ProSale==0?1:2; //1:上架 2:下架
                            product.Tariff = "0";
                            product.Price = g.ProRetPrice;
                            product.Brands = g.ProBrand;
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
        /// <remarks>2016-11-03 周 创建</remarks>
        public override Result<string> GetGoodsSku(string skuid)
        {
            var result = new Result<string>()
            {
                Status = true,
                StatusCode = 1,
                Message = "更新成功"
            };

            lock (lockHelper)//防止多人同时操作造成产品重复添加
            {
                try
                {
                    ///接口参数
                    Dictionary<string, string> date = new Dictionary<string, string>();
                    date.Add("appKey", Config.Account);//AppKey[必填]
                    date.Add("proSkuNo", skuid);//商品Sku[必填]
                    ///调用接口
                    Client client = new Client(Config.GatewayUrl, "IOpenAPI.GetProductSkuInfo", Config.Account, Config.Secert, "json");
                    ///获得接口返回值
                    string sAPIResult = "";
                    try
                    {
                        sAPIResult = client.Post(date);
                    }
                    catch (Exception ex)
                    {
                        result.Message = "第三方平台的Api_Url无效。" + ex.Message;
                        result.Status = false;
                        result.StatusCode = -2;
                        return result;
                    }

                    string ApiCode = Comm.get_JsonValueByName(sAPIResult, "Code");
                    if (ApiCode == "101")
                    {
                        string ApiResult = Comm.get_JsonValueByName(sAPIResult, "Result");
                        List<Pro_Sku_Info> listResult = new List<Pro_Sku_Info>();
                        listResult = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Pro_Sku_Info>>(ApiResult);
                        //var products = BLL.Supply.ScProductBo.Instance.GetScProductList((int)Code);

                        foreach (Pro_Sku_Info g in listResult)
                        {
                            var ScProduct = BLL.Supply.ScProductBo.Instance.GetScProductInfo(g.ProSkuNo, (int)Code);
                            if (ScProduct!=null)
                            {
                                ScProduct Product = Hyt.DataAccess.Supply.IScProductDao.Instance.GetScProductInfo(ScProduct.SysNo);
                                var jsonObject = JObject.Parse(Product.Receipt);
                                //PdProductList PdData = new PdProductList();
                                string ProTitle = g.ProTitle;
                                string ProNo = g.ProNo;
                                string ProBrand = jsonObject["ProBrand"].ToString();
                                string ProClass = jsonObject["ProClass"].ToString();
                                string ProWeight = jsonObject["ProWeight"].ToString();
                                string ProSimg = jsonObject["ProSimg"].ToString();
                                string ProRemark = jsonObject["ProRemark"].ToString();
                                string ProSale = jsonObject["ProSale"].ToString();
                                string Pro_Unit = jsonObject["Pro_Unit"].ToString();
                                string ProTagPrice = jsonObject["ProTagPrice"].ToString();
                                string ProFxPrice = jsonObject["ProFxPrice"].ToString();
                                string ProRetPrice = jsonObject["ProRetPrice"].ToString();
                                string ProCount = g.ProCount.ToString();
                                string ProductSpec = jsonObject["ProductSpec"].ToString();
                                string Receipt = "{\"ProTitle\":\"" + ProTitle + "\",\"ProNo\":\"" + ProNo + "\",\"ProBrand\":\"" + ProBrand + "\",\"ProClass\":\"" + ProClass + "\""
                                                + ",\"ProWeight\":\"" + ProWeight + "\",\"ProSimg\":\"" + ProSimg + "\",\"ProRemark\":\"" + ProRemark + "\",\"ProSale\":\"" + ProSale + "\""
                                                + ",\"Pro_Unit\":\"" + Pro_Unit + "\",\"ProTagPrice\":\"" + ProTagPrice + "\",\"ProFxPrice\":\"" + ProFxPrice + "\",\"ProRetPrice\":\"" + ProRetPrice + "\""
                                                + ",\"ProCount\":\"" + ProCount + "\",\"ProductSpec\":" + ProductSpec + ""
                                                +"}";
                                try
                                {
                                    BLL.Supply.ScProductBo.Instance.UpdateScProduct(g.ProSkuNo, ProTitle, Receipt);
                                }
                                catch (Exception ex)
                                {
                                    result.Message = "更新商品" + ProNo + "出现错误：" + ex.Message;
                                    result.Status = false;
                                    result.StatusCode = -2;
                                    return result;
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
        /// 获取所有商品SKU(有效)
        /// </summary>
        /// <returns></returns>
        /// <remarks>2016-11-03 周 创建</remarks>
        public override Result<string> GetAllGoodsSku()
        {
            var result = new Result<string>()
            {
                Status = false,
                StatusCode = -1,
                Message = "未知错误"
            };
            int count = 1;
            lock (lockHelper)//防止多人同时操作造成产品重复添加
            {
                result.Data = GetAllGoodsInfos(count);
                result.Status = true;
                return result;
            }

        }
        /// <summary>
        /// 获取所有商品
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        /// <remarks>2016-11-03 周 创建</remarks>
        private string GetAllGoodsInfos(int count)
        {
            string html = "[";
            string Data = "";
            ///接口参数
            Dictionary<string, string> date = new Dictionary<string, string>();
            date.Add("appKey", Config.Account);//AppKey[必填]
            date.Add("proNo", "");//商品货号[可填]
            date.Add("proClassCode", "");//商品类别[可填]
            date.Add("proSale", "2");//商品状态[可填] 0：销售中 1：下架 2：全部[默认]
            date.Add("startTime", "");//商品添加开始时间[可填]
            date.Add("endTime", "");//商品添加结束时间[可填]
            date.Add("pageIndex", count.ToString());//页数[必填]
            date.Add("pageSize", "500");//每页数量[必填][数量：10、20、50]
            ///调用接口GetProducts
            Client client = new Client(Config.GatewayUrl, "IOpenAPI.GetProducts", Config.Account, Config.Secert, "json");
            ///获得接口返回值
            string sAPIResult = "";
            try
            {
                sAPIResult = client.Post(date);
            }
            catch (Exception ex)
            {
                Data = "第三方平台的Api_Url无效。" + ex.Message;
            }

            string ApiCode = Comm.get_JsonValueByName(sAPIResult, "Code");
            string PageSumRaws = Comm.get_JsonValueByName(sAPIResult, "SumNum");
            if (ApiCode == "101")
            {
                string ApiResult = Comm.get_JsonValueByName(sAPIResult, "Result");
                int PageCount =int.Parse((int.Parse(PageSumRaws) / 500).ToString())+1;
                for (int i = 1; i <= PageCount;i++)
                {
                    ///接口参数
                    Dictionary<string, string> date2 = new Dictionary<string, string>();
                    date2.Add("appKey", Config.Account);//AppKey[必填]
                    date2.Add("proNo", "");//商品货号[可填]
                    date2.Add("proClassCode", "");//商品类别[可填]
                    date2.Add("proSale", "2");//商品状态[可填] 0：销售中 1：下架 2：全部[默认]
                    date2.Add("startTime", "");//商品添加开始时间[可填]
                    date2.Add("endTime", "");//商品添加结束时间[可填]
                    date2.Add("pageIndex", i.ToString());//页数[必填]
                    date2.Add("pageSize", "500");//每页数量[必填][数量：10、20、50]
                    ///调用接口GetProducts
                    Client client2 = new Client(Config.GatewayUrl, "IOpenAPI.GetProducts", Config.Account, Config.Secert, "json");
                    ///获得接口返回值
                    string sAPIResult2 = "";
                    try
                    {
                        sAPIResult2 = client2.Post(date2);
                        string ApiCode2 = Comm.get_JsonValueByName(sAPIResult2, "Code");
                        {
                            if(ApiCode2=="101")
                            {
                                string ApiResult2 = Comm.get_JsonValueByName(sAPIResult2, "Result");
                                {
                                    ApiResult2 = ApiResult2.Substring(1);
                                    ApiResult2 = ApiResult2.TrimEnd(']');
                                    Data += ApiResult2 + ",";
                                }
                            }
                        }
                        
                    }
                    catch (Exception ex)
                    {
                        Data = "第三方平台的Api_Url无效。" + ex.Message;
                    }
                }
                Data = Data.TrimEnd(',');
                Data += "]";
                html += Data;
            }
           

            return html;
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
                                //PdProductList PdData = new PdProductList();
                                string ErpCode = jsonObject["ProNo"].ToString();//商品编号
                                string ProductName = jsonObject["ProTitle"].ToString();//前台商品名称
                                string EasName = jsonObject["ProTitle"].ToString();//后台商品名称
                                string DefaultImage = jsonObject["ProSimg"].ToString();//默认小图
                                string Remark = jsonObject["ProRemark"].ToString();//详情

                                //商品类目
                                var CategoryName = jsonObject["ProClass"].ToString();
                                PdCategorySql PdCmodel = new PdCategorySql();
                                PdCmodel.ParentSysNo = 0;
                                PdCmodel.CategoryName = CategoryName;
                                PdCmodel.Code = "";
                                PdCmodel.SeoTitle = CategoryName;
                                PdCmodel.SeoKeyword = CategoryName;
                                PdCmodel.SeoDescription = CategoryName;
                                PdCmodel.TemplateSysNo = 0;
                                PdCmodel.IsOnline = 1;
                                PdCmodel.Status = 1;
                                PdCmodel.CreatedBy = 1;
                                PdCmodel.CreatedDate = DateTime.Now;
                                PdCmodel.LastUpdateBy = 1;
                                PdCmodel.LastUpdateDate = DateTime.Now;

                                PdCategoryAssociation PdCAmodel = new PdCategoryAssociation();
                                PdCAmodel.IsMaster = 1;
                                PdCAmodel.CreatedBy = 1;
                                PdCAmodel.CreatedDate = DateTime.Now;
                                PdCAmodel.LastUpdateBy = 1;
                                PdCAmodel.LastUpdateDate = DateTime.Now;
                                
                                //品牌
                                int BrandSysNo;
                                var BrandName = jsonObject["ProBrand"].ToString();
                                PdBrand pEnity = PdBrandBo.Instance.GetEntityByName(BrandName);
                                //判断商品品牌是否存在
                                if (pEnity != null)
                                {
                                    BrandSysNo = pEnity.SysNo;
                                }
                                else
                                {
                                    var pmodel = new PdBrand();
                                    pmodel.Name = BrandName;
                                    pmodel.Status = 1;
                                    BrandSysNo = IPdBrandDao.Instance.Create(pmodel);
                                }

                                string GrosWeight = jsonObject["ProWeight"].ToString();
                                string Price = jsonObject["ProRetPrice"].ToString();
                                var prmodel = SetPriceModel(Decimal.Parse(Price), (int)Hyt.Model.WorkflowStatus.ProductStatus.产品价格来源.基础价格, 0);// new PdPrice();
                                var sprmodel = SetPriceModel(Decimal.Parse(Price), (int)Hyt.Model.WorkflowStatus.ProductStatus.产品价格来源.会员等级价, 1);// new PdPrice();
                                var sspmodel = SetPriceModel(Decimal.Parse(Price), (int)Hyt.Model.WorkflowStatus.ProductStatus.产品价格来源.门店销售价, 0);// new PdPrice();

                                //绑定原产地
                                var orginSysNo = 0;
                                var orginName = "其他";
                                Origin Origin = BLL.Basic.OriginBo.Instance.GetEntityByName(orginName);
                                orginSysNo = Origin != null ? Origin.SysNo : DataAccess.Basic.IOriginDao.Instance.Insert(new Origin() { Origin_Name = orginName, CreatedBy = UserSysNo, CreatedDate = DateTime.Now, LastUpdateBy = UserSysNo, LastUpdateDate = DateTime.Now });

                                var currentInfo = BLL.Authentication.AdminAuthenticationBo.Instance.Current;
                                int agentSysNo = 1;
                                int dealerSysNo = 0;
                                if (currentInfo.Dealer != null)
                                {
                                    dealerSysNo = currentInfo.Dealer.SysNo;
                                    agentSysNo = currentInfo.DealerCreatedBy;
                                }
                                int TradeMode = 0;

                                //获取库存


                                //创建商品
                                var excellst = new List<PdProductList>();
                                var model = new PdProductList
                                {
                                    ErpCode = ErpCode,//商品编号
                                    ProductName = ProductName,//商品名称
                                    EasName = EasName,//商品名称
                                    BrandSysNo = BrandSysNo,//品牌
                                    ProductType = TradeMode,//商品类型（模式）
                                    OriginSysNo = orginSysNo,//国家
                                    Barcode = "",
                                    GrosWeight = Decimal.Parse(GrosWeight),//重量
                                    Tax = "0",
                                    PriceRate = 0M,
                                    PriceValue = 0M,
                                    TradePrice = 0M,
                                    PdPrice = prmodel,
                                    PdSalePrice = sprmodel,
                                    PdStoreSalePrice = sspmodel,
                                    PdCategorySql = PdCmodel,
                                    PdCategoryAssociation = PdCAmodel,
                                    DealerSysNo = dealerSysNo,
                                    AgentSysNo = agentSysNo,//默认为总部代理商
                                    CreatedBy = 1,
                                    CreatedDate = DateTime.Now,
                                    LastUpdateBy = 1,
                                    LastUpdateDate = DateTime.Now,
                                    SalesMeasurementUnit = "g",
                                    ProductDesc = System.Web.HttpUtility.UrlEncode(Remark, System.Text.Encoding.GetEncoding("UTF-8")).Replace("+", " ")//描述
                                };
                                excellst.Add(model);
                                try
                                {
                                    //新增商品
                                    IPdProductDao.Instance.CreatePdProduct(excellst);
                                    //根据编号获取商品
                                    PdProduct Entity = IPdProductDao.Instance.GetEntityByErpCode(ErpCode);
                                    if (Entity != null)
                                    {
                                        string images = jsonObject["ProSimg"].ToString();
                                        var productImage = new PdProductImage();
                                        productImage.ProductSysNo = Entity.SysNo;
                                        productImage.DisplayOrder = 0;
                                        productImage.ImageUrl = images;
                                        productImage.Status = (int)Hyt.Model.WorkflowStatus.ProductStatus.商品图片状态.显示;
                                        int res = IPdProductImageDao.Instance.Insert(productImage);
                                        

                                        //更新商品临时表的ProductSysNo
                                        ScProductBo.Instance.UpdateProductSysNo(SysNo, Entity.SysNo);
                                        //创建商品库存
                                        ScProductBo.Instance.ProCreateSupplyStock((int)Code, Entity.SysNo, decimal.Parse(jsonObject["ProCount"].ToString()), UserSysNo);
                                    }
                                    else
                                    {
                                        result.Message = "获取商品数据错误";
                                        result.Status = false;
                                        return result;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    result.Message = string.Format("数据错误:{0}", ex.Message);
                                    result.Status = false;
                                    return result;
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
        private string Get(string url, IDictionary<string, string> postData)
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

            var strResult = GetResponseGet(url, pStr.ToString());
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
        private string GetResponseGet(string url, string param)
        {
            System.Net.HttpWebRequest request = System.Net.WebRequest.Create(url) as System.Net.HttpWebRequest;
            //request.ContentType = "application/json";
            request.Method = "GET";
            //request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/47.0.2526.111 Safari/537.36";
            System.Net.HttpWebResponse result = request.GetResponse() as System.Net.HttpWebResponse;
            System.IO.StreamReader sr = new System.IO.StreamReader(result.GetResponseStream(), Encoding.UTF8);
            string strResult = sr.ReadToEnd();
            sr.Close();
            //Console.WriteLine(strResult);
            return strResult;
        }

        private PdPrice SetPriceModel(decimal price, int priceSource, int sourceSysNo)
        {
            var model = new PdPrice();
            model.ProductSysNo = 1;
            model.Price = price;
            model.PriceSource = priceSource;
            model.SourceSysNo = sourceSysNo;
            model.Status = 1;
            return model;
        }
        #endregion
    }

}
