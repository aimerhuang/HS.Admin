using Hyt.Model.TYO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extra.Express.Model
{
    /// <summary>
    /// 快递参数
    /// </summary>
    /// <remarks>2017-12-13 杨浩 创建</remarks>
    public class ExpressParameters
    {
        #region 圆通电子面单参数
        /// <summary>
        /// 商家代码（必须与customerId一致） 	 	
        /// </summary>
        public String ClientID { get; set; }
        /// <summary>
        /// 物流公司ID 	Y 	必须为YTO
        /// </summary>
        public String LogisticProviderID { get; set; }
        /// <summary>
        /// 商家代码 (由商家设置， 必须与clientID一致) Y 	
        /// </summary>
        public String CustomerId { get; set; }
        /// <summary>
        /// 物流订单号  Y 。注： clientID +数字，
        /// </summary>
        public String TxLogisticID { get; set; }
        /// <summary>
        /// 业务交易号（可选） 	N 
        /// </summary>
        public String TradeNo { get; set; }
        /// <summary>
        /// 物流运单号 	 N 	
        /// </summary>
        public String MailNo { get; set; }
        /// <summary>
        /// 订单类型（该字段是为向下兼容预留） N 	必须为正整数
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 订单类型(0-COD,1-普通订单,2-便携式订单,3-退货单,4-到付)   	Y 	
        /// </summary>
        public int OrderType { get; set; }
        /// <summary>
        /// 服务类型(1-上门揽收, 2-次日达 4-次晨达 8-当日达,0-自己联系)。默认为0 		Y 	
        /// </summary>
        public long ServiceType { get; set; }
        /// <summary>
        /// 订单flag标识，默认为 0，暂无意义 	N 	
        /// </summary>
        public int Flag { get; set; }
        ///// <summary>
        ///// 发件人信息 	 	Y 	
        ///// </summary>
        //public Sender sender { get; set; }
        ///// <summary>
        ///// 收件人信息 	  	Y 	 
        ///// </summary>
        //public Receiver receiver { get; set; }
        /// <summary>
        /// 物流公司上门取货时间段，通过”yyyy-MM-dd HH:mm:ss”格式化，本文中所有时间格式相同。  	N 	
        /// </summary>
        public DateTime SendStartTime { get; set; }
        /// <summary>
        ///物流公司上门取货时间段  N 	
        /// </summary>
        public DateTime SendEndTime { get; set; }
        /// <summary>
        /// 商品金额，包括优惠和运费，但无服务费   	N 	
        /// </summary>
        public double GoodsValue { get; set; }
        /// <summary>
        /// goodsValue+总服务费   	N 	
        /// </summary>
        public double TotalValue { get; set; }
        /// <summary>
        /// 代收金额，如果是代收订单， 必须大于0；非代收设置为0.0	N 	
        /// </summary>
        public double AgencyFund { get; set; }
        /// <summary>
        /// 货物价值   	N 	
        /// </summary>
        public double ItemsValue { get; set; }
        /// <summary>
        /// 货物总重量 	 	N 	
        /// </summary>
        public double ItemsWeight { get; set; }
        /// <summary>
        /// 商品名称    Y 
        /// </summary>
        public String ItemName { get; set; }
        /// <summary>
        /// 商品数量  Y 
        /// </summary>
        public int Number { get; set; }
        /// <summary>
        /// 商品单价（两位小数） N 	
        /// </summary>
        public double ItemValue { get; set; }
        /// <summary>
        /// 商品类型（保留字段，暂时不用） 	N 	
        /// </summary>
        public int Special { get; set; }
        /// <summary>
        /// 备注 	N 	
        /// </summary>
        public String Remark { get; set; }
        /// <summary>
        /// 保值金额 （保价金额为货品价值（大于等于100少于3w），默认为0.0）   	N 	
        /// </summary>
        public double InsuranceValue { get; set; }
        /// <summary>
        /// 保值金额=insuranceValue*货品数量(默认为0.0）  	N 	
        /// </summary>
        public double TotalServiceFee { get; set; }
        /// <summary>
        /// 物流公司分润[COD] （暂时没有使用，默认为0.0）  	N 	
        /// </summary>
        public double CodSplitFee { get; set; } 
        #endregion

        #region 收件人信息
        /// <summary>
        /// 收件人用户姓名    	Y 	
        /// </summary>
        public String Rname { get; set; }
        /// <summary>
        /// 收件人用户邮编 	n                               	
        /// </summary>
        public int Rpostcode { get; set; }
        /// <summary>
        /// 收件人用户电话，包括区号、电话号码及分机号，中间用“-”分隔； n  	  
        /// </summary>
        public string Rphone { get; set; }
        /// <summary>
        /// 收件人用户移动电话， 手机和电话至少填一项 	n 	 
        /// </summary>
        public string Rmobile { get; set; }
        /// <summary>
        /// 收件人省份  y 	
        /// </summary>
        public string Rprov { get; set; }
        /// <summary>
        /// 收件人城市与区县， 城市与区县用英文逗号隔开 	y 	 
        /// </summary>
        public string Rcity { get; set; }
        /// <summary>
        /// 收件人详细地址（注：不包含省市区） 	y 	
        /// </summary>
        public string Raddress { get; set; } 
        #endregion

        #region 发件人信息

        /// <summary>
        /// 发件人用户姓名    	Y 	
        /// </summary>
        public String Sname { get; set; }
        /// <summary>
        /// 发件人用户邮编 	n                               	
        /// </summary>
        public int Spostcode { get; set; }
        /// <summary>
        /// 发件人用户电话，包括区号、电话号码及分机号，中间用“-”分隔； n  	  
        /// </summary>
        public string Sphone { get; set; }
        /// <summary>
        /// 发件人用户移动电话， 手机和电话至少填一项 	n 	 
        /// </summary>
        public string Smobile { get; set; }
        /// <summary>
        /// 发件人省份  y 	
        /// </summary>
        public string Sprov { get; set; }
        /// <summary>
        /// 发件人城市与区县， 城市与区县用英文逗号隔开 	y 	 
        /// </summary>
        public string Scity { get; set; }
        /// <summary>
        /// 发件人详细地址（注：不包含省市区） 	y 	
        /// </summary>
        public string Saddress { get; set; }	 	  
        #endregion

        #region KK接口 参数
        /// <summary>
        /// 客户编码（测试用TESTSTD）
        /// </summary>
        public string ClientId { get; set; }
        /// <summary>
        /// 申报海关（GZHG）
        /// </summary>
        public string CustomsID { get; set; }
        /// <summary>
        /// 交易订单编号，新增
        /// </summary>
        public string TradeId { get; set; }
        /// <summary>
        /// 物流订单编号
        /// </summary>
        public string OrderCode { get; set; }
        /// <summary>
        /// 运单号
        /// </summary>
        public string Waybill { get; set; }
        /// <summary>
        /// 总运单号
        /// </summary>
        public string TotalWayBill { get; set; }
        /// <summary>
        /// 件数
        /// </summary>
        public int PackNo { get; set; }
        /// <summary>
        /// 毛重，单位：KG
        /// </summary>
        public double GrossWeigt { get; set; }
        /// <summary>
        /// 净重，单位：KG
        /// </summary>
        public double NetWeight { get; set; }
        /// <summary>
        /// 主要货物名称 不能超过19个字符
        /// </summary>
        public string GoodsName { get; set; }
        /// <summary>
        /// 发件地区
        /// </summary>
        public string SendArea { get; set; }
        /// <summary>
        /// 收件地区
        /// </summary>
        public string ConsigneeArea { get; set; }
        /// <summary>
        /// 收件人名称
        /// </summary>
        public string Consignee { get; set; }
        /// <summary>
        /// 收件人地址
        /// </summary>
        public string ConsigneeAddress { get; set; }
        /// <summary>
        /// 收件人联系方式
        /// </summary>
        public string ConsigneeTel { get; set; }
        /// <summary>
        /// 邮编
        /// </summary>
        public string ZipCode { get; set; }
        ///<summary>
        /// 关区代码
        /// </summary>
        public string CustomsCode { get; set; }
        /// <summary>
        /// 价值
        /// </summary>
        public double Worth { get; set; }
        /// <summary>
        /// 进口日期
        /// </summary>
        public string ImportDateStr { get; set; }
        /// <summary>
        /// 币制,142等
        /// </summary>
        public string CurrCode { get; set; }
        /// <summary>
        /// 操作类型，A-新增；M-修改
        /// </summary>
        public string ModifyMark { get; set; }
        /// <summary>
        /// 业务类型，1-一般进口；3-保税进口
        /// </summary>
        public string BusinessType { get; set; }
        /// <summary>
        /// 保价费，新增
        /// </summary>
        public double InsuredFee { get; set; }
        /// <summary>
        /// 运费，新增
        /// </summary>
        public double Freight { get; set; }
        /// <summary>
        /// 扩展字段，key=value格式，新增
        /// </summary>
        public string Feature { get; set; } 
        #endregion

    }
}
