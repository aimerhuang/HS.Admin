using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Procurement
{
    public class CBPmGoodsDelivery : PmGoodsDelivery
    {
        /// <summary>
        /// 当前操作人
        /// </summary>
        public string CurrentName { get; set; }
        /// <summary>
        /// 当前操作时间
        /// </summary>
        public string CurrentTime { get; set; }
        /// <summary>
        /// 货品装箱配送但明细
        /// </summary>
        public List<CBPmGoodsDeliveryItem> ListItems { get; set; }
    }

    /// <summary>
    /// 货品装箱配送单
    /// </summary>
    public class PmGoodsDelivery
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        public int SysNo { get; set; }
        /// <summary>
        /// 配送单编号
        /// </summary>
        public string gd_PaketNumber { get; set; }
        /// <summary>
        /// 配送公司
        /// </summary>
        public string gd_DeliveryCompanyName { get; set; }
        /// <summary>
        /// 集装箱名称
        /// </summary>
        public string gd_DeliveryContainer { get; set; }
        /// <summary>
        /// 物流配送号
        /// </summary>
        public string gd_DeliveryNumber { get; set; }
        /// <summary>
        /// 配送时间
        /// </summary>
        public DateTime gd_DeliveryTime { get; set; }
        /// <summary>
        /// 配送单定义人员
        /// </summary>
        public int gd_DeliveryUserSys { get; set; }
        /// <summary>
        /// 收货时间
        /// </summary>
        public DateTime gd_GetDeliveryTime { get; set; }
        /// <summary>
        /// 配送单状态
        /// </summary>
        public int gd_Status { get; set; }
        /// <summary>
        /// 厂家采购单id
        /// </summary>
        public int gd_PSysNo { get; set; }
        /// <summary>
        /// 物流配送信息
        /// </summary>
        public string gd_DeliveryInfo { get;set;}
        /// <summary>
        /// 获取商品配送单类型。1为厂家采购，2为商品库配送发货
        /// </summary>
        public int gd_Type { get; set; }
    }
}
