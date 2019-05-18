using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Weixin
{
    /// <summary>
    /// 微信推送消息
    /// </summary>
    /// <remarks>2013-12-05 陶辉 创建</remarks>
    public class RequestXml
    {
        private string toUserName = "";
        /// <summary>
        /// 消息接收方微信号，一般为公众平台账号微信号
        /// </summary>
        public string ToUserName
        {
            get { return toUserName; }
            set { toUserName = value; }
        }

        private string fromUserName = "";
        /// <summary>
        /// 消息发送方微信号
        /// </summary>
        public string FromUserName
        {
            get { return fromUserName; }
            set { fromUserName = value; }
        }

        private string createTime = "";
        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreateTime
        {
            get { return createTime; }
            set { createTime = value; }
        }

        private string msgType = "";
        /// <summary>
        /// 信息类型 地理位置:location,文本消息:text,消息类型:image
        /// </summary>
        public string MsgType
        {
            get { return msgType; }
            set { msgType = value; }
        }

        private string content = "";
        /// <summary>
        /// 信息内容
        /// </summary>
        public string Content
        {
            get { return content; }
            set { content = value; }
        }

        private string location_X = "";
        /// <summary>
        /// 地理位置纬度
        /// </summary>
        public string Location_X
        {
            get { return location_X; }
            set { location_X = value; }
        }

        private string location_Y = "";
        /// <summary>
        /// 地理位置经度
        /// </summary>
        public string Location_Y
        {
            get { return location_Y; }
            set { location_Y = value; }
        }

        private string scale = "";
        /// <summary>
        /// 地图缩放大小
        /// </summary>
        public string Scale
        {
            get { return scale; }
            set { scale = value; }
        }

        private string label = "";
        /// <summary>
        /// 地理位置信息
        /// </summary>
        public string Label
        {
            get { return label; }
            set { label = value; }
        }

        private string picUrl = "";
        /// <summary>
        /// 图片链接，开发者可以用HTTP GET获取
        /// </summary>
        public string PicUrl
        {
            get { return picUrl; }
            set { picUrl = value; }
        }

        private string subscribeEvent = "";
        /// <summary>
        /// 订阅、取消订阅事件
        /// </summary>
        public string SubscribeEvent
        {
            get { return subscribeEvent; }
            set { subscribeEvent = value; }
        }

    }
}
