using Hyt.BLL.Order;
using Hyt.Model;
using Hyt.Model.Transfer;
using Hyt.Service.Contract;
using Hyt.Service.Contract.ApiPay;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text;

namespace Hyt.Service.Implement.ApiPay
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class Pay : BaseService,IPay
    {
        /// <summary>
        /// 海关报关
        /// </summary>
        /// <param name="orderSysNo">订单编号</param>
        /// <param name="payCode">支付编码</param>
        /// <remarks>2015-10-12 杨浩 创建</remarks>
        public Result ApplyToCustoms(int orderSysNo, int payCode)
        {
            var result=new Result()
            {
                Status=true
            };

            try
            {
                var soOrder = SoOrderBo.Instance.GetEntity(orderSysNo);
                result = BLL.ApiFactory.ApiProviderFactory.GetPayInstance(payCode).ApplyToCustoms(soOrder);
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = "系统异常";
                BLL.Log.LocalLogBo.Instance.Write(ex.Message, "ApplyToCustoms");
            }
           
            return result;
        }

        /// <summary>
        /// 海关报关查询
        /// </summary>
        /// <param name="orderSysNo">订单编号</param>
        /// <param name="payCode">支付编码</param>
        /// <returns></returns>
        /// <remarks>2015-12-31 杨浩 创建</remarks>
        public Result CustomsQuery(int orderSysNo, int payCode)
        {
            var result = new Result()
            {
                Status = true
            };
            result = BLL.ApiFactory.ApiProviderFactory.GetPayInstance(payCode).CustomsQuery(orderSysNo);
            return result;
        }
    }
}
