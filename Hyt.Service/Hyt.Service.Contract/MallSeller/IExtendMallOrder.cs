using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace Hyt.Service.Contract.MallSeller
{
    using Hyt.Model;
    using Hyt.Service.Contract.Base;
    using Hyt.Service.Contract.MallSeller.Model;

    /// <summary>
    /// 第三方商城接口
    /// </summary>
    [ServiceKnownType(typeof(Result))]
    [ServiceContract]
    public interface IExtendMallOrder : IBaseServiceContract
    {
        /// <summary>
        /// 用户调用该接口可实现自己联系发货（线下物流）
        /// </summary>
        /// <param name="request">请求参数</param>
        /// <returns>处理结果</returns>
        /// <remarks>2014-05-12 黄波 创建</remarks>
        [CustomOperationBehavior(false)]
        [OperationContract]
        Result LogisticsSend(LogisticsSendRequest request);

        /// <summary>
        /// 更新订单备注信息
        /// </summary>
        /// <param name="request">请求参数</param>
        /// <remarks>2014-05-12 黄波 创建</remarks>
        [CustomOperationBehavior(false)]
        [OperationContract]
        Result UpdateMemo(UpdateMemoRequest request);
    }
}