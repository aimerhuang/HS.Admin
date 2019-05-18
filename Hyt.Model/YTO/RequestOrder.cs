using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.TYO
{
    /// <summary>
    /// 圆通电子面单接口参数
    /// </summary>
    /// <remarks>2017-12-9 廖移凤 创建</remarks>.
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public class RequestOrder
    {
        /// <summary>
        /// 商家代码（必须与customerId一致） 	 	
        /// </summary>
       public String clientID { get; set; } 	
        /// <summary>
        /// 物流公司ID 	Y 	必须为YTO
        /// </summary>
      public String logisticProviderID { get; set; }
        /// <summary>
        /// 商家代码 (由商家设置， 必须与clientID一致) Y 	
        /// </summary>
      public String  customerId { get; set; }	
        /// <summary>
        /// 物流订单号  Y 。注： clientID +数字，
        /// </summary>
     public String  txLogisticID { get; set; }	 	
        /// <summary>
        /// 业务交易号（可选） 	N 
        /// </summary>
      public String tradeNo { get; set; }	 	 	
        /// <summary>
        /// 物流运单号 	 N 	
        /// </summary>
      public String mailNo { get; set; }	
        /// <summary>
        /// 订单类型（该字段是为向下兼容预留） N 	必须为正整数
        /// </summary>
      public int  type { get; set; }
        /// <summary>
        /// 订单类型(0-COD,1-普通订单,2-便携式订单,3-退货单,4-到付)   	Y 	
        /// </summary>
    public int   orderType { get; set; }	
        /// <summary>
        /// 服务类型(1-上门揽收, 2-次日达 4-次晨达 8-当日达,0-自己联系)。默认为0 		Y 	
        /// </summary>
     public long  serviceType { get; set; }	
        /// <summary>
        /// 订单flag标识，默认为 0，暂无意义 	N 	
        /// </summary>
      public int flag { get; set; }	
        /// <summary>
        /// 发件人信息 	 	Y 	
        /// </summary>
      public Sender sender { get; set; }	
        /// <summary>
        /// 收件人信息 	  	Y 	 
        /// </summary>
      public Receiver receiver { get; set; }	
        /// <summary>
        /// 物流公司上门取货时间段，通过”yyyy-MM-dd HH:mm:ss”格式化，本文中所有时间格式相同。  	N 	
        /// </summary>
      public DateTime sendStartTime { get; set; }	
        /// <summary>
        ///物流公司上门取货时间段  N 	
        /// </summary>
     public  DateTime sendEndTime { get; set; }	 	  	
        /// <summary>
        /// 商品金额，包括优惠和运费，但无服务费   	N 	
        /// </summary>
      public double goodsValue { get; set; }	
        /// <summary>
        /// goodsValue+总服务费   	N 	
        /// </summary>
      public double totalValue { get; set; }	
        /// <summary>
        /// 代收金额，如果是代收订单， 必须大于0；非代收设置为0.0	N 	
        /// </summary>
      public double agencyFund { get; set; }	
        /// <summary>
        /// 货物价值   	N 	
        /// </summary>
     public  double itemsValue { get; set; }	
        /// <summary>
        /// 货物总重量 	 	N 	
        /// </summary>
      public double itemsWeight { get; set; }	
        /// <summary>
        /// 商品名称    Y 
        /// </summary>
      public String itemName 	{ get; set; }
        /// <summary>
        /// 商品数量  Y 
        /// </summary>
      public int number 	{ get; set; }
        /// <summary>
        /// 商品单价（两位小数） N 	
        /// </summary>
      public double itemValue { get; set; }	
        /// <summary>
        /// 商品类型（保留字段，暂时不用） 	N 	
        /// </summary>
      public int special { get; set; }	
        /// <summary>
        /// 备注 	N 	
        /// </summary>
      public String remark { get; set; }	
        /// <summary>
        /// 保值金额 （保价金额为货品价值（大于等于100少于3w），默认为0.0）   	N 	
        /// </summary>
      public double insuranceValue { get; set; }	
        /// <summary>
        /// 保值金额=insuranceValue*货品数量(默认为0.0）  	N 	
        /// </summary>
      public double totalServiceFee { get; set; }	
        /// <summary>
        /// 物流公司分润[COD] （暂时没有使用，默认为0.0）  	N 	
        /// </summary>
      public double codSplitFee { get; set; }	

     

    }
    /// <summary>
    /// 圆通电子面单接口收件人信息
    /// </summary>
    /// <remarks>2017-12-9 廖移凤 创建</remarks>
  [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public class Receiver
   { 
        /// <summary>
        /// 用户姓名    	Y 	
        /// </summary>
      public String name  { get; set; }	     
    	/// <summary>
    	/// 用户邮编 	n                               	
    	/// </summary>
      public int postcode { get; set; }		
        /// <summary>
        /// 用户电话，包括区号、电话号码及分机号，中间用“-”分隔； n  	  
        /// </summary>
      public string  phone { get; set; }		 
        /// <summary>
        /// 用户移动电话， 手机和电话至少填一项 	n 	 
        /// </summary>
      public string mobile{ get; set; }	 	
        /// <summary>
        /// 省份  y 	
        /// </summary>
      public string  prov{ get; set; }	   
        /// <summary>
        /// 城市与区县， 城市与区县用英文逗号隔开 	y 	 
        /// </summary>
      public  string city{ get; set; }	 
        /// <summary>
      /// 详细地址（注：不包含省市区） 	y 	
        /// </summary>
       public string address{ get; set; }	 	 

    }

    /// <summary>
    /// 圆通电子面单接口发件人信息 	 
    /// </summary>
    /// <remarks>2017-12-9 廖移凤 创建</remarks>
  [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public class Sender
   {
        /// <summary>
        /// 用户姓名    	Y 	
        /// </summary>
        public String name { get; set; }
        /// <summary>
        /// 用户邮编 	n                               	
        /// </summary>
        public int postcode { get; set; }
        /// <summary>
        /// 用户电话，包括区号、电话号码及分机号，中间用“-”分隔； n  	  
        /// </summary>
        public string phone { get; set; }
        /// <summary>
        /// 用户移动电话， 手机和电话至少填一项 	n 	 
        /// </summary>
        public string mobile { get; set; }
        /// <summary>
        /// 省份  y 	
        /// </summary>
        public string prov { get; set; }
        /// <summary>
        /// 城市与区县， 城市与区县用英文逗号隔开 	y 	 
        /// </summary>
        public string city { get; set; }
        /// <summary>
        /// 详细地址（注：不包含省市区） 	y 	
        /// </summary>
        public string address { get; set; }	 	 
    
    }
}
