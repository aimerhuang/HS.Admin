
namespace Hyt.Model.B2CApp
{
    /// <summary>
    /// 版本
    /// </summary>
    /// <remarks>2013-7-22 杨浩 添加</remarks>
    public class Version
    {
        /// <summary>
        /// 版本号
        /// </summary>
        public string VersionNumber { get; set; }

        /// <summary>
        /// 版本更新地址
        /// </summary>
        public string VersionLink { get; set; }

        /// <summary>
        /// 版本升级信息
        /// </summary>
        public string UpgradeInfo { get; set; }
    }
}
