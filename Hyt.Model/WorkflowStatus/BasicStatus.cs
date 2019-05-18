using System.ComponentModel;

namespace Hyt.Model.WorkflowStatus
{
    /// <summary>
    /// 基础状态
    /// </summary>
    /// <remarks>2013-09-10 吴文强 创建</remarks>
    public class BasicStatus
    {
        /// <summary>
        /// 地区状态
        /// 数据表:BsArea 字段:Status
        /// </summary>
        /// <remarks>2013-06-18 吴文强 创建</remarks>
        public enum 地区状态
        {
            有效 = 1,
            无效 = 0,
        }

        /// <summary>
        /// 地区级别
        /// 数据表:BsArea 字段:AreaLevel
        /// </summary>
        /// <remarks>2013-07-31 吴文强 创建</remarks>
        public enum 地区级别
        {
            省 = 1,
            市 = 2,
            区 = 3,
        }

        /// <summary>
        /// 支付方式是否网上支付
        /// 数据表:BsPaymentType 字段:IsOnlinePay
        /// </summary>
        /// <remarks>2013-06-18 吴文强 创建</remarks>
        public enum 支付方式是否网上支付
        {
            是 = 1,
            否 = 0,
        }

        /// <summary>
        /// 支付方式前台是否可见
        /// 数据表:BsPaymentType 字段:IsOnlineVisible
        /// </summary>
        /// <remarks>2013-06-18 吴文强 创建</remarks>
        public enum 支付方式前台是否可见
        {
            是 = 1,
            否 = 0,
        }

        /// <summary>
        /// 支付方式类型
        /// 数据表:BsPaymentType 字段:PaymentType
        /// </summary>
        /// <remarks>2013-06-18 吴文强 创建</remarks>
        public enum 支付方式类型
        {
            预付 = 10,
            到付 = 20,
        }

        /// <summary>
        /// 是否需要卡号
        /// 数据表:BsPaymentType 字段:RequiredCardNumber
        /// </summary>
        /// <remarks>2013-07-10 吴文强 创建</remarks>
        public enum 是否需要卡号
        {
            是 = 1,
            否 = 0,
        }

        /// <summary>
        /// 支付方式状态
        /// 数据表:BsPaymentType 字段:Status
        /// </summary>
        /// <remarks>2013-06-18 吴文强 创建</remarks>
        public enum 支付方式状态
        {
            启用 = 1,
            禁用 = 0,
        }

        /// <summary>
        /// 组织机构状态
        /// 数据表:BsOrganization 字段:Status
        /// </summary>
        /// <remarks>2013-06-18 吴文强 创建</remarks>
        public enum 组织机构状态
        {
            启用 = 1,
            禁用 = 0,
        }

        /// <summary>
        /// 码表状态
        /// 数据表:BsCode 字段:Status
        /// </summary>
        /// <remarks>2013-10-14 吴文强 创建</remarks>
        public enum 码表状态
        {
            启用 = 1,
            禁用 = 0,
        }
        /// <summary>
        /// 配送地图类型
        /// </summary>
        /// <remarks>2015-05-14 杨浩 创建</remarks>
        public enum 地图类型
        {
            百度地图 = 10,
            高德地图 = 20,
        }
    }
}
