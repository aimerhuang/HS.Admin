using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 添加系列子品图片
	/// </summary>
	public class SerialProductImgUploadRequest 
		: IYhdRequest<SerialProductImgUploadResponse> 
	{
		/**系列产品ID,与outerId二选一(productId优先) */
			public long?  ProductId{ get; set; }

		/**外部产品ID,与productId二选一 */
			public string  OuterId{ get; set; }

		/**选项ID（颜色属性值ID），从yhd.serial.product.attribute.get接口获取。若此项未填，上传的图片将适用于系列产品下的所有子品。若此项有值，则必须是颜色属性值ID，其它属性值ID将提示错误。 */
			public long?  ItemId{ get; set; }

		/**主图名称 */
			public string  MainImageName{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.serial.product.img.upload";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("productId", this.ProductId);
			parameters.Add("outerId", this.OuterId);
			parameters.Add("itemId", this.ItemId);
			parameters.Add("mainImageName", this.MainImageName);
			return parameters;
		}
		#endregion
	}
}
