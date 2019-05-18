using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class WareWriteUpOrDownRequest : IJdRequest<WareWriteUpOrDownResponse>
{
		                                                                                                                                                                                                                                                                                                                                                                                              
public   		string
   note  { get; set; }

                  
                                                            
                                                                                                                                                             
public   		Nullable<long>
   wareId  { get; set; }

                  
                                                            
                                                                                           
public   		Nullable<int>
   opType  { get; set; }

                  
                                                            

		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.ware.write.upOrDown";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("note", this.note);
			parameters.Add("wareId", this.wareId);
			parameters.Add("opType", this.opType);
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








        
 

