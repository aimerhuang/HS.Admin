using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 新增一个产品(单品)
	/// </summary>
	public class ProductAddRequest 
		: IYhdRequest<ProductAddResponse> 
	{
		/**产品类型(0普通产品;1图书) */
			public int?  ProductType{ get; set; }

		/**产品类别ID(从获取授权产品类别yhd.category.products.get接口中获取叶子ID) */
			public long?  CategoryId{ get; set; }

		/**商家产品类别ID列表(从获取商家产品类别yhd.category.merchant.products.get接口中获取叶子ID) */
			public string  MerchantCategoryId{ get; set; }

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

		/**节能补贴金额(图书产品不支持,不能大于市场价,最多两位小数,50~400) */
			public double?  SubsidyAmount{ get; set; }

		/**重量(毛重KG,最多两位小数) */
			public double?  Weight{ get; set; }

		/**库存(大于或等于0) */
			public long?  VirtualStockNum{ get; set; }

		/**是否可销(0否;1是) */
			public int?  CanSale{ get; set; }

		/**产品描述(不超过300kb) */
			public string  ProductDescription{ get; set; }

		/**电子凭证(是不是虚拟产品，如：游戏点卡、充值卡等) */
			public string  ElectronicCerticate{ get; set; }

		/**产品属性列表(每个属性之间用逗号分隔,属性ID和属性值之间用冒号分隔。如属性ID:属性值),属性id不能重复 */
			public string  ProdAttributeInfoList{ get; set; }

		/**属性项列表:1.每个<b>属性值对</b>以逗号分隔（<b>属性值对</b>：由属性id、选项id、子选项id组成）；2.属性类型为单选且没有子属性，<b>属性值对</b>以‘属性id：选项id’形式组合；3.属性类型为单选且有子属性，<b>属性值对</b>以‘属性id：选项id_子选项id’形式组合；4.属性类型为多选，<b>属性值对</b>以‘属性id：选项id|选项id’形式组合。<font color="red">备注</font>：属性id不能重复,也不能同prodAttributeInfoList中的属性ID相同,同一个属性ID不能有相同的选项ID */
			public string  ProdAttributeItemInfoList{ get; set; }

		/**标题(图书独有属性) */
			public string  BookTitle{ get; set; }

		/**出版社推荐语(图书独有属性) */
			public string  Recommended{ get; set; }

		/**作者简介(图书独有属性) */
			public string  AuthorIntroduction{ get; set; }

		/**目录(图书独有属性) */
			public string  Catalog{ get; set; }

		/**书摘(图书独有属性) */
			public string  Digest{ get; set; }

		/**内容简介(图书独有属性) */
			public string  ContentIntroduction{ get; set; }

		/**媒体报道(图书独有属性) */
			public string  MediaReport{ get; set; }

		/**图片 Urls(艺龙、当当使用) */
			public string  ImgUrls{ get; set; }

		/**产品条形码 */
			public string  Barcode{ get; set; }

		/**区域商家信息，（outerId可有可无，且不要包含英文逗号和英文分号），如果是集团商家导入商品，此项为必填；示例：区域商家id_1:运费模板id_1:库存_1:1号店价_1:商家编码_1;区域商家id_2:运费模板id_2:库存_2:1号店价_2:商家编码_2 */
			public string  AreaMerchantProductList{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.product.add";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("productType", this.ProductType);
			parameters.Add("categoryId", this.CategoryId);
			parameters.Add("merchantCategoryId", this.MerchantCategoryId);
			parameters.Add("productCname", this.ProductCname);
			parameters.Add("productSubTitle", this.ProductSubTitle);
			parameters.Add("productNamePrefix", this.ProductNamePrefix);
			parameters.Add("brandId", this.BrandId);
			parameters.Add("outerId", this.OuterId);
			parameters.Add("productMarketPrice", this.ProductMarketPrice);
			parameters.Add("productSalePrice", this.ProductSalePrice);
			parameters.Add("subsidyAmount", this.SubsidyAmount);
			parameters.Add("weight", this.Weight);
			parameters.Add("virtualStockNum", this.VirtualStockNum);
			parameters.Add("canSale", this.CanSale);
			parameters.Add("productDescription", this.ProductDescription);
			parameters.Add("electronicCerticate", this.ElectronicCerticate);
			parameters.Add("prodAttributeInfoList", this.ProdAttributeInfoList);
			parameters.Add("prodAttributeItemInfoList", this.ProdAttributeItemInfoList);
			parameters.Add("bookTitle", this.BookTitle);
			parameters.Add("recommended", this.Recommended);
			parameters.Add("authorIntroduction", this.AuthorIntroduction);
			parameters.Add("catalog", this.Catalog);
			parameters.Add("digest", this.Digest);
			parameters.Add("contentIntroduction", this.ContentIntroduction);
			parameters.Add("mediaReport", this.MediaReport);
			parameters.Add("imgUrls", this.ImgUrls);
			parameters.Add("barcode", this.Barcode);
			parameters.Add("areaMerchantProductList", this.AreaMerchantProductList);
			return parameters;
		}
		#endregion
	}
}
