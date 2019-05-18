using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Extra.Erp.Model.Sale
{
    #region 单据
    /*实体大小写以eas xml数据格式为准*/
    /// <summary>
    /// Eas销售出库
    /// </summary>
    /// <remarks>2013-9-23 杨浩 创建</remarks>
    public class SaleIssueBill
    {
        /// <summary>
        ///单据的抬头
        /// </summary>
        public BillHead billHead { get; set; }
        /// <summary>
        /// 其他出库单据列表
        /// </summary>
        public List<entry> billEntries { get; set; }

        /// <summary>
        /// 代表商城系统的单据编号,或者单据唯一性标识
        /// </summary>
        [XmlAttribute]
        public string thirdSysBillID { get; set; }

        /// <summary>
        /// 代表是否检查重复性 请填true或者false  true代表需要检查单据是否重复导入,false代表允许重复导入.
        /// </summary>
        [XmlAttribute]
        public string checkDuplicate { get; set; }

        /// <summary>
        /// true:提交，false:保存
        /// </summary>
        [XmlAttribute]
        public string isSubmit { get; set; }
    }

    /// <summary>
    /// 单据的抬头
    /// </summary>
    /// <remarks>2013-9-18 杨浩 创建</remarks>
    public class BillHead
    {
        /// <summary>
        /// 创建者
        /// </summary>
        public string creator { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public string createTime { get; set; }
        /// <summary>
        /// 最后修改者
        /// </summary>
        public string lastUpdateUser { get; set; }
        /// <summary>
        /// 最后修改时间
        /// </summary>
        public string lastUpdateTime { get; set; }

        /// <summary>
        /// 控制单元
        /// </summary>
        public string CU
        {
            get { return "00"; }
            set { }
        }

        /// <summary>
        /// 单据编号
        /// </summary>
        public string number { get; set; }

        /// <summary>
        /// 业务日期
        /// </summary>
        public string bizDate { get; set; }

        /// <summary>
        /// 经手人
        /// </summary>
        public string handler { get; set; }
        /// <summary>
        /// 参考信息
        /// </summary>
        public string description { get; set; }
        /// <summary>
        /// 是否曾经生效
        /// </summary>
        public int hasEffected { get; set; }
        /// <summary>
        /// 审核人
        /// </summary>
        public int auditor { get; set; }

        /// <summary>
        /// 原始单据ID
        /// </summary>
        public string sourceBillId
        {
            get { return string.Empty; }
        }

        /// <summary>
        /// 来源功能
        /// </summary>
        public string sourceFunction { get; set; }

        /// <summary>
        /// 审核时间
        /// </summary>
        public string auditTime
        {
            get { return string.Empty; }
            set { }
        }

        /// <summary>
        /// 单据状态 4:审核
        /// </summary>
        public int baseStatus { get; set; }
        /// <summary>
        /// 业务类型
        /// </summary>
        public int bizType { get; set; }
        /// <summary>
        /// 来源单据类型
        /// </summary>
        public int sourceBillType { get; set; }
        /// <summary>
        /// 单据类型
        /// </summary>
        public int billType { get; set; }
        /// <summary>
        /// 业务年度
        /// </summary>
        public int year { get; set; }
        /// <summary>
        /// 业务期间 4位的年+2位的月
        /// </summary>
        public string period { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public int modifier { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public string modificationTime
        {
            get { return string.Empty; }
            set { }
        }
        /// <summary>
        /// 库存组织
        /// </summary>
        public string storageOrgUnit { get; set; }
        /// <summary>
        /// 部门
        /// </summary>
        public int adminOrgUnit { get; set; }
        /// <summary>
        /// 库管员
        /// </summary>
        public int stocker { get; set; }
        /// <summary>
        /// 凭证
        /// </summary>
        public string voucher { get; set; }
        /// <summary>
        /// 总数量
        /// </summary>
        public int totalQty { get; set; }
        /// <summary>
        /// 总金额
        /// </summary>
        public decimal totalAmount { get; set; }
        /// <summary>
        /// 是否生成凭证
        /// </summary>
        public int fiVouchered { get; set; }
        /// <summary>
        /// 总标准成本
        /// </summary>
        public decimal totalStandardCost { get; set; }
        /// <summary>
        /// 总实际成本
        /// </summary>
        public decimal totalActualCost { get; set; }
        /// <summary>
        /// 是否冲销
        /// </summary>
        public int isReversed { get; set; }
        /// <summary>
        /// 事务类型 0001 借出出库 ，借出归还 0002，010 普通销售出库
        /// </summary>
        public string transactionType { get; set; }
        /// <summary>
        /// 是否是初始化单
        /// </summary>
        public int isInitBill { get; set; }

        /*.......................*/

        /// <summary>
        /// 送货客户 必填
        /// </summary>
        public string customer { get; set; }
        /// <summary>
        /// 币别
        /// </summary>
        public string currency { get; set; }
        /// <summary>
        /// 汇率
        /// </summary>
        public double exchangeRate { get; set; }
        /// <summary>
        /// 付款方式
        /// </summary>
        public string paymentType { get; set; }
        /// <summary>
        /// 折算方式 0：直接汇率，1：间接汇率
        /// </summary>
        public int convertMode
        {
            get { return 0; }
            set { }
        }
        /// <summary>
        /// 是否系统单据
        /// </summary>
        public int isSysBill { get; set; }

        /// <summary>
        /// 订单类型
        /// </summary>
        public string dingdanleixing { get; set; }
    }

    /// <summary>
    /// 单据明细
    /// </summary>
    /// <remarks>2013-9-18 杨浩 创建</remarks>
    public class entry : Model.Borrowing.BaseEntry
    {
        /// <summary>
        /// 折扣方式 空=-1,折扣比率=0，单位折扣额=1
        /// </summary>
        public int discountType { get; set; }
        /// <summary>
        /// 折扣额
        /// </summary>
        public decimal discountAmount { get; set; }
        /// <summary>
        /// 单位折扣率
        /// </summary>
        public string discount { get; set; }
        /// <summary>
        /// 已核销数量
        /// </summary>
        public int writtenOffQty { get; set; }
        /// <summary>
        /// 已核销金额
        /// </summary>
        public decimal writtenOffAmount { get; set; }
        /// <summary>
        /// 未核销数量
        /// </summary>
        public int unWriteOffQty { get; set; }
        /// <summary>
        /// 未核销金额
        /// </summary>
        public decimal unWriteOffAmount { get; set; }
        /// <summary>
        /// 客户订单号
        /// </summary>
        public string orderNumber { get; set; }
        /// <summary>
        /// 核心单据号
        /// </summary>
        public string saleOrderNumber { get; set; }
        /// <summary>
        /// 核心单据行行号
        /// </summary>
        public string saleOrderEntrySeq { get; set; }
        /// <summary>
        /// 税率
        /// </summary>
        public double taxRate { get; set; }
        /// <summary>
        /// 税额
        /// </summary>
        public decimal tax { get; set; }
        /// <summary>
        /// 本位币税额
        /// </summary>
        public decimal localTax { get; set; }
        /// <summary>
        /// 本位币单价
        /// </summary>
        public decimal localPrice { get; set; }
        /// <summary>
        /// 本位币金额
        /// </summary>
        public decimal localAmount { get; set; }
        /// <summary>
        /// 不含税金额
        /// </summary>
        public decimal nonTaxAmount { get; set; }
        /// <summary>
        /// 本位币不含税金额
        /// </summary>
        public decimal localNonTaxAmount { get; set; }
        /// <summary>
        /// 已开票数量
        /// </summary>
        public int drewQty { get; set; }
        /// <summary>
        /// 已核销基本数量
        /// </summary>
        public int writtenOffBaseQty { get; set; }
        /// <summary>
        /// 未核销基本数量
        /// </summary>
        public int unWriteOffBaseQty { get; set; }
        /// <summary>
        /// 已开票基本数量
        /// </summary>
        public int drewBaseQty { get; set; }
        /// <summary>
        /// 核心单据ID
        /// </summary>
        public string saleOrder { get; set; }
        /// <summary>
        /// 核心单据行ID
        /// </summary>
        public string saleOrderEntry { get; set; }
        /// <summary>
        /// 核心单据类型
        /// </summary>
        public string coreBillType { get; set; }
        /// <summary>
        /// 可退货基本数量
        /// </summary>
        public int unReturnedBaseQty { get; set; }
        /// <summary>
        /// 是否锁库
        /// </summary>
        public int isLocked { get; set; }
        /// <summary>
        /// 库存台账ID
        /// </summary>
        public string inventoryID { get; set; }
        /// <summary>
        /// 订单单价
        /// </summary>
        public decimal orderPrice { get; set; }
        /// <summary>
        /// 含税单价
        /// </summary>
        public decimal taxPrice { get; set; }
        /// <summary>
        /// 实际单价
        /// </summary>
        public decimal actualPrice { get; set; }
        /// <summary>
        /// 销售组织
        /// </summary>
        public string saleOrgUnit { get; set; }
        /// <summary>
        /// 销售组
        /// </summary>
        public string saleGroup { get; set; }
        /// <summary>
        /// 销售员
        /// </summary>
        public string salePerson { get; set; }
    }
    #endregion

    #region 实体
    /// <summary>
    /// 销售实体
    /// </summary>
    /// <remarks>2013-9-23 杨浩 创建</remarks>
    public class SaleInfoGroup
    {
        /// <summary>
        /// 借货单系统编号
        /// </summary>
        public int ProductLendSysNo { get; set; }
        /// <summary>
        /// 销售出库实体列表
        /// </summary>
        public List<SaleInfo> SaleInfoList = new List<SaleInfo>();
    }

    /// <summary>
    /// 销售出库
    /// </summary>
    /// <remarks>2013-9-23 杨浩 创建</remarks>
    public class SaleInfo
    {
        /// <summary>
        /// Erp商品编码
        /// </summary>
        public string ErpCode { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public int Quantity { get; set; }
        /// <summary>
        /// 是否赠品(1是，0否)
        /// </summary>
        public int IsPresent { get; set; }
        /// <summary>
        /// Erp仓库编号
        /// </summary>
        public string WarehouseNumber { get; set; }
        /// <summary>
        /// 实际金额
        /// </summary>
        public decimal Amount { get; set; }


        /// <summary>
        /// 折扣额(优惠金额)
        /// </summary>
        public decimal DiscountAmount { get; set; }
        /// <summary>
        /// 出库要明细备注(用户账号,快递方式)
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 组织机构代码(销售组织）
        /// </summary>
        public string OrganizationCode { get; set; }

        /// <summary>
        /// 组织机构代码（库存组织) 默认等于销售组织
        /// </summary>
        public string StorageOrganizationCode { get; set; }

        /// <summary>
        /// 仓库编号
        /// </summary>
        public int WarehouseSysNo { get; set; }

        /// <summary>
        /// 销售退货的原出库仓库erp编码
        /// </summary>
        public string SalePerson { get; set; }

        /// <summary>
        /// 应收客户EAS编码(订单创建人所在企业)
        /// IMS 2015-01-29 黄波 添加
        /// </summary>
        public string ReceivableCustomer { get; set; }

        /// <summary>
        /// 收款客户EAS编号(如果订单支付方式未到付,则收款客户是送货方,如果是预付,收款人则是订单创建方)
        /// IMS 2015-01-29 黄波 添加
        /// </summary>
        public string ReceiptCustomer { get; set; }

        /// <summary>
        /// 分录中客户.订货或者送货客户
        /// IMS 2015-01-29 黄波 添加
        /// </summary>
        public string DeliveryCustomer { get; set; }


        /// <summary>
        /// 销售单价
        /// </summary>
        public decimal? SalesUnitPrice { get; set; }

        /// <summary>
        /// 明细编号
        /// </summary>
        public int? ItemID { get; set; }

        /// <summary>
        /// 串码列表
        /// </summary>
        public List<string> Imeis { get; set; }

        /// <summary>
        /// 对象克隆
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            return this.MemberwiseClone();
        } 
    }

    /// <summary>
    /// 实体参数包装
    /// </summary>
    /// <remarks>2013-9-23 杨浩 创建</remarks>
    public class SaleInfoWraper
    {
        /// <summary>
        /// 销售出库实体列表
        /// </summary>
        public List<SaleInfo> Model { get; set; }
        /// <summary>
        /// 出库状态
        /// </summary>
        public 出库状态 Type { get; set; }
        /// <summary>
        /// 单据摘要
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 送货客户(商城客户:3003999997(如果为升舱订单 客服编号就要对应))
        /// </summary>
        public string Customer { get; set; }
        /// <summary>
        /// 时间戳
        /// </summary>
        public string TimeSpan 
        {
            get { return DateTime.Now.ToString("yyyyMMddHHmmssfff"); }
            set { }
        }
    }

    #endregion
}
