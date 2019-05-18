using Hyt.BLL.Finance;
using Hyt.BLL.Order;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;
using Hyt.Service.Contract;
using Hyt.Service.Contract.ApiPay;
using Hyt.Service.Contract.ApiPayNotify;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using System.Transactions;
using System.Xml;

namespace Hyt.Service.Implement.ApiPayNotify
{
    /// <summary>
    /// 支付异步回执
    /// </summary>
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class PayNotify : BaseService, IPayNotify
    {

        /// <summary>
        /// 易宝异步回执
        /// </summary>
        /// <param name="stream">http请求上下文信息</param>
        /// <returns></returns>
        /// <remarks>2015-10-12 杨浩 创建</remarks>
        public string EhkingNotifyReceipt(Stream stream)
        {
            var result = new Result()
            {
                Status = true
            };

            try
            {

                BLL.Log.LocalLogBo.Instance.Write("异步调用成功！", "EhkingNotify");
                stream.Position = 0;
                using (StreamReader sr = new StreamReader(stream))
                {
                    string requeststr = sr.ReadToEnd();
                    sr.Dispose();
                    result.Message = requeststr;
                    BLL.Log.LocalLogBo.Instance.Write(requeststr, "EhkingNotifyReceipt");
                    result = BLL.ApiFactory.ApiProviderFactory.GetPayInstance((int)Hyt.Model.CommonEnum.PayCode.易宝).NotifyReceipt(requeststr);
                }
            }
            catch (Exception ex)
            {

                BLL.Log.LocalLogBo.Instance.Write(ex.Message, "EhkingNotifyReceipt");
                result.Status = false;
                result.Message = ex.Message;
            }

            return result.Status ? "SUCCESS" : "FAILURE";

        }

        /// <summary>
        /// 易宝支付海关报关异步回执
        /// </summary>
        /// <param name="nameValuePair"></param>
        /// <returns></returns>
        /// <remarks>2015-12-31 杨浩 创建</remarks>
        public Result EhkingNotifyReceipt(NotifyResult notifyResult)
        {
            var result = new Result()
            {
                Status = true
            };

            try
            {
                BLL.Log.LocalLogBo.Instance.Write("\r\n异步调用成功！", "EhkingNotify");
                string json = JsonConvert.SerializeObject(notifyResult);

                BLL.Log.LocalLogBo.Instance.Write("\r\n异步调用成功2！" + json, "EhkingNotifyResult");

                result = BLL.ApiFactory.ApiProviderFactory.GetPayInstance((int)Hyt.Model.CommonEnum.PayCode.易宝).NotifyReceipt(json);

            }
            catch (Exception ex)
            {
                BLL.Log.LocalLogBo.Instance.Write(ex.Message, "EhkingNotifyReceiptException");
                result.Status = false;
                result.Message = ex.Message;
            }

            return result;
        }

        /// <summary>
        /// 易扫购订单异步回调
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        /// <remarks>2016-5-8 杨浩 创建</remarks>
        public string EhkingScannNotifyReceipt(Stream stream)
        {
            return "";

        }
    }
}