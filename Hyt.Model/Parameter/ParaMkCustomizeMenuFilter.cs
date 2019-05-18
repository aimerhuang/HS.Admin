using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Parameter
{
    /// <summary>
    /// 分销商查询
    /// </summary>
    /// <remarks>2014-1-15 沈强 创建</remarks>
    public class ParaMkCustomizeMenuFilter
    {
        /// <summary>
        /// 类型
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 菜单单名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 当前页号
        /// </summary>
        public int CurrentPage { get; set; }
        /// <summary>
        /// 分页大小
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// 分销商
        /// </summary>
        public int DealerSysNo { get; set; }
        /// <summary>
        /// 是否绑定经销商
        /// </summary>
        public bool IsBindDealer { get; set; }
        /// <summary>
        /// 是否绑定所有经销商
        /// </summary>
        public bool IsBindAllDealer { get; set; }
        /// <summary>
        /// 经销商创建人
        /// </summary>
        public int DealerCreatedBy { get; set; }
        /// <summary>
        /// 搜索条件选中的分销商
        /// </summary>
        public int SelectedDealerSysNo { get; set; }
        /// <summary>
        /// 父级ID
        /// </summary>
        public int Pid { get; set; }
    }
}
