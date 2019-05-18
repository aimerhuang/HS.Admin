namespace Hyt.Model.WorkflowStatus
{
    /// <summary>
    /// 仓库状态
    /// </summary>
    /// <remarks>2013-06-08 杨浩 创建</remarks>
    public class WarehouseStatus
    {
        /// <summary>
        /// 入库单据类型
        /// 数据表:WhStockIn 字段:SourceType
        /// </summary>
        /// <remarks>2013-06-18 杨浩 创建</remarks>
        public enum 入库单据类型
        {
            出库单 = 10,
            RMA单 = 20,
            借货单 = 30,
            采购单 = 40,
            调拨出库单 = 50
        }

        /// <summary>
        /// 入库物流方式
        /// 数据表:WhStockIn 字段:DeliveryType
        /// </summary>
        /// <remarks>2013-06-18 吴文强 创建</remarks>
        public enum 入库物流方式
        {
            上门取货 = 10,
            快递入库 = 20,
            送至仓库 = 30,
            拒收 = 40,
            还货 = 50,
            作废出库 = 60,
            已调货出库单作废 = 70,
            调拨出库 = 80
        }

        /// <summary>
        /// 入库单状态
        /// 数据表:WhStockIn 字段:Status
        /// </summary>
        /// <remarks>2013-06-18 吴文强 创建</remarks>
        public enum 入库单状态
        {
            待入库 = 10,
            部分入库 = 20,
            已入库 = 50,
            作废 = -10,
        }

        /// <summary>
        /// 是否到付
        /// 数据表:WhStockOut 字段:IsCOD
        /// </summary>
        /// <remarks>2013-06-18 吴文强 创建</remarks>
        public enum 是否到付
        {
            是 = 1,
            否 = 0,
        }
        /// <summary>
        /// 出库单状态
        /// 数据表:WhStockOut 字段:Status
        /// </summary>
        /// <remarks>2013-06-18 吴文强 创建</remarks>
        public enum 出库单状态
        {
            调货中 = 9,
            待出库 = 10,
            待拣货 = 20,
            待打包 = 30,
            待配送 = 40,
            配送中 = 50,
            已签收 = 60,
            拒收 = 70,
            部分退货 = 80,
            全部退货 = 90,
            作废 = -10,
        }

        /// <summary>
        /// 调货单状态
        /// </summary>
        public enum 调货状态
        {
            待处理 = 10,
            已确认 = 20,
            已作废 = -10,
            已打回 = -20,
        }
        /// <summary>
        /// 出库单自提状态
        /// 数据表:WhStockOut 字段:Status
        /// </summary>
        /// <remarks>2013-07-05 吴文强 创建</remarks>
        public enum 出库单自提状态
        {
            待确认 = 10,
            待自提 = 20,
            已自提 = 60,
            作废 = -10,
        }

        /// <summary>
        /// 是否已经打印包裹单
        /// 数据表:WhStockOut 字段:IsPrintedPackageCover
        /// </summary>
        /// <remarks>2013-06-18 吴文强 创建</remarks>
        public enum 是否已经打印包裹单
        {
            是 = 1,
            否 = 0,
        }

        /// <summary>
        /// 是否已经打印拣货单
        /// 数据表:WhStockOut 字段:IsPrintedPickupCover
        /// </summary>
        /// <remarks>2013-06-18 吴文强 创建</remarks>
        public enum 是否已经打印拣货单
        {
            是 = 1,
            否 = 0,
        }

        /// <summary>
        /// 出库单明细状态
        /// 数据表:WhStockOutItem 字段:Status
        /// </summary>
        /// <remarks>2013-06-18 吴文强 创建</remarks>
        public enum 出库单明细状态
        {
            有效 = 1,
            无效 = 0,
        }

        /// <summary>
        /// 仓库状态
        /// 数据表:WhWarehouse 字段:Status
        /// </summary>
        /// <remarks>2013-06-18 吴文强 创建</remarks>
        public enum 仓库状态
        {
            启用 = 1,
            禁用 = 0,
        }

        /// <summary>
        /// 仓库类型
        /// 数据表:WhWarehouse 字段:WarehouseType
        /// </summary>
        /// <remarks>2013-06-18 吴文强 创建</remarks>
        public enum 仓库类型
        {
            仓库 = 10,
            门店 = 20,
            保税 = 30,
            直邮 = 40,
            内部 =50,
            贸易仓 = 60,
            配送仓 = 70,
            动产质押仓 = 80,
            企业代采 = 110,
            仓储寄存 = 120,
            寄存仓 = 90,
        }

        /// <summary>
        /// 借货是否强制放行
        /// 数据表:WhProductLend 字段:IsEnforceAllow
        /// </summary>
        /// <remarks>2013-07-09 吴文强 创建</remarks>
        public enum 借货是否强制放行
        {
            是 = 1,
            否 = 0,
        }

        /// <summary>
        /// 借货单状态
        /// 数据表:WhProductLend 字段:Status
        /// </summary>
        /// <remarks>2013-07-09 吴文强 创建</remarks>
        public enum 借货单状态
        {
            待出库 = 10,
            已出库 = 20,

            /// <summary>
            /// 已补单已还货
            /// </summary>
            已完成 = 30,
            作废 = -10,
        }

        /// <summary>
        /// 是否默认仓库
        /// 数据表:WhWarehouseArea 字段:IsDefault
        /// </summary>
        /// <remarks>2013-08-13 吴文强 创建</remarks>
        public enum 是否默认仓库
        {
            是 = 1,
            否 = 0,
        }

        /// <summary>
        /// 仓库支持的取件方式状态
        /// 数据表:WhWarehousePickupType 字段:Status
        /// </summary>
        /// <remarks>2013-08-13 吴文强 创建</remarks>
        public enum 仓库支持的取件方式状态
        {
            有效 = 1,
            无效 = 0,
        }

        /// <summary>
        /// 是否自营
        /// 数据表:WhWarehouse 字段:IsSelfSupport
        /// </summary>
        /// <remarks>2014-07-22 余勇 创建</remarks>
        public enum 是否自营
        {
            是 = 1,
            否 = 0,
        }
        #region "退换货"
        /// <summary>
        /// 退换货单状态
        /// 数据表: 字段:Status
        /// </summary>
        /// <remarks>2013-06-08 吴文强 创建</remarks>
        public enum 退换货单状态
        {
            待审核 = 10,
            待入库 = 20,
            待退款 = 30,
            已完成 = 50,
            作废 = -10
        }
        #endregion

        /// <summary>
        /// 标注打印类型
        /// </summary>
        /// <remarks>2015-10-29 谭显锋 创建</remarks>
        public enum 标注打印类型
        {
            拣货单 = 10,
            包裹单 = 20,
        }

        /// <summary>
        /// 拣货单打印类型
        /// </summary>
        /// <remarks>2015-11-6 谭显锋 创建</remarks>
        public enum 拣货单打印类型
        {
            四联单 = 10,
            热敏单 = 20,
        }
        /// <summary>
        /// 仓库库位状态
        /// 数据表:WhWarehousePosition 字段:Status
        /// </summary>
        /// <remarks>2016-06-15 王耀发 创建</remarks>
        public enum 仓库库位状态
        {
            启用 = 1,
            禁用 = 0,
        }
        /// <summary>
        /// 仓库出库单来源编号
        /// 数据表:WhInventoryOut 字段:SourceType
        /// </summary>
        /// <remarks>2016-6-23 杨浩 创建</remarks>
        public enum 出库单来源
        {
            采购单 = 10,
            调货单 = 20,
        }
        /// <summary>
        /// 入库单状态
        /// 数据表:WhStockIn 字段:Status
        /// </summary>
        /// <remarks>2013-06-18 王耀发 创建</remarks>
        public enum 采购退货出库单状态
        {
            待出库 = 10,
            部分出库 = 20,
            已出库 = 50,
            作废 = -10,
        }

        /// <summary>
        /// 库存调拨出库单状态
        /// </summary>
        /// <remarks>2016-07-01 陈海裕创建</remarks>
        public enum 库存调拨出库单状态
        {
            待出库 = 10,
            部分出库 = 20,
            已出库 = 50,
            作废 = -10,
        }

        /// <summary>
        /// 库存调拨单状态
        /// </summary>
        /// <remarks>2016-06-29 陈海裕 创建</remarks>
        public enum 库存调拨单状态
        {
            待审核 = 0,
            出库中 = 10,
            入库中 = 20,
            已完成 = 100
        }
    }
}
