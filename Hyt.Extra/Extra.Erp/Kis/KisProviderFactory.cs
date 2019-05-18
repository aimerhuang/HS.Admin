using Extra.Erp.Properties;

namespace Extra.Erp.Kis
{
    /// <summary>
    /// Kis实例
    /// </summary>
    /// <remarks>2013-9-1 杨浩 创建</remarks>
    public class KisProviderFactory
    {
        private static IKisProvider provider = null;

        static KisProviderFactory()
        {
            //在需要关闭Erp时，实现KisNullProvider
            if (ErpConfigs.Instance.GetErpConfig().Enable == 1)
            {
                provider = new KisProvider();
            }
            else
            {
                provider = new KisNullProvider();
            }
        }

        /// <summary>
        /// 创建Kis实例
        /// </summary>
        /// <returns></returns>
        /// <remarks>2013-9-1 杨浩 创建</remarks>
        public static IKisProvider CreateProvider()
        {
            return provider;
        }
    }
}
