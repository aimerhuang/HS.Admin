using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class EclpGoodsTransportGoodsInfoRequest : IJdRequest<EclpGoodsTransportGoodsInfoResponse>
{
		                                                                                                                                  
public   		string
   deptNo  { get; set; }

                  
                                                            
                                                          
public   		string
   isvGoodsNo  { get; set; }

                  
                                                            
                                                          
public   		string
   spGoodsNo  { get; set; }

                  
                                                            
                                                          
public   		string
   barcodes  { get; set; }

                  
                                                            
                                                          
public   		string
   thirdCategoryNo  { get; set; }

                  
                                                            
                                                          
public   		string
   goodsName  { get; set; }

                  
                                                            
                                                          
public   		string
   abbreviation  { get; set; }

                  
                                                            
                                                          
public   		string
   brandNo  { get; set; }

                  
                                                            
                                                          
public   		string
   brandName  { get; set; }

                  
                                                            
                                                          
public   		string
   manufacturer  { get; set; }

                  
                                                            
                                                          
public   		string
   produceAddress  { get; set; }

                  
                                                            
                                                          
public   		string
   standard  { get; set; }

                  
                                                            
                                                          
public   		string
   color  { get; set; }

                  
                                                            
                                                          
public   		string
   size  { get; set; }

                  
                                                            
                                                          
public   		string
   sizeDefinition  { get; set; }

                  
                                                            
                                                          
public   		Nullable<double>
   grossWeight  { get; set; }

                  
                                                            
                                                          
public   		Nullable<double>
   netWeight  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   length  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   width  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   height  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   safeDays  { get; set; }

                  
                                                            
                                                          
public   		string
   instoreThreshold  { get; set; }

                  
                                                            
                                                          
public   		string
   outstoreThreshold  { get; set; }

                  
                                                            
                                                          
public   		string
   serial  { get; set; }

                  
                                                            
                                                          
public   		string
   batch  { get; set; }

                  
                                                            
                                                          
public   		string
   cheapGift  { get; set; }

                  
                                                            
                                                          
public   		string
   quality  { get; set; }

                  
                                                            
                                                          
public   		string
   expensive  { get; set; }

                  
                                                            
                                                          
public   		string
   luxury  { get; set; }

                  
                                                            
                                                          
public   		string
   breakable  { get; set; }

                  
                                                            
                                                          
public   		string
   liquid  { get; set; }

                  
                                                            
                                                          
public   		string
   consumables  { get; set; }

                  
                                                            
                                                          
public   		string
   abnormal  { get; set; }

                  
                                                            
                                                          
public   		string
   imported  { get; set; }

                  
                                                            
                                                          
public   		string
   health  { get; set; }

                  
                                                            
                                                          
public   		string
   temperature  { get; set; }

                  
                                                            
                                                          
public   		string
   temperatureCeil  { get; set; }

                  
                                                            
                                                          
public   		string
   temperatureFloor  { get; set; }

                  
                                                            
                                                          
public   		string
   humidity  { get; set; }

                  
                                                            
                                                          
public   		string
   humidityCeil  { get; set; }

                  
                                                            
                                                          
public   		string
   humidityFloor  { get; set; }

                  
                                                            
                                                          
public   		string
   movable  { get; set; }

                  
                                                            
                                                          
public   		string
   service3g  { get; set; }

                  
                                                            
                                                          
public   		string
   sample  { get; set; }

                  
                                                            
                                                          
public   		string
   odor  { get; set; }

                  
                                                            
                                                          
public   		string
   sex  { get; set; }

                  
                                                            
                                                          
public   		string
   precious  { get; set; }

                  
                                                            
                                                          
public   		string
   mixedBatch  { get; set; }

                  
                                                            
                                                          
public   		string
   reserve1  { get; set; }

                  
                                                            
                                                          
public   		string
   reserve2  { get; set; }

                  
                                                            
                                                          
public   		string
   reserve3  { get; set; }

                  
                                                            
                                                          
public   		string
   reserve4  { get; set; }

                  
                                                            
                                                          
public   		string
   reserve5  { get; set; }

                  
                                                            
                                                          
public   		string
   reserve6  { get; set; }

                  
                                                            
                                                          
public   		string
   reserve7  { get; set; }

                  
                                                            
                                                          
public   		string
   reserve8  { get; set; }

                  
                                                            
                                                          
public   		string
   reserve9  { get; set; }

                  
                                                            
                                                          
public   		string
   reserve10  { get; set; }

                  
                                                            
                                                          
public   		string
   fashionNo  { get; set; }

                  
                                                            
                                                          
public   		string
   goodsUnit  { get; set; }

                  
                                                            
                                                          
public   		string
   customMade  { get; set; }

                  
                                                            
                                                                  
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.eclp.goods.transportGoodsInfo";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("deptNo", this.deptNo);
			parameters.Add("isvGoodsNo", this.isvGoodsNo);
			parameters.Add("spGoodsNo", this.spGoodsNo);
			parameters.Add("barcodes", this.barcodes);
			parameters.Add("thirdCategoryNo", this.thirdCategoryNo);
			parameters.Add("goodsName", this.goodsName);
			parameters.Add("abbreviation", this.abbreviation);
			parameters.Add("brandNo", this.brandNo);
			parameters.Add("brandName", this.brandName);
			parameters.Add("manufacturer", this.manufacturer);
			parameters.Add("produceAddress", this.produceAddress);
			parameters.Add("standard", this.standard);
			parameters.Add("color", this.color);
			parameters.Add("size", this.size);
			parameters.Add("sizeDefinition", this.sizeDefinition);
			parameters.Add("grossWeight", this.grossWeight);
			parameters.Add("netWeight", this.netWeight);
			parameters.Add("length", this.length);
			parameters.Add("width", this.width);
			parameters.Add("height", this.height);
			parameters.Add("safeDays", this.safeDays);
			parameters.Add("instoreThreshold", this.instoreThreshold);
			parameters.Add("outstoreThreshold", this.outstoreThreshold);
			parameters.Add("serial", this.serial);
			parameters.Add("batch", this.batch);
			parameters.Add("cheapGift", this.cheapGift);
			parameters.Add("quality", this.quality);
			parameters.Add("expensive", this.expensive);
			parameters.Add("luxury", this.luxury);
			parameters.Add("breakable", this.breakable);
			parameters.Add("liquid", this.liquid);
			parameters.Add("consumables", this.consumables);
			parameters.Add("abnormal", this.abnormal);
			parameters.Add("imported", this.imported);
			parameters.Add("health", this.health);
			parameters.Add("temperature", this.temperature);
			parameters.Add("temperatureCeil", this.temperatureCeil);
			parameters.Add("temperatureFloor", this.temperatureFloor);
			parameters.Add("humidity", this.humidity);
			parameters.Add("humidityCeil", this.humidityCeil);
			parameters.Add("humidityFloor", this.humidityFloor);
			parameters.Add("movable", this.movable);
			parameters.Add("service3g", this.service3g);
			parameters.Add("sample", this.sample);
			parameters.Add("odor", this.odor);
			parameters.Add("sex", this.sex);
			parameters.Add("precious", this.precious);
			parameters.Add("mixedBatch", this.mixedBatch);
			parameters.Add("reserve1", this.reserve1);
			parameters.Add("reserve2", this.reserve2);
			parameters.Add("reserve3", this.reserve3);
			parameters.Add("reserve4", this.reserve4);
			parameters.Add("reserve5", this.reserve5);
			parameters.Add("reserve6", this.reserve6);
			parameters.Add("reserve7", this.reserve7);
			parameters.Add("reserve8", this.reserve8);
			parameters.Add("reserve9", this.reserve9);
			parameters.Add("reserve10", this.reserve10);
			parameters.Add("fashionNo", this.fashionNo);
			parameters.Add("goodsUnit", this.goodsUnit);
			parameters.Add("customMade", this.customMade);
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








        
 

