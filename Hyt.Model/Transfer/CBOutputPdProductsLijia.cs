using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Transfer
{
    /// <summary>
    /// 导出商品利嘉模板
    /// </summary>
    /// <remarks>2017-05-17 罗勤尧 创建</remarks>
    public class CBOutputPdProductsLijia
    {
        //public string 自动编码 { get; set; }
        public string 商品编码 { get; set; }
        //public string 前台显示名称 { get; set; }
        public string 名称 { get; set; }
        public string 商品分类 { get; set; }
        public string 品牌 { get; set; }
        public string 商品简称 { get; set; }
        public string 规格颜色 { get; set; }
        public decimal 长 { get; set; }
        public decimal 宽 { get; set; }
        public decimal 高 { get; set; }
        public decimal 直径 { get; set; }
        public decimal 重量 { get; set; }
        public string 供应商 {
            get { return "信营国际"; }
        }
        public string 供应商地址 { get; set; }
        public string 供应商联系方式 { get; set; }
        public string  采购地 { get; set; }
        public string  批采价格条目 { get; set; }
        public string 采购周期
        { get; set; }
        public int 是否快速单
        { get; set; }
        public string 包装内含
        { get; set; }
        public string 描述
        { get; set; }
        public string 英文名称

        { get; set; }
        public string 英文描述

        { get; set; }
        public string 备注

        { get; set; }
    }
}
