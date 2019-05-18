using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extra.UpGrade.GeGeJiaModel
{
    #region 格家订单查询参数

    /// <summary>
    /// 格格家订单查询参数
    /// </summary>
    /// <remarks>2017-08-22 黄杰 创建</remarks>
    public class GeGeJiaResponseResult
    {
        /// <summary>
        /// 业务处理成功或失败
        /// </summary>
        public bool success { get; set; }

        /// <summary>
        /// 订单，order列表
        /// </summary>
        public OrderFormParameter orders { get; set; }

        /// <summary>
        /// 订单总数
        /// </summary>
        public int totalCount { get; set; }
        
        /// <summary>
        /// 当前页订单数目
        /// </summary>
        public int currentCount { get; set; }

        /// <summary>
        /// 最大页数
        /// </summary>
        public int maxPage { get; set; }

        /// <summary>
        /// success=false时返回，错误码，详情请见错误码列表
        /// </summary>
        public string errCode { get; set; }

        /// <summary>
        /// success=false时返回，错描述
        /// </summary>
        public string errMsg { get; set; }

        /// <summary>
        /// 对errMsg的补充
        /// </summary>
        public string detail { get; set; }

    }

    /// <summary>
    /// 订单参数
    /// </summary>
    /// <remarks>2017-08-22 黄杰 创建</remarks>
    public class OrderFormParameter
    {
        /// <summary>
        /// 订单编号
        /// </summary>
        public string number { get; set; }

        /// <summary>
        /// 订单类型，0：渠道订单，1：格格家订单，2：格格团订单，3：格格团全球购订单，4：环球捕手订单，5：燕网订单，6：b2b订单，7：手q，8：云店
        /// </summary>
        public int type { get; set; }

        /// <summary>
        /// 订单状态，1：未付款，2：待发货，3：已发货，4：交易成功，5：用户取消（待退款团购），6：超时取消（已退款团购），7：团购进行中(团购)
        /// </summary>
        public int status { get; set; }

        /// <summary>
        /// 订单创建时间
        /// </summary>
        public string createTime { get; set; }

        /// <summary>
        /// 订单付款时间
        /// </summary>
        public string payTime { get; set; }

        /// <summary>
        /// 订单总金额
        /// </summary>
        public double totalPrice { get; set; }

        /// <summary>
        /// 订单实付金额
        /// </summary>
        public double realPrice { get; set; }

        /// <summary>
        /// 运费
        /// </summary>
        public double freight { get; set; }

        /// <summary>
        /// 优惠券金额
        /// </summary>
        public double couponPrice { get; set; }

        /// <summary>
        /// 订单导出状态，0未导出，1已导出
        /// </summary>
        public int exportStatus { get; set; }

        /// <summary>
        /// 订单审核状态，1：待审核，2：审核通过，3：审核不通过，只有checkStatus=2的订单才能发货
        /// </summary>
        public int checkStatus { get; set; }

        /// <summary>
        /// 订单冻结状态，冻结状态；0：未冻结，1：已冻结，2：已解冻，3：永久冻结，只有freezeStatus=0、2状态的订单才能发货
        /// </summary>
        public int freezeStatus { get; set; }

        /// <summary>
        /// 订单支付方式，枚举值：1[银联]、2[支付宝]、3[微信]、4[其他]
        /// </summary>
        public PayChannels payChannel { get; set; }

        /// <summary>
        /// 第三方平台支付交易号，使用积分付款的订单无交易号
        /// </summary>
        public string payTid { get; set; }

        /// <summary>
        /// 备注：不要发EMS
        /// </summary>
        public double remark { get; set; }

        /// <summary>
        /// 收货人信息
        /// </summary>
        public Consignee receiver { get; set; }

        /// <summary>
        /// 订单商品明细
        /// </summary>
        public OrderFormInfo items { get; set; }
    }


    /// <summary>
    /// 格格家订单备注支付类型
    /// </summary>
    /// <remarks>2017-08-22 黄杰 创建</remarks>
    public enum PayChannels
    {
        银联 = 1,
        支付宝 = 2,
        微信 = 3,
        其他 = 4
    }

    /// <summary>
    /// 收货人信息
    /// </summary>
    /// <remarks>2017-08-22 黄杰 创建</remarks>
    public class Consignee
    {
        /// <summary>
        /// 收货人姓名
        /// </summary>
        public string receiverName { get; set; }

        /// <summary>
        /// 收货人手机号
        /// </summary>
        public string receiverMobile { get; set; }

        /// <summary>
        /// 收货人身份证
        /// </summary>
        public string receiverIdCard { get; set; }

        /// <summary>
        /// 收货省份
        /// </summary>
        public string provinceName { get; set; }

        /// <summary>
        /// 收货省份编码
        /// </summary>
        public string provinceCode { get; set; }

        /// <summary>
        /// 收货城市
        /// </summary>
        public string cityName { get; set; }

        /// <summary>
        /// 收货城市编码
        /// </summary>
        public string cityCode { get; set; }

        /// <summary>
        /// 收货地区
        /// </summary>
        public string districtName { get; set; }

        /// <summary>
        /// 收货地区编码
        /// </summary>
        public string districtCode { get; set; }

        /// <summary>
        /// 收货详细地址
        /// </summary>
        public string detailAddress { get; set; }

    }

    /// <summary>
    /// 订单商品明细
    /// </summary>
    /// <remarks>2017-08-22 黄杰 创建</remarks>
    public class OrderFormInfo
    {
        /// <summary>
        /// 商品名称
        /// </summary>
        public string itemName { get; set; }

        /// <summary>
        /// 商品条码，对应商家后台商品条码
        /// </summary>
        public string itemCode { get; set; }

        /// <summary>
        /// 商品售价
        /// </summary>
        public double salesPrice { get; set; }

        /// <summary>
        /// 商品数量
        /// </summary>
        public int itemCount { get; set; }
    }

    #endregion
}
