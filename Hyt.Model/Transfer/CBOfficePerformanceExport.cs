using System;

namespace Hyt.Model.Transfer
{
    /// <summary>
    /// 办事处绩效-导出excel
    /// </summary>
    /// <remarks>2014-01-13 周唐炬 添加注释</remarks>
    public class CBOfficePerformanceExport
    {
        public string 办事处 { get; set; }
        public decimal 配送_升舱_百城达 { get; set; }
        public decimal 配送_升舱_第三方 { get; set; }
        public decimal 配送_信营全球购B2B2C_百城达 { get; set; }
        public decimal 配送_信营全球购B2B2C_第三方 { get; set; }
        public decimal 配送_代发_百城达 { get; set; }
        public decimal 配送_代发_第三方 { get; set; }
    }
}