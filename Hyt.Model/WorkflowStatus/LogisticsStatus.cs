using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Hyt.Model.WorkflowStatus
{
    /// <summary>
    /// 物流状态
    /// </summary>
    /// <remarks>2013-06-08 吴文强 创建</remarks>
    public class LogisticsStatus
    {

        /// <summary>
        /// 配送方式前台是否可见
        /// 数据表:LgDeliveryType 字段:IsOnlineVisible
        /// </summary>
        /// <remarks>2013-06-18 吴文强 创建</remarks>
        public enum 配送方式前台是否可见
        {
            可见 = 1,
            不可见 = 0,
        }

        /// <summary>
        /// 配送方式状态
        /// 数据表:LgDeliveryType 字段:Status
        /// </summary>
        /// <remarks>2013-06-18 吴文强 创建</remarks> 
        public enum 配送方式状态
        {
            启用 = 1,
            禁用 = 0,
        }
        /// <summary>
        /// 配送方式是否第三方快递
        /// 数据表:LgDeliveryType 字段:IsThirdPartyExpress
        /// </summary>
        /// <remarks>2013-11-20 郑荣华 创建</remarks>
        public enum 是否第三方快递
        {
            是 = 1,
            否 = 0,
        }
        /// <summary>
        /// 配送方式配送响应优先级别(0-5级,级别越高,处理优先级越高)
        /// 数据表:LgDeliveryType 字段:DeliveryLevel
        /// </summary>
        /// <remarks>2013-11-20 郑荣华 创建</remarks>
        public enum 配送响应优先级别
        {
            零级 = 0,
            一级 = 1,
            二级 = 2,
            三级 = 3,
            四级 = 4,
            五级 = 5,
        }
        /// <summary>
        /// 取件单状态
        /// 数据表:LgPickUp 字段:Status
        /// </summary>
        /// <remarks>2013-06-18 吴文强 创建</remarks>
        public enum 取件单状态
        {
            待取件 = 10,
            取件中 = 15,
            已取件 = 20,
            已入库 = 30,
            作废 = -10,
        }

        /// <summary>
        /// 结算单状态
        /// 数据表:LgSettlement 字段:Status
        /// </summary>
        /// <remarks>2013-06-18 吴文强 创建</remarks>
        public enum 结算单状态
        {
            待结算 = 10,
            已结算 = 20,
            作废 = -10,
        }

        /// <summary>
        /// 结算单明细状态
        /// 数据表:LgSettlementItem 字段:Status
        /// </summary>
        /// <remarks>2013-08-20 吴文强 创建</remarks>
        public enum 结算单明细状态
        {
            待结算 = 10,
            已结算 = 20,
            作废 = -10,
        }

        /// <summary>
        /// 配送单状态
        /// 数据表:LgDelivery 字段:Status
        /// </summary>
        /// <remarks>2013-06-18 吴文强 创建</remarks>
        public enum 配送单状态
        {
            待配送 = 10,
            配送在途 = 20,
            已结算 = 30,
            作废 = -10,
        }

        /// <summary>
        /// 是否强制放行
        /// 数据表:LgDelivery 字段:IsEnforceAllow
        /// </summary>
        /// <remarks>2013-06-18 吴文强 创建</remarks>
        public enum 是否强制放行
        {
            是 = 1,
            否 = 0,
        }

        /// <summary>
        /// 配送单据类型
        /// 数据表:LgDeliveryItem 字段:NoteType
        /// </summary>
        /// <remarks>2013-06-18 吴文强 创建</remarks>
        public enum 配送单据类型
        {
            出库单 = 10,
            取件单 = 20
        }

        /// <summary>
        /// 是否到付
        /// 数据表:LgDeliveryItem 字段:IsCOD
        /// </summary>
        /// <remarks>2013-06-18 吴文强 创建</remarks>
        public enum 是否到付
        {
            是 = 1,
            否 = 0,
        }

        /// <summary>
        /// 配送单明细状态
        /// 数据表:LgDeliveryItem 字段:Status
        /// </summary>
        /// <remarks>2013-06-18 吴文强 创建</remarks>
        public enum 配送单明细状态
        {
            /// <summary>
            /// 待签收/待取件
            /// </summary>
            待签收 = 10,
            拒收 = 20,
            未送达 = 30,

            /// <summary>
            /// 已签收/已取件
            /// </summary>
            已签收 = 50,
            作废 = -10,
        }

        /// <summary>
        /// 配送员是否允许借货
        /// 数据表:LgDeliveryUserCredit 字段:IsAllowBorrow
        /// </summary>
        /// <remarks>2013-06-18 吴文强 创建</remarks>
        public enum 配送员是否允许借货
        {
            是 = 1,
            否 = 0,
        }

        /// <summary>
        /// 配送员是否允许配送
        /// 数据表:LgDeliveryUserCredit 字段:IsAllowDelivery
        /// </summary>
        /// <remarks>2013-06-18 吴文强 创建</remarks>
        public enum 配送员是否允许配送
        {
            是 = 1,
            否 = 0,
        }

        /// <summary>
        /// 取件方式状态
        /// 数据表:LgPickupType 字段:Status
        /// </summary>
        /// <remarks>2013-08-13 吴文强 创建</remarks>
        public enum 取件方式状态
        {
            有效 = 1,
            无效 = 0,
        }


        /// <summary>
        /// AppApp签收单据类型
        /// 数据表:LgAppSignStatus 字段:Status
        /// </summary>
        /// <remarks>2014-01-14 吴文强 创建</remarks>
        public enum App签收单据类型
        {
            出库单 = 10,
            取货单 = 20
        }

        /// <summary>
        /// App签收状态
        /// 数据表:LgAppSignStatus 字段:Status
        /// </summary>
        /// <remarks>2014-01-14 吴文强 创建</remarks>
        public enum App签收状态
        {
            拒收 = 20,
            /// <summary>
            /// 未送达/未取件
            /// </summary>
            未送达 = 30,
            部分签收 = 40,
            /// <summary>
            /// 已签收/已取件
            /// </summary>
            已签收 = 50,
        }

        /// <summary>
        /// 运费模板状态
        /// 数据表:LgFreightModule 字段:Status
        /// </summary>
        /// <remarks>2015-08-11 王耀发 创建</remarks>
        public enum 运费模板状态
        {
            待审核 = 10,
            已审核 = 20,
            作废 = -10
        }

        /// <summary>
        /// 电子面单状态
        /// </summary>
        public enum 电子面单状态
        {
            作废 = -10,
            未确认 = 10,
            已确认 = 20,
        }

        public enum 电子面单操作类型
        {
            新增订单 = 10,
            取消订单 = -10,
            确认订单 = 20
        }
    }
}
