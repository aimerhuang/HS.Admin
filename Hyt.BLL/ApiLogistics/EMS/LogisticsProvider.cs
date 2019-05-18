using Hyt.Model;
using Hyt.Model.Transfer;
using Hyt.Model.Common;
using Hyt.Model.Logis.XinYi;
using Hyt.Model.Manual;
using Hyt.Model.WorkflowStatus;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Hyt.BLL.ApiLogistics.EMS
{
    /// <summary>
    /// 广州EMS物流接口
    /// </summary>
    /// <remarks>2016-3-17 杨浩 创建</remarks>
    /// <remarks>2016-3-17 陈海裕 修改</remarks>
    public class LogisticsProvider : ILogisticsProvider
    {
        const string OnNumber = "12386818";
        const string WhNumber = "STORE_GZNS";
        const string CopGNo = "HT-B74-000009";
        const string skucode = "HT-B74-000009-001";
        /// <summary>
        /// API协议版本
        /// </summary>
        private string v = "1.0"; 
        /// <summary>
        /// 格式化
        /// </summary>
        private string format = "json";
        /// <summary>
        /// api服务url
        /// </summary>
        private string apiUrl = "http://wtd.nat123.net/wtdex-service/ws/openapi/rest/route";

        public LogisticsProvider() { }

        /// <summary>
        /// Base64加密
        /// </summary>
        /// <param name="source">待加密的明文</param>
        /// <returns></returns>
        /// <remarks>2016-3-8 杨浩 创建</remarks>
        private string EncodeBase64(string source)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(source);
            return Convert.ToBase64String(bytes);
        }

        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="param">需要加密的参数</param>
        /// <returns></returns>
        /// <remarks>2016-3-10 杨浩 创建</remarks>
        private string MD5(string param)
        {
            //传输参数前处理
            byte[] text = Encoding.UTF8.GetBytes(param);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] output = md5.ComputeHash(text);
            return BitConverter.ToString(output).Replace("-", "").ToLower();
        }


        /// <summary>
        /// 获得签名
        /// a)	按照如下顺序拼成字符串：
        ///     secret+appkey+format+ method + params+ timestamp + tocken+ v + secret
        /// b)	使用md5加密
        ///     字符串要保证为utf-8格式的，并使用md5算法签名(结果是小写的)
        /// c)	Base64加密
        ///     将md5加密结果再使用Base64加密，该字符串即为sign。
        /// </summary>
        /// <param name="method">api方法名称</param>
        /// <param name="_params">参数</param>
        /// <param name="timestamp">时间戳</param>
        /// <returns></returns>
        /// <remarks>2016-3-8 杨浩 创建</remarks>
        private string GetSign(string method, string _params, string timestamp)
        {
            string sign = config.Secret + config.AppKey + format + method + _params + timestamp + config.Token + config.Secret;
            sign = MD5(sign);
            sign = EncodeBase64(sign);
            return sign;
        }
        /// <summary>
        /// 初始化api
        /// </summary>
        /// <returns></returns>
        /// <param name="method">api方法名称</param>
        /// <param name="_params">参数</param>
        /// <param name="timestamp">时间戳</param>
        /// <remarks>2016-3-8 杨浩 创建</remarks>
        private string InitParams(string method, string _params, string timestamp)
        {
            string url = "appkey=" + config.AppKey + "&sign=" + GetSign(method, _params, timestamp) + "&token=" + config.Token + "&timestamp=" + timestamp + "&v=" + v + "&format=" + format + "&method=" + method + "&params=" + System.Web.HttpUtility.UrlEncode(_params, System.Text.UTF8Encoding.UTF8);
            return url;
        }

        /// <summary>
        /// 物流标示
        /// </summary>
        /// <remarks>2016-3-8 杨浩 创建</remarks>
        public override Hyt.Model.CommonEnum.物流代码 Code
        {
            get { return Hyt.Model.CommonEnum.物流代码.广州EMS; }
        }

        /// <summary>
        /// 获取回调结果
        /// </summary>
        /// <param name="result">api反馈结果</param>
        /// <returns></returns>
        ///  <remarks>2016-3-8 杨浩 创建</remarks>
        private Result GetResponseResult(string responseStr)
        {
            var result = new Result()
            {
                Status = false
            };
            var jobject = JObject.Parse(responseStr);
            if (jobject.Property("response") != null)
            {
                var response = jobject["response"];
                string code = response["code"].ToString();
                if (code == "1")
                    result.Status = true;
            }
            else
            {
                result.Message = responseStr;
            }

            return result;
        }

        ///// <summary>
        ///// 添加产品
        ///// </summary>
        ///// <param name="product">产品</param>
        ///// <remarks>2016-3-8 杨浩 创建</remarks>
        //public override Result AddProduct(PdProduct product)
        //{
        //    string _params = "{\"barcode\":\"20014011\",\"brand\":\"宝洁\",\"categoryName\":\"paper\",\"channel\":\"www.jd.com\",\"childProductList\":{\"childProducts\":[]},\"currency\":\"cny\",\"foreign\":\"china\",\"grossWt\":1,\"imageUrl\":null,\"model\":\"大中型\",\"netWt\":1,\"notes\":null,\"origin\":\"gz\",\"prodAttributeInfo\":\"11:22,33:44\",\"productId\":\"1001\",\"productName\":\"帮宝适纸内裤\",\"quality\":\"perfect\",\"salePrice\":100,\"saleUnits\":\"CNY\",\"unitPrice\":200, \" menuFact\":\"中国\"}";

        //    string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        //    string method = "wtdex.prod.product.add";

        //    _params = InitParams(method, _params, timestamp);

        //    var _result = Hyt.Util.WebUtil.PostForm(apiUrl, _params);
        //    return GetResponseResult(_result);
        //}
        /// <summary>
        /// 获取产品详情
        /// </summary>
        /// <param name="productId">商品编码</param>
        /// <returns></returns>
        public override Result GetProduct(string productId)
        {
            string _params = "";
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string method = "wtdex.prod.product.get";
            _params = InitParams(method, _params, timestamp);

            var _result = Hyt.Util.WebUtil.PostForm(apiUrl, _params);
            return GetResponseResult(_result);
        }
      
        ///// <summary>
        ///// 推送商品
        ///// </summary>
        ///// <param name="product"></param>
        ///// <returns></returns>
        //public override Result PushProduct(PdProduct product)
        //{
        //    Result result = new Result();
        //    Goods goods = new Goods();
        //    goods.WarehouseCode = WhNumber;
        //    goods.OnNumber = OnNumber;
        //    goods.GNo = product.SysNo.ToString();
        //    goods.CopGNo = product.SysNo.ToString();
        //    goods.GName = product.ProductName;
        //    goods.GModel = product.Volume;
        //    goods.BARCode = product.Barcode;
        //    goods.Notes = product.ProductSummary;
        //    goods.Unit = "007";
        //    goods.GoodsMes = product.ProductDeclare;
        //    // goods.OpType = product.IsPushLogistics == 0 ? "新增" : "修改";
        //    goods.ShortName = product.ProductShortTitle;
        //    //goods.Manufactory = product.Manufactory;
        //    goods.Brand = ((CBPdProduct)product).BrandName;
        //    goods.Original = "110";//((CBPdProduct)product).Original;
        //    // goods.PurchasePlace = ((CBPdProduct)product).Original;
        //    goods.GrossWt = product.GrosWeight;
        //    goods.NetWt = product.NetWeight;
        //    goods.CodeTS = product.ErpCode;
        //    goods.DecPrice = (int)((CBPdProduct)product).PdPrice.Value.First(p => p.PriceSource == 0).Price * 100;
        //    goods.IEFlag = "I";
        //    goods.GNote = product.PackageDesc;
        //    List<Goods> goodsList = new List<Goods>();
        //    goodsList.Add(goods);
        //    var jsonData = new { Goods = goodsList };
        //    var jSetting = new Newtonsoft.Json.JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
        //    string json = JsonConvert.SerializeObject(jsonData, Formatting.None, jSetting);

        //    using (var service = new Hyt.Infrastructure.Communication.ServiceProxy<Hyt.Service.Contract.LogisApp.ILogistics>())
        //    {
        //        // result = service.Channel.PushProduct(json);

        //    }

        //    return result;
        //}
        ///// <summary>
        ///// 推送订单
        ///// </summary>
        ///// <param name="stockIn"></param>
        ///// <param name="itemList"></param>
        ///// <returns></returns>
        //public override Result PushInOrder(PdProductStockIn stockIn, List<PdProductStockInDetailList> itemList)
        //{
        //    Result result = new Result();
        //    OrderHead headData = new OrderHead();
        //    headData.OnNumber = OnNumber;
        //    headData.WhNumber = WhNumber;
        //    headData.RoNumber = stockIn.SysNo.ToString();
        //    headData.OrderDate = stockIn.CreatedDate.ToString("yyyy-MM-dd HH:mm:ss");

        //    List<OrderDetail> detailList = new List<OrderDetail>();
        //    int indx = 0;
        //    foreach (PdProductStockInDetailList item in itemList)
        //    {
        //        indx++;
        //        detailList.Add(new OrderDetail()
        //        {
        //            RowNum = indx,
        //            RoNumber = stockIn.SysNo.ToString(),
        //            CopGNo = item.PdProductSysNo.ToString(),
        //            GoodsBatchNo = stockIn.StockInNo,
        //            SkuCode = item.PdProductSysNo.ToString(),
        //            Qty = (int)(item.StorageQuantity),
        //            // ProPrice = (int)(item.Price*100)
        //        });
        //    }
        //    List<OrderHead> headDataList = new List<OrderHead>();
        //    headDataList.Add(headData);
        //    var jsonData = new { OrderHead = headDataList, OrderDetail = detailList };
        //    var jSetting = new Newtonsoft.Json.JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
        //    string json = JsonConvert.SerializeObject(jsonData, Formatting.None, jSetting);

        //    using (var service = new Hyt.Infrastructure.Communication.ServiceProxy<Hyt.Service.Contract.LogisApp.ILogistics>())
        //    {
        //        // result = service.Channel.PushInOrder(json);

        //    }

        //    return result;
        //}


        public override Result AddOrderTrade(int orderSysno)
        {
            throw new NotImplementedException();
        }

        public override Result GetOrderExpressno(string orderId)
        {
            throw new NotImplementedException();
        }

        public override Result GetOrderTrade(string orderId)
        {
            throw new NotImplementedException();
        }

        public override Result<string> GetLogisticsTracking(int orderSysNo)
        {
            throw new NotImplementedException();
        }

        public override Result CancelOrderTrade(int orderSysNo, string reason = "")
        {
            throw new NotImplementedException();
        }
    }
}
