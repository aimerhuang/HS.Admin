using System;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace Hyt.Service.Contract.Base
{
    /// <summary>
    /// 实现可用于扩展服务端应用程序中的操作的运行时行为的方法。
    /// </summary>
    /// <remarks>
    /// 2013-7-10  杨浩 添加
    /// </remarks>
    public class CustomOperationBehaviorAttribute : Attribute, IOperationBehavior
    {
        #region 属性

        //是否验证登录状态
        private bool _isCheck=true;

        #endregion

        #region 构造器
        public CustomOperationBehaviorAttribute()
        {

        }
        /// <summary>
        /// 是否验证登录状态
        /// </summary>
        /// <param name="isCheck">为false表明只处理方法异常</param>
        public CustomOperationBehaviorAttribute(bool isCheck)
        {
            this._isCheck = isCheck;
        }
        #endregion

        #region IOperationBehavior实现

        public void AddBindingParameters(OperationDescription operationDescription,BindingParameterCollection bindingParameters)
        {

        }

        public void ApplyClientBehavior(OperationDescription operationDescription, ClientOperation clientOperation)
        {

        }

        public void ApplyDispatchBehavior(OperationDescription operationDescription, DispatchOperation dispatchOperation)
        {
            dispatchOperation.Invoker = new CustomInvoker(dispatchOperation.Invoker, _isCheck);
        }

        public void Validate(OperationDescription operationDescription)
        {

        }

        #endregion
    }
}
