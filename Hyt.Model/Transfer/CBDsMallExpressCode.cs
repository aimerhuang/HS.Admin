
namespace Hyt.Model.Transfer
{
    /// <summary>
    /// 经销商城快递代码
    /// </summary>
    /// <remarks>2015-1-19 缪竞华 创建</remarks>
    public class CBDsMallExpressCode : DsMallExpressCode
    {
        /// <summary>
        /// 商城代号
        /// </summary>
        public string MallCode { get; set; }

        /// <summary>
        /// 商城名称
        /// </summary>
        public string MallName { get; set; }

        /// <summary>
        /// 配送方式名称
        /// </summary>
        public string DeliveryTypeName { get; set; }
    }
}
