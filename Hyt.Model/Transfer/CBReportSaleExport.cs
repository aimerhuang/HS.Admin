using System;

namespace Hyt.Model.Transfer
{
    public class CBReportSaleExport
    {
        public DateTime 下单日期 { get; set; }
        public string 订单号 { get; set; }
        public string 订单来源 { get; set; }
        public string 下单门店 { get; set; }
        public DateTime 出库日期 { get; set; }
        public string 会员名 { get; set; }
        public string ERP编码 { get; set; }
        public string 产品名称 { get; set; }
        public int 数量 { get; set; }
        public decimal 单价 { get; set; }
        public decimal 优惠 { get; set; }
        public decimal 销售金额 { get; set; }
        public decimal 实收金额 { get; set; }
        public string 出入库仓库 { get; set; }
        public string 收款方式 { get; set; }
        //public string 退款方式 { get; set; }
        public string 配送方式 { get; set; }
        //public string 售后方式 { get; set; }
        //public string 发票号 { get; set; }
        public string 快递单号 { get; set; }
        public string 联系电话 { get; set; }
        public string 送货员 { get; set; }
        public string 客服 { get; set; }
        public string 结算状态 { get; set; }
        public string 店铺名称 { get; set; }
        public string 商城订单号 { get; set; }

        public string 收货人 { get; set; }

        public string 省 { get; set; }
        public string 市 { get; set; }
        public string 区 { get; set; }

        
        public string 收货地址 { get; set; }

        public string 对内备注 { get; set; }

        public int 出库单号 { get; set; }

        public DateTime? 发货日期 { get; set; }
    }

}