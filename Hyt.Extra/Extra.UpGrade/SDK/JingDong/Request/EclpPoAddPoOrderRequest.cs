using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class EclpPoAddPoOrderRequest : IJdRequest<EclpPoAddPoOrderResponse>
{
		                                                                                                                                  
public   		string
   spPoOrderNo  { get; set; }

                  
                                                            
                                                          
public   		string
   deptNo  { get; set; }

                  
                                                            
                                                          
public   		string
   whNo  { get; set; }

                  
                                                            
                                                          
public   		string
   supplierNo  { get; set; }

                  
                                                            
                                                          
public   		string
   billType  { get; set; }

                  
                                                            
                                                          
public   		string
   acceptUnQcFlag  { get; set; }

                  
                                                            
                                                          
public   		string
   boxFlag  { get; set; }

                  
                                                            
                                                                                                                                                                                                                                                                                                                                                            		public  		string
   boxNo  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   boxGoodsNo  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   boxGoodsQty  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   boxSerialNo  { get; set; }
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                   		public  		string
   deptGoodsNo  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   numApplication  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   goodsStatus  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   barCodeType  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   sidCheckout  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   qualityCheckRate  { get; set; }
                                                                                                                                                                       
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.eclp.po.addPoOrder";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("spPoOrderNo", this.spPoOrderNo);
			parameters.Add("deptNo", this.deptNo);
			parameters.Add("whNo", this.whNo);
			parameters.Add("supplierNo", this.supplierNo);
			parameters.Add("billType", this.billType);
			parameters.Add("acceptUnQcFlag", this.acceptUnQcFlag);
			parameters.Add("boxFlag", this.boxFlag);
			parameters.Add("boxNo", this.boxNo);
			parameters.Add("boxGoodsNo", this.boxGoodsNo);
			parameters.Add("boxGoodsQty", this.boxGoodsQty);
			parameters.Add("boxSerialNo", this.boxSerialNo);
			parameters.Add("deptGoodsNo", this.deptGoodsNo);
			parameters.Add("numApplication", this.numApplication);
			parameters.Add("goodsStatus", this.goodsStatus);
			parameters.Add("barCodeType", this.barCodeType);
			parameters.Add("sidCheckout", this.sidCheckout);
			parameters.Add("qualityCheckRate", this.qualityCheckRate);
            parameters.AddAll(this.otherParameters);
            return parameters;
        }

        public void Validate()
        {
        }

        public void AddOtherParameter(string key, string value)
        {
            if (this.otherParameters == null)
            {
                this.otherParameters = new JdDictionary();
            }
            this.otherParameters.Add(key, value);
        }

}
}








        
 

