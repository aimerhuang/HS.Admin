using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.WorkflowStatus
{
    /// <summary>
    /// 营销状态
    /// </summary>
    /// <remarks>2013-10-23 吴文强 创建</remarks>
    public class MarketingStatus
    {
        /// <summary>
        /// 微信消息类型
        /// 数据表:MkWeixinConfig 字段:FollowType
        /// 数据表:MkWeixinConfig 字段:MessageType
        /// </summary>
        /// <remarks>2013-10-23 吴文强 创建</remarks>
        public enum 微信消息类型
        {
            文本	=10 ,
            图片	=20 ,
        }

        /// <summary>
        /// 微信关键词状态
        /// 数据表:MkWeixinKeywords 字段:Status
        /// </summary>
        /// <remarks>2013-10-23 吴文强 创建</remarks>
        public enum 微信关键词状态
        {
            启用 = 1,
            禁用 = 0,
        }

        /// <summary>
        /// 微信关键词回复状态
        /// 数据表:MkWeixinKeywordsReply 字段:Status
        /// </summary>
        /// <remarks>2013-10-14 吴文强 创建</remarks>
        public enum 微信关键词回复状态
        {
            启用 = 1,
            禁用 = 0,
        }

        /// <summary>
        /// 微信关键词回复类型
        /// 数据表:MkWeixinKeywordsReply 字段:ReplyType
        /// </summary>
        /// <remarks>2013-10-24 吴文强 创建</remarks>
        public enum 微信关键词回复类型
        {
            文本 = 10,
            图文 = 20,
        }

        /// <summary>
        /// 微信消息类型  咨询 = 10,回复 = 20
        /// 数据表：MkWeixinQuestion 字段：Type
        /// </summary>
        /// <remarks>2013-11-05 陶辉 </remarks>
        public enum 微信咨询类型
        {
            咨询 = 10,
            回复 = 20
        }

        /// <summary>
        /// 微信消息类型 未读 = 0,已读 = 1
        /// 数据表：MkWeixinQuestion 字段：Status
        /// </summary>
        /// <remarks>2013-11-05 陶辉 </remarks>
        public enum 微信咨询消息状态
        {
            未读 = 0,
            已读 = 1
        }

        /// <summary>
        /// 友情链接管理状态:待审(10),已审(20),作废(-10)
        /// 数据表：MkBlogroll 字段：Status
        /// </summary>
        /// <remarks>2013-12-09 苟治国 </remarks>
        public enum 友情链接管理状态
        {
            待审 = 10,
            已审 = 20,
            作废 = -10
        }
    }
}
