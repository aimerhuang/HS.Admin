using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Transfer
{
    /// <summary>
    /// 导出商品 2合并
    /// </summary>
    /// <remarks>2017-1-10 杨云奕 创建</remarks>
    public class CBOutputPdProductStocks2
    {
       
        public string 商品编码 { get; set; }
        public string 后台显示名称 { get; set; }
        public string 分类内容1 { get; set; }
        public string 分类内容2 { get; set; }
        public string 分类内容3 { get; set; }
        public string 条形码 { get; set; }
        public string 海关备案号 { get; set; }
        public decimal 采购价格 { get; set; }
        public decimal 会员价格 { get; set; }
        public decimal 仓库1 { get; set; }
        public decimal 仓库2 { get; set; }
        public string 库存异议 { get; set; }
        public decimal 库存数量 { get; set; }
      
    }
    /// <summary>
    /// 导出商品
    /// </summary>
    /// <remarks>2015-12-30 王耀发 创建</remarks>
    public class CBOutputPdProductStocks
    {
        public string 仓库名称 { get; set; }
        public string 商品编码 { get; set; }
        public string 后台显示名称 { get; set; }
        public string 分类内容1 { get; set; }
        public string 分类内容2 { get; set; }
        public string 分类内容3 { get; set; }
        public string 条形码 { get; set; }
        public string 海关备案号 { get; set; }
        public decimal 采购价格 { get; set; }
        public decimal 库存数量 { get; set; }
        public decimal 会员价格 { get; set; }
        public string 异议库存 { get; set; }
    }

    /// <summary>
    /// 导出商品
    /// </summary>
    /// <remarks>2015-12-30 王耀发 创建</remarks>
    public class CBOutputPdProductAlarmStocks
    {
        public string 商品编码 { get; set; }
        public string 商品名称 { get; set; }
        public string 仓库名称 { get; set; }
        public int 库存数量 { get; set; }
        public int 上限 { get; set; }

        public int 下限 { get; set; }
    }
}
