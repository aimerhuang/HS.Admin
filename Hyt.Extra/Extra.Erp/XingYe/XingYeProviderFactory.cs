using Extra.Erp.Properties;

namespace Extra.Erp.XingYe
{
    /// <summary>
    /// XingYe实例
    /// </summary>
    /// <remarks>2013-9-1 杨浩 创建</remarks>
    public class XingYeProviderFactory
    {
        private static IXingYeProvider provider = null;

        static XingYeProviderFactory()
        {
            //在需要关闭Erp时，实现XingYeNullProvider
            if (ErpConfigs.Instance.GetErpConfig().Enable == 1)
            {
                provider = new XingYeProvider();
            }
            else
            {
                provider = new XingYeNullProvider();
            }
        }

        /// <summary>
        /// 创建XingYe实例
        /// </summary>
        /// <returns></returns>
        /// <remarks>2013-9-1 杨浩 创建</remarks>
        public static IXingYeProvider CreateProvider()
        {
            return provider;
        }
    }
}
