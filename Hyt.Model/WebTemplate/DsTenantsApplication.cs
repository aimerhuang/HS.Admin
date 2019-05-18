using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.WebTemplate
{
    public class CBDsTenantsApplication : DsTenantsApplication
    {
        public string Name { get; set; }
        public string IDCardNo { get; set; }
        public string CountryName { get; set; }
        public string AreaName { get; set; }
    }
    /// <summary>
    /// 杨云奕 2016-1-4 添加
    /// </summary>
    public class DsTenantsApplication
    {

        public int SysNo { get; set; }
        /// <summary>
        /// 经销商名称（店铺名称）
        /// </summary>
        public string ta_ShopName { get; set; }
        /// <summary>
        /// 经销商英文名称（若有英文版画则使用）
        /// </summary>
        public string ta_ShopEName { get; set; }
        /// <summary>
        /// 联系人
        /// </summary>
        public string ta_LinkName { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        public string ta_LinkTele { get; set; }
        /// <summary>
        /// 联系人手机
        /// </summary>
        public string ta_LinkPhone { get; set; }
        /// <summary>
        /// 联系人传真
        /// </summary>
        public string ta_LinkFax { get; set; }
        /// <summary>
        /// 联系人电子邮件
        /// </summary>
        public string ta_LinkEMall { get; set; }
        /// <summary>
        /// 联系人邮编
        /// </summary>
        public string ta_LinkMallCode { get; set; }
        /// <summary>
        /// 联系人国家
        /// </summary>
        public string ta_Country { get; set; }
        /// <summary>
        /// 联系人国家代码
        /// </summary>
        public int ta_AreaSysNo { get; set; }
        /// <summary>
        /// 联系人地址
        /// </summary>
        public string ta_Address { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string ta_DisInfo { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int ta_Status { get; set; }
        /// <summary>
        /// 用户id
        /// </summary>
        public int ta_UserId { get; set; }
        /// <summary>
        /// 后台登陆账号
        /// </summary>
        public string ta_AdUserCode { get; set; }
        /// <summary>
        /// 后台密码
        /// </summary>
        public string ta_AdUserPass { get; set; }

        /// <summary>
        /// 加盟费
        /// </summary>
        public decimal ta_JoiningFee { get; set; }
        /// <summary>
        /// 付款情况
        /// </summary>
        public int ta_JoinFeePayStatus { get; set; }
        /// <summary>
        /// 收款单单据凭证
        /// </summary>
        public int ta_ReceiptSysNo { get; set; }
        /// <summary>
        /// 品牌名称
        /// </summary>
        public string ta_BrandName { get; set; }
        /// <summary>
        /// 商务类型
        /// </summary>
        public string ta_BusinessType { get; set; }
        /// <summary>
        /// 公司名称
        /// </summary>
        public string ta_CompanyName { get; set; }

        /// <summary>
        /// 营业执照
        /// </summary>
        public string ta_BusinessLicense { get; set; }
        /// <summary>
        /// 法人身份证
        /// </summary>
        public string ta_LegalIDCard { get; set; }
        /// <summary>
        /// 公司开户证明
        /// </summary>
        public string ta_OpenAccount { get; set; }
        /// <summary>
        /// 资质证明
        /// </summary>
        public string ta_Credentials { get; set; }
        /// <summary>
        /// 退货地址
        /// </summary>
        public string ta_ReturnAddress { get; set; }
        /// <summary>
        /// 退货联系人
        /// </summary>
        public string ta_ReturnUserName { get; set; }
        /// <summary>
        /// 退货联系电话
        /// </summary>
        public string ta_ReturnUserPhone { get; set; }

        public DateTime ta_CreateDate { get; set; }

        /// <summary>
        /// 身份证背面
        /// </summary>
        public string ta_IDCardBack { get; set; }
        /// <summary>
        /// 组织机构代码
        /// </summary>
        public string ta_OrganCode { get; set; }
        /// <summary>
        /// 税务登记
        /// </summary>
        public string ta_TaxRegist { get; set; }

        /// <summary>
        /// 进货证明
        /// </summary>
        public string ta_InGoods { get; set; }

        /// <summary>
        /// 合格证明
        /// </summary>
        public string ta_Certificate { get; set; }
        /// <summary>
        /// 生产证明
        /// </summary>
        public string ta_Production { get; set; }
        /// <summary>
        /// 商家入驻类型
        /// </summary>
        public int ta_TransType { get; set; }
        /// <summary>
        /// 主营类型
        /// </summary>
        public string ta_MainSellType { get; set; }

        /// <summary>
        /// 身份证编号
        /// </summary>
        public string ta_IDCardNumber { get; set; }
        /// <summary>
        /// 卫生许可证
        /// </summary>
        public string ta_HygieneImage { get; set; }
        /// <summary>
        /// 门店门头
        /// </summary>
        public string ta_StoresDoor { get; set; }
        /// <summary>
        /// 门店平面图
        /// </summary>
        public string ta_StorePlan { get; set; }
    }
}