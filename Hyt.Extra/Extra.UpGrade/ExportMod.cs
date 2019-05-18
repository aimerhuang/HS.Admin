using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extra.UpGrade
{
    /// <summary>
    /// 导出excel标题数据
    /// </summary>
    public class ExportHead
    {
        public string HeadText { get; set; }
        public string SubHeadText { get; set; }
        public Dictionary<string, ExportHeadMod> ThDataList = new Dictionary<string, ExportHeadMod>();
    }

    public class ExportHeadMod
    {
        public string Text { get; set; }
        public string Type { get; set; }
        public override string ToString()
        {
            return Text;
        }

        public string GetTypeInfo()
        { 
            if(!string.IsNullOrEmpty(Type)&&Type=="数值")
            {
                return "";
            }
            else
            {
                return "STYLE='MSO-NUMBER-FORMAT:\\@'";
            }
        }
    }
    /// <summary>
    /// Excel数据模型
    /// </summary>
    public class ExportMod
    {

        public Dictionary<string, object> TdDataList = new Dictionary<string, object>();
    }
}
