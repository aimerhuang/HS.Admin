using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.WorkflowStatus
{
    /// <summary>
    /// 联盟状态
    /// </summary>
    /// <remarks>2013-09-10 吴文强 创建</remarks>
    public class UnionStatus
    {
        /// <summary>
        /// 联盟网站状态
        /// 数据表:UnUnionSite 字段:Status
        /// </summary>
        /// <remarks>2013-10-14 吴文强 创建</remarks>
        public enum 联盟网站状态
        {
            启用 = 1,
            禁用 = 0,
        }

        /// <summary>
        /// 联盟广告类型
        /// 数据表:UnUnionSite 字段:AdvertisementType
        /// </summary>
        /// <remarks>2013-10-14 吴文强 创建</remarks>
        public enum 联盟广告类型
        {
            CPC = 10,
            CPA = 20,
            CPS = 30,
        }

        /// <summary>
        /// 联盟广告状态
        /// 数据表:UnUnionSite 字段:Status
        /// </summary>
        /// <remarks>2013-10-14 吴文强 创建</remarks>
        public enum 联盟广告状态
        {
            启用 = 1,
            禁用 = 0,
        }

        /// <summary>
        /// CPS商品状态
        /// 数据表:UnCpsProduct 字段:Status
        /// </summary>
        /// <remarks>2013-10-14 吴文强 创建</remarks>
        public enum CPS商品状态
        {
            启用 = 1,
            禁用 = 0,
        }

        /// <summary>
        /// 广告日志是否有效
        /// 数据表:UnAdvertisementLog 字段:IsValid
        /// </summary>
        /// <remarks>2013-10-14 吴文强 创建</remarks>
        public enum 广告日志是否有效
        {
            有效 = 1,
            无效 = 0,
        }

    }
}
