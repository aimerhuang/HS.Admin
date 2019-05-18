
using System;
using System.ComponentModel;
namespace Hyt.Model
{
    /// <summary>
	/// 推送订单返回信息表
	/// </summary>
    /// <remarks>
    /// 2015-09-02 王耀发 T4生成
    /// </remarks>
	[Serializable]
    public partial class OutboundReturn
	{

        /// <summary>
        /// 系统编号
        /// </summary>	
        [Description("系统编号")]
        public int SysNo { get; set; }
        /// <summary>
        /// 一号仓生成的发货运单号
        /// </summary>	
        [Description("一号仓生成的发货运单号")]
        public string OutboundOrderNo { get; set; }
        /// <summary>
        /// soOrderSysNo
        /// </summary>	
        [Description("soOrderSysNo")]
        public int soOrderSysNo { get; set; }
        /// <summary>
        /// 0表示成功 1表示失败
        /// </summary>	
        [Description("0表示成功 1表示失败")]
        public string Code { get; set; }
        /// <summary>
        /// 当发生错误时，显示错误信息
        /// </summary>	
        [Description("当发生错误时，显示错误信息")]
        public string Msg { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>	
        [Description("创建人")]
        public int CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>	
        [Description("创建时间")]
        public DateTime CreatedDate { get; set; }
        /// <summary>
        /// 最后更新人
        /// </summary>	
        [Description("最后更新人")]
        public int LastUpdateBy { get; set; }
        /// <summary>
        /// 最后更新时间
        /// </summary>	
        [Description("最后更新时间")]
        public DateTime LastUpdateDate { get; set; }   
 	}
}

	