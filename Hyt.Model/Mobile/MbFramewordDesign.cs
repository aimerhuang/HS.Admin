using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Mobile
{
    public class MBDesignFrame
    {

        public int SysNo { get; set; }
        public int PSysNo { get; set; }
        public int CustomerSysNo { get; set; }
        public string PageText { get; set; }
        public string TipCode { get; set; }
        public string DesignType { get; set; }
        public string DesignAttr { get; set; }
        public string DesignDataAttr { get; set; }
        public string DesignDataPath { get; set; }
        public string DesignDataPathParams { get; set; }
        public DateTime CreateTime { get; set; }
        public int CreateBy { get; set; }
        public DateTime UpdateTime { get; set; }
        public int UpdateBy { get; set; }
        public int SortBy { get; set; }
        public string DataDesignType { get; set; }
        public string BindDesignData { get; set; }

    }
}
