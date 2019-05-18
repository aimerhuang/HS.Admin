using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.WebTemplate
{
    /// <summary>
    /// 经销商选用的模板
    /// </summary>
    public class DsDealerMallTemplate
    {
        public int SysNo { get; set; }
        public int TemplateId { get; set; }
        public int DealerId { get; set; }
        public int MallId { get; set; }
        public int IsUsed { get; set; }
        public DateTime UseTime { get; set; }
    }
}
