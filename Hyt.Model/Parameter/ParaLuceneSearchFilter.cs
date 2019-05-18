using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Model.WorkflowStatus;

namespace Hyt.Model.Parameter
{
    /// <summary>
    /// Lucene查询参数对象
    /// </summary>
    /// <remarks>2013-09-13 邵斌 创建</remarks>
    public class ParaLuceneSearchFilter
    {
        /// <summary>
        /// 搜索关键字
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 商品分类系统编号
        /// </summary>
        public int? CategorySysNo { get; set; }

        /// <summary>
        /// 筛选属性值系统编号
        /// </summary>
        public List<int> AttributeOptions { get; set; }

        /// <summary>
        /// 页记录数
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 当前页
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 页总数
        /// </summary>
        public int PageCount { get; set; }

        /// <summary>
        /// 记录数
        /// </summary>
        public int RecCount { get; set; }

        /// <summary>
        /// 是否高亮显示
        /// </summary>
        public bool HighLight { get; set; }

        /// <summary>
        /// 商品排序字段
        /// </summary>
        public CommonEnum.LuceneProductSortType Sort { get; set; }

        /// <summary>
        ///  是否是降序
        /// </summary>
        public bool IsDescending { get; set; }

        /// <summary>
        /// 商品系统编号
        /// </summary>
        public int ProductSysNo { get; set; }

        /// <summary>
        /// 会员价格来源类型
        /// </summary>
        public ProductStatus.产品价格来源 PriceSource { get; set; }

        /// <summary>
        /// 等级系统编号
        /// </summary>
        public int LevelSysNo { get; set; }
    }
}
