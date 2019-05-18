using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class VcItemUpdateRequest : IJdRequest<VcItemUpdateResponse>
{
		                                                                                                                                                                   
public   		string
   wareId  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   props  { get; set; }

                  
                                                            
                                                          
public   		string
   inputPids  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   inputStr  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   originalPlace  { get; set; }

                  
                                                                                                                                    
                                                          
public   		Nullable<int>
   length  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   width  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   height  { get; set; }

                  
                                                            
                                                          
public   		string
   weight  { get; set; }

                  
                                                            
                                                          
public   		string
   salerCode  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   salerName  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   purchaserCode  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   purchaserName  { get; set; }

                  
                                                                                                                                    
                                                          
public   		Nullable<int>
   brandId  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   zhBrand  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   enBrand  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   warranty  { get; set; }

                  
                                                            
                                                          
public   		string
   pkgInfo  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   webSite  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   tel  { get; set; }

                  
                                                            
                                                          
public   		string
   intro  { get; set; }

                  
                                                            
                                 
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.vc.item.update";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("ware_id", this.wareId);
			parameters.Add("props", this.props);
			parameters.Add("input_pids", this.inputPids);
			parameters.Add("input_str", this.inputStr);
			parameters.Add("original_place", this.originalPlace);
			parameters.Add("length", this.length);
			parameters.Add("width", this.width);
			parameters.Add("height", this.height);
			parameters.Add("weight", this.weight);
			parameters.Add("saler_code", this.salerCode);
			parameters.Add("saler_name", this.salerName);
			parameters.Add("purchaser_code", this.purchaserCode);
			parameters.Add("purchaser_name", this.purchaserName);
			parameters.Add("brand_id", this.brandId);
			parameters.Add("zh_brand", this.zhBrand);
			parameters.Add("en_brand", this.enBrand);
			parameters.Add("warranty", this.warranty);
			parameters.Add("pkg_info", this.pkgInfo);
			parameters.Add("web_site", this.webSite);
			parameters.Add("tel", this.tel);
			parameters.Add("intro", this.intro);
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








        
 

