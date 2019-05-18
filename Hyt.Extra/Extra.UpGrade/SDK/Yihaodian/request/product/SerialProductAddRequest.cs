using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 新增一个系列产品
	/// </summary>
	public class SerialProductAddRequest 
		: IYhdRequest<SerialProductAddResponse> 
	{
		/**产品类别ID(从获取授权产品类别yhd.category.products.get接口中获取叶子ID,需要支持系列产品) */
			public long?  CategoryId{ get; set; }

		/**商家产品类别ID列表(逗号分隔,从yhd.category.merchant.products.get接口中获取叶子ID) */
			public string  MerchantCategoryIdList{ get; set; }

		/**产品名称(产品名称开头必须以品牌名或品牌名加英文空格开头,不超过100个字符) */
			public string  ProductCname{ get; set; }

		/**产品名称副标题(不超过100个字符或汉字) */
			public string  ProductSubTitle{ get; set; }

		/**产品名称前缀(不超过10个字符) */
			public string  ProductNamePrefix{ get; set; }

		/**品牌ID(从获取授权品牌yhd.category.brands.get接口中获取) */
			public long?  BrandId{ get; set; }

		/**外部产品编码(不超过30个字符) */
			public string  OuterId{ get; set; }

		/**市场价(最多两位小数) */
			public double?  ProductMarketPrice{ get; set; }

		/**销售价(不能大于市场价,最多两位小数) */
			public double?  ProductSalePrice{ get; set; }

		/**重量(毛重KG,最多两位小数) */
			public double?  Weight{ get; set; }

		/**库存(大于或等于0) */
			public long?  VirtualStockNum{ get; set; }

		/**是否可销(0否;1是) */
			public int?  CanSale{ get; set; }

		/**产品描述(不超过300kb) */
			public string  ProductDescription{ get; set; }

		/**是否虚拟商品(是或否) */
			public string  ElectronicCerticate{ get; set; }

		/**产品属性列表(每个属性之间用逗号分隔,属性ID和属性值之间用冒号分隔。如属性ID:属性值),属性id不能重复 */
			public string  ProdAttributeInfoList{ get; set; }

		/**属性项列表:
1.每个<b>属性值对</b>以逗号分隔（<b>属性值对</b>：由属性id、选项id、子选项id组成）；
2.属性类型为单选且没有子属性，<b>属性值对</b>以‘属性id：选项id’形式组合；
3.属性类型为单选且有子属性，<b>属性值对</b>以‘属性id：选项id_子选项id’形式组合；
4.属性类型为多选，<b>属性值对</b>以‘属性id：选项id|选项id’形式组合。
<font color="red">备注</font>：属性id不能重复,也不能同prodAttributeInfoList中的属性ID相同,同一个属性ID不能有相同的选项ID */
			public string  ProdAttributeItemInfoList{ get; set; }

		/**系列子品信息列表(逗号分隔,最多150个子品),<a target="_blank" style="color:red" href="http://open.yhd.com/doc2/apiObjDetail.do?objName=SerialAttributesInfo"> 说明</a> */
			public string  SerialAttributesInfoList{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.serial.product.add";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("categoryId", this.CategoryId);
			parameters.Add("merchantCategoryIdList", this.MerchantCategoryIdList);
			parameters.Add("productCname", this.ProductCname);
			parameters.Add("productSubTitle", this.ProductSubTitle);
			parameters.Add("productNamePrefix", this.ProductNamePrefix);
			parameters.Add("brandId", this.BrandId);
			parameters.Add("outerId", this.OuterId);
			parameters.Add("productMarketPrice", this.ProductMarketPrice);
			parameters.Add("productSalePrice", this.ProductSalePrice);
			parameters.Add("weight", this.Weight);
			parameters.Add("virtualStockNum", this.VirtualStockNum);
			parameters.Add("canSale", this.CanSale);
			parameters.Add("productDescription", this.ProductDescription);
			parameters.Add("electronicCerticate", this.ElectronicCerticate);
			parameters.Add("prodAttributeInfoList", this.ProdAttributeInfoList);
			parameters.Add("prodAttributeItemInfoList", this.ProdAttributeItemInfoList);
			parameters.Add("serialAttributesInfoList", this.SerialAttributesInfoList);
			return parameters;
		}
		#endregion
	}
}
