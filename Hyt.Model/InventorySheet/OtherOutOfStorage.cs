using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Hyt.Model.InventorySheet
{
    /// <summary>
    /// 其他出入库实体
    /// </summary>
    public class OtherOutOfStorage
    {
        /// 系统编号
        /// </summary>
        [Description("系统编号")]
        public int? SysNo { get; set; }

        /// 编号
        /// </summary>
        [Description("编号")]
        public string Code { get; set; }

        /// 源单类型
        /// </summary>
        [Description("源单类型")]
        public int Type { get; set; }

        /// 供应商系统编号
        /// </summary>
        [Description("供应商系统编号")]
        public int SupplierSysNo { get; set; }

        /// 供应商名称
        /// </summary>
        [Description("供应商名称")]
        public string SupplierName { get; set; }


        /// 部门系统编号
        /// </summary>
        [Description("部门系统编号")]
        public int DepartmentSysNo { get; set; }


        /// 部门名称
        /// </summary>
        [Description("部门名称")]
        public string DepartmentName { get; set; }


        /// 摘要
        /// </summary>
        [Description("摘要")]
        public string Abstract { get; set; }


        /// 创建日期
        /// </summary>
        [Description("创建日期")]
        public DateTime? AddTime { get; set; }


        /// 收货仓库系统编号
        /// </summary>
        [Description("收货仓库系统编号")]
        public int? CollectWarehouseSysNo { get; set; }


        /// 收货仓库名称
        /// </summary>
        [Description("收货仓库名称")]
        public string CollectWarehouseName { get; set; }


        /// 审核用户系统编号
        /// </summary>
        [Description("审核用户系统编号")]
        public int ToexamineSysNo { get; set; }


        /// 审核用户名称
        /// </summary>
        [Description("审核用户名称")]
        public string ToexamineName { get; set; }


        /// 记账用户系统编号
        /// </summary>
        [Description("记账用户系统编号")]
        public int AccountingSysNo { get; set; }


        /// 记账用户名称
        /// </summary>
        [Description("记账用户名称")]
        public string AccountingName { get; set; }


        /// 验收用户系统编号
        /// </summary>
        [Description("验收用户系统编号")]
        public int CheckSysNo { get; set; }


        /// 验收用户名称
        /// </summary>
        [Description("验收用户名称")]
        public string CheckName { get; set; }

        /// 保管用户系统编号
        /// </summary>
        [Description("保管用户系统编号")]
        public int SafekeepingSysNo { get; set; }


        /// 保管用户名称
        /// </summary>
        [Description("保管用户名称")]
        public string SafekeepingName { get; set; }


        /// 业务员系统编号
        /// </summary>
        [Description("业务员系统编号")]
        public int SalesmanSysNo { get; set; }

        /// 业务员名称
        /// </summary>
        [Description("业务员名称")]
        public string SalesmanName { get; set; }


        /// 审核日期
        /// </summary>
        [Description("审核日期")]
        public DateTime? ToexamineTime { get; set; }

        /// 负责人系统编号
        /// </summary>
        [Description("负责人系统编号")]
        public int ResponsibleSysNo { get; set; }


        /// 负责人名称
        /// </summary>
        [Description("负责人名称")]
        public string ResponsibleName { get; set; }

        /// 制单人系统编号
        /// </summary>
        [Description("制单人系统编号")]
        public int SingleSysNo { get; set; }

        /// 制单人名称
        /// </summary>
        [Description("制单人名称")]
        public string SingleName { get; set; }

        /// 状态
        /// </summary>
        [Description("状态")]
        public int? Status { get; set; }
        

        #region 扩展属性
        /// <summary>
        /// 商品详情
        /// </summary>
        public List<OtherOutOfStorageDetailed> ListData { get; set; }
        #endregion
    }

    /// <summary>
    /// 原单类型枚举
    /// </summary>
    public enum OtherOutOfStorageTypeEnum
    { 
        其他入库=1,
        其他出库=2
    }


    /// <summary>
    /// 出入库单状态枚举
    /// </summary>
    public enum OtherOutOfStorageStatusEnum
    {
        审核中 = 1,
        完成 = 2
    }
}
