using Extra.Erp.Properties;

namespace Extra.Erp
{
    /// <summary>
    /// Eas实例
    /// </summary>
    /// <remarks>2013-9-1 杨浩 创建</remarks>
    public class EasProviderFactory
    {
        private static IEasProvider provider = null;

        static EasProviderFactory()
        {
            //在需要关闭Erp时，实现EasNullProvider
            if (Settings.Default.Enable)
            {
                provider = new EasProvider();
            }
            else
            {
                provider = new EasNullProvider();
            }
        }

        /// <summary>
        /// 创建Eas实例
        /// </summary>
        /// <returns></returns>
        /// <remarks>2013-9-1 杨浩 创建</remarks>
        public static IEasProvider CreateProvider()
        {
            return provider;
        }
    }
}
