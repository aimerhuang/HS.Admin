using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Extra.Erp.Model.Receiving
{
    #region 单据

    /*实体大小写以eas xml数据格式为准*/
    /// <summary>
    /// 收款单据
    /// </summary>
    /// <remarks>2013-9-25 杨浩 创建</remarks>
    public class ReceivingBill
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
    }

    /// <summary>
    /// 单据的抬头
    /// </summary>
    /// <remarks>2013-9-25 杨浩 创建</remarks>
    public class BillHead
    {
        /// <summary>
        /// 创建者
        /// </summary>
        public string creator { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public string createtime { get; set; }
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
        public string bizdate { get; set; }

        /// <summary>
        /// 经手人
        /// </summary>
        public string handler { get; set; }
        /// <summary>
        /// 摘要
        /// </summary>
        public string description { get; set; }
        /// <summary>
        /// 审核人
        /// </summary>
        public string auditor { get; set; }
        /// <summary>
        /// 收款类型
        /// </summary>
        public int recbilltype { get; set; }
        /// <summary>
        /// 收付款方式
        /// </summary>
        public string fundtype { get; set; }
        /// <summary>
        /// 收款类型 销售回款(应收系统)=100,退销售回款=102,其他(出纳系统)=999
        /// </summary>
        public string rectype { get; set; }
        /// <summary>
        /// 付款方式
        /// </summary>
        public string paymenttype { get; set; }
        /// <summary>
        /// 币别
        /// </summary>
        public string currency { get; set; }
        /// <summary>
        /// 实收金额合计
        /// </summary>
        public decimal actrecamt { get; set; }
        /// <summary>
        /// 实收本位币金额合计
        /// </summary>
        public decimal actreclocamt { get; set; }
        /// <summary>
        /// 汇率
        /// </summary>
        public double exchangerate { get; set; }
        /// <summary>
        /// 收款科目
        /// </summary>
        public string payeeaccount { get; set; }
        /// <summary>
        /// 收款账户
        /// </summary>
        public string payeeaccountbank { get; set; }
        /// <summary>
        /// 结算方式
        /// </summary>
        public string settlementtype { get; set; }
        /// <summary>
        /// 业务种类
        /// </summary>
        public string biztype { get; set; }
        /// <summary>
        /// 收款银行
        /// </summary>
        public string payeebank { get; set; }
        /// <summary>
        /// 结算号
        /// </summary>
        public string settlementnumber { get; set; }
        /// <summary>
        /// 部门
        /// </summary>
        public string adminorgunit { get; set; }
        /// <summary>
        /// 人员
        /// </summary>
        public string person { get; set; }
        /// <summary>
        /// 对方科目
        /// </summary>
        public string oppaccount { get; set; }
        /// <summary>
        /// 成本中心
        /// </summary>
        public string costcenter { get; set; }
        /// <summary>
        /// 流入预算项目
        /// </summary>
        public string oppbgitemnumber { get; set; }
        /// <summary>
        /// 往来户类型
        /// </summary>
        public string payertype { get; set; }
        /// <summary>
        /// 往来户编号
        /// </summary>
        public string payernumber { get; set; }
        /// <summary>
        /// 付款银行
        /// </summary>
        public string payerbank { get; set; }
        /// <summary>
        /// 付款账号
        /// </summary>
        public string payeraccountbank { get; set; }
        /// <summary>
        /// 原始单据id
        /// </summary>
        public string sourcebillid { get; set; }
        /// <summary>
        /// 来源功能
        /// </summary>
        public string sourcefunction { get; set; }
        /// <summary>
        /// 审核时间
        /// </summary>
        public string auditdate { get; set; }
        /// <summary>
        /// 单据状态
        /// </summary>
        public string billstatus { get; set; }
        /// <summary>
        /// 来源单据类型
        /// </summary>
        public string sourcebilltype { get; set; }
        /// <summary>
        /// 来源系统
        /// </summary>
        public string sourcetype { get; set; }
        /// <summary>
        /// 业务来源
        /// </summary>
        public string sourcesystype { get; set; }
        /// <summary>
        /// 公司
        /// </summary>
        public string company { get; set; }
        /// <summary>
        /// 最后调汇汇率
        /// </summary>
        public double lastexhangerate { get; set; }
        /// <summary>
        /// 计划项目
        /// </summary>
        public string fpitem { get; set; }
        /// <summary>
        /// 结算类型
        /// </summary>
        public string settlebiztype { get; set; }
        /// <summary>
        /// 出纳
        /// </summary>
        public string cashier { get; set; }
        /// <summary>
        /// 会计
        /// </summary>
        public string accountant { get; set; }

        /// <summary>
        /// 基本状态 保存=10,已提交=11,已审批=12,已收款=14,已付款=15,审批中=6,已审核=8
        /// </summary>
        public int baseStatus { get; set; }
    }

    /// <summary>
    /// 单据明细
    /// </summary>
    /// <remarks>2013-9-25 杨浩 创建</remarks>
    public class entry
    {
        /// <summary>
        /// 单据分录序列号
        /// </summary>
        public int seq { get; set; }
        /// <summary>
        /// 收款类型
        /// </summary>
        public int recbilltype { get; set; }
        /// <summary>
        /// 应收金额
        /// </summary>
        public decimal amount { get; set; }
        /// <summary>
        /// 应收本位币金额
        /// </summary>
        public decimal localamt { get; set; }
        /// <summary>
        /// 现金折扣
        /// </summary>
        public double rebate { get; set; }
        /// <summary>
        /// 现金折扣本位币金额
        /// </summary>
        public decimal rebatelocamt { get; set; }
        /// <summary>
        /// 实收金额
        /// </summary>
        public decimal actualamt { get; set; }
        /// <summary>
        /// 实收本位币金额
        /// </summary>
        public decimal actuallocamt { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string remark { get; set; }
        /// <summary>
        /// 核心单据类型
        /// </summary>
        public int corebilltype { get; set; }
        /// <summary>
        /// 核心单据编码
        /// </summary>
        public string corebillnumber { get; set; }
        /// <summary>
        /// 核心单据分录序列号
        /// </summary>
        public string corebillentryseq { get; set; }
        /// <summary>
        /// 项目号
        /// </summary>
        public string project { get; set; }
        /// <summary>
        /// 跟踪号
        /// </summary>
        public string tracknumber { get; set; }
        /// <summary>
        /// 销售合同号
        /// </summary>
        public string contractnum { get; set; }
        /// <summary>
        /// 销售合同行号
        /// </summary>
        public string contractentryseq { get; set; }
        /// <summary>
        /// 锁定金额
        /// </summary>
        public decimal lockamt { get; set; }
        /// <summary>
        /// 未锁定金额
        /// </summary>
        public decimal unlockamt { get; set; }
        /// <summary>
        /// 已结算金额
        /// </summary>
        public decimal amountvc { get; set; }
        /// <summary>
        /// 已结算金额本位币
        /// </summary>
        public decimal localamtvc { get; set; }
        /// <summary>
        /// 未结算金额
        /// </summary>
        public decimal unvcamount { get; set; }
        /// <summary>
        /// 未结算金额本位币
        /// </summary>
        public decimal unvclocamount { get; set; }
        /// <summary>
        /// 已匹配金额
        /// </summary>
        public decimal matchedamount { get; set; }
        /// <summary>
        /// 已匹配金额本位币
        /// </summary>
        public decimal matchedamountloc { get; set; }
        /// <summary>
        /// 对方科目
        /// </summary>
        public string oppaccount { get; set; }
        /// <summary>
        /// 对方科目名
        /// </summary>
        public string oppbgitemname { get; set; }

        /// <summary>
        /// 仓库编码
        /// </summary>
        public string ck { get; set; }
    }

    #endregion

    #region 实体

    /// <summary>
    /// 收款实体
    /// </summary>
    /// <remarks>2013-9-25 杨浩 创建</remarks>
    public class ReceivingInfo
    {
        /// <summary>
        /// 商城订单号
        /// </summary>
        public string OrderSysNo { get; set; }
        /// <summary>
        ///商城仓库系统号
        /// </summary>
        public int WarehouseSysNo { get; set; }
        /// <summary>
        /// Eas收款仓库编码
        /// </summary>
        public string WarehouseNumber { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 明细备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 收款科目
        /// </summary>
        public string PayeeAccount { get; set; }
        /// <summary>
        /// 收款账户
        /// </summary>
        public string PayeeAccountBank { get; set; }
        /// <summary>
        /// 结算方式
        /// </summary>
        public string SettlementType { get; set; }
        /// <summary>
        /// 组织机构代码
        /// </summary>
        public string OrganizationCode { get; set; }
    }

    /// <summary>
    /// 收款实体包装(Json后记录到日志)
    /// </summary>
    /// <remarks>2013-9-25 杨浩 创建</remarks>
    internal class ReceivingInfoWraper
    {
        /// <summary>
        /// 收款实体列表
        /// </summary>
        public List<ReceivingInfo> Model { get; set; }
        /// <summary>
        /// 单据摘要
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 往来账客户编号(商城客户:3003999997(如果为升舱订单 客服编号就要对应))
        /// </summary>
        public string Customer { get; set; }
        /// <summary>
        /// 收款单类型(5:商品收款单;10:服务收款单)
        /// </summary>
        public 收款单类型 ReceivingType { get; set; }

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
