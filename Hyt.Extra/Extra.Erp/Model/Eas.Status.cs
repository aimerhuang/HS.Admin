using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extra.Erp.Model
{
    /// <summary>
    /// 常量
    /// </summary>
    /// <remarks>2013-9-25 杨浩 创建</remarks>
    public class EasConstant
    {
        /// <summary>
        /// 商城客户
        /// </summary>
        public const string HytCustomer = "3003999997";

        /// <summary>
        /// 仓库：商城
        /// </summary>
        public const string HytWharehouse = "ZK0280884";

        /// <summary>
        /// 收款科目（银行存款_总部银行存款）
        /// </summary>
        public const string PayeeAccount = "1002.01";

        /// <summary>
        /// 支付宝收款账户
        /// </summary>
        public const string PayeeAccountBank_Alipay = "10089";

        /// <summary>
        /// 网银收款账户
        /// </summary>
        public const string PayeeAccountBank_ChinaBank = "10120";

        /// <summary>
        /// 结算方式—现金
        /// </summary>
        public const string SettlementType_Cash = "01";

        /// <summary>
        /// 结算方式—支付宝
        /// </summary>
        public const string SettlementType_Alipay = "014";

        /// <summary>
        /// 结算方式—网银
        /// </summary>
        public const string SettlementType_ChinaBank = "018";

        /// <summary>
        /// 销售单
        /// </summary>
        public const string SO = "";

        /// <summary>
        /// Eas 接口已关闭 
        /// </summary>
        public const string EAS_MESSAGE_CLOSE = "Eas 接口已关闭";

        /// <summary>
        /// 等待同步
        /// </summary>
        public const string EAS_WAIT = "等待同步";

        /// <summary>
        /// 基础资料不全:前缀
        /// </summary>
        public const string Information = "基础资料不全";

    }

    /// <summary>
    /// 借货状态
    /// </summary>
    /// <remarks>2013-9-25 杨浩 创建</remarks>
    public enum 借货状态
    {
        借货 = 5,
        还货 = 10
    }

    /// <summary>
    /// 出库状态
    /// </summary>
    /// <remarks>2013-9-25 杨浩 创建</remarks>
    public enum 出库状态
    {
        出库 = 5,
        退货 = 10
    }

    /// <summary>
    /// EAS收付款类型    
    /// </summary>
    /// <remarks>2013-9-25 杨浩 创建</remarks>
    public enum 收款单类型
    {
        商品收款单 = 5,
        服务收款单 = 15,
        退销售回款=20
    }

    /// <summary>
    /// 结果
    /// </summary>
    /// <remarks>2013-9-25 杨浩 创建</remarks>
    public class Result
    {
        /// <summary>
        /// 是否
        /// </summary>
        public bool Status;

        /// <summary>
        /// 状态代码
        /// </summary>
        public string StatusCode;

        /// <summary>
        /// 消息
        /// </summary>

        public String Message = string.Empty;

        /// 页大小
        /// </summary>
        public int PageSize;
        /// <summary>
        /// 当前页
        /// </summary>
        public int PageIndex;
        /// <summary>
        /// 总记录数
        /// </summary>
        public int RecordCount;
    }


    /// <summary>
    /// Eas同步状态
    /// </summary>
    /// 数据表:EasSyncLog 字段:Status
    public enum 同步状态 : int
    {
        成功 = 1,
        失败 = 0,
        作废 = -1,
        等待同步 = 5
    }

    /// <summary>
    /// 采购状态
    /// </summary>
    /// <remarks>2013-9-25 杨浩 创建</remarks>
    public enum 采购状态
    {
        入库 = 5,
        退货 = 10
    }
    /// <summary>
    /// 调拨状态
    /// </summary>
    /// <remarks>2013-9-25 杨浩 创建</remarks>
    public enum 调拨状态
    {
        入库 = 5,
    }

    /// <summary>
    /// 接口类型
    /// </summary>
    /// <remarks>2013-9-25 杨浩 创建</remarks>
    public enum 接口类型 : byte
    {
        空接口 = 0,
        配送员借货还货 = 5,
        收款单据导入 = 10,
        销售出库退货 = 20,
        利嘉销售出库退货= 30,
        采购入库退货 = 40,
        调拨单据导入 = 50,
        WMS销售出库 = 60,
        WMS退货入库 = 70,
    }

    /// <summary>
    /// 返回结果
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Result<T> : Result
    {
        public T Data { get; set; } 
       
    }


    /// <summary>
    /// 返回结果(Kis版)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class KisResult<T> : KisResult
    {    
        /// <summary>
        /// 兼容Kis
        /// </summary>
        public T data { get; set; }
    }
    /// <summary>
    /// 结果(Kis版)
    /// </summary>
    /// <remarks>2013-9-25 杨浩 创建</remarks>
    public class KisResult
    {      
        /// <summary>
        /// 是否
        /// </summary>
        public bool success;

        /// <summary>
        /// 状态代码
        /// </summary>
        public int error_code;

        /// <summary>
        /// 消息
        /// </summary>
        public string message = string.Empty;
        /// <summary>
        /// 页大小
        /// </summary>
        public int pageSize;
        /// <summary>
        /// 当前页
        /// </summary>
        public int pageIndex;
        /// <summary>
        /// 总记录数
        /// </summary>
        public int recordCount;
    }

}
