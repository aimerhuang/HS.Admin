using Hyt.BLL.ApiPay.QianDai;
using Hyt.BLL.QianDai.Extends;
using Hyt.DataAccess.Order;
using Hyt.Model;
using Hyt.Model.Logis.XinYi;
using Hyt.Model.WorkflowStatus;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Hyt.Admin.Controllers
{
    public class OrderResultController : Controller
    {

        /// <summary>
        /// 汇付天下异步回执
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ContentResult ReceiveBg()
        {
            string orderId = Request.Params["orderId"].ToString();
            string orderAmt = Request.Params["orderAmt"];
            decimal price = Convert.ToDecimal(orderAmt);
            string s = orderId.Split('_')[0];  //获取订单号
            int id = Convert.ToInt32(s);
            var result = BLL.ApiFactory.ApiProviderFactory.GetPayInstance(19).NotifyReceipt(null);
            bool r1 = Hyt.BLL.Order.SoOrderBo.Instance.UpdateOrderApiStatus((int)OrderStatus.支付报关状态.成功, 0, id);
            bool r2 = Hyt.BLL.Order.SoOrderBo.Instance.UpdateOrderApiStatus((int)OrderStatus.支付报关状态.成功, 1, id);

            Hyt.BLL.Order.SoOrderBo.Instance.UpdateOrderPayType(id, 22); //更新支付方式
            Hyt.BLL.Log.LocalLogBo.Instance.Write("1订单号：" + id.ToString() + "，订单状态修改：" + orderAmt.ToString() + r2.ToString(), "tg");  //日志

            FnOnlinePayment payment = new FnOnlinePayment();
            var date = DateTime.Now;
            payment.CreatedDate = date;
            payment.CreatedBy = 0;
            payment.LastUpdateDate = date;
            payment.OperatedDate = date;
            payment.Operator = 0;
            payment.PaymentTypeSysNo = 22;  //汇付支付
            payment.Amount = price /100;  
            payment.VoucherNo = id.ToString() + "_" + DateTime.Now.ToString("yyyyMMddHHmmss");
            payment.SourceSysNo = id;
            payment.BusinessOrderSysNo = orderId;
            var results = Hyt.BLL.Finance.FinanceBo.Instance.UpdateOrderPayStatus(payment, Hyt.Model.SystemPredefined.PaymentType.汇付支付);  //写付款单 
            Hyt.BLL.Log.LocalLogBo.Instance.Write("2订单号：" + id.ToString() + "，订单状态修改：" + orderAmt.ToString() + r2.ToString(), "tg");  //日志
            
            Hyt.BLL.Order.SoOrderBo.Instance.UpdatePayStatus(id, OrderStatus.销售单支付状态.已支付); //更新支付状态
            ISoOrderDao.Instance.UpdateOrderPayDte(id, date); //同步支付时间的到订单主表

            Hyt.BLL.Log.LocalLogBo.Instance.Write("3订单号：" + id.ToString() + "，订单状态修改：" + results.ToString() + payment.BusinessOrderSysNo.ToString(), "tg");  //日志
            return Content("<result>1</result>");
        }      
    }

    
}
