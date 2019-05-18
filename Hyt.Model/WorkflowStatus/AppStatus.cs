using System.ComponentModel;

namespace Hyt.Model.WorkflowStatus
{
    /// <summary>
    /// App状态
    /// </summary>
    /// <remarks>2013-09-10 吴文强 创建</remarks>
    public class AppStatus
    {
        /// <summary>
        /// App代码
        /// 数据表:ApVersion 字段:AppCode
        /// </summary>
        /// <remarks>2013-09-10 吴文强 创建</remarks>
        public enum App代码
        {
            商城Android = 10001,
            商城Ios = 20001,
            百城通 = 10101,
        }

        /// <summary>
        /// App推送App类型
        /// 数据表:ApPushService 字段:AppType
        /// </summary>
        /// <remarks>2014-01-14 吴文强 创建</remarks>
        public enum App推送App类型
        {
            Iphone = 10,
            Android = 20,
            全部 = 90,
        }

        /// <summary>
        /// App推送服务类型
        /// 数据表:ApPushService 字段:ServiceType
        /// </summary>
        /// <remarks>2014-01-14 吴文强 创建</remarks>
        public enum App推送服务类型
        {
            产品详情 = 1,
            App浏览器 = 2,
            个人消息 = 3,
            优惠券 = 4,
        }

        /// <summary>
        /// App推送服务状态
        /// 数据表:ApPushService 字段:Status
        /// </summary>
        /// <remarks>2014-01-14 吴文强 创建</remarks>
        /// <remarks>2014-01-14 邵  斌 添加已审核状态</remarks>
        public enum App推送服务状态
        {
            待审 = 10,
            已审核 = 20,
            已发送 = 30,
            作废 = -10,
            失败 = -30,
        }

    }
}
