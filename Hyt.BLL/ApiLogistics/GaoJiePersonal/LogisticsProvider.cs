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
using Hyt.Model.Exception;
using Hyt.Model.Order;


namespace Hyt.BLL.ApiLogistics.GaoJiePersonal
{
    /// <summary>
    /// 高捷个人物品接口
    /// </summary>
    /// <remarks>
    /// <remarks>2016-7-26 杨浩 创建</remarks>
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
        //private string url = "http://test.cargo100.com/api/index.php?act=air_import_order_per&op=order";
        /// <summary>
        /// 访问地址(正式)
        /// </summary>
        private string url = "http://oms.goldjet.com.cn/api/index.php?act=air_import_order_per&op=order";

        private GaoJieConfig configGj = Hyt.BLL.Config.Config.Instance.GetGaoJieConfig();
        #region 属性
        /// <summary>
        /// 物流代码
        /// </summary>
        /// <remarks>2016-7-26 杨浩 创建</remarks>
        public override CommonEnum.物流代码 Code
        {
            get { return CommonEnum.物流代码.高捷个人物品; }
        }
        #endregion

        #region 公有方法
        /// <summary>
        /// Base64加密
        /// </summary>
        /// <param name="source">待加密的明文</param>
        /// <returns></returns>
        /// <remarks>2016-7-26 杨浩 创建</remarks>
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
        /// <remarks>2016-7-26 杨浩 创建</remarks>
        private string MD5(string param)
        {
            string md5Str = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(param, "MD5").ToLower();
            return md5Str;
        }

        public string GetResponse(string url, string param)
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
        #endregion

 
        public override Result GetProduct(string productId)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 添加交易订单
        /// </summary>
        /// <param name="orderSysno">订单编号</param>
        /// <returns></returns>
        /// <remarks>2016-7-26 杨浩 创建</remarks>
        public override Result AddOrderTrade(int orderSysno)
        {

            var result = new Result();

            try
            {
                #region 基础数据
                var pager = new Pager<CBPdProduct>()
                {
                    CurrentPage=1,
                    PageSize = 99999
                };
                pager.PageFilter.Status = -2;
                pager = BLL.Product.PdProductBo.Instance.GetCBPdProductList(pager);

                var orderInfo = BLL.Order.SoOrderBo.Instance.GetEntityNoCache(orderSysno);
                var warehouseInfo = BLL.Warehouse.WhWarehouseBo.Instance.GetWarehouse(orderInfo.DefaultWarehouseSysNo);

                var srenity = Hyt.BLL.Order.OutboundReturnBo.Instance.GetSoReceiveAddressBysoOrderSysNo(orderSysno);
                var districtEntity = Hyt.BLL.Basic.BasicAreaBo.Instance.GetArea(srenity.AreaSysNo);
                string district = districtEntity.AreaName.Trim();
                var cityEntity = Hyt.BLL.Basic.BasicAreaBo.Instance.GetArea(districtEntity.ParentSysNo);
                string city = cityEntity.AreaName.Trim();
                var provinceEntity = Hyt.BLL.Basic.BasicAreaBo.Instance.GetArea(cityEntity.ParentSysNo);
                string province = provinceEntity.AreaName.Trim();
                #endregion

                var orders = new StringBuilder();

                #region 订单商品列表
                int num = 0;
                decimal goodsGweight = 0m;//毛重
                var goodList = BLL.Order.SoOrderBo.Instance.GetOrderItemsByOrderId(orderSysno);
                string goods = "";
                foreach (var good in goodList)
                {
                  
                    var proudctInfo = pager.Rows.Where(x => x.SysNo == good.ProductSysNo).FirstOrDefault();
                    if(proudctInfo==null)
                    {
                        result.Status = false;
                        result.Message =string.Format("产品{0}不存在！",good.ProductSysNo);
                        return result;
                    }
                   var originInfo=BLL.Basic.OriginBo.Instance.GetEntity(proudctInfo.OriginSysNo);
                   var brandInfo=BLL.Product.PdBrandBo.Instance.GetEntity(proudctInfo.BrandSysNo);
                   string ycg_code = originInfo != null ? originInfo.CusOriginNO : "";

                    if (goods != "")
                        goods += ",";

                    goods += "{\"goodsPtcode\":\"" + proudctInfo.ProductDeclare + "\",";//税号
                    //goods += "{\"goodsPtcode\":\"09010110\",";//税号
                    goods += "\"goodsHsCode\":\"\",";//海关HS编码
                    goods += "\"goodSize\":\"\",";//计量单位
                    goods += "\"goodsName\":\"" + proudctInfo.ProductName + "\",";
                    goods += "\"brand\":\"" +brandInfo.Name+ "\",";
                    //goods += "\"goodSpec\":\"" + proudctInfo.Volume + "\",";
                    goods += "\"goodSpec\":\"" + proudctInfo.GrosWeight + "kg\",";
                    goods += "\"goodsNum\":" + good.Quantity + ",";//
                    goods += "\"goodsPrice\":" + good.SalesUnitPrice + ",";//申报单价
                    goods += "\"goodsTotal\":" + good.SalesAmount + ","; //申报总价
                    goods += "\"curr\":\"142\",";
                    goods += "\"goodsGweight\":\"" + proudctInfo.GrosWeight + "\",";//毛重
                    goods += "\"suttleWeight\":\"" + proudctInfo.NetWeight + "\",";//净重
                    goods += "\"GoodsUrl\":\"\",";
                    goods += "\"ycg_code\":\""+originInfo.CusOriginNO+"\",";//国家代码
                    goods += "\"Note\":\"\"}";

                    goodsGweight += proudctInfo.GrosWeight;
                    num += good.Quantity;
                }
                #endregion

                #region 订单主体

                #region 仓库所在国家
               
                var _prvinceInfo = Hyt.BLL.Basic.BasicAreaBo.Instance.GetArea(warehouseInfo.ProvinceSysNo);
                var countryInfo = Hyt.BLL.Basic.BasicAreaBo.Instance.GetArea(_prvinceInfo.ParentSysNo);
                #endregion
                
                orders.Append("{\"EntRecordNo\":\"\",");//企业的内部申报单编号
                orders.Append("\"EntInternalNo\":\"\",");//企业在海关备案申报系统中的备案号
                orders.Append("\"pfreightNo\":\"\",");//总运单号
                orders.Append("\"expressNum\":\"\",");
                orders.Append("\"orderSn\":\"" + orderInfo.OrderNo + "\",");
                orders.Append("\"DeclareCustoms\":\"" + configGj.DeclareCustoms + "\",");//申报地海关
                orders.Append("\"IePort\":\"" + configGj.IePort + "\",");//进出口口岸
                orders.Append("\"Voyage\": \"\",");//运输工具航次
                orders.Append("\"IeDate\":" +Hyt.Util.DateUtil.ConvertDateTimeInt(DateTime.Now)+",");//进出口日期
                orders.Append("\"buyerName\":\"" + srenity.Name + "\",");
                //orders.Append("\"buyerName\":\"杨浩\",");
                orders.Append("\"buyerIdcard\":\"" + srenity.IDCardNo + "\",");
                orders.Append("\"buyerAddress\":\"" + province + "^^^" + city + "^^^" + district + "^^^" + srenity.StreetAddress + "\",");
                orders.Append("\"recipientPhoneNo\":\"" + srenity.MobilePhoneNumber + "\",");//收件人电话号码
                orders.Append("\"provinceCode\":\"" + provinceEntity.AreaCode + "\",");//收件人省市区代码
                orders.Append("\"postCode\":\"" + srenity.ZipCode + "\",");
                orders.Append("\"num\":\"" + num + "\",");
                orders.Append("\"senderName\":\"" + warehouseInfo.Contact + "\",");
                orders.Append("\"senderCountryCode\":\"" + countryInfo.AreaCode + "\",");
                orders.Append("\"senderCity\":\"" + warehouseInfo.CityName + "\",");
                orders.Append("\"senderAddress\":\"" + warehouseInfo.StreetAddress + "\",");
                orders.Append("\"senderPhone\":\"" + warehouseInfo.Phone + "\",");
                orders.Append("\"curr\":\"142\",");
                orders.Append("\"goodsGweight\":" + goodsGweight + ",");
                orders.Append("\"Notes\":\"\",");
                orders.Append("\"goods\":[");
                orders.Append(goods);
                orders.Append("]}");
                #endregion 

                #region 推送至高捷接口
                string strorder = EncodeBase64(EncodeBase64(orders.ToString()));

                var strPost = "&seName=" + EncodeBase64(configGj.seller) + "&key=" + EncodeBase64(configGj.api_key) + "&mark=" + EncodeBase64("order") + "&confirm=" + EncodeBase64("2") + "&order=" + strorder;

                var strResult = GetResponse(url, strPost);
                var json = JObject.Parse(strResult);//{"state": "error","message": "用户名或密码不能为空"}




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

                string state = json["state"].ToString().ToLower();
                //推送成功
                if (state == "success")
                {                  
                    result.Status = true;
                    result.Message = json["message"].ToString();
                    soOrderSyncLogisticsLog.StatusCode = "";
                    soOrderSyncLogisticsLog.StatusMsg = result.Message;
                    //更新物流状态CBLogisticsSendStatus
                    BLL.Order.SoOrderBo.Instance.UpdateOrderApiStatus((int)Hyt.Model.WorkflowStatus.OrderStatus.跨境物流推送状态.已推送,3,orderSysno);
                }
                else
                {                   
                    result.Status = false;
                    result.Message = json["message"].ToString();
                    soOrderSyncLogisticsLog.StatusCode = "";
                    soOrderSyncLogisticsLog.StatusMsg = result.Message;
                    BLL.Order.SoOrderBo.Instance.UpdateOrderApiStatus((int)Hyt.Model.WorkflowStatus.OrderStatus.跨境物流推送状态.失败, 3, orderSysno);
                }

                SoOrderSyncLogisticsLogBo.Instance.Insert(soOrderSyncLogisticsLog);    

                #endregion

            }
            catch (ApiException ex)
            {
                result.Status = false;
                result.Message = ex.Message;
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
            var orderInfo = BLL.Order.SoOrderBo.Instance.GetEntity(int.Parse(orderId));

            string strorder = "{\"orderSn\":\"" + orderInfo.OrderNo + "\"}";
            strorder = EncodeBase64(EncodeBase64(strorder));

            var strPost = "&seName=" + EncodeBase64(configGj.seller) + "&key=" + EncodeBase64(configGj.api_key) + "&mark=" + EncodeBase64("ex_code")+"&confirm=" + EncodeBase64("")+"&order=" + strorder;

            var strResult = GetResponse(url, strPost);
            var json = JObject.Parse(strResult);
            string jsons = json.ToString();

            result.Status = true;
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

            string destinationCode = "";
            string pkgCode = ""; ;
            string billProvideSiteCode = "";
            string expressNo="";
            if (json["state"].ToString().ToLower() == "success")
            {
                destinationCode = json["destinationCode"].ToString();
                pkgCode = json["nodeCode"]["pkgCode"].ToString();
                billProvideSiteCode = json["nodeCode"]["billProvideSiteCode"].ToString();
                expressNo = json["expressNo"].ToString();
            }

            result.Data = " var destinationCode='" + destinationCode + "';var pkgCode='" + pkgCode + "';var billProvideSiteCode='" + billProvideSiteCode + "';"+(expressNo!=""?"expressNo='" + expressNo + "';":"");
            return result;
        }

        public override Result GetOrderTrade(string orderId)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 查询订单物流信息
        /// </summary>
        /// <param name="orderSysNo">订单号</param>
        /// <returns></returns>
        /// <remarks>2016-7-27 杨浩 创建</remarks>
        public override Result<string> GetLogisticsTracking(int orderSysNo)
        {
            var result = new Result<string>();
            result.StatusCode = -1;
            result.Status = false;
            
            try
            {
                var orderInfo = BLL.Order.SoOrderBo.Instance.GetEntityNoCache(orderSysNo);
                string strorder = "{\"orderSn\":\"" + orderInfo.OrderNo + "\"}";
                strorder = EncodeBase64(EncodeBase64(strorder));

                var strPost = "&seName=" + EncodeBase64(configGj.seller) + "&key=" + EncodeBase64(configGj.api_key) + "&mark=" + EncodeBase64("sel") + "&confirm=" + EncodeBase64("") + "&order=" + strorder;

                var strResult =GetResponse(url, strPost).Trim('[').Trim(']');              
                var json = JObject.Parse(strResult);
                if (json["flag"].ToString().ToLower() == "ok")
                {
                    string express = json["express_no"].ToString();
                    string o_state = json["o_state"].ToString();
                    if (o_state == "11")//订单通过
                    {
                        result.Status = true;
                        result.StatusCode = 0;
                       
                        if (!string.IsNullOrWhiteSpace(express))
                        {
                            if (orderInfo.CBLogisticsSendStatus != (int)Hyt.Model.WorkflowStatus.OrderStatus.跨境物流推送状态.成功)
                            {
                                //var _result = BLL.Order.SoOrderBo.Instance.Ship(orderSysNo, express, orderInfo);//自动发货

                                //if (_result.Status)
                                //{
                                //    BLL.Order.SoOrderBo.Instance.UpdateOrderApiStatus((int)Hyt.Model.WorkflowStatus.OrderStatus.跨境物流推送状态.成功, 3, orderSysNo);
                                //}                               
                                //else
                                //{
                                //    result.Status = false;
                                //    result.StatusCode = -2;
                                //    result.Message = _result.Message;
                                //}
                            }
                       
                            
                            result.Data = json["info"].ToString();
                        }

                    }
                    //else if (o_state == "12")//订单不通过
                    //{
                    //    result.StatusCode = -12;
                    //    result.Status = false;
                    //    result.Message = "订单不通过";
                    //    BLL.Order.SoOrderBo.Instance.UpdateOrderApiStatus((int)Hyt.Model.WorkflowStatus.OrderStatus.跨境物流推送状态.失败, 3, orderSysNo);
                    //}
                    else
                    {
                        result.StatusCode =int.Parse(o_state);
                        result.Message = json["o_info"].ToString();
                        BLL.Order.SoOrderBo.Instance.UpdateOrderApiStatus((int)Hyt.Model.WorkflowStatus.OrderStatus.跨境物流推送状态.失败, 3, orderSysNo);
                    }
                }
                else
                {
                    result.Message = json["o_info"].ToString();
                }
                         
            }
            catch (ApiException ex)
            {
                result.StatusCode = -1;
                result.Status = false;
                result.Message = ex.Message;
            }
            return result;
        }

        public override Result CancelOrderTrade(int orderSysNo, string reason = "")
        {
            var result = new Result();

            return result;
        }


    }
}
