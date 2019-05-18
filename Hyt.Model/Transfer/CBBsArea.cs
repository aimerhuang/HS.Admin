using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Hyt.Model.WorkflowStatus;

namespace Hyt.Model.Transfer
{
    /// <summary>
    /// 地区数据
    /// </summary>
    /// <remarks> 2013-07-10 周唐炬 创建</remarks>
    [DataContract]
    public class CBBsArea
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        /// <remarks></remarks>
        [DataMember]
        public int SysNo { get; set; }

        /// <summary>
        /// 父级地区编号
        /// </summary>
        /// <remarks></remarks>
        [DataMember]
        public int ParentSysNo { get; set; }

        /// <summary>
        /// 地区名称
        /// </summary>
        /// <remarks></remarks>
        [DataMember]
        public string AreaName { get; set; }

        /// <summary>
        /// 地区编码
        /// </summary>
        /// <remarks></remarks>
        [DataMember]
        public string AreaCode { get; set; }

        /// <summary>
        /// 地区级别
        /// </summary>
        /// <remarks></remarks>
        [DataMember]
        public int AreaLevel { get; set; }
    }

    /// <summary>
    /// 地区信息分页查询实体
    /// </summary>
    /// <remarks>
    /// 2013-08-02 郑荣华 创建
    /// </remarks>
    public class CBBsArea2 : BsArea
    {
        /// <summary>
        /// 地区全称
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// 地区等级名
        /// </summary>
        public string AreaLevelName { get; set; }

        /// <summary>
        /// 父级地区名称
        /// </summary>
        public string ParentName { get; set; }

        /// <summary>
        /// 该地区下的所有仓库名
        /// </summary>
        public string WarehouseNames { get; set; }

        /// <summary>
        /// 为该地区默认配货的仓库名
        /// </summary>
        public string DefaultWarehouseName { get; set; }

        /// <summary>
        /// 是否为该地区的默认仓库 1:是 0:否
        /// </summary>
        public int IsDefault { get; set; }
    }

    /// <summary>
    /// 地区信息详细信息查询
    /// </summary>
    /// <remarks>
    /// 2013-08-22 唐永勤 创建
    /// </remarks>
    public class CBBsAreaDetail : BsArea
    {
        /// <summary>
        /// 省名称
        /// </summary>
        /// <remarks>
        /// 2013-08-12 唐永勤 创建
        /// </remarks>
        public string Province { get; set; }

        /// <summary>
        /// 省编号
        /// </summary>
        /// <remarks>
        /// 2013-08-12 唐永勤 创建
        /// </remarks>
        public int ProvinceSysno { get; set; }

        /// <summary>
        /// 市名称
        /// </summary>
        /// <remarks>
        /// 2013-08-12 唐永勤 创建
        /// </remarks>
        public string City { get; set; }

        /// <summary>
        /// 市编号
        /// </summary>
        /// <remarks>
        /// 2013-08-12 唐永勤 创建
        /// </remarks>
        public int CitySysno { get; set; }

        /// <summary>
        /// 区域名称
        /// </summary>
        /// <remarks>
        /// 2013-08-12 唐永勤 创建
        /// </remarks>
        public string Region { get; set; }

        /// <summary>
        /// 区域编号
        /// </summary>
        /// <remarks>
        /// 2013-08-12 唐永勤 创建
        /// </remarks>
        public int RegionSysno { get; set; }
    }

    /// <summary>
    /// 地区仓库
    /// </summary>
    /// <remarks>
    /// 2013/12/1 何方 创建
    /// </remarks>
    public class CBAreaWarehouse
    {
        /// <summary>
        /// The area sys no
        /// </summary>
        /// <remarks>2013/12/1 何方 创建</remarks>
        public int AreaSysNo { get;set;}

        /// <summary>
        /// The area status
        /// </summary>
        /// <remarks>2013/12/1 何方 创建</remarks>
        public int Status { get; set; }
        /// <summary>
        /// The area sys name
        /// </summary>
        /// <remarks>2013/12/1 何方 创建</remarks>
        public string AreaName { get; set; }
        /// <summary>
        /// The default warehouse sys no
        /// </summary>
        /// <remarks>2013/12/1 何方 创建</remarks>
        public int DefaultWarehouseSysNo { get; set; }
        /// <summary>
        /// all The warehouse sys no,like 1,2,3
        /// </summary>
        /// <remarks>2013/12/1 何方 创建</remarks>
        public string AllWarehouseSysNo { get; set; }

        /// <summary>
        /// 用于查询分页
        /// </summary>
        /// <remarks>2013/12/1 何方 创建</remarks>
        public string FLUENTDATA_ROWNUMBER { get; set; }
    }
}
