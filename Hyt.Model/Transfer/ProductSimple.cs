using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Transfer
{
    /// <summary>
    /// 商品搜索简单类
    /// </summary>
    /// <remarks>2013-06-19 黄志勇 创建</remarks>
    public class ProductSimple
    {
        public int ProductID  {get;set;}
        public string ProductName  {get;set;}
        public decimal PriceBase  {get;set;}
        public decimal PriceReal { get; set; }
    }

    /// <summary>
    /// 商品搜索简单类分页用
    /// </summary>
    /// <remarks>2013-06-19 黄志勇 创建</remarks>
    public class ProductSimplePager
    {
        public int count { get; set; }
        public List<ProductSimple> list { get; set; }
    }
}
