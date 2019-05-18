using System.ComponentModel;

namespace Hyt.Model.WorkflowStatus
{
    /// <summary>
    /// 销售单状态
    /// </summary>
    /// <remarks>2013-06-08 吴文强 创建</remarks>
    public class OrderStatus
    {
        public enum 商检反馈报文类型
        {
            商品检查,
            商品订单,
            商品审核报文,
        }
        /// <summary>
        /// 支付单报关状态
        /// 数据表:SoOrder 字段:CustomsPayStatus
        /// </summary>
        ///  <remarks>2016-06-19 杨浩 创建</remarks>
        public enum 支付报关状态
        {
            未提交 = 0,
            处理中 = 10,
            失败 = 20,
            成功 = 100,
        }
        /// <summary>
        /// 支付单申报国检状态
        /// 数据表:SoOrder 字段:CustomsPayStatus
        /// </summary>
        public enum 支付申报国检状态
        {
            未提交 = 0,
            处理中 = 10,
            失败 = 20,
            成功 = 100,
        }
        /// <summary>
        /// 海关报关状态
        /// 数据表:SoOrder 字段:CustomsStatus
        /// </summary>
        ///  <remarks>2016-1-2 杨浩 创建</remarks>
        public enum 海关报关状态
        {
            未提交 = 0,
            处理中 = 10,
            失败 = 20,
            作废 = 30,
            成功 = 100,
        }
        /// <summary>
        /// 微信海关报关状态
        /// 数据表:SoOrder 字段:CustomsStatus
        /// </summary>
        ///  <remarks>2016-1-2 杨浩 创建</remarks>
        public enum 海关微信申报状态
        {
            待申报 = 1,
            待修改申报 = 2,
            申报中 = 3,
            申报成功 = 4,
            申报失败 = 5
        }
        /// <summary>
        /// 海关报关状态
        /// 数据表:SoOrder 字段:CustomsStatus、字段:NsStatus
        /// </summary>
        ///  <remarks>2016-1-2 杨浩 创建</remarks>
        public enum 商检状态
        {
            未推送 = 0,
            已推送 = 1,
            已通过 = 2
        }
        /// <summary>
        /// 发票类型
        /// 数据表:SoOrder 字段:InvoiceType
        /// </summary>
        /// <remarks>2013-06-19 吴文强 创建</remarks>
        public enum 发票类型
        {
            普票 = 10,
            专票 = 20,
        }

        /// <summary>
        /// 是否开发票
        /// 数据表:SoOrder 字段:InvoiceSysNo
        /// </summary>
        /// <remarks>2013-06-18 吴文强 创建</remarks>
        public enum 是否开发票
        {
            是 = 1,
            否 = 0,
        }

        /// <summary>
        /// 销售方式
        /// 数据表:SoOrder 字段:SalesType
        /// </summary>
        /// <remarks>2013-06-19 吴文强 创建</remarks>
        public enum 销售方式
        {
            普通订单 = 10,
            团购订单 = 20,
            秒杀订单 = 30,
            调价订单 = 40,
            经销订单 = 50,
            市场部订单 = 60,
            售后订单 = 100,
        }

        ///// <summary>
        ///// 是否增票专用票
        ///// 数据表:SoOrder 字段:IsVAT
        ///// </summary>
        ///// <remarks>2013-06-18 吴文强 创建</remarks>
        //public enum 是否增票专用票
        //{
        //    是 = 1,
        //    否 = 0,
        //}

        /// <summary>
        /// 销售单来源
        /// 数据表:SoOrder 字段:OrderSource
        /// </summary>
        /// <remarks>2013-06-18 吴文强 创建</remarks>
        public enum 销售单来源
        {
            [Description("PC商城")]
            PC网站 = 10,
            [Description("手机商城")]
            手机商城 = 15,
            门店下单 = 20,
            //手机商城 = 30,
            客服下单 = 50,
            业务员下单 = 60,
            业务员补单 = 70,
            分销商升舱 = 80,
            RMA下单 = 100,
            众筹 = 110,
            你他购 = 120,
            预约下单 = 130,
            积分商城下单 = 150,
            三方商城 = 140,
            国内货栈=1000
            //采购退货单=140,
            //调拨单 = 150,
        }

        /// <summary>
        /// 销售单对用户隐藏
        /// 数据表:SoOrder 字段:IsHiddenToCustomer
        /// </summary>
        /// <remarks>2013-06-18 吴文强 创建</remarks>
        public enum 销售单对用户隐藏
        {
            是 = 1,
            否 = 0,
        }

        /// <summary>
        /// 销售单支付状态
        /// 数据表:SOMaster 字段:PayStatus
        /// </summary>
        /// <remarks>2013-06-08 吴文强 创建</remarks>
        public enum 销售单支付状态
        {
            未支付 = 10,
            已支付 = 20,
            支付异常 = 30
        }

        /// <summary>
        /// 销售单作废人类型
        /// 数据表:SoOrder 字段:CancelUserType
        /// </summary>
        /// <remarks>2013-06-18 吴文强 创建</remarks>
        public enum 销售单作废人类型
        {
            前台用户 = 10,
            后台用户 = 20,
        }

        /// <summary>
        /// 销售单回滚类型
        /// 数据表:SoOrder 字段:CancelUserType
        /// </summary>
        /// <remarks>2017-09-9 罗勤尧 创建</remarks>
        public enum 销售单锁定库存释放类型
        {
            前台用户 = 10,
            后台用户 = 20,
        }
        /// <summary>
        /// 配送前是否联系
        /// 数据表:SoOrder 字段:ContactBeforeDelivery
        /// </summary>
        /// <remarks>2013-06-18 吴文强 创建</remarks>
        public enum 配送前是否联系
        {
            是 = 1,
            否 = 0,
        }

        /// <summary>
        /// 销售单状态
        /// 数据表:SOMaster 字段:Status
        /// </summary>
        /// <remarks>2013-06-08 吴文强 创建</remarks>
        public enum 销售单状态
        {
            待审核 = 10,
            待支付 = 20,
            已申请退款 = 21,
            待创建出库单 = 30,
            部分创建出库单 = 40,
            已创建出库单 = 50,
            出库待接收 = 55,
            已完成 = 100,
            作废 = -10
        }

        /// <summary>
        /// 销售单推送状态
        /// 数据表:SOOrder 字段:SendStatus
        /// </summary>
        /// <remarks>2013-06-08 吴文强 创建</remarks>
        public enum 销售单推送状态
        {
            未推送 = 0,
            已推送 = 1
        }
        /// <summary>
        /// 销售单优惠券状态
        /// 数据表:SoCoupon 字段:Status
        /// </summary>
        /// <remarks>2013-09-16 吴文强 创建</remarks>
        public enum 销售单优惠券状态
        {
            初始 = 10,
            已扣回 = 20,
        }

        public enum 跨境物流推送状态
        {
            未推送 = 0,
            已推送 = 1,
            成功=2,
            失败=-1,

        }

        #region 自定义枚举

        #endregion

        public enum 是否对客户显示订单日志
        {
            是 = 1,
            否 = 0
        }

        /// <summary>
        /// 添加推送又一城订单
        /// 数据库：SoAddOrderToU1City 字段：Status
        /// </summary>
        public enum 又一城订单是否已添加订单
        {
            是 = 1,
            否 = 0
        }
    }
}
