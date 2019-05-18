using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class WareWriteAddRequest : IJdRequest<WareWriteAddResponse>
{
		                                                                                                                                                                                                                                                                                                                                  
public   		string
   title  { get; set; }

                  
                                                            
                                                                                           
public   		Nullable<long>
   categoryId  { get; set; }

                  
                                                            
                                                          
public   		Nullable<long>
   brandId  { get; set; }

                  
                                                            
                                                          
public   		Nullable<long>
   templateId  { get; set; }

                  
                                                            
                                                          
public   		Nullable<long>
   transportId  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   wareStatus  { get; set; }

                  
                                                            
                                                          
public   		string
   outerId  { get; set; }

                  
                                                            
                                                          
public   		string
   itemNum  { get; set; }

                  
                                                            
                                                          
public   		string
   barCode  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   wareLocation  { get; set; }

                  
                                                            
                                                          
public   		Nullable<long>
   delivery  { get; set; }

                  
                                                            
                                                                                                                      
public   		string
   url  { get; set; }

                  
                                                            
                                                          
public   		string
   urlWords  { get; set; }

                  
                                                            
                                                          
public   		string
   words  { get; set; }

                  
                                                            
                                                                                           
public   		string
   wrap  { get; set; }

                  
                                                            
                                                          
public   		string
   packListing  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   length  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   width  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   height  { get; set; }

                  
                                                            
                                                          
public   		string
   weight  { get; set; }

                  
                                                            
                                                                                                                                                                                                                                                                                                                                                            		public  		string
   attrId  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   attrValues  { get; set; }
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                  		public  		string
   featureKey  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   featureValue  { get; set; }
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                  		public  		string
   colorId  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   imgId  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   imgIndex  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   imgUrl  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   imgZoneId  { get; set; }
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                 		public  		string
   shopCategory  { get; set; }
                                                                                                                                                                                                
public   		string
   mobileDesc  { get; set; }

                  
                                                            
                                                          
public   		string
   introduction  { get; set; }

                  
                                                            
                                                          
public   		string
   afterSales  { get; set; }

                  
                                                            
                                                          
public   		string
   jdPrice  { get; set; }

                  
                                                            
                                                          
public   		string
   marketPrice  { get; set; }

                  
                                                            
                                                                                                                                                                                                                                                                                                                                                                                                                                                      		public  		string
   saleAttrs  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   skuFeatures  { get; set; }
                          
                                                                                                                                      
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.ware.write.add";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("title", this.title);
			parameters.Add("categoryId", this.categoryId);
			parameters.Add("brandId", this.brandId);
			parameters.Add("templateId", this.templateId);
			parameters.Add("transportId", this.transportId);
			parameters.Add("wareStatus", this.wareStatus);
			parameters.Add("outerId", this.outerId);
			parameters.Add("itemNum", this.itemNum);
			parameters.Add("barCode", this.barCode);
			parameters.Add("wareLocation", this.wareLocation);
			parameters.Add("delivery", this.delivery);
			parameters.Add("url", this.url);
			parameters.Add("urlWords", this.urlWords);
			parameters.Add("words", this.words);
			parameters.Add("wrap", this.wrap);
			parameters.Add("packListing", this.packListing);
			parameters.Add("length", this.length);
			parameters.Add("width", this.width);
			parameters.Add("height", this.height);
			parameters.Add("weight", this.weight);
			parameters.Add("attrId", this.attrId);
			parameters.Add("attrValues", this.attrValues);
			parameters.Add("featureKey", this.featureKey);
			parameters.Add("featureValue", this.featureValue);
			parameters.Add("colorId", this.colorId);
			parameters.Add("imgId", this.imgId);
			parameters.Add("imgIndex", this.imgIndex);
			parameters.Add("imgUrl", this.imgUrl);
			parameters.Add("imgZoneId", this.imgZoneId);
			parameters.Add("shopCategory", this.shopCategory);
			parameters.Add("mobileDesc", this.mobileDesc);
			parameters.Add("introduction", this.introduction);
			parameters.Add("afterSales", this.afterSales);
			parameters.Add("jdPrice", this.jdPrice);
			parameters.Add("marketPrice", this.marketPrice);
			parameters.Add("saleAttrs", this.saleAttrs);
			parameters.Add("skuFeatures", this.skuFeatures);
			parameters.Add("outerId", this.outerId);
			parameters.Add("barCode", this.barCode);
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








        
 

