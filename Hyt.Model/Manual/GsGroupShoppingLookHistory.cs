using System;

namespace Hyt.Model
{
    /// <summary>
    /// 团购浏览历史对象
    /// </summary>
    /// <remarks></remarks>
   public class GsGroupShoppingLookHistory
    {
        /// <summary>
        /// 团购编号
        /// </summary>
        public int SysNo;

        /// <summary>
        /// 团购标题
        /// </summary>
        public string Title;

        /// <summary>
        /// 团购图标
        /// </summary>
        public string Picture;

        /// <summary>
        /// 团购用户
        /// </summary>
        public int UserSysno;

       /// <summary>
       /// 团购价格
       /// </summary>
        public decimal Price;

       /// <summary>
       /// 最近浏览时间
       /// </summary>
        public DateTime UpdateTime;
    }
}
