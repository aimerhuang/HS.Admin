using Hyt.BLL.QianDai.Extends;
using Hyt.BLL.Finance;
using Hyt.Model;
using Hyt.Model.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web.Script.Serialization;
using Hyt.Model.VipCard.QianDai;
using Newtonsoft.Json.Linq;
using System.Collections.Specialized;
using Hyt.BLL.VipCard;
using Hyt.Model.Generated;

namespace Hyt.BLL.ApiPay.QianDai
{
    /// <summary>
    /// 钱袋宝
    /// </summary>
    /// <remarks>2016-11-04 杨浩 添加注释</remarks>
    public class PayProvider : IPayProvider
    {
        private QianDaiPayConfig config = Hyt.BLL.Config.Config.Instance.GetQianDaiPayConfig();
        public override Model.CommonEnum.PayCode Code
        {
            get { return CommonEnum.PayCode.钱袋宝; }           
        }

        public override Result RestartPushCustomsOrder(int orderId)
        {
            return base.RestartPushCustomsOrder(orderId);
        }
        public override Model.Result ApplyToCustoms(Model.SoOrder order)
        {
            Result result = new Result();
            try
            {
                var list = FinanceBo.Instance.GetOnlinePaymentList(order.SysNo);
                FnOnlinePayment payment;
                if (list.Count > 0)
                {
                    payment = list[0];
                }
                else
                {
                    result.Status = false;
                    result.Message = "付款单信息无效，请核实订单是否付款？";
                    return result;
                }

                var baoguanArray = new Object[1];
                baoguanArray[0] = new
                {
                    no_order = payment.BusinessOrderSysNo,      //商户网站的订单号；如果是拆单报关模式，填写：子订单号

                    e_commerce_code = config.EbcCode,
                    e_commerce_name = config.EbcName,
                    pay_amount = order.OrderAmount,         	//支付金额，拆分后的支付金额之和不能超过订单金额
                    goods_amount = order.ProductAmount,       	//报关货款
                    tax_amount = order.TaxFee,           //报关税款
                    freight = order.FreightAmount,              //报关运费
                    biz_type_code = "2",        //业务类型：直购进口：1,网购保税进口：2
                    baojian = config.ICPType,				//留空表示不报检，如果需要报检，请传报检地 如：nansha
                    baojian_no = config.ICPCode,				//报检时必填,商户网站在报检局取得的备案号
                    baojian_name = config.ICPName,			//报检时必填,商户网站在报检局备案的企业名称
                    app_type = "1",				//报送类型，1表示新增，2表示变更
                    channel = config.EbcType					//报送通道，1表示总署版
                };

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                string baoguan = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(serializer.Serialize(baoguanArray)));

                SortedDictionary<string, string> sParaTemp = new SortedDictionary<string, string>();
                sParaTemp.Add("partner_id", config.Partner);
                sParaTemp.Add("charset_name", config.Charset_name.ToLower());
                sParaTemp.Add("sign_type", config.Sign_type);

                sParaTemp.Add("url_custom", config.Server_BackUrl);
                sParaTemp.Add("no_order", payment.BusinessOrderSysNo);
                sParaTemp.Add("baoguan", baoguan);

                result.Message = new Submit().BuildRequest(sParaTemp, "baoguan");
                QDResult qdresult = Hyt.Util.Serialization.JsonUtil.ToObject<QDResult>(result.Message);
                if (qdresult == null)
                {
                    result.Status = false;                 
                    return result;
                }
                //-554代表 订单重复,无法新增报关
                if (qdresult.ret_code == 0 || qdresult.ret_code == -554)
                {
                    Hyt.BLL.Order.SoOrderBo.Instance.UpdateOrderApiStatus(10, 0, order.SysNo);
                    result.Status = true;
                }
                else
                {
                    result.Status = false ;
                    result.Message = qdresult.ret_message;
                }
                return result;
            }
            catch(Exception e)
            {
                result.Status = false;
                result.Message = e.Message;
                return result;
            }
           
        }

        public override Model.Result CustomsQuery(int orderId)
        {
            Result result = new Result() {
                Status=false,
                Message="无法主动获取，钱袋宝收到回执后会主动调用我们提供的接口通知。"
            };
            return result;
        }

        #region 电子会员卡   
        /// <summary>
        /// 会员卡创建
        /// </summary>
        /// <param name="account">用户信息</param>
        /// <returns></returns>
        /// <remarks>2017-02-08 杨浩 创建</remarks>
        public override Result VipCardCreate(object obj)
        {
            var result = base.VipCardCreate(obj);
            var vipCard = (CrQianDaiVipCard)obj;

            var sParaTemp = new SortedDictionary<string, string>();
            sParaTemp.Add("partner_id", config.Partner);
            sParaTemp.Add("charset_name", config.Charset_name.ToLower());
            sParaTemp.Add("sign_type", config.Sign_type);

            sParaTemp.Add("user_name", vipCard.UserName);
            sParaTemp.Add("id_no", vipCard.IdNo);
            sParaTemp.Add("phone", vipCard.Phone);
           
            
            string responsestr = new Submit().BuildRequest(sParaTemp, "vipCard/create");
            if (responsestr.StartsWith("报错："))
            {
                result.StatusCode = 1;
                result.Message = responsestr;
                return result;
            }
           
            var back = JObject.Parse(responsestr);
            if (back["ret_code"].ToString() == "0")
            {
                vipCard.CardId = int.Parse(back["card_id"].ToString());

                if (BLL.VipCard.QianDaiVipCardBo.Instance.GetVipCardByCardId(vipCard.CardId) == null)
                {
                    var vipCardInfo = BLL.VipCard.QianDaiVipCardBo.Instance.CreateVipCard(vipCard);

                    if (vipCardInfo != null && vipCardInfo.SysNo > 0)
                    {
                        result.Status = true;
                    }
                }
                else
                {
                    result.Status = true;
                    result.StatusCode = 2;                
                }

                result.Message = vipCard.CardId.ToString();
            }
            else
            {
                result.StatusCode = 3;
                result.Message = back["ret_message"].ToString();
            }

            return result;
        }

       
        /// <summary>
        /// 会员卡充值
        /// </summary>
        /// <param name="rechargeNo">充值流水号</param>
        /// <param name="cardId">会员卡号</param>
        /// <param name="money">充值金额</param>
        /// <returns></returns>
        /// <remarks>2017-02-08 杨浩 创建</remarks>
        public override Result VipCardRecharge(string rechargeNo, int cardId, decimal money)
        {
            var result = base.VipCardRechargeResult(rechargeNo);
            var sParaTemp = new SortedDictionary<string, string>();
            sParaTemp.Add("partner_id", config.Partner);
            sParaTemp.Add("charset_name", config.Charset_name.ToLower());
            sParaTemp.Add("sign_type", config.Sign_type);

            sParaTemp.Add("recharge_no", rechargeNo);
            sParaTemp.Add("card_id", cardId.ToString());
            sParaTemp.Add("money", money.ToString());
            sParaTemp.Add("notify_url", "http://xrc.com/QianDaiVipCard/VipCardRechargeNotify");

            string responsestr = new Submit().BuildRequest(sParaTemp, "vipCard/recharge");
            if (responsestr.StartsWith("报错："))
            {
                result.StatusCode = 1;
                result.Message = responsestr;
                return result;
            }

            var back = JObject.Parse(responsestr);
            if (back["ret_code"].ToString() != "0")
            {           
                result.StatusCode = 3;
                result.Message = back["ret_message"].ToString() + "-" + money.ToString();
            }
            else
            {
                var qianDaiVipCardRechargeLog = new CrQianDaiVipCardRechargeLog()
                {
                    RechargeNo =rechargeNo,
                    CardId = cardId,
                    Money = money,
                    CreateDate=DateTime.Now,
                    CreatedBy=BLL.Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo,
                    Status="-2",
                };

                QianDaiVipCardRechargeLogBo.Instance.CreateCrQianDaiVipCardRechargeLog(qianDaiVipCardRechargeLog);
            }

            return result;
        }

        /// <summary>
        /// 充值异步
        /// </summary>
        /// <param name="sPara">参数</param>
        /// <param name="rechargeNo">交易流水号</param>
        /// <param name="sign">签名</param>
        /// <returns></returns>
        /// <remarks>2017-02-08 杨浩 创建</remarks>
        public override Result VipCardRechargeNotify(SortedDictionary<string,string> sPara, string rechargeNo, string sign)
        {
            var result = base.VipCardRechargeNotify(sPara,rechargeNo, sign);
            if (sPara.Count > 0)//判断是否有带返回参数
            {
                var qiandaibaoNotify = new Notify();
                bool verifyResult = qiandaibaoNotify.Verify(sPara, rechargeNo, sign);
                if (verifyResult)//验证成功
                {

                    result.Message = "success";//请不要修改或删除

                    if (sPara["ret_code"].ToString() != "0")
                    {
                        result.StatusCode = 3;
                    }
                    else
                    {

                        if (sPara["status"].ToString() == "1")
                            result.StatusCode = 0;

                        var model = BLL.VipCard.QianDaiVipCardRechargeLogBo.Instance.GetQianDaiVipCardRechargeLogByRechargeNo(rechargeNo);

                        if (sPara.Keys.Contains("shop_balance"))
                            model.ShopBalance = decimal.Parse(sPara["shop_balance"]);
                        if (sPara.Keys.Contains("card_balance"))
                            model.CardBalance = decimal.Parse(sPara["card_balance"]);

                        model.RetMessage = sPara["ret_message"];
                        model.Status = sPara["status"];
                        model.LastUpdateDate = DateTime.Now;

                        BLL.VipCard.QianDaiVipCardRechargeLogBo.Instance.Update(model);
                    }
                    /////////////////////////////////////////////////////////////////////////////////////////////////////////////
                }
                else//验证失败
                {
                    result.Message = "fail";
                }
            }
            else
            {
                result.Message = "无通知参数";
            }

            return result;
        }
        /// <summary>
        /// 充值结果
        /// </summary>
        /// <param name="rechargeNo">充值流水号</param>
        /// <returns></returns>
        /// <remarks>2017-02-08 杨浩 创建</remarks>
        public override Result VipCardRechargeResult(string rechargeNo)
        {
            var result = new Result();

            var model = BLL.VipCard.QianDaiVipCardRechargeLogBo.Instance.GetQianDaiVipCardRechargeLogByRechargeNo(rechargeNo);

            if (model == null)
            {
                result.StatusCode = 1;
                result.Status = false;
                result.Message = "充值流水号不存在！";
                return result;
            }
            var sParaTemp = new SortedDictionary<string, string>();
            sParaTemp.Add("partner_id", config.Partner);
            sParaTemp.Add("charset_name", config.Charset_name.ToLower());
            sParaTemp.Add("sign_type", config.Sign_type);

            sParaTemp.Add("recharge_no", rechargeNo);

            string responsestr = new Submit().BuildRequest(sParaTemp, "vipCard/rechargeResult");
            if (responsestr.StartsWith("报错："))
            {
                result.StatusCode = 1;
                result.Message = responsestr;
                return result;
            }

            var back = JObject.Parse(responsestr);
            result.Message = back["ret_message"].ToString();

            if (back["ret_code"].ToString() != "0")
            {
                result.StatusCode = 3;
            }
            else
            {

                if (back["status"].ToString() == "1")
                    result.StatusCode = 0;

                if(back.Property("shop_balance")!=null)
                    model.ShopBalance = decimal.Parse(back["shop_balance"].ToString());
                if (back.Property("card_balance") != null)
                    model.CardBalance = decimal.Parse(back["card_balance"].ToString());
                model.RetMessage = back["ret_message"].ToString();
                model.Status = back["status"].ToString();
                model.LastUpdateDate = DateTime.Now;

                BLL.VipCard.QianDaiVipCardRechargeLogBo.Instance.Update(model);
            }
            return result;
        }

        /// <summary>
        /// 会员卡查询
        /// </summary>
        /// <param name="obj">查询参数</param>
        /// <returns></returns>
        /// <remarks>2017-02-08 杨浩 创建</remarks>
        public override Result VipCardQuery(object obj)
        {
            var result =new Result<string>();
            var sParaTemp = new SortedDictionary<string, string>();
            sParaTemp.Add("partner_id", config.Partner);
            sParaTemp.Add("charset_name", config.Charset_name.ToLower());
            sParaTemp.Add("sign_type", config.Sign_type);
            var vipCardInfo = (CrQianDaiVipCard)obj;
            if(vipCardInfo.CardId>0)
                sParaTemp.Add("card_id", vipCardInfo.CardId.ToString());

            if (!string.IsNullOrWhiteSpace(vipCardInfo.IdNo))
                sParaTemp.Add("id_no", vipCardInfo.IdNo);


            if (!string.IsNullOrWhiteSpace(vipCardInfo.Phone))
                sParaTemp.Add("phone", vipCardInfo.Phone);

            sParaTemp.Add("page_size","1");
            sParaTemp.Add("page_no", "1");
            
            string responsestr = new Submit().BuildRequest(sParaTemp, "vipCard/query");
            if (responsestr.StartsWith("报错："))
            {
                result.StatusCode = 1;
                result.Message = responsestr;
                return result;
            }

            var back = JObject.Parse(responsestr);
            if (back["ret_code"].ToString() != "0")
            {
                result.StatusCode = 3;
                result.Message = back["ret_message"].ToString();
            }
            else
            {
                result.StatusCode = 0;
                result.Status = true;
                result.Data = back["items"].ToString();
            }
            return result;
        }

      
        /// <summary>
        /// 会员卡消费
        /// </summary>
        /// <param name="noOrder">商户订单号</param>
        /// <param name="cardId">会员卡号</param>
        /// <returns></returns>
        /// <remarks>2017-4-2 杨浩 创建</remarks>
        public override Result VipCardConsume(string noOrder, string cardId)
        {
            var result = new Result();
            var sParaTemp = new SortedDictionary<string, string>();
            sParaTemp.Add("partner_id", config.Partner);
            sParaTemp.Add("charset_name", config.Charset_name.ToLower());
            sParaTemp.Add("sign_type", config.Sign_type);

            sParaTemp.Add("no_order", noOrder);
            sParaTemp.Add("card_id", cardId);
            sParaTemp.Add("ip",Hyt.Util.WebUtil.GetUserIp());

            string responsestr = new Submit().BuildRequest(sParaTemp, "vipCard/consume");
            if (responsestr.StartsWith("报错："))
            {
                result.StatusCode = 1;
                result.Message = responsestr;
                return result;
            }

            var back = JObject.Parse(responsestr);
            result.Message = back["ret_message"].ToString();

            if (back["ret_code"].ToString() != "0")
            {
                result.StatusCode = 3;
            }
            else
            {
                result.Status = true;
                result.StatusCode = 0;
            }
            return result;
        }
        /// <summary>
        /// 支付结果异步
        /// </summary>
        /// <param name="sPara">参数</param>
        /// <returns></returns>
        /// <remarks>2017-04-02 杨浩 创建</remarks>
        public override Result PayNotify(SortedDictionary<string, string> sPara)
        {
            var result = new Result();

            if (sPara.Count > 0)//判断是否有带返回参数
            {
                var qiandaibaoNotify = new Notify();

                bool verifyResult = qiandaibaoNotify.Verify(sPara, sPara["notify_id"], sPara["sign"]);

                if (verifyResult)//验证成功
                {
                    /////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    //请在这里加上商户的业务逻辑程序代码
                    //——请根据您的业务逻辑来编写程序（以下代码仅作参考）——
                    //获取钱袋宝的通知返回参数，可参考技术文档中服务器异步通知参数列表

                    //商户订单号
                    string no_order = sPara["no_order"];
                    //钱袋宝交易号
                    string back_order_no = sPara["back_order_no"];
                    //交易状态
                    string result_pay = sPara["result_pay"];


                    if (sPara["result_pay"] == "success")
                    {
                        //判断该笔订单是否在商户网站中已经做过处理
                        //如果没有做过处理，根据订单号（no_order）在商户网站的订单系统中查到该笔订单的详细，并执行商户的业务程序
                        //请务必判断请求时的money_order、partner_id与通知时获取的money_order、partner_id为一致的
                        //如果有做过处理，不执行商户的业务程序

                        var payment = new FnOnlinePayment();

                        var dateTime = DateTime.Now;

                        payment.CreatedDate = dateTime;
                        payment.CreatedBy = 0;
                        payment.LastUpdateDate = dateTime;
                        payment.OperatedDate = dateTime;
                        payment.Operator = 0;
                        payment.PaymentTypeSysNo = Hyt.Model.SystemPredefined.PaymentType.钱袋宝;
                        payment.SourceSysNo = Convert.ToInt32(sPara["no_order"]);
                        payment.Amount = decimal.Parse(sPara["money_order"]);
                        payment.VoucherNo = sPara["back_order_no"];                   
                        payment.BusinessOrderSysNo = no_order;
                        //数据验证成功,写付款单
                        var _result = FinanceBo.Instance.UpdateOrderPayStatus(payment, Hyt.Model.SystemPredefined.PaymentType.钱袋宝);
                        result.Status = true;
                        result.StatusCode = 0;
                        result.Message = "success";
                        BLL.Order.SoOrderPayLogBo.Instance.UpdateOrderPayLogStatus(payment.SourceSysNo, Hyt.Model.SystemPredefined.PaymentType.钱袋宝, 20);
                        return result;                                             
                    }
                    else
                    {
                        result.StatusCode = 1;
                        result.Message = "fail-》" + sPara["ret_ message"];
                    }
                }
                else//验证失败
                {
                    result.StatusCode =2;
                    result.Message = "fail_2";
                }
            }
            else
            {
                result.StatusCode = 3;
                result.Message = "fail_3";
            }

            return result;
        }
        /// <summary>
        /// 提交订单到支付方
        /// </summary>
        /// <param name="order">订单实体</param>
        /// <param name="orderPayLogSysNo">订单支付日志系统编号</param>
        /// <returns></returns>
        /// <remarks>2017-04-02 杨浩 创建</remarks>
        public override Result SubmitOrderToPay(SoOrder order,int  orderPayLogSysNo=0)
        {
            var result = new Result();

            var vipCardInfo=BLL.VipCard.QianDaiVipCardBo.Instance.GetVipCardByCustomerSysNo(order.CustomerSysNo);
            if(vipCardInfo==null)
            {

                //result=VipCardCreate(null);
                //if (result.Status)
                //{
                //    result.Message;
                //}
                result.StatusCode = 1;
                result.Message ="订单："+order.SysNo+"所属会员没有绑定会员卡，请绑定之后再试！";
                return result;
            }

            var receiveAddress = Hyt.BLL.Order.SoOrderBo.Instance.GetOrderReceiveAddress(order.ReceiveAddressSysNo);

            var sParaTemp = new SortedDictionary<string, string>();
            sParaTemp.Add("partner_id", config.Partner);
            sParaTemp.Add("charset_name", config.Charset_name.ToLower());
            sParaTemp.Add("sign_type", config.Sign_type);

      

            sParaTemp.Add("notify_url", "http://120.25.160.10:1010/QianDaiVipCard/PayNotify");
            sParaTemp.Add("url_return", "http://cx.com/7");
            sParaTemp.Add("no_order", order.SysNo.ToString());
            sParaTemp.Add("name_goods", "会员卡支付商品");
            sParaTemp.Add("money_order", order.OrderAmount.ToString("F2"));
            sParaTemp.Add("info_order", "会员卡支付商品");
            sParaTemp.Add("url_order", "http://cx.com/3");
            sParaTemp.Add("timestamp", "");
            sParaTemp.Add("dt_order", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            sParaTemp.Add("valid_order", "86400");
            sParaTemp.Add("buyer_id", order.CustomerSysNo.ToString());
            sParaTemp.Add("user_name", receiveAddress.Name);
            sParaTemp.Add("id_no", receiveAddress.IDCardNo);
            sParaTemp.Add("phone", receiveAddress.MobilePhoneNumber);
            sParaTemp.Add("id_type", "身份证");
            sParaTemp.Add("is_import","1");
            sParaTemp.Add("platform", "pc");
            sParaTemp.Add("flag", order.SysNo.ToString());
            sParaTemp.Add("return_type", "json");//输出方式，创建订单后接口输出内容方式，可选项(html,json)
            sParaTemp.Add("pay_type", "vcard");

      
            string responsestr = new Submit().BuildRequest(sParaTemp, "order");

            if (responsestr.StartsWith("报错："))
            {
                result.StatusCode = 1;
                result.Message = responsestr;
                return result;
            }

            var back = JObject.Parse(responsestr);
            result.Message = back["ret_message"].ToString();

            if (back["ret_code"].ToString() != "0")
            {
                result.StatusCode = 3;
            }
            else
            {

                result=VipCardConsume(order.SysNo.ToString(), vipCardInfo.CardId.ToString());

                var orderPayLog = new SoOrderPayLog()
                {
                    OrderSysNo = order.SysNo,
                    PaymentTypeSysNo = Hyt.Model.SystemPredefined.PaymentType.钱袋宝,
                    Status = 10,
                    SubmitOrderNumber = order.SysNo.ToString(),
                };


                if (orderPayLogSysNo <= 0)               
                    BLL.Order.SoOrderPayLogBo.Instance.InsertEntity(orderPayLog);                               
            }
            return result;
        }

        /// <summary>
        /// 会员卡提现
        /// </summary>
        /// <param name="withdrawNo">提现流水号，要求唯一性 </param>
        /// <param name="cardId">卡号 </param>
        /// <param name="money">提现金额</param>
        /// <returns></returns>
        /// <remarks>2017-02-08 杨浩 创建</remarks>
        public override Result VipCardWithdraw(string withdrawNo, int cardId, decimal money)
        {
            var result = new Result();
            var sParaTemp = new SortedDictionary<string, string>();
            sParaTemp.Add("partner_id", config.Partner);
            sParaTemp.Add("charset_name", config.Charset_name.ToLower());
            sParaTemp.Add("sign_type", config.Sign_type);

            sParaTemp.Add("withdraw_no", withdrawNo);
            sParaTemp.Add("card_id", cardId.ToString());
            sParaTemp.Add("money", money.ToString());
            sParaTemp.Add("notify_url", "http://xrc.com/QianDaiVipCard/VipCardWithdrawNotify");

            string responsestr = new Submit().BuildRequest(sParaTemp, "vipCard/withdraw");
            if (responsestr.StartsWith("报错："))
            {
                result.StatusCode = 1;
                result.Message = responsestr;
                return result;
            }

            var back = JObject.Parse(responsestr);
            if (back["ret_code"].ToString() != "0")
            {
                result.StatusCode = 3;
                result.Message = back["ret_message"].ToString() + "-" + money.ToString();
            }
            else
            {
                var qianDaiVipCardWithdrawLog = new CrQianDaiVipCardWithdrawLog()
                {
                    WithdrawNo = withdrawNo,
                    CardId = cardId,
                    Money = money,
                    CreateDate = DateTime.Now,
                    CreatedBy = BLL.Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo,
                    Status = "-2",
                };

                QianDaiVipCardWithdrawLogBo.Instance.CreateCrQianDaiVipCardWithdrawLog(qianDaiVipCardWithdrawLog);
            }

            return result;
        }
        #endregion
    }

    class QDResult
    {
        public int ret_code { get; set; }
        public string ret_message { get; set; }
    }


}
