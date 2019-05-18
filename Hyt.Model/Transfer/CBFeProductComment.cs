using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model
{
    /// <summary>
    ///  商品评论查询实体类
    /// </summary>
    /// <remarks>
    /// 2013-06-28 14:46 苟治国 创建
    /// 2013-07-10  杨晗 修改
    /// 2013-08-12  邵斌 修改 添加回复和晒单属性
    /// </remarks>
    [Serializable]
    public class CBFeProductComment : FeProductComment
    {
        /// <summary>
        /// 用户账号
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 会员名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 用户等级
        /// </summary>
        public int LevelSysNo { get; set; }

        /// <summary>
        /// 用户等级名称
        /// </summary>
        public string LevelName { get; set; }

        /// <summary>
        /// 会员手机号
        /// </summary>
        public string MobilePhoneNumber { get; set; }

        /// <summary>
        /// 用户头像
        /// </summary>
        public string HeadImage { get; set; }

        /// <summary>
        /// 商品编号
        /// </summary>
        public string ErpCode { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 评价回复
        /// </summary>
        public IList<CBFeProductCommentReply> Reply { get; set; }

        /// <summary>
        /// 商品晒单图片
        /// </summary>
        public IList<FeProductCommentImage> ShowMyProductImage { get; set; }
    }
}
