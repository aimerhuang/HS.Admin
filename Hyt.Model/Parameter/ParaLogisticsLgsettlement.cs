using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Parameter
{
    /// <summary>
    /// 结算单列表高级查询参数名
    /// </summary>
    /// <remarks>2013-07-06 黄伟 创建</remarks>
    public class ParaLogisticsLgsettlement
    {
        private DateTime? _endDate;
        /// <summary>
        /// 结算单系统编号快捷查询
        /// </summary>
        public int? TxtSysNoInput { get; set; }
        /// <summary>
        /// 结算单系统编号
        /// </summary>
        public int? SettleSysNoAdv { get; set; }
        /// <summary>
        /// 出库单系统编号
        /// </summary>
        public int? DoSysNoAdv { get; set; }
        /// <summary>
        /// 仓库系统编号
        /// </summary>
        public int? TxtWareHouse { get; set; }
        /// <summary>
        /// 配送人员系统编号
        /// </summary>
        public int? SelDelManAdv { get; set; }
        /// <summary>
        /// 创建人UserName
        /// </summary>
        public string CreatedByAdv { get; set; }
        /// <summary>
        /// 结算状态系统编号
        /// </summary>
        public int? SelStatusAdv { get; set; }
        /// <summary>
        /// 开始日期
        /// </summary>
        public DateTime? BeginDateAdv { get; set; }
        /// <summary>
        /// 结束日期
        /// </summary>
        public DateTime? EndDateAdv
        {
            get
            {
                //查询日期上限+1
                return _endDate == null ? (DateTime?)null : _endDate.Value.AddDays(1);
            }
            set { _endDate = value; }
        }
        ///// <summary>
        ///// 仓库系统编号
        ///// </summary>
        //public int? WarehouseSysNo { get; set; }
    }
}
