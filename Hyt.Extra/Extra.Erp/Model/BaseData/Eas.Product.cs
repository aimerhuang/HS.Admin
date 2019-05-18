using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extra.Erp.Model.BaseData
{
    /// <summary>
    /// 商品详情
    /// </summary>
    /// <remarks>2016-9-19 杨云奕 添加</remarks>
    public class EasProduct : EasBaseAccount
    {
        /// <summary>
        /// 编码
        /// </summary>
        public string FNumber { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string FName { get; set; }
        /// <summary>
        /// 全编码
        /// </summary>
        public string FFullNumber { get; set; }
        /// <summary>
        /// 全名称
        /// </summary>
        public string FFullName { get; set; }
        /// <summary>
        /// 层次
        /// </summary>
        public string FLevel { get; set; }
        /// <summary>
        /// 是否明细数据
        /// </summary>
        public string fdetail { get; set; }
        /// <summary>
        /// 规格型号
        /// </summary>
        public string FModel { get;set;}
        /// <summary>
        /// 计量单位
        /// </summary>
        public string FUnitName { get; set; }
        /// <summary>
        /// 默认仓库
        /// </summary>
        public string FDefaultLoc { get; set; }
        /// <summary>
        /// 安全库存数量
        /// </summary>
        public decimal FSecInv { get; set; }
        /// <summary>
        /// 采购单价
        /// </summary>
        public decimal FOrderPrice { get; set; }
        /// <summary>
        /// 销售单价
        /// </summary>
        public decimal FSalePrice { get; set; }
        /// <summary>
        /// 保质期(天)
        /// </summary>
        public int FKFPeriod { get; set; }
        /// <summary>
        /// 毛重
        /// </summary>
        public decimal FGrossWeight { get; set; }
        /// <summary>
        /// 净重
        /// </summary>
        public decimal FNetWeight { get; set; }
        /// <summary>
        /// 长度
        /// </summary>
        public decimal FLength { get; set; }
        /// <summary>
        /// 宽度
        /// </summary>
        public decimal FWidth { get; set; }
        /// <summary>
        /// 高度
        /// </summary>
        public decimal FHeight { get; set; }
        /// <summary>
        /// 体积
        /// </summary>
        public decimal FSize { get; set;  }
        public int isParent { get; set; }
    }
}
