using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class WareUpdateRequest : IJdRequest<WareUpdateResponse>
{
		                                                                                                                                  
public   		string
   isPayFirst  { get; set; }

                  
                                                                                                                                                            
                                                          
public   		string
   isCanVat  { get; set; }

                  
                                                                                                                                                            
                                                          
public   		string
   isImported  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   isHealthProduct  { get; set; }

                  
                                                                                                                                                            
                                                          
public   		string
   shelfLifeDays  { get; set; }

                  
                                                                                                                                                            
                                                          
public   		string
   isSerialNo  { get; set; }

                  
                                                                                                                                                            
                                                          
public   		string
   isAppliancesCard  { get; set; }

                  
                                                                                                                                                            
                                                          
public   		string
   isSpecialWet  { get; set; }

                  
                                                                                                                                                            
                                                          
public   		string
   wareBigSmallModel  { get; set; }

                  
                                                                                                                                                                                    
                                                          
public   		string
   warePackType  { get; set; }

                  
                                                                                                                                                            
                                                          
public   		string
   isShelfLife  { get; set; }

                  
                                                                                                                                                            
                                                          
public   		string
   wareId  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   shopCategory  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   title  { get; set; }

                  
                                                            
                                                          
public   		string
   upcCode  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   optionType  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   itemNum  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   stockNum  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   wareLocation  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   producter  { get; set; }

                  
                                                            
                                                          
public   		string
   wrap  { get; set; }

                  
                                                            
                                                          
public   		string
   length  { get; set; }

                  
                                                            
                                                          
public   		string
   wide  { get; set; }

                  
                                                            
                                                          
public   		string
   high  { get; set; }

                  
                                                            
                                                          
public   		string
   weight  { get; set; }

                  
                                                            
                                                          
public   		string
   costPrice  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   marketPrice  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   jdPrice  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   notes  { get; set; }

                  
                                                            
                                                          
public   		string
   rate  { get; set; }

                  
                                                            
                                                          
public   		string
   packListing  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   service  { get; set; }

                  
                                                            
                                                          
public   		string
   skuProperties  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   attributes  { get; set; }

                  
                                                            
                                                          
public   		string
   skuPrices  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   skuStocks  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   tradeNo  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   propertyAlias  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   outerId  { get; set; }

                  
                                                            
                                                          
public   		string
   inputPids  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   inputStrs  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   hasCheckCode  { get; set; }

                  
                                                                                                                                                            
                                                          
public   		string
   adContent  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   listTime  { get; set; }

                  
                                                                                                                                    
                                 
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "360buy.ware.update";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("is_pay_first", this.isPayFirst);
			parameters.Add("is_can_vat", this.isCanVat);
			parameters.Add("is_imported", this.isImported);
			parameters.Add("is_health_product", this.isHealthProduct);
			parameters.Add("shelf_life_days", this.shelfLifeDays);
			parameters.Add("is_serial_no", this.isSerialNo);
			parameters.Add("is_appliances_card", this.isAppliancesCard);
			parameters.Add("is_special_wet", this.isSpecialWet);
			parameters.Add("ware_big_small_model", this.wareBigSmallModel);
			parameters.Add("ware_pack_type", this.warePackType);
			parameters.Add("is_shelf_life", this.isShelfLife);
			parameters.Add("ware_id", this.wareId);
			parameters.Add("shop_category", this.shopCategory);
			parameters.Add("title", this.title);
			parameters.Add("upc_code", this.upcCode);
			parameters.Add("option_type", this.optionType);
			parameters.Add("item_num", this.itemNum);
			parameters.Add("stock_num", this.stockNum);
			parameters.Add("ware_location", this.wareLocation);
			parameters.Add("producter", this.producter);
			parameters.Add("wrap", this.wrap);
			parameters.Add("length", this.length);
			parameters.Add("wide", this.wide);
			parameters.Add("high", this.high);
			parameters.Add("weight", this.weight);
			parameters.Add("cost_price", this.costPrice);
			parameters.Add("market_price", this.marketPrice);
			parameters.Add("jd_price", this.jdPrice);
			parameters.Add("notes", this.notes);
			parameters.Add("rate", this.rate);
			parameters.Add("pack_listing", this.packListing);
			parameters.Add("service", this.service);
			parameters.Add("sku_properties", this.skuProperties);
			parameters.Add("attributes", this.attributes);
			parameters.Add("sku_prices", this.skuPrices);
			parameters.Add("sku_stocks", this.skuStocks);
			parameters.Add("trade_no", this.tradeNo);
			parameters.Add("property_alias", this.propertyAlias);
			parameters.Add("outerId", this.outerId);
			parameters.Add("input_pids", this.inputPids);
			parameters.Add("input_strs", this.inputStrs);
			parameters.Add("has_check_code", this.hasCheckCode);
			parameters.Add("ad_content", this.adContent);
			parameters.Add("list_time", this.listTime);
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








        
 

