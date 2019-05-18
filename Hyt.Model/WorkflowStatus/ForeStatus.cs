using System.ComponentModel;

namespace Hyt.Model.WorkflowStatus
{
    /// <summary>
    /// 前台对象状态
    /// </summary>
    /// <remarks>2013-06-18 吴文强 创建</remarks>
    public class ForeStatus
    {
        /// <summary>
        /// 广告组平台类型
        /// 数据表:FeAdvertGroup 字段:PlatformType
        /// </summary>
        /// <remarks>2013-06-18 吴文强 创建</remarks>
        public enum 广告组平台类型
        {
            PC网站 = 10,
            手机商城 = 30,
            商城IphoneApp = 31,
            商城AndroidApp = 32,
        }

        /// <summary>
        /// 广告组类型
        /// 数据表:FeAdvertGroup 字段:Type
        /// </summary>
        /// <remarks>2013-06-18 吴文强 创建</remarks>
        public enum 广告组类型
        {
            图片 = 10,
            代码 = 20,
        }

        /// <summary>
        /// 广告组状态
        /// 数据表:FeAdvertGroup 字段:Status
        /// </summary>
        /// <remarks>2013-06-18 吴文强 创建</remarks>
        public enum 广告组状态
        {
            启用 = 1,
            禁用 = 0,
        }

        /// <summary>
        /// 广告打开方式
        /// 数据表:FeAdvertItem 字段:OpenType
        /// </summary>
        /// <remarks>2013-06-18 吴文强 创建</remarks>
        public enum 广告打开方式
        {
            新窗口 = 1,
            原窗口 = 0,
        }

        /// <summary>
        /// 广告项状态
        /// 数据表:FeAdvertItem 字段:Status
        /// </summary>
        /// <remarks>2013-06-18 吴文强 创建</remarks>
        public enum 广告项状态
        {
            待审 = 10,
            已审 = 20,
            作废 = -10,
        }

        /// <summary>
        /// 商品组平台类型
        /// 数据表:FeProductGroup 字段:PlatformType
        /// </summary>
        /// <remarks>2013-06-18 吴文强 创建</remarks>
        public enum 商品组平台类型
        {
            PC网站 = 10,
            手机商城 = 30,
            商城IphoneApp = 31,
            商城AndroidApp = 32,
        }

        /// <summary>
        /// 商品组状态
        /// 数据表:FeProductGroup 字段:Status
        /// </summary>
        /// <remarks>2013-06-18 吴文强 创建</remarks>
        public enum 商品组状态
        {
            启用 = 1,
            禁用 = 0,
        }

        /// <summary>
        /// 显示标志
        /// 数据表:FeProductItem 字段:DispalySymbol
        /// </summary>
        /// <remarks>2013-06-18 吴文强 创建</remarks>
        public enum 显示标志
        {
            正常 = 10,
            新品 = 20,
            热销 = 30,
            特价 = 40,
        }

        /// <summary>
        /// 商品项状态
        /// 数据表:FeProductItem 字段:Status
        /// </summary>
        /// <remarks>2013-06-18 吴文强 创建</remarks>
        public enum 商品项状态
        {
            待审 = 10,
            已审 = 20,
            作废 = -10,
        }

        /// <summary>
        /// 是否精华
        /// 数据表:FeProductComment 字段:IsBest
        /// </summary>
        /// <remarks>2013-06-18 吴文强 创建</remarks>
        public enum 是否精华
        {
            是 = 1,
            否 = 0,
        }

        /// <summary>
        /// 是否置顶
        /// 数据表:FeProductComment 字段:IsTop
        /// </summary>
        /// <remarks>2013-06-18 吴文强 创建</remarks>
        public enum 是否置顶
        {
            是 = 1,
            否 = 0,
        }

        /// <summary>
        /// 是否评论
        /// 数据表:FeProductComment 字段:IsComment
        /// </summary>
        /// <remarks>2013-07-26 吴文强 创建</remarks>
        public enum 是否评论
        {
            是 = 1,
            否 = 0,
        }

        /// <summary>
        /// 是否晒单
        /// 数据表:FeProductComment 字段:IsShare
        /// </summary>
        /// <remarks>2013-07-26 吴文强 创建</remarks>
        public enum 是否晒单
        {
            是 = 1,
            否 = 0,
        }

        /// <summary>
        /// 商品评论状态
        /// 数据表:FeProductComment 字段:CommentStatus
        /// </summary>
        /// <remarks>2013-06-18 吴文强 创建</remarks>
        public enum 商品评论状态
        {
            待审 = 10,
            已审 = 20,
            作废 = -10,
        }

        /// <summary>
        /// 商品晒单状态
        /// 数据表:FeProductComment 字段:ShareStatus
        /// </summary>
        /// <remarks>2013-07-25 吴文强 创建</remarks>
        public enum 商品晒单状态
        {
            待审 = 10,
            已审 = 20,
            作废 = -10,
        }

        /// <summary>
        /// 商品评论回复状态
        /// 数据表:FeProductCommentReply 字段:Status
        /// </summary>
        /// <remarks>2013-06-18 吴文强 创建</remarks>
        public enum 商品评论回复状态
        {
            待审 = 10,
            已审 = 20,
            作废 = -10,
        }

        /// <summary>
        /// 晒单图片状态
        /// 数据表:FeProductCommentImage 字段:Status
        /// </summary>
        /// <remarks>2013-06-18 吴文强 创建</remarks>
        public enum 晒单图片状态
        {
            待审 = 10,
            已审 = 20,
            作废 = -10,
        }

        /// <summary>
        /// 文章分类类型
        /// 数据表:FeArticleCategory 字段:Type
        /// </summary>
        /// <remarks>2013-06-18 吴文强 创建</remarks>
        public enum 文章分类类型
        {
            新闻 = 10,
            帮助 = 20,
            APP新闻 = 30,
            APP帮助 = 40,
        }

        /// <summary>
        /// 文章分类状态
        /// 数据表:FeArticleCategory 字段:Status
        /// </summary>
        /// <remarks>2013-06-18 吴文强 创建</remarks>
        public enum 文章分类状态
        {
            待审 = 10,
            已审 = 20,
            作废 = -10,
        }

        /// <summary>
        /// 文章状态
        /// 数据表:FeArticle 字段:Status
        /// </summary>
        /// <remarks>2013-06-18 吴文强 创建</remarks>
        public enum 文章状态
        {
            待审 = 10,
            已审 = 20,
            作废 = -10,
        }

        /// <summary>
        /// 搜索关键字状态
        /// 数据表:FeSearchKeyword 字段:Status
        /// </summary>
        /// <remarks>2013-06-18 吴文强 创建</remarks>
        public enum 搜索关键字状态
        {
            前台显示 = 1,
            后台记录 = 0,
        }

        /// <summary>
        /// 新闻状态
        /// 数据表:FeNews 字段:Status
        /// </summary>
        /// <remarks>2014-01-14 吴文强 创建</remarks>
        public enum 新闻状态
        {
            待审 = 10,
            已审 = 20,
            作废 = -10,
        }

        /// <summary>
        /// 软件分类状态
        /// 数据表:FeSoftCategory 字段:Status
        /// </summary>
        /// <remarks>2014-01-14 吴文强 创建</remarks>
        public enum 软件分类状态
        {
            启用 = 1,
            禁用 = 0,
        }

        /// <summary>
        /// 软件下载状态
        /// 数据表:FeSoftware 字段:Status
        /// </summary>
        /// <remarks>2014-01-14 吴文强 创建</remarks>
        public enum 软件下载状态
        {
            待审 = 10,
            已审 = 20,
            作废 = -10,
        }

        /// <summary>
        /// 软件列表状态
        /// 数据表:FeSoftwareList 字段:Status
        /// </summary>
        /// <remarks>2014-01-14 吴文强 创建</remarks>
        public enum 软件列表状态
        {
            普通 = 10,
            Iphone = 20,
            Android = 30,
        }

    }
}
