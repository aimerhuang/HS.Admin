using System;
using System.ServiceModel;
using System.ServiceModel.Dispatcher;
using System.Web;
using Hyt.Model.WorkflowStatus;

namespace Hyt.Service.Contract.Base
{
    /// <summary>
    /// 自定义拦截器
    /// </summary>
    /// <remarks>
    /// 2013-7-10  杨浩 添加
    /// </remarks>
    public class CustomInvoker : IOperationInvoker
    {
        private IOperationInvoker _invoker;
        //是否验证登录状态
        private bool _isCheck = true;

        public CustomInvoker(IOperationInvoker invoker,bool isCheck)
        {
            this._invoker = invoker;
            this._isCheck = isCheck;
        }

        #region 实现IOperationInvoker

        /// <summary>
        /// 从一个实例和输入对象的集合返回一个对象和输出对象的集合
        /// </summary>
        /// <param name="instance">要调用的对象</param>
        /// <param name="inputs">方法的输入</param>
        /// <param name="outputs">方法的输出</param>
        /// <returns>返回值</returns>
        public object Invoke(object instance, object[] inputs, out object[] outputs)
        {
            outputs = new object[0];

            
            //是否验证登录状态                    
            if (_isCheck)
            {
                //获取用户的安全令牌
                string token = ContractToken.GetToken();

                //验证用户的安全令牌
                if (!ContractToken.CheckToken(token))
                {
                    return new Model.Result
                        {
                            Message = "令牌错误或用户未登录",
                            Status = false,
                            StatusCode = (int)Hyt.Model.B2CApp.AppEnum.StatusCode.用户未登录
                        };
                }
            }
            //方法异常捕获
            try
            {
                object result = _invoker.Invoke(instance, inputs, out outputs);
                return result;
            }
            catch (Exception e)
            {
                LogStatus.系统日志来源 source;
                string agent = HttpContext.Current.Request.UserAgent;
                if (agent != null)
                {
                    if (agent.Contains("Android"))
                        source = LogStatus.系统日志来源.商城AndroidApp;
                    else if (agent.Contains("iPhone") || agent.Contains("iPad") || agent.Contains("iPod"))
                        source = LogStatus.系统日志来源.商城IphoneApp;
                    else
                        source = LogStatus.系统日志来源.前台;

                    //Hyt.BLL.Log.SysLog.Instance.Error(source, e.Message.Replace("\r\n", " "), LogStatus.系统日志目标类型.用户,
                                                      //Model.SystemPredefined.User.SystemUser, e);
                }
                return new Model.Result
                    {
#if DEBUG
                        Message =e.Message,
#else 
                        //Message=(e is Model.HytException?e.Message:"服务遇到错误"),
                        Message = e.Message,
#endif
                        Status = false,
                        StatusCode = (int)Hyt.Model.B2CApp.AppEnum.StatusCode.服务异常
                    };
            }
        }

        public virtual object[] AllocateInputs()
        {
            return _invoker.AllocateInputs();
        }

        public IAsyncResult InvokeBegin(object instance, object[] inputs, AsyncCallback callback, object state)
        {
            return _invoker.InvokeBegin(instance, inputs, callback, state);
        }

        public object InvokeEnd(object instance, out object[] outputs, IAsyncResult result)
        {
            return _invoker.InvokeEnd(instance, out outputs, result);
        }

        public bool IsSynchronous
        {
            get { return _invoker.IsSynchronous; }
        }

        #endregion
       
    }
}
