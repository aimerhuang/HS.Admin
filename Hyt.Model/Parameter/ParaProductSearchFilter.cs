using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Parameter
{
    /// <summary>
    /// 入库单筛选字段
    /// </summary>
    /// <remarks>2013-07-22 邵斌 创建</remarks>
    [Serializable]
    public class ParaProductSearchFilter : PdProduct
    {
        /// <summary>
        /// 商品分类编号
        /// </summary>
        public int ProductCategorySysNo { get; set; }

        /// <summary>
        /// 商品分类名称
        /// </summary>
        public string ProductCategoryName { get; set; }

        /// <summary>
        /// 商品价格
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// 商品价格
        /// </summary>
        public decimal UserPrice { get; set; }
        /// <summary>
        /// 关联关系码
        /// </summary>
        public string RelationCode { get; set; }

        /// <summary>
        /// 需要过滤属性
        /// </summary>
        public bool RequiredFilterAttribute { get; set; }

        /// <summary>
        /// 是否同步显示前台展示商品（只显示前台的商品）
        /// </summary>
        public bool SyncWebFront { get; set; }

        /// <summary>
        /// 搜索等级参数
        /// </summary>
        public int LeavelSysNo { get; set; }

        /// <summary>
        /// 搜索中的仓库门店编号
        /// </summary>
        public int shopNo { get; set; }

        /// <summary>
        /// 经销商名称
        /// </summary>
        public string DealerName { get; set; }


        /// <summary>
        /// 基础价格
        /// </summary>
        public decimal BasicPrice { get; set; }


        /// <summary>
        /// 商品对应仓库编号
        /// </summary>
        public int WarehouseSysNo { get; set; }
        /// <summary>
        /// 分销商系统编号
        /// </summary>
        public int DealerSysNo { get; set; }

        /// <summary>
        /// 是否筛选有库存商品
        /// </summary>
        public bool SelectStockProduct { get; set; }
    }
}

