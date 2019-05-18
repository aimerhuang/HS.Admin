using System.Collections.Generic;
namespace Hyt.Model
{
    /// <summary>
    /// 仓库信息搜索条件类
    /// </summary>
    /// <remarks>2013-08-07 周瑜 创建</remarks>
    public class WarehouseSearchCondition
    {
        /// <summary>
        /// 地区系统编号
        /// </summary>
        public int? AreaSysNo { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int? Status { get; set; }
        /// <summary>
        /// 仓库名称
        /// </summary>
        public string WarehouseName { get; set; }
        /// <summary>
        /// 仓库支持配送方式系统编号
        /// </summary>
        public int? DeliveryType { get; set; }
        /// <summary>
        /// 后台仓库名称
        /// </summary>
        public string BackWarehouseName { get; set; }

        /// <summary>
        /// 仓库类型
        /// </summary>
        public int? WarehouseType { get; set; }
        /// <summary>
        /// 是否自营
        /// </summary>
        /// <remarks>2016-05-27 杨浩 创建</remarks>
        public int? IsSelfSupport { get; set; }

        private bool isAllWarehouse = true;
        /// <summary>
        /// 是否所有仓库权限
        /// </summary>
        public bool IsAllWarehouse 
        {
            get 
            { 
                return isAllWarehouse;
            } 
            set
            {
                isAllWarehouse=value;
            }
        }
        /// <summary>
        /// 拥有的仓库
        /// </summary>
        public IList<WhWarehouse> Warehouses { get; set; }

    }
}
