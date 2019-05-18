using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 资质审核信息更新
	/// </summary>
	public class QcInfoUpdateRequest 
		: IYhdRequest<QcInfoUpdateResponse> 
	{
		/**商家订单ID */
			public long?  CustomOrderId{ get; set; }

		/**用户名称。用户注册帐号名称 */
			public string  UserName{ get; set; }

		/**认证结果。0：未通过；1：通过 */
			public int?  Certified{ get; set; }

		/**审核通过的申请类目 */
			public string  Categorys{ get; set; }

		/**品牌名称、授权级别列表，多个单位之间逗号分隔，每个单位的品牌和授权级别之间冒号分隔。 */
			public string  Brands{ get; set; }

		/**经营类型。1：专卖店；2：专营店；3：旗舰店； */
			public int?  ShopCategory{ get; set; }

		/**拒绝类型。1:基本资料填写错误、2:资料提供不齐全、3:基本资质不合格、99:其它 */
			public int?  RefuseType{ get; set; }

		/**拒绝原因说明。最大是1024字符。 */
			public string  RefuseDesc{ get; set; }

		/**订单完成时间 */
			public string  Finished{ get; set; }

		/**企业状态 */
			public string  LiceStateTitle{ get; set; }

		/**公司名称 */
			public string  ComName{ get; set; }

		/**公司注册号 */
			public string  RegNum{ get; set; }

		/**企业注册地址 */
			public string  RegAddress{ get; set; }

		/**法定代表人 */
			public string  LegalRep{ get; set; }

		/**注册资本（万元） */
			public string  RegCapital{ get; set; }

		/**企业类型 */
			public string  ComType{ get; set; }

		/**经营范围。最大2048个字符 */
			public string  BusinessScope{ get; set; }

		/**成立日期 */
			public string  FoundDate{ get; set; }

		/**经营期限开始时间 */
			public string  LiceStartDate{ get; set; }

		/**经营期限结束时间 */
			public string  LiceEndDate{ get; set; }

		/**登记机关 */
			public string  IssuingAuthority{ get; set; }

		/**最近年检时间（年份） */
			public string  LastReviewedDate{ get; set; }

		/**企业工商资质是否通过。1：通过；0：未通过 */
			public int?  ComCertified{ get; set; }

		/**工商资质备注 */
			public string  ComRemark{ get; set; }

		/**品牌相关数据列表。具体参数封装为json格式；json格式内部请参考QcBrandInfo对象； */
			public string  BrandList{ get; set; }

		/**授权店长姓名。审核通过（certified=1）时，才可以更新此字段。 */
			public string  Grantors{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.qc.info.update";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("customOrderId", this.CustomOrderId);
			parameters.Add("userName", this.UserName);
			parameters.Add("certified", this.Certified);
			parameters.Add("categorys", this.Categorys);
			parameters.Add("brands", this.Brands);
			parameters.Add("shopCategory", this.ShopCategory);
			parameters.Add("refuseType", this.RefuseType);
			parameters.Add("refuseDesc", this.RefuseDesc);
			parameters.Add("finished", this.Finished);
			parameters.Add("liceStateTitle", this.LiceStateTitle);
			parameters.Add("comName", this.ComName);
			parameters.Add("regNum", this.RegNum);
			parameters.Add("regAddress", this.RegAddress);
			parameters.Add("legalRep", this.LegalRep);
			parameters.Add("regCapital", this.RegCapital);
			parameters.Add("comType", this.ComType);
			parameters.Add("businessScope", this.BusinessScope);
			parameters.Add("foundDate", this.FoundDate);
			parameters.Add("liceStartDate", this.LiceStartDate);
			parameters.Add("liceEndDate", this.LiceEndDate);
			parameters.Add("issuingAuthority", this.IssuingAuthority);
			parameters.Add("lastReviewedDate", this.LastReviewedDate);
			parameters.Add("comCertified", this.ComCertified);
			parameters.Add("comRemark", this.ComRemark);
			parameters.Add("brandList", this.BrandList);
			parameters.Add("grantors", this.Grantors);
			return parameters;
		}
		#endregion
	}
}
