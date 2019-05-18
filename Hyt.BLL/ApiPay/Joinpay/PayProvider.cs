using Hyt.BLL.Finance;
using Hyt.BLL.Order;
using Hyt.Model;
using Hyt.Model.Common;
using Hyt.Model.Icp.GZNanSha;
using Hyt.Model.WorkflowStatus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Xml;

namespace Hyt.BLL.ApiPay.Joinpay
{
    /// <summary>
    /// 汇聚支付接口 
    /// </summary>
    /// <remarks>2017-12-23 杨浩 添加</remarks>
    public class PayProvider : IPayProvider
    {
        private PayConfig config = Hyt.BLL.Config.Config.Instance.GetPayConfig();
        private AlipayCustomsConfig customsConfig = Hyt.BLL.Config.Config.Instance.GetAlipayCustomsConfig();
        public override Model.CommonEnum.PayCode Code
        {
            get { return CommonEnum.PayCode.汇聚支付; }
        }
        /// <summary>
        /// 海关报关
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public override Model.Result ApplyToCustoms(Model.SoOrder order)
        {
            IList<FnOnlinePayment> list = FinanceBo.Instance.GetOnlinePaymentList(order.SysNo);
            var result = new Result();

        
            return result;
        }

   

        public override Model.Result CustomsQuery(int orderId)
        {
            
            return new Result() { Message="", Status=true };
        }

        /// <summary>
        /// 查询订单支付状态
        /// </summary>
        /// <returns></returns>
        /// <remarks>2016-12-20 杨浩 创建</remarks> 
        public override Result QueryOrderState(string orderId)
        {
            var reslut = new Result();
            try
            {
                string secret = "dab0786f5a0c46cb8a6e474aeb08b864";//888100050941685测试密钥
                string merchantNo = "888100700008487";//商户号             
                string url = "https://www.joinpay.com/trade/queryOrder.action";//请求地址
                var sb = new StringBuilder();//签名字符串+密钥          
                sb.Append(merchantNo);
                sb.Append(orderId);
                string request = url + "?p1_MerchantNo=" + merchantNo + "&p2_OrderNo=" + orderId + "&hmac=" + HttpUtility.UrlEncode(Md5(sb.ToString() + secret));
                string responseStr = Hyt.Util.MyHttp.GetResponse(request, "UTF-8");
                var jsonObj = LitJson.JsonMapper.ToObject(responseStr);

                int ra_Status = int.Parse(jsonObj["ra_Status"].ToString());
                reslut.StatusCode = ra_Status;
                if (ra_Status == 100)  //100:成功 101:失败 102:已创建 103:已取消
                {
                    reslut.Status = true;
                }
                else
                {
                    reslut.Status = false;
                }
            }
            catch (Exception ex)
            {
                reslut.Status = false;
                reslut.Message = ex.Message;
                reslut.StatusCode = -100;
            }
       


            return reslut;
        }

        /// <summary>
        /// md5加密
        /// </summary>
        /// <param name="password">需要加密的字符</param>
        /// <returns>MD5加密字符串</returns>
        /// <remarks>2017-12-23 杨浩 创建</remarks>
        private string Md5(string data)
        {
            MD5CryptoServiceProvider x = new MD5CryptoServiceProvider();
            byte[] bs = System.Text.Encoding.UTF8.GetBytes(data);
            bs = x.ComputeHash(bs);
            StringBuilder s = new StringBuilder();
            foreach (byte b in bs)
            {
                s.Append(b.ToString("x2").ToLower());
            }
            data = s.ToString();
            return data;
        }

    }

    /// <summary>
    /// 支付宝报关实体
    /// </summary>
    /// <remarks>2015-10-24 杨云奕 添加</remarks>
    public class AlipayCustomsMdl
    {
        #region 基本参数
        /// <summary>
        /// 接口名称
        /// </summary>
        public string service { get; set; }
        /// <summary>
        /// 合作者身份ID
        /// </summary>
        public string partner { get; set; }
        /// <summary>
        /// 参数编码字符集
        /// </summary>
        public string _input_charset { get; set; }
        /// <summary>
        /// 签名方式
        /// </summary>
        public string sign_type { get; set; }
        /// <summary>
        /// 签名
        /// </summary>
        public string sign { get; set; }
        #endregion
        #region 业务参数
        /// <summary>
        /// 报关流水号，商户自己生成
        /// </summary>
        public string out_request_no { get; set; }
        /// <summary>
        /// 支付宝交易号
        /// </summary>
        public string trade_no { get; set; }
        /// <summary>
        /// 商户海关备案编码
        /// </summary>
        public string merchant_customs_code { get; set; }
        /// <summary>
        /// 报关金额
        /// </summary>
        public decimal amount { get; set; }
        /// <summary>
        /// 海关编码
        /// </summary>
        public string customs_place { get; set; }
        /// <summary>
        /// 商户海关备案名称
        /// </summary>
        public string merchant_customs_name { get; set; }
        /// <summary>
        /// 是否拆单
        /// </summary>
        public string is_split { get; set; }
        /// <summary>
        /// 子订单号
        /// </summary>
        public string sub_out_biz_no { get; set; }

        #endregion
    }
}
