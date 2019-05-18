using Hyt.Model;
using Hyt.Model.Transfer;
using Hyt.Service.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text;

namespace Hyt.Service.Implement
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class ExpressService : IExpressService
    {
        /// <summary>
        /// </summary>
        /// <param name="transactionSysNos">The transaction sys no.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        /// <remarks>
        /// 
        /// </remarks>
        public Result<List<OrderTransactionModel>> GetOrderTransactionModel(List<string> transactionSysNos)
        {
            return BLL.Logistics.LgDeliveryBo.Instance.SreachExpress(transactionSysNos);
        }
    }
}
