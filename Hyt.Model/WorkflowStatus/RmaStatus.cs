using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.WorkflowStatus
{
    /// <summary>
    /// 售后状态
    /// </summary>
    /// <remarks>2013-07-11 吴文强 创建</remarks>
    public class RmaStatus
    {
        /// <summary>
        /// 退换货申请单来源
        /// 数据表:RcReturn 字段:Source
        /// </summary>
        /// <remarks>2013-07-11 吴文强 创建</remarks>
        public enum 退换货申请单来源
        {
            会员 = 10,
            客服 = 20,
            门店 = 30,
            部分签收 = 40,
            拒收 = 50,
            分销商 = 110
        }

        /// <summary>
        /// 退换货处理部门
        /// 数据表:RcReturn 字段:HandleDepartment
        /// </summary>
        /// <remarks>2013-07-11 吴文强 创建</remarks>
        public enum 退换货处理部门
        {
            客服中心 = 10,
            门店 = 20,
        }

        /// <summary>
        /// RMA类型
        /// 数据表:RcReturn 字段:RmaType
        /// </summary>
        /// <remarks>2013-07-11 吴文强 创建</remarks>
        public enum RMA类型
        {
            售后换货 = 10,
            售后退货 = 20,
            仅退款 = 30,
        }

        /// <summary>
        /// 是否取回发票
        /// 数据表:RcReturn 字段:IsPickUpInvoice
        /// </summary>
        /// <remarks>2013-07-11 吴文强 创建</remarks>
        public enum 是否取回发票
        {
            是 = 1,
            否 = 0,
        }

        /// <summary>
        /// 退换货退款方式
        /// 数据表:RcReturn 字段:RefundType
        /// </summary>
        /// <remarks>2013-07-11 吴文强 创建</remarks>
        public enum 退换货退款方式
        {
            原路返回 = 10,
            至银行卡 = 20,
            门店退款 = 30,
            账户积分 = 40,
            账户余额 = 50,
            分销商预存 = 110,
            所退款作为换货=120,
        }

        /// <summary>
        /// 退换货状态
        /// 数据表:RcReturn 字段:Status
        /// </summary>
        /// <remarks>2013-07-11 吴文强 创建</remarks>
        public enum 退换货状态
        {
            待审核 = 10,
            待入库 = 20,
            待退款 = 30,
            已完成 = 50,
            作废 = -10,
        }

        /// <summary>
        /// 退款状态
        /// 数据表:RcReturn 字段:Status
        /// </summary>
        /// <remarks>2013-07-11 吴文强 创建</remarks>
        public enum 退款状态
        {
            待审核 = 10,
            待退款 = 30,
            已完成 = 50,
            作废 = -10,
        }

        /// <summary>
        /// 商品退换货类型
        /// 数据表:RcReturnItem 字段:ReturnType
        /// </summary>
        /// <remarks>2013-07-11 吴文强 创建</remarks>
        public enum 商品退换货类型
        {
            新品 = 10,
            坏品 = 20,
            二手 = 30,
        }

        /// <summary>
        /// 商品退款价格类型
        /// 数据表:RcReturnItem 字段:ReturnPriceType
        /// </summary>
        /// <remarks>2013-07-11 吴文强 创建</remarks>
        public enum 商品退款价格类型
        {
            原价 = 10,
            自定义价格 = 20,
        }
    }
}
