using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class EclpCloudReceiveSnapshotRequest : IJdRequest<EclpCloudReceiveSnapshotResponse>
{
		                                                                                                                                                                                                                                                                                                                                                                        		public  		string
   ownerNo  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   skuNo  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   erpQty  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   erpWmsQty  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   erpNotlessQty  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   erpNotplusQty  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   isvStockSnapshotInList  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   isvStockSnapshotOutList  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   warehouseNo  { get; set; }
                                                                                                                                                                       
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.eclp.cloud.receiveSnapshot";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("ownerNo", this.ownerNo);
			parameters.Add("skuNo", this.skuNo);
			parameters.Add("erpQty", this.erpQty);
			parameters.Add("erpWmsQty", this.erpWmsQty);
			parameters.Add("erpNotlessQty", this.erpNotlessQty);
			parameters.Add("erpNotplusQty", this.erpNotplusQty);
			parameters.Add("isvStockSnapshotInList", this.isvStockSnapshotInList);
			parameters.Add("isvStockSnapshotOutList", this.isvStockSnapshotOutList);
			parameters.Add("warehouseNo", this.warehouseNo);
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








        
 

