
using Hyt.Model;

namespace Hyt.Tool.ImageBuilder.BLL
{
    /// <summary>
    /// 业务基类(提供实例)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <remarks>
    /// 2013-6-26 苟治国 创建 </remarks>
    /// </remarks>
    /// 
    public class BOBase<T> where T:BOBase<T>,new()
    {
        private static T _instance = null;

        /// <summary>
        /// 返回当前业务逻辑实例
        /// </summary>
        /// <returns>返回当前业务逻辑实例</returns>
        /// <remarks>
        /// 苟治国 创建
        /// </remarks>
        public static T Instance 
        {
            get 
            {
                if (_instance == null)
                {
                    _instance = new T();
                }
                return _instance;
            }
        }
    }
}
