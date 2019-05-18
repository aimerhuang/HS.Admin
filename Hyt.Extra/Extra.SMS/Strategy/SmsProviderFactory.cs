namespace Extra.SMS
{
    /// <summary>
    /// 短信实例管理
    /// </summary>
    /// <remarks>2013-6-26 杨浩 添加</remarks>
    public class SmsProviderFactory
    {
        private static ISmsProvider provider = null;

        /// <summary>
        /// 静态构造函数
        /// </summary>
        /// <remarks>2013-6-26 杨浩 添加</remarks>
        static SmsProviderFactory()
        {
           // provider = new SMS.Mandao.SmsProvider();
#if !DEBUG
            provider = new SMS.Mandao.SmsProvider();
#else
           //provider = new SMS.Strategy.SmsNullProvider();
#endif
            provider = new Extra.SMS.CBlue.SmsProvider();
        }

        /// <summary>
        /// 创建实例
        /// </summary>
        /// <returns>ISmsProvider</returns>
        /// <remarks>2013-6-26 杨浩 添加</remarks>
        public static ISmsProvider CreateProvider()
        {
            return provider;
        }
    }
}
