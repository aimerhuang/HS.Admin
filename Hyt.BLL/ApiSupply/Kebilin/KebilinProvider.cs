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

namespace Hyt.BLL.ApiSupply.Kebilin
{
    /// <summary>
    /// 客比邻接口
    /// </summary>
    /// <remarks>2016-2-23 杨浩 创建</remarks>
    /// <remarks>2016-3-14 17:54 刘伟豪 重构</remarks>
    public class KebilinProvider : ISupplyProvider
    {
        public override CommonEnum.供应链代码 Code
        {
            get { return CommonEnum.供应链代码.客比邻; }
        }
        protected override SupplyInfo Config
        {
            get { return Hyt.BLL.Config.Config.Instance.GetSupplyConfig().SupplyList.FirstOrDefault(s => s.Key == Code.ToString()); }
        }
        private static object lockHelper = new object();

        /// <summary>
        /// 客比邻传递参数头部
        /// </summary>
        /// <remarks>2016-3-15 11:58 刘伟豪 创建</remarks>
        private class KebilinHead
        {
            public string userid { get; set; }
            public string timestamp { get; set; }
            public string sign { get; set; }
            public string f { get; set; }
        }
        /// <summary>
        /// 客比邻传递参数
        /// </summary>
        /// <remarks>2016-3-15 11:58 刘伟豪 创建</remarks>
        private class KebilinData
        {
            public KebilinHead head = new KebilinHead();
            public object body { get; set; }
        }
        private KebilinData data = new KebilinData();

        public KebilinProvider()
        {
            var stamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            var sign = base.Encrypt_MD5(Config.Account + Config.Secert + stamp);

            data.head.userid = Config.Account;
            data.head.timestamp = stamp;
            data.head.sign = sign;
        }

        /// <summary>
        /// 获取所有商品
        /// </summary>
        /// <remarks>2016-3-14 刘伟豪 创建</remarks>
        public override Result<string> GetGoodsList(ParaSupplyProductFilter paraFilte = null)
        {
            //调用方法名
            data.head.f = "get_goods_list";
            //传递参数主题
            string page_no = "", page_size = "";
            if (paraFilte != null)
            {
                page_no = paraFilte.Id.ToString();
                page_size = paraFilte.PageSize.ToString();
            }
            data.body = new { serial = "", page_no = page_no, page_size = page_size, starttime = "", endtime = "" };

            //防止并发
            lock (lockHelper)
            {
                var responseStr = Post(data);
                var back = JObject.Parse(responseStr);
                var result = GetResult(back);

                if (result.Status)
                {
                    try
                    {
                        var productList = Hyt.BLL.Supply.ScProductBo.Instance.GetScProductList((int)Code);

                        var goodList = back["root"]["body"];
                        foreach (var good in goodList)
                        {
                            var sku = good["skus"]["sku"];

                            var _product = productList.FirstOrDefault(p => p.SKU == sku["sku"].ToString());
                            ScProduct product = new ScProduct();
                            if (_product != null)
                            {
                                product = _product;
                            }

                            product.ProductName = good["goods_name"].ToString();
                            product.SKU = sku["sku"].ToString();
                            product.SupplyCode = (int)Code;
                            product.Receipt = JsonConvert.SerializeObject(good);
                            product.Status = good["status"].ToString() == "true" ? 1 : 2;
                            product.Cradle = good["madein"].ToString();
                            product.Brands = good["brand"].ToString();
                            product.Tariff = (decimal.Parse(good["rate"].ToString()) * 100).ToString("F0");
                            product.Price = decimal.Parse(good["price"].ToString());

                            if (_product == null)
                            {
                                product.SysNo = BLL.Supply.ScProductBo.Instance.AddScProduct(product);
                                productList.Add(product);
                            }
                            else
                            {
                                Hyt.BLL.Supply.ScProductBo.Instance.UpdateScProduct(product);
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
                //result.Data = responseStr;
                return result;
            }
        }
        public override Result<string> GetGoodsSku(string skuid)
        {
            throw new NotImplementedException();
        }
        public override Result<string> StockInSupplyProduct(string sysNos)
        {
            throw new NotImplementedException();
        }
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
        public override Result<string> SendOrder(int orderSysNo)
        {
            var result = new Result<string>()
            {
                Status = false,
                StatusCode = 0,
                Message = ""
            };

            //调用方法名
            data.head.f = "send_order";

            var order = BLL.Order.SoOrderBo.Instance.GetEntity(orderSysNo);
            if (order == null)
            {
                result.Message = "该订单不存在";
            }
            else
            {

            }
            return result;
        }
        public override Result<string> GetAllGoodsSku()
        {
            throw new NotImplementedException();
        }


        #region 私有方法
        /// <summary>
        /// 检验返回结果
        /// </summary>
        /// <param name="back"></param>
        /// <remarks>2016-3-18 刘伟豪 创建</remarks>
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
                if (back.Property("root") != null)
                {
                    var head = back["root"]["head"];
                    if (head["oper_state"].ToString() == "1")
                    {
                        result.Message = "操作成功";
                        result.StatusCode = 1;
                        result.Status = true;
                    }
                    else if (head["oper_state"].ToString() == "0")
                    {
                        result.Message = head["error_msg"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return result;
        }

        private string Post(KebilinData data)
        {
            return GetResponse(Config.GatewayUrl, JsonConvert.SerializeObject(data));
        }
        /// <summary>
        /// 传递请求并接受返回结果，传递参数为Json格式仅支持POST方法
        /// </summary>
        /// <param name="url">接口网关</param>
        /// <param name="param">传递参数</param>
        /// <remarks>2016-3-14 刘伟豪 创建</remarks>
        private string GetResponse(string url, string param)
        {
            byte[] postData = Encoding.UTF8.GetBytes(param);
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url);
            req.Method = "POST";
            req.KeepAlive = true;
            req.Timeout = 300000;
            req.ContentType = "application/json;charset=UTF-8";
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