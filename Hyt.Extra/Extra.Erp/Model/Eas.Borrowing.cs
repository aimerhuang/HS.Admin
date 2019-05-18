using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Extra.Erp.Model.Borrowing
{
    #region 借货单据

    /*实体大小写以eas xml数据格式为准*/

    /// <summary>
    /// Eas借货单据
    /// </summary>
    /// <remarks>2013-9-18 杨浩 创建</remarks>
    public class OtherIssueBill
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
    }

    /// <summary>
    /// 单据
    /// </summary>
    /// <remarks>2013-9-18 杨浩 创建</remarks>
    public class BaseEntry
    {
        /// <summary>
        /// 分录的顺序号 从1开始 123456 以此类推
        /// </summary>
        public int seq { get; set; }
        /// <summary>
        /// 物料
        /// </summary>
        public string material { get; set; }
        /// <summary>
        /// 辅助属性
        /// </summary>
        public string assistProperty { get; set; }

        /// <summary>
        /// 计量单位 默认GG：个
        /// </summary>
        public string unit { get; set; }

        /// <summary>
        /// 基本计量单位 默认GG：个
        /// </summary>
        public string baseUnit { get; set; }

        /// <summary>
        /// 源单据Id
        /// </summary>
        public string sourceBillId { get; set; }

        /// <summary>
        /// 来源单据编号
        /// </summary>
        public string sourceBillNumber
        {
            get { return string.Empty; }
            set { }
        }

        /// <summary>
        /// 来源单据分录的Id
        /// </summary>
        public string sourceBillEntryId
        {
            get { return string.Empty; }
            set { }
        }

        /// <summary>
        /// 来源单据分录序号（不做处理）
        /// </summary>
        public string sourceBillEntrySeq
        {
            get { return string.Empty; }
            set { }
        }

        /// <summary>
        /// 辅助计量单位换算系数[非必填]
        /// </summary>
        public string assCoefficient
        {
            get { return string.Empty; }
            set { }
        }

        /// <summary>
        /// 单据状态 0: 新增，1:暂存状态,2:提交状态,3:删除状态,4:审核状态,5: release状态,6:blocked状态,7:关闭状态
        /// </summary>
        public int baseStatus { get; set; }

        /// <summary>
        /// 未关联数量[非必填]
        /// </summary>
        public int associateQty
        {
            get { return 0; }
        }
        /// <summary>
        /// 来源单据类型
        /// </summary>
        public int sourceBillType { get; set; }
        /// <summary>
        /// 辅助计量单位
        /// </summary>
        public string assistUnit { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string remark { get; set; }
        /// <summary>
        /// 原因代码
        /// </summary>
        public string reasonCode { get; set; }
        /// <summary>
        /// 库存组织
        /// </summary>
        public string storageOrgUnit { get; set; }

        /// <summary>
        /// 财务组织 默认30：品胜品牌
        /// </summary>
        public int companyOrgUnit
        {
            get { return 30; }
            set { }
        }

        /// <summary>
        /// 仓库
        /// </summary>
        public string warehouse { get; set; }
        /// <summary>
        /// 库位
        /// </summary>
        public string location { get; set; }
        /// <summary>
        /// 仓管员
        /// </summary>
        public string stocker { get; set; }
        /// <summary>
        /// 批次
        /// </summary>
        public string lot { get; set; }
        /// <summary>
        /// 数量【正为出 负为退】
        /// </summary>
        public int qty { get; set; }
        /// <summary>
        /// 辅助数量
        /// </summary>
        public int assistQty { get; set; }
        /// <summary>
        /// 基本单位数量
        /// </summary>
        public int baseQty { get; set; }

        /// <summary>
        /// 冲销数量[非必填]
        /// </summary>
        public int reverseQty
        {
            get { return 0; }
            set { }
        }

        /// <summary>
        /// 退货数量[非必填]
        /// </summary>
        public int returnsQty
        {
            get { return 0; }
            set { }
        }

        /// <summary>
        /// 实际含税单价[非必填]
        /// </summary>
        public decimal price { get; set; }

        /// <summary>
        /// 销售单价
        /// </summary>
        public decimal salePrice { get; set; }

        /// <summary>
        /// 金额[非必填]
        /// </summary>
        public decimal amount { get; set; }

        /// <summary>
        /// 单位标准成本[非必填]
        /// </summary>
        public decimal unitStandardCost { get; set; }

        /// <summary>
        /// 标准成本[非必填]
        /// </summary>
        public decimal standardCost { get; set; }
        /// <summary>
        /// 单位实际成本[非必填]
        /// </summary>
        public decimal unitActualCost { get; set; }
        /// <summary>
        /// 实际成本[非必填]
        /// </summary>
        public decimal actualCost { get; set; }
        /// <summary>
        /// 是否赠品
        /// </summary>
        public int isPresent { get; set; }
        /// <summary>
        /// 生产日期
        /// </summary>
        public string mfg
        {
            get { return string.Empty; }
            set { }
        }
        /// <summary>
        /// 到期日期
        /// </summary>
        public string exp
        {
            get { return string.Empty; }
            set { }
        }
        /// <summary>
        /// 冲销基本数量[非必填]
        /// </summary>
        public int reverseBaseQty { get { return 0; } set { } }
        /// <summary>
        /// 退货基本数量[非必填]
        /// </summary>
        public int returnBaseQty { get { return 0; } set { } }
    }

    /// <summary>
    /// 其他出库单据
    /// </summary>
    /// <remarks>2013-9-18 杨浩 创建</remarks>
    public class entry : BaseEntry
    {
        /// <summary>
        /// 供应商
        /// </summary>
        public string supplier { get; set; }
        /// <summary>
        /// 客户
        /// </summary>
        public int customer { get; set; }
        /// <summary>
        /// 库存类型 默认G:普通
        /// </summary>
        public string storeType { get; set; }
        /// <summary>
        /// 库存状态
        /// </summary>
        public int storeStatus { get; set; }
    }

    #endregion

    #region 借货实体

    /// <summary>
    /// 借货实体
    /// </summary>
    public class BorrowInfoGroup
    {
        /// <summary>
        /// 入库单编号
        /// </summary>
        public int StockInSysno { get; set; }
        /// <summary>
        /// 借货单系统编号
        /// </summary>
        public int ProductLendSysNo { get; set; }
        /// <summary>
        /// 借货实体列表
        /// </summary>
        public List<BorrowInfo> BorrowInfoList = new List<BorrowInfo>();
    }

    /// <summary>
    /// 借货实体
    /// </summary>
    public class BorrowInfo
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
        /// 配送员进货价
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 明细备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 商城仓库编号
        /// </summary>
        public int WarehouseSysNo { get; set; }
    }

    /// <summary>
    /// 参数包装，内部使用
    /// </summary>
    internal class BorrowInfoWraper
    {
        public List<BorrowInfo> Model { get; set; }
        public 借货状态 Type { get; set; }
        public string Description { get; set; }

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
