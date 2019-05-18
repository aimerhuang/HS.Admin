using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model
{
    /// <summary>
    /// 文章扩展属性类
    /// </summary>
    /// <remarks>
    /// 2013-07-02 杨晗 创建
    /// </remarks>
    public class CBFeArticle : FeArticle
    {
        /// <summary>
        /// 文章创建人
        /// </summary>
        /// <remarks>
        /// 2013-07-02 杨晗 创建
        /// </remarks>
        public string CreatedByName { get; set; }
        /// <summary>
        /// 文章最后更新人
        /// </summary>
        /// <remarks>
        /// 2013-07-02 杨晗 创建
        /// </remarks>
        public string LastupdateByName { get; set; }
        /// <summary>
        /// 分销商名称
        /// </summary>
        public string DealerName { get; set; }
    }
}
