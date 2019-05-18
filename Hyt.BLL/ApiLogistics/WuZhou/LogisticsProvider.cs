using Hyt.BLL.Order;
using Hyt.Model;
using Hyt.Model.Api;
using Hyt.Model.Order;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;
using Hyt.Util;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using Hyt.BLL.Finance;
using Hyt.BLL.ApiLogistics.Anna;
using Hyt.Util.Serialization;
using Hyt.Model.WuZhou;

namespace Hyt.BLL.ApiLogistics.WuZhou
{
    /// <summary>
    /// 五洲商会接口
    /// </summary>
    /// <remarks>
    /// 2017-8-29 杨浩 创建
    /// </remarks>
    public class LogisticsProvider : ILogisticsProvider
    {
        public LogisticsProvider() { }
        /// <summary>
        /// 物流标示
        /// </summary>
        /// <remarks>2015-12-26 杨浩 创建</remarks>
        public override Hyt.Model.CommonEnum.物流代码 Code
        {
            get { return Hyt.Model.CommonEnum.物流代码.五洲四海商务; }
        }

        public override Result GetProduct(string productId)
        {
            throw new NotImplementedException();
        }

        #region 给一个字符串进行MD5加密
        /// <summary>  
        /// 给一个字符串进行MD5加密  
        /// </summary>  
        /// <param   name="strText">待加密字符串</param>  
        /// <returns>加密后的字符串</returns>  
        /// <remarks>2013-10-22 杨浩 添加</remarks>
        public string MD5Encrypt(string strText)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] result = md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(strText));

            string outString = "";
            for (int i = 0; i < result.Length; i++)
            {
                outString += result[i].ToString("x2");
            }

            return outString;

        }
        #endregion

        #region Base64编码
        private string EncodeBase64(string source)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(source);
            return Convert.ToBase64String(bytes);
        }
        #endregion

        #region url编码
        public static string UrlEncode(string src)
        {
            StringBuilder strTo = new StringBuilder();
            byte[] byStr = System.Text.Encoding.UTF8.GetBytes(src);
            for (int i = 0; i < byStr.Length; i++)
            {
                if ((byStr[i] >= 48 && byStr[i] <= 57) || (byStr[i] >= 65 && byStr[i] <= 90) || (byStr[i] >= 97 && byStr[i] <= 122))
                {
                    strTo.Append((char)byStr[i]);
                }
                else
                {
                    strTo.Append(@"%" + Convert.ToString(byStr[i], 16));
                }
            }

            return (strTo.ToString());
        }
        #endregion

        /// <summary>
        /// 添加交易订单
        /// </summary>
        /// <param name="orderSysno"></param>
        /// <returns>2016-09-07 杨浩 创建</returns>
        public override Result AddOrderTrade(int orderSysno)
        {
            var result = new Result<string>();
            result.Status = true;
            result.StatusCode = 0;
            result.Message = "接口调用成功";

            if (orderSysno <= 0)
            {
                return result;
            }

            try
            {
                var orderInfo = BLL.Order.SoOrderBo.Instance.GetEntity(orderSysno);

                CBFnOnlinePayment payment = FinanceBo.Instance.GetOnPaymentBySourceSysNo(orderInfo.SysNo);

                CrCustomer customer = Hyt.BLL.CRM.CrCustomerBo.Instance.GetModel(orderInfo.CustomerSysNo);

                var order = new AddWuZhouRequest();

                order.item_list = new List<Item_Lists>();

                var pager = new Pager<AddWuZhouRequest>() { PageSize = 999999, CurrentPage = 1 };
                //pager = Hyt.BLL.ApiLogistics.WuZhou.WuZhouBll.PdProductBo.Instance.GetWuZhouList(pager);
                IList<CBSoOrderItem> datao = SoOrderBo.Instance.GetCBOrderItemsByOrderId(orderInfo.SysNo);

                orderInfo.OrderItemList = new List<SoOrderItem>();
                List<int> ProSysNo = new List<int>();
                foreach (CBSoOrderItem item in datao)
                {
                    ProSysNo.Add(item.ProductSysNo);

                    orderInfo.OrderItemList.Add(item);
                }
                IList<CBPdProduct> productList = Hyt.BLL.Product.PdProductBo.Instance.GetProductInfoList(ProSysNo);
                foreach (var mod in productList)
                {

                    var tempitem = datao.First(p => p.ProductSysNo == mod.SysNo);
                    if (tempitem != null)
                    {
                        tempitem.OrginCountry = Hyt.BLL.Basic.OriginBo.Instance.GetEntity(mod.OriginSysNo).Origin_Name;
                    }
                }
                
                SoReceiveAddress srenity = Hyt.BLL.Order.OutboundReturnBo.Instance.GetSoReceiveAddressBysoOrderSysNo(orderSysno);


                #region 订单信息

                order.outer_code = orderSysno.ToString();
                order.goods_total = orderInfo.OrderAmount;
                order.order_pay = orderInfo.OrderAmount;
                order.logis_pay = orderInfo.FreightAmount;
                order.favourable = orderInfo.CouponAmount;
                order.item_count = 0;
                order.ebp_account = customer.Account;
                order.buyer_name = customer.Name;
                order.buyer_idtype = 1;
                order.buyer_idnumber = customer.IDCardNo;
                order.buyer_tel = customer.MobilePhoneNumber;
                order.consignee = srenity.Name;
                order.consignee_postcode = srenity.ZipCode;
                order.consignee_tel = srenity.MobilePhoneNumber;
                order.consignee_province = srenity.AreaSysNo.ToString();
                order.consignee_city = srenity.AreaSysNo.ToString();
                order.consignee_district = srenity.AreaSysNo.ToString();
                order.consignee_addr = srenity.StreetAddress;
                order.consignee_email = srenity.EmailAddress;
                order.ebc_code = "";
                order.ebp_code = "";
                order.pay_code = "";
                order.payment_no = "";
                order.is_ordermsg = 1;
                order.is_paymsg = 1;
                order.is_logismsg = 1;
                order.is_invtmsg = 1;
                order.express_name = "";
                order.logis_num = "";
                order.note = orderInfo.Remarks;

                #endregion

                //循环商品信息
                foreach (var item in datao)
                {
                    var productInfo = productList.Where(x => x.SysNo == item.ProductSysNo).FirstOrDefault();
                    if (productInfo == null)
                    {
                        result.Status = false;
                        result.StatusCode = 1;
                        result.Message = "产品系统编号【" + item.ProductSysNo + "】在系统中不存在";
                        return result;
                    }

                    var orderItem = new Item_Lists();
                    orderItem.sku_code = item.TransactionSysNo;
                    orderItem.chcus_sku = item.ProductSysNo.ToString();
                    orderItem.sku_price = item.SalesUnitPrice;
                    orderItem.qty = item.Quantity;
                    orderItem.total = item.SalesUnitPrice;
                    orderItem.discount = 0;
                    orderItem.note = orderInfo.DeliveryRemarks;

                    order.item_list.Add(orderItem);
                }


                //Dictionary<string, string> dicKeyList = new Dictionary<string, string>();
                //dicKeyList.Add("outer_code", orderSysno.ToString());


                //ERP的合作者帐号
                string partner = "";
                //由ERP分配
                string send_key = "";
                //ERP的外部接口帐号
                string interface_id = "";
                //请求业务
                string method = "neworder";

                //将数组装换为json格式
                var orders = Hyt.Util.Serialization.JsonUtil.EntityToJson(order);
                //进行md5加密
                string md5 = MD5Encrypt(orders + send_key).ToLower();
                //进行Base64编码
                string base64 = EncodeBase64(md5);
                //进行url编码
                string sign = UrlEncode(base64);
                //请求参数
                string requiredParameter = "/" + method + "/?partner=" + partner + "&interface_id=" + interface_id + "&sign=" + sign + "&content=" + orders + ""; 
                //请求地址
                string requestUrl = "ht";
                //完整参数
                string url = requestUrl + requiredParameter;

                string testData = MyHttp.GetResponse(url, orders, "utf-8");
                //{"success":true,"errCode":"","errMsg":"","courierCode":"SF","mailNo":"444746031132"}
                PosDataResult postResult = JsonUtil.ToObject<PosDataResult>(testData);
                if (postResult.success)
                {
                    Hyt.BLL.CrossBorderLogistics.CrossBorderLogisticsOrderBo.Instance.InsertEntity(
                        new CrossBorderLogisticsOrder()
                        {
                            ExpressNo = postResult.courierCode + "_" + postResult.mailNo,
                            LogisticsCode = 0,
                            LogisticsOrderId = "",
                            SoOrderSysNo = orderSysno,
                        }
                    );
                    string express = postResult.courierCode + "_" + postResult.mailNo;
                    BLL.Order.SoOrderBo.Instance.UpdateOrderApiStatus((int)Hyt.Model.WorkflowStatus.OrderStatus.跨境物流推送状态.成功, 3, orderInfo.SysNo);
                    result.Status = postResult.success;
                }
                else
                {
                    result.Status = postResult.success;
                    result.Message = testData;
                }
            }
            catch (Exception e)
            {
                result.Status = false;
                result.Message = e.Message;
            }
            return result;
        }



        /// <summary>
        /// 取消订单
        /// </summary>
        /// <param name="orderSysNo"></param>
        /// <returns></returns>
        /// <remarks>2016-07-05 陈海裕 创建</remarks>
        public override Result CancelOrderTrade(int orderSysNo, string reason = "")
        {
            Result<string> result = new Result<string>();
            result.Status = false;
            result.StatusCode = 0;
            result.Message = "接口调用失败";

            if (orderSysNo <= 0)
            {
                return result;
            }

            try
            {


            }
            catch (Exception ex)
            {
                result.Message = "向" + this.Code + "物流取消订单报错：" + ex.StackTrace;
                return result;
            }

            return result;
        }


        /// <summary>
        /// 查询订单物流信息
        /// </summary>
        /// <param name="orderSysNo">订单号</param>
        /// <returns></returns>
        /// <remarks>2016-09-09 杨浩 创建</remarks>
        public override Result<string> GetLogisticsTracking(int orderSysNo)
        {
            var result = new Result<string>();
            result.Status = false;
            result.StatusCode = 0;
            result.Message = "接口调用失败";

            if (orderSysNo <= 0)
            {
                return result;
            }

            var orderInfo = BLL.Order.SoOrderBo.Instance.GetEntity(orderSysNo);

            return result;
        }



        /// <summary>
        /// 获取运单号
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        /// <remarks>2016-07-19 陈海裕 创建</remarks>
        public override Result GetOrderExpressno(string orderId)
        {
            Result<string> result = new Result<string>();
            result.Status = false;
            result.StatusCode = 0;
            result.Message = "接口调用失败";


            return result;

        }

        public override Result GetOrderTrade(string orderId)
        {
            throw new NotImplementedException();
        }

        public Result<string> GetBaseData(int type)
        {
            Result<string> result = new Result<string>();
            result.Status = false;
            result.StatusCode = 0;
            result.Message = "接口调用失败";



            return result;
        }




    }


}
