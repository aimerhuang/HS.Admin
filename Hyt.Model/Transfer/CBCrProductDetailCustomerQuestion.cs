using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model
{
    /// <summary>
    /// 商品详情中产品咨询
    /// </summary>
    /// <remarks>2014-01-24 邵斌 创建</remarks>
    [Serializable]
    public class CBCrProductDetailCustomerQuestion:CrCustomerQuestion
    {
        /// <summary>
        /// 会员账号
        /// </summary>
        /// <remarks>2014-01-24 邵斌 创建</remarks>
        public string CustomerAccount { get; set; }

        /// <summary>
        /// 会员昵称
        /// </summary>
        /// <remarks>2014-01-24 邵斌 创建</remarks>
        public string CustomerNickName { get; set; }

        /// <summary>
        /// 客户头像
        /// </summary>
        /// <remarks>2014-01-24 邵斌 创建</remarks>
        public string HeadImage { get; set; }

        /// <summary>
        /// 会员等级编号
        /// </summary>
        /// <remarks>2014-01-24 邵斌 创建</remarks>
        public int LevelSysNo { get; set; }

        /// <summary>
        /// 会员等级名称
        /// </summary>
        /// <remarks>2014-01-24 邵斌 创建</remarks>
        public string LevelName { get; set; }

        /// <summary>
        /// 会员手机名称
        /// </summary>
        /// <remarks>2014-01-24 邵斌 创建</remarks>
        public string MobilePhoneNumber { get; set; }

        /// <summary>
        /// 客服账号
        /// </summary>
        /// <remarks>2014-01-24 邵斌 创建</remarks>
        public string UserName { get; set; }
    }
}
