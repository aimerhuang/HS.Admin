using Extra.Erp.Properties;

namespace Extra.Erp.LiJia
{
    /// <summary>
    /// LiJia实例
    /// </summary>
    /// <remarks>2013-9-1 杨浩 创建</remarks>
    public class LiJiaProviderFactory
    {
        private static ILiJiaProvider provider = null;

        static LiJiaProviderFactory()
        {
            //在需要关闭Erp时，实现KisNullProvider
            if (ErpConfigs.Instance.GetErpConfig().Enable == 1)
            {
                provider = new LiJiaProvider();
            }
            else
            {
                provider = new LiJiaNullProvider();
            }
        }

        /// <summary>
        /// 创建Kis实例
        /// </summary>
        /// <returns></returns>
        /// <remarks>2013-9-1 杨浩 创建</remarks>
        public static ILiJiaProvider CreateProvider()
        {
            return provider;
        }
    }
}
