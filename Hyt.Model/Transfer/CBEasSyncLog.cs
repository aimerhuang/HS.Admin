using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Hyt.Model.Transfer
{
    public class CBEasSyncLog : IEasSyncLog
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        [Description("系统编号")]
        public int SysNo { get; set; }

        /// <summary>
        /// 流程编号
        /// </summary>
        [Description("流程编号")]
        public string FlowIdentify { get; set; }

        /// <summary>
        /// 单据号
        /// </summary>
        [Description("单据号")]
        public string VoucherNo { get; set; }

        /// <summary>
        /// 单据金额
        /// </summary>
        [Description("单据金额")]
        public decimal VoucherAmount { get; set; }

        /// <summary>
        /// 接口名称
        /// </summary>
        [Description("接口名称")]
        public string Name { get; set; }

        /// <summary>
        /// 仓库名
        /// </summary>
        [Description("仓库名")]
        public string WarehouseName { get; set; }

        /// <summary>
        /// 同步消息
        /// </summary>
        [Description("同步消息")]
        public string Message { get; set; }

        /// <summary>
        /// Eas状态代码
        /// </summary>
        [Description("Eas状态代码")]
        public string StatusCode { get; set; }

        /// <summary>
        /// 状态:成功(1),失败(0)
        /// </summary>
        [Description("状态:成功(1),失败(0),作废 = -1,等待同步 = 5")]
        public int Status { get; set; }

        /// <summary>
        /// 同步次数
        /// </summary>
        [Description("同步次数")]
        public int SyncNumber { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Description("创建时间")]
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// 同步时间
        /// </summary>
        [Description("同步时间")]
        public DateTime LastsyncTime { get; set; }

        /// <summary>
        /// 最后更新日期
        /// </summary>
        [Description("最后更新日期")]
        public DateTime LastupdateDate { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Description("备注")]
        public string Remarks { get; set; }
       
        /// <summary>
        /// 接口类型
        /// </summary>
        [Description("接口类型")]
        public int InterfaceType { get; set; }

        /// <summary>
        /// 同步耗时(毫秒)
        /// </summary>
        [Description("同步耗时(毫秒)")]
        public int ElapsedTime { get; set; }
       
        /// <summary>
        /// 同步数据
        /// </summary>
        [Description("同步数据")]
        public string Data { get; set; }

        /// <summary>
        /// 数据Md5
        /// </summary>
        [Description("数据Md5")]
        public string DataMd5 { get; set; }
       
        /// <summary>
        /// 创建人
        /// </summary>
        [Description("创建人")]
        public int CreatedBy { get; set; }

        /// <summary>
        /// 最后更新人
        /// </summary>
        [Description("最后更新人")]
        public int LastupdateBy { get; set; }
       
    }
}
