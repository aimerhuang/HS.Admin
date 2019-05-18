using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class LdopReceiveEportSendRequest : IJdRequest<LdopReceiveEportSendResponse>
{
		                                                                                                                                  
public   		string
   deliveryId  { get; set; }

                  
                                                            
                                                          
public   		string
   customerCode  { get; set; }

                  
                                                            
                                                          
public   		string
   packCategory  { get; set; }

                  
                                                            
                                                          
public   		string
   cbeCode  { get; set; }

                  
                                                            
                                                          
public   		string
   cbeName  { get; set; }

                  
                                                            
                                                          
public   		string
   senderUserCountry  { get; set; }

                  
                                                            
                                                          
public   		string
   transferType  { get; set; }

                  
                                                            
                                                          
public   		string
   transferTypeinsp  { get; set; }

                  
                                                            
                                                          
public   		string
   shipNameInsp  { get; set; }

                  
                                                            
                                                          
public   		string
   shipCodeInsp  { get; set; }

                  
                                                            
                                                          
public   		string
   transferRegioninsp  { get; set; }

                  
                                                            
                                                          
public   		string
   packCategoryinsp  { get; set; }

                  
                                                            
                                                          
public   		string
   idType  { get; set; }

                  
                                                            
                                                          
public   		string
   idCode  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   billMode  { get; set; }

                  
                                                            
                                                          
public   		string
   jcborderport  { get; set; }

                  
                                                            
                                                          
public   		string
   jcborderportInsp  { get; set; }

                  
                                                            
                                                          
public   		string
   declareport  { get; set; }

                  
                                                            
                                                          
public   		string
   applyportinsp  { get; set; }

                  
                                                            
                                                          
public   		string
   batchCode  { get; set; }

                  
                                                            
                                                          
public   		string
   shipName  { get; set; }

                  
                                                            
                                                          
public   		string
   tradeCountry  { get; set; }

                  
                                                            
                                                          
public   		string
   cbeCodeinsp  { get; set; }

                  
                                                            
                                                          
public   		string
   coininsp  { get; set; }

                  
                                                            
                                                          
public   		string
   ecpCode  { get; set; }

                  
                                                            
                                                          
public   		string
   ecpName  { get; set; }

                  
                                                            
                                                          
public   		string
   jcborderTime  { get; set; }

                  
                                                            
                                                                                                                                                                                                                                                                                                                                                            		public  		string
   amount  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   goodsName  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   weight  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   netWeight  { get; set; }
                                                                                                                                                                       
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.ldop.receive.eport.send";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("deliveryId", this.deliveryId);
			parameters.Add("customerCode", this.customerCode);
			parameters.Add("packCategory", this.packCategory);
			parameters.Add("cbeCode", this.cbeCode);
			parameters.Add("cbeName", this.cbeName);
			parameters.Add("senderUserCountry", this.senderUserCountry);
			parameters.Add("transferType", this.transferType);
			parameters.Add("transferTypeinsp", this.transferTypeinsp);
			parameters.Add("shipNameInsp", this.shipNameInsp);
			parameters.Add("shipCodeInsp", this.shipCodeInsp);
			parameters.Add("transferRegioninsp", this.transferRegioninsp);
			parameters.Add("packCategoryinsp", this.packCategoryinsp);
			parameters.Add("idType", this.idType);
			parameters.Add("idCode", this.idCode);
			parameters.Add("billMode", this.billMode);
			parameters.Add("jcborderport", this.jcborderport);
			parameters.Add("jcborderportInsp", this.jcborderportInsp);
			parameters.Add("declareport", this.declareport);
			parameters.Add("applyportinsp", this.applyportinsp);
			parameters.Add("batchCode", this.batchCode);
			parameters.Add("shipName", this.shipName);
			parameters.Add("tradeCountry", this.tradeCountry);
			parameters.Add("cbeCodeinsp", this.cbeCodeinsp);
			parameters.Add("coininsp", this.coininsp);
			parameters.Add("ecpCode", this.ecpCode);
			parameters.Add("ecpName", this.ecpName);
			parameters.Add("jcborderTime", this.jcborderTime);
			parameters.Add("amount", this.amount);
			parameters.Add("goodsName", this.goodsName);
			parameters.Add("weight", this.weight);
			parameters.Add("netWeight", this.netWeight);
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








        
 

