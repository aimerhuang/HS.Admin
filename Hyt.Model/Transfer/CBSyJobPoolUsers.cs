using System;
namespace Hyt.Model
{
    /// <summary>
    /// 订单池客服实体类
    /// </summary>
    /// <remarks>
    /// 2013/6/14 14:46 余勇 创建
    /// </remarks>
    public partial class CBSyJobPoolUsers 
    {
        #region 自定义字段
        /// <summary>
        /// 客服编号
        /// </summary>
        public int SysNo { set; get; }

        /// <summary>
        /// 客服姓名
        /// </summary>
        public string UserName { set; get; }

        /// <summary>
        /// 任务数量
        /// </summary>
        public int TaskNum { set; get; }

        /// <summary>
        /// 最大任务数
        /// </summary>
        public int MaxTaskQuantity { set; get; }

        #endregion
    }
}

