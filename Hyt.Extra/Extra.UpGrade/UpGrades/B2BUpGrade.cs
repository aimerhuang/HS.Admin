using Extra.UpGrade.Model;
using Extra.UpGrade.Provider;
using Hyt.Model;
using Hyt.Model.Order;
using Hyt.Model.UpGrade;
using Hyt.Util;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace Extra.UpGrade.UpGrades
{
    /// <summary>
    /// 国内货站订单实现
    /// </summary>
    /// 2018-3-15 吴琨 创建
    public class B2BUpGrade : IUpGrade
    {

        private static string url = "http://112.74.67.53:8099";
        private static string key = "a7d8d9cf17654172b422d4126a55f615";
        /// <summary>
        /// 获取已升舱待发货订单
        /// </summary>
        /// <param name="param"></param>
        /// <param name="auth"></param>
        /// <returns></returns>
        public Result<List<UpGradeOrder>> GetUpGradedWaitSend(OrderParameters param, AuthorizationParameters auth)
        {
            var result = new Result<List<UpGradeOrder>>();
            try
            {              
                var list = new List<UpGradeOrder>();
                var request = (HttpWebRequest)WebRequest.Create(url + "/Order/getOrder?Key=" + key + "&paymentStatus=20&Status=60");
                var response = (HttpWebResponse)request.GetResponse();
                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                var trade = JObject.Parse(responseString);
                if (!Convert.ToBoolean(trade["Status"].ToString()))
                {
                    return result;
                }

                for (int i = 0; i < trade["Data"].Count(); i++)
                {
                    UpGradeOrder data = new UpGradeOrder();
                    //商城订单相关信息
                    HytOrderDealerInfo f = new HytOrderDealerInfo();
                    f.DealerSysNo = 727;
                    f.DealerMallSysNo = 30;
                    f.ShopName = "货栈";
                    f.HytPayStatus = 20;
                    f.HytPayment = Convert.ToDecimal(trade["Data"][i]["OrderAmount"].ToString());
                    f.HytOrderId = trade["Data"][i]["OrderCode"].ToString();
                    f.OrderTransactionSysNo = "";
                    f.DeliveryStatus = "待配送";
                    f.HytOrderTime = Convert.ToDateTime(trade["Data"][i]["AddDate"].ToString());
                    f.IsSelfSupport = 1;
                    f.HytPayType = Hyt.Model.SystemPredefined.PaymentType.分销商预存;
                    data.HytOrderDealer = f;
                    //第三方卖家信息
                    MallOrderBuyerInfo m = new MallOrderBuyerInfo();
                    m.MallOrderId = trade["Data"][i]["OrderCode"].ToString();
                    m.BuyerNick = "货栈";
                    data.MallOrderBuyer = m;
                    //订单收货信息
                    MallOrderReceiveInfo s = new MallOrderReceiveInfo();
                    s.IdCard = trade["Data"][i]["ConsigneeIDCardNo"].ToString();
                    s.ReceiveContact = trade["Data"][i]["ConsigneeName"].ToString();
                    s.Mobile = trade["Data"][i]["ConsigneeMobile"].ToString();
                    s.Province = trade["Data"][i]["Province"].ToString();
                    s.City = trade["Data"][i]["City"].ToString();
                    s.District = trade["Data"][i]["District"].ToString();
                    s.ReceiveAddress = trade["Data"][i]["ConsigneeAddress"].ToString();
                    data.MallOrderReceive = s;
                    if (data.UpGradeOrderItems == null)
                        data.UpGradeOrderItems = new List<UpGradeOrderItem>();

                    //订单明细列表
                    for (int ii = 0; ii < trade["Data"][i]["Product"].Count(); ii++)
                    {
                        var r = trade["Data"][i]["Product"][ii];
                        AddB2CProduct(r["ProductCode"].ToString());
                        UpGradeOrderItem mx = new UpGradeOrderItem();
                        mx.OrderId = trade["Data"][i]["OrderCode"].ToString();
                        mx.MallProductName = r["ProductName"].ToString();
                        mx.MallProductCode = r["ProductCode"].ToString();
                        mx.HytProductErpCode = r["ProductCode"].ToString();
                        mx.HytProductName = r["ProductName"].ToString();
                        mx.HytPrice = Convert.ToDecimal(r["OriginalUnitPrice"].ToString());
                        mx.MallPrice = Convert.ToDecimal(r["OriginalUnitPrice"].ToString());
                        mx.MallAmount = Convert.ToDecimal(r["SalesAmount"].ToString());
                        mx.Quantity = Convert.ToInt32(r["Count"].ToString()) * GetNumberInt(r["Specifications"].ToString());
                        mx.DiscountFee = 0;
                        mx.ProductSalesType = 10;
                        data.UpGradeOrderItems.Add(mx);
                    }
                    list.Add(data);
                }
                result.Status = true;
                result.Data = list;
            }
            catch (Exception ex)
            {
                  
                result.Status = false;
                result.Message = ex.Message;
            }

            return result;
        }


        /// <summary>
        /// 批量获取指定时间区间的订单
        /// </summary>
        /// <param name="param"></param>
        /// <param name="auth"></param>
        /// <returns></returns>
        public Result<List<UpGradeOrder>> GetOrderList(OrderParameters param, AuthorizationParameters auth)
        {
            Result<List<UpGradeOrder>> result = new Result<List<UpGradeOrder>>();
            List<UpGradeOrder> list = new List<UpGradeOrder>();
            var urlstr = url + "/Order/getOrder?Key=" + key + "&paymentStatus=20&Status=60";
            urlstr += "&beginDate=" + param.StartDate;
            urlstr += "&endDate=" + param.EndDate;
            var request = (HttpWebRequest)WebRequest.Create(urlstr);
            var response = (HttpWebResponse)request.GetResponse();
            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
            var trade = JObject.Parse(responseString);
            if (!Convert.ToBoolean(trade["Status"].ToString()))
            {
                result.Message = trade["Message"].ToString();
                result.Status = false;
                return result;
            }
            #region 读取订单
            for (int i = 0; i < trade["Data"].Count(); i++)
            {
                var data = new UpGradeOrder();
                //商城订单相关信息
                HytOrderDealerInfo f = new HytOrderDealerInfo();
                f.DealerSysNo = 727;
                f.DealerMallSysNo = 30;
                f.ShopName = "货栈";
                f.HytPayStatus = 20;

                f.HytPayment = Convert.ToDecimal(trade["Data"][i]["OrderAmount"].ToString());
                f.HytOrderId = trade["Data"][i]["OrderCode"].ToString();
                f.OrderTransactionSysNo = "";
                f.DeliveryStatus = "待配送";
                f.HytOrderTime = Convert.ToDateTime(trade["Data"][i]["AddDate"].ToString());
                f.IsSelfSupport = 1;
                data.HytOrderDealer = f;
                //第三方卖家信息
                var m = new MallOrderBuyerInfo();
                m.MallOrderId = trade["Data"][i]["OrderCode"].ToString();
                m.BuyerNick = "货栈";
                data.MallOrderBuyer = m;
                //订单收货信息
                MallOrderReceiveInfo s = new MallOrderReceiveInfo();
                s.IdCard = trade["Data"][i]["ConsigneeIDCardNo"].ToString();
                s.ReceiveContact = trade["Data"][i]["ConsigneeName"].ToString();
                s.Mobile = trade["Data"][i]["ConsigneeMobile"].ToString();
                s.Province = trade["Data"][i]["Province"].ToString();
                s.City = trade["Data"][i]["City"].ToString();
                s.District = trade["Data"][i]["District"].ToString();
                s.ReceiveAddress = trade["Data"][i]["ConsigneeAddress"].ToString();
                data.MallOrderReceive = s;
                if (data.UpGradeOrderItems == null)
                    data.UpGradeOrderItems = new List<UpGradeOrderItem>();
                var paymentInfo = new MallOrderPaymentInfo();
                paymentInfo.Payment = f.HytPayment;
                paymentInfo.PostFee = Convert.ToDecimal(trade["Data"][i]["FreightAmount"].ToString());
                if (trade["Data"][i]["fnReceiptVoucher"].Count()>0)
                {
                    paymentInfo.Payment = Convert.ToDecimal(trade["Data"][i]["fnReceiptVoucher"]["Amount"].ToString());
                    paymentInfo.PayTime = Convert.ToDateTime(trade["Data"][i]["fnReceiptVoucher"]["CreatedDate"].ToString());
                    paymentInfo.AlipayNo = trade["Data"][i]["fnReceiptVoucher"]["VoucherNo"].ToString();
                    paymentInfo.TotalTaxAmount = Convert.ToDecimal(trade["Data"][i]["TaxFee"].ToString());
                }
                else
                {
                    result.Status = false;
                    result.Message = "接口获取的订单号【"+f.HytOrderId+"】没有支付信息！";
                    return result;
                }
                data.MallOrderPayment = paymentInfo;
                //订单明细列表
                for (int ii = 0; ii < trade["Data"][i]["Product"].Count(); ii++)
                {
                    var r = trade["Data"][i]["Product"][ii];
                    AddB2CProduct(r["ProductCode"].ToString());
                    UpGradeOrderItem mx = new UpGradeOrderItem();
                    mx.OrderId = trade["Data"][i]["OrderCode"].ToString();
                    mx.MallProductName = r["ProductName"].ToString();
                    mx.MallProductCode = r["ProductCode"].ToString();
                    mx.HytProductErpCode = r["ProductCode"].ToString();
                    mx.HytProductName = r["ProductName"].ToString();
                    mx.HytPrice = Convert.ToDecimal(r["OriginalUnitPrice"].ToString());
                    mx.MallPrice = Convert.ToDecimal(r["OriginalUnitPrice"].ToString());
                    mx.MallAmount = Convert.ToDecimal(r["SalesAmount"].ToString());
                    mx.Quantity = Convert.ToInt32(r["Count"].ToString()) * GetNumberInt(r["Specifications"].ToString());
                    mx.DiscountFee = 0;
                    mx.ProductSalesType = 10;
                    data.UpGradeOrderItems.Add(mx);
                }
                list.Add(data);
            }
            #endregion

            result.Status = true;
            result.Data = list;
            return result;
        }

        /// <summary>
        ///  获取单笔订单详情 
        /// </summary>
        /// <param name="param"></param>
        /// <param name="auth"></param>
        /// <returns></returns>
        public Result<UpGradeOrder> GetOrderDetail(OrderParameters param, AuthorizationParameters auth)
        {
            return null;
        }

        /// <summary>
        /// 用户调用该接口可实现自己联系发货（线下物流）
        /// </summary>
        /// <param name="param"></param>
        /// <param name="auth"></param>
        /// <returns></returns>
        public Result SendDelivery(DeliveryParameters param, AuthorizationParameters auth)
        {
            Result result = new Result();
            var urlstr = url + "/Order/addShipGoods";
            var parameters = new NameValueCollection { 
                { "orderCode", param.MallOrderId }, 
                { "expressNo",param.HytExpressNo }, 
                { "key", key }
            };
            var rtn = HttpHelper.PostData(urlstr, parameters);
            var json = JObject.Parse(rtn);
            result.Status=Convert.ToBoolean(json["Status"].ToString());
            result.Message = Convert.ToString(json["Message"].ToString());
            result.StatusCode = Convert.ToInt32(json["StatusCode"].ToString());
            return result;
        }

        /// <summary>
        /// 获取可合并升舱订单列表
        /// </summary>
        /// <param name="param"></param>
        /// <param name="auth"></param>
        /// <returns></returns>
        public Result<List<UpGradeOrder>> GetCombineOrders(OrderParameters param, AuthorizationParameters auth)
        {
            return null;
        }

        /// <summary>
        /// 使用授权码获取登录令牌
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public Result<AccessTokenResult> GetAuthorizationCode(string code)
        {
            return null;
        }

        /// <summary>
        /// 更新订单备注信息
        /// </summary>
        /// <param name="remarks"></param>
        /// <param name="auth"></param>
        /// <returns></returns>
        public Result UpdateTradeRemarks(IRemarksParameters remarks, AuthorizationParameters auth)
        {
            return null;
        }


        /// <summary>
        /// 根据商品编号调用接口判断是否在B2C存在此商品，若不存在则接口自动创建 
        /// </summary>
        /// <param name="ErpCode"></param>
        /// 2018-3-29 吴琨 创建
        private void AddB2CProduct(string ErpCode)
        {
            if (!string.IsNullOrEmpty(ErpCode))
            {
                var pdrequest = (HttpWebRequest)WebRequest.Create(url + "/Order/AddPdProduct?ErpCode=" + ErpCode);
                var pdresponse = (HttpWebResponse)pdrequest.GetResponse();
                var pdresponseString = new StreamReader(pdresponse.GetResponseStream()).ReadToEnd();
            }
        }

        /// <summary>
        /// 获取字符串中的数字
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>数字</returns>
        private static int GetNumberInt(string str)
        {
            int result = 0;
            if (str != null && str != string.Empty)
            {
                // 正则表达式剔除非数字字符（不包含小数点.）
                str = Regex.Replace(str, @"[^\d.\d]", "");
                // 如果是数字，则转换为decimal类型
                if (Regex.IsMatch(str, @"^[+-]?\d*[.]?\d*$"))
                {
                    result = int.Parse(str);
                }
            }
            return result;
        }


        public Result<UpGradeExpress> GetExpress(AuthorizationParameters auth)
        {
            throw new NotImplementedException();
        }
    }
}
