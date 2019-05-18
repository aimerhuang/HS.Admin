using Com.Alipay;
using Com.Alipay.Business;
using Com.Alipay.Domain;
using Com.Alipay.Model;
using Hyt.Model;
using Hyt.Model.Pos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extra.AliF2F
{
    public class BarcodePayControl
    {
        #region 条码支付功能
        

        /// <summary>
        /// 条码支付操作
        /// </summary>
        /// <param name="posOrder"></param>
        /// <returns></returns>
        public Result<AlipayF2FPayResult> AlipayDsPosOrder(DBDsPosOrder posOrder)
        {
            Result<AlipayF2FPayResult> resultMod = new Result<AlipayF2FPayResult>();
            IAlipayTradeService serviceClient = F2FBiz.CreateClientInstance(Config.serverUrl, Config.appId, Config.merchant_private_key, Config.version,
                            Config.sign_type, Config.alipay_public_key, Config.charset);

            AlipayF2FPayResult payResult = serviceClient.tradePay(BuildPayContent(posOrder));

            switch (payResult.Status)
            {
                case ResultEnum.SUCCESS:
                    DoSuccessProcess(payResult, ref resultMod);
                    break;
                case ResultEnum.FAILED:
                    DoFailedProcess(payResult, ref resultMod);
                    break;
                case ResultEnum.UNKNOWN:
                    resultMod.Status = false;
                    resultMod.Message= "网络异常，请检查网络配置后，更换外部订单号重试";
                    resultMod.Data = payResult;
                    break;
            }

            return resultMod;
            //Response.Redirect("result.aspx?Text=" + result);
        }
        private AlipayTradePayContentBuilder BuildPayContent(DBDsPosOrder posOrder)
        {
            
            //扫码枪扫描到的用户手机钱包中的付款条码
            AlipayTradePayContentBuilder builder = new AlipayTradePayContentBuilder();

            builder.out_trade_no = posOrder.SerialNumber;
            builder.scene = "bar_code";
            builder.auth_code = posOrder.PayAuthCode;
            builder.total_amount = (posOrder.TotalSellValue - posOrder.PointMoney).ToString("0.00");
            builder.discountable_amount = "0";
            builder.undiscountable_amount = posOrder.TotalSellValue.ToString("0.00");
            builder.operator_id = posOrder.Clerker;
            builder.subject = "条码支付";
            builder.timeout_express = "2m";
            builder.body = "订单商品描述";
            builder.store_id = posOrder.StoreName;    //很重要的参数，可以用作之后的营销     
            builder.seller_id = Config.pid;       //可以是具体的收款账号。
            
            //传入商品信息详情
            List<GoodsInfo> gList = new List<GoodsInfo>();
            foreach(DsPosOrderItem posItem in posOrder.items)
            {
                GoodsInfo goods = new GoodsInfo();
                goods.goods_id = posItem.ProSysNo.ToString();
                goods.goods_name = posItem.ProName;
                goods.price = posItem.ProTotalValue.ToString("0.00");
                goods.quantity = posItem.ProNum.ToString();
                gList.Add(goods);
            }
            builder.goods_detail = gList;
            //扩展参数
            //系统商接入可以填此参数用作返佣
            //ExtendParams exParam = new ExtendParams();
            //exParam.sysServiceProviderId = "20880000000000";
            //builder.extendParams = exParam;

            return builder;

        }

        /// <summary>
        /// 请添加支付成功后的处理
        /// </summary>
        private void DoSuccessProcess(AlipayF2FPayResult payResult, ref Result<AlipayF2FPayResult> resultMod)
        {

            //请添加支付成功后的处理
            resultMod.Status = true;
            resultMod.Message = "支付成功";
            resultMod.Data = payResult;
           // System.Console.WriteLine("支付成功");
            //result = payResult.response.Body;
        }

        private void DoFailedProcess(AlipayF2FPayResult payResult, ref Result<AlipayF2FPayResult> resultMod)
        {
            resultMod.Status = false;
            resultMod.Message = "支付失败";
            resultMod.Data = payResult;
            //请添加支付失败后的处理
            //System.Console.WriteLine("支付失败");
            //result = payResult.response.Body;
        }
        #endregion

        #region 条码支付退款
        public Result<AlipayF2FRefundResult> AlipayRefundDsPosOrder(CBDsPosReturnOrder returnOrder)
        {
            Result<AlipayF2FRefundResult> result = new Result<AlipayF2FRefundResult>();

            IAlipayTradeService serviceClient = F2FBiz.CreateClientInstance(Config.serverUrl, Config.appId, Config.merchant_private_key, Config.version,
                             Config.sign_type, Config.alipay_public_key, Config.charset);

            AlipayTradeRefundContentBuilder builder = BuildContent(returnOrder);


            AlipayF2FRefundResult refundResult = serviceClient.tradeRefund(builder);

            //请在这里加上商户的业务逻辑程序代码
            //——请根据您的业务逻辑来编写程序（以下代码仅作参考）——
            switch (refundResult.Status)
            {
                case ResultEnum.SUCCESS:
                    DoSuccessProcess(refundResult, ref result);
                    break;
                case ResultEnum.FAILED:
                    DoFailedProcess(refundResult, ref result);
                    break;
                case ResultEnum.UNKNOWN:
                    if (refundResult.response == null)
                    {
                        result.Message = "配置或网络异常，请检查";
                        //result = "配置或网络异常，请检查";
                    }
                    else
                    {
                        result.Message = "系统异常，请走人工退款流程";
                        //result = "系统异常，请走人工退款流程";
                    }
                    result.Data = refundResult;
                    result.Status = false;
                    break;
            }
            return result;
        }
        private AlipayTradeRefundContentBuilder BuildContent(CBDsPosReturnOrder returnOrder)
        {
            AlipayTradeRefundContentBuilder builder = new AlipayTradeRefundContentBuilder();

            //支付宝交易号与商户网站订单号不能同时为空
            builder.out_trade_no = returnOrder.AliPayNumber;

            //退款请求单号保持唯一性。
            builder.out_request_no = returnOrder.SerialNumber;

            //退款金额
            builder.refund_amount = returnOrder.TotalMayReturnValue.ToString("C");

            builder.refund_reason = "refund reason";

            return builder;

        }

        private void DoSuccessProcess(AlipayF2FRefundResult refundResult, ref Result<AlipayF2FRefundResult> result)
        {

            //请添加退款成功后的处理
            //result = refundResult.response.Body;
            result.Message = "退款成功";
            result.Data = refundResult;
            result.Status = true;
        }

        private void DoFailedProcess(AlipayF2FRefundResult refundResult, ref Result<AlipayF2FRefundResult> result)
        {

            //请添加退款失败后的处理
            //result = refundResult.response.Body;
            result.Message = "退款失败";
            result.Data = refundResult;
            result.Status = false;
        }
        #endregion

        #region 查询订单
        public Result<AlipayF2FQueryResult> GetAliDsPosOrderQuery(string out_trade_no)
        {
            Result<AlipayF2FQueryResult> result = new Result<AlipayF2FQueryResult>();

            IAlipayTradeService serviceClient = F2FBiz.CreateClientInstance(Config.serverUrl, Config.appId, Config.merchant_private_key, Config.version,
                             Config.sign_type, Config.alipay_public_key, Config.charset);

            AlipayF2FQueryResult queryResult = serviceClient.tradeQuery(out_trade_no);

            //请在这里加上商户的业务逻辑程序代码
            //——请根据您的业务逻辑来编写程序（以下代码仅作参考）——
            switch (queryResult.Status)
            {
                case ResultEnum.SUCCESS:
                    DoSuccessProcess(queryResult, ref result);
                    break;
                case ResultEnum.FAILED:
                    DoFailedProcess(queryResult, ref result);
                    break;
                case ResultEnum.UNKNOWN:
                    result.Status = false;
                    if (queryResult.response == null)
                    {
                        //result = "网络异常，请检查网络配置";
                        //result = "配置或网络异常，请检查";
                        result.Message = "配置或网络异常，请检查";
                    }
                    else
                    {
                        //result = "系统异常，请重试";
                        result.Message = "系统异常，请重试";
                    }
                    result.Data = queryResult;
                    break;
            }
            return result;
        }
        /// <summary>
        /// 返回数据失败原因
        /// </summary>
        /// <param name="queryResult"></param>
        /// <param name="result"></param>
        private void DoFailedProcess(AlipayF2FQueryResult queryResult, 
            ref Result<AlipayF2FQueryResult> result)
        {
            result.Message = "退款失败";
            result.Data = queryResult;
            result.Status = true;
        }
        /// <summary>
        /// 返回数据成功原因
        /// </summary>
        /// <param name="queryResult"></param>
        /// <param name="result"></param>
        private void DoSuccessProcess(AlipayF2FQueryResult queryResult, 
            ref Result<AlipayF2FQueryResult> result)
        {
            result.Message = "退款成功";
            result.Data = queryResult;
            result.Status = false;
        }
        #endregion
    }
}
