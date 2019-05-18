using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Hyt.Model.InventorySheet
{
    /// <summary>
    /// 盘点单
    /// </summary>
    public class WhInventory
    {
        /// 系统编号
        /// </summary>
        [Description("系统编号")]
        public int SysNo { get; set; }


        /// 盘点单编号
        /// </summary>
        [Description("盘点单编号")]
        public string Code { get; set; }

        /// 备注
        /// </summary>
        [Description("备注")]
        public string Remarks { get; set; }

        /// 盘点人
        /// </summary>
        [Description("盘点人")]
        public int CreatedBy { get; set; }

        /// 盘点人名称
        /// </summary>
        [Description("盘点人名称")]
        public string CreatedName { get; set; }


        /// 盘点状态
        /// </summary>
        [Description("盘点状态")]
        public int? Status { get; set; }
       

        /// 盘点条件
        /// </summary>
        [Description("盘点条件")]
        public string InventoryWhere { get; set; }


        /// <summary>
        /// 盘点的仓库系统编码
        /// </summary>
        [Description("盘点的仓库系统编码")]
        public string WhInventorySysNo { get; set; }


        /// <summary>
        /// 盘点的商品类别系统编码
        /// </summary>
        [Description("盘点的商品类别系统编码")]
        public string ProductTypeSysNo { get; set; }


        /// <summary>
        /// 盘点的商品品牌系统编码
        /// </summary>
        [Description("盘点的商品品牌系统编码")]
        public string ProductBrandSysNo { get; set; }


        /// 创建时间
        /// </summary>
        [Description("创建时间")]
        public DateTime AddTime { get; set; }
    }


    public class WhlnventoryEnum {

        /// <summary>
        /// 盘点类型
        /// </summary>
        public enum WhlnventoryType { 
            仓位商品=1,
            商品类别 =2,
            品牌=3

        }



        /// <summary>
        /// 查询条件名称枚举
        /// </summary>
        public enum WhlnventoryName {
            物料代码=1,
            物料名称=2,
            仓位代码=3,
            仓位名称=4,
            条形码=5
        }

        public enum WhlnventoryWhere { 
            and =1,
            or=2
        }


        /// <summary>
        /// 盘点状态
        /// </summary>
        public enum WhlnventoryStatus { 
            未处理=1,
            打印中=2,
            数据录入=3,
            编制报告=4,
            完成=10
        }


        /// <summary>
        /// 盘点报告单状态状态
        /// </summary>
        public enum WhInventoryReporStatus
        {
            盘盈入库 = 5,
            盘亏入库 = 6,
            盘盈审核 = 7,
            盘亏审核 = 8,
            完成 = 9
        }
    }
}
