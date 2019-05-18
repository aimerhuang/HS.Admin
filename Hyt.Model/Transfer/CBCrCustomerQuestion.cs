using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model
{
    /// <summary>
    /// 商品咨询查询实体类
    /// </summary>
    /// <remarks>
    /// 2013-06-25 苟治国 创建
    /// 2013-08-13 邵  斌 添加会员等级信息
    /// 2013-08-21 郑荣华 改为继承
    /// </remarks>
    public class CBCrCustomerQuestion : CrCustomerQuestion
    {
        /// <summary>
        /// 商品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 商品图片
        /// </summary>
        /// <remarks>
        /// 2013-06-25 苟治国 创建
        /// </remarks>
        public string productImage { get; set; }

        /// <summary>
        /// 会员编号
        /// </summary>
        /// <remarks>
        /// 2013-06-25 苟治国 创建
        /// </remarks>
        public int CustomerNo { get; set; }

        /// <summary>
        /// 会员手机名称
        /// </summary>
        /// <remarks>
        /// 2013-06-25 苟治国 创建
        /// </remarks>
        public string MobilePhoneNumber { get; set; }

        /// <summary>
        /// 客服账号
        /// </summary>
        /// <remarks>
        /// 2013-06-25 苟治国 创建
        /// </remarks>
        public string UserName { get; set; }

        /// <summary>
        /// 会员昵称
        /// </summary>
        /// <remarks>
        /// 2013-06-25 苟治国 创建
        /// </remarks>
        public string NickName { get; set; }
        
    }
}
