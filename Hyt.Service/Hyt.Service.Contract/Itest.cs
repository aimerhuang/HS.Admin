using System.ServiceModel;
using Hyt.Service.Contract.Base;

namespace Hyt.Service.Contract
{
    /// <summary>
    /// 表示与“”相关的应用层服务契约。
    /// </summary>
    [ServiceContract(Namespace = "http://www.huiyuanti.com")]
    public interface Itest : IBaseServiceContract
    {
        #region Methods

        /// <summary>
        /// 测试接口
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [OperationContract]
        // 在定义interface的时候， 如果一个函数需要抛出异常， 必须为操作设定异常类型，以便让WCF framework在通知会员端的时候知道是什么异常并如何封送
        [FaultContract(typeof(FaultData))]
        string GetData(int value);

        #endregion
    }
}
