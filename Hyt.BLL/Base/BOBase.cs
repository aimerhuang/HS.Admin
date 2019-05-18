
namespace Hyt.BLL
{
    /// <summary>
    /// 业务基类(提供实例)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <remarks>2013-6-26 杨浩 创建 </remarks>
    public class BOBase<T> where T : BOBase<T>, new()
    {
        private static readonly T _instance = new T();

        /// <summary>
        /// 返回当前业务逻辑实例
        /// </summary>
        /// <returns>实例</returns>
        /// <remarks>2013-6-26 杨浩 创建 </remarks>
        public static T Instance
        {
            get
            {
                return _instance;
            }
        }
    }
}
