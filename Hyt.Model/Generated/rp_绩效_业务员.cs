
using System;
using System.ComponentModel;
namespace Hyt.Model
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// 2014-01-06 杨浩 T4生成
    /// </remarks>
    [Serializable]
    public partial class RP_绩效_业务员
    {
        /// <summary>
        /// 
        /// </summary>
        [Description("")]
        public int 业务员编号 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Description("")]
        public string 姓名 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Description("")]
        public int 仓库编号 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Description("")]
        public string 仓库 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Description("")]
        public int 配送次数_白班 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Description("")]
        public int 配送次数_夜班 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Description("")]
        public int 配送单量_白班 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Description("")]
        public int 配送单量_夜班 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Description("")]
        public decimal 配送金额_升舱 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Description("")]
        public decimal 配送金额_商城 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Description("")]
        public decimal 自销金额 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Description("")]
        public string 统计日期 { get; set; }

        public int 配送单量 { get; set; }

        public int 百城通签收订单 { get; set; }

        // <summary>
        /// 
        /// </summary>
        [Description("")]
        public string 办事处 { get; set; }
    }
}

