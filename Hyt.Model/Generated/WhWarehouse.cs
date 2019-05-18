using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Hyt.Model
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// 2013-08-27 杨浩 T4生成
    /// </remarks>
    [Serializable]
    [DataContract]
    public partial class WhWarehouse
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        [Description("系统编号")]
        [DataMember]
        public int SysNo { get; set; }
        /// <summary>
        /// 仓库ERP编号
        /// </summary>
        [Description("仓库ERP编号")]
        public string ErpCode { get; set; }
        /// <summary>
        /// 售后库位ERP编号
        /// </summary>
        [Description("售后库位ERP编号")]
        public string ErpRmaCode { get; set; }
        /// <summary>
        /// 前台仓库名称
        /// </summary>
        [Description("前台仓库名称")]
        [DataMember]
        public string WarehouseName { get; set; }
        /// <summary>
        /// 后台仓库名称
        /// </summary>
        [Description("后台仓库名称")]
        [DataMember]
        public string BackWarehouseName { get; set; }
        /// <summary>
        /// 地区编号
        /// </summary>
        [Description("地区编号")]
        public int AreaSysNo { get; set; }
        /// <summary>
        /// 街道地址
        /// </summary>
        [Description("街道地址")]
        public string StreetAddress { get; set; }
        /// <summary>
        /// 联系人
        /// </summary>
        [Description("联系人")]
        public string Contact { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        [Description("电话")]
        public string Phone { get; set; }
        /// <summary>
        /// 状态：有效（1）、无效（0）
        /// </summary>
        [Description("状态：有效（1）、无效（0）")]
        public int Status { get; set; }
        /// <summary>
        /// 仓库类型：仓库（10）、门店（20）
        /// </summary>
        [Description("仓库类型：仓库（10）、门店（20）")]
        public int WarehouseType { get; set; }
        /// <summary>
        /// 纬度
        /// </summary>
        [Description("纬度")]
        [DataMember]
        public double Latitude { get; set; }
        /// <summary>
        /// 经度
        /// </summary>
        [Description("经度")]
        [DataMember]
        public double Longitude { get; set; }
        /// <summary>
        /// 图片
        /// </summary>
        [Description("图片")]
        [DataMember]
        public string ImgUrl { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        [Description("创建人")]
        public int CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [Description("创建时间")]
        public DateTime CreatedDate { get; set; }
        /// <summary>
        /// 最后更新人
        /// </summary>
        [Description("最后更新人")]
        public int LastUpdateBy { get; set; }
        /// <summary>
        /// 最后更新时间
        /// </summary>
        [Description("最后更新时间")]
        public DateTime LastUpdateDate { get; set; }
        /// <summary>
        /// 是否自营
        /// </summary>
        [Description("是否自营")]
        public int IsSelfSupport { get; set; }
        /// <summary>
        /// 物流
        /// </summary>
        [Description("物流")]
        public int Logistics { get; set; }
        /// <summary>
        /// 海关
        /// </summary>
        [Description("海关")]
        public int Customs { get; set; }
        /// <summary>
        /// 商检
        /// </summary>
        [Description("商检")]
        public int Inspection { get; set; }
        /// <summary>
        /// 供应链
        /// </summary>
        [Description("供应链")]
        public int Supply { get; set; }
        /// <summary>
        /// 物流对应仓库编码
        /// </summary>
        [Description("物流对应仓库编码")]
        public string LogisWarehouseCode { get; set; }
        /// <summary>
        /// 利嘉对应仓库编码
        /// 罗勤尧添加 2017 05 26
        /// </summary>
        [Description("利嘉对应仓库编码")]
        public string LiJiaStoreCode { get; set; }
    }
}

