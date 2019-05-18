
using System;

namespace Hyt.Infrastructure
{
    /// <summary>
    /// 表示派生类是一次性的对象
    /// </summary>
    /// <Remark>2013-6-26 杨浩 创建</Remark>
    public abstract class DisposableObject : IDisposable
    {
        #region Finalization Constructs
        /// <summary>
        /// 析构函数隐式释放对象.
        /// </summary>
        /// <returns></returns>
        /// <Remark>2013-6-26 杨浩 创建</Remark>
        ~DisposableObject()
        {
            this.Dispose(false);
        }
        #endregion

        #region Protected Methods
        /// <summary>
        /// 对象回收.
        /// </summary>
        /// <returns></returns>
        /// <Remark>2013-6-26 杨浩 创建</Remark>
        protected abstract void Dispose(bool disposing);

        /// <summary>
        /// 显示强制回收不被使用的对象
        /// </summary>
        /// <returns></returns>
        /// <Remark>2013-6-26 杨浩 创建</Remark>
        protected void ExplicitDispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion

        #region IDisposable Members
        /// <summary>
        /// 执行应用程序定义的任务相关联的释放,或非托管资源的重置
        /// </summary>
        /// <returns></returns>
        /// <Remark>2013-6-26 杨浩 创建</Remark>
        public void Dispose()
        {
            this.ExplicitDispose();
        }
        #endregion
    }
}
