using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Hyt.Model.B2CApp
{
    /// <summary>
    /// 调货申请单
    /// </summary>
    /// <remarks>2015-9-14 杨云奕 添加</remarks>
    public class DBApplyOrder
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        public int SysNo { get; set; }
       /// <summary>
       /// 分店编号
       /// </summary>
        public int shopNo { get; set; }
        /// <summary>
        /// 分店仓库编号
        /// </summary>
        public int WareHouseNo { get; set; }
        /// <summary>
        /// 申请人
        /// </summary>
        public int CreateUserNo { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateData { get; set; }
        /// <summary>
        /// 审核人
        /// </summary>
        public int AuditUserNo { get; set; }
        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime AuditTime { get; set; }
        /// <summary>
        /// 状态信息
        /// </summary>
        public string OnLineStatus { get; set; }
        /// <summary>
        /// 状态编码
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 附加内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 最后一次修改人信息
        /// </summary>
        public int UpdateBy { get; set; }
        /// <summary>
        /// 最后一次修改时间
        /// </summary>
        public DateTime UpdateTime { get; set; }

        /// <summary>
        /// 关联申请单列表
        /// </summary>
        public List<DBApplyOrderItem> ApplyItemList = new List<DBApplyOrderItem>();

        /// <summary>
        /// 关联发货单列表
        /// </summary>
        public List<DBShipmentOrder> ShipmentItemList = new List<DBShipmentOrder>();

        /// <summary>
        /// 关联日志信息列表
        /// </summary>
        public List<DBOrderLog> OrderLogList = new List<DBOrderLog>();

    }
    /// <summary>
    /// 调货申请单明细
    /// </summary>
    /// <remarks>2015-9-14 杨云奕 添加</remarks>
    public class DBApplyOrderItem {
        /// <summary>
        /// 系统自动生成编号
        /// </summary>
        public int SysNo { get; set; }
        /// <summary>
        /// 调拨申请但编号
        /// </summary>
        public int DbApplyOrderNo { get; set; }
        /// <summary>
        /// 货品编号
        /// </summary>
        public int ProductSysNo { get; set; }
        /// <summary>
        /// 申请调拨货品数量
        /// </summary>
        public int DbProductNum { get; set; }
        /// <summary>
        /// 货品名称
        /// </summary>
        public string DBProductName { get; set; }
    }

    /// <summary>
    /// 成本变动记录表(财务)
    /// </summary>
    /// <remarks>2015-9-14 杨云奕 添加</remarks>
    public class DBCastOrder
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        public int SysNo { get; set; }
        /// <summary>
        /// 调拨申请单
        /// </summary>
        public int DBApplyOrderNo { get; set; }
        /// <summary>
        /// 财务审核人
        /// </summary>
        public int DBFinanceBy { get; set; }
        /// <summary>
        /// 财务审核时间
        /// </summary>
        public DateTime DBFinanceDate { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public string DBFinanceStatus { get; set; }
        /// <summary>
        /// 附加内容
        /// </summary>
        public string DBFinanceContent { get; set; }

        /// <summary>
        /// 成本变动记录明细列表
        /// </summary>
        public List<DBCastOrderItem> CaseItemList = new List<DBCastOrderItem>();
    }
    
    /// <summary>
    /// 成本变动记录明细(财务)
    /// </summary>
    /// <remarks>2015-9-14 杨云奕 添加</remarks>
    public class DBCastOrderItem
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        public int SysNo { get; set; }
        /// <summary>
        /// 成本变动明细表编号
        /// </summary>
        public int DBCastOrderNo { get; set; }
        /// <summary>
        /// 变动分店
        /// </summary>
        public int DBShopNo { get; set; }
        /// <summary>
        /// 变动分店的仓库
        /// </summary>
        public int DBWareHouseNo { get; set; }
        /// <summary>
        /// 变动前成本
        /// </summary>
        public double DBOldCost { get; set; }
        /// <summary>
        /// 变动后成本
        /// </summary>
        public double DBNewCost { get; set; }
        /// <summary>
        /// 盈利情况
        /// </summary>
        public double DBProfitLoss { get; set; }

        /// <summary>
        /// 货品编号
        /// </summary>
        public int DBGoodsNo { get; set; }
    }

    /// <summary>
    /// 调拨订单操作日志
    /// </summary>
    public class DBOrderLog
    {
        /// <summary>
        /// 系统自动编号
        /// </summary>
        public int SysNo { get; set; }
        /// <summary>
        /// 调拨单编号
        /// </summary>
        public int ApplyOrderNo { get; set; }
        /// <summary>
        /// 类型调拨申请/发货申请
        /// </summary>
        public string OrderStatus { get; set; }
        /// <summary>
        /// 描述内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 操作者编号
        /// </summary>
        public int SysUserNo { get; set; }
        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime SysUserDate { get; set; }
    }

    /// <summary>
    /// 出货申请单
    /// </summary>
    public class DBShipmentOrder {
        /// <summary>
        ///系统自动编号
        /// </summary>
        public int SysNo { get; set; }
        /// <summary>
        /// 调货申请单
        /// </summary>
        public int DBApplyOrderNo { get; set; }
        /// <summary>
        /// 门店编号
        /// </summary>
        public int DBShopNo { get; set; }
        /// <summary>
        /// 库房编号
        /// </summary>
        public int DBWareHouseNo { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public int DBCreateDate { get; set; }
        /// <summary>
        /// 创建人员
        /// </summary>
        public int DBCreateBy { get; set; }
        /// <summary>
        /// 附加内容
        /// </summary>
        public int DBContent { get; set; }
        /// <summary>
        /// 物流费用
        /// </summary>
        public int DBWareHouseCost { get; set; }
        /// <summary>
        /// 审核人员
        /// </summary>
        public int DBStatusBy { get; set; }
        /// <summary>
        /// 审核时间
        /// </summary>
        public int DBStatusDate { get; set; }
        /// <summary>
        /// 修改人员
        /// </summary>
        public int DBUPdateBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public int DBUpDateDate { get; set; }
        /// <summary>
        /// 状态值
        /// </summary>
        public int DBStatus { get; set; }
        /// <summary>
        /// 状态内容
        /// </summary>
        public int OnLineStatus { get; set; }
        /// <summary>
        /// 配送形式
        /// </summary>
        public int DBLogisticMode { get; set; }

        /// <summary>
        /// 关联日志信息列表
        /// </summary>
        public List<DBOrderLog> OrderLogList = new List<DBOrderLog>();
    }

    /// <summary>
    /// 出货申请单明细
    /// </summary>
    public class DBShipmentOrderItem {
        /// <summary>
        /// 系统自动编号
        /// </summary>
        public int SysNo { get; set; }
        /// <summary>
        /// 出货申请单编号
        /// </summary>
        public int ShipmentOrderNo { get; set; }
        /// <summary>
        /// 货号
        /// </summary>
        public int GoodsNo { get; set; }
        /// <summary>
        /// 货物名称
        /// </summary>
        public string GoodsName { get; set; }
        /// <summary>
        /// 成本金额
        /// </summary>
        public double GoodsCost { get; set; }
        /// <summary>
        /// 出货数量
        /// </summary>
        public int GoodsNum { get; set; }
        /// <summary>
        /// 建议售价
        /// </summary>
        public double GoodsSalePrice { get; set; }

    }


    /// <summary>
    /// 计算公式字典
    /// </summary>
    /// <remarks>2015-9-15 杨云奕 添加</remarks>
    public class DBCalculateDictionary
    {

        /// <summary>
        /// 系统自动编号
        /// </summary>		

        public int SysNo
        {
            get;
            set;
        }
        /// <summary>
        /// 字典名称
        /// </summary>		
        
        public string DBDicName
        {
            get;
            set;
        }
        /// <summary>
        /// 替换数据键
        /// </summary>		

        public string DBDicKey
        {
            get;
            set;
        }

    }

    /// <summary>
    /// 货物调动成本波动变化计算公式表
    /// </summary>
    /// <remarks>2015-9-15 杨云奕 添加</remarks>
    public class DBCostCalculate
    {

        /// <summary>
        /// 项目自动编号
        /// </summary>		

        public int SysNo
        {
            get;
            set;
        }
        /// <summary>
        /// 公式描述名称
        /// </summary>		

        public string DBCostName
        {
            get;
            set;
        }
        /// <summary>
        /// 关联编号
        /// </summary>		

        public int DBLinkNo
        {
            get;
            set;
        }
        /// <summary>
        /// 关联类型
        /// </summary>		

        public int DBLinkTypeNo
        {
            get;
            set;
        }
        /// <summary>
        /// 关联类型名称
        /// </summary>		

        public string DBLinkTypeName
        {
            get;
            set;
        }
        /// <summary>
        /// 是否开关
        /// </summary>		

        public int DBBSwitch
        {
            get;
            set;
        }
        /// <summary>
        /// 公式格式
        /// </summary>		

        public string DBCalculateData
        {
            get;
            set;
        }
        /// <summary>
        /// 创建人员
        /// </summary>		

        public int DBCreateBy
        {
            get;
            set;
        }
        /// <summary>
        /// 创建时间
        /// </summary>		

        public DateTime DBCreateDate
        {
            get;
            set;
        }
        /// <summary>
        /// 修改人员
        /// </summary>		

        public int DBUpdateBy
        {
            get;
            set;
        }
        /// <summary>
        /// 修改时间
        /// </summary>		

        public DateTime DBUpDateDate
        {
            get;
            set;
        }

    }

}
