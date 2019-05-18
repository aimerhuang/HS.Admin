using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 商城产品团购更新接口
	/// </summary>
	public class GroupRealProductUpdateRequest 
		: IYhdRequest<GroupRealProductUpdateResponse> 
	{
		/**团购id */
			public long?  GroupId{ get; set; }

		/**团购名称 */
			public string  GroupName{ get; set; }

		/**团购短标题 */
			public string  ShortName{ get; set; }

		/**是否生活团，0：否 1：是 */
			public int?  VirtualType{ get; set; }

		/**团购分类ID */
			public long?  GroupCategoryId{ get; set; }

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
			return "yhd.group.real.product.update";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("groupId", this.GroupId);
			parameters.Add("groupName", this.GroupName);
			parameters.Add("shortName", this.ShortName);
			parameters.Add("virtualType", this.VirtualType);
			parameters.Add("groupCategoryId", this.GroupCategoryId);
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
