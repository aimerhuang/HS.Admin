using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Util.JPush
{
    /// <summary>
    /// 极光推送请求对象
    /// </summary>
    /// <remarks>2014-01-17 邵斌 创建</remarks>
    public class JPushRequest
    {
        /// <summary>
        /// 系统唯一编号
        /// </summary>
        /// <remarks>2014-01-17 邵斌 创建</remarks>
        public int SendNo { get; set; }

        /// <summary>
        /// 接收者类型。1、指定的 IMEI。此时必须指定 appKeys。2、指定的 tag。3、指定的 alias。4、 对指定 appkey 的所有用户推送消息。
        /// </summary>
        /// <remarks>2014-01-17 邵斌 创建</remarks>
        public int ReceiveType { get; set; }

        /// <summary>
        ///  发送消息的类型：1、通知 2、自定义消息
        /// </summary>
        /// <remarks>2014-01-17 邵斌 创建</remarks>
        public int MsgType { get; set; }

        /// <summary>
        /// 发送范围值，与 receiver_type相对应。 1、IMEI只支持一个 2、tag 支持多个，使用 "," 间隔（tag 支持多达 10 个）。 3、alias 支持多个，使用 "," 间隔（alias 支持多达 1000 个）。 4、不需要填
        /// </summary>
        /// <remarks>2014-01-17 邵斌 创建</remarks>
        public string ReceiverValue { get; set; }

        /// <summary>
        /// 发送消息的内容。 与 msg_type 相对应的值
        /// </summary>
        /// <remarks>2014-01-17 邵斌 创建</remarks>
        public string MsgContent { get; set; }

        /// <summary>
        /// 目标用户终端手机的平台类型，如： android, ios 多个请使用逗号分隔
        /// </summary>
        /// <remarks>2014-01-17 邵斌 创建</remarks>
        public PlatformEnum Platform { get; set; }

        /// <summary>
        /// 此处发送描述，描述此次发送调用。不会发到用户。
        /// 该字段主要用来在极光推送段进行检索，用俩查找该条推送信息是有那个功能进行推送的
        /// </summary>
        /// <remarks>2014-01-17 邵斌 创建</remarks>
        public string Description { get; set; }

        /// <summary>
        /// 消息在服务器中停留时间
        /// </summary>
        /// <remarks>2014-01-17 邵斌 创建</remarks>
        public int TimeToLive { get; set; }
    }
}
