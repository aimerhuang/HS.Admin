using Hyt.Model.Generated;
using System;

namespace Hyt.Model.Transfer
{
    /// <summary>
    /// 商城地区关联维护
    /// </summary>
    /// <remarks>2014-10-14 缪竞华 创建</remarks>
    [Serializable()]
    public class CBDsMallAreaRelation : DsMallAreaAssociation
    {
        /// <summary>
        /// 分销商名称
        /// </summary>
        public string DealerName { get; set; }

        /// <summary>
        /// 商城名称
        /// </summary>
        public string MallName { get; set; }

        /// <summary>
        /// 商城地区名称
        /// </summary>
        public string AreaName { get; set; }
    }
}
