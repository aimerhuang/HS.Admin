using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 删除系列子品图片
	/// </summary>
	public class SerialProdimgDelRequest 
		: IYhdRequest<SerialProdimgDelResponse> 
	{
		/**1号店系列产品ID,与outerId二选一(productId优先) */
			public long?  ProductId{ get; set; }

		/**系列产品外部ID,与productId二选一 */
			public string  OuterId{ get; set; }

		/**选项ID（颜色属性值ID），从获取产品系列属性接口获取 */
			public long?  ItemId{ get; set; }

		/**要删除的图片id列表（不同图片id用逗号分隔），pids为空，表示删除此产品下所有图片 */
			public string  Pids{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.serial.prodimg.del";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("productId", this.ProductId);
			parameters.Add("outerId", this.OuterId);
			parameters.Add("itemId", this.ItemId);
			parameters.Add("pids", this.Pids);
			return parameters;
		}
		#endregion
	}
}
