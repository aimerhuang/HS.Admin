using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Transfer
{
    /// <summary>
    /// 导出订单（用于信营）
    /// </summary>
    /// <remarks>2016-8-4 罗远康 创建</remarks>
    /// <remarks>2017-05-03 罗勤尧 修改</remarks>
    public class CBXXOutputSoOrders
    {
        #region
        public string 序号 { get; set; }
        public string 店铺 { get; set; }
        public string 会员名 { get; set; }
        public string 订单号 { get; set; }
        public string 销售订单号 { get; set; }
        public string 订单人姓名 { get; set; }
        public string 订单人证件号 { get; set; }
        public string 订单人电话 { get; set; }
        public string 销售日期 { get; set; }
        public string 订单日期 { get; set; }
        public string 订单状态 { get; set; }
        public string 买家留言 { get; set; }
        public string 对内备注 { get; set; }
        public string 配送备注 { get; set; }
        public string 收件人地址 { get; set; }
        public string 商品品名 { get; set; }
        public string 商品货号 { get; set; }
        public string 商品条形码 { get; set; }
        public string 商品图片 { get; set; }
        public string 申报数量 { get; set; }
        public string 申报单价 { get; set; }
        public string 申报总价 { get; set; }
        public string 调价金额 { get; set; }
        
        public string 优惠劵金额 { get; set; }
        public string 运费 { get; set; }
        public string 税款 { get; set; }
        public string 毛重 { get; set; }
        public string 净重 { get; set; }
        public string 交易号 { get; set; }
        public string 买家支付账号 { get; set; }
        public string 快递单号 { get; set; }
        public string 支付方式 { get; set; }
        public string 运送方式 { get; set; }
        public string 发货仓库 { get; set; }
        public string 单位 { get; set; }
        public string 付款日期 { get; set; }

        public string 代理商 { get; set; }
        public string 标记 { get; set; }
         public string 第三方付款日期 { get; set; }
        #endregion
        //#region 可以导入金蝶ERP订单数据
        //public string 订单日期 { get; set; }
        //public string 所属网店 { get; set; }
        //public string 交易状态 { get; set; }
        //public string 本地订单号 { get; set; }
        //public string 交易号 { get; set; }
        //public string 支付方式 { get; set; }
        //public string 订单人证件 { get; set; }
        //public string 买家昵称 { get; set; }
        //public string 收货人名 { get; set; }
        //public string 手机号码 { get; set; }
        //public string 省 { get; set; }
        //public string 市 { get; set; }
        //public string 区县 { get; set; }
        //public string 总_金额 { get; set; }
        //public string 总_优惠金额 { get; set; }
        //public string 总_运费 { get; set; }
        //public string 实收金额 { get; set; }
        //public string 制单人 { get; set; }
        //public string 邮政编码 { get; set; }
        //public string 物流公司 { get; set; }
        //public string 快递单号 { get; set; }
        //public string 标记 { get; set; }
        //public string 付款日期 { get; set; }
        //public string 订单来源 { get; set; }
        //public string 运送方式 { get; set; }
        //public string 买家留言 { get; set; }
        //public string 整单优惠金额 { get; set; }
        //public string 买家邮箱 { get; set; }
        //public string 税费 { get; set; }
        //public string 毛重 { get; set; }
        //public string 净重 { get; set; }
        //public string 商品代码 { get; set; }
        //public string 商品名称 { get; set; }
        //public string 辅助属性 { get; set; }
        //public string 单位 { get; set; }
        //public string 数量 { get; set; }
        //public string 单价 { get; set; }
        //public string 单品_金额 { get; set; }
        //public string 单品_优惠金额 { get; set; }
        //public string 单品_运费 { get; set; }
        //public string 商品条码 { get; set; }
        //public string 发货仓库 { get; set; }
        //public string 网上订单号 { get; set; }
        //#endregion
    }
}
