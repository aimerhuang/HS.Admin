using Hyt.Model;
using Hyt.Model.Transfer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace Hyt.Service.Contract
{
    /// <summary>
    /// 运单查询契约服务
    /// </summary>
    /// <remarks>2014-02-17 沈强 创建</remarks>
    [ServiceContract]
    [ServiceKnownType(typeof(Result))]
    public interface IExpressService
    {
        [OperationContract]
        Result<List<OrderTransactionModel>> GetOrderTransactionModel(List<string> transactionSysNo);
    }
}
