using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extra.UpGrade.Model
{
    /// <summary>
    /// 拍拍订单信息
    /// </summary>
    /// <remarks>2013-11-10 陶辉 创建</remarks>
    public class DealInfo
    {
        /// <summary>
        /// 返回码
        /// </summary>
        public string errorCode { get; set; }

        /// <summary>
        /// 错误描述
        /// </summary>
        public string errorMessage { get; set; }

        /// <summary>
        /// 促销信息
        /// </summary>
        public string comboInfo { get; set; }

        /// <summary>
        /// 订单编码
        /// </summary>
        public string dealCode { get; set; }

        /// <summary>
        /// 买家下单时的留言内容
        /// </summary>
        public string buyerRemark { get; set; }

        /// <summary>
        /// 财付通付款单号
        /// </summary>
        public string tenpayCode { get; set; }

        /// <summary>
        /// 物流的编码
        /// </summary>
        public string wuliuId { get; set; }

        /// <summary>
        /// 收货人地址信息
        /// </summary>
        public string receiverAddress { get; set; }

        /// <summary>
        /// 手机
        /// </summary>
        public string receiverMobile { get; set; }

        /// <summary>
        /// 收货人姓名
        /// </summary>
        public string receiverName { get; set; }

        /// <summary>
        /// 收货人电话号码
        /// </summary>
        public string receiverPhone { get; set; }

        /// <summary>
        /// 收货人邮编
        /// </summary>
        public string receiverPostcode { get; set; }

        /// <summary>
        /// 买家名称
        /// </summary>
        public string buyerName { get; set; }

        /// <summary>
        /// 买家QQ号
        /// </summary>
        public string buyerUin { get; set; }

        /// <summary>
        /// 运费
        /// </summary>
        public string freight { get; set; }

        /// <summary>
        /// 费用合计,一共要付的钱（包括可折合钱：积分、财付券之类）
        /// </summary>
        public decimal dealPayFeeTotal { get; set; }

        /// <summary>
        /// 总支付现金
        /// </summary>
        public decimal totalCash { get; set; }

        /// <summary>
        /// 财付券支付金额
        /// </summary>
        public decimal dealPayFeeTicket { get; set; }

        /// <summary>
        /// 折扣优惠金额
        /// </summary>
        public decimal couponFee { get; set; }

        /// <summary>
        /// 订单状态
        /// </summary>
        public string dealState { get; set; }

        /// <summary>
        /// 下单时间
        /// </summary>
        public DateTime createTime { get; set; }

        /// <summary>
        /// 买家付款时间
        /// </summary>
        public DateTime payTime { get; set; }

        /// <summary>
        /// 订单的备注类型
        /// </summary>
        public string dealNoteType { get; set; }

        /// <summary>
        /// 订单的备注内容
        /// </summary>
        public string dealNote { get; set; }

        /// <summary>
        /// 订单的商品列表
        /// </summary>
        public List<DealDetail> itemList { get; set; }

    }
}
