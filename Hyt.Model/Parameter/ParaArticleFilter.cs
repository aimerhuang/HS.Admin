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
    public class ParaArticleFilter
    {
        /// <summary>
        /// 索引页
        /// </summary>
        public int? pageIndex { get; set; }

        /// <summary>
        /// 分页大小
        /// </summary>
        public int pageSize { get; set; }
        /// <summary>
        /// 类别系统号集合
        /// </summary>
        public List<int> ids { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int? searchStaus { get; set; } 
        /// <summary>
        /// 文章标题名称
        /// </summary>
        public string searchName { get; set; } 
        /// <summary>
        /// 
        /// </summary>
        public int DealerSysNo { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<int> intDealerSysNoList { get; set; }

        /// <summary>
        /// 分销商编号集合
        /// </summary>
        public List<int> DealerSysNoList { get; set; }
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
    }
}
