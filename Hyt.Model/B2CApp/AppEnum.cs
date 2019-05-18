
namespace Hyt.Model.B2CApp
{
    /// <summary>
    /// App枚举
    /// </summary>
    /// <remarks>2013-8-15 杨浩 添加</remarks>
    public class AppEnum
    {
        /// <summary>
        /// App系统类型
        /// </summary>
        /// <remarks>2013-8-15 杨浩 添加</remarks>
        public enum AppType
        {
            Android = 10001,
            Ios = 20001
        }

        /// <summary>
        /// App网络模式
        /// </summary>
        /// <remarks>2013-8-15 杨浩 添加</remarks>
        public enum NetworkType:byte
        {
            /// <summary>
            /// wifi网络模式
            /// </summary>
            Wifi = 1,
            /// <summary>
            /// 2G网络模式
            /// </summary>
            _2G = 2,
            /// <summary>
            /// 3G网络模式
            /// </summary>
            _3G = 3
        }

        /// <summary>
        /// 可操作项枚举
        /// </summary>
        /// <remarks>2013-8-15 杨浩 添加</remarks>
        public enum OperateStatus
        {
            /// <summary>
            ///显示 取消和支付 操作
            /// </summary>
            CancelAndPay,
            /// <summary>
            /// 取消操作
            /// </summary>
            Cancel,
            /// <summary>
            /// 没有操作
            /// </summary>
            None,
            /// <summary>
            /// 查看物流
            /// </summary>
            Logistics,
            /// <summary>
            /// 未评价未晒单
            /// </summary>
            CommentAndShow,
            /// <summary>
            /// 已评价已晒单
            /// </summary>
            CommentedAndShowed,

            /// <summary>
            /// 未评价已晒单
            /// </summary>
            CommentAndShowed,

            /// <summary>
            /// 已评价未晒单
            /// </summary>
            CommentedAndShow,

        }

        /// <summary>
        /// 订单过滤条件
        /// </summary>
        /// <remarks>2013-8-15 杨浩 添加</remarks>
        public enum OrderFilter
        {
            /// <summary>
            /// 显示所有订单
            /// </summary>
            All=5,
            /// <summary>
            /// 配送中
            /// </summary>
            Deliveries=10,
            /// <summary>
            /// 待支付
            /// </summary>
            Obligation=20,
            /// <summary>
            /// 待评价
            /// </summary>
            待评价 = 30
        }

        /// <summary>
        /// 服务状态
        /// </summary>
        /// <remarks>2013-8-15 杨浩 添加</remarks>
        public enum StatusCode
        {
            用户未登录=1000,
            服务异常=1001
        }

        /// <summary>
        /// 接口状态
        /// </summary>
        /// <remarks>2016-9-9 杨浩 创建</remarks>
        public enum ApiStatusCode
        {
            接口异常=-1,

        }
    }
}
