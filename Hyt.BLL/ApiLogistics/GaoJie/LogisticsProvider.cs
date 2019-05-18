using Hyt.Model;
using Hyt.Model.Transfer;
using Hyt.Model.Logis.XinYi;
using Hyt.Model.Manual;
using Hyt.Model.WorkflowStatus;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Hyt.BLL.Finance;
using System.Net;
using System.IO;
using System.Security.Cryptography;
using Newtonsoft.Json.Linq;
using Hyt.BLL.Order;
using Hyt.BLL.Logistics;
using Hyt.Model.Common;
using Hyt.Model.Order;

namespace Hyt.BLL.ApiLogistics.GaoJie
{
    /// <summary>
    /// 高捷物流接口
    /// </summary>
    /// <remarks>
    /// <remarks>2016-5-25 王耀发 创建</remarks>
    /// </remarks>
    public class LogisticsProvider : ILogisticsProvider
    {
        ///// <summary>
        ///// 账号(测试) 
        ///// </summary>
        //private string seller = "tester";

        ///// <summary>
        ///// 验证码(测试) 
        ///// </summary>
        //private string api_key = "c33367701511b4f6020ec61ded352059";

        /// <summary>
        /// 访问地址(测试)
        /// </summary>
        //private string url = "http://test.cargo100.com/api/index.php?act=order_bc&op=order";
        /// <summary>
        /// 访问地址(正式)
        /// </summary>
        private string url = "http://oms.goldjet.com.cn/api/index.php?act=order_bc&op=order";
        /// <summary>
        /// 物流地址（测试）
        /// </summary>
        private string expressUrl = "http://oms.goldjet.com.cn/api/index.php?act=express_order&op=index";
        /// <summary>
        /// Base64加密
        /// </summary>
        /// <param name="source">待加密的明文</param>
        /// <returns></returns>
        /// <remarks>2016-5-25 王耀发 创建</remarks>
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
        /// <remarks>2016-5-25 王耀发 创建</remarks>
        private string MD5(string param)
        {
            string md5Str = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(param, "MD5").ToLower();
            return md5Str;
        }

        /// <summary>
        /// 推送订单
        /// </summary>
        /// <remarks>2016-5-25 王耀发 创建</remarks>
        public override Hyt.Model.CommonEnum.物流代码 Code
        {
            get { return Hyt.Model.CommonEnum.物流代码.高捷; }
        }
    

        static string GetResponse(string url, string param)
        {
            byte[] postData = Encoding.UTF8.GetBytes(param);

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "POST";
            req.Accept = "application/json";
            req.ContentType = "application/x-www-form-urlencoded";
            req.KeepAlive = true;
            req.Timeout = 300000;
            req.ContentLength = postData.Length;

            Stream reqStream = req.GetRequestStream();

            reqStream.Write(postData, 0, postData.Length);

            reqStream.Close();
            HttpWebResponse rsp = (HttpWebResponse)req.GetResponse();

            Stream stream = rsp.GetResponseStream();

            StreamReader sr = new StreamReader(stream, Encoding.UTF8);
            string result = sr.ReadToEnd();
            sr.Close();
            stream.Close();

            return result;
        }
  

        public override Result GetProduct(string productId)
        {
            throw new NotImplementedException();
        }

        public override Result AddOrderTrade(int orderSysno)
        {
            //throw new NotImplementedException();
            List<SoOrderSyncLogisticsLog> logList = SoOrderSyncLogisticsLogBo.Instance.GetModelList(orderSysno);
            List<SoOrderSyncLogisticsLog> haveLogisticsList = logList.Where(p => p.Code == (int)this.Code && p.StatusCode == "OK" && !string.IsNullOrEmpty(p.LogisticsOrderId)).ToList();
            SoOrderSyncLogisticsLog nowLogistics=null;
            if (haveLogisticsList.Count == 0)
            {
                Result expressResult = GetExpressno(orderSysno.ToString());
                nowLogistics = (expressResult as Result<SoOrderSyncLogisticsLog>).Data;
                if(!expressResult.Status)
                {
                    return expressResult;
                }
            }
            else
            {
                nowLogistics = haveLogisticsList[0];
            }


            var order = BLL.Order.SoOrderBo.Instance.GetEntity(orderSysno);

            var config = Hyt.BLL.Config.Config.Instance.GetGaoJieConfig();

            var result = new Result<string>();

            WhWarehouse warehouseMod = Hyt.BLL.Warehouse.WhWarehouseBo.Instance.GetWarehouse(order.DefaultWarehouseSysNo);
            BsArea wareDistrictEntity = Hyt.BLL.Basic.BasicAreaBo.Instance.GetArea(warehouseMod.AreaSysNo);
            BsArea wareCityEntity = Hyt.BLL.Basic.BasicAreaBo.Instance.GetArea(wareDistrictEntity.ParentSysNo);

            SoReceiveAddress srenity = Hyt.BLL.Order.OutboundReturnBo.Instance.GetSoReceiveAddressBysoOrderSysNo(order.SysNo);
            string buyer_idcard = "";
            if (!string.IsNullOrEmpty(srenity.IDCardNo))
            {
                buyer_idcard = srenity.IDCardNo.Trim();
            }

            BsArea DistrictEntity = Hyt.BLL.Basic.BasicAreaBo.Instance.GetArea(srenity.AreaSysNo);
            string District = DistrictEntity.AreaName.Trim();
            BsArea CityEntity = Hyt.BLL.Basic.BasicAreaBo.Instance.GetArea(DistrictEntity.ParentSysNo);
            string City = CityEntity.AreaName.Trim();
            BsArea ProvinceEntity = Hyt.BLL.Basic.BasicAreaBo.Instance.GetArea(CityEntity.ParentSysNo);
            string Province = ProvinceEntity.AreaName.Trim();

            CBFnOnlinePayment payment = FinanceBo.Instance.GetOnPaymentBySourceSysNo(order.SysNo);

            IList<CBSoOrderItem> datao = SoOrderBo.Instance.GetCBOrderItemsByOrderId(order.SysNo);
            List<int> ProSysNo = new List<int>();
            foreach (CBSoOrderItem item in datao)
            {
                ProSysNo.Add(item.ProductSysNo);
            }
            IList<CBPdProduct> productList = Hyt.BLL.Product.PdProductBo.Instance.GetProductInfoList(ProSysNo);
            decimal goodsGweight = 0;
            foreach(var mod in productList)
            {
                goodsGweight += mod.GrosWeight;
            }
            LgGaoJiePushInfo model = new LgGaoJiePushInfo();
            string strorder = "";
            strorder += "{\"order_sn\":\"" + order.OrderNo + "\",\"pfreight_no\":\"\",\"express_num\":\"" + nowLogistics.LogisticsOrderId + "\",";
            strorder += "\"sender_name\":\"" + warehouseMod.Contact + "\",\"sender_city\":\"" + wareCityEntity.AreaName + "\",\"sender_address\":\"" + warehouseMod.StreetAddress + "\",";
            strorder += "\"sender_phone\":\"" + warehouseMod.Phone + "\",\"sender_country_code\":\"" + wareCityEntity.AreaCode + "\",";
            strorder += "\"buyer_name\":\"" + srenity.Name + "\",\"buyer_idcard\":\"" + buyer_idcard + "\",\"buyer_phone\":\"" + srenity.MobilePhoneNumber + "\",";
            strorder += "\"province_code\":\"" + ProvinceEntity.AreaCode + "" + "\",\"buyer_address\":\"" + Province + "^^^" + City + "^^^" + District + "^^^" + srenity.StreetAddress + "\",";
            strorder += "\"curr\":\"502" + "\",\"pkg_gweight\":" + goodsGweight + ",\"p_name\":\"" + (payment == null ? "" : payment.PaymentName) + "\",\"p_no\":\"" + (payment == null ? "" : payment.VoucherNo) + "\",";
            strorder += "\"p_time\":\"" + (payment == null ? DateTime.Now.ToString("yyyy-mm-dd hh :MM") : payment.CreatedDate.ToString("yyyy-mm-dd hh :MM")) + "\",\"sh_fee\":\"" + order.FreightAmount.ToString() + "\",";
            strorder += "\"cus_tax\":\"" + order.TaxFee.ToString() + "\",\"pweb\":\"" + config.pweb + "\",\"web_name\":\"" + config.web_name + "\",\"re_no\":\"" + config.re_no + "\",\"re_name\":\"" + config.re_name + "\",";
            strorder += "\"order_goods\":";
            strorder += "[";
            string str = "";
            int i = 0;

            

            foreach (CBSoOrderItem item in datao)
            {
                //LgGaoJieGoodsInfo goodsInfo = LogisticsBo.Instance.GetLgGaoJieGoodsInfoEntityByPid(item.ProductSysNo);
                List<CBPdProduct> tempProductList = productList.Where(p => p.SysNo == item.ProductSysNo).ToList();

                var originInfo = BLL.Basic.OriginBo.Instance.GetEntity(tempProductList[0].OriginSysNo);
                var brandInfo = BLL.Product.PdBrandBo.Instance.GetEntity(tempProductList[0].BrandSysNo);
                string ycg_code = originInfo != null ? originInfo.CusOriginNO : "";

                if (i > 0)
                {
                    str += ",{";
                }
                else
                {
                    str += "{";
                }
                str += "\"goods_ptcode\":\"" + tempProductList[0].ProductDeclare;
                str += "\",\"goods_name\":\"" + tempProductList[0].ProductName;
                str += "\",\"brand\":\"" + brandInfo.Name;
                str += "\",\"goods_spec\":\"" + tempProductList[0].GrosWeight+"kg";
                str += "\",\"goods_num\":\"" + item.Quantity.ToString();
                str += "\",\"goods_price\":\"" + item.SalesUnitPrice.ToString();
                str += "\",\"ycg_code\":\"" + originInfo.CusOriginNO;
                str += "\",\"goods_barcode\":\"" + tempProductList[0].Barcode.Trim() + "\"";
                str += "}";
                i++;
            }
            strorder += str;
            strorder += "]}";

            string orders = strorder;
            strorder = EncodeBase64(EncodeBase64(strorder));

            var strPost = "&seller=" + EncodeBase64(config.seller) + "&api_key=" + EncodeBase64(config.api_key) + "&mark=" + EncodeBase64("order") + "&confirm=" + EncodeBase64("2") + "&order=" + strorder;

            var strResult = GetResponse(url, strPost);
            var json = JObject.Parse(strResult);
            string jsons = json.ToString();

            result.Status = true;
            result.Data = json.ToString();

            var soOrderSyncLogisticsLog = new SoOrderSyncLogisticsLog();
            soOrderSyncLogisticsLog.OrderSysNo = orderSysno;
            soOrderSyncLogisticsLog.Code = (int)this.Code;
            soOrderSyncLogisticsLog.CreateDate = DateTime.Now;
            soOrderSyncLogisticsLog.CreatedBy = BLL.Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo;
            soOrderSyncLogisticsLog.LastUpdateBy = 0;
            soOrderSyncLogisticsLog.LastUpdateDate = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;

            soOrderSyncLogisticsLog.Packets = orders.ToString();
            soOrderSyncLogisticsLog.ReceiptContent = json.ToString();
            soOrderSyncLogisticsLog.LogisticsOrderId = "";

             
            //var model = new LgGaoJiePushInfo();
            //model.OrderSysNo = orderSysno;
            //model.OrderInfo = orders.ToString();
            ////保存订单回执信息
            //model.ReturnInfo = json.ToString();
            ////保存推送回执信息
            //LogisticsBo.Instance.InsertLgGaoJiePushInfoEntity(model, BLL.Authentication.AdminAuthenticationBo.Instance.Current.Base);

            string state = json["flag"].ToString().ToLower();
            //推送成功
            if (state == "ok" || json["info"].ToString()=="重复下单")
            {
                result.Status = true;
                result.Message = json["info"].ToString();
                soOrderSyncLogisticsLog.StatusCode = "";
                soOrderSyncLogisticsLog.StatusMsg = result.Message;
                //更新物流状态CBLogisticsSendStatus
                BLL.Order.SoOrderBo.Instance.UpdateOrderApiStatus((int)Hyt.Model.WorkflowStatus.OrderStatus.跨境物流推送状态.已推送, 3, orderSysno);
            }
            else
            {
                result.Status = false;
                result.Message = json["info"].ToString();
                soOrderSyncLogisticsLog.StatusCode = "";
                soOrderSyncLogisticsLog.StatusMsg = result.Message;
                BLL.Order.SoOrderBo.Instance.UpdateOrderApiStatus((int)Hyt.Model.WorkflowStatus.OrderStatus.跨境物流推送状态.失败, 3, orderSysno);
            }

            SoOrderSyncLogisticsLogBo.Instance.Insert(soOrderSyncLogisticsLog);    

            return result;
        }

        /// <summary>
        /// 获取快递单号
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        /// <remarks>2016-9-3 杨浩 创建</remarks>
        private Result GetExpressno(string orderId)
        {
            var config = Hyt.BLL.Config.Config.Instance.GetGaoJieConfig();
            var result = new Result<SoOrderSyncLogisticsLog>();
            var orderInfo = BLL.Order.SoOrderBo.Instance.GetEntity(int.Parse(orderId));
            WhWarehouse warehouseMod = Hyt.BLL.Warehouse.WhWarehouseBo.Instance.GetWarehouse(orderInfo.DefaultWarehouseSysNo);
            BsArea wareDistrictEntity = Hyt.BLL.Basic.BasicAreaBo.Instance.GetArea(warehouseMod.AreaSysNo);
            BsArea wareCityEntity = Hyt.BLL.Basic.BasicAreaBo.Instance.GetArea(wareDistrictEntity.ParentSysNo);

            SoReceiveAddress srenity = Hyt.BLL.Order.OutboundReturnBo.Instance.GetSoReceiveAddressBysoOrderSysNo(orderInfo.SysNo);
            string buyer_idcard = "";
            if (!string.IsNullOrEmpty(srenity.IDCardNo))
            {
                buyer_idcard = srenity.IDCardNo.Trim();
            }

            BsArea DistrictEntity = Hyt.BLL.Basic.BasicAreaBo.Instance.GetArea(srenity.AreaSysNo);
            string District = DistrictEntity.AreaName.Trim();
            BsArea CityEntity = Hyt.BLL.Basic.BasicAreaBo.Instance.GetArea(DistrictEntity.ParentSysNo);
            string City = CityEntity.AreaName.Trim();
            BsArea ProvinceEntity = Hyt.BLL.Basic.BasicAreaBo.Instance.GetArea(CityEntity.ParentSysNo);
            string Province = ProvinceEntity.AreaName.Trim();

            CBFnOnlinePayment payment = FinanceBo.Instance.GetOnPaymentBySourceSysNo(orderInfo.SysNo);

            IList<CBSoOrderItem> datao = SoOrderBo.Instance.GetCBOrderItemsByOrderId(orderInfo.SysNo);

            string goods_Names = "";
            int goods_Nums = 0;
            decimal goods_Weight = 0;
            foreach (var mod in datao)
            {
                if (!string.IsNullOrEmpty(goods_Names))
                {
                    goods_Names += ",";
                }
                goods_Names += mod.ProductName;

                goods_Nums += mod.Quantity;

                goods_Weight += mod.GrosWeight;
            }

            StringBuilder strorder = new StringBuilder();//"{\"orderSn\":\"" + orderInfo.OrderNo + "\"}";
            strorder.Append("{");
            strorder.Append("    \"order_sn\":\"" + orderInfo.OrderNo + "\",");
            strorder.Append("    \"send_name\":\"" + warehouseMod.Contact + "\",");
            strorder.Append("    \"send_telno\":\"" + warehouseMod.Phone + "\",");
            strorder.Append("    \"send_cardno\":\"\",");
            strorder.Append("    \"send_postcode\":\"\",");
            strorder.Append("    \"send_code\":\"1\",");
            strorder.Append("    \"receive_name\":\"" + srenity.Name + "\",");
            strorder.Append("    \"receive_telno\":\"" + srenity.MobilePhoneNumber + "\",");
            strorder.Append("    \"receive_cardno\":\"" + srenity.IDCardNo + "\",");
            strorder.Append("    \"receive_postcode\":\"\",");
            strorder.Append("    \"receive_province\":\"" + Province + "\",");
            strorder.Append("    \"receive_city\":\"" + City + "\",");
            strorder.Append("    \"receive_area\":\"" + District + "\",");
            strorder.Append("    \"receive_address\":\"" + Province + " " + City + " " + District + " " + srenity.StreetAddress + "\",");
            strorder.Append("    \"goods_name\":\"" + goods_Names + "\",");
            strorder.Append("    \"goods_sku\":\"\",");
            strorder.Append("    \"goods_num\":\"" + goods_Nums + "\",");
            strorder.Append("    \"notes\":\"\",");
            strorder.Append("    \"weight\":\"" + goods_Weight + "\"");
            strorder.Append("}");

            string enStrorder = "";
            enStrorder = EncodeBase64(EncodeBase64(strorder.ToString()));

            string strPost = "&seller=" + EncodeBase64(config.seller) + "&api_key=" + EncodeBase64(config.api_key) + "&express=" + EncodeBase64("3") + "&order_type=" + EncodeBase64("1") + "&order=" + enStrorder;

            var strResult = GetResponse(expressUrl, strPost);
            var json = JObject.Parse(strResult);
            string jsons = json.ToString();


            result.Message = json.ToString();

            /*{
                "orderSn": "So00003784",
                "state": "success",
                "expressNo": "50323423506251",
                "expressCode": "3",
                "destinationCode": "粤-肇庆",
                "nodeCode": {
                    "pkgCode": "广州夏良转运中心",
                    "billProvideSiteName": "广州新花山站",
                    "billProvideSiteCode": "Z04"
                  }
             }*/

            //string destinationCode = "";
            //string pkgCode = ""; ;
            //string billProvideSiteCode = "";
            //string expressNo = "";
            //if (json["state"].ToString().ToLower() == "success")
            //{
            //    destinationCode = json["destinationCode"].ToString();
            //    pkgCode = json["nodeCode"]["pkgCode"].ToString();
            //    billProvideSiteCode = json["nodeCode"]["billProvideSiteCode"].ToString();
            //    expressNo = json["expressNo"].ToString();
            //}

            ///订单物流信息
            SoOrderSyncLogisticsLog soLogisticsLogMod = new SoOrderSyncLogisticsLog();
            soLogisticsLogMod.OrderSysNo = orderInfo.SysNo;
            soLogisticsLogMod.Code = (int)this.Code;
            try
            {
                soLogisticsLogMod.LogisticsOrderId = json["express_no"].ToString();
            }
            catch { }

            soLogisticsLogMod.StatusCode = json["flag"].ToString();
            try
            {
                soLogisticsLogMod.StatusMsg = json["express_notes"].ToString();
            }
            catch
            {
                try
                {
                    soLogisticsLogMod.StatusMsg = json["info"].ToString();
                }
                catch { }
            }

            soLogisticsLogMod.Packets = strorder.ToString();
            soLogisticsLogMod.ReceiptContent = strResult;
            soLogisticsLogMod.CreateDate = DateTime.Now;
            soLogisticsLogMod.LastUpdateDate = DateTime.Now;
            SoOrderSyncLogisticsLogBo.Instance.Insert(soLogisticsLogMod);
            if (json["flag"].ToString().ToUpper() == "OK")
            {
                result.Status = true;
                result.StatusCode = 100;
                result.Data = soLogisticsLogMod;
            }
            else
            {
                result.Status = false;
                result.StatusCode = 0;
                result.Data = soLogisticsLogMod;
            }

            return result;
        }
        /// <summary>
        /// 获取运单信息
        /// </summary>
        /// <param name="orderId">订单编号</param>
        /// <returns></returns>
        /// <remarks>2016-8-4 杨浩 创建</remarks>
        public override Result GetOrderExpressno(string orderId)
        {
            var result = new Result<string>();

            var logList = SoOrderSyncLogisticsLogBo.Instance.GetModelList(int.Parse(orderId));
            var haveLogisticsInfo = logList.Where(p => p.Code == (int)this.Code && p.StatusCode == "OK" && !string.IsNullOrEmpty(p.LogisticsOrderId)).FirstOrDefault();
            string destinationCode = "";
            string pkgCode = "";
            string billProvideSiteCode = "";
            string expressNo = "";
            if (haveLogisticsInfo!=null)
            {
               var expressnoInfo=JObject.Parse(haveLogisticsInfo.ReceiptContent);

               var UTF8 = new System.Text.UTF8Encoding();
               Byte[] bytesStr = UTF8.GetBytes(expressnoInfo["express_notes"].ToString());

               var notes=(System.Collections.Hashtable)Hyt.Util.Serialization.PHPSerializer.UnSerialize(bytesStr);
               expressNo = expressnoInfo["express_no"].ToString();
               pkgCode = notes["pkgCode"].ToString();
               billProvideSiteCode = notes["billProvideSiteCode"].ToString();

               destinationCode = expressnoInfo["destcode"].ToString();
            }
         

            //{"flag":"OK","order_sn":"So00003962","express_no":"50002210302661","destcode":"\u7ca4-\u8087\u5e86(\u7ca4-1)","origincode":"020","express_notes":"a:3:{s:7:\"pkgCode\";s:18:\"\u5e7f\u5dde\u5206\u62e8\u4e2d\u5fc3\";s:19:\"billProvideSiteName\";s:18:\"\u4e0a\u6d77\u8f6c\u8fd0\u4e2d\u5fc3\";s:19:\"billProvideSiteCode\";s:0:\"\";}"}
            result.Data = " var destinationCode='" + destinationCode + "';var pkgCode='" + pkgCode + "';var billProvideSiteCode='" + billProvideSiteCode + "';" + (expressNo != "" ? "expressNo='" + expressNo + "';" : "");
            return result;
        }

        public override Result GetOrderTrade(string orderId)
        {
            

            var config = Hyt.BLL.Config.Config.Instance.GetGaoJieConfig();

            var result = new Result();

            //SoReceiveAddress srenity = Hyt.BLL.Order.OutboundReturnBo.Instance.GetSoReceiveAddressBysoOrderSysNo(order.Order.SysNo);
            //string buyer_idcard = "";
            //if (!string.IsNullOrEmpty(srenity.IDCardNo))
            //{
            //    buyer_idcard = srenity.IDCardNo.Trim();
            //}

            //BsArea DistrictEntity = Hyt.BLL.Basic.BasicAreaBo.Instance.GetArea(srenity.AreaSysNo);
            //string District = DistrictEntity.AreaName.Trim();
            //BsArea CityEntity = Hyt.BLL.Basic.BasicAreaBo.Instance.GetArea(DistrictEntity.ParentSysNo);
            //string City = CityEntity.AreaName.Trim();
            //BsArea ProvinceEntity = Hyt.BLL.Basic.BasicAreaBo.Instance.GetArea(CityEntity.ParentSysNo);
            //string Province = ProvinceEntity.AreaName.Trim();

            //CBFnOnlinePayment payment = FinanceBo.Instance.GetOnPaymentBySourceSysNo(order.Order.SysNo);

            //LgGaoJiePushInfo model = new LgGaoJiePushInfo();

            //string strorder = "{\"order_sn\":\"" + order.Order.OrderNo + "\",\"pfreight_no\":\"" + order.pfreight_no + "\",\"express_num\":\"" + order.express_num + "\",";
            //strorder += "\"sender_name\":\"" + order.sender_name + "\",\"sender_city\":\"" + order.sender_city + "\",\"sender_address\":\"" + order.sender_address + "\",";
            //strorder += "\"sender_phone\":\"" + order.sender_phone + "\",\"sender_country_code\":\"" + order.sender_country_code + "\",";
            //strorder += "\"buyer_name\":\"" + srenity.Name + "\",\"buyer_idcard\":\"" + buyer_idcard + "\",\"buyer_phone\":\"" + srenity.MobilePhoneNumber + "\",";
            //strorder += "\"province_code\":\"" + "\",\"buyer_address\":\"" + Province + "^^^" + City + "^^^" + District + "^^^" + srenity.StreetAddress + "\",";
            //strorder += "\"curr\":\"502" + "\",\"goods_gweight\":\"" + order.goods_gweight + "\",\"p_name\":\"" + (payment == null ? "" : payment.PaymentName) + "\",\"p_no\":\"" + (payment == null ? "" : payment.BusinessOrderSysNo) + "\",";
            //strorder += "\"p_time\":\"" + (payment == null ? DateTime.Now.ToString("yyyy-mm-dd hh :MM") : payment.CreatedDate.ToString("yyyy-mm-dd hh :MM")) + "\",\"sh_fee\":\"" + order.Order.FreightAmount.ToString() + "\",";
            //strorder += "\"cus_tax\":\"" + order.Order.TaxFee.ToString() + "\",\"pweb\":\"" + config.pweb + "\",\"web_name\":\"" + config.web_name + "\",\"re_no\":\"" + config.re_no + "\",\"re_name\":\"" + config.re_name + "\",";
            //strorder += "\"order_goods\":";
            //strorder += "[";
            //string str = "";
            //int i = 0;

            //IList<CBSoOrderItem> datao = SoOrderBo.Instance.GetCBOrderItemsByOrderId(order.Order.SysNo);

            //foreach (CBSoOrderItem item in datao)
            //{
            //    LgGaoJieGoodsInfo goodsInfo = LogisticsBo.Instance.GetLgGaoJieGoodsInfoEntityByPid(item.ProductSysNo);
            //    if (i > 0)
            //    {
            //        str += ",{";
            //    }
            //    else
            //    {
            //        str += "{";
            //    }
            //    str += "\"goods_ptcode\":\"" + goodsInfo.goods_ptcode.Trim();
            //    str += "\",\"goods_name\":\"" + goodsInfo.goods_name.Trim();
            //    str += "\",\"brand\":\"" + goodsInfo.brand.Trim();
            //    str += "\",\"goods_spec\":\"" + goodsInfo.goods_spec.Trim();
            //    str += "\",\"goods_num\":\"" + item.Quantity.ToString();
            //    str += "\",\"goods_price\":\"" + item.SalesUnitPrice.ToString();
            //    str += "\",\"ycg_code\":\"" + goodsInfo.ycg_code.Trim();
            //    str += "\",\"hs_code\":\"" + goodsInfo.hs_code.Trim();
            //    str += "\",\"goods_barcode\":\"" + goodsInfo.goods_barcode.Trim() + "\"";
            //    str += "}";
            //    i++;
            //}
            //strorder += str;
            //strorder += "]}";
            ////保存订单推送信息
            //model.OrderInfo = strorder;

            //strorder = EncodeBase64(EncodeBase64(strorder));

            //var strPost = "&seller=" + EncodeBase64(config.seller) + "&api_key=" + EncodeBase64(config.api_key) + "&mark=" + EncodeBase64("order") + "&confirm=" + EncodeBase64("2") + "&order=" + strorder;

            //var strResult = GetResponse(url, strPost);
            //var json = JObject.Parse(strResult);

            ////保存订单回执信息
            //model.ReturnInfo = json.ToString();

            //string flag = json["flag"].ToString();
            ////推送成功
            //if (flag == "OK")
            //{
            //    result.Status = true;
            //    result.Message = json["info"].ToString();
            //    //更新物流状态CBLogisticsSendStatus
            //    BLL.Order.SoOrderBo.Instance.UpdateOrderApiStatus(1, 3, order.Order.SysNo);
            //}
            //else
            //{
            //    result.Status = false;
            //    result.Message = json["info"].ToString();
            //}
            ////保存推送回执信息
            //LogisticsBo.Instance.InsertLgGaoJiePushInfoEntity(model, BLL.Authentication.AdminAuthenticationBo.Instance.Current.Base);

            return result;
        }
        /// <summary>
        /// 查询订单物流信息
        /// </summary>
        /// <param name="orderSysNo">订单号</param>
        /// <returns></returns>
        /// <remarks>2016-8-22 杨浩 创建</remarks>
        public override Result<string> GetLogisticsTracking(int orderSysNo)
        {
            var order = BLL.Order.SoOrderBo.Instance.GetEntity(orderSysNo);

            var config = Hyt.BLL.Config.Config.Instance.GetGaoJieConfig();

            var result = new Result<string>();

           
            string strorder = "{\"order_sn\":\"" + order.OrderNo + "\"}";
            string orders = strorder;
            strorder = EncodeBase64(EncodeBase64(strorder));
            
            var strPost = "&seller=" + EncodeBase64(config.seller) + "&api_key=" + EncodeBase64(config.api_key) + "&mark=" + EncodeBase64("sel") + "&confirm=" + EncodeBase64("") + "&order=" + strorder;

            var strResult = GetResponse(url, strPost);
            var json = JObject.Parse(strResult);
            string jsons = json.ToString();

            result.Status = true;
            result.Data = json.ToString();

            var soOrderSyncLogisticsLog = new SoOrderSyncLogisticsLog();
            soOrderSyncLogisticsLog.OrderSysNo = orderSysNo;
            soOrderSyncLogisticsLog.Code = (int)this.Code;
            soOrderSyncLogisticsLog.CreateDate = DateTime.Now;
            soOrderSyncLogisticsLog.CreatedBy = BLL.Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo;
            soOrderSyncLogisticsLog.LastUpdateBy = 0;
            soOrderSyncLogisticsLog.LastUpdateDate = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;

            soOrderSyncLogisticsLog.Packets = orders.ToString();
            soOrderSyncLogisticsLog.ReceiptContent = json.ToString();
            soOrderSyncLogisticsLog.LogisticsOrderId = "";


            //var model = new LgGaoJiePushInfo();
            //model.OrderSysNo = orderSysno;
            //model.OrderInfo = orders.ToString();
            ////保存订单回执信息
            //model.ReturnInfo = json.ToString();
            ////保存推送回执信息
            //LogisticsBo.Instance.InsertLgGaoJiePushInfoEntity(model, BLL.Authentication.AdminAuthenticationBo.Instance.Current.Base);

            string state = json["flag"].ToString().ToLower();
            //推送成功
            if (state == "ok")
            {
                result.Status = true;
                result.Message = json["wms_info"].ToString();
                soOrderSyncLogisticsLog.StatusCode = "";
                soOrderSyncLogisticsLog.StatusMsg = result.Message;
                //更新物流状态CBLogisticsSendStatus
                BLL.Order.SoOrderBo.Instance.UpdateOrderApiStatus((int)Hyt.Model.WorkflowStatus.OrderStatus.跨境物流推送状态.已推送, 3, orderSysNo);
                SoOrderSyncLogisticsLogBo.Instance.Insert(soOrderSyncLogisticsLog);    
            }

            return result;
        }

        public override Result CancelOrderTrade(int orderSysNo, string reason = "")
        {
            throw new NotImplementedException();
        }
    }
}
