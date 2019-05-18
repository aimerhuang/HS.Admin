using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 商城产品团购新增接口
	/// </summary>
	public class GroupRealProductAddRequest 
		: IYhdRequest<GroupRealProductAddResponse> 
	{
		/**团购名称 */
			public string  GroupName{ get; set; }

		/**团购短标题 */
			public string  ShortName{ get; set; }

		/**团购开始时间 */
			public string  StartTime{ get; set; }

		/**团购结束时间 */
			public string  EndTime{ get; set; }

		/**团购预告时间 */
			public string  PreviewTime{ get; set; }

		/**是否生活团，0：否 1：是 */
			public int?  VirtualType{ get; set; }

		/**团购分类ID */
			public long?  GroupCategoryId{ get; set; }

		/**产品ID */
			public long?  ProductId{ get; set; }

		/**团购价格 */
			public double?  GroupPrice{ get; set; }

		/**团购上限 */
			public int?  MaxStockNum{ get; set; }

		/**每人购买下限 */
			public int?  MinGroupNum{ get; set; }

		/**每人购买上限 */
			public int?  MaxGroupNum{ get; set; }

		/**imageUrl：主图（详情页图片），自营可不填（540×360） */
			public string  ImageUrl{ get; set; }

		/**是否不限量，0：限量 1：不限量，默认为0 */
			public int?  UnlimitedFlag{ get; set; }

		/**系列子品信息（子品ID:销售上限;子品ID:销售上限） */
			public string  SubProductList{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.group.real.product.add";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("groupName", this.GroupName);
			parameters.Add("shortName", this.ShortName);
			parameters.Add("startTime", this.StartTime);
			parameters.Add("endTime", this.EndTime);
			parameters.Add("previewTime", this.PreviewTime);
			parameters.Add("virtualType", this.VirtualType);
			parameters.Add("groupCategoryId", this.GroupCategoryId);
			parameters.Add("productId", this.ProductId);
			parameters.Add("groupPrice", this.GroupPrice);
			parameters.Add("maxStockNum", this.MaxStockNum);
			parameters.Add("minGroupNum", this.MinGroupNum);
			parameters.Add("maxGroupNum", this.MaxGroupNum);
			parameters.Add("imageUrl", this.ImageUrl);
			parameters.Add("unlimitedFlag", this.UnlimitedFlag);
			parameters.Add("subProductList", this.SubProductList);
			return parameters;
		}
		#endregion
	}
}
