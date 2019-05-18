using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Hyt.Model;

namespace Hyt.Admin.Models
{
    /// <summary>
    /// 6.退换货单编辑实体
    /// </summary>
    /// <remarks>2013－07-11 黄志勇 创建</remarks>
    public class ReturnEdit : RcReturn
    {
        /// <summary>
        /// 提示消息
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 有强制退换货权限
        /// </summary>
        public bool ReturnAble { get; set; }
        /// <summary>
        /// 有修改合计退款金额权限
        /// </summary>
        public bool ModifyAmountAble { get; set; }
        /// <summary>
        /// 有保存权限
        /// </summary>
        public bool SaveAble { get; set; }
        /// <summary>
        /// 有审核通过权限
        /// </summary>
        public bool AuditAble { get; set; }
        /// <summary>
        /// 有作废权限
        /// </summary>
        public bool CancelAble { get; set; }
        /// <summary>
        /// 退换货出库单列表
        /// </summary>
        public List<ReturnEditOutStore> ReturnEditOutStore { get; set; }

        /// <summary>
        /// 商品入库仓库列表
        /// </summary>
        public List<WhWarehouse> WhWarehouseList { get; set; }

        /// <summary>
        /// 商品入库仓库名称
        /// </summary>
        public string WarehouseName { get; set; }

        /// <summary>
        /// 商品入库仓库名称
        /// </summary>
        public string BackWarehouseName { get; set; }

        /// <summary>
        /// 取件方式
        /// </summary>
        public List<LgPickupType> LgPickupType { get; set; }
        /// <summary>
        /// 取件方式系统编号
        /// </summary>
        public int PickUpShipTypeSysNo { get; set; }
        /// <summary>
        /// 取件地址
        /// </summary>
        public SoReceiveAddress PickUpAddress { get; set; }

        /// <summary>
        /// 会员收货地址
        /// </summary>
        public SoReceiveAddress SoReceiveAddress { get; set; }

        /// <summary>
        /// 订单发票类型 余勇 添加
        /// </summary>
        public int InvoiceType { get; set; }

        /// <summary>
        /// 退换货图片 朱家宏 添加
        /// </summary>
        public List<RcReturnImage> RmaImages { get; set; }

        /// <summary>
        /// 会员编号
        /// </summary>
        public int CustomerSysNo { get; set; }
    }

}