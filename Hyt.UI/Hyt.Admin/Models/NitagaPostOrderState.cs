using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Hyt.Admin.Models;

namespace Hyt.Admin.Models
{
    public class NitagaPostOrderState
    {
        /// <summary>
        /// 订单状态POST利嘉平台
        /// </summary>
        /// <param name="orderId">你他购订单编号</param>
        /// <param name="steta">订单状态</param>
        /// <returns></returns>
        public string OrderState(int orderId, string state)
        {
            //推送数据
            string url = "http://www.nitago.com/api/third/orders.php?do=tagset";//订单状态推送
            Dictionary<string, string> date = new Dictionary<string, string>();
            date.Add("orders_id", Convert.ToString(orderId));
            date.Add("orders_tag", state);
            ////数据生成JSON格式
            //JavaScriptSerializer jss = new JavaScriptSerializer();
            //string jsonDate = jss.Serialize(date);
            //执行推送
            Nitaga Nitaga = new Nitaga(url);
            string sAPIResult = Nitaga.Post(date);
            return sAPIResult;
        }

    }
}
